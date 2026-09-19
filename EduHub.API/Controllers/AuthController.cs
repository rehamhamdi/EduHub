using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using EduHub.API.Data;
using EduHub.API.Models;
using EduHub.API.Models.DTOs;

namespace EduHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private const string JwtKey = "jktgfkldkfldkg123456789012345678";
        private const int JwtDurationMinutes = 120;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("SignUp")]
        public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto dto)
        {
            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = dto.Password,
                Role = "Student"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var student = new Student
            {
                Name = dto.Name,
                Email = dto.Email,
                University = dto.University,
                UserId = user.Id
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            var response = new AuthResponseDto
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role.ToString(),
            };

            return Ok(response);
        }

        [HttpPost("LogIn")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
        {
            var user = _context.Users
                .FirstOrDefault(x =>
                    x.Email == dto.Email &&
                    x.Password == dto.Password);

            if (user == null)
            {
                return Unauthorized("Invalid Email Or Password");
            }

            var token = CreateToken(user);

            var response = new AuthResponseDto
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role.ToString(),
                Token = token
            };

            return Ok(response);
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(
                    JwtRegisteredClaimNames.Sub,
                    user.Id.ToString()),

                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.Id.ToString()),

                new Claim(
                    ClaimTypes.Name,
                    user.Name),

                new Claim(
                    ClaimTypes.Role,
                    user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(JwtKey));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(JwtDurationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}