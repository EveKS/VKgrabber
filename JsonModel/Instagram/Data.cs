using Newtonsoft.Json;

namespace VkGroupManager.JsonModel.Instagram
{
    public class Data
    {

        [JsonProperty("user")]
        public User User { get; set; }
    }
}
