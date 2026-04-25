using AMS.AMS.Models;
using AttendanceManagementSystem.DTOs;

namespace AMS.AMS.Repositories
{
    public interface IEnrollmentRepository
    {
        Task<IEnumerable<Enrollment>> GetAllAsync();
        Task<Enrollment?> GetByIdAsync(int id);
        Task<Enrollment> CreateAsync(EnrollmentDTO dto);
        Task<Enrollment?> UpdateAsync(int id, EnrollmentDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}