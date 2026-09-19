using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EduHub.API.Data;
using EduHub.API.Models;
using EduHub.API.Models.DTOs;

namespace EduHub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin, Student")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseResponseDto>>> GetCourses()
        {
            var courses = await _context.Courses
                .Select(c => new CourseResponseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Instructor = c.Instructor
                })
                .ToListAsync();

            return Ok(courses);
        }

        [Authorize(Roles = "Student")]
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseResponseDto>> GetCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return NotFound(new { message = "Course not found" });

            return Ok(new CourseResponseDto
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                Instructor = course.Instructor
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<CourseResponseDto>> CreateCourse(CourseCreateDto dto)
        {
            var course = new Course
            {
                Name = dto.Name,
                Description = dto.Description,
                Instructor = dto.Instructor
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            var response = new CourseResponseDto
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                Instructor = course.Instructor
            };

            return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, response);
        }

        
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<CourseResponseDto>> UpdateCourse(
    int id,
    CourseUpdateDto dto)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
                return NotFound(new { message = "Course not found" });

            course.Name = dto.Name;
            course.Description = dto.Description;
            course.Instructor = dto.Instructor;

            await _context.SaveChangesAsync();

            var response = new CourseResponseDto
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                Instructor = course.Instructor
            };

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return NotFound(new { message = "Course not found" });

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return Ok("Course Deleted Successfully");
        }
    }
}
