using Newtonsoft.Json;

namespace VkGroupManager.JsonModel.Instagram
{
    public class ProfilePage
    {

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("logging_page_id")]
        public string LoggingPageId { get; set; }
    }
}
