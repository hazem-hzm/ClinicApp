namespace API.Entities;

public class MedicalRecord
{
    public int Id { get; set; }

    public string PatientId { get; set; } = null!;
    public string DoctorId { get; set; } = null!;

    public string Diagnosis { get; set; } = null!;
    public string? Treatment { get; set; }
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // navigation
    public Patient Patient { get; set; } = null!;
    public Doctor Doctor { get; set; } = null!;
}
