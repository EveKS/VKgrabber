﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkGroupManager.Models;

namespace VkGroupManager.Components.Head
{
    public class LoginStatusViewComponent : ViewComponent
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public LoginStatusViewComponent(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                return View("LoggedIn", user);
            }
            else
            {
                return View();
            }
        }
    }
}
