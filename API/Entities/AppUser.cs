using Microsoft.AspNetCore.Identity;

namespace API.Entities;

public class AppUser : IdentityUser
{
    public required string DisplayName { get; set; }

    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }

    // optional roles in domain
    public Patient? Patient { get; set; }
    public Doctor? Doctor { get; set; }
}
