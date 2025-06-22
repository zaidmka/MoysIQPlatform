using System.Net.Http.Json;
using MoysIQPlatform.Shared.Models.Tests;

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
	}
}