using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkGroupManager.JsonModel.VK;

namespace VkGroupManager.ViewModels.VK
{
    public class VkWallViewModel
    {
        public string WallGetId { get; set; }
        public GroupInfo GroupInfo { get; set; }
        public List<VkItemViewModel> VkItemViewModels { get; set; }
    }
}
