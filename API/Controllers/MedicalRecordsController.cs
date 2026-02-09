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
public class MedicalRecordsController(AppDbContext context) : ControllerBase
{

    [Authorize(Roles = "DOCTOR")]
    [HttpPost]
    public async Task<ActionResult> CreateRecord(CreateMedicalRecordDto dto)
    {
        var doctorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // ensure patient exists
        var patientExists = await context.Patients.AnyAsync(p => p.Id == dto.PatientId);
        if (!patientExists) return NotFound("Patient not found");

        var record = new MedicalRecord
        {
            PatientId = dto.PatientId,
            DoctorId = doctorId!,
            Diagnosis = dto.Diagnosis,
            Treatment = dto.Treatment,
            CreatedAt = DateTime.UtcNow
        };

        context.MedicalRecords.Add(record);
        await context.SaveChangesAsync();

        return Ok("Medical record created");
    }


    [Authorize(Roles = "PATIENT")]
    [HttpGet("me")]
    public async Task<ActionResult<IReadOnlyList<MedicalRecordDto>>> GetMyRecords()
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


    [Authorize(Roles = "DOCTOR")]
    [HttpGet("doctor")]
    public async Task<ActionResult<IReadOnlyList<MedicalRecordDto>>> GetDoctorRecords()
    {
        var doctorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var records = await context.MedicalRecords
            .Where(r => r.DoctorId == doctorId)
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


    [Authorize(Roles = "ADMIN")]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MedicalRecordDto>>> GetAllRecords()
    {
        var records = await context.MedicalRecords
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
