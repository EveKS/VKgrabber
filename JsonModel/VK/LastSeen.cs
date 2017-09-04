using Newtonsoft.Json;

namespace VkGroupManager.JsonModel.VK
{
    public class LastSeen
    {

        [JsonProperty("time")]
        public int Time { get; set; }

        [JsonProperty("platform")]
        public int Platform { get; set; }
    }
}
