using AMS.AMS.Models;
using AttendanceManagementSystem.DTOs;

namespace AMS.AMS.Services
{
    public interface IClassService
    {
        Task<IEnumerable<Class>> GetAllClassesAsync();
        Task<Class?> GetClassByIdAsync(int id);
        Task<(bool Success, string Message, Class? Data)> CreateClassAsync(ClassDTO dto);
        Task<(bool Success, string Message, Class? Data)> UpdateClassAsync(int id, ClassDTO dto);
        Task<(bool Success, string Message)> DeleteClassAsync(int id);
    }
}