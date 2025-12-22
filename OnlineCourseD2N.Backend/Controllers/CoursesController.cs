using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineCourseD2N.Backend.Data;
using System.Security.Claims;

// 👇 ALIAS BIAR GAK BINGUNG
using CourseDto = OnlineCourseD2N.Shared.Models.Course;
using TrainerDto = OnlineCourseD2N.Shared.Models.Trainer;
using CourseEntity = OnlineCourseD2N.Backend.Data.Course;

namespace OnlineCourseD2N.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CoursesController(AppDbContext context)
        {
            _context = context;
        }

        // ==========================================
        // 1. GET ALL (Mapping Entity -> DTO)
        // ==========================================
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses()
        {
            var role = User.FindFirstValue(ClaimTypes.Role);
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int.TryParse(userIdString, out int currentUserId);

            var query = _context.Courses
                .Include(c => c.Trainer)
                .ThenInclude(t => t.User) // 🔥 WAJIB: Ambil User biar nama muncul
                .AsQueryable();

            if (role == "Trainer")
            {
                var trainerId = await _context.Trainers
                    .Where(t => t.UserId == currentUserId)
                    .Select(t => t.TrainerId)
                    .FirstOrDefaultAsync();

                query = query.Where(c => c.TrainerId == trainerId);
            }

            var entities = await query.ToListAsync();

            // Panggil Helper MapToDto
            var dtos = entities.Select(MapToDto).ToList();

            return Ok(dtos);
        }

        // ==========================================
        // 2. GET BY ID (Mapping Entity -> DTO)
        // ==========================================
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetCourse(int id)
        {
            var entity = await _context.Courses
                .Include(c => c.Trainer)
                .ThenInclude(t => t.User) // 🔥 WAJIB
                .FirstOrDefaultAsync(c => c.CourseId == id);

            if (entity == null) return NotFound();

            return Ok(MapToDto(entity));
        }

        // ==========================================
        // 3. SEARCH (Mapping Entity -> DTO)
        // ==========================================
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<CourseDto>>> SearchCourses(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return await GetCourses();

            var entities = await _context.Courses
                .Include(c => c.Trainer)
                .ThenInclude(t => t.User) // 🔥 WAJIB
                .Where(c => c.Title.Contains(keyword) ||
                            c.Description.Contains(keyword) ||
                            c.Category.Contains(keyword) ||
                            c.Trainer.User.Name.Contains(keyword)) // Bisa cari by nama trainer
                .ToListAsync();

            var dtos = entities.Select(MapToDto).ToList();
            return Ok(dtos);
        }

        [HttpPost]
        public async Task<ActionResult<CourseDto>> CreateCourse([FromBody] CourseDto request)
        {
            // Mapping DTO ke Entity
            var newCourse = new CourseEntity
            {
                Title = request.Title,
                Category = request.Category,
                Description = request.Description,
                Duration = request.Duration,
                Level = request.Level,
                CoverImage = request.CoverImage,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                LocationAddress = request.LocationAddress,
                TrainerId = request.TrainerId
            };

            _context.Courses.Add(newCourse);
            await _context.SaveChangesAsync();

            // Return DTO (reload biar dapat data trainer lengkap jika perlu, atau return simple)
            request.CourseId = newCourse.CourseId;
            return CreatedAtAction(nameof(GetCourse), new { id = newCourse.CourseId }, request);
        }

        // ==========================================
        // 5. UPDATE (DTO -> Entity)
        // ==========================================
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] CourseDto request)
        {
            if (id != request.CourseId) return BadRequest();

            var entity = await _context.Courses.FindAsync(id);
            if (entity == null) return NotFound();

            // Update Field
            entity.Title = request.Title;
            entity.Category = request.Category;
            entity.Description = request.Description;
            entity.Duration = request.Duration;
            entity.Level = request.Level;
            entity.CoverImage = request.CoverImage;
            entity.Latitude = request.Latitude;
            entity.Longitude = request.Longitude;
            entity.LocationAddress = request.LocationAddress;
            entity.TrainerId = request.TrainerId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Courses.Any(e => e.CourseId == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // ==========================================
        // 6. DELETE
        // ==========================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ==========================================
        // HELPER: MAPPING MANUAL
        // ==========================================
        private static CourseDto MapToDto(CourseEntity c)
        {
            return new CourseDto
            {
                CourseId = c.CourseId,
                Title = c.Title,
                Category = c.Category,
                Description = c.Description,
                Duration = c.Duration,
                Level = c.Level,
                CoverImage = c.CoverImage,
                Latitude = c.Latitude,
                Longitude = c.Longitude,
                LocationAddress = c.LocationAddress,
                TrainerId = c.TrainerId,

                // Mapping Trainer agar Namanya muncul di Frontend
                Trainer = c.Trainer == null ? null : new TrainerDto
                {
                    TrainerId = c.Trainer.TrainerId,
                    UserId = c.Trainer.UserId,
                    // Ambil nama dari User
                    Name = c.Trainer.User?.Name ?? "Tanpa Nama",
                    Email = c.Trainer.User?.Email ?? "-",
                    Expertise = c.Trainer.Expertise,
                    Bio = c.Trainer.Bio,
                    FotoProfil = c.Trainer.FotoProfil
                }
            };
        }
    }
}