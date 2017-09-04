using Newtonsoft.Json;
using System.Collections.Generic;

namespace VkGroupManager.JsonModel.Instagram
{
    public class Media
    {

        [JsonProperty("nodes")]
        public IList<Node> Nodes { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("page_info")]
        public PageInfo PageInfo { get; set; }
    }
}
