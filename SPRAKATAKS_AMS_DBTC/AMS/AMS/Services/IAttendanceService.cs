using AMS.AMS.Models;
using AttendanceManagementSystem.DTOs;

namespace AMS.AMS.Services
{
    public interface IAttendanceService
    {
        Task<IEnumerable<Attendance>> GetAllAttendanceAsync();
        Task<Attendance?> GetAttendanceByIdAsync(int id);
        Task<IEnumerable<Attendance>> GetAttendanceByClassAsync(int classId);
        Task<(bool Success, string Message, Attendance? Data)> CreateAttendanceAsync(AttendanceDTO dto);
        Task<(bool Success, string Message, Attendance? Data)> UpdateAttendanceAsync(int id, AttendanceDTO dto);
        Task<(bool Success, string Message)> DeleteAttendanceAsync(int id);
    }
}