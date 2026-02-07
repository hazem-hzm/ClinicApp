namespace API.Entities;

public class Appointment
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string PatientId { get; set; } = null!;
    public Patient Patient { get; set; } = null!;

    public string DoctorId { get; set; } = null!;
    public Doctor Doctor { get; set; } = null!;

    public DateTime AppointmentDate { get; set; }
    public string? Reason { get; set; }
    public string? Notes { get; set; }
    public string Status { get; set; } = "Scheduled";
}