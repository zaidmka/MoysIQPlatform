using MoysIQPlatform.Server.Data;
using MoysIQPlatform.Shared.Models.Tests;
using System.Data;

namespace MoysIQPlatform.Server.Services.TestService
{
	public class TestService : ITestService
	{
		private readonly DataContext _context;

		public TestService(DataContext context)
		{
			_context = context;
		}

		public async Task AttachQuestionsAsync(AttachQuestionsDto dto, int employeeId, string role)
		{

			var allowedRoles = new[] { "Admin", "Editor" };
			if (!allowedRoles.Contains(role))
				throw new UnauthorizedAccessException("You are not authorized to attach questions.");

			var test = await _context.Tests
				.Include(t => t.TestQuestions)
				.FirstOrDefaultAsync(t => t.Id == dto.TestId);

			if (test == null)
				throw new Exception("Test not found.");

			// Delete existing questions 
			_context.TestQuestions.RemoveRange(test.TestQuestions);

			foreach (var q in dto.Questions)
			{
				test.TestQuestions.Add(new TestQuestion
				{
					QuestionId = q.QuestionId,
					IsMandatory = q.IsMandatory
				});
			}

			await _context.SaveChangesAsync();
		}



		public async Task<int> CreateTestAsync(CreateTestDto dto, int employeeId, string employeeRole)

		{
			var employee = await _context.Employees
				.FirstOrDefaultAsync(e => e.Id == employeeId);

			if (employee == null)
				throw new Exception("Employee not found.");

			var allowedRoles = new[] { "Editor", "Admin" };

			if (!allowedRoles.Contains(employee.Role))
				throw new UnauthorizedAccessException("You are not authorized to create tests.");

			var test = new Test
			{
				Title = dto.Title,
				StartTime = dto.StartTime,
				EndTime = dto.EndTime,
				CreatedByEmployeeId = employeeId
			};

			_context.Tests.Add(test);
			await _context.SaveChangesAsync();
			return test.Id;
		}

		public async Task<List<TestDto>> GetAvailableTestsAsync()
		{
			try
			{
				var now = DateTime.UtcNow;

				var tests = await _context.Tests
					.Where(t => t.StartTime <= now && t.EndTime > now)
					.Select(t => new TestDto
					{
						Id = t.Id,
						Title = t.Title,
						StartTime = t.StartTime,
						EndTime = t.EndTime
					})
					.ToListAsync();
				if (tests == null || !tests.Any())
					return new List<TestDto>();

				return (tests);

			}
			catch (Exception ex)
			{
				// Log the exception (not shown here)
				throw new Exception("An error occurred while retrieving available tests.", ex);
			}
		}

		public async Task<List<QuestionSelection>> GetQuestionsForTestAsync(int testId)
		{
			var testQuestions = await _context.TestQuestions
				.Where(tq => tq.TestId == testId)
				.Select(tq => new QuestionSelection
				{
					QuestionId = tq.QuestionId,
					IsMandatory = tq.IsMandatory
				})
				.ToListAsync();

			return testQuestions;
		}

		public async Task<TestDto> GetTestByIdAsync(int testId, int employeeId, string employeeRole)
		{
			var allowedRoles = new[] { "Admin", "Editor" };
			if (!allowedRoles.Contains(employeeRole))
				throw new UnauthorizedAccessException("You are not authorized to attach questions.");

			var test = await _context.Tests
				.Where(t => t.Id == testId)
				.FirstOrDefaultAsync();
			if (test == null)
				throw new Exception("Test not found.");
			return new TestDto
			{
				Id = test.Id,
				Title = test.Title,
				StartTime = test.StartTime,
				EndTime = test.EndTime
			};

		}
	}
}
