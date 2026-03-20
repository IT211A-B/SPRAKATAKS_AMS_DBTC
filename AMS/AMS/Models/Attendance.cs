using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMS.AMS.Models
{
    public class Attendance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int StudentId { get; set; }

        public Student? Student { get; set; }

        public int ClassId { get; set; }

        public Class? Class { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;

        // "Present" | "Absent" | "Late"
        public string Status { get; set; } = "Present";
    }
}