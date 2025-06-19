global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;

using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using MoysIQPlatform.Server.Data;
using MoysIQPlatform.Server.Services.EmployeeService;
using MoysIQPlatform.Server.Services.QuestionsServices;

// ✅ Enable legacy encoding support (for older PDFs, etc.)
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

// ✅ Load environment variables from .env
DotNetEnv.Env.Load();

// ✅ Read sensitive values from environment
var connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION");
var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET")!;
var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER")!;
var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")!;

var builder = WebApplication.CreateBuilder(args);

// ✅ EF Core with PostgreSQL
builder.Services.AddDbContext<DataContext>(options =>
	options.UseNpgsql(connectionString));

// ✅ JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidIssuer = jwtIssuer,

			ValidateAudience = true,
			ValidAudience = jwtAudience,

			ValidateLifetime = true,

			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
			ValidateIssuerSigningKey = true
		};
	});

// ✅ App Services
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IQuestionsServices, QuestionsServices>();

// ✅ Rate Limiting
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

// ✅ Swagger + JWT Support
builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new OpenApiInfo { Title = "MoysIQPlatform API", Version = "v1" });

	options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
	{
		Description = "JWT Bearer - Example: Bearer {token}",
		In = ParameterLocation.Header,
		Name = "Authorization",
		Type = SecuritySchemeType.ApiKey
	});

	options.OperationFilter<SecurityRequirementsOperationFilter>();
});

// ✅ UI & Compression
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddResponseCompression(options =>
{
	options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
		new[] { "application/octet-stream" });
});

var app = builder.Build();

// ✅ Secure Response Headers
app.Use(async (context, next) =>
{
	context.Response.Headers["X-Content-Type-Options"] = "nosniff";
	context.Response.Headers["X-Frame-Options"] = "DENY";
	context.Response.Headers["Referrer-Policy"] = "no-referrer";
	context.Response.Headers["Content-Security-Policy"] =
		"default-src 'self'; style-src 'self' 'unsafe-inline'; script-src 'self' 'unsafe-inline' 'unsafe-eval'";
	await next();
});

// ✅ Dev / Prod Middleware
if (app.Environment.IsDevelopment())
{
	app.UseWebAssemblyDebugging();
	app.UseSwagger();
	app.UseSwaggerUI();
}
else
{
	app.UseExceptionHandler("/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseResponseCompression();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseIpRateLimiting(); // before authentication to limit requests
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
