using AMS.AMS.Models;
using AMS.AMS.Repositories;
using AttendanceManagementSystem.DTOs;

namespace AMS.AMS.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository _teacherRepo;

        public TeacherService(ITeacherRepository teacherRepo)
        {
            _teacherRepo = teacherRepo;
        }

        public async Task<IEnumerable<Teacher>> GetAllTeachersAsync()
        {
            return await _teacherRepo.GetAllAsync();
        }

        public async Task<Teacher?> GetTeacherByIdAsync(int id)
        {
            return await _teacherRepo.GetByIdAsync(id);
        }

        public async Task<(bool Success, string Message, Teacher? Data)> CreateTeacherAsync(TeacherDTO dto)
        {
            // Rule 1 — First name and last name must not be empty
            if (string.IsNullOrWhiteSpace(dto.FirstName) ||
                string.IsNullOrWhiteSpace(dto.LastName))
                return (false, "First name and last name are required.", null);

            // Rule 2 — Email must not be empty
            if (string.IsNullOrWhiteSpace(dto.Email))
                return (false, "Email is required.", null);

            // All rules passed — create teacher
            var teacher = await _teacherRepo.CreateAsync(dto);
            return (true, "Teacher created successfully.", teacher);
        }

        public async Task<(bool Success, string Message, Teacher? Data)> UpdateTeacherAsync(int id, TeacherDTO dto)
        {
            // Rule 1 — Teacher must exist
            var existing = await _teacherRepo.GetByIdAsync(id);
            if (existing == null)
                return (false, "Teacher not found.", null);

            // Rule 2 — First name and last name must not be empty
            if (string.IsNullOrWhiteSpace(dto.FirstName) ||
                string.IsNullOrWhiteSpace(dto.LastName))
                return (false, "First name and last name are required.", null);

            // Rule 3 — Email must not be empty
            if (string.IsNullOrWhiteSpace(dto.Email))
                return (false, "Email is required.", null);

            // All rules passed — update teacher
            var updated = await _teacherRepo.UpdateAsync(id, dto);
            return (true, "Teacher updated successfully.", updated);
        }

        public async Task<(bool Success, string Message)> DeleteTeacherAsync(int id)
        {
            // Rule 1 — Teacher must exist
            var existing = await _teacherRepo.GetByIdAsync(id);
            if (existing == null)
                return (false, "Teacher not found.");

            // All rules passed — delete teacher
            await _teacherRepo.DeleteAsync(id);
            return (true, "Teacher deleted successfully.");
        }
    }
}