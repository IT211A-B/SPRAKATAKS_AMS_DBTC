namespace AttendanceManagementSystem.DTOs
{
    public class AttendanceDTO
    {
        public int StudentId { get; set; }

        public int ClassId { get; set; }

        public DateTime Date { get; set; }

        // "Present" | "Absent" | "Late"
        public string Status { get; set; } = "Present";
    }
}