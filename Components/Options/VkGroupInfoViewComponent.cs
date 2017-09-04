using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public class VkGroupInfoViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(VkProfileViewModel vkProfileViewModel)
        {  
            return View(vkProfileViewModel);
        }
    }
}
