using Newtonsoft.Json;
using System.Collections.Generic;

namespace VkGroupManager.JsonModel.Instagram
{
    public class Node
    {

        [JsonProperty("__typename")]
        public string Typename { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("comments_disabled")]
        public bool CommentsDisabled { get; set; }

        [JsonProperty("dimensions")]
        public Dimensions Dimensions { get; set; }

        [JsonProperty("gating_info")]
        public object GatingInfo { get; set; }

        [JsonProperty("media_preview")]
        public string MediaPreview { get; set; }

        [JsonProperty("owner")]
        public Owner Owner { get; set; }

        [JsonProperty("thumbnail_src")]
        public string ThumbnailSrc { get; set; }

        [JsonProperty("thumbnail_resources")]
        public IList<object> ThumbnailResources { get; set; }

        [JsonProperty("is_video")]
        public bool IsVideo { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("date")]
        public int Date { get; set; }

        [JsonProperty("display_src")]
        public string DisplaySrc { get; set; }

        [JsonProperty("caption")]
        public string Caption { get; set; }

        [JsonProperty("comments")]
        public Comments Comments { get; set; }

        [JsonProperty("likes")]
        public Likes Likes { get; set; }

        [JsonProperty("video_views")]
        public int? VideoViews { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("edge_media_to_caption")]
        public EdgeMediaToCaption EdgeMediaToCaption { get; set; }

        [JsonProperty("shortcode")]
        public string Shortcode { get; set; }

        [JsonProperty("edge_media_to_comment")]
        public EdgeMediaToComment EdgeMediaToComment { get; set; }

        [JsonProperty("taken_at_timestamp")]
        public int TakenAtTimestamp { get; set; }

        [JsonProperty("display_url")]
        public string DisplayUrl { get; set; }

        [JsonProperty("edge_media_preview_like")]
        public EdgeMediaPreviewLike EdgeMediaPreviewLike { get; set; }
    }
}
