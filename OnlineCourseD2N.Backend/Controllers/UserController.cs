using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineCourseD2N.Backend.Data;
using OnlineCourseD2N.Shared.Models; // Pastikan ini ada

namespace OnlineCourseD2N.Backend.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize(Roles = "Admin")] // 🔒 Hanya Admin yang boleh akses
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsersDTO>>> GetUsers()
        {
            // 1. Ambil data Entity dari Database
            var users = await _context.Users.ToListAsync();

            // 2. MAPPING: Ubah Entity ke DTO (PENTING!)
            // Agar password tidak ikut terkirim & tipe data cocok dengan Service
            var userDtos = users.Select(u => new UsersDTO
            {
                UserId = u.UserId,
                Name = u.Name,
                Email = u.Email,
                Role = u.Role
            }).ToList();

            return Ok(userDtos);
        }

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            // Opsional: Cegah Admin Utama terhapus
            if (user.Email == "admin@ruangles.com")
            {
                return BadRequest("Super Admin tidak boleh dihapus!");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}