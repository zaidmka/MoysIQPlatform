global using MoysIQPlatform.Client.Services.EmployeeService;
global using MoysIQPlatform.Client.Services.QuestionService;
global using MoysIQPlatform.Client.Services.TestService;
global using MoysIQPlatform.Client.Services.StudentService;

using GzMagnet.Client;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using MoysIQPlatform.Client;
using MoysIQPlatform.Client.Services.Auth;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Root Components
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Enable role-based authorization
builder.Services.AddAuthorizationCore();

// Register authentication state provider (JWT)
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

// Register custom auth message handler
builder.Services.AddScoped<CustomAuthorizationMessageHandler>();

// Configure global HttpClient with Authorization handler
builder.Services.AddScoped(sp =>
{
	var jsRuntime = sp.GetRequiredService<IJSRuntime>();
	var navigation = sp.GetRequiredService<NavigationManager>();

	var handler = new CustomAuthorizationMessageHandler(jsRuntime, navigation)
	{
		InnerHandler = new HttpClientHandler()
	};

	return new HttpClient(handler)
	{
		BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
	};
});

// Register application services
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<ITestService, TestService>();
builder.Services.AddScoped<IStudentService, StudentService>();

// Add MudBlazor services
builder.Services.AddMudServices();

// Run the application
await builder.Build().RunAsync();

