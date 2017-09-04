using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VkGroupManager.ViewModels.User
{
    public class ChangePasswordViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "Новый email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string NewPassword { get; set; }
    }
}
