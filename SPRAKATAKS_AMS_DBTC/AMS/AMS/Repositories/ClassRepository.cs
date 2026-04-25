using AMS.AMS.Models;
using AttendanceManagementSystem.DTOs;
using Microsoft.EntityFrameworkCore;

namespace AMS.AMS.Repositories
{
    public class ClassRepository : IClassRepository
    {
        private readonly AppDbContext _context;

        public ClassRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Class>> GetAllAsync()
        {
            return await _context.Classes
                .Include(c => c.Course)
                .Include(c => c.Enrollments)
                .Include(c => c.Attendances)
                .ToListAsync();
        }

        public async Task<Class?> GetByIdAsync(int id)
        {
            return await _context.Classes
                .Include(c => c.Course)
                .Include(c => c.Enrollments)
                .Include(c => c.Attendances)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Class> CreateAsync(ClassDTO dto)
        {
            var cls = new Class
            {
                Section = dto.Section,
                YearLevel = dto.YearLevel,
                CourseId = dto.CourseId
            };

            _context.Classes.Add(cls);
            await _context.SaveChangesAsync();
            return cls;
        }

        public async Task<Class?> UpdateAsync(int id, ClassDTO dto)
        {
            var cls = await _context.Classes.FindAsync(id);
            if (cls == null) return null;

            cls.Section = dto.Section;
            cls.YearLevel = dto.YearLevel;
            cls.CourseId = dto.CourseId;

            await _context.SaveChangesAsync();
            return cls;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var cls = await _context.Classes.FindAsync(id);
            if (cls == null) return false;

            _context.Classes.Remove(cls);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}