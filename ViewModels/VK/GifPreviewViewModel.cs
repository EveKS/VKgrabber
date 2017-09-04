using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkGroupManager.JsonModel.VK;

namespace VkGroupManager.ViewModels.VK
{
    public class GifPreviewViewModel
    {
        public string Gif { get; set; }
        public string DocExt { get; set; }
        public List<Size> PreviewPhoto { get; set; }
    }
}
