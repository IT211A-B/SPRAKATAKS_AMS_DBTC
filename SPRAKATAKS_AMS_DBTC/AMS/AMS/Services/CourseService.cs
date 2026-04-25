using AMS.AMS.Models;
using AMS.AMS.Repositories;
using AttendanceManagementSystem.DTOs;

namespace AMS.AMS.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepo;
        private readonly ITeacherRepository _teacherRepo;

        public CourseService(
            ICourseRepository courseRepo,
            ITeacherRepository teacherRepo)
        {
            _courseRepo = courseRepo;
            _teacherRepo = teacherRepo;
        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            return await _courseRepo.GetAllAsync();
        }

        public async Task<Course?> GetCourseByIdAsync(int id)
        {
            return await _courseRepo.GetByIdAsync(id);
        }

        public async Task<(bool Success, string Message, Course? Data)> CreateCourseAsync(CourseDTO dto)
        {
            // Rule 1 — Course name must not be empty
            if (string.IsNullOrWhiteSpace(dto.CourseName))
                return (false, "Course name is required.", null);

            // Rule 2 — Teacher must exist
            var teacher = await _teacherRepo.GetByIdAsync(dto.TeacherId);
            if (teacher == null)
                return (false, "Teacher not found.", null);

            // All rules passed — create course
            var course = await _courseRepo.CreateAsync(dto);
            return (true, "Course created successfully.", course);
        }

        public async Task<(bool Success, string Message, Course? Data)> UpdateCourseAsync(int id, CourseDTO dto)
        {
            // Rule 1 — Course must exist
            var existing = await _courseRepo.GetByIdAsync(id);
            if (existing == null)
                return (false, "Course not found.", null);

            // Rule 2 — Course name must not be empty
            if (string.IsNullOrWhiteSpace(dto.CourseName))
                return (false, "Course name is required.", null);

            // Rule 3 — Teacher must exist
            var teacher = await _teacherRepo.GetByIdAsync(dto.TeacherId);
            if (teacher == null)
                return (false, "Teacher not found.", null);

            // All rules passed — update course
            var updated = await _courseRepo.UpdateAsync(id, dto);
            return (true, "Course updated successfully.", updated);
        }

        public async Task<(bool Success, string Message)> DeleteCourseAsync(int id)
        {
            // Rule 1 — Course must exist
            var existing = await _courseRepo.GetByIdAsync(id);
            if (existing == null)
                return (false, "Course not found.");

            // All rules passed — delete course
            await _courseRepo.DeleteAsync(id);
            return (true, "Course deleted successfully.");
        }
    }
}