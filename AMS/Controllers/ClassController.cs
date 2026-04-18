using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AMS.AMS.Models;
using AttendanceManagementSystem.DTOs;
using AMS.AMS;

namespace AMS.Controllers
{
    /// <summary>
    /// Manages class sections in the DBTC Attendance Management System.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ClassesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ClassesController(AppDbContext context) { _context = context; }

        /// <summary>Get all classes with optional pagination.</summary>
        /// <param name="page">Page number (default: 1)</param>
        /// <param name="pageSize">Records per page (default: 10, max: 100)</param>
        /// <response code="200">Returns paginated list of classes</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IEnumerable<Class>> GetClasses([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            pageSize = Math.Min(pageSize, 100); page = Math.Max(page, 1);
            var total = await _context.Classes.CountAsync();
            var classes = await _context.Classes.Include(c => c.Course).Include(c => c.Enrollments)
                .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            Response.Headers.Append("X-Total-Count", total.ToString());
            Response.Headers.Append("X-Page", page.ToString());
            Response.Headers.Append("X-Page-Size", pageSize.ToString());
            Response.Headers.Append("X-Total-Pages", ((int)Math.Ceiling((double)total / pageSize)).ToString());
            return classes;
        }

        /// <summary>Get a single class by ID.</summary>
        /// <response code="200">Returns the class</response>
        /// <response code="404">Class not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Class>> GetClass(int id)
        {
            var cls = await _context.Classes.Include(c => c.Course).Include(c => c.Enrollments).FirstOrDefaultAsync(c => c.Id == id);
            if (cls == null) return NotFound(new { message = $"Class with ID {id} was not found." });
            return Ok(cls);
        }

        /// <summary>Create a new class section.</summary>
        /// <response code="200">Class created successfully</response>
        /// <response code="400">Invalid or missing fields</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateClass(ClassDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Section) || string.IsNullOrWhiteSpace(dto.YearLevel))
                return BadRequest(new { message = "Section and YearLevel are required." });
            var cls = new Class { Section = dto.Section, YearLevel = dto.YearLevel, CourseId = dto.CourseId };
            _context.Classes.Add(cls);
            await _context.SaveChangesAsync();
            return Ok(cls);
        }

        /// <summary>Update an existing class section.</summary>
        /// <response code="204">Updated successfully</response>
        /// <response code="404">Class not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateClass(int id, ClassDTO dto)
        {
            var cls = await _context.Classes.FindAsync(id);
            if (cls == null) return NotFound(new { message = $"Class with ID {id} was not found." });
            cls.Section = dto.Section; cls.YearLevel = dto.YearLevel; cls.CourseId = dto.CourseId;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>Delete a class by ID.</summary>
        /// <response code="204">Deleted successfully</response>
        /// <response code="404">Class not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteClass(int id)
        {
            var cls = await _context.Classes.FindAsync(id);
            if (cls == null) return NotFound(new { message = $"Class with ID {id} was not found." });
            _context.Classes.Remove(cls);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}