using AMS.AMS.Models;
using AttendanceManagementSystem.DTOs;

namespace AMS.AMS.Repositories
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetAllAsync();
        Task<Student?> GetByIdAsync(int id);
        Task<Student> CreateAsync(StudentDTO dto);
        Task<Student?> UpdateAsync(int id, StudentDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}