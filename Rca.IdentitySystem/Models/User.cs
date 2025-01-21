using Microsoft.AspNetCore.Identity;

namespace Rca.IdentitySystem.Models
{
    public class User: IdentityUser
    {
        public string Department { get; set; }
        public string Position { get; set; }
    }
}
