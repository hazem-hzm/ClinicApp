using Microsoft.AspNetCore.Mvc;
using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class PatientsController(AppDbContext context) : BaseApiController
    {
        // GET: api/patients
        [HttpGet]
        public async Task<IActionResult> GetPatients()
        {
            var patients = await context.Patients.ToListAsync();
            return Ok(patients);
        }

        // GET: api/patients/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatient(int id)
        {
            var patient = await context.Patients.FindAsync(id);
            if (patient == null) return NotFound();
            return Ok(patient);
        }

        // POST: api/patients/{patientId}/appointments
        [HttpPost("{patientId}/appointments")]
        public async Task<IActionResult> CreateAppointment(string patientId, [FromBody] CreateAppointmentDto dto)
        {
            // Check if patient exists
            var patient = await context.Patients.FindAsync(patientId);
            if (patient == null) return NotFound($"Patient with id {patientId} not found.");

            // Check if doctor exists
            var doctor = await context.Doctors.FindAsync(dto.DoctorId);
            if (doctor == null) return NotFound($"Doctor with id {dto.DoctorId} not found.");

            var appointment = new Appointment
            {
                PatientId = patientId,
                DoctorId = dto.DoctorId,
                AppointmentDate = dto.AppointmentDate,
                Reason = dto.Reason,
                Notes = dto.Notes,
                Status = "Scheduled"
            };

            context.Appointments.Add(appointment);
            await context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetAppointmentById),
                new { appointmentId = appointment.Id },
                appointment
            );
        }

        // GET: api/patients/appointments/{appointmentId}
        [HttpGet("appointments/{appointmentId}")]
        public async Task<IActionResult> GetAppointmentById(string appointmentId)
        {
            var appointment = await context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.Id == appointmentId);

            if (appointment == null) return NotFound();
            return Ok(appointment);
        }

        public class CreateAppointmentDto
        {
            public string DoctorId { get; set; } = null!;
            public DateTime AppointmentDate { get; set; }
            public string? Reason { get; set; }
            public string? Notes { get; set; }
        }
    }
}