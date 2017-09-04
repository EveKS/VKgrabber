using Newtonsoft.Json;

namespace VkGroupManager.JsonModel.Instagram
{
    public class Follows
    {

        [JsonProperty("count")]
        public int Count { get; set; }
    }
}
