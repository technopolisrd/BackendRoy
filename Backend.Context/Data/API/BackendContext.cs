using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Backend.Entities;

namespace Backend.Context.Data.API
{
    public class BackendContext : DbContext
    {
        public BackendContext(DbContextOptions<BackendContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
