using Newtonsoft.Json;
using System.Collections.Generic;

namespace VkGroupManager.JsonModel.VK
{
    public class Profile
    {
        public string ProfileId { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("sex")]
        public int Sex { get; set; }

        [JsonProperty("bdate")]
        public string Bdate { get; set; }

        [JsonProperty("city")]
        public City City { get; set; }

        [JsonProperty("country")]
        public Country Country { get; set; }

        [JsonProperty("photo_50")]
        public string Photo50 { get; set; }

        [JsonProperty("photo_100")]
        public string Photo100 { get; set; }

        [JsonProperty("photo_200")]
        public string Photo200 { get; set; }

        [JsonProperty("photo_max")]
        public string PhotoMax { get; set; }

        [JsonProperty("photo_200_orig")]
        public string Photo200Orig { get; set; }

        [JsonProperty("photo_id")]
        public string PhotoId { get; set; }

        [JsonProperty("has_photo")]
        public int HasPhoto { get; set; }

        [JsonProperty("verified")]
        public int Verified { get; set; }

        [JsonProperty("home_town")]
        public string HomeTown { get; set; }

        public string WallGetId { get; set; }
        public WallGet WallGet { get; set; }
    }
}
