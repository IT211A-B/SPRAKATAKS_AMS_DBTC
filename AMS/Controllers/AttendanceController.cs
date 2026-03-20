using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AMS.AMS.Models;
using AttendanceManagementSystem.DTOs;
using AMS.AMS;

namespace AMS.Controllers
{
    /// <summary>
    /// Manages attendance records in the DBTC Attendance Management System.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AttendancesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public AttendancesController(AppDbContext context) { _context = context; }

        /// <summary>Get all attendance records with optional pagination.</summary>
        /// <param name="page">Page number (default: 1)</param>
        /// <param name="pageSize">Records per page (default: 10, max: 100)</param>
        /// <response code="200">Returns paginated list of attendance records</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IEnumerable<Attendance>> GetAttendances([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            pageSize = Math.Min(pageSize, 100); page = Math.Max(page, 1);
            var total = await _context.Attendances.CountAsync();
            var records = await _context.Attendances
                .Include(a => a.Student).Include(a => a.Class)
                .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            Response.Headers.Append("X-Total-Count", total.ToString());
            Response.Headers.Append("X-Page", page.ToString());
            Response.Headers.Append("X-Page-Size", pageSize.ToString());
            Response.Headers.Append("X-Total-Pages", ((int)Math.Ceiling((double)total / pageSize)).ToString());
            return records;
        }

        /// <summary>Get a single attendance record by ID.</summary>
        /// <response code="200">Returns the attendance record</response>
        /// <response code="404">Attendance record not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Attendance>> GetAttendance(int id)
        {
            var a = await _context.Attendances.Include(x => x.Student).Include(x => x.Class).FirstOrDefaultAsync(x => x.Id == id);
            if (a == null) return NotFound(new { message = $"Attendance record with ID {id} was not found." });
            return Ok(a);
        }

        /// <summary>Get all attendance records for a specific class.</summary>
        /// <param name="classId">The class ID to filter by</param>
        /// <response code="200">Returns attendance records for the class</response>
        [HttpGet("byclass/{classId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<Attendance>> GetByClass(int classId)
        {
            return await _context.Attendances.Include(a => a.Student).Where(a => a.ClassId == classId).ToListAsync();
        }

        /// <summary>Mark attendance for a student.</summary>
        /// <response code="200">Attendance marked successfully</response>
        /// <response code="400">Invalid or missing fields</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAttendance(AttendanceDTO dto)
        {
            if (dto.StudentId == 0 || dto.ClassId == 0)
                return BadRequest(new { message = "StudentId and ClassId are required." });
            var attendance = new Attendance { StudentId = dto.StudentId, ClassId = dto.ClassId, Date = dto.Date, Status = dto.Status };
            _context.Attendances.Add(attendance);
            await _context.SaveChangesAsync();
            return Ok(attendance);
        }

        /// <summary>Update an attendance record.</summary>
        /// <response code="204">Updated successfully</response>
        /// <response code="404">Attendance record not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAttendance(int id, AttendanceDTO dto)
        {
            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance == null) return NotFound(new { message = $"Attendance record with ID {id} was not found." });
            attendance.StudentId = dto.StudentId; attendance.ClassId = dto.ClassId;
            attendance.Date = dto.Date; attendance.Status = dto.Status;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>Delete an attendance record by ID.</summary>
        /// <response code="204">Deleted successfully</response>
        /// <response code="404">Attendance record not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAttendance(int id)
        {
            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance == null) return NotFound(new { message = $"Attendance record with ID {id} was not found." });
            _context.Attendances.Remove(attendance);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}