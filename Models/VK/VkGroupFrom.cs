using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using VkGroupManager.JsonModel.VK;

namespace VkGroupManager.Models.VK
{
    public class VkGroupFrom
    {
        public string VkGroupFromId { get; set; }

        public string GroupId { get; set; }

        [Display(Name = "domain")]
        public string Domain { get; set; }

        public string Atribute { get; set; }

        public string FilterId { get; set; }
        public Filter Filter { get; set; }

        public string GroupInfoId { get; set; }
        public GroupInfo GroupInfo { get; set; }

        public string VkGroupId { get; set; }
        public VkGroup VkGroup { get; set; }

        public IList<WallGet> WallGets { get; set; }
        public VkGroupFrom()
        {
            WallGets = new List<WallGet>();
        }
    }
}
