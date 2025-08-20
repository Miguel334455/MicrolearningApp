using Back.Context;
using Back.DTOs;
using Back.Models;
using Back.Services;
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
    public class UsersController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly JwtService _jwtService;
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context,IUserService userService, JwtService jwtService)
        {
            _context = context;
            _userService = userService;
            _jwtService = jwtService;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserWithCoursesDTO>> GetUserWithCourses(int id)
        {
            var user = await _context.Users
                .Include(u => u.UserCourses!)
                    .ThenInclude(uc => uc.Course)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return NotFound();

            var dto = new UserWithCoursesDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Courses = user.UserCourses
                    .Select(uc => new CourseDTO
                    {
                        Id = uc.Course.Id,
                        Name = uc.Course.Name
                    })
                    .ToList()
            };

            return Ok(dto);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("register")]
        public async Task<ActionResult<CreateUserDTO>> PostUser(CreateUserDTO dto)
        {
            var exists = await _context.Users.AnyAsync(u => u.Email.ToLower() == dto.Email.ToLower());

            if (exists) return BadRequest("Ya existe un usuario con ese correo.");

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var result = new CreateUserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };

            return CreatedAtAction(nameof(GetUserWithCourses), new { id = user.Id }, result);
        }
        [HttpPost("login")]
        public IActionResult Login(LoginDTO dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                return Unauthorized("Invalid credentials.");

            var token = _jwtService.Generate(user);
            return Ok(new { token });
        }


        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
