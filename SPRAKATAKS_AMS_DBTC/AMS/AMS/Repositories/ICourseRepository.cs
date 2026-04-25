using AMS.AMS.Models;
using AttendanceManagementSystem.DTOs;

namespace AMS.AMS.Repositories
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllAsync();
        Task<Course?> GetByIdAsync(int id);
        Task<Course> CreateAsync(CourseDTO dto);
        Task<Course?> UpdateAsync(int id, CourseDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}