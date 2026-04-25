using AMS.AMS.Models;
using AttendanceManagementSystem.DTOs;

namespace AMS.AMS.Repositories
{
    public interface ITeacherRepository
    {
        Task<IEnumerable<Teacher>> GetAllAsync();
        Task<Teacher?> GetByIdAsync(int id);
        Task<Teacher> CreateAsync(TeacherDTO dto);
        Task<Teacher?> UpdateAsync(int id, TeacherDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}