using System.Security.Claims;
using API.Data;
using API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController(AppDbContext context) : ControllerBase
{

    [Authorize(Roles = "Admin,Doctor")]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PatientDto>>> GetPatients()
    {
        var patients = await context.Patients
            .Select(p => new PatientDto
            {
                Id = p.Id,
                DisplayName = p.User.DisplayName,
                DateOfBirth = p.DateOfBirth
            })
            .ToListAsync();

        return Ok(patients);
    }


    [Authorize(Roles = "Admin,Doctor")]
    [HttpGet("{id}")]
    public async Task<ActionResult<PatientDto>> GetPatient(string id)
    {
        var patient = await context.Patients
            .Where(p => p.Id == id)
            .Select(p => new PatientDto
            {
                Id = p.Id,
                DisplayName = p.User.DisplayName,
                DateOfBirth = p.DateOfBirth
            })
            .FirstOrDefaultAsync();

        if (patient == null) return NotFound();

        return Ok(patient);
    }


    [Authorize(Roles = "Patient")]
    [HttpGet("me")]
    public async Task<ActionResult<PatientDto>> GetMe()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var patient = await context.Patients
            .Where(p => p.Id == userId)
            .Select(p => new PatientDto
            {
                Id = p.Id,
                DisplayName = p.User.DisplayName,
                DateOfBirth = p.DateOfBirth
            })
            .FirstOrDefaultAsync();

        if (patient == null) return NotFound();

        return Ok(patient);
    }


    [Authorize(Roles = "Patient")]
    [HttpGet("me/appointments")]
    public async Task<ActionResult<IReadOnlyList<AppointmentDto>>> GetMyAppointments()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var appointments = await context.Appointments
            .Where(a => a.PatientId == userId)
            .Select(a => new AppointmentDto
            {
                Id = a.Id,
                DoctorName = a.Doctor.User.DisplayName,
                Date = a.Date,
                Status = a.Status
            })
            .ToListAsync();

        return Ok(appointments);
    }


    [Authorize(Roles = "Patient")]
    [HttpGet("me/medical-records")]
    public async Task<ActionResult<IReadOnlyList<MedicalRecordDto>>> GetMyMedicalRecords()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var records = await context.MedicalRecords
            .Where(r => r.PatientId == userId)
            .Select(r => new MedicalRecordDto
            {
                Id = r.Id,
                DoctorName = r.Doctor.User.DisplayName,
                Diagnosis = r.Diagnosis,
                Treatment = r.Treatment,
                CreatedAt = r.CreatedAt
            })
            .ToListAsync();

        return Ok(records);
    }
}
