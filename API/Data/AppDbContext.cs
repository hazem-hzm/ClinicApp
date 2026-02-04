using Microsoft.EntityFrameworkCore;
using API.Entities;
namespace API.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<AppUser> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<AppUser>().HasKey(u => u.Id);
        modelBuilder.Entity<AppUser>().Property(u => u.DisplayName).IsRequired().HasMaxLength(50);
        modelBuilder.Entity<AppUser>().Property(u => u.Email).IsRequired().HasMaxLength(100);
        modelBuilder.Entity<AppUser>().Property(u => u.PasswordHash).IsRequired().HasMaxLength(100);
        modelBuilder.Entity<AppUser>().HasIndex(u => u.Email).IsUnique();
    }
}