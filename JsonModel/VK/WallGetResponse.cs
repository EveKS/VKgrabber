using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace VkGroupManager.JsonModel.VK
{

    public class WallGetResponse
    {

        [JsonProperty("response")]
        public WallGet Response { get; set; }
    }
}
