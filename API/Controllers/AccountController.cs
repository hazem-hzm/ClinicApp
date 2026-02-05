using Microsoft.AspNetCore.Mvc;
using API.DTOs;
using API.Entities;
using API.Data;
using Microsoft.EntityFrameworkCore;
using API.Extensions;
using API.Interfaces;
using System;
using System.Security.Cryptography;
using System.Text;
namespace API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AccountController(AppDbContext context, ITokenService tokenService) : ControllerBase
{
    private readonly AppDbContext _context = context;
    private readonly ITokenService _tokenService = tokenService;

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto request)
    {
        if (await UserExists(request.Email)) return BadRequest("Email is taken");
        using var hmac = new HMACSHA512();
        var user = new AppUser
        {
            DisplayName = request.DisplayName,
            Email = request.Email,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password)),
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user.ToDto(_tokenService);
    }   
    private async Task<bool> UserExists(string email)
    {
        return await _context.Users.AnyAsync(x => x.Email == email);
    }
}