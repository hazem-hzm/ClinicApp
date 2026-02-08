namespace API.DTOs;
public class PatientDto
{
    public string Id { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public DateOnly DateOfBirth { get; set; }
}
