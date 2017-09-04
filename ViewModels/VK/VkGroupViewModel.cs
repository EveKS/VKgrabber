using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using VkGroupManager.Models;
using VkGroupManager.Models.VK;

namespace VkGroupManager.ViewModels.VK
{
    public class VkGroupViewModel
    {
        public string VkUserId { get; set; }
        public string VkGroupId { get; set; }

        public string GroupId { get; set; }
        public string Domain { get; set; }
        public string ExpiresIn { get; set; }

        public string GroupName { get; set; }

        [MaxLength(450)]
        [Display(Name = "client_id")]
        public string ClientId { get; set; }

        [MaxLength(450)]
        [Display(Name = "redirect_uri")]
        public string RedirectUri { get; set; }

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

        public string AccessToken { get; set; }

        public string Photo { get; set; }
        public string AddedGroup { get; set; }
        public List<VkGroupFrom> VkGroupsFrom { get; set; }
    }
}
