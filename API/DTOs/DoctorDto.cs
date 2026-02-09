namespace API.DTOs;

public class DoctorDto
{
    public string Id { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string Specialty { get; set; } = null!;
    public int YearsOfExperience { get; set; }
}
