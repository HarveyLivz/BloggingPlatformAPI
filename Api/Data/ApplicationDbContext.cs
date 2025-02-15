using Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .HasOne(p => p.Author)
                .WithOne(u => u.Post)
                .HasForeignKey<Post>(p => p.AuthorId)
                .OnDelete(DeleteBehavior.Cascade); // Adjust delete behavior as needed
        }
    }
}
