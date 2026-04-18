namespace AttendanceManagementSystem.DTOs
{
    public class EnrollmentDTO
    {
        public int StudentId { get; set; }

        public int ClassId { get; set; }

        // "Enrolled" | "Pending" | "Dropped"
        public string Status { get; set; } = "Enrolled";
    }
}