using Newtonsoft.Json;

namespace VkGroupManager.JsonModel.VK
{
    public class Video
    {
        public string VideoId { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("owner_id")]
        public int OwnerId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("duration")]
        public int Duration { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("date")]
        public int Date { get; set; }

        [JsonProperty("comments")]
        public int Comments { get; set; }

        [JsonProperty("views")]
        public int Views { get; set; }

        [JsonProperty("photo_130")]
        public string Photo130 { get; set; }

        [JsonProperty("photo_320")]
        public string Photo320 { get; set; }

        [JsonProperty("photo_640")]
        public string Photo640 { get; set; }

        [JsonProperty("access_key")]
        public string AccessKey { get; set; }

        [JsonProperty("platform")]
        public string Platform { get; set; }

        [JsonProperty("can_add")]
        public int CanAdd { get; set; }

        [JsonProperty("src")]
        public string Src { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("file_size")]
        public int FileSize { get; set; }
    }
}
