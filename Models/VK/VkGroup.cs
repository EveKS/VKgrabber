using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using VkGroupManager.JsonModel.VK;

namespace VkGroupManager.Models.VK
{
    public class VkGroup
    {
        public string VkGroupId { get; set; }

        [Display(Name = "group_id")]
        public string GroupId { get; set; }

        [Display(Name = "domain")]
        public string Domain { get; set; }

        public string AccessToken { get; set; }
        public string ExpiresIn { get; set; }

        public string VkUserId { get; set; }
        public VkUser VkUser { get; set; }

        public string Atribute { get; set; }

        public int? MaxLoad { get; set; }
        public int? MaxFrom { get; set; }

        public int? TimeFrom { get; set; }
        public int? TimeTo { get; set; }

        public string FilterId { get; set; }
        public Filter Filter { get; set; }

        public string GroupInfoId { get; set; }
        public GroupInfo GroupInfo { get; set; }

        public IList<Instagram> Instagrams { get; set; }
        public IList<VkGroupFrom> VkGroupsFrom { get; set; }
        public VkGroup()
        {
            Instagrams = new List<Instagram>();
            VkGroupsFrom = new List<VkGroupFrom>();
        }
    }
}
