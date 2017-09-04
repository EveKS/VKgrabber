using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkGroupManager.Models;
using VkGroupManager.Models.VK;

namespace VkGroupManager.Components.Options
{
    public class VkClientOptionsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(VkClient vkClient)
        {
            return View(vkClient);
        }
    }
}
