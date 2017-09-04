using Newtonsoft.Json;
using VkGroupManager.Models.VK;

namespace VkGroupManager.JsonModel.VK
{
    public class GroupInfo
    {
        public string GroupInfoId { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("screen_name")]
        public string ScreenName { get; set; }

        [JsonProperty("is_closed")]
        public int IsClosed { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("is_admin")]
        public int IsAdmin { get; set; }

        [JsonProperty("is_member")]
        public int IsMember { get; set; }

        [JsonProperty("photo_50")]
        public string Photo50 { get; set; }

        [JsonProperty("photo_100")]
        public string Photo100 { get; set; }

        [JsonProperty("photo_200")]
        public string Photo200 { get; set; }

        public string WallGetId { get; set; }
        public WallGet WallGet { get; set; }
    }
}
