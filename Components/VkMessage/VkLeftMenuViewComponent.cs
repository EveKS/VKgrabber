using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkGroupManager.ViewModels.VK;

namespace VkGroupManager.Components.VkMessage
{
    public class VkLeftMenuViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(VkResponseAndGroupVk vkResponseAndGroupVk)
        {
            return View(vkResponseAndGroupVk);
        }
    }
}
