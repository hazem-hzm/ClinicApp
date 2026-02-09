using API.Data;
using API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController(AppDbContext context) : ControllerBase
{

    [Authorize] // any logged-in user
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<DoctorDto>>> GetDoctors()
    {
        var doctors = await context.Doctors
            .Select(d => new DoctorDto
            {
                Id = d.Id,
                DisplayName = d.User.DisplayName,
                Specialty = d.Specialty,
                YearsOfExperience = d.YearsOfExperience
            })
            .ToListAsync();

        return Ok(doctors);
    }


    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<DoctorDto>> GetDoctor(string id)
    {
        var doctor = await context.Doctors
            .Where(d => d.Id == id)
            .Select(d => new DoctorDto
            {
                Id = d.Id,
                DisplayName = d.User.DisplayName,
                Specialty = d.Specialty,
                YearsOfExperience = d.YearsOfExperience
            })
            .FirstOrDefaultAsync();

        if (doctor == null) return NotFound();

        return Ok(doctor);
    }


    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult> CreateDoctor(CreateDoctorDto dto)
    {
        // ensure user exists
        var userExists = await context.Users.AnyAsync(u => u.Id == dto.UserId);
        if (!userExists) return NotFound("User not found");

        // prevent duplicate doctor profile
        var alreadyDoctor = await context.Doctors.AnyAsync(d => d.Id == dto.UserId);
        if (alreadyDoctor) return BadRequest("Doctor profile already exists");

        var doctor = new Entities.Doctor
        {
            Id = dto.UserId,
            Specialty = dto.Specialty,
            YearsOfExperience = dto.YearsOfExperience
        };

        context.Doctors.Add(doctor);
        await context.SaveChangesAsync();

        return Ok("Doctor created");
    }
}
