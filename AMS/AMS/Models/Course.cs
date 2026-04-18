using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMS.AMS.Models { 
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public required string CourseName { get; set; }

        public int TeacherId { get; set; }

        public Teacher? Teacher { get; set; }

        public ICollection<Student>? Students { get; set; }
    }
}