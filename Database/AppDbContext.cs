using DiscordApp.Database.Tables;
using Microsoft.EntityFrameworkCore;

namespace DiscordApp.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Passport> Passport { get; set; }
        public DbSet<Autobranches> Autobranches { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}