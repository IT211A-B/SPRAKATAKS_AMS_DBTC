using AMS.AMS.Models;
using AttendanceManagementSystem.DTOs;

namespace AMS.AMS.Services
{
    public interface ICourseService
    {
        Task<IEnumerable<Course>> GetAllCoursesAsync();
        Task<Course?> GetCourseByIdAsync(int id);
        Task<(bool Success, string Message, Course? Data)> CreateCourseAsync(CourseDTO dto);
        Task<(bool Success, string Message, Course? Data)> UpdateCourseAsync(int id, CourseDTO dto);
        Task<(bool Success, string Message)> DeleteCourseAsync(int id);
    }
}