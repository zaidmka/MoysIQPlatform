using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MoysIQPlatform.Server.Data;
using MoysIQPlatform.Shared.Models;
using MoysIQPlatform.Shared.Models.Accounts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MoysIQPlatform.Server.Services.StudentService
{
	public class StudentService : IStudentService
	{
		private readonly DataContext _context;

		public StudentService(DataContext context)
		{
			_context = context;
		}

		public async Task<ServiceResponse<string>> Register(Student student, string password)
		{
			if (await StudentExists(student.Email))
			{
				return new ServiceResponse<string>
				{
					Success = false,
					Message = "Student already exists."
				};
			}

			var hasher = new PasswordHasher<Student>();
			student.PasswordHash = hasher.HashPassword(student, password);
			student.CreatedAt = DateTime.UtcNow;

			_context.Students.Add(student);
			await _context.SaveChangesAsync();

			var token = CreateToken(student);

			return new ServiceResponse<string>
			{
				Success = true,
				Message = "Student registered successfully.",
				Data = token
			};
		}

		public async Task<ServiceResponse<string>> Login(string email, string password)
		{
			var student = await _context.Students.FirstOrDefaultAsync(s => s.Email.ToLower() == email.ToLower());

			if (student == null)
				return new ServiceResponse<string> { Success = false, Message = "Student not found." };

			var hasher = new PasswordHasher<Student>();
			var result = hasher.VerifyHashedPassword(student, student.PasswordHash, password);

			if (result == PasswordVerificationResult.Failed)
				return new ServiceResponse<string> { Success = false, Message = "Invalid password." };

			var token = CreateToken(student);

			return new ServiceResponse<string>
			{
				Success = true,
				Data = token,
				Message = "Login successful."
			};
		}

		public async Task<bool> StudentExists(string email)
		{
			return await _context.Students.AnyAsync(s => s.Email.ToLower() == email.ToLower());
		}

		private string CreateToken(Student student)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, student.Id.ToString()),
				new Claim(ClaimTypes.Email, student.Email),
				new Claim(ClaimTypes.Name, student.FullName),
				new Claim(ClaimTypes.Role, "Student"), // Fixed role
				new Claim("is_approved", student.IsApproved.ToString().ToLower()) // ✅ custom claim

			};

			var secret = Environment.GetEnvironmentVariable("JWT_SECRET")!;
			var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER")!;
			var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")!;

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

			var token = new JwtSecurityToken(
				issuer: issuer,
				audience: audience,
				claims: claims,
				expires: DateTime.UtcNow.AddDays(1),
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
