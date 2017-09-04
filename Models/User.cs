using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkGroupManager.Models.VK;

namespace VkGroupManager.Models
{
    public class User : IdentityUser
    {
        public string InstagramToken { get; set; }
        public string InstagramOwnerId { get; set; }

        public string VkUserId { get; set; }
        public VkUser VkUser { get; set; }

        public string OrderId { get; set; }
        public Order Order { get; set; }

        public DateTime? RegesterDate { get; set; }
    }
}
