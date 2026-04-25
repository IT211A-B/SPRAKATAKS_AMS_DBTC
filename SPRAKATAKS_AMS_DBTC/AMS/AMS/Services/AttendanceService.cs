using AMS.AMS.Models;
using AMS.AMS.Repositories;
using AttendanceManagementSystem.DTOs;

namespace AMS.AMS.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepo;
        private readonly IStudentRepository _studentRepo;
        private readonly IClassRepository _classRepo;

        public AttendanceService(
            IAttendanceRepository attendanceRepo,
            IStudentRepository studentRepo,
            IClassRepository classRepo)
        {
            _attendanceRepo = attendanceRepo;
            _studentRepo = studentRepo;
            _classRepo = classRepo;
        }

        public async Task<IEnumerable<Attendance>> GetAllAttendanceAsync()
        {
            return await _attendanceRepo.GetAllAsync();
        }

        public async Task<Attendance?> GetAttendanceByIdAsync(int id)
        {
            return await _attendanceRepo.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Attendance>> GetAttendanceByClassAsync(int classId)
        {
            return await _attendanceRepo.GetByClassAsync(classId);
        }

        public async Task<(bool Success, string Message, Attendance? Data)> CreateAttendanceAsync(AttendanceDTO dto)
        {
            // Rule 1 — Student must exist
            var student = await _studentRepo.GetByIdAsync(dto.StudentId);
            if (student == null)
                return (false, "Student not found.", null);

            // Rule 2 — Class must exist
            var cls = await _classRepo.GetByIdAsync(dto.ClassId);
            if (cls == null)
                return (false, "Class not found.", null);

            // Rule 3 — Date cannot be in the future
            if (dto.Date.Date > DateTime.UtcNow.Date)
                return (false, "Attendance date cannot be in the future.", null);

            // Rule 4 — Status must be valid
            var validStatuses = new[] { "Present", "Absent", "Late" };
            if (!validStatuses.Contains(dto.Status))
                return (false, "Status must be Present, Absent, or Late.", null);

            // Rule 5 — Attendance cannot be marked twice for the same student in the same class on the same day
            var all = await _attendanceRepo.GetAllAsync();
            bool alreadyMarked = all.Any(a =>
                a.StudentId == dto.StudentId &&
                a.ClassId == dto.ClassId &&
                a.Date.Date == dto.Date.Date);
            if (alreadyMarked)
                return (false, "Attendance already marked for this student in this class on this date.", null);

            // All rules passed — create attendance
            var attendance = await _attendanceRepo.CreateAsync(dto);
            return (true, "Attendance marked successfully.", attendance);
        }

        public async Task<(bool Success, string Message, Attendance? Data)> UpdateAttendanceAsync(int id, AttendanceDTO dto)
        {
            // Rule 1 — Attendance must exist
            var existing = await _attendanceRepo.GetByIdAsync(id);
            if (existing == null)
                return (false, "Attendance record not found.", null);

            // Rule 2 — Student must exist
            var student = await _studentRepo.GetByIdAsync(dto.StudentId);
            if (student == null)
                return (false, "Student not found.", null);

            // Rule 3 — Class must exist
            var cls = await _classRepo.GetByIdAsync(dto.ClassId);
            if (cls == null)
                return (false, "Class not found.", null);

            // Rule 4 — Status must be valid
            var validStatuses = new[] { "Present", "Absent", "Late" };
            if (!validStatuses.Contains(dto.Status))
                return (false, "Status must be Present, Absent, or Late.", null);

            // All rules passed — update attendance
            var updated = await _attendanceRepo.UpdateAsync(id, dto);
            return (true, "Attendance updated successfully.", updated);
        }

        public async Task<(bool Success, string Message)> DeleteAttendanceAsync(int id)
        {
            // Rule 1 — Attendance must exist
            var existing = await _attendanceRepo.GetByIdAsync(id);
            if (existing == null)
                return (false, "Attendance record not found.");

            // All rules passed — delete attendance
            await _attendanceRepo.DeleteAsync(id);
            return (true, "Attendance deleted successfully.");
        }
    }
}