using Microsoft.EntityFrameworkCore;
using Backend.Entities.SecurityAccounts;

namespace Backend.Context.Data.Security
{
    public class SecurityContext : DbContext
    {       

        public SecurityContext(DbContextOptions<SecurityContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }

    }
}
