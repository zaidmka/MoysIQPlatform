using MoysIQPlatform.Shared.Models.Tests;

namespace MoysIQPlatform.Server.Services.TestService
{
	public interface ITestService
	{
		Task<int> CreateTestAsync(CreateTestDto dto, int employeeId, string employeeRole);
		Task AttachQuestionsAsync(AttachQuestionsDto dto, int employeeId, string role);
		Task <List<TestDto>> GetAvailableTestsAsync();
		Task<List<QuestionSelection>> GetQuestionsForTestAsync(int testId);
		Task<TestDto> GetTestByIdAsync(int testId, int employeeId, string employeeRole);

	}
}
