using MoysIQPlatform.Shared.Models;
using MoysIQPlatform.Shared.Models.Questions;

namespace MoysIQPlatform.Server.Services.QuestionsServices
{
	public interface IQuestionsServices
	{
		Task<ServiceResponse<List<QuestionWithEmployeeDto>>> GetAllQuestions();
		Task<ServiceResponse<Question>> GetQuestionById(int id);
		Task<ServiceResponse<QuestionWithEmployeeDto>> CreateQuestion(QuestionCreateDto dto);
		Task<ServiceResponse<bool>> UpdateQuestion(int id, Question question);
		Task<ServiceResponse<bool>> DeleteQuestion(int id);

	}
}
