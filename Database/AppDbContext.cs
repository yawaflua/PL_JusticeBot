using DiscordApp.Database.Tables;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DiscordApp.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Passport> Passport { get; set; }
        public DbSet<Autobranches> Autobranches { get; set; }
        public DbSet<ArtsPatents> ArtPatents { get; set; }
        public DbSet<BooksPatents> BookPatents { get; set; }
        public DbSet<Bizness> Bizness { get; set; }
        public DbSet<Reports> Reports { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Redirects> Redirects { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}