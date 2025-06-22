using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MoysIQPlatform.Server.Data;
using MoysIQPlatform.Shared.Models;
using MoysIQPlatform.Shared.Models.Accounts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MoysIQPlatform.Server.Services.EmployeeService
{
	public class EmployeeService : IEmployeeService
	{
		private readonly DataContext _dataContext;
		private readonly IConfiguration _configuration;

		public EmployeeService(DataContext dataContext, IConfiguration configuration)
		{
			_dataContext = dataContext;
			_configuration = configuration;
		}

		// Employee registration
		public async Task<ServiceResponse<UserDto>> EmployeeRegister(Employee employee, string password)
		{
			if (await EmployeeExists(employee.Email))
			{
				return new ServiceResponse<UserDto>
				{
					Success = false,
					Message = "Employee already exists!"
				};
			}

			var hasher = new PasswordHasher<Employee>();
			employee.PasswordHash = hasher.HashPassword(employee, password);
			employee.CreatedAt = DateTime.UtcNow;

			_dataContext.Employees.Add(employee);
			await _dataContext.SaveChangesAsync();

			return new ServiceResponse<UserDto>
			{
				Success = true,
				Message = "Employee registered successfully.",
				Data = new UserDto
				{
					Id = employee.Id,
					Email = employee.Email,
					FullName = employee.FullName,
					Role = employee.Role
				}
			};
		}

		// Employee login
		public async Task<ServiceResponse<string>> Login(string email, string password)
		{
			var employee = await _dataContext.Employees
				.FirstOrDefaultAsync(e => e.Email.ToLower() == email.ToLower());

			if (employee == null)
			{
				return new ServiceResponse<string>
				{
					Success = false,
					Message = "Employee not found."
				};
			}

			var hasher = new PasswordHasher<Employee>();
			var result = hasher.VerifyHashedPassword(employee, employee.PasswordHash, password);

			if (result == PasswordVerificationResult.Failed)
			{
				return new ServiceResponse<string>
				{
					Success = false,
					Message = "Incorrect password."
				};
			}

			string token = CreateToken(employee);
			return new ServiceResponse<string>
			{
				Success = true,
				Data = token,
				Message = "Login successful."
			};
		}

		// Check if employee email exists
		public async Task<bool> EmployeeExists(string email)
		{
			return await _dataContext.Employees.AnyAsync(e =>
				e.Email.ToLower() == email.ToLower());
		}

		// Generate JWT token
		private string CreateToken(Employee employee)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, employee.Id.ToString()),
				new Claim(ClaimTypes.Email, employee.Email),
				new Claim(ClaimTypes.Name, employee.FullName),
				new Claim(ClaimTypes.Role, employee.Role),
				new Claim("is_approved", employee.IsApproved.ToString().ToLower()) // ✅ custom claim

			};

			// Load JWT secrets from environment via IConfiguration
			var secret = Environment.GetEnvironmentVariable("JWT_SECRET")!;
			var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER")!;
			var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")!;

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

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
