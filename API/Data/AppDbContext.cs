using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<MedicalRecord> MedicalRecords => Set<MedicalRecord>();
    

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // 🔹 AppUser ↔ Patient (1-1 shared PK)
        builder.Entity<Patient>()
            .HasOne(p => p.User)
            .WithOne(u => u.Patient)
            .HasForeignKey<Patient>(p => p.Id)
            .OnDelete(DeleteBehavior.Cascade);

        // 🔹 AppUser ↔ Doctor (1-1 shared PK)
        builder.Entity<Doctor>()
            .HasOne(d => d.User)
            .WithOne(u => u.Doctor)
            .HasForeignKey<Doctor>(d => d.Id)
            .OnDelete(DeleteBehavior.Cascade);

        // 🔹 Appointment ↔ Patient (many-to-1)
        builder.Entity<Appointment>()
            .HasOne(a => a.Patient)
            .WithMany(p => p.Appointments)
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        // 🔹 Appointment ↔ Doctor (many-to-1)
        builder.Entity<Appointment>()
            .HasOne(a => a.Doctor)
            .WithMany(d => d.Appointments)
            .HasForeignKey(a => a.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        // 🔹 MedicalRecord ↔ Patient (many-to-1)
        builder.Entity<MedicalRecord>()
            .HasOne(m => m.Patient)
            .WithMany(p => p.MedicalRecords)
            .HasForeignKey(m => m.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        // 🔹 MedicalRecord ↔ Doctor (many-to-1)
        builder.Entity<MedicalRecord>()
            .HasOne(m => m.Doctor)
            .WithMany(d => d.MedicalRecords)
            .HasForeignKey(m => m.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);



    }
}
