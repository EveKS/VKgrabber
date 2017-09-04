using Newtonsoft.Json;

namespace VkGroupManager.JsonModel.VK
{
    public class Views
    {
        public string ViewsId { get; set; }

        [JsonProperty("count")]
        public int? Count { get; set; }
    }
}
