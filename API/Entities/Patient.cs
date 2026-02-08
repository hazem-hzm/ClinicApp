namespace API.Entities;

public class Patient
{
    public string Id { get; set; } = null!;   // same as AppUser.Id

    public DateOnly DateOfBirth { get; set; }
    public string Gender { get; set; } = null!;
    public string? MedicalNotes { get; set; }

    // navigation
    public AppUser User { get; set; } = null!;
    public ICollection<Appointment> Appointments { get; set; } = [];
    public ICollection<MedicalRecord> MedicalRecords { get; set; } = [];

}
