using Newtonsoft.Json;

namespace VkGroupManager.JsonModel.VK
{
    public class Country
    {
        public string CountryId { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }
}
