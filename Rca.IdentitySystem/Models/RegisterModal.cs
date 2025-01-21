﻿using System.ComponentModel.DataAnnotations;

namespace Rca.IdentitySystem.Models
{
    public class RegisterModal
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required]
        [DataType(dataType: DataType.Password)]
        public string Password { get; set; }
        
        [Required]
        public string Department { get; set; }

        [Required]
        public string Position { get; set; }


    }
}
