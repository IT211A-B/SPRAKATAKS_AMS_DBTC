using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMS.AMS.Models
{
    public class Enrollment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int StudentId { get; set; }

        public Student? Student { get; set; }

        public int ClassId { get; set; }

        public Class? Class { get; set; }

        public DateTime DateEnrolled { get; set; } = DateTime.UtcNow;

        public string Status { get; set; } = "Enrolled";
    }
}