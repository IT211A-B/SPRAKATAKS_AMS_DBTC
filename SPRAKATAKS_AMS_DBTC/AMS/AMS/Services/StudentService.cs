using AMS.AMS.Models;
using AMS.AMS.Repositories;
using AttendanceManagementSystem.DTOs;

namespace AMS.AMS.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepo;
        private readonly ICourseRepository _courseRepo;

        public StudentService(
            IStudentRepository studentRepo,
            ICourseRepository courseRepo)
        {
            _studentRepo = studentRepo;
            _courseRepo = courseRepo;
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await _studentRepo.GetAllAsync();
        }

        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            return await _studentRepo.GetByIdAsync(id);
        }

        public async Task<(bool Success, string Message, Student? Data)> CreateStudentAsync(StudentDTO dto)
        {
            // Rule 1 — First name and last name must not be empty
            if (string.IsNullOrWhiteSpace(dto.FirstName) ||
                string.IsNullOrWhiteSpace(dto.LastName))
                return (false, "First name and last name are required.", null);

            // Rule 2 — Email must not be empty
            if (string.IsNullOrWhiteSpace(dto.Email))
                return (false, "Email is required.", null);

            // Rule 3 — Course must exist
            var course = await _courseRepo.GetByIdAsync(dto.CourseId);
            if (course == null)
                return (false, "Course not found.", null);

            // All rules passed — create student
            var student = await _studentRepo.CreateAsync(dto);
            return (true, "Student created successfully.", student);
        }

        public async Task<(bool Success, string Message, Student? Data)> UpdateStudentAsync(int id, StudentDTO dto)
        {
            // Rule 1 — Student must exist
            var existing = await _studentRepo.GetByIdAsync(id);
            if (existing == null)
                return (false, "Student not found.", null);

            // Rule 2 — First name and last name must not be empty
            if (string.IsNullOrWhiteSpace(dto.FirstName) ||
                string.IsNullOrWhiteSpace(dto.LastName))
                return (false, "First name and last name are required.", null);

            // Rule 3 — Email must not be empty
            if (string.IsNullOrWhiteSpace(dto.Email))
                return (false, "Email is required.", null);

            // Rule 4 — Course must exist
            var course = await _courseRepo.GetByIdAsync(dto.CourseId);
            if (course == null)
                return (false, "Course not found.", null);

            // All rules passed — update student
            var updated = await _studentRepo.UpdateAsync(id, dto);
            return (true, "Student updated successfully.", updated);
        }

        public async Task<(bool Success, string Message)> DeleteStudentAsync(int id)
        {
            // Rule 1 — Student must exist
            var existing = await _studentRepo.GetByIdAsync(id);
            if (existing == null)
                return (false, "Student not found.");

            // All rules passed — delete student
            await _studentRepo.DeleteAsync(id);
            return (true, "Student deleted successfully.");
        }
    }
}