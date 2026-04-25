using AMS.AMS.Models;
using AttendanceManagementSystem.DTOs;

namespace AMS.AMS.Repositories
{
    public interface IClassRepository
    {
        Task<IEnumerable<Class>> GetAllAsync();
        Task<Class?> GetByIdAsync(int id);
        Task<Class> CreateAsync(ClassDTO dto);
        Task<Class?> UpdateAsync(int id, ClassDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}