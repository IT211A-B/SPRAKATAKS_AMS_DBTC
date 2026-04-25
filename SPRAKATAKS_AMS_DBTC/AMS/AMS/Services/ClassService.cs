using AMS.AMS.Models;
using AMS.AMS.Repositories;
using AttendanceManagementSystem.DTOs;

namespace AMS.AMS.Services
{
    public class ClassService : IClassService
    {
        private readonly IClassRepository _classRepo;
        private readonly ICourseRepository _courseRepo;

        public ClassService(
            IClassRepository classRepo,
            ICourseRepository courseRepo)
        {
            _classRepo = classRepo;
            _courseRepo = courseRepo;
        }

        public async Task<IEnumerable<Class>> GetAllClassesAsync()
        {
            return await _classRepo.GetAllAsync();
        }

        public async Task<Class?> GetClassByIdAsync(int id)
        {
            return await _classRepo.GetByIdAsync(id);
        }

        public async Task<(bool Success, string Message, Class? Data)> CreateClassAsync(ClassDTO dto)
        {
            // Rule 1 — Section must not be empty
            if (string.IsNullOrWhiteSpace(dto.Section))
                return (false, "Section is required.", null);

            // Rule 2 — Year level must not be empty
            if (string.IsNullOrWhiteSpace(dto.YearLevel))
                return (false, "Year level is required.", null);

            // Rule 3 — Course must exist
            var course = await _courseRepo.GetByIdAsync(dto.CourseId);
            if (course == null)
                return (false, "Course not found.", null);

            // All rules passed — create class
            var cls = await _classRepo.CreateAsync(dto);
            return (true, "Class created successfully.", cls);
        }

        public async Task<(bool Success, string Message, Class? Data)> UpdateClassAsync(int id, ClassDTO dto)
        {
            // Rule 1 — Class must exist
            var existing = await _classRepo.GetByIdAsync(id);
            if (existing == null)
                return (false, "Class not found.", null);

            // Rule 2 — Section must not be empty
            if (string.IsNullOrWhiteSpace(dto.Section))
                return (false, "Section is required.", null);

            // Rule 3 — Year level must not be empty
            if (string.IsNullOrWhiteSpace(dto.YearLevel))
                return (false, "Year level is required.", null);

            // Rule 4 — Course must exist
            var course = await _courseRepo.GetByIdAsync(dto.CourseId);
            if (course == null)
                return (false, "Course not found.", null);

            // All rules passed — update class
            var updated = await _classRepo.UpdateAsync(id, dto);
            return (true, "Class updated successfully.", updated);
        }

        public async Task<(bool Success, string Message)> DeleteClassAsync(int id)
        {
            // Rule 1 — Class must exist
            var existing = await _classRepo.GetByIdAsync(id);
            if (existing == null)
                return (false, "Class not found.");

            // All rules passed — delete class
            await _classRepo.DeleteAsync(id);
            return (true, "Class deleted successfully.");
        }
    }
}