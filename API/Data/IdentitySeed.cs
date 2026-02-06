using Microsoft.AspNetCore.Identity;

namespace API.Data;

public static class IdentitySeed
{
    public static async Task SeedRolesAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        if (!await roleManager.RoleExistsAsync("PATIENT"))
        {
            await roleManager.CreateAsync(new IdentityRole("PATIENT"));
        }
    }
}
