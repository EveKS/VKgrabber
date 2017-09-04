using Newtonsoft.Json;

namespace VkGroupManager.JsonModel.Instagram
{
    public class Gatekeepers
    {

        [JsonProperty("bn")]
        public bool Bn { get; set; }

        [JsonProperty("ld")]
        public bool Ld { get; set; }

        [JsonProperty("pl")]
        public bool Pl { get; set; }
    }
}
