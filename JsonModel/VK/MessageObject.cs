using Newtonsoft.Json;

namespace VkGroupManager.JsonModel.VK
{
    public class MessageObject
    {
        public string MessageObjectId { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("date")]
        public int Date { get; set; }

        [JsonProperty("out")]
        public int Out { get; set; }

        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("read_state")]
        public int ReadState { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }
    }
}
