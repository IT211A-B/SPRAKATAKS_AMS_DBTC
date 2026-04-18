using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMS.AMS.Models
{
    public class Class
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public required string Section { get; set; }

        public required string YearLevel { get; set; }

        public int CourseId { get; set; }

        public Course? Course { get; set; }

        public ICollection<Enrollment>? Enrollments { get; set; }

        public ICollection<Attendance>? Attendances { get; set; }
    }
}