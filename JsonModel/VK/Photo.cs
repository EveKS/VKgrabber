using Newtonsoft.Json;
using System.Collections.Generic;

namespace VkGroupManager.JsonModel.VK
{
    public class Photo
    {
        public string PhotoId { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("album_id")]
        public int AlbumId { get; set; }

        [JsonProperty("owner_id")]
        public int OwnerId { get; set; }

        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("photo_75")]
        public string Photo75 { get; set; }

        [JsonProperty("photo_130")]
        public string Photo130 { get; set; }

        [JsonProperty("photo_604")]
        public string Photo604 { get; set; }

        [JsonProperty("photo_807")]
        public string Photo807 { get; set; }

        [JsonProperty("photo_1280")]
        public string Photo1280 { get; set; }

        [JsonProperty("photo_2560")]
        public string Photo2560 { get; set; }        

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("date")]
        public int Date { get; set; }

        [JsonProperty("post_id")]
        public int PostId { get; set; }

        [JsonProperty("access_key")]
        public string AccessKey { get; set; }

        [JsonProperty("sizes")]
        public IList<Size> Sizes { get; set; }

        public Photo()
        {
            Sizes = new List<Size>();
        }
    }
}
