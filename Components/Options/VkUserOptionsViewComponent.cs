using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkGroupManager.Models;
using VkGroupManager.Models.VK;
using VkGroupManager.ViewModels.VK;

namespace VkGroupManager.Components.Options
{
    public class VkUserOptionsViewComponent : ViewComponent
    {
        private ApplicationContext _context;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public VkUserOptionsViewComponent(ApplicationContext context,
            SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(VkClient vkClient)
        {
            if (_context != null && _signInManager.IsSignedIn(HttpContext.User))
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);

                var vkUser = await _context
                    .VkUserSet?
                    .FirstOrDefaultAsync(id => id.VkUserId == user.VkUserId);

                var vkClientViewModel = new VkClientViewModel
                {
                    AccessToken = vkUser?.AccessToken,
                    UserId = vkUser?.UserVkId
                };

                if (vkClient != null)
                {
                    vkClientViewModel.ResponseType = vkClient.ResponseType;
                    vkClientViewModel.RedirectUri = vkClient.RedirectUri;
                    vkClientViewModel.Scope = vkClient.Scope;
                    vkClientViewModel.ClientId = vkClient.ClientId;
                    vkClientViewModel.Display = vkClient.Display;
                }

                return View(vkClientViewModel);
            }

            return View();
        }
    }
}
