using Newtonsoft.Json;

namespace VkGroupManager.JsonModel.VK
{
    public class PostSource
    {
        public string PostSourceId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
