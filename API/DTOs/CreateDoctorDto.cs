namespace API.DTOs;

public class CreateDoctorDto
{
    public string UserId { get; set; } = null!;
    public string Specialty { get; set; } = null!;
    public int YearsOfExperience { get; set; }
}
