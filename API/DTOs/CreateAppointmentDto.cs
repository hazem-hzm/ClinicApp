namespace API.DTOs;

public class CreateAppointmentDto
{
    public string DoctorId { get; set; } = null!;
    public DateTime Date { get; set; }
}
