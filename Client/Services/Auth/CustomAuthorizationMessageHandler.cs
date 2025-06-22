using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http.Headers;

namespace MoysIQPlatform.Client.Services.Auth
{
	public class CustomAuthorizationMessageHandler : DelegatingHandler
	{
		private readonly IJSRuntime _jsRuntime;
		private readonly NavigationManager _navigation;

		public CustomAuthorizationMessageHandler(IJSRuntime jsRuntime, NavigationManager navigation)
		{
			_jsRuntime = jsRuntime;
			_navigation = navigation;
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			string? token = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "authToken");

			if (!string.IsNullOrWhiteSpace(token))
			{
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
			}
			else
			{
				_navigation.NavigateTo("login");
			}

			return await base.SendAsync(request, cancellationToken);
		}
	}
}
