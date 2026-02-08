namespace API.Entities;

public class Doctor
{
    public string Id { get; set; } = null!;   // same as AppUser.Id

    public string Specialty { get; set; } = null!;
    public int YearsOfExperience { get; set; }

    // navigation
    public AppUser User { get; set; } = null!;
    public ICollection<Appointment> Appointments { get; set; } = [];
    public ICollection<MedicalRecord> MedicalRecords { get; set; } = [];

}
