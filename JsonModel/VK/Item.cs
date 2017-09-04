using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace VkGroupManager.JsonModel.VK
{
    public class Item
    {
        public string ItemId { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("from_id")]
        public int FromId { get; set; }

        [JsonProperty("owner_id")]
        public int OwnerId { get; set; }

        [JsonProperty("date")]
        public long Date { get; set; }

        [JsonProperty("marked_as_ads")]
        public int MarkedAsAds { get; set; }

        [JsonProperty("post_type")]
        public string PostType { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        public string BackUpText { get; set; }

        [JsonProperty("can_pin")]
        public int CanPin { get; set; }

        [JsonProperty("attachments")]
        public IList<Attachment> Attachments { get; set; }

        public string PostSourceId { get; set; }
        [JsonProperty("post_source")]
        public PostSource PostSource { get; set; }

        public string CommentsId { get; set; }
        [JsonProperty("comments")]
        public Comments Comments { get; set; }

        public string LikesId { get; set; }
        [JsonProperty("likes")]
        public Likes Likes { get; set; }

        public string RepostsId { get; set; }
        [JsonProperty("reposts")]
        public Reposts Reposts { get; set; }

        public string ViewsId { get; set; }
        [JsonProperty("views")]
        public Views Views { get; set; }

        public string Statuse { get; set; }
        public long PublishDate { get; set; }
        public long TimeKey { get; set; }
        public long? AddedTime { get; set; }

        public string WallGetId { get; set; }
        public WallGet WallGet { get; set; }

        public Item()
        {
            Attachments = new List<Attachment>();
        }
    }
}
