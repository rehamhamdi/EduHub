namespace EduHub.API.Models.DTOs
{
    public class EnrollmentCreateDto
    {
        public int CourseId { get; set; }
    }

    public class EnrollmentResponseDto
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string Instructor { get; set; } = string.Empty;
        public DateTime EnrollmentDate { get; set; }
    }
}
