using Newtonsoft.Json;

namespace VkGroupManager.JsonModel.Instagram
{
    public class Comments
    {

        [JsonProperty("count")]
        public int Count { get; set; }
    }
}
