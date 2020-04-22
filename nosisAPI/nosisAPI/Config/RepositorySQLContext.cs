using Microsoft.EntityFrameworkCore;
using nosisAPI.Models;

namespace nosisAPI.Config
{
    public class RepositorySQLContext : DbContext
    {
        public RepositorySQLContext(DbContextOptions options) : base(options)
        {}

        public DbSet<Clientes> Clientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("LOYALTY");
        }
    }
}
