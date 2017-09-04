using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkGroupManager.Models;
using VkGroupManager.Service.Telegram;
using VkGroupManager.Services;

namespace VkGroupManager.Controllers
{
    public class EmailController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private IEmailSender _emailSender;

        private ITelegramService _telegramService;

        public EmailController(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IEmailSender emailSender,
            ITelegramService telegramService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;

            _telegramService = telegramService;
        }

        [HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<JsonResult> GetEmail(string name, string email, string message)
        {
            try
            {
                if (ModelState.IsValid &&
                    !string.IsNullOrWhiteSpace(name) &&
                    !string.IsNullOrWhiteSpace(email) &&
                    !string.IsNullOrWhiteSpace(message))
                {
                    var user = await _userManager.GetUserAsync(HttpContext.User);

                    await _emailSender.SendEmailAsync("contact@vkgraber.ru",
                        $"Сообщение от {name} на vkgraber.ru",
                             $"name: {name} <br />" +
                             $"email: {email} <br />" +
                             $"user: {user?.UserName} <br />" +
                             $"user-email: {user?.Email} <br />" +
                             $"message: {message}");

                    await _emailSender.SendEmailAsync(email,
                        $"Вы отправили сообщение на сайте vkgraber.ru",
                             $"Имя: {name} <br />" +
                             $"Email: {email} <br />" +
                             $"Сообщение: {message}");

                    return Json(new { ok = "ok" });
                }
            }
            catch(Exception ex)
            {
                await _telegramService.SendMessageExceptionAsync(ex);
            }

            return Json(new { ok = "error" });
        }
    }
}
