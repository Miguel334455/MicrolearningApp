using Back.Context;
using Back.DTOs;
using Back.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCoursesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserCoursesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/UserCourses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserCourse>>> GetUserCourses()
        {
            return await _context.UserCourses.ToListAsync();
        }

        // GET: api/UserCourses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserCourse>> GetUserCourse(int id)
        {
            var userCourse = await _context.UserCourses.FindAsync(id);

            if (userCourse == null)
            {
                return NotFound();
            }

            return userCourse;
        }

        // PUT: api/UserCourses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserCourse(int id, UserCourse userCourse)
        {
            if (id != userCourse.UserCourseId)
            {
                return BadRequest();
            }

            _context.Entry(userCourse).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserCourseExists(id))
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

        // POST: api/UserCourses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> SubscribeUserToCourse(SubscribeUserToCourseDTO dto)
        {
            // Validar que el usuario y curso existen
            var userExists = await _context.Users.AnyAsync(u => u.Id == dto.UserId);
            var courseExists = await _context.Courses.AnyAsync(c => c.Id == dto.CourseId);

            if (!userExists || !courseExists)
                return NotFound("Usuario o curso no encontrado");

            // Evitar suscripción duplicada
            var alreadyExists = await _context.UserCourses
                .AnyAsync(uc => uc.UserId == dto.UserId && uc.CourseId == dto.CourseId);

            if (alreadyExists)
                return Conflict("El usuario ya está suscrito a este curso");

            // Crear suscripción
            var userCourse = new UserCourse
            {
                UserId = dto.UserId,
                CourseId = dto.CourseId
            };

            _context.UserCourses.Add(userCourse);
            await _context.SaveChangesAsync();

            return Ok("Suscripción exitosa");
        }

        // DELETE: api/UserCourses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserCourse(int id)
        {
            var userCourse = await _context.UserCourses.FindAsync(id);
            if (userCourse == null)
            {
                return NotFound();
            }

            _context.UserCourses.Remove(userCourse);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserCourseExists(int id)
        {
            return _context.UserCourses.Any(e => e.UserCourseId == id);
        }
    }
}
