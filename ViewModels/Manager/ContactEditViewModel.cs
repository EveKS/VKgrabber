using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VkGroupManager.ViewModels.Manager
{
    public class ContactEditViewModel
    {
        public int ContactId { get; set; }

        [Required(ErrorMessage = "Имя отсутствует")]

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
