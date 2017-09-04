using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkGroupManager.Models.VK;

namespace VkGroupManager.ViewModels.VK
{
    public class VkProfileViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Photo { get; set; }

        public string AccessToken { get; set; }

        public string Domain { get; set; }

        public string UserVkId { get; set; }

        public bool IsOrdered { get; set; }

        public Models.Order Order { get; set; }

        public List<VkGroup> VkGroups { get; set; }
    }
}
