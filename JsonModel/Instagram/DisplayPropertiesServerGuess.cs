using Newtonsoft.Json;

namespace VkGroupManager.JsonModel.Instagram
{
    public class DisplayPropertiesServerGuess
    {

        [JsonProperty("pixel_ratio")]
        public double PixelRatio { get; set; }

        [JsonProperty("viewport_width")]
        public int ViewportWidth { get; set; }
    }
}
