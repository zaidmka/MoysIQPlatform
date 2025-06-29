using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoysIQPlatform.Server.Services.TestService;
using MoysIQPlatform.Shared.Models;
using MoysIQPlatform.Shared.Models.Questions;
using MoysIQPlatform.Shared.Models.Tests;
using System.Security.Claims;

namespace MoysIQPlatform.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TestController : ControllerBase
	{
		private readonly ITestService _testService;

		public TestController(ITestService testService)
		{
			_testService = testService;
		}


		[Authorize]
		[HttpPost("create")]
		public async Task<IActionResult> CreateTest([FromBody] CreateTestDto dto)
		{
			try
			{
				var employeeId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("Missing ID claim."));
				var employeeRole = User.FindFirst(ClaimTypes.Role)?.Value ?? throw new Exception("Missing role claim.");

				var allowedRoles = new[] { "Admin", "Editor" };
				if (!allowedRoles.Any(r => employeeRole.Contains(r)))
					return Forbid("⛔ You are not allowed to create tests.");

				var newId = await _testService.CreateTestAsync(dto, employeeId, employeeRole);
				return Ok(newId);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[Authorize]
		[HttpPost("attach-questions")]
		public async Task<IActionResult> AttachQuestions([FromBody] AttachQuestionsDto dto)
		{
			try
			{
				var employeeId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("Missing ID claim."));
				var employeeRole = User.FindFirst(ClaimTypes.Role)?.Value ?? throw new Exception("Missing role claim.");

				await _testService.AttachQuestionsAsync(dto, employeeId, employeeRole);
				return Ok("Questions attached successfully.");
			}
			catch (UnauthorizedAccessException ex)
			{
				return Forbid(ex.Message);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		[Authorize]
		[HttpGet("available")]
		public async Task<IActionResult> GetAvailableTests()
		{
			try
			{
				var tests = await _testService.GetAvailableTestsAsync();
				return Ok(tests);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		[Authorize]
		[HttpGet("questions/{testId}")]
		public async Task<IActionResult> GetQuestionsForTest(int testId)
		{
			try
			{
				var questions = await _testService.GetQuestionsForTestAsync(testId);
				return Ok(questions);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		[Authorize]
		[HttpGet("{testId}")]
		public async Task<IActionResult> GetTestById(int testId)
		{
			try
			{
				var employeeId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("Missing ID claim."));
				var employeeRole = User.FindFirst(ClaimTypes.Role)?.Value ?? throw new Exception("Missing role claim.");
				var test = await _testService.GetTestByIdAsync(testId, employeeId, employeeRole);
				return Ok(test);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[Authorize]
		[HttpGet("valid/{testId}")]
		public async Task<ActionResult<ServiceResponse<List<StudentAnswerOptionsDto>>>> GetValidTest(int testId)
		{
			try
			{
				var studentId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("Missing ID claim."));
				var validTest = await _testService.GetValidTest(testId,studentId);
				return Ok(validTest);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[Authorize]
		[HttpPost("submit-answers")]
		public async Task<ActionResult<ServiceResponse<List<StudentAnswerDto>>>> SubmitAnswers([FromBody] List<StudentAnswerDto> studentAnswers)
		{
			try
			{
				var response = await _testService.StudentAnswerSubmit(studentAnswers);
				if (response.Success)
					return Ok(response);
				else
					return BadRequest(response);
			}
			catch (Exception ex)
			{
				return BadRequest(new ServiceResponse<List<StudentAnswerDto>>
				{
					Data = null,
					Message = $"❌ Exception: {ex.Message}",
					Success = false
				});
			}
		}

		[Authorize]
		[HttpGet("is-submitted/{testId}")]
		public async Task<ActionResult<ServiceResponse<bool>>> IsTestSubmitted(int testId)
		{
			try
			{
				var studentId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("Missing ID claim."));
				var response = await _testService.IsTestSubmit(testId, studentId);
				return Ok(response);
			}
			catch (Exception ex)
			{
				return BadRequest(new ServiceResponse<bool>
				{
					Data = false,
					Message = $"❌ Exception: {ex.Message}",
					Success = false
				});
			}
		}

		[Authorize]
		[HttpGet("student-scores")]
		public async Task<ActionResult<ServiceResponse<List<StudentScore>>>> GetStudentScores()
		{
			try
			{
				var studentId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("Missing ID claim."));

				var response = await _testService.GetStudentScoreAsync(studentId);
				return Ok(response);
			}
			catch (Exception ex)
			{
				return BadRequest(new ServiceResponse<List<StudentScore>>
				{
					Data = null,
					Message = $"❌ Exception: {ex.Message}",
					Success = false
				});
			}
		}

		[Authorize]
		[HttpGet("student-answers")]
		public async Task<ActionResult<ServiceResponse<List<StudentAnswerSnapshot>>>> GetStudentAnswerSnapshot()
		{
			try
			{
				var studentId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("Missing ID claim."));

				var response = await _testService.GetStudentAnswerSnapshotAsync(studentId);
				return Ok(response);
			}
			catch (Exception ex)
			{
				return BadRequest(new ServiceResponse<List<StudentAnswerSnapshot>>
				{
					Data = null,
					Message = $"❌ Exception: {ex.Message}",
					Success = false
				});
			}
		}



	}
}
