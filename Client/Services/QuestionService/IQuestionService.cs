using MoysIQPlatform.Shared.Models;
using MoysIQPlatform.Shared.Models.Questions;

namespace MoysIQPlatform.Client.Services.QuestionService;

public interface IQuestionService
{
	Task<ServiceResponse<List<QuestionWithEmployeeDto>>> GetAllQuestions();
	Task<ServiceResponse<Question>> GetQuestionById(int id);
	Task<ServiceResponse<QuestionWithEmployeeDto>> CreateQuestion(QuestionCreateDto dto);
	Task<ServiceResponse<bool>> DeleteQuestion(int id);
}
