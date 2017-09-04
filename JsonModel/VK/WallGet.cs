using Newtonsoft.Json;
using System.Collections.Generic;
using VkGroupManager.Models.VK;

namespace VkGroupManager.JsonModel.VK
{
    public class WallGet
    {
        public string WallGetId { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("items")]
        public IList<Item> Items { get; set; }

        [JsonProperty("profiles")]
        public IList<Profile> Profiles { get; set; }

        [JsonProperty("groups")]
        public IList<GroupInfo> GroupInfos { get; set; }

        public string VkGroupFromId { get; set; }
        public VkGroupFrom VkGroupFrom { get; set; }

        public WallGet()
        {
            Items = new List<Item>();
            Profiles = new List<Profile>();
            GroupInfos = new List<GroupInfo>();
        }
    }
}
