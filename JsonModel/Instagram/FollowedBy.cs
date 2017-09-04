using Newtonsoft.Json;

namespace VkGroupManager.JsonModel.Instagram
{
    public class FollowedBy
    {

        [JsonProperty("count")]
        public int Count { get; set; }
    }
}
