namespace API.DTOs;

public class CreateMedicalRecordDto
{
    public string PatientId { get; set; } = null!;
    public string Diagnosis { get; set; } = null!;
    public string Treatment { get; set; } = null!;
}
