using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoysIQPlatform.Server.Services.StudentService;
using MoysIQPlatform.Shared.Models;
using MoysIQPlatform.Shared.Models.Accounts;
using System.Security.Claims;

namespace MoysIQPlatform.Server.Controllers;

[Route("api/student")]
[ApiController]
public class StudentController(IStudentService studentService) : ControllerBase
{
	[HttpPost("register")]
	public async Task<ActionResult<ServiceResponse<string>>> Register(StudentRegister request)
	{
		var student = new Student
		{
			FullName = request.FullName,
			Email = request.Email,
			SchoolName = request.SchoolName,
			Grade = request.Grade,
			Gender = request.Gender,
			BirthDay = request.BirthDay
		};

		var result = await studentService.Register(student, request.Password);

		if (!result.Success)
			return BadRequest(new { result.Message });

		return Ok(result);
	}

	[HttpPost("login")]
	public async Task<ActionResult<ServiceResponse<string>>> Login(StudentLogin request)
	{
		var result = await studentService.Login(request.Email, request.Password);

		if (!result.Success)
			return BadRequest(new { result.Message });

		return Ok(result);
	}

	[Authorize(Roles = "Student")]
	[HttpGet("me")]
	public IActionResult Me()
	{
		var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
		var email = User.FindFirstValue(ClaimTypes.Email);
		var fullName = User.FindFirstValue(ClaimTypes.Name);

		return Ok(new { id, email, fullName });
	}
}
