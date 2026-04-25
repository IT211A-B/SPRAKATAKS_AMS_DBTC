using AMS.AMS.Models;
using AttendanceManagementSystem.DTOs;

namespace AMS.AMS.Services
{
    public interface IEnrollmentService
    {
        Task<IEnumerable<Enrollment>> GetAllEnrollmentsAsync();
        Task<Enrollment?> GetEnrollmentByIdAsync(int id);
        Task<(bool Success, string Message, Enrollment? Data)> CreateEnrollmentAsync(EnrollmentDTO dto);
        Task<(bool Success, string Message, Enrollment? Data)> UpdateEnrollmentAsync(int id, EnrollmentDTO dto);
        Task<(bool Success, string Message)> DeleteEnrollmentAsync(int id);
    }
}