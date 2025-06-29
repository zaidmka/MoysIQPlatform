using MoysIQPlatform.Shared.Models;
using MoysIQPlatform.Shared.Models.Questions;
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
		Task<ServiceResponse<List<StudentTestQuestionDto>>> GetValidTest(int testId, int studentId);
		Task<ServiceResponse<List<StudentAnswerDto>>> StudentAnswerSubmit(List<StudentAnswerDto> studentAnswers);
		Task<ServiceResponse<bool>> IsTestSubmit(int testId, int studentId);
		Task<ServiceResponse<List<StudentScore>>> GetStudentScoreAsync(int studentId);
		Task<ServiceResponse<List<StudentAnswerSnapshot>>> GetStudentAnswerSnapshotAsync(int studentId);


	}
}
