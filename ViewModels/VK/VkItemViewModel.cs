using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkGroupManager.JsonModel.VK;
using VkGroupManager.Models;

namespace VkGroupManager.ViewModels.VK
{
    public class VkItemViewModel
    {
        public int? Likes { get; set; }
        public int? Views { get; set; }
        public int? Reposts { get; set; }
        public int? Coments { get; set; }

        public string GroupId { get; set; }
        public GroupInfo GroupInfo { get; set; }
        public string ItemId { get; set; }
        public string Statuse { get; set; }
        public IEnumerable<string> Gif { get; set; }
        public IEnumerable<string> DocExt { get; set; }

        public IEnumerable<GifPreviewViewModel> GifPrew { get; set; }

        public Filter Filter { get; set; }
        public Filter FilterAll { get; set; }
        public string Atribute { get; set; }
        public string AtributeAll { get; set; }

        public long PublishDate { get; set; }
        public long? Date { get; set; }
        public string Text { get; set; }
        public IEnumerable<String> Photo { get; set; }
    }
}
