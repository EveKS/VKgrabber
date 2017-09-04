using Newtonsoft.Json;
using System.Collections.Generic;

namespace VkGroupManager.JsonModel.Instagram
{
    public class EntryData
    {

        [JsonProperty("ProfilePage")]
        public IList<ProfilePage> ProfilePage { get; set; }
    }
}
