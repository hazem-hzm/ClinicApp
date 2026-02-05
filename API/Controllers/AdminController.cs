using Microsoft.AspNetCore.Mvc;
using API.Entities;
using API.Data;
using Microsoft.EntityFrameworkCore;
using API.DTOs;
using API.Extensions;
using API.Interfaces;
using System.Linq;
namespace API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AdminController(AppDbContext context, ITokenService tokenService) : ControllerBase
{
    private readonly AppDbContext _context = context;
    private readonly ITokenService _tokenService = tokenService;

    [HttpGet("users")]
    public async Task<ActionResult<IReadOnlyList<UserDto>>> GetUsers()
    {
        var users = await _context.Users.ToListAsync();
        return Ok(users.Select(u => u.ToDto(_tokenService)).ToList());
    }
}