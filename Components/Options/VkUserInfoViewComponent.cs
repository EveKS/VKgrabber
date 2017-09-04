using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using VkGroupManager.JsonModel.VK;
using VkGroupManager.Models;
using VkGroupManager.Models.VK;
using VkGroupManager.Service.JSON;
using VkGroupManager.Service.Telegram;
using VkGroupManager.Service.VK;
using VkGroupManager.ViewModels.VK;

namespace VkGroupManager.Components.Options
{
    public class VkUserInfoViewComponent : ViewComponent
    {
        private ApplicationContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private ITelegramService _telegramService;
        private IJsonService _jsonService;
        private IVkService _vkService;

        public VkUserInfoViewComponent(ApplicationContext context,
            SignInManager<User> signInManager, UserManager<User> userManager,
            ITelegramService telegramService,
            IJsonService jsonService,
            IVkService vkService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
            _telegramService = telegramService;
            _jsonService = jsonService;
            _vkService = vkService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (_context != null && _signInManager.IsSignedIn(HttpContext.User))
            {
                try
                {
                    var user = await _userManager.GetUserAsync(HttpContext.User);
                    var order = await _context.Orders
                        .FirstOrDefaultAsync(id => user.OrderId == id.OrderId);
                    var isPremium = order?.DateEnd > DateTime.Now;

                    var vkUser = await _context
                        .VkUserSet?
                        .AsNoTracking()
                        .FirstOrDefaultAsync(id => id.VkUserId == user.VkUserId);

                    List<VkGroup> vkGroups = null;

                    if (isPremium)
                    {
                        vkGroups = await _context
                        .VkGroupSet
                        .Where(id => id.VkUserId == vkUser.VkUserId)
                        .Include(gr => gr.GroupInfo)
                        .AsNoTracking()
                        .ToListAsync();
                    }
                    else
                    {
                        vkGroups = new List<VkGroup>
                        {
                            await _context
                                .VkGroupSet
                                .Where(id => id.VkUserId == vkUser.VkUserId)
                                .Include(gr => gr.GroupInfo)
                                .AsNoTracking().FirstOrDefaultAsync()
                        };
                    }

                    var result = await _vkService.GetProfilesAsync(vkUser);

                    var profile = _jsonService.JsonConvertDeserializeObject<GetProfiles>(result)?
                        .Profile?.FirstOrDefault();

                    VkProfileViewModel vkProfile = new VkProfileViewModel();
                    if (profile != null)
                    {
                        vkProfile = new VkProfileViewModel
                        {
                            FirstName = profile.FirstName,
                            LastName = profile.LastName,
                            AccessToken = vkUser.AccessToken,
                            VkGroups = vkGroups,
                            UserVkId = vkUser.UserVkId,
                            IsOrdered = isPremium,
                            Order = order
                        };

                        if (profile.HasPhoto != 0)
                        {
                            vkProfile.Photo = profile.PhotoMax ??
                                profile.Photo200Orig ??
                                profile.Photo200 ??
                                profile.Photo100 ??
                                profile.Photo50;
                        }
                    }

                    return View(vkProfile);
                }
                catch (Exception ex)
                {
                    await _telegramService.SendMessageExceptionAsync(ex);
                }
            }

            return View();
        }
    }
}
