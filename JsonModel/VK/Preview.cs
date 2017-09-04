using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VkGroupManager.JsonModel.VK
{
    public class Preview
    {
        public string PreviewId { get; set; }

        [JsonProperty("photo")]
        public Photo Photo { get; set; }

        [JsonProperty("video")]
        public Video Video { get; set; }
    }
}
