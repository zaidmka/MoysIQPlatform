using MoysIQPlatform.Shared.Models;
using MoysIQPlatform.Shared.Models.Accounts;
using System.Net.Http.Json;

namespace MoysIQPlatform.Client.Services.EmployeeService
{
	public class EmployeeService:IEmployeeService
	{
		private readonly HttpClient _http;

		public EmployeeService(HttpClient http)
		{
			_http = http;

		}

		public async Task<ServiceResponse<bool>> ChangePassword(EmployeeChangePassword request)
		{
			var result = await _http.PostAsJsonAsync("api/employee/change-password", request.Password);
			return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
		}

		public async Task<ServiceResponse<string>> EmployeeLogin(EmployeeLogin request)
		{
			var result = await _http.PostAsJsonAsync("api/employee/login", request);
			return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
		}

		public async Task<ServiceResponse<int>> EmployeeRegister(EmployeeRegister request)
		{
			var result = await _http.PostAsJsonAsync("api/employee/register", request);
			return await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();
		}
	}
}
