using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController(AppDbContext context) : ControllerBase
{

    [Authorize(Roles = "PATIENT")]
    [HttpPost]
    public async Task<ActionResult> CreateAppointment(CreateAppointmentDto dto)
    {
        var patientId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // check doctor exists
        var doctorExists = await context.Doctors.AnyAsync(d => d.Id == dto.DoctorId);
        if (!doctorExists) return NotFound("Doctor not found");

        // prevent double booking
        var isBooked = await context.Appointments.AnyAsync(a =>
            a.DoctorId == dto.DoctorId &&
            a.Date == dto.Date &&
            a.Status != "Cancelled");

        if (isBooked) return BadRequest("This time slot is already booked");

        var appointment = new Appointment
        {
            PatientId = patientId!,
            DoctorId = dto.DoctorId,
            Date = dto.Date,
            Status = "Pending"
        };

        context.Appointments.Add(appointment);
        await context.SaveChangesAsync();

        return Ok("Appointment created successfully");
    }


    [Authorize(Roles = "DOCTOR")]
    [HttpGet("doctor")]
    public async Task<ActionResult<IReadOnlyList<AppointmentDto>>> GetDoctorAppointments()
    {
        var doctorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var appointments = await context.Appointments
            .Where(a => a.DoctorId == doctorId)
            .Select(a => new AppointmentDto
            {
                Id = a.Id,
                Date = a.Date,
                Status = a.Status,
                PatientName = a.Patient.User.DisplayName,
                DoctorName = a.Doctor.User.DisplayName
            })
            .ToListAsync();

        return Ok(appointments);
    }


    [Authorize(Roles = "ADMIN")]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AppointmentDto>>> GetAllAppointments()
    {
        var appointments = await context.Appointments
            .Select(a => new AppointmentDto
            {
                Id = a.Id,
                Date = a.Date,
                Status = a.Status,
                PatientName = a.Patient.User.DisplayName,
                DoctorName = a.Doctor.User.DisplayName
            })
            .ToListAsync();

        return Ok(appointments);
    }


    [Authorize(Roles = "DOCTOR,ADMIN")]
    [HttpPatch("{id}/status")]
    public async Task<ActionResult> UpdateStatus(int id, [FromQuery] string status)
    {
        var appointment = await context.Appointments.FindAsync(id);
        if (appointment == null) return NotFound();

        appointment.Status = status;
        await context.SaveChangesAsync();

        return Ok("Status updated");
    }


    [Authorize(Roles = "PATIENT")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> CancelAppointment(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var appointment = await context.Appointments
            .FirstOrDefaultAsync(a => a.Id == id && a.PatientId == userId);

        if (appointment == null) return NotFound();

        appointment.Status = "Cancelled";
        await context.SaveChangesAsync();

        return Ok("Appointment cancelled");
    }
}
