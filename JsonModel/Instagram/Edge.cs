using Newtonsoft.Json;

namespace VkGroupManager.JsonModel.Instagram
{
    public class Edge
    {

        [JsonProperty("node")]
        public Node Node { get; set; }
    }
}
