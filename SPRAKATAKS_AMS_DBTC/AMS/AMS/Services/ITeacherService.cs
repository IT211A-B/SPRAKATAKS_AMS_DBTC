using AMS.AMS.Models;
using AttendanceManagementSystem.DTOs;

namespace AMS.AMS.Services
{
    public interface ITeacherService
    {
        Task<IEnumerable<Teacher>> GetAllTeachersAsync();
        Task<Teacher?> GetTeacherByIdAsync(int id);
        Task<(bool Success, string Message, Teacher? Data)> CreateTeacherAsync(TeacherDTO dto);
        Task<(bool Success, string Message, Teacher? Data)> UpdateTeacherAsync(int id, TeacherDTO dto);
        Task<(bool Success, string Message)> DeleteTeacherAsync(int id);
    }
}