using Newtonsoft.Json;

namespace VkGroupManager.JsonModel.Instagram
{
    public class User
    {

        [JsonProperty("biography")]
        public string Biography { get; set; }

        [JsonProperty("blocked_by_viewer")]
        public bool BlockedByViewer { get; set; }

        [JsonProperty("country_block")]
        public bool CountryBlock { get; set; }

        [JsonProperty("external_url")]
        public string ExternalUrl { get; set; }

        [JsonProperty("external_url_linkshimmed")]
        public string ExternalUrlLinkshimmed { get; set; }

        [JsonProperty("followed_by")]
        public FollowedBy FollowedBy { get; set; }

        [JsonProperty("followed_by_viewer")]
        public bool FollowedByViewer { get; set; }

        [JsonProperty("follows")]
        public Follows Follows { get; set; }

        [JsonProperty("follows_viewer")]
        public bool FollowsViewer { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("has_blocked_viewer")]
        public bool HasBlockedViewer { get; set; }

        [JsonProperty("has_requested_viewer")]
        public bool HasRequestedViewer { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("is_private")]
        public bool IsPrivate { get; set; }

        [JsonProperty("is_verified")]
        public bool IsVerified { get; set; }

        [JsonProperty("profile_pic_url")]
        public string ProfilePicUrl { get; set; }

        [JsonProperty("profile_pic_url_hd")]
        public string ProfilePicUrlHd { get; set; }

        [JsonProperty("requested_by_viewer")]
        public bool RequestedByViewer { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("connected_fb_page")]
        public object ConnectedFbPage { get; set; }

        [JsonProperty("media")]
        public Media Media { get; set; }

        [JsonProperty("edge_owner_to_timeline_media")]
        public EdgeOwnerToTimelineMedia EdgeOwnerToTimelineMedia { get; set; }
    }
}
