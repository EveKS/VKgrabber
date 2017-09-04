using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VkGroupManager.Models.VK
{
    public class VkUser
    {
        public string VkUserId { get; set; }

        public string AccessToken { get; set; }
        public string ExpiresIn { get; set; }
        public string UserVkId { get; set; }

        public IList<VkGroup> VkGroups { get; set; }
        public VkUser()
        {
            VkGroups = new List<VkGroup>();
        }
    }
}
