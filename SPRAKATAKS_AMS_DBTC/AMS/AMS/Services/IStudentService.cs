using AMS.AMS.Models;
using AttendanceManagementSystem.DTOs;

namespace AMS.AMS.Services
{
    public interface IStudentService
    {
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task<Student?> GetStudentByIdAsync(int id);
        Task<(bool Success, string Message, Student? Data)> CreateStudentAsync(StudentDTO dto);
        Task<(bool Success, string Message, Student? Data)> UpdateStudentAsync(int id, StudentDTO dto);
        Task<(bool Success, string Message)> DeleteStudentAsync(int id);
    }
}