using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi_lab1.Context;
using WebApi_lab1.Models;

namespace WebApi_lab1.Controllers
{
    [Route("api/[controller]")] //api/student
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly CompanyContext _context;

        public StudentController(CompanyContext context)
        {
            _context = context;
        }

        // 1. Get all students
        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<Student>>> GetAll()
        {
            return await _context.Students.ToListAsync();
        }

        // 2. Get student by Id
        [HttpGet("byId/{id}")]
        public async Task<ActionResult<Student>> GetById(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return student;
        }

        // 3. Get students by Name
        [HttpGet("byName/{name}")]
        public async Task<ActionResult<List<Student>>> GetByName(string name)
        {
            var students = await _context.Students
                                         .Where(s => s.Name.Contains(name))
                                         .ToListAsync();

            if (students.Count == 0)
            {
                return NotFound();
            }

            return students;
        }

        // 4. Add a new student
        [HttpPost("add")]
        public async Task<ActionResult<Student>> Add( Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = student.Id }, student);
        }

        // 5. Update student
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Student student)
        {
            if (id != student.Id)
            {
                return BadRequest();
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Students.Any(e => e.Id == id))
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

        // 6. Delete student
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // 7. Update Student Name ONLY
        [HttpPatch("updateName/{id}")]
        public async Task<IActionResult> UpdateStudentName(int id, [FromBody] string newName)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            student.Name = newName;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}