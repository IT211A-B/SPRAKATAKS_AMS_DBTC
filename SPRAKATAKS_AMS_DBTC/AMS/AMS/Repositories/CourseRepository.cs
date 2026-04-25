using AMS.AMS.Models;
using AttendanceManagementSystem.DTOs;
using Microsoft.EntityFrameworkCore;

namespace AMS.AMS.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly AppDbContext _context;

        public CourseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            return await _context.Courses
                .Include(c => c.Teacher)
                .Include(c => c.Students)
                .ToListAsync();
        }

        public async Task<Course?> GetByIdAsync(int id)
        {
            return await _context.Courses
                .Include(c => c.Teacher)
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Course> CreateAsync(CourseDTO dto)
        {
            var course = new Course
            {
                CourseName = dto.CourseName,
                TeacherId = dto.TeacherId
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<Course?> UpdateAsync(int id, CourseDTO dto)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return null;

            course.CourseName = dto.CourseName;
            course.TeacherId = dto.TeacherId;

            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return false;

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}