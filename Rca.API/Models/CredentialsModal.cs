using System.ComponentModel.DataAnnotations;

namespace Rca.API.Models
{
    public class CredentialsModal
    {
        [Required]
        [Display(Description = "User Name")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
