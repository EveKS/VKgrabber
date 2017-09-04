using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VkGroupManager.JsonModel.VK
{

    public class NewMessage
    {
        public string NewMessageId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("object")]
        public MessageObject MessageObject { get; set; }

        [JsonProperty("group_id")]
        public int VkGroupId { get; set; }

        [JsonProperty("secret")]
        public string Secret { get; set; }
    }
}
