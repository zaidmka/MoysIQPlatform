using GzMagnet.Client;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using MoysIQPlatform.Shared.Models;
using MoysIQPlatform.Shared.Models.Accounts;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace MoysIQPlatform.Client.Services.EmployeeService;

public class EmployeeService(
	HttpClient http,
	IJSRuntime js,
	AuthenticationStateProvider authProvider
) : IEmployeeService
{
	private readonly CustomAuthStateProvider _authProvider = (CustomAuthStateProvider)authProvider;

	public async Task<ServiceResponse<UserDto>> Register(EmployeeRegister request)
	{
		var response = await http.PostAsJsonAsync("api/employee/register", request);
		var result = await response.Content.ReadFromJsonAsync<ServiceResponse<UserDto>>();

		if (result?.Success == true && result.Data is not null)
		{
			await js.InvokeVoidAsync("sessionStorage.setItem", "authToken", result.Data);
			await _authProvider.GetAuthenticationStateAsync();
		}

		return result!;
	}

	public async Task<ServiceResponse<string>> Login(EmployeeLogin request)
	{
		var response = await http.PostAsJsonAsync("api/employee/login", request);
		var result = await response.Content.ReadFromJsonAsync<ServiceResponse<string>>();

		if (result?.Success == true && result.Data is not null)
		{
			await js.InvokeVoidAsync("sessionStorage.setItem", "authToken", result.Data);
			await _authProvider.GetAuthenticationStateAsync();
			http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Data);
		}

		return result!;
	}

	public async Task Logout()
	{
		await js.InvokeVoidAsync("sessionStorage.removeItem", "authToken");
		http.DefaultRequestHeaders.Authorization = null;
		await _authProvider.GetAuthenticationStateAsync();
	}
}
