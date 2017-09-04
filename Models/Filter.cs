using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VkGroupManager.Models
{
    public class Filter
    {
        public string FilterId { get; set; }

        public string RepalaceFrom1 { get; set; }
        public string RepalaceTo1 { get; set; }
        public string RepalaceFrom2 { get; set; }
        public string RepalaceTo2 { get; set; }

        public bool? GetWithLink { get; set; }
        public bool? GetWithPicture { get; set; }
        public bool? GetWithVkLink { get; set; }
        public bool? GetWithWikiPage { get; set; }

        public bool? CopyWithAuthor { get; set; }

        public bool? RemoveSmile { get; set; }
        public bool? RemoveAuthor { get; set; }
        public bool? RemoveText { get; set; }
        public bool? RemoveTag { get; set; }

        public bool? GetOnlyGroupPost { get; set; }
        // todo: public bool? GetOnlyOthersPost { get; set; }
    }
}
