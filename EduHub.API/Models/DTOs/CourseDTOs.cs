using System.ComponentModel.DataAnnotations;

namespace EduHub.API.Models.DTOs
{
    public class CourseCreateDto
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Instructor { get; set; } = string.Empty;
    }

    public class CourseUpdateDto : CourseCreateDto
    {
    }

    public class CourseResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Instructor { get; set; } = string.Empty;
    }
}
