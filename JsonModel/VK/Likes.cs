using Newtonsoft.Json;

namespace VkGroupManager.JsonModel.VK
{
    public class Likes
    {
        public string LikesId { get; set; }

        [JsonProperty("count")]
        public int? Count { get; set; }

        [JsonProperty("user_likes")]
        public int UserLikes { get; set; }

        [JsonProperty("can_like")]
        public int CanLike { get; set; }

        [JsonProperty("can_publish")]
        public int CanPublish { get; set; }
    }
}
