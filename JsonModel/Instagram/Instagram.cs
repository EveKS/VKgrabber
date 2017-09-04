using Newtonsoft.Json;
using System;
using System.Text;

namespace VkGroupManager.JsonModel.Instagram
{

#region skip
    public class P
    {

        [JsonProperty("use_new_styles")]
        public string UseNewStyles { get; set; }
    }

    public class Ebd
    {

        [JsonProperty("g")]
        public string G { get; set; }

        [JsonProperty("p")]
        public P P { get; set; }
    }

    public class Bc3l
    {

        [JsonProperty("g")]
        public string G { get; set; }

        [JsonProperty("p")]
        public P P { get; set; }
    }

    public class Ccp
    {

        [JsonProperty("g")]
        public string G { get; set; }

        [JsonProperty("p")]
        public P P { get; set; }
    }

    public class CreateUpsell
    {

        [JsonProperty("g")]
        public string G { get; set; }

        [JsonProperty("p")]
        public P P { get; set; }
    }

    public class Disc
    {

        [JsonProperty("g")]
        public string G { get; set; }

        [JsonProperty("p")]
        public P P { get; set; }
    }

    public class Feed
    {

        [JsonProperty("g")]
        public string G { get; set; }

        [JsonProperty("p")]
        public P P { get; set; }
    }

    public class SuUniverse
    {

        [JsonProperty("g")]
        public string G { get; set; }

        [JsonProperty("p")]
        public P P { get; set; }
    }

    public class Us
    {

        [JsonProperty("g")]
        public string G { get; set; }

        [JsonProperty("p")]
        public P P { get; set; }
    }

    public class UsLi
    {

        [JsonProperty("g")]
        public string G { get; set; }

        [JsonProperty("p")]
        public P P { get; set; }
    }

    public class Nav
    {

        [JsonProperty("g")]
        public string G { get; set; }

        [JsonProperty("p")]
        public P P { get; set; }
    }

    public class NavLo
    {

        [JsonProperty("g")]
        public string G { get; set; }

        [JsonProperty("p")]
        public P P { get; set; }
    }

    public class Profile
    {

        [JsonProperty("g")]
        public string G { get; set; }

        [JsonProperty("p")]
        public P P { get; set; }
    }

    public class Deact
    {

        [JsonProperty("g")]
        public string G { get; set; }

        [JsonProperty("p")]
        public P P { get; set; }
    }

    public class Sidecar
    {

        [JsonProperty("g")]
        public string G { get; set; }

        [JsonProperty("p")]
        public P P { get; set; }
    }

    public class Video
    {

        [JsonProperty("g")]
        public string G { get; set; }

        [JsonProperty("p")]
        public P P { get; set; }
    }

    public class Filters
    {

        [JsonProperty("g")]
        public string G { get; set; }

        [JsonProperty("p")]
        public P P { get; set; }
    }

    public class Typeahead
    {

        [JsonProperty("g")]
        public string G { get; set; }

        [JsonProperty("p")]
        public P P { get; set; }
    }

    public class LocationTag
    {

        [JsonProperty("g")]
        public string G { get; set; }

        [JsonProperty("p")]
        public P P { get; set; }
    }

    public class PwLink
    {

        [JsonProperty("g")]
        public string G { get; set; }

        [JsonProperty("p")]
        public P P { get; set; }
    }

    public class DeltaDefaults
    {

        [JsonProperty("g")]
        public string G { get; set; }

        [JsonProperty("p")]
        public P P { get; set; }
    }

    public class Appsell
    {

        [JsonProperty("g")]
        public string G { get; set; }

        [JsonProperty("p")]
        public P P { get; set; }
    }

    public class ProfileSensitivity
    {

        [JsonProperty("g")]
        public string G { get; set; }

        [JsonProperty("p")]
        public P P { get; set; }
    }

    public class Save
    {

        [JsonProperty("g")]
        public string G { get; set; }

        [JsonProperty("p")]
        public P P { get; set; }
    }

    public class Stale
    {

        [JsonProperty("g")]
        public string G { get; set; }

        [JsonProperty("p")]
        public P P { get; set; }
    }

    public class Reg
    {

        [JsonProperty("g")]
        public string G { get; set; }

        [JsonProperty("p")]
        public P P { get; set; }
    }

    public class Qe
    {

        [JsonProperty("ebd")]
        public Ebd Ebd { get; set; }

        [JsonProperty("bc3l")]
        public Bc3l Bc3l { get; set; }

        [JsonProperty("ccp")]
        public Ccp Ccp { get; set; }

        [JsonProperty("create_upsell")]
        public CreateUpsell CreateUpsell { get; set; }

        [JsonProperty("disc")]
        public Disc Disc { get; set; }

        [JsonProperty("feed")]
        public Feed Feed { get; set; }

        [JsonProperty("su_universe")]
        public SuUniverse SuUniverse { get; set; }

        [JsonProperty("us")]
        public Us Us { get; set; }

        [JsonProperty("us_li")]
        public UsLi UsLi { get; set; }

        [JsonProperty("nav")]
        public Nav Nav { get; set; }

        [JsonProperty("nav_lo")]
        public NavLo NavLo { get; set; }

        [JsonProperty("profile")]
        public Profile Profile { get; set; }

        [JsonProperty("deact")]
        public Deact Deact { get; set; }

        [JsonProperty("sidecar")]
        public Sidecar Sidecar { get; set; }

        [JsonProperty("video")]
        public Video Video { get; set; }

        [JsonProperty("filters")]
        public Filters Filters { get; set; }

        [JsonProperty("typeahead")]
        public Typeahead Typeahead { get; set; }

        [JsonProperty("location_tag")]
        public LocationTag LocationTag { get; set; }

        [JsonProperty("pw_link")]
        public PwLink PwLink { get; set; }

        [JsonProperty("delta_defaults")]
        public DeltaDefaults DeltaDefaults { get; set; }

        [JsonProperty("appsell")]
        public Appsell Appsell { get; set; }

        [JsonProperty("profile_sensitivity")]
        public ProfileSensitivity ProfileSensitivity { get; set; }

        [JsonProperty("save")]
        public Save Save { get; set; }

        [JsonProperty("stale")]
        public Stale Stale { get; set; }

        [JsonProperty("reg")]
        public Reg Reg { get; set; }
    }
#endregion

    public class Instagram
    {

        [JsonProperty("activity_counts")]
        public object ActivityCounts { get; set; }

        [JsonProperty("config")]
        public Config Config { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        [JsonProperty("language_code")]
        public string LanguageCode { get; set; }

        [JsonProperty("entry_data")]
        public EntryData EntryData { get; set; }

        [JsonProperty("gatekeepers")]
        public Gatekeepers Gatekeepers { get; set; }

        [JsonProperty("qe")]
        public Qe Qe { get; set; }

        [JsonProperty("hostname")]
        public string Hostname { get; set; }

        [JsonProperty("display_properties_server_guess")]
        public DisplayPropertiesServerGuess DisplayPropertiesServerGuess { get; set; }

        [JsonProperty("environment_switcher_visible_server_guess")]
        public bool EnvironmentSwitcherVisibleServerGuess { get; set; }

        [JsonProperty("platform")]
        public string Platform { get; set; }

        [JsonProperty("is_canary")]
        public bool IsCanary { get; set; }

        [JsonProperty("probably_has_app")]
        public bool ProbablyHasApp { get; set; }

        [JsonProperty("show_app_install")]
        public bool ShowAppInstall { get; set; }
    }
}
