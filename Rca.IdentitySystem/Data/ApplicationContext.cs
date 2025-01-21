using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rca.IdentitySystem.Models;

namespace Rca.IdentitySystem.Data
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            :base(options)
        {
            
        }

    }
}
