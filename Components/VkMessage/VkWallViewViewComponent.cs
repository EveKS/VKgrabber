using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VkGroupManager.JsonModel.Instagram;
using VkGroupManager.JsonModel.InstagramNext;
using VkGroupManager.JsonModel.VK;
using VkGroupManager.Models;
using VkGroupManager.Models.VK;
using VkGroupManager.Service.Instagram;
using VkGroupManager.Service.JSON;
using VkGroupManager.Service.Telegram;
using VkGroupManager.Service.Text;
using VkGroupManager.Service.VK;
using VkGroupManager.ViewModels.VK;

namespace VkGroupManager.Components.VkMessage
{
    public class VkWallViewViewComponent : ViewComponent
    {
        const int TAKE = 50;

        private ApplicationContext _context;
        private readonly SignInManager<Models.User> _signInManager;
        private readonly UserManager<Models.User> _userManager;
        private IInstagramService _instagramService;
        private ITelegramService _telegramService;
        private ITextService _textService;
        private IJsonService _jsonService;
        private IVkService _vkService;

        public VkWallViewViewComponent(ApplicationContext context,
            SignInManager<Models.User> signInManager, UserManager<Models.User> userManager,
            ITelegramService telegramService,
            IJsonService jsonService,
            IVkService vkService,
            ITextService textService,
            IInstagramService instagramService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;

            _instagramService = instagramService;
            _telegramService = telegramService;
            _textService = textService;
            _jsonService = jsonService;
            _vkService = vkService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string groupId, string isLoad, string isInst, string url)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(groupId))
                {
                    #region Get info
                    var user = await _userManager.GetUserAsync(HttpContext.User);
                    var vkUser = await _context.VkUserSet
                        .FirstOrDefaultAsync(u => u.VkUserId == user.VkUserId);
                    // Информация о текущем состоянии премиум
                    var order = await _context.Orders
                        .FirstOrDefaultAsync(id => user.OrderId == id.OrderId);
                    var isPremium = order?.DateEnd > DateTime.Now;

                    var photo = await _context.VkGroupSet
                        .Where(g => g.VkGroupId == groupId)
                        .Select(inf => inf.GroupInfo.Photo100 ?? inf.GroupInfo.Photo200 ?? inf.GroupInfo.Photo50)
                        .FirstOrDefaultAsync();

                    var vkGroup = await _context.VkGroupSet
                        .Include(inf => inf.GroupInfo)
                        .FirstOrDefaultAsync(g => g.VkGroupId == groupId);

                    #endregion
                    if (isInst == "true" || isInst == "next")
                    {
                        var view = await InstagramLoadAsync(vkUser, vkGroup, user,
                            order, photo, isPremium, groupId, isInst, url);
                        return view;
                    }
                    else
                    {
                        var view = await VkLoadAsync(vkUser, vkGroup, user,
                            order, photo, isPremium, groupId, isLoad);
                        return view;
                    }
                }
            }
            catch (Exception ex)
            {
                await _telegramService.SendMessageExceptionAsync(ex);
            }

            return View();
        }

        #region Vk
        private async Task<IViewComponentResult> VkLoadAsync(VkUser vkUser, VkGroup vkGroup,
            Models.User user, Models.Order order,
            string photo, bool isPremium, string groupId, string isLoad)
        {
            try
            {
                var toLoad = true;
                if (!isPremium)
                {
                    var firstId = (await _context.VkGroupSet
                            .FirstOrDefaultAsync(id => id.VkUserId == vkUser.VkUserId)).VkGroupId;

                    toLoad = vkGroup.VkGroupId == firstId;
                }

                // Не грузить левые группы, для непремиум
                if (toLoad)
                {
                    var vkGroupFrom = await _context.VkGroupFromSet
                        .Include(inf => inf.GroupInfo)
                        .Include(set => set.Filter)
                        .Where(g => g.VkGroupId == vkGroup.VkGroupId)
                        .Take(isPremium ? 20 : 10)

                        .AsNoTracking()
                        .ToListAsync();

                    if (isLoad == "true" && vkGroupFrom != null && vkGroupFrom.Count > 0)
                    {
                        Stopwatch sw = Stopwatch.StartNew();
                        var oldItem = await _context.VkGroupSet
                            .Where(g => g.VkGroupId == groupId)
                            .SelectMany(g => g.VkGroupsFrom
                            .SelectMany(gr => gr.WallGets
                            .SelectMany(wall => wall.Items.Select(item => new VkItemViewModel
                            {
                                Text = item.Text,
                                Date = item.Date
                            }))))

                            .AsNoTracking()
                            .ToListAsync();

                        var count = vkGroup.MaxFrom ?? (TAKE / vkGroupFrom.Count) * 2;
                        var responses = new List<WallGet>(vkGroupFrom.Count);

                        if (vkUser != null)
                        {
                            foreach (var group in vkGroupFrom)
                            {

                                var onlyGroup = vkGroup.Filter?.GetOnlyGroupPost == true ||
                                    group.Filter?.GetOnlyGroupPost == true;

                                var result = await _vkService.WallGetAsync(vkUser.AccessToken, 
                                    $"{count}", "0", $"-{group.GroupId}", group.Domain, onlyGroup);

                                if (!result.Contains("error_code"))
                                {
                                    WallGet wallGetResponse = _jsonService.JsonConvertDeserializeObject<WallGetResponse>(result)?
                                                    .Response;

                                    if (wallGetResponse != null)
                                    {
                                        wallGetResponse = await _textService.SortedTextAsync(wallGetResponse, oldItem);

                                        for (int i = 0; i < wallGetResponse.Items.Count; i++)
                                        {
                                            var item = wallGetResponse.Items[i];

                                            var tag = $"{group.Atribute} {vkGroup.Atribute}".Trim(' ');
                                            item.Text = await _textService.TextFilterAsync(item.Text, vkGroup.Filter, group.Filter);
                                            item.Text = _textService.AddTag(item.Text, tag);
                                        }

                                        wallGetResponse.VkGroupFromId = group.VkGroupFromId;
                                        responses.Add(wallGetResponse);
                                    }
                                }
                                else
                                {
                                    await _telegramService.SendMessage(result);
                                }
                            }
                        }

                        await _context.AddRangeAsync(responses);
                        await _context.SaveChangesAsync();

                        sw.Stop();
                        await _telegramService.SendMessage($"time1: {sw.Elapsed.TotalMilliseconds}");
                        sw = Stopwatch.StartNew();

                        DateTime dateNow = DateTime.Now;
                        long unixTime = ((DateTimeOffset)dateNow).ToUnixTimeSeconds() - 30 * 60;

                        var date = DateTime.Now;
                        var take = vkGroup.MaxLoad ?? TAKE;
                        var vkItem = (await _context.VkGroupSet
                            .Where(g => g.VkGroupId == groupId)
                            .SelectMany(g => g.VkGroupsFrom
                            .SelectMany(gr => gr.WallGets
                            .SelectMany(wall => wall.Items
                                .Where(st =>
                                string.IsNullOrWhiteSpace(st.Statuse) ||
                                (st.Statuse == "Selected" && st.AddedTime < unixTime)
                                ))))
                                .OrderByDescending(ch => ch.Statuse == "Selected")
                                .Take(take)
                                .Select(item => new
                                {
                                    Filter = item.WallGet.VkGroupFrom.Filter,
                                    Atribute = item.WallGet.VkGroupFrom.Atribute,
                                    Statuse = item.Statuse,
                                    GroupInfo = item.WallGet.VkGroupFrom.GroupInfo,
                                    ItemId = item.ItemId,
                                    Likes = item.Likes.Count,
                                    Reposts = item.Reposts.Count,
                                    Views = item.Views.Count,
                                    Date = item.Date,
                                    GifPrew = item.Attachments.Select(doc =>
                                        new 
                                        {
                                            Gif = doc.Doc.Url,
                                            DocExt = doc.Doc.Ext,
                                            Preview = doc.Doc.Preview.Photo.Sizes
                                        }),
                                    Photo = item.Attachments.Select(ph => ph.Photo.Photo604),
                                    Text = item.Text
                                })
                                .AsNoTracking()
                            .ToListAsync())

                            .Select(item => new VkItemViewModel
                            {
                                Filter = item.Filter,
                                Atribute = item.Atribute,
                                Statuse = item.Statuse,
                                GroupInfo = item.GroupInfo,
                                ItemId = item.ItemId,
                                Likes = item.Likes,
                                Reposts = item.Reposts,
                                Views = item.Views,
                                Date = item.Date,
                                GifPrew = item.GifPrew.Select(doc =>
                                    new GifPreviewViewModel
                                    {
                                        Gif = doc.Gif,
                                        DocExt = doc.DocExt,
                                        PreviewPhoto = doc.Preview.ToList()
                                    }).ToList(),
                                Photo = item.Photo.ToList(),
                                Text = item.Text
                            });

                        vkItem = await _textService.SortingItemAsync(vkItem, vkGroup.Filter);
                        sw.Stop();
                        await _telegramService.SendMessage($"time2: {sw.Elapsed.TotalMilliseconds}");

                        return View(new VkResponseAndGroupVk
                        {
                            Responses = vkItem.OrderByDescending(ch => Tuple.Create(ch.Statuse == "Selected", ch.Likes + ch.Reposts, ch.Likes, ch.Reposts, ch.Views)),
                            VkGroupViewModel = new VkGroupViewModel
                            {
                                VkGroupId = groupId,
                                VkGroupsFrom = vkGroupFrom,
                                Photo = photo,
                                Domain = vkGroup?.GroupInfo?.ScreenName ?? vkGroup?.Domain,
                                GroupName = vkGroup?.GroupInfo.Name
                            },
                            AttributeAll = vkGroup.Atribute,
                        });
                    }

                    return View(new VkResponseAndGroupVk
                    {
                        VkGroupViewModel = new VkGroupViewModel
                        {
                            VkGroupId = groupId,
                            VkGroupsFrom = vkGroupFrom,
                            Photo = photo,
                            Domain = vkGroup?.GroupInfo?.ScreenName ?? vkGroup?.Domain,
                            GroupName = vkGroup?.GroupInfo.Name
                        },
                        AttributeAll = vkGroup.Atribute
                    });
                }
            }
            catch (Exception ex)
            {
                await _telegramService.SendMessageExceptionAsync(ex);
            }

            return View();
        }
        #endregion

        #region instagram
        private async Task<IViewComponentResult> InstagramLoadAsync(VkUser vkUser, VkGroup vkGroup,
            Models.User user, Models.Order order,
            string photo, bool isPremium, string groupId, string isInst, string url)
        {
            try
            {
                var json = await _instagramService.GetInstagramJsonAsync(url);

                var instagram = _jsonService.JsonConvertDeserializeObject<JsonModel.Instagram.Instagram>(json);
                var profiles = instagram?.EntryData?.ProfilePage;
                string jsonNext = null;

                if (profiles != null)
                {
                    var token = profiles.FirstOrDefault()?.User?.Media?.PageInfo?.EndCursor;
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        var owner = profiles.FirstOrDefault()?.User?.Media?.Nodes?.FirstOrDefault()?.Owner?.Id;
                        if (!string.IsNullOrWhiteSpace(owner))
                        {
                            user.InstagramToken = token;
                            user.InstagramOwnerId = owner;

                            _context.Update(user);
                            await _context.SaveChangesAsync();

                            var query_id = (await _context.VkClientSet.FirstOrDefaultAsync())
                                .InstagramQueryId;

                            if (!string.IsNullOrWhiteSpace(query_id))
                            {
                                var count = 12;
                                jsonNext = await _instagramService.GetNextJsonAsync(query_id, owner, count, token);
                            }
                        }
                    }

                    var srcs = profiles.SelectMany(prof =>
                        prof?.User?.Media?.Nodes?.Select(node => new VkItemViewModel
                        {
                            Likes = node?.EdgeMediaPreviewLike?.Count ?? node?.Likes.Count,
                            Coments = node?.EdgeMediaToComment?.Count ?? node.Comments?.Count,
                            Text = string.Join("\n", node?.EdgeMediaToCaption?.Edges?.Select(o => o?.Node?.Text) ?? new List<string> { node?.Caption }),
                            Photo = new List<string> { node?.DisplaySrc },
                            Date = node?.Date ?? node?.TakenAtTimestamp
                        }))
                        .ToList();

                    if (!string.IsNullOrWhiteSpace(jsonNext))
                    {
                        var instagramNext = _jsonService.JsonConvertDeserializeObject<InstagramNext>(jsonNext);
                        if (instagramNext.Status == "ok")
                        {
                            var media = instagramNext?.Data?.User?.EdgeOwnerToTimelineMedia;

                            if (media != null)
                            {
                                var srcsNext = media.Edges?.Select(prof =>
                                    new VkItemViewModel
                                    {
                                        Likes = prof?.Node?.EdgeMediaPreviewLike?.Count,
                                        Coments = prof?.Node?.EdgeMediaToComment?.Count,
                                        Text = string.Join("\n", prof?.Node?.EdgeMediaToCaption?.Edges?.Select(o => o?.Node?.Text) ?? new List<string> { string.Empty }),
                                        Photo = new List<string> { prof?.Node?.DisplayUrl },
                                        Date = prof?.Node?.TakenAtTimestamp
                                    })
                                    .ToList();

                                srcs = srcs.Union(srcsNext).ToList();
                                if (media.PageInfo?.HasNextPage == true)
                                {
                                    user.InstagramToken = media.PageInfo?.EndCursor;

                                    _context.Update(user);
                                    await _context.SaveChangesAsync();
                                }
                            }
                        }
                        else
                        {
                            await _telegramService.SendMessage($"instagram query error: {instagramNext.Message}");
                        }
                    }

                    #region Загружаем выделенное, удаляем не использованное
                    var srcsDd = await _context.InstagramSet
                        .Where(sel => sel.VkGroupId == vkGroup.VkGroupId && sel.Statuse == "Selected")
                        .Select(node => new VkItemViewModel
                        {
                            ItemId = node.InstagramId,
                            Statuse = node.Statuse,
                            Text = node.Text,
                            Photo = new List<string> { node.Url },
                            Date = node.Date,
                            Coments = node.Coment,
                            Likes = node.Likes
                        })
                        .AsNoTracking()
                        .ToListAsync();

                    var remove = _context.InstagramSet
                        .Where(sel => sel.VkGroupId == vkGroup.VkGroupId && sel.Statuse != "Selected");

                    _context.RemoveRange(remove);
                    await _context.SaveChangesAsync();
                    #endregion

                    return View(new VkResponseAndGroupVk
                    {
                        Responses = srcs.Union(srcsDd).OrderByDescending(ch => Tuple.Create(ch.Statuse == "Selected", ch.Likes)),
                        VkGroupViewModel = new VkGroupViewModel
                        {
                            VkGroupId = groupId,
                            Photo = photo,
                            Domain = vkGroup?.GroupInfo?.ScreenName ?? vkGroup?.Domain,
                            GroupName = vkGroup?.GroupInfo.Name
                        },
                        IsInst = "true"
                    });
                }
            }
            catch (Exception ex)
            {
                await _telegramService.SendMessageExceptionAsync(ex);
            }

            return View();
        }
        #endregion
    }
}
