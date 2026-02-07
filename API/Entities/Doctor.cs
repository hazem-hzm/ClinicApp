namespace API.Entities;

public class Doctor
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string UserId { get; set; } = null!;
    public required string FullName { get; set; }
    public string? Specialization { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public AppUser User { get; set; } = null!;
    public string Gender { get; set; } = string.Empty;
}