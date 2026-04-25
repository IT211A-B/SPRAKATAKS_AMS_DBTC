using AMS.AMS.Models;
using AMS.AMS.Repositories;
using AttendanceManagementSystem.DTOs;

namespace AMS.AMS.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _enrollmentRepo;
        private readonly IStudentRepository _studentRepo;
        private readonly IClassRepository _classRepo;

        public EnrollmentService(
            IEnrollmentRepository enrollmentRepo,
            IStudentRepository studentRepo,
            IClassRepository classRepo)
        {
            _enrollmentRepo = enrollmentRepo;
            _studentRepo = studentRepo;
            _classRepo = classRepo;
        }

        public async Task<IEnumerable<Enrollment>> GetAllEnrollmentsAsync()
        {
            return await _enrollmentRepo.GetAllAsync();
        }

        public async Task<Enrollment?> GetEnrollmentByIdAsync(int id)
        {
            return await _enrollmentRepo.GetByIdAsync(id);
        }

        public async Task<(bool Success, string Message, Enrollment? Data)> CreateEnrollmentAsync(EnrollmentDTO dto)
        {
            // Rule 1 — Student must exist
            var student = await _studentRepo.GetByIdAsync(dto.StudentId);
            if (student == null)
                return (false, "Student not found.", null);

            // Rule 2 — Class must exist
            var cls = await _classRepo.GetByIdAsync(dto.ClassId);
            if (cls == null)
                return (false, "Class not found.", null);

            // Rule 3 — Student cannot be enrolled in the same class twice
            var all = await _enrollmentRepo.GetAllAsync();
            bool alreadyEnrolled = all.Any(e =>
                e.StudentId == dto.StudentId &&
                e.ClassId == dto.ClassId);
            if (alreadyEnrolled)
                return (false, "Student is already enrolled in this class.", null);

            // All rules passed — create enrollment
            var enrollment = await _enrollmentRepo.CreateAsync(dto);
            return (true, "Student enrolled successfully.", enrollment);
        }

        public async Task<(bool Success, string Message, Enrollment? Data)> UpdateEnrollmentAsync(int id, EnrollmentDTO dto)
        {
            // Rule 1 — Enrollment must exist
            var existing = await _enrollmentRepo.GetByIdAsync(id);
            if (existing == null)
                return (false, "Enrollment not found.", null);

            // Rule 2 — Student must exist
            var student = await _studentRepo.GetByIdAsync(dto.StudentId);
            if (student == null)
                return (false, "Student not found.", null);

            // Rule 3 — Class must exist
            var cls = await _classRepo.GetByIdAsync(dto.ClassId);
            if (cls == null)
                return (false, "Class not found.", null);

            // Rule 4 — Status must be valid
            var validStatuses = new[] { "Enrolled", "Pending", "Dropped" };
            if (!validStatuses.Contains(dto.Status))
                return (false, "Status must be Enrolled, Pending, or Dropped.", null);

            // All rules passed — update enrollment
            var updated = await _enrollmentRepo.UpdateAsync(id, dto);
            return (true, "Enrollment updated successfully.", updated);
        }

        public async Task<(bool Success, string Message)> DeleteEnrollmentAsync(int id)
        {
            // Rule 1 — Enrollment must exist
            var existing = await _enrollmentRepo.GetByIdAsync(id);
            if (existing == null)
                return (false, "Enrollment not found.");

            // All rules passed — delete enrollment
            await _enrollmentRepo.DeleteAsync(id);
            return (true, "Enrollment deleted successfully.");
        }
    }
}