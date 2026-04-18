using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AMS.AMS.Models;
using AttendanceManagementSystem.DTOs;
using AMS.AMS;

namespace AMS.Controllers
{
    /// <summary>
    /// Manages course offerings in the DBTC Attendance Management System.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CoursesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CoursesController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all courses with optional pagination.
        /// </summary>
        /// <param name="page">Page number (default: 1)</param>
        /// <param name="pageSize">Number of records per page (default: 10, max: 100)</param>
        /// <returns>Paginated list of courses with teacher and student information.</returns>
        /// <response code="200">Returns the paginated list of courses</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IEnumerable<Course>> GetCourses(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            pageSize = Math.Min(pageSize, 100);
            page = Math.Max(page, 1);

            var totalCount = await _context.Courses.CountAsync();

            var courses = await _context.Courses
                .Include(c => c.Teacher)
                .Include(c => c.Students)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            Response.Headers.Append("X-Total-Count", totalCount.ToString());
            Response.Headers.Append("X-Page", page.ToString());
            Response.Headers.Append("X-Page-Size", pageSize.ToString());
            Response.Headers.Append("X-Total-Pages", ((int)Math.Ceiling((double)totalCount / pageSize)).ToString());

            return courses;
        }

        /// <summary>
        /// Get a single course by ID.
        /// </summary>
        /// <param name="id">The course ID</param>
        /// <returns>The course record with teacher and enrolled students.</returns>
        /// <response code="200">Returns the course</response>
        /// <response code="404">Course not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Teacher)
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
                return NotFound(new { message = $"Course with ID {id} was not found." });

            return Ok(course);
        }

        /// <summary>
        /// Create a new course.
        /// </summary>
        /// <param name="dto">Course data: CourseName, TeacherId</param>
        /// <returns>The newly created course record.</returns>
        /// <response code="200">Course created successfully</response>
        /// <response code="400">Invalid or missing fields</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCourse(CourseDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.CourseName))
                return BadRequest(new { message = "CourseName is required." });

            var course = new Course
            {
                CourseName = dto.CourseName,
                TeacherId = dto.TeacherId
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return Ok(course);
        }

        /// <summary>
        /// Update an existing course.
        /// </summary>
        /// <param name="id">The course ID to update</param>
        /// <param name="dto">Updated course data</param>
        /// <response code="204">Course updated successfully</response>
        /// <response code="404">Course not found</response>
        /// <response code="400">Invalid or missing fields</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCourse(int id, CourseDTO dto)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
                return NotFound(new { message = $"Course with ID {id} was not found." });

            if (string.IsNullOrWhiteSpace(dto.CourseName))
                return BadRequest(new { message = "CourseName is required." });

            course.CourseName = dto.CourseName;
            course.TeacherId = dto.TeacherId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Delete a course by ID.
        /// </summary>
        /// <param name="id">The course ID to delete</param>
        /// <response code="204">Course deleted successfully</response>
        /// <response code="404">Course not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
                return NotFound(new { message = $"Course with ID {id} was not found." });

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}