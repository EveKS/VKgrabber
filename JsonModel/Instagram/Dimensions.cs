using Newtonsoft.Json;

namespace VkGroupManager.JsonModel.Instagram
{
    public class Dimensions
    {

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }
    }
}
