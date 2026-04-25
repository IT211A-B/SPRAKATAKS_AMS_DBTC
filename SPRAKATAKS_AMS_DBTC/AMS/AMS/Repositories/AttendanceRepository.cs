using AMS.AMS.Models;
using AttendanceManagementSystem.DTOs;
using Microsoft.EntityFrameworkCore;

namespace AMS.AMS.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly AppDbContext _context;

        public AttendanceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Attendance>> GetAllAsync()
        {
            return await _context.Attendances
                .Include(a => a.Student)
                .Include(a => a.Class)
                .ToListAsync();
        }

        public async Task<Attendance?> GetByIdAsync(int id)
        {
            return await _context.Attendances
                .Include(a => a.Student)
                .Include(a => a.Class)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Attendance>> GetByClassAsync(int classId)
        {
            return await _context.Attendances
                .Include(a => a.Student)
                .Include(a => a.Class)
                .Where(a => a.ClassId == classId)
                .ToListAsync();
        }

        public async Task<Attendance> CreateAsync(AttendanceDTO dto)
        {
            var attendance = new Attendance
            {
                StudentId = dto.StudentId,
                ClassId = dto.ClassId,
                Date = dto.Date,
                Status = dto.Status
            };

            _context.Attendances.Add(attendance);
            await _context.SaveChangesAsync();
            return attendance;
        }

        public async Task<Attendance?> UpdateAsync(int id, AttendanceDTO dto)
        {
            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance == null) return null;

            attendance.StudentId = dto.StudentId;
            attendance.ClassId = dto.ClassId;
            attendance.Date = dto.Date;
            attendance.Status = dto.Status;

            await _context.SaveChangesAsync();
            return attendance;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance == null) return false;

            _context.Attendances.Remove(attendance);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}