using Microsoft.AspNetCore.Mvc;
using API.Entities;
using API.Data;
using Microsoft.EntityFrameworkCore;
namespace API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly AppDbContext _context;
    public AdminController(AppDbContext context)
    {
        _context = context;
    }
    [HttpGet("users")]
    public async Task<ActionResult<IReadOnlyList<AppUser>>> GetUsers()
    {
        return Ok(await _context.Users.ToListAsync<AppUser>());
    }
}