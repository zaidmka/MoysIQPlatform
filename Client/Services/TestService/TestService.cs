using MoysIQPlatform.Shared.Models;
using MoysIQPlatform.Shared.Models.Questions;
using MoysIQPlatform.Shared.Models.Tests;
using System.Net.Http.Json;

namespace MoysIQPlatform.Client.Services.TestService
{
	public class TestService : ITestService
	{
		private readonly HttpClient _http;

		public TestService(HttpClient http)
		{
			_http = http;
		}

		public async Task<int> CreateTestAsync(CreateTestDto dto)
		{
			var response = await _http.PostAsJsonAsync("api/test/create", dto);
			return await response.Content.ReadFromJsonAsync<int>();
		}

		public async Task<List<TestDto>> GetAvailableTestsAsync()
		{
			return await _http.GetFromJsonAsync<List<TestDto>>("api/test/available")
				?? new List<TestDto>();
		}

		public async Task AttachQuestionsAsync(AttachQuestionsDto dto)
		{
			var response = await _http.PostAsJsonAsync("api/test/attach-questions", dto);
			response.EnsureSuccessStatusCode();

		}

		public async Task<List<QuestionSelection>> GetQuestionsForTestAsync(int testId)
		{
			var response = await _http.GetAsync($"api/test/questions/{testId}");
			return await response.Content.ReadFromJsonAsync<List<QuestionSelection>>();
		}

		public async Task<TestDto> GetTestByIdAsync(int testId)
		{
			var response = await _http.GetAsync($"api/test/{testId}");
			return await response.Content.ReadFromJsonAsync<TestDto>();
		}

		public async Task<List<StudentTestQuestionDto>> GetValidTestQuestionsAsync(int testId)
		{
			var response = await _http.GetFromJsonAsync<ServiceResponse<List<StudentTestQuestionDto>>>(
				$"api/Test/valid/{testId}");

			if (response != null && response.Success)
				return response.Data ?? new List<StudentTestQuestionDto>();
			else
				throw new Exception(response?.Message ?? "Failed to fetch test questions.");
		}

		public async Task<ServiceResponse<List<StudentAnswerDto>>> StudentAnswerSubmit(List<StudentAnswerDto> studentAnswers)
		{
			var response = await _http.PostAsJsonAsync("api/test/submit-answers", studentAnswers);

			var result = await response.Content.ReadFromJsonAsync<ServiceResponse<List<StudentAnswerDto>>>();

			if (response.IsSuccessStatusCode && result != null)
			{
				return result;
			}
			else
			{
				throw new Exception(result?.Message ?? "حدث خطأ أثناء إرسال الإجابات.");
			}
		}

		public async Task<ServiceResponse<bool>> IsTestSubmit(int testId)
		{
			
			return await _http.GetFromJsonAsync<ServiceResponse<bool>>($"api/test/is-submitted/{testId}");
		}

		public async Task<ServiceResponse<List<StudentScore>>> StudentScoreAsync()
		{
			return await _http.GetFromJsonAsync<ServiceResponse<List<StudentScore>>>($"api/test/student-scores/");
		}

		public async Task<ServiceResponse<List<StudentAnswerSnapshot>>> StudentAnswersSnapshots()
		{
			return await _http.GetFromJsonAsync<ServiceResponse<List<StudentAnswerSnapshot>>>($"api/test/student-answers/");
		}
	}
}