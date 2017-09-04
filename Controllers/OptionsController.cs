using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VkGroupManager.JsonModel.VK;
using VkGroupManager.Models;
using VkGroupManager.Models.VK;
using VkGroupManager.Service.JSON;
using VkGroupManager.Service.Telegram;
using VkGroupManager.Service.Text;
using VkGroupManager.Service.VK;
using VkGroupManager.ViewModels.VK;

namespace VkGroupManager.Controllers
{
    [Authorize]
    public class OptionsController : Controller
    {
        #region head
        const int MAX_GROUP = 7;
        const int MAX_GROUP_FROM = 10;

        private static Random rnd = new Random();
        private static Object thisLock = new Object();
        private ApplicationContext _context;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        private ITelegramService _telegramService;
        private IJsonService _jsonService;
        private ITextService _textService;
        private IVkService _vkService;

        public OptionsController(ApplicationContext context,
            SignInManager<User> signInManager, UserManager<User> userManager,
            ITelegramService telegramService,
            IJsonService jsonService,
            IVkService vkService,
            ITextService textService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;

            _telegramService = telegramService;
            _textService = textService;
            _jsonService = jsonService;
            _vkService = vkService;
        }
        #endregion

        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 300)]
        public async Task<IActionResult> Index()
        {
            if (_context != null && _signInManager.IsSignedIn(HttpContext.User))
            {
                var vkClient = await _context
                    .VkClientSet?
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                if (vkClient == null)
                {
                    vkClient = new VkClient();
                }

                return View(vkClient);
            }

            return View();
        }

        #region vkuser vs group
        /// <summary>
        /// Настройки клиента vk
        /// </summary>
        /// <param name="vkClient"></param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> VkClientSet(VkClient vkClient)
        {
            if (ModelState.IsValid && _context != null)
            {
                var client = await _context.VkClientSet?
                    .FirstOrDefaultAsync(id => id.VkClientId == vkClient.VkClientId)
                    ?? new VkClient();

                client.ClientId = vkClient.ClientId;
                client.ClientSecret = vkClient.ClientSecret;
                client.Display = vkClient.Display;
                client.RedirectUri = vkClient.RedirectUri;
                client.ResponseType = vkClient.ResponseType;
                client.Revoke = vkClient.Revoke;
                client.Scope = vkClient.Scope;
                client.ServerUrl = vkClient.ServerUrl;
                client.State = vkClient.State;
                client.Version = vkClient.Version;
                client.InstagramQueryId = vkClient.InstagramQueryId;

                if (string.IsNullOrWhiteSpace(vkClient.VkClientId))
                {
                    await _context.AddAsync(client);
                }
                else
                {
                    _context.Update(client);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View("Index", vkClient);
        }

        /// <summary>
        /// Добавляем пользователя vk и его токен
        /// </summary>
        /// <param name="AccessToken"></param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> VkSetUserToken(string AccessToken)
        {
            if (ModelState.IsValid && !string.IsNullOrWhiteSpace(AccessToken) && _context != null)
            {
                var isNew = false;
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var vkUser = await _context.VkUserSet.
                    FirstOrDefaultAsync(vk => vk.VkUserId == user.VkUserId);
                if (vkUser == null)
                {
                    isNew = true;
                    vkUser = new VkUser();
                }

                string access_token =
                    @"(access_token)=(?<access_token>[^&]+)";
                string expires_in =
                    @"(expires_in)=(?<expires_in>[^&]+)";
                string user_id =
                    @"(user_id)=(?<user_id>[^&]+)";

                vkUser.AccessToken = Regex.Matches(AccessToken, access_token, RegexOptions.CultureInvariant)
                    .OfType<Match>()
                    .Select(m => m.Groups["access_token"].Value)
                    .FirstOrDefault();

                vkUser.ExpiresIn = Regex.Matches(AccessToken, expires_in, RegexOptions.CultureInvariant)
                    .OfType<Match>()
                    .Select(m => m.Groups["expires_in"].Value)
                    .FirstOrDefault();

                vkUser.UserVkId = Regex.Matches(AccessToken, user_id, RegexOptions.CultureInvariant)
                    .OfType<Match>()
                    .Select(m => m.Groups["user_id"].Value)
                    .FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(vkUser.AccessToken) &&
                    !string.IsNullOrWhiteSpace(vkUser.ExpiresIn) &&
                    !string.IsNullOrWhiteSpace(vkUser.UserVkId))
                {
                    if (!_context.VkUserSet.Any(id => id.UserVkId == vkUser.UserVkId))
                    {
                        if (isNew)
                        {
                            await _context.AddAsync(vkUser);
                            user.VkUserId = vkUser.VkUserId;
                            await _context.SaveChangesAsync();
                            _context.Update(vkUser);
                        }
                        else
                        {
                            _context.Update(vkUser);
                        }

                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Пользователь, с таким Id Vk уже есть в базе");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Пожалуйста, вставте строку, с адресом содержащим токен - целиком");
                }
            }

            return RedirectToAction("Index", AccessToken);
        }

        /// <summary>
        /// Удаляем пользователя vk и его токен
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteToken()
        {
            if (ModelState.IsValid && _context != null)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);

                var vkUser = await _context.VkUserSet.
                    FirstOrDefaultAsync(vk => vk.VkUserId == user.VkUserId);

                if (vkUser != null)
                {
                    vkUser.AccessToken = null;
                    vkUser.ExpiresIn = null;
                    vkUser.UserVkId = null;
                    _context.Update(vkUser);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Добавляем пользователю его группы
        /// </summary>
        /// <param name="Domain"></param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> SetGroup(string Domain)
        {
            if (ModelState.IsValid)
            {
                var groupId = string.Empty;

                if (!string.IsNullOrWhiteSpace(Domain))
                {
                    var user = await _userManager.GetUserAsync(HttpContext.User);
                    // Информация о текущем состоянии премиум
                    var order = await _context.Orders
                        .FirstOrDefaultAsync(id => user.OrderId == id.OrderId);

                    var vkUser = await _context.VkUserSet.
                        Include(gr => gr.VkGroups).
                        FirstOrDefaultAsync(vk => vk.VkUserId == user.VkUserId);
                    // сколько групп уже создано
                    var count = vkUser.VkGroups.Count;

                    // Если премиум или групп еще не создано // +Ограничение
                    if ((order?.DateEnd > DateTime.Now || count == 0) && count < MAX_GROUP)
                    {
                        string group_id_pattern =
                            @"((https:\/\/vk.com\/)|(http:\/\/vk.com\/))?(club|public)?(?<group_id>[^\s^,^\n]+)";

                        var group_id = Regex.Matches(Domain, group_id_pattern, RegexOptions.CultureInvariant)
                            .OfType<Match>()
                            .Select(m => m.Groups["group_id"].Value)
                            .FirstOrDefault();

                        // Нет-ли такой группы в списке текущего пользователя
                        // todo: в дальнейшем добавить проверку у всех пользователей, 1на группа 1н админ
                        if (!_context.VkGroupSet.Any(g => g.GroupId == group_id))
                        {
                            var groupInfo = await _vkService.GetByIdAsync(vkUser.AccessToken, group_id);

                            // есть-ли такой Id группы
                            if (!string.IsNullOrWhiteSpace(groupInfo)
                                || groupInfo.Contains("error"))
                            {
                                var info = _jsonService.JsonConvertDeserializeObject<GetGroupInfo>(groupInfo)?
                                    .GroupsInfo.FirstOrDefault();

                                // Админ по токену
                                if (info != null && info.IsAdmin != 0)
                                {
                                    // админ по id
                                    if (await _vkService.IsAdmin(vkUser.AccessToken, vkUser.UserVkId, info.Id.ToString()))
                                    {
                                        var vkGroup = new VkGroup()
                                        {
                                            GroupInfo = info
                                        };

                                        if (int.TryParse(group_id, out var domain))
                                        {
                                            vkGroup.Domain = null;
                                            vkGroup.GroupId = group_id;
                                        }
                                        else
                                        {
                                            vkGroup.GroupId = info.Id.ToString();
                                            vkGroup.Domain = group_id;
                                        }

                                        vkGroup.VkUserId = vkUser.VkUserId;
                                        await _context.AddAsync(vkGroup);
                                        _context.Update(vkUser);
                                        _context.Update(user);
                                        await _context.SaveChangesAsync();

                                        return RedirectToAction("Index");
                                    }
                                    else
                                    {
                                        // возможно подмена адресной строки с токеном
                                        ModelState.AddModelError(string.Empty, "Вы не являетесь администратором данной группы");
                                    }
                                }
                                else
                                {
                                    ModelState.AddModelError(string.Empty, "Была допущена ошибка или Вы не являетесь администратором данной группы");
                                }
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Данная группа, уже есть, в списке ваших групп");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Была допущена ошибка или данной группы не существует");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Вы не можете иметь более одной группы");
                    }
                }
            }

            return View("Index");
        }

        /// <summary>
        /// Удалить группу пользователя
        /// </summary>
        /// <param name="group_id"></param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<JsonResult> DeleateGroup(string group_id)
        {
            try
            {
                if (ModelState.IsValid && !string.IsNullOrWhiteSpace(group_id) && _context != null)
                {
                    var vkGroup = await _context
                        .VkGroupSet
                        .Include(i => i.Instagrams)
                        .Include(f => f.Filter)
                        .Include(i => i.GroupInfo)
                        .FirstOrDefaultAsync(id => id.VkGroupId == group_id);

                    if (vkGroup != null)
                    {
                        var vkGroupFrom = await _context
                            .VkGroupFromSet
                            .Include(w => w.WallGets)
                            .Include(f => f.Filter)
                            .Include(inf => inf.GroupInfo)
                            .Where(id => id.VkGroupId == vkGroup.VkGroupId)
                            .ToListAsync();

                        _context.RemoveRange(vkGroupFrom);
                        _context.Remove(vkGroup);

                        await _context.SaveChangesAsync();
                        return Json(new { ok = "ok" });
                    }
                }
            }catch(Exception ex) { await _telegramService.SendMessageExceptionAsync(ex); }
            return Json(new { ok = "error" });
        }

        /// <summary>
        /// Настройки группы пользователя
        /// </summary>
        /// <param name="group_id"></param>
        /// <param name="select_from"></param>
        /// <param name="select_to"></param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<JsonResult> GroupSetup(string group_id, string select_from, string select_to,
            string max_from, string max_load)
        {
            if (ModelState.IsValid &&
                !string.IsNullOrWhiteSpace(group_id) && _context != null &&
                int.TryParse(select_from, out var tfrom) &&
                int.TryParse(select_to, out var selectTo) &&
                int.TryParse(max_from, out var maxfrom) &&
                int.TryParse(max_load, out var maxload) &&
                tfrom > 0 && selectTo > 0 && maxfrom > 0 && maxload > 0)
            {
                if (tfrom > selectTo)
                {
                    var tmp = tfrom;
                    tfrom = selectTo;
                    selectTo = tfrom;
                }

                var vkGroup = await _context.VkGroupSet
                    .FirstOrDefaultAsync(id => id.VkGroupId == group_id);

                if (vkGroup != null)
                {
                    vkGroup.TimeFrom = tfrom;
                    vkGroup.TimeTo = selectTo;

                    vkGroup.MaxFrom = maxfrom;
                    vkGroup.MaxLoad = maxload;

                    _context.Update(vkGroup);
                    await _context.SaveChangesAsync();
                    return Json(new { ok = "ok" });
                }
            }

            return Json(new { ok = "error" });
        }
        #endregion

        #region Instagram and Vk posts
        /// <summary>
        /// Отображаем страницу с постами
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="isLoad"></param>
        /// <returns></returns>
        [ResponseCache(VaryByHeader = "User-Agent", Duration = 60 * 60 * 24 * 30)]
        public async Task<IActionResult> GetWall(string groupId, string isLoad, string IsInst, string url)
        {
            if (_context != null && !string.IsNullOrWhiteSpace(groupId))
            {
                var vkGroup = await _context.VkGroupSet?
                    .Include(f => f.Filter)
                    .FirstOrDefaultAsync(id => id.VkGroupId == groupId);

                if (vkGroup != null)
                {
                    var vkGroupsFrom = await _context.VkGroupFromSet?
                        .Include(f => f.Filter)
                        .Include(inf => inf.GroupInfo)
                        .Where(id => id.VkGroupId == vkGroup.VkGroupId)
                        .ToListAsync();

                    var vkGroupViewModel = new VkGroupViewModel
                    {
                        VkGroupId = vkGroup.VkGroupId,
                        GroupId = vkGroup.GroupId,
                        VkGroupsFrom = vkGroupsFrom,
                        Domain = vkGroup.Domain
                    };

                    return View(new VkResponseAndGroupVk
                    {
                        VkGroupViewModel = vkGroupViewModel,
                        AttributeAll = vkGroup.Atribute,
                        Filter = vkGroup.Filter,
                        IsLoad = isLoad,
                        IsInst = IsInst,
                        Url = url
                    });
                }
            }

            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult GetWall(string groupId, string IsInst, string url)
        {
            if (ModelState.IsValid)
            {
                var send = new { groupId = groupId, IsInst = IsInst, url = url };
                return RedirectToAction("GetWall", send);
            }

            return View();
        }
        #endregion

        #region подгруппы и фильтры
        /// <summary>
        /// Добавляем группы, откуда берем посты
        /// </summary>
        /// <param name="vkGroupViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> SetGroupFrom(VkGroupViewModel vkGroupViewModel)
        {
            try
            {
                if (ModelState.IsValid && !string.IsNullOrWhiteSpace(vkGroupViewModel.AddedGroup) && !string.IsNullOrWhiteSpace(vkGroupViewModel.VkGroupId))
                {
                    string group_id_pattern =
                        @"((https:\/\/vk.com\/)|(http:\/\/vk.com\/))?(club|public)?(?<group_id>[^\s^,^\n]+)";

                    var user = await _userManager.GetUserAsync(HttpContext.User);

                    var vkUser = await _context.VkUserSet.
                        FirstOrDefaultAsync(vk => vk.VkUserId == user.VkUserId);
                    // Информация о текущем состоянии премиум
                    var order = await _context.Orders
                        .FirstOrDefaultAsync(id => user.OrderId == id.OrderId);

                    var group_ids = Regex.Matches(vkGroupViewModel.AddedGroup, group_id_pattern, RegexOptions.CultureInvariant)
                        .OfType<Match>()
                        .Select(m => m.Groups["group_id"].Value);

                    var vkGroup = await _context.VkGroupSet?
                        .Include(f => f.VkGroupsFrom)
                        .FirstOrDefaultAsync(id => id.VkGroupId == vkGroupViewModel.VkGroupId);
                    // сколько групп уже создано
                    var count = vkGroup.VkGroupsFrom.Count;

                    // Если премиум или групп еще не создано // +Ограничение
                    if ((order?.DateEnd > DateTime.Now || count < MAX_GROUP_FROM) && count < MAX_GROUP_FROM * 2)
                    {
                        if (vkGroup != null)
                        {
                            var vkGroupFrom = new List<VkGroupFrom>(group_ids.Count());

                            foreach (var group_id in group_ids)
                            {
                                var groupInfo = await _vkService.GetByIdAsync(vkUser.AccessToken, group_id);
                                // есть-ли такой Id группы
                                if (!string.IsNullOrWhiteSpace(groupInfo)
                                    && !groupInfo.Contains("error"))
                                {
                                    var info = _jsonService.JsonConvertDeserializeObject<GetGroupInfo>(groupInfo)?
                                        .GroupsInfo.FirstOrDefault();
                                    if (info != null)
                                    {
                                        // проверяем, нет-ли такой группы уже
                                        if (!vkGroup.VkGroupsFrom.Any(g => g.Domain == group_id || g.GroupId == group_id))
                                        {
                                            if (int.TryParse(group_id, out var domain))
                                            {
                                                vkGroupFrom.Add(new VkGroupFrom
                                                {
                                                    GroupId = group_id,
                                                    VkGroupId = vkGroup.VkGroupId,
                                                    GroupInfo = info
                                                });
                                            }
                                            else
                                            {
                                                vkGroupFrom.Add(new VkGroupFrom
                                                {
                                                    Domain = group_id,
                                                    VkGroupId = vkGroup.VkGroupId,
                                                    GroupId = info.Id.ToString(),
                                                    GroupInfo = info
                                                });
                                            }
                                        }
                                    }
                                }
                            }

                            await _context.AddRangeAsync(vkGroupFrom);
                            await _context.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, $"Максимальное количество групп источников равняется {(order?.DateEnd > DateTime.Now ? (MAX_GROUP_FROM * 2) : MAX_GROUP_FROM) }");
                    }
                }
            }
            catch (Exception ex)
            {
                await _telegramService.SendMessageExceptionAsync(ex);
            }

            return RedirectToAction("GetWall", new { groupId = vkGroupViewModel.VkGroupId });
        }

        // POST: Options/DeleateGroupFrom/
        /// <summary>
        /// Удаление группы, из которой берутся посты
        /// </summary>
        /// <param name="group_id"></param>
        /// <param name="all"></param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<JsonResult> DeleateGroupFrom(string group_id, string all)
        {
            if (ModelState.IsValid && !string.IsNullOrWhiteSpace(group_id) && _context != null)
            {
                if (all == "all")
                {
                    var vkGroupFrom = await _context.VkGroupFromSet
                        .Include(w => w.WallGets)
                        .Include(f => f.Filter)
                        .Include(inf => inf.GroupInfo)
                        .Where(id => id.VkGroupId == group_id)
                        .ToListAsync();

                    _context.RemoveRange(vkGroupFrom);
                }
                else
                {
                    var vkGroupFrom = await _context.VkGroupFromSet
                        .Include(w => w.WallGets)
                        .Include(f => f.Filter)
                        .Include(inf => inf.GroupInfo)
                        .FirstOrDefaultAsync(id => id.VkGroupFromId == group_id);

                    _context.Remove(vkGroupFrom);
                }

                await _context.SaveChangesAsync();
                return Json(new { ok = "ok" });
            }

            return Json(new { ok = "error" });
        }

        /// <summary>
        /// Замена текста в посте
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<JsonResult> ChangeText(string Text, string Id, string inst, string group_id)
        {
            if (ModelState.IsValid && _context != null)
            {
                if (!string.IsNullOrWhiteSpace(Id))
                {
                    var item = await _context.ItemSet
                        .FirstOrDefaultAsync(id => id.ItemId == Id);

                    if (item != null)
                    {
                        item.BackUpText = item.Text;
                        item.Text = Text.Trim();
                        _context.Update(item);
                        await _context.SaveChangesAsync();

                        return Json(new { ok = "ok" });
                    }
                }
                else if (!string.IsNullOrWhiteSpace(inst))
                {
                    var instagram = await _context.InstagramSet
                        .FirstOrDefaultAsync(id => id.InstagramId == inst);
                    if (instagram != null)
                    {
                        instagram.BackUpText = instagram.Text;
                        instagram.Text = await _textService.RemoveLinkAsync(instagram.Text);
                        _context.Update(instagram);
                    }
                    else
                    {
                        var vkGroup = await _context.VkGroupSet
                            .FirstOrDefaultAsync(id => id.VkGroupId == group_id);
                        if (vkGroup != null)
                        {
                            instagram = new Instagram
                            {
                                VkGroupId = vkGroup.VkGroupId,
                                Text = Text
                            };
                        }

                        await _context.AddAsync(instagram);
                    }

                    await _context.SaveChangesAsync();

                    return Json(new
                    {
                        ok = "ok",
                        inst_id = instagram.InstagramId
                    });
                }
            }

            return Json(new { ok = "error" });
        }

        // todo: поменять название метода
        /// <summary>
        /// Устанавливаяем настройки фильтра
        /// </summary>
        /// <param name="tags"></param>
        /// <param name="group_id"></param>
        /// <param name="all"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<JsonResult> AddTag(string tags, string group_id, string all, Filter filter)
        {
            if (ModelState.IsValid && !string.IsNullOrWhiteSpace(group_id) && _context != null)
            {
                if (!string.IsNullOrWhiteSpace(all) && all == "all")
                {
                    var vkGroup = await _context.VkGroupSet
                        .Include(f => f.Filter)
                        .FirstOrDefaultAsync(id => id.VkGroupId == group_id);

                    if (vkGroup != null)
                    {
                        if (vkGroup.Filter == null)
                        {
                            vkGroup.Filter = filter;
                        }
                        else
                        {
                            vkGroup.Filter.CopyWithAuthor = filter.CopyWithAuthor;
                            vkGroup.Filter.GetOnlyGroupPost = filter.GetOnlyGroupPost;
                            vkGroup.Filter.GetWithLink = filter.GetWithLink;
                            vkGroup.Filter.GetWithPicture = filter.GetWithPicture;
                            vkGroup.Filter.GetWithVkLink = filter.GetWithVkLink;
                            vkGroup.Filter.GetWithWikiPage = filter.GetWithWikiPage;
                            vkGroup.Filter.RemoveAuthor = filter.RemoveAuthor;
                            vkGroup.Filter.RemoveTag = filter.RemoveTag;
                            vkGroup.Filter.RemoveText = filter.RemoveText;
                            vkGroup.Filter.RepalaceFrom1 = filter.RepalaceFrom1;
                            vkGroup.Filter.RepalaceFrom2 = filter.RepalaceFrom2;
                            vkGroup.Filter.RepalaceTo1 = filter.RepalaceTo1;
                            vkGroup.Filter.RepalaceTo2 = filter.RepalaceTo2;
                        }

                        vkGroup.Atribute = tags;
                        _context.Update(vkGroup);
                        await _context.SaveChangesAsync();
                        return Json(new { ok = "ok" });
                    }
                }
                else
                {
                    var vkGroupFrom = await _context.VkGroupFromSet
                        .Include(f => f.Filter)
                        .FirstOrDefaultAsync(id => id.VkGroupFromId == group_id);

                    if (vkGroupFrom != null)
                    {
                        if (vkGroupFrom.Filter == null)
                        {
                            vkGroupFrom.Filter = filter;
                        }
                        else
                        {
                            vkGroupFrom.Filter.CopyWithAuthor = filter.CopyWithAuthor;
                            vkGroupFrom.Filter.GetOnlyGroupPost = filter.GetOnlyGroupPost;
                            vkGroupFrom.Filter.GetWithLink = filter.GetWithLink;
                            vkGroupFrom.Filter.GetWithPicture = filter.GetWithPicture;
                            vkGroupFrom.Filter.GetWithVkLink = filter.GetWithVkLink;
                            vkGroupFrom.Filter.GetWithWikiPage = filter.GetWithWikiPage;
                            vkGroupFrom.Filter.RemoveAuthor = filter.RemoveAuthor;
                            vkGroupFrom.Filter.RemoveTag = filter.RemoveTag;
                            vkGroupFrom.Filter.RemoveText = filter.RemoveText;
                            vkGroupFrom.Filter.RepalaceFrom1 = filter.RepalaceFrom1;
                            vkGroupFrom.Filter.RepalaceFrom2 = filter.RepalaceFrom2;
                            vkGroupFrom.Filter.RepalaceTo1 = filter.RepalaceTo1;
                            vkGroupFrom.Filter.RepalaceTo2 = filter.RepalaceTo2;
                        }

                        vkGroupFrom.Atribute = tags;
                        _context.Update(vkGroupFrom);
                        await _context.SaveChangesAsync();
                        return Json(new { ok = "ok" });
                    }
                }
            }

            return Json(new { ok = "error" });
        }
        #endregion

        #region присвоение статуса посту
        /// <summary>
        /// Пост был выделен
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<JsonResult> SelectText(string Id,
            string inst, string text, string url, string group_id, string coments, string likes, string date)
        {
            try
            {
                if (ModelState.IsValid && _context != null)
                {
                    if (!string.IsNullOrWhiteSpace(Id))
                    {
                        var item = await _context.ItemSet
                            .FirstOrDefaultAsync(id => id.ItemId == Id);

                        if (item != null)
                        {
                            DateTime dateNow = DateTime.Now;
                            long unixTime = ((DateTimeOffset)dateNow).ToUnixTimeSeconds();

                            item.Statuse = "Selected";
                            item.AddedTime = unixTime;
                            _context.Update(item);
                            await _context.SaveChangesAsync();

                            return Json(new { ok = "ok" });
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(inst) && !string.IsNullOrWhiteSpace(url))
                    {
                        var instagram = await _context.InstagramSet
                            .FirstOrDefaultAsync(id => id.InstagramId == inst);

                        if (instagram != null)
                        {
                            instagram.Statuse = "Selected";
                            instagram.Text = text;
                            instagram.Url = url;

                            _context.Update(instagram);
                        }
                        else
                        {
                            var vkGroup = await _context.VkGroupSet
                                .FirstOrDefaultAsync(id => id.VkGroupId == group_id);
                            if (vkGroup != null)
                            {
                                int.TryParse(date, out var d);
                                int.TryParse(likes, out var l);
                                int.TryParse(coments, out var c);

                                instagram = new Instagram()
                                {
                                    Date = d,
                                    Likes = l,
                                    Coment = c,
                                    Url = url,
                                    Text = text,
                                    VkGroupId = group_id,
                                    Statuse = "Selected"
                                };

                                await _context.AddAsync(instagram);
                            }
                        }

                        await _context.SaveChangesAsync();
                        return Json(new { ok = "ok", inst_id = instagram.InstagramId });
                    }
                }
            }
            catch (Exception ex)
            {
                await _telegramService.SendMessageExceptionAsync(ex);
            }

            return Json(new { ok = "error" });
        }

        /// <summary>
        /// Было снято выделение с поста
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<JsonResult> UnselectText(string Id,
            string inst)
        {
            if (ModelState.IsValid && _context != null)
            {
                if (!string.IsNullOrWhiteSpace(Id))
                {
                    var item = await _context.ItemSet
                        .FirstOrDefaultAsync(id => id.ItemId == Id);

                    if (item != null)
                    {
                        item.Statuse = null;
                        _context.Update(item);
                        await _context.SaveChangesAsync();

                        return Json(new { ok = "ok" });
                    }
                }
                else if (!string.IsNullOrWhiteSpace(inst))
                {
                    var instagram = await _context.InstagramSet
                        .FirstOrDefaultAsync(id => id.InstagramId == inst);

                    if (instagram != null)
                    {
                        instagram.Statuse = null;

                        _context.Update(instagram);
                    }

                    await _context.SaveChangesAsync();
                    return Json(new { ok = "ok", inst_id = instagram.InstagramId });
                }
            }

            return Json(new { ok = "error" });
        }

        /// <summary>
        /// Пост был удален
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<JsonResult> DeleateText(string Id,
            string inst)
        {
            if (ModelState.IsValid && _context != null)
            {
                if (!string.IsNullOrWhiteSpace(Id))
                {
                    var item = await _context.ItemSet
                        .FirstOrDefaultAsync(id => id.ItemId == Id);

                    if (item != null)
                    {
                        item.Statuse = "Deleated";
                        _context.Update(item);
                        await _context.SaveChangesAsync();

                        return Json(new { ok = "ok" });
                    }
                }
                else if (!string.IsNullOrWhiteSpace(inst))
                {
                    var instagram = await _context.InstagramSet
                        .FirstOrDefaultAsync(id => id.InstagramId == inst);

                    if (instagram != null)
                    {
                        _context.Remove(instagram);
                    }

                    await _context.SaveChangesAsync();
                    return Json(new { ok = "ok", inst_id = "true" });
                }
            }

            return Json(new { ok = "error" });
        }

        /// <summary>
        /// Отмена удаления поста
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<JsonResult> UndeleateText(string Id)
        {
            if (ModelState.IsValid && !string.IsNullOrWhiteSpace(Id) && _context != null)
            {
                var item = await _context.ItemSet
                    .FirstOrDefaultAsync(id => id.ItemId == Id);

                if (item != null)
                {
                    item.Statuse = null;
                    _context.Update(item);
                    await _context.SaveChangesAsync();

                    return Json(new { ok = "ok" });
                }
            }

            return Json(new { ok = "error" });
        }
        #endregion

        #region отправка постов
        /// <summary>
        /// Постинг выбранных постов
        /// </summary>
        /// <param name="group_id"></param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<JsonResult> WallPost(string group_id, string inst)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    lock (thisLock)
                    {
                        return SetWall(group_id, inst).Result;
                    }
                }
            }
            catch (Exception ex)
            {
                await _telegramService.SendMessageExceptionAsync(ex);
            }

            return Json(new { ok = "error" });
        }

        /// <summary>
        /// Загружаем нужные посты и информацию о них
        /// </summary>
        /// <param name="group_id"></param>
        /// <returns></returns>
        private async Task<JsonResult> SetWall(string group_id, string inst)
        {
            try
            {
                if (_context != null)
                {
                    var addTime = 30 * 60;
                    DateTime dateNow = DateTime.Now;
                    long unixTime = ((DateTimeOffset)dateNow).ToUnixTimeSeconds();

                    // достаём пользователя для токена
                    var user = await _userManager.GetUserAsync(HttpContext.User);
                    var vkUser = await _context.VkUserSet.
                        FirstOrDefaultAsync(vk => vk.VkUserId == user.VkUserId);

                    // достаём группу
                    var vkGroup = await _context.VkGroupSet.
                        FirstOrDefaultAsync(id => id.VkGroupId == group_id);

                    if (vkGroup != null)
                    {
                        //WallPostAsync
                        var access_token = vkUser.AccessToken;
                        // берем посты из очереди, для определения времени публикаций
                        var timeList = await _vkService.GetTimePostAsync(access_token, "100", "0", $"-{vkGroup.GroupId}", vkGroup.Domain);
                        var count = timeList.Count;

                        // Проверяем нет-ли в очереди больше 75 постов
                        if (count < 75)
                        {
                            var timeFrom = vkGroup.TimeFrom;
                            var timeTo = vkGroup.TimeTo;

                            // настройка времени
                            await SetTimeAsync(group_id, count, unixTime, addTime, timeList, timeFrom, timeTo, inst);

                            // создаем vkItem для публикации
                            IEnumerable<VkItemViewModel> vkItem = null;
                            if (string.IsNullOrWhiteSpace(inst))
                            {
                                vkItem = (await _context.VkGroupFromSet
                                    .Where(id => id.VkGroupId == group_id)
                                    .SelectMany(gr => gr.WallGets
                                    .SelectMany(wall => wall.Items
                                    .Where(st =>
                                        st.Statuse == "Cansend" &&
                                        //st.PublishDate > unixTime && //не грузить старое... ?
                                        (st.TimeKey == unixTime || st.TimeKey + addTime < unixTime))
                                    .Select(item => new
                                    {
                                        DocExt = item.Attachments.Select(doc => doc.Doc.Ext),
                                        Gif = item.Attachments.Select(doc => doc.Doc.Url),
                                        PublishDate = item.PublishDate,
                                        GroupId = gr.GroupId,
                                        ItemId = item.ItemId,
                                        Photo = item.Attachments.Select(ph => ph.Photo),
                                        Text = item.Text
                                    })))
                                    .ToListAsync())
                                    .Select(item => new VkItemViewModel
                                    {
                                        DocExt = item.DocExt?.ToList(),
                                        Gif = item.Gif?.ToList(),
                                        PublishDate = item.PublishDate,
                                        GroupId = item.GroupId,
                                        ItemId = item.ItemId,
                                        Photo = item.Photo?.Select(ph => ph?.Photo2560 ?? ph?.Photo1280 ?? ph?.Photo807
                                            ?? ph?.Photo604 ?? ph?.Photo130 ?? ph?.Photo75).ToList(),
                                        Text = item.Text
                                    });
                            }
                            else
                            {
                                vkItem = await _context.InstagramSet
                                    .Where(id => id.VkGroupId == group_id)
                                    .Where(st =>
                                        st.Statuse == "Cansend" &&
                                        //st.PublishDate > unixTime &&
                                        (st.TimeKey == unixTime || st.TimeKey + addTime < unixTime))
                                    .Select(item => new VkItemViewModel
                                    {
                                        PublishDate = item.PublishDate,
                                        GroupId = group_id,
                                        ItemId = item.InstagramId,
                                        Photo = new List<string> { item.Url },
                                        Text = item.Text
                                    })
                                    .ToListAsync();
                            }

                            foreach (var item in vkItem)
                            {
                                var type = string.Empty;
                                if (item.DocExt?.FirstOrDefault() == "gif")
                                {
                                    item.Photo = item.Gif;
                                    type = "gif";
                                }

                                var wallPost = await _vkService
                                    .WallPostAsync(access_token, vkGroup.GroupId, item.Text, item.PublishDate.ToString(), type, item.Photo?.ToArray());

                                if (!string.IsNullOrWhiteSpace(wallPost) && !wallPost.Contains("error"))
                                {
                                    if (string.IsNullOrWhiteSpace(inst))
                                    {
                                        var it = await _context.ItemSet
                                            .FirstOrDefaultAsync(id => id.ItemId == item.ItemId);

                                        it.Statuse = "Sanded";
                                        _context.Update(it);
                                    }
                                    else
                                    {
                                        var it = await _context.InstagramSet
                                            .FirstOrDefaultAsync(id => id.InstagramId == item.ItemId);

                                        it.Statuse = "Sanded";
                                        _context.Update(it);
                                    }

                                    await _context.SaveChangesAsync();

                                    await _telegramService.SendMessage($"User: {user.UserName}\nVkUser: {vkUser.UserVkId}\nGroup: {vkGroup.Domain ?? vkGroup.GroupId}\nTime: {DateTimeOffset.FromUnixTimeSeconds(item.PublishDate):F}");
                                }
                                else
                                {
                                    await _telegramService.SendMessage($"{user.UserName} error: {string.Concat(wallPost.Take(2500))}");

                                    if (wallPost.Contains("can only schedule 25 posts on a day"))
                                    {
                                        break;
                                    }
                                }
                            }

                            return Json(new { ok = "ok" });
                        }
                        else
                        {
                            await _telegramService.SendMessage($"{user.UserName} длинна очереди более 75, отмена.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await _telegramService.SendMessageExceptionAsync(ex);
            }

            return Json(new { ok = "error" });
        }

        /// <summary>
        /// Менеджмент времени
        /// </summary>
        /// <param name="group_id"></param>
        /// <param name="count"></param>
        /// <param name="unixTime"></param>
        /// <param name="addTime"></param>
        /// <param name="timeList"></param>
        /// <param name="timeFrom"></param>
        /// <param name="timeTo"></param>
        /// <returns></returns>
        private async Task SetTimeAsync(string group_id, int count, long unixTime, int addTime, List<long> timeList, int? timeFrom, int? timeTo, string inst)
        {
            try
            {
                var length = 0;
                long timeBefore = unixTime;

                // инстаграм или нет
                if (string.IsNullOrWhiteSpace(inst))
                {
                    // время будущих постов
                    var times = await _context.VkGroupFromSet
                        .Where(id => id.VkGroupId == group_id)
                        .SelectMany(gr => gr.WallGets
                        .SelectMany(wall => wall.Items
                        .Select(it => it.PublishDate)))
                        .Where(v => v > unixTime)
                        .ToListAsync();

                    timeList = timeList.Union(times).ToList();
                }
                else
                {
                    var times = await _context.InstagramSet
                        .Where(id => id.VkGroupId == group_id)
                        .Select(it => it.PublishDate)
                        .Where(v => v > unixTime)
                        .ToListAsync();

                    timeList = timeList.Union(times).ToList();
                }

                timeList.Sort();

                if (string.IsNullOrWhiteSpace(inst))
                {
                    // выбираем нужные посты, для публикации, для установки времени, и статус Cansend
                    var upStatuse = await _context.VkGroupFromSet
                    .Where(id => id.VkGroupId == group_id)
                    .SelectMany(gr => gr.WallGets
                    .SelectMany(wall => wall.Items
                        .Where(st => st.Statuse == "Selected" ||
                        (st.Statuse == "Cansend" && st.PublishDate < timeBefore) ||
                        (st.Statuse == "Cansend" && (st.TimeKey + 30 * 60) < timeBefore))))
                    .ToListAsync();

                    UpdateTime(count, unixTime, addTime, timeList, timeFrom, timeTo, length, timeBefore, upStatuse);

                    _context.UpdateRange(upStatuse);
                    await _context.SaveChangesAsync();
                    timeList = null;
                }
                else
                {
                    var upStatuse = await _context.InstagramSet
                        .Where(id => id.VkGroupId == group_id)
                        .Where(st => st.Statuse == "Selected" ||
                        (st.Statuse == "Cansend" && st.PublishDate < timeBefore) ||
                        (st.Statuse == "Cansend" && (st.TimeKey + 30 * 60) < timeBefore))
                        .ToListAsync();

                    UpdateTime(count, unixTime, addTime, timeList, timeFrom, timeTo, length, timeBefore, upStatuse);

                    _context.UpdateRange(upStatuse);
                    await _context.SaveChangesAsync();
                    timeList = null;
                }
            }
            catch (Exception ex)
            {
                await _telegramService.SendMessageExceptionAsync(ex);
            }
        }

        /// <summary>
        /// Устанавливаем нужное время
        /// </summary>
        /// <param name="count"></param>
        /// <param name="unixTime"></param>
        /// <param name="addTime"></param>
        /// <param name="timeList"></param>
        /// <param name="timeFrom"></param>
        /// <param name="timeTo"></param>
        /// <param name="length"></param>
        /// <param name="timeBefore"></param>
        /// <param name="upStatuse"></param>
        private void UpdateTime(int count, long unixTime, int addTime, List<long> timeList, int? timeFrom, int? timeTo, int length, long timeBefore, dynamic upStatuse)
        {
            foreach (var item in upStatuse)
            {
                if (timeFrom != null && timeTo != null)
                {
                    addTime = rnd.Next(timeFrom.Value * 60, timeTo.Value * 60);
                }

                count = timeList.Count;

                var tmpTime = unixTime;
                if (count > 0 && timeList[0] > unixTime)
                {
                    var newTime = unixTime;
                    while (newTime < timeList[0])
                    {
                        var middle = (timeList[0] - newTime) / 2;
                        if (middle > (addTime - 60))
                        {
                            if (!timeList.Contains(newTime))
                            {
                                unixTime = newTime;
                                break;
                            }
                        }

                        newTime += addTime;
                    }
                }

                if (unixTime == tmpTime)
                {
                    for (int i = 0; i < count - 1; i++)
                    {
                        var newTime = timeList[i] + addTime;
                        var middle = (timeList[i + 1] - timeList[i]) / 2;
                        if (middle > (addTime - 60) &&
                            !timeList.Contains(newTime))
                        {
                            unixTime = newTime;
                            break;
                        }
                    }
                }

                if (tmpTime == unixTime)
                {
                    if (count > 0)
                    {
                        unixTime = timeList[count - 1] + addTime;
                    }
                    else
                    {
                        unixTime += addTime;
                    }
                }

                while (timeList.Contains(unixTime))
                {
                    unixTime += addTime;
                }

                timeList.Add(unixTime);
                timeList.Sort();

                item.Statuse = "Cansend";
                item.PublishDate = unixTime;
                item.TimeKey = timeBefore;
                length++;
            }
        }
        #endregion

        #region Auto replace
        /// <summary>
        /// удалить хэш-тэги из текста
        /// </summary>
        /// <param name="text_id"></param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<JsonResult> RemoveTag(string text_id, string inst, string text, string group_id)
        {
            if (ModelState.IsValid && _context != null)
            {
                if (!string.IsNullOrWhiteSpace(text_id))
                {
                    var item = await _context.ItemSet
                        .FirstOrDefaultAsync(id => id.ItemId == text_id);

                    if (item != null)
                    {
                        item.BackUpText = item.Text;

                        item.Text = await _textService.RemoveTagAsync(item.Text);

                        _context.Update(item);
                        await _context.SaveChangesAsync();

                        return Json(new { ok = "ok", text = item.Text });
                    }
                }
                else if (!string.IsNullOrWhiteSpace(inst))
                {
                    var instagram = await _context.InstagramSet
                        .FirstOrDefaultAsync(id => id.InstagramId == inst);
                    if (instagram != null)
                    {
                        instagram.BackUpText = instagram.Text;
                        instagram.Text = await _textService.RemoveTagAsync(instagram.Text);
                        _context.Update(instagram);
                    }
                    else
                    {
                        var vkGroup = await _context.VkGroupSet
                            .FirstOrDefaultAsync(id => id.VkGroupId == group_id);
                        if (vkGroup != null)
                        {
                            instagram = new Instagram
                            {
                                BackUpText = text,
                                VkGroupId = vkGroup.VkGroupId,
                                Text = await _textService.RemoveTagAsync(text)
                            };
                        }

                        await _context.AddAsync(instagram);
                    }

                    await _context.SaveChangesAsync();

                    return Json(new
                    {
                        ok = "ok",
                        text = instagram.Text,
                        inst_id = instagram.InstagramId
                    });
                }
            }

            return Json(new { ok = "error" });
        }

        /// <summary>
        /// Удаляем ссылки из текста
        /// </summary>
        /// <param name="text_id"></param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<JsonResult> RemoveLink(string text_id, string inst, string text, string group_id)
        {
            if (ModelState.IsValid && _context != null)
            {
                if (!string.IsNullOrWhiteSpace(text_id))
                {
                    var item = await _context.ItemSet
                    .FirstOrDefaultAsync(id => id.ItemId == text_id);

                    if (item != null)
                    {
                        item.BackUpText = item.Text;

                        item.Text = await _textService.RemoveLinkAsync(item.Text);

                        _context.Update(item);
                        await _context.SaveChangesAsync();

                        return Json(new { ok = "ok", text = item.Text });
                    }
                }
                else if (!string.IsNullOrWhiteSpace(inst))
                {
                    var instagram = await _context.InstagramSet
                        .FirstOrDefaultAsync(id => id.InstagramId == inst);
                    if (instagram != null)
                    {
                        instagram.BackUpText = instagram.Text;
                        instagram.Text = await _textService.RemoveLinkAsync(instagram.Text);
                        _context.Update(instagram);
                    }
                    else
                    {
                        var vkGroup = await _context.VkGroupSet
                            .FirstOrDefaultAsync(id => id.VkGroupId == group_id);
                        if (vkGroup != null)
                        {
                            instagram = new Instagram
                            {
                                BackUpText = text,
                                VkGroupId = vkGroup.VkGroupId,
                                Text = await _textService.RemoveLinkAsync(text)
                            };
                        }

                        await _context.AddAsync(instagram);
                    }

                    await _context.SaveChangesAsync();

                    return Json(new
                    {
                        ok = "ok",
                        text = instagram.Text,
                        inst_id = instagram.InstagramId
                    });
                }
            }

            return Json(new { ok = "error" });
        }

        /// <summary>
        /// Удаление автора из текста
        /// </summary>
        /// <param name="text_id"></param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<JsonResult> RemoveAuthor(string text_id, string inst, string text, string group_id)
        {
            if (ModelState.IsValid && _context != null)
            {
                if (!string.IsNullOrWhiteSpace(text_id))
                {
                    var item = await _context.ItemSet
                    .FirstOrDefaultAsync(id => id.ItemId == text_id);

                    if (item != null)
                    {
                        item.BackUpText = item.Text;

                        item.Text = await _textService.RemoveAuthorAsync(item.Text);

                        _context.Update(item);
                        await _context.SaveChangesAsync();

                        return Json(new { ok = "ok", text = item.Text });
                    }
                }
                else if (!string.IsNullOrWhiteSpace(inst))
                {
                    var instagram = await _context.InstagramSet
                        .FirstOrDefaultAsync(id => id.InstagramId == inst);
                    if (instagram != null)
                    {
                        instagram.BackUpText = instagram.Text;
                        instagram.Text = await _textService.RemoveAuthorAsync(instagram.Text);
                        _context.Update(instagram);
                    }
                    else
                    {
                        var vkGroup = await _context.VkGroupSet
                            .FirstOrDefaultAsync(id => id.VkGroupId == group_id);
                        if (vkGroup != null)
                        {
                            instagram = new Instagram
                            {
                                BackUpText = text,
                                VkGroupId = vkGroup.VkGroupId,
                                Text = await _textService.RemoveAuthorAsync(text)
                            };
                        }

                        await _context.AddAsync(instagram);
                    }

                    await _context.SaveChangesAsync();

                    return Json(new
                    {
                        ok = "ok",
                        text = instagram.Text,
                        inst_id = instagram.InstagramId
                    });
                }
            }

            return Json(new { ok = "error" });
        }

        /// <summary>
        /// Удалить вики ссылку
        /// </summary>
        /// <param name="text_id"></param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<JsonResult> RemoveWikiLink(string text_id, string inst, string text, string group_id)
        {
            if (ModelState.IsValid && _context != null)
            {
                if (!string.IsNullOrWhiteSpace(text_id))
                {
                    var item = await _context.ItemSet
                    .FirstOrDefaultAsync(id => id.ItemId == text_id);

                    if (item != null)
                    {
                        item.BackUpText = item.Text;

                        item.Text = await _textService.RemoveVkLinkAsync(item.Text);

                        _context.Update(item);
                        await _context.SaveChangesAsync();

                        return Json(new { ok = "ok", text = item.Text });
                    }
                }
                else if (!string.IsNullOrWhiteSpace(inst))
                {
                    var instagram = await _context.InstagramSet
                        .FirstOrDefaultAsync(id => id.InstagramId == inst);
                    if (instagram != null)
                    {
                        instagram.BackUpText = instagram.Text;
                        instagram.Text = await _textService.RemoveVkLinkAsync(instagram.Text);
                        _context.Update(instagram);
                    }
                    else
                    {
                        var vkGroup = await _context.VkGroupSet
                            .FirstOrDefaultAsync(id => id.VkGroupId == group_id);
                        if (vkGroup != null)
                        {
                            instagram = new Instagram
                            {
                                BackUpText = text,
                                VkGroupId = vkGroup.VkGroupId,
                                Text = await _textService.RemoveVkLinkAsync(text)
                            };
                        }

                        await _context.AddAsync(instagram);
                    }

                    await _context.SaveChangesAsync();

                    return Json(new
                    {
                        ok = "ok",
                        text = instagram.Text,
                        inst_id = instagram.InstagramId
                    });
                }
            }

            return Json(new { ok = "error" });
        }

        /// <summary>
        /// Удалить текст
        /// </summary>
        /// <param name="text_id"></param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<JsonResult> RemoveText(string text_id, string inst, string text, string group_id)
        {
            if (ModelState.IsValid && _context != null)
            {
                if (!string.IsNullOrWhiteSpace(text_id))
                {
                    var item = await _context.ItemSet
                    .FirstOrDefaultAsync(id => id.ItemId == text_id);

                    if (item != null)
                    {
                        item.BackUpText = item.Text;

                        item.Text = string.Empty;

                        _context.Update(item);
                        await _context.SaveChangesAsync();

                        return Json(new { ok = "ok", text = item.Text });
                    }
                }
                else if (!string.IsNullOrWhiteSpace(inst))
                {
                    var instagram = await _context.InstagramSet
                        .FirstOrDefaultAsync(id => id.InstagramId == inst);
                    if (instagram != null)
                    {
                        instagram.BackUpText = instagram.Text;
                        instagram.Text = string.Empty;
                        _context.Update(instagram);
                    }
                    else
                    {
                        var vkGroup = await _context.VkGroupSet
                            .FirstOrDefaultAsync(id => id.VkGroupId == group_id);
                        if (vkGroup != null)
                        {
                            instagram = new Instagram
                            {
                                BackUpText = text,
                                VkGroupId = vkGroup.VkGroupId,
                                Text = string.Empty
                            };
                        }

                        await _context.AddAsync(instagram);
                    }

                    await _context.SaveChangesAsync();

                    return Json(new
                    {
                        ok = "ok",
                        text = instagram.Text,
                        inst_id = instagram.InstagramId
                    });
                }
            }

            return Json(new { ok = "error" });
        }

        /// <summary>
        /// Восстановить текст
        /// </summary>
        /// <param name="text_id"></param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<JsonResult> BackUpText(string text_id, string inst, string text, string group_id)
        {
            if (ModelState.IsValid && _context != null)
            {
                if (!string.IsNullOrWhiteSpace(text_id))
                {
                    var item = await _context.ItemSet
                    .FirstOrDefaultAsync(id => id.ItemId == text_id);

                    if (item != null)
                    {
                        // Возможность вернуть обратно
                        var tmp = item.BackUpText;
                        item.BackUpText = item.Text;
                        item.Text = tmp;

                        _context.Update(item);
                        await _context.SaveChangesAsync();

                        return Json(new { ok = "ok", text = item.Text });
                    }
                }
                else if (!string.IsNullOrWhiteSpace(inst))
                {
                    var instagram = await _context.InstagramSet
                        .FirstOrDefaultAsync(id => id.InstagramId == inst);
                    if (instagram != null)
                    {
                        // Возможность вернуть обратно
                        var tmp = instagram.BackUpText;
                        instagram.BackUpText = instagram.Text;
                        instagram.Text = tmp;

                        _context.Update(instagram);
                    }

                    await _context.SaveChangesAsync();

                    return Json(new
                    {
                        ok = "ok",
                        text = instagram?.Text,
                        inst_id = instagram?.InstagramId
                    });
                }
            }

            return Json(new { ok = "error" });
        }
        #endregion
    }
}
