using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineCourseD2N.Backend.Data;

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

        // GET: api/trainers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Trainer>>> GetTrainers()
        {
            return await _context.Trainers
                .Include(t => t.Courses)
                .ToListAsync();
        }

        // GET: api/trainers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Trainer>> GetTrainer(int id)
        {
            var trainer = await _context.Trainers
                .Include(t => t.Courses)
                .FirstOrDefaultAsync(t => t.TrainerId == id);

            if (trainer == null)
                return NotFound();

            return trainer;
        }

        // GET: api/trainers/search?keyword=john
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Trainer>>> SearchTrainers(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return await GetTrainers();

            return await _context.Trainers
                .Where(t => t.Name.Contains(keyword) ||
                            t.Expertise.Contains(keyword) ||
                            t.Email.Contains(keyword))
                .Include(t => t.Courses)
                .ToListAsync();
        }

        // POST: api/trainers
        [HttpPost]
        public async Task<ActionResult<Trainer>> CreateTrainer( [FromBody] Trainer trainer)
        {
            _context.Trainers.Add(trainer);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTrainer), new { id = trainer.TrainerId }, trainer);
        }

        // PUT: api/trainers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrainer(int id, Trainer trainer)
        {
            if (id != trainer.TrainerId)
                return BadRequest();

            _context.Entry(trainer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Trainers.Any(e => e.TrainerId == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/trainers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrainer(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer == null)
                return NotFound();

            _context.Trainers.Remove(trainer);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
