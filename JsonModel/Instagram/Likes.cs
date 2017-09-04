using Newtonsoft.Json;

namespace VkGroupManager.JsonModel.Instagram
{
    public class Likes
    {

        [JsonProperty("count")]
        public int Count { get; set; }
    }
}
