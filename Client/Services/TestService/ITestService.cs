using MoysIQPlatform.Shared.Models.Tests;

namespace MoysIQPlatform.Client.Services.TestService
{
	public interface ITestService
	{
		Task<int> CreateTestAsync(CreateTestDto dto);
		Task<List<TestDto>> GetAvailableTestsAsync();
		Task AttachQuestionsAsync(AttachQuestionsDto dto);
		Task<List<QuestionSelection>> GetQuestionsForTestAsync(int testId);
		Task<TestDto> GetTestByIdAsync(int testId);
	}
}
