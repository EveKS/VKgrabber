using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VkGroupManager.ViewModels.VK
{
    public class VkClientViewModel
    {
        public string VkClientId { get; set; }

        [MaxLength(450)]
        [Display(Name = "client_id")]
        public string ClientId { get; set; }

        [MaxLength(450)]
        [Display(Name = "client_secret")]
        public string ClientSecret { get; set; }

        [MaxLength(450)]
        [Display(Name = "redirect_uri")]
        public string RedirectUri { get; set; }

        [MaxLength(450)]
        [Display(Name = "server_url")]
        public string ServerUrl { get; set; }

        [MaxLength(450)]
        [Display(Name = "scope")]
        public string Scope { get; set; }

        [MaxLength(50)]
        [Display(Name = "display")]
        public string Display { get; set; }

        [MaxLength(50)]
        [Display(Name = "response_type")]
        public string ResponseType { get; set; }

        [MaxLength(450)]
        [Display(Name = "state")]
        public string State { get; set; }

        [MaxLength(450)]
        [Display(Name = "revoke")]
        public string Revoke { get; set; }

        [MaxLength(50)]
        [Display(Name = "v")]
        public string Version { get; set; }

        public string AccessToken { get; set; }

        public string UserId { get; set; }
    }
}
