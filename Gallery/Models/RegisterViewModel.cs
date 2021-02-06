using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gallery.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [StringLength(18, ErrorMessage = "The password must be at least 8 characters long.", MinimumLength = 8)]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match")]
        public string ConfirmPassword { get; set; }
    }
}
