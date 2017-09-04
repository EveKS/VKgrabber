using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VkGroupManager.JsonModel.VK
{

    public class GetProfiles
    {

        [JsonProperty("response")]
        public IList<Profile> Profile { get; set; }
    }
}
