namespace API.DTOs;
public class AppointmentDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; } = null!;

    public string PatientName { get; set; } = null!;
    public string DoctorName { get; set; } = null!;
}
