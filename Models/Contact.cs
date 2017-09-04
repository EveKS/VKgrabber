using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VkGroupManager.Models
{
    public class Contact
    {
        public int ContactId { get; set; }

        public string OwnerID { get; set; }

        [DataType(DataType.EmailAddress)]
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
