using Newtonsoft.Json;
using System.Collections.Generic;

namespace VkGroupManager.JsonModel.Instagram
{
    public class EdgeMediaToCaption
    {

        [JsonProperty("edges")]
        public IList<Edge> Edges { get; set; }
    }
}
