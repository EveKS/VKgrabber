using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VkGroupManager.JsonModel.InstagramNext
{
    public class PageInfo
    {

        [JsonProperty("has_next_page")]
        public bool HasNextPage { get; set; }

        [JsonProperty("end_cursor")]
        public string EndCursor { get; set; }
    }

    public class Edge
    {

        [JsonProperty("node")]
        public Node Node { get; set; }
    }

    public class EdgeMediaToCaption
    {

        [JsonProperty("edges")]
        public IList<Edge> Edges { get; set; }
    }

    public class EdgeMediaToComment
    {

        [JsonProperty("count")]
        public int Count { get; set; }
    }

    public class Dimensions
    {

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }
    }

    public class EdgeMediaPreviewLike
    {

        [JsonProperty("count")]
        public int Count { get; set; }
    }

    public class Owner
    {

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class Node
    {

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("__typename")]
        public string Typename { get; set; }

        [JsonProperty("edge_media_to_caption")]
        public EdgeMediaToCaption EdgeMediaToCaption { get; set; }

        [JsonProperty("shortcode")]
        public string Shortcode { get; set; }

        [JsonProperty("edge_media_to_comment")]
        public EdgeMediaToComment EdgeMediaToComment { get; set; }

        [JsonProperty("comments_disabled")]
        public bool CommentsDisabled { get; set; }

        [JsonProperty("taken_at_timestamp")]
        public int TakenAtTimestamp { get; set; }

        [JsonProperty("dimensions")]
        public Dimensions Dimensions { get; set; }

        [JsonProperty("display_url")]
        public string DisplayUrl { get; set; }

        [JsonProperty("edge_media_preview_like")]
        public EdgeMediaPreviewLike EdgeMediaPreviewLike { get; set; }

        [JsonProperty("owner")]
        public Owner Owner { get; set; }

        [JsonProperty("thumbnail_src")]
        public string ThumbnailSrc { get; set; }

        [JsonProperty("thumbnail_resources")]
        public IList<object> ThumbnailResources { get; set; }

        [JsonProperty("is_video")]
        public bool IsVideo { get; set; }

        [JsonProperty("video_view_count")]
        public int? VideoViewCount { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class EdgeOwnerToTimelineMedia
    {

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("page_info")]
        public PageInfo PageInfo { get; set; }

        [JsonProperty("edges")]
        public IList<Edge> Edges { get; set; }
    }

    public class User
    {

        [JsonProperty("edge_owner_to_timeline_media")]
        public EdgeOwnerToTimelineMedia EdgeOwnerToTimelineMedia { get; set; }
    }

    public class Data
    {

        [JsonProperty("user")]
        public User User { get; set; }
    }

    public class InstagramNext
    {

        [JsonProperty("data")]
        public Data Data { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }        
    }

}
