using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Backend.Entities;
using Backend.Entities.Tables;

namespace Backend.Context.Data.API
{
    public class BackendContext : DbContext
    {
        public BackendContext(DbContextOptions<BackendContext> options) : base(options)
        {
        }
        
        #region TABLES

        public DbSet<Customer> customers { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Global Filters

            modelBuilder.Entity<Customer>().HasQueryFilter(x => !x.Deferred);

            #endregion

            #region Default Values

            modelBuilder.Entity<Customer>().Property(x => x.Deferred).HasDefaultValue(false);

            #endregion


        }
    }
}
