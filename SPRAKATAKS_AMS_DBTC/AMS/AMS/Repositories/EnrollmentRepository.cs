using AMS.AMS.Models;
using AttendanceManagementSystem.DTOs;
using Microsoft.EntityFrameworkCore;

namespace AMS.AMS.Repositories
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly AppDbContext _context;

        public EnrollmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Enrollment>> GetAllAsync()
        {
            return await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Class)
                .ToListAsync();
        }

        public async Task<Enrollment?> GetByIdAsync(int id)
        {
            return await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Class)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Enrollment> CreateAsync(EnrollmentDTO dto)
        {
            var enrollment = new Enrollment
            {
                StudentId = dto.StudentId,
                ClassId = dto.ClassId,
                Status = dto.Status,
                DateEnrolled = DateTime.UtcNow
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();
            return enrollment;
        }

        public async Task<Enrollment?> UpdateAsync(int id, EnrollmentDTO dto)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null) return null;

            enrollment.StudentId = dto.StudentId;
            enrollment.ClassId = dto.ClassId;
            enrollment.Status = dto.Status;

            await _context.SaveChangesAsync();
            return enrollment;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null) return false;

            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}