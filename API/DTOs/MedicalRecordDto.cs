namespace API.DTOs;
public class MedicalRecordDto
{
    public int Id { get; set; }
    public string Diagnosis { get; set; } = null!;
    public string? Treatment { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public string DoctorName { get; set; } = null!;
}
