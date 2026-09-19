using System.Security.Claims;
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
    [Authorize(Roles = "Student")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        private async Task<Student?> GetCurrentStudentAsync()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
                return null;

            return await _context.Students.FirstOrDefaultAsync(s => s.UserId == userId);
        }

        [HttpPost]
        public async Task<ActionResult<EnrollmentResponseDto>> Enroll(EnrollmentCreateDto dto)
        {
            var student = await GetCurrentStudentAsync();
            if (student == null)
                return Unauthorized(new { message = "Student profile not found" });

            var course = await _context.Courses.FindAsync(dto.CourseId);
            if (course == null)
                return NotFound(new { message = "Course not found" });

            var alreadyEnrolled = await _context.Enrollments
                .AnyAsync(e => e.StudentId == student.Id && e.CourseId == dto.CourseId);
            if (alreadyEnrolled)
                return BadRequest(new { message = "Already enrolled in this course" });

            var enrollment = new Enrollment
            {
                StudentId = student.Id,
                CourseId = dto.CourseId,
                EnrollmentDate = DateTime.UtcNow
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            return Ok(new EnrollmentResponseDto
            {
                Id = enrollment.Id,
                CourseId = course.Id,
                CourseName = course.Name,
                Instructor = course.Instructor,
                EnrollmentDate = enrollment.EnrollmentDate
            });
        }

        [HttpGet("my")]
        public async Task<ActionResult<IEnumerable<EnrollmentResponseDto>>> GetMyEnrollments()
        {
            var student = await GetCurrentStudentAsync();
            if (student == null)
                return Unauthorized(new { message = "Student profile not found" });

            var enrollments = await _context.Enrollments
                .Where(e => e.StudentId == student.Id)
                .Include(e => e.Course)
                .Select(e => new EnrollmentResponseDto
                {
                    Id = e.Id,
                    CourseId = e.CourseId,
                    CourseName = e.Course!.Name,
                    Instructor = e.Course!.Instructor,
                    EnrollmentDate = e.EnrollmentDate
                })
                .ToListAsync();

            return Ok(enrollments);
        }
    }
}
