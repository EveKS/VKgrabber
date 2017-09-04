using Newtonsoft.Json;

namespace VkGroupManager.JsonModel.VK
{
    public class Attachment
    {
        public string AttachmentId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("photo")]
        public Photo Photo { get; set; }

        [JsonProperty("video")]
        public Video Video { get; set; }

        [JsonProperty("doc")]
        public Doc Doc { get; set; }

        public string ItemId { get; set; }
        public Item Item { get; set; }
    }
}
