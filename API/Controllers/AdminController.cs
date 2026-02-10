using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.Entities;
using API.Data;
using Microsoft.EntityFrameworkCore;
using API.DTOs;
using API.Extensions;
using API.Interfaces;
using System.Linq;
using System.Data;
using System.Security.Cryptography;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "ADMIN")]
public class AdminController(AppDbContext context, ITokenService tokenService) : ControllerBase
{
    private readonly AppDbContext _context = context;
    private readonly ITokenService _tokenService = tokenService;

    [HttpGet("users")]
    public async Task<ActionResult<IReadOnlyList<UserDto>>> GetUsers()
    {
        var users = await _context.Users.ToListAsync();

        var userDtos = new List<UserDto>(users.Count);
        foreach (var user in users)
        {
            userDtos.Add(await user.ToDto(_tokenService));
        }

        return Ok(userDtos);
    }
}