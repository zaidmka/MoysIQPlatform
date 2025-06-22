using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoysIQPlatform.Shared.Models;
using MoysIQPlatform.Shared.Models.Accounts;
using MoysIQPlatform.Server.Services.EmployeeService;
using System.Security.Claims;

namespace MoysIQPlatform.Server.Controllers
{
	[Route("api/employee")]
	[ApiController]
	public class EmployeeController(IEmployeeService employeeService) : ControllerBase
	{
		[HttpPost("register")]
		public async Task<ActionResult<ServiceResponse<UserDto>>> Register(EmployeeRegister request)
		{
			var employee = new Employee
			{
				Email = request.Email,
				FullName = request.FullName,
				Department = request.Department,
				Role = request.Role,
				WorkLocation = request.WorkLocation,
				PhoneNumber = request.PhoneNumber,
				Gender = request.Gender
			};

			var result = await employeeService.EmployeeRegister(employee, request.Password);

			if (!result.Success)
				return BadRequest(new { message = result.Message });

			// optional: extract user info from token later
			return Ok(new ServiceResponse<UserDto> { Success = true, Data = result.Data, Message = result.Message });
		}

		[HttpPost("login")]
		public async Task<ActionResult<ServiceResponse<string>>> Login(EmployeeLogin request)
		{
			var response = await employeeService.Login(request.Email, request.Password);

			if (!response.Success)
				return BadRequest(new { message = response.Message });

			return Ok(response);
		}

		[Authorize]
		[HttpGet("me")]
		public IActionResult Me()
		{
			var email = User.FindFirstValue(ClaimTypes.Email);
			var name = User.FindFirstValue(ClaimTypes.Name);
			var role = User.FindFirstValue(ClaimTypes.Role);
			var id = User.FindFirstValue(ClaimTypes.NameIdentifier);

			return Ok(new
			{
				Id = id,
				Email = email,
				Name = name,
				Role = role
			});
		}

		[Authorize(Roles = "Admin")]
		[HttpGet("admin")]
		public IActionResult AdminOnly()
		{
			return Ok(new { message = "You are an admin!" });
		}
	}
}
