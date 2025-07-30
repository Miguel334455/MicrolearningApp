using Back.Context;
using Back.DTOs;
using Back.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CoursesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseFullDTO>>> GetAllCoursesFull()
        {
            var courses = await _context.Courses
                .Include(c => c.UserCourses!)
                .ThenInclude(uc => uc.User)
                .Include(c => c.Lessons!)
                .ToListAsync();

            var result = courses.Select(course => new CourseFullDTO
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                IdCategory = course.IdCategory,
                Users = course.UserCourses
                    .Select(uc => new UserDTO
                    {
                        Id = uc.User.Id,
                        Name = uc.User.Name,
                        Email = uc.User.Email
                    }).ToList(),
                Lessons = course.Lessons
                    .Select(lesson => new LessonDTO
                    {
                        Id = lesson.Id,
                        Content = lesson.content
                    }).ToList()
            }).ToList();

            return Ok(result);
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseFullDTO>> GetCourseFull(int id)
        {
            var course = await _context.Courses
                .Include(c => c.UserCourses!)
                .ThenInclude(uc => uc.User)
                .Include(c => c.Lessons!)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
                return NotFound();

            var dto = new CourseFullDTO
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                IdCategory = course.IdCategory,
                Users = course.UserCourses
                    .Select(uc => new UserDTO
                    {
                        Id = uc.User.Id,
                        Name = uc.User.Name,
                        Email = uc.User.Email
                    })
                    .ToList(),
                Lessons = course.Lessons
                    .Select(l => new LessonDTO
                    {
                        Id = l.Id,
                        Content = l.content
                    })
                    .ToList()
            };

            return Ok(dto);
        }


        // PUT: api/Courses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, Course course)
        {
            if (id != course.Id)
            {
                return BadRequest();
            }

            _context.Entry(course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Courses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(CreateCourseDTO dto)
        {
            var course = new Course
            {
                Name = dto.Name,
                Description = dto.Description,
                IdCategory = dto.IdCategory
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCourseFull), new { id = course.Id }, course);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }
    }
}
