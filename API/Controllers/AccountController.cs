using Microsoft.AspNetCore.Mvc;
using API.DTOs;
using API.Entities;
using API.Data;
using Microsoft.EntityFrameworkCore;
namespace API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly AppDbContext _context;
    public AccountController(AppDbContext context)
    {
        _context = context;
    }
    [HttpPost("register")]
    public async Task<ActionResult<AppUser>> Register(RegisterDto request)
    {
        if (await UserExists(request.Email)) return BadRequest("Email is taken");
        var user = new AppUser
        {
            DisplayName = request.DisplayName,
            Email = request.Email,
            PasswordHash = request.Password
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }   
    private async Task<bool> UserExists(string email)
    {
        return await _context.Users.AnyAsync(x => x.Email == email);
    }
}