using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AMS.AMS.Models;
using AttendanceManagementSystem.DTOs;
using AMS.AMS;

namespace AMS.Controllers
{
    /// <summary>
    /// Manages student enrollments in the DBTC Attendance Management System.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public EnrollmentsController(AppDbContext context) { _context = context; }

        /// <summary>Get all enrollments with optional pagination.</summary>
        /// <param name="page">Page number (default: 1)</param>
        /// <param name="pageSize">Records per page (default: 10, max: 100)</param>
        /// <response code="200">Returns paginated list of enrollments</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IEnumerable<Enrollment>> GetEnrollments([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            pageSize = Math.Min(pageSize, 100); page = Math.Max(page, 1);
            var total = await _context.Enrollments.CountAsync();
            var enrollments = await _context.Enrollments
                .Include(e => e.Student).Include(e => e.Class).ThenInclude(c => c.Course)
                .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            Response.Headers.Append("X-Total-Count", total.ToString());
            Response.Headers.Append("X-Page", page.ToString());
            Response.Headers.Append("X-Page-Size", pageSize.ToString());
            Response.Headers.Append("X-Total-Pages", ((int)Math.Ceiling((double)total / pageSize)).ToString());
            return enrollments;
        }

        /// <summary>Get a single enrollment by ID.</summary>
        /// <response code="200">Returns the enrollment</response>
        /// <response code="404">Enrollment not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Enrollment>> GetEnrollment(int id)
        {
            var e = await _context.Enrollments.Include(x => x.Student).Include(x => x.Class).FirstOrDefaultAsync(x => x.Id == id);
            if (e == null) return NotFound(new { message = $"Enrollment with ID {id} was not found." });
            return Ok(e);
        }

        /// <summary>Create a new enrollment.</summary>
        /// <response code="200">Enrollment created successfully</response>
        /// <response code="400">Invalid or missing fields</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateEnrollment(EnrollmentDTO dto)
        {
            if (dto.StudentId == 0 || dto.ClassId == 0)
                return BadRequest(new { message = "StudentId and ClassId are required." });
            var enrollment = new Enrollment { StudentId = dto.StudentId, ClassId = dto.ClassId, Status = dto.Status, DateEnrolled = DateTime.UtcNow };
            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();
            return Ok(enrollment);
        }

        /// <summary>Update an existing enrollment.</summary>
        /// <response code="204">Updated successfully</response>
        /// <response code="404">Enrollment not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateEnrollment(int id, EnrollmentDTO dto)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null) return NotFound(new { message = $"Enrollment with ID {id} was not found." });
            enrollment.StudentId = dto.StudentId; enrollment.ClassId = dto.ClassId; enrollment.Status = dto.Status;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>Delete an enrollment by ID.</summary>
        /// <response code="204">Deleted successfully</response>
        /// <response code="404">Enrollment not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteEnrollment(int id)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null) return NotFound(new { message = $"Enrollment with ID {id} was not found." });
            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}