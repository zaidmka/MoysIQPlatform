using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoysIQPlatform.Shared.Models.Accounts;
using Microsoft.AspNetCore.Authorization;
using MoysIQPlatform.Server.Services.EmployeeService;
using MoysIQPlatform.Shared.Models;

namespace MoysIQPlatform.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EmployeeController : ControllerBase
	{
		private readonly IEmployeeService _employeeService;

		public EmployeeController(IEmployeeService employeeService)
		{
			_employeeService = employeeService;
		}
		[HttpPost("register")]

		public async Task<ActionResult<ServiceResponse<int>>> EmployeeRegister(EmployeeRegister request)
		{
			var response = await _employeeService.EmployeeRegister(new Employee
			{
				Email = request.Email,
				FullName = request.FullName,
				Department = request.Department,
				Role = request.Role,
				WorkLocation = request.WorkLocation,
				PhoneNumber = request.PhoneNumber,
				Gender= request.Gender
			},
			request.Password);
			if (!response.Success)
			{
				return BadRequest(response);
			}

			return Ok(response);
		}
	}
}
