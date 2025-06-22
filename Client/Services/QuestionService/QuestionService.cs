using System.Net.Http.Json;
using MoysIQPlatform.Shared.Models;
using MoysIQPlatform.Shared.Models.Questions;

namespace MoysIQPlatform.Client.Services.QuestionService;

public class QuestionService(HttpClient http) : IQuestionService
{
	public async Task<ServiceResponse<List<QuestionWithEmployeeDto>>> GetAllQuestions()
	{
		var response = await http.GetFromJsonAsync<ServiceResponse<List<QuestionWithEmployeeDto>>>("api/questions");
		return response!;
	}

	public async Task<ServiceResponse<Question>> GetQuestionById(int id)
	{
		var response = await http.GetFromJsonAsync<ServiceResponse<Question>>($"api/questions/{id}");
		return response!;
	}

	public async Task<ServiceResponse<QuestionWithEmployeeDto>> CreateQuestion(QuestionCreateDto dto)
	{
		var response = await http.PostAsJsonAsync("api/questions", dto);
		return await response.Content.ReadFromJsonAsync<ServiceResponse<QuestionWithEmployeeDto>>() ?? new();
	}

	public async Task<ServiceResponse<bool>> DeleteQuestion(int id)
	{
		var response = await http.DeleteAsync($"api/questions/{id}");
		return await response.Content.ReadFromJsonAsync<ServiceResponse<bool>>() ?? new();
	}
}
