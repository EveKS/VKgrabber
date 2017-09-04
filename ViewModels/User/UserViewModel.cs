using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VkGroupManager.ViewModels.User
{
    public class UserViewModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public bool? EmailConfirmed { get; set; }
        public DateTime? RegesterDate { get; set; }

        public ContactStatus Status { get; set; }
    }

    public enum ContactStatus
    {
        Submitted,
        Approved,
        Rejected
    }
}
