using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AMS.AMS.Models;
using AttendanceManagementSystem.DTOs;
using AMS.AMS;

namespace AMS.Controllers
{
    /// <summary>
    /// Manages teacher (faculty) records in the DBTC Attendance Management System.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TeachersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TeachersController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all teachers with optional pagination.
        /// </summary>
        /// <param name="page">Page number (default: 1)</param>
        /// <param name="pageSize">Number of records per page (default: 10, max: 100)</param>
        /// <returns>Paginated list of all teachers.</returns>
        /// <response code="200">Returns the paginated list of teachers</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IEnumerable<Teacher>> GetTeachers(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            pageSize = Math.Min(pageSize, 100);
            page = Math.Max(page, 1);

            var totalCount = await _context.Teachers.CountAsync();

            var teachers = await _context.Teachers
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            Response.Headers.Append("X-Total-Count", totalCount.ToString());
            Response.Headers.Append("X-Page", page.ToString());
            Response.Headers.Append("X-Page-Size", pageSize.ToString());
            Response.Headers.Append("X-Total-Pages", ((int)Math.Ceiling((double)totalCount / pageSize)).ToString());

            return teachers;
        }

        /// <summary>
        /// Get a single teacher by ID.
        /// </summary>
        /// <param name="id">The teacher ID</param>
        /// <returns>The teacher record.</returns>
        /// <response code="200">Returns the teacher</response>
        /// <response code="404">Teacher not found</response>
        [HttpPost("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Teacher>> GetTeacher(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);

            if (teacher == null)
                return NotFound(new { message = $"Teacher with ID {id} was not found." });

            return Ok(teacher);
        }

        /// <summary>
        /// Create a new teacher.
        /// </summary>
        /// <param name="dto">Teacher data: FirstName, LastName, Email</param>
        /// <returns>The newly created teacher record.</returns>
        /// <response code="200">Teacher created successfully</response>
        /// <response code="400">Invalid or missing fields</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTeacher(TeacherDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.FirstName) ||
                string.IsNullOrWhiteSpace(dto.LastName) ||
                string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest(new { message = "FirstName, LastName, and Email are required." });

            var teacher = new Teacher
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email
            };

            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();

            return Ok(teacher);
        }

        /// <summary>
        /// Update an existing teacher.
        /// </summary>
        /// <param name="id">The teacher ID to update</param>
        /// <param name="dto">Updated teacher data</param>
        /// <response code="204">Teacher updated successfully</response>
        /// <response code="404">Teacher not found</response>
        /// <response code="400">Invalid or missing fields</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateTeacher(int id, TeacherDTO dto)
        {
            var teacher = await _context.Teachers.FindAsync(id);

            if (teacher == null)
                return NotFound(new { message = $"Teacher with ID {id} was not found." });

            if (string.IsNullOrWhiteSpace(dto.FirstName) ||
                string.IsNullOrWhiteSpace(dto.LastName) ||
                string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest(new { message = "FirstName, LastName, and Email are required." });

            teacher.FirstName = dto.FirstName;
            teacher.LastName = dto.LastName;
            teacher.Email = dto.Email;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Delete a teacher by ID.
        /// </summary>
        /// <param name="id">The teacher ID to delete</param>
        /// <response code="204">Teacher deleted successfully</response>
        /// <response code="404">Teacher not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);

            if (teacher == null)
                return NotFound(new { message = $"Teacher with ID {id} was not found." });

            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}