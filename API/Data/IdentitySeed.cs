using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public static class IdentitySeed
{
    public static async Task SeedRolesAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        string[] roles = { "PATIENT", "DOCTOR", "ADMIN" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
    public static async Task SeedDoctorAsync(IServiceProvider services)
{
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var context = services.GetRequiredService<AppDbContext>();

    const string doctorEmail = "doctor@clinic.com";
    const string doctorPassword = "Doctor123!";

    // Check if user already exists
    var user = await userManager.FindByEmailAsync(doctorEmail);
    if (user != null) return;

    user = new AppUser
    {
        UserName = doctorEmail,
        Email = doctorEmail,
        DisplayName = "Dr. John Smith"
    };

    var result = await userManager.CreateAsync(user, doctorPassword);
    if (!result.Succeeded)
    {
        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        throw new Exception($"Failed to create doctor user: {errors}");
    }

    // Assign DOCTOR role
    await userManager.AddToRoleAsync(user, "DOCTOR");

    // Prevent duplicate Doctor record
    var doctorExists = context.Doctors.Any(d => d.Id == user.Id);
    if (doctorExists) return;

    user.PhoneNumber = "123456789";
    await userManager.UpdateAsync(user);

    var doctor = new Doctor
    {
        Id = user.Id,
        Specialty = "Cardiology",
        YearsOfExperience = 10
    };

    context.Doctors.Add(doctor);
    await context.SaveChangesAsync();
}


    
}
