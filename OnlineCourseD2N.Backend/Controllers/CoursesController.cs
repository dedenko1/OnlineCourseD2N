using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineCourseD2N.Backend.Data;

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

        // GET: api/courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            return await _context.Courses
                .Include(c => c.Trainer)
                .ToListAsync();
        }

        // GET: api/courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Trainer)
                .FirstOrDefaultAsync(c => c.CourseId == id);

            if (course == null)
                return NotFound();

            return course;
        }

        // GET: api/courses/search?keyword=blazor
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Course>>> SearchCourses(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return await GetCourses();

            return await _context.Courses
                .Include(c => c.Trainer)
                .Where(c => c.Title.Contains(keyword) ||
                            c.Description.Contains(keyword) ||
                            c.Category.Contains(keyword))
                .ToListAsync();
        }

        // POST: api/courses
        [HttpPost]
        public async Task<ActionResult<Course>> CreateCourse(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCourse), new { id = course.CourseId }, course);
        }

        // PUT: api/courses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, Course course)
        {
            if (id != course.CourseId)
                return BadRequest();

            _context.Entry(course).State = EntityState.Modified;

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

        // DELETE: api/courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return NotFound();

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
