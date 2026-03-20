using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AMS.AMS.Models;
using AttendanceManagementSystem.DTOs;
using AMS.AMS;

namespace AMS.Controllers
{
    /// <summary>
    /// Manages student records in the DBTC Attendance Management System.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class StudentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudentsController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all students with optional pagination.
        /// </summary>
        /// <param name="page">Page number (default: 1)</param>
        /// <param name="pageSize">Number of records per page (default: 10, max: 100)</param>
        /// <returns>Paginated list of students with their course information.</returns>
        /// <response code="200">Returns the paginated list of students</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            pageSize = Math.Min(pageSize, 100);
            page = Math.Max(page, 1);

            var totalCount = await _context.Students.CountAsync();

            var students = await _context.Students
                .Include(s => s.Course)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            Response.Headers.Append("X-Total-Count", totalCount.ToString());
            Response.Headers.Append("X-Page", page.ToString());
            Response.Headers.Append("X-Page-Size", pageSize.ToString());
            Response.Headers.Append("X-Total-Pages", ((int)Math.Ceiling((double)totalCount / pageSize)).ToString());

            return Ok(students);
        }

        /// <summary>
        /// Get a single student by ID.
        /// </summary>
        /// <param name="id">The student ID</param>
        /// <returns>The student record with course information.</returns>
        /// <response code="200">Returns the student</response>
        /// <response code="404">Student not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _context.Students
                .Include(s => s.Course)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
                return NotFound(new { message = $"Student with ID {id} was not found." });

            return Ok(student);
        }

        /// <summary>
        /// Create a new student.
        /// </summary>
        /// <param name="dto">Student data: FirstName, LastName, Email, CourseId</param>
        /// <returns>The newly created student record.</returns>
        /// <response code="200">Student created successfully</response>
        /// <response code="400">Invalid or missing fields</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Student>> CreateStudent(StudentDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.FirstName) ||
                string.IsNullOrWhiteSpace(dto.LastName) ||
                string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest(new { message = "FirstName, LastName, and Email are required." });

            var student = new Student
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                CourseId = dto.CourseId
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return Ok(student);
        }

        /// <summary>
        /// Update an existing student.
        /// </summary>
        /// <param name="id">The student ID to update</param>
        /// <param name="dto">Updated student data</param>
        /// <response code="204">Student updated successfully</response>
        /// <response code="404">Student not found</response>
        /// <response code="400">Invalid or missing fields</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateStudent(int id, StudentDTO dto)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
                return NotFound(new { message = $"Student with ID {id} was not found." });

            if (string.IsNullOrWhiteSpace(dto.FirstName) ||
                string.IsNullOrWhiteSpace(dto.LastName) ||
                string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest(new { message = "FirstName, LastName, and Email are required." });

            student.FirstName = dto.FirstName;
            student.LastName = dto.LastName;
            student.Email = dto.Email;
            student.CourseId = dto.CourseId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Delete a student by ID.
        /// </summary>
        /// <param name="id">The student ID to delete</param>
        /// <response code="204">Student deleted successfully</response>
        /// <response code="404">Student not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
                return NotFound(new { message = $"Student with ID {id} was not found." });

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}