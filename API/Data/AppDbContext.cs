using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using API.Entities;
namespace API.Data;
public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser>(options)
{
    public DbSet<Patient> Patients { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<AppUser>().HasKey(u => u.Id);
        modelBuilder.Entity<AppUser>().Property(u => u.DisplayName).IsRequired().HasMaxLength(50);
        modelBuilder.Entity<AppUser>().Property(u => u.Email).IsRequired().HasMaxLength(100);
        modelBuilder.Entity<AppUser>().Property(u => u.PasswordHash).IsRequired().HasMaxLength(100);
        modelBuilder.Entity<AppUser>().HasIndex(u => u.Email).IsUnique();

        modelBuilder.Entity<Patient>().HasKey(p => p.Id);
        modelBuilder.Entity<Patient>().Property(p => p.FullName).IsRequired().HasMaxLength(100);
        modelBuilder.Entity<Patient>().Property(p => p.PhoneNumber).HasMaxLength(20);
        modelBuilder.Entity<Patient>().Property(p => p.Address).HasMaxLength(200);


    }
}