using MoysIQPlatform.Shared.Models;
using MoysIQPlatform.Shared.Models.Questions;
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
		Task<List<StudentTestQuestionDto>> GetValidTestQuestionsAsync(int testId);
		Task<ServiceResponse<List<StudentAnswerDto>>> StudentAnswerSubmit(List<StudentAnswerDto> studentAnswers);
		Task<ServiceResponse<bool>> IsTestSubmit(int testId);


	}
}
