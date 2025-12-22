using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineCourseD2N.Backend.Data;
using OnlineCourseD2N.Shared.Models; // Pastikan namespace ini benar

// Alias agar tidak bentrok
using TrainerDto = OnlineCourseD2N.Shared.Models.Trainer;
using CourseDto = OnlineCourseD2N.Shared.Models.Course;
using TrainerEntity = OnlineCourseD2N.Backend.Data.Trainer;
using UserEntity = OnlineCourseD2N.Backend.Data.Users;

namespace OnlineCourseD2N.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TrainersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrainerDto>>> GetTrainers()
        {
            var entities = await _context.Trainers
                .Include(t => t.User)
                .ToListAsync();

            var dtos = entities.Select(MapToDto).ToList();
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TrainerDto>> GetTrainer(int id)
        {
            var t = await _context.Trainers
                .Include(x => x.User)
                .Include(x => x.Courses)
                .FirstOrDefaultAsync(x => x.TrainerId == id);

            if (t == null) return NotFound();

            return Ok(MapToDto(t));
        }

        // ==========================================
        // 3. SEARCH
        // ==========================================
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<TrainerDto>>> SearchTrainers(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return await GetTrainers();

            var entities = await _context.Trainers
                .Include(t => t.User)
                .Where(t => t.User.Name.Contains(keyword) ||
                            t.Expertise.Contains(keyword) ||
                            t.User.Email.Contains(keyword))
                .ToListAsync();

            var dtos = entities.Select(MapToDto).ToList();
            return Ok(dtos);
        }

        // ==========================================
        // 4. CREATE (Auto Create User & Trainer)
        // ==========================================
        [HttpPost]
        public async Task<ActionResult<TrainerDto>> CreateTrainer([FromBody] TrainerDto request)
        {
            // Cek apakah email sudah terdaftar sebagai User?
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            UserEntity userToLink;

            if (existingUser != null)
            {
                // Jika user sudah ada, cek apakah dia sudah jadi Trainer?
                var alreadyTrainer = await _context.Trainers.AnyAsync(t => t.UserId == existingUser.UserId);
                if (alreadyTrainer)
                {
                    return BadRequest($"Email '{request.Email}' sudah terdaftar sebagai Trainer.");
                }

                // Gunakan user yang ada
                userToLink = existingUser;

                // Update nama jika perlu
                if (!string.IsNullOrEmpty(request.Name)) userToLink.Name = request.Name;
            }
            else
            {
                // 🔥 Jika User belum ada, BUAT BARU OTOMATIS
                userToLink = new UserEntity
                {
                    Name = request.Name,
                    Email = request.Email,
                    Role = "Trainer",
                    // Password default: 123456 (Hash pake BCrypt)
                    Password = BCrypt.Net.BCrypt.HashPassword("123456")
                };

                _context.Users.Add(userToLink);
                await _context.SaveChangesAsync(); // Save biar dapat UserId
            }

            // Pastikan Role jadi Trainer
            userToLink.Role = "Trainer";

            // Buat Profil Trainer
            var newTrainer = new TrainerEntity
            {
                UserId = userToLink.UserId,
                Expertise = request.Expertise,
                Bio = request.Bio,
                FotoProfil = request.FotoProfil
            };

            _context.Trainers.Add(newTrainer);
            await _context.SaveChangesAsync();

            // Return hasil
            var resultDto = MapToDto(newTrainer);
            // Isi manual data user karena Entity tadi belum di-reload
            resultDto.Name = userToLink.Name;
            resultDto.Email = userToLink.Email;

            return CreatedAtAction(nameof(GetTrainer), new { id = newTrainer.TrainerId }, resultDto);
        }

        // ==========================================
        // 5. UPDATE (Update Trainer & User Info)
        // ==========================================
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrainer(int id, [FromBody] TrainerDto request)
        {
            if (id != request.TrainerId)
                return BadRequest("ID URL tidak cocok");

            var trainerEntity = await _context.Trainers.FindAsync(id);
            if (trainerEntity == null) return NotFound();

            // Update Tabel Trainer
            trainerEntity.Expertise = request.Expertise;
            trainerEntity.Bio = request.Bio;
            trainerEntity.FotoProfil = request.FotoProfil;

            // 🔥 Update Tabel User (Nama & Email)
            var userEntity = await _context.Users.FindAsync(trainerEntity.UserId);
            if (userEntity != null)
            {
                userEntity.Name = request.Name;
                userEntity.Email = request.Email;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Trainers.Any(e => e.TrainerId == id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        // ==========================================
        // 6. DELETE
        // ==========================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrainer(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer == null) return NotFound();

            // Opsional: Hapus User-nya juga? 
            // Kalau "Ya" (Cascade), cukup hapus User-nya.
            // Kalau "Tidak" (hanya copot jabatan), hapus Trainernya saja.

            // Skenario: Copot jabatan Trainer, kembalikan jadi User biasa
            var user = await _context.Users.FindAsync(trainer.UserId);
            if (user != null)
            {
                user.Role = "User";
            }

            _context.Trainers.Remove(trainer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ==========================================
        // HELPER MAPPING
        // ==========================================
        private static TrainerDto MapToDto(TrainerEntity t)
        {
            return new TrainerDto
            {
                TrainerId = t.TrainerId,
                UserId = t.UserId,

                Name = t.User?.Name ?? "Tanpa Nama",
                Email = t.User?.Email ?? "-",

                Expertise = t.Expertise,
                Bio = t.Bio,
                FotoProfil = t.FotoProfil,

                // Mapping Course (Jika di-include)
                Courses = t.Courses?.Select(c => new CourseDto
                {
                    CourseId = c.CourseId,
                    Title = c.Title,
                    Category = c.Category,
                    Level = c.Level,
                    Duration = c.Duration,
                    CoverImage = c.CoverImage,
                    Description = c.Description
                }).ToList()
            };
        }
    }
}