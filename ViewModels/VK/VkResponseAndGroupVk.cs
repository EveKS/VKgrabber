using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkGroupManager.JsonModel.VK;
using VkGroupManager.Models;
using VkGroupManager.ViewModels.Order;

namespace VkGroupManager.ViewModels.VK
{
    public class VkResponseAndGroupVk
    {
        public IEnumerable<VkItemViewModel> Responses { get; set; }
        public VkGroupViewModel VkGroupViewModel { get; set; }
        public string AttributeAll { get; set; }
        public Filter Filter { get; set; }
        public string IsLoad { get; set; }
        public string IsInst { get; set; }
        public string Url { get; set; }
        public OrderViewModel Order { get; set; }
    }
}
