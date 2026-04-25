using AMS.AMS.Models;
using AttendanceManagementSystem.DTOs;

namespace AMS.AMS.Repositories
{
    public interface IAttendanceRepository
    {
        Task<IEnumerable<Attendance>> GetAllAsync();
        Task<Attendance?> GetByIdAsync(int id);
        Task<IEnumerable<Attendance>> GetByClassAsync(int classId);
        Task<Attendance> CreateAsync(AttendanceDTO dto);
        Task<Attendance?> UpdateAsync(int id, AttendanceDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}