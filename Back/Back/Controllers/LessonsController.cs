using Back.Context;
using Back.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonsController : ControllerBase
    {
        private readonly AppDbContext _context;

        // GET: api/Lessons/animal/{animalName}
        [HttpPost("info/{subject}")]
        public async Task<IActionResult> GetList([FromRoute] string subject, int courseid)
        {

            AnswerGeneratorService generator = new AnswerGeneratorService();

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://en.wikipedia.org/api/rest_v1/page/summary/");

            var response = await client.GetAsync($"{subject}");

            var lesson = new Lesson
            {
                courseId = courseid, 
                content = "This is a placeholder content."
            };

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();

                // Parse the JSON to extract the summary
                dynamic wikiData = JsonConvert.DeserializeObject(data);
                string summary = wikiData.extract != null ? (string)wikiData.extract : "No summary available.";

                // Compose a prompt for ChatGPT
                string prompt = $"Hazme una leccion de alrededor de 500 palabras en español usando el siguiente resumen como base:\n\n{summary}";

                string res = await generator.GenerateAnswer(prompt);

                lesson.content = res;

                // Save the lesson to the database
                await PostLesson(lesson);


                return Ok(res);
            }
            else
            {
                return StatusCode((int)response.StatusCode, "Error fetching data from external API");
            }
        }

        public LessonsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Lessons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lesson>>> GetLesson()
        {
            return await _context.Lesson.ToListAsync();
        }

        // GET: api/Lessons/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Lesson>> GetLesson(int id)
        {
            var lesson = await _context.Lesson.FindAsync(id);

            if (lesson == null)
            {
                return NotFound();
            }

            return lesson;
        }

        // PUT: api/Lessons/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutLesson(int id, Lesson lesson)
        {
            if (id != lesson.Id)
            {
                return BadRequest();
            }

            _context.Entry(lesson).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LessonExists(id))
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

        // POST: api/Lessons
        [HttpPost]
        public async Task<ActionResult<Lesson>> PostLesson(Lesson lesson)
        {
            _context.Lesson.Add(lesson);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLesson", new { id = lesson.Id }, lesson);
        }

        // DELETE: api/Lessons/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteLesson(int id)
        {
            var lesson = await _context.Lesson.FindAsync(id);
            if (lesson == null)
            {
                return NotFound();
            }

            _context.Lesson.Remove(lesson);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LessonExists(int id)
        {
            return _context.Lesson.Any(e => e.Id == id);
        }
    }
}
