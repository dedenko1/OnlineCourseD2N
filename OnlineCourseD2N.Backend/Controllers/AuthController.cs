using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineCourseD2N.Backend.Data;
using OnlineCourseD2N.Shared.Models;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AuthController(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginDTO request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
        {
            return BadRequest(new AuthResponse { Success = false, Message = "Email atau Password salah." });
        }

        string token = CreateToken(user);

        return Ok(new AuthResponse { Success = true, Token = token });
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterDTO request)
    {
        // 1. Cek apakah email sudah ada?
        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
        {
            return BadRequest(new AuthResponse { Success = false, Message = "Email sudah terdaftar." });
        }

        // 2. Hash Password (PENTING!)
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        // 3. Buat User Baru
        var newUser = new Users
        {
            Name = request.Name,
            Email = request.Email,
            Password = passwordHash, // Simpan yang sudah di-hash
            Role = "User" // Default role
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return Ok(new AuthResponse { Success = true, Message = "Registrasi berhasil! Silakan login." });
    }

    // Fungsi bikin JWT Token
    private string CreateToken(Users user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        // Ambil secret key dari appsettings.json (Pastikan kamu sudah set di sana)
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}