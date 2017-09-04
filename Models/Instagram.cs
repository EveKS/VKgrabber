using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkGroupManager.Models.VK;

namespace VkGroupManager.Models
{
    public class Instagram
    {
        public string InstagramId { get; set; }

        public string Url { get; set; }
        public string Text { get; set; }
        public string BackUpText { get; set; }
        public long PublishDate { get; set; }
        public long TimeKey { get; set; }

        public int? Coment { get; set; }
        public int? Likes { get; set; }
        public long? Date { get; set; }

        public string Statuse { get; set; }

        public string VkGroupId { get; set; }
        public VkGroup VkGroup { get; set; }
    }
}
