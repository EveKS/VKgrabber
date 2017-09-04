using Newtonsoft.Json;

namespace VkGroupManager.JsonModel.VK
{
    public class Counters
    {

        [JsonProperty("albums")]
        public int Albums { get; set; }

        [JsonProperty("videos")]
        public int Videos { get; set; }

        [JsonProperty("audios")]
        public int Audios { get; set; }

        [JsonProperty("notes")]
        public int Notes { get; set; }

        [JsonProperty("photos")]
        public int Photos { get; set; }

        [JsonProperty("groups")]
        public int Groups { get; set; }

        [JsonProperty("gifts")]
        public int Gifts { get; set; }

        [JsonProperty("followers")]
        public int Followers { get; set; }
    }
}
