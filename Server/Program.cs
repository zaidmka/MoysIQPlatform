﻿global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using MoysIQPlatform.Server.Services.EmployeeService;
global using MoysIQPlatform.Server.Services.QuestionsServices;
global using MoysIQPlatform.Server.Services.StudentService;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MoysIQPlatform.Server.Data;
using MoysIQPlatform.Server.Services.TestService;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using dotenv.net;



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

builder.Services.AddHttpContextAccessor(); // For accessing HTTP context in services

builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IQuestionsServices, QuestionsServices>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ITestService, TestService>();




// ✅ Rate Limiting
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

// ✅ Swagger + JWT Support
builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "MoysIQPlatform API",
		Version = "v1"
	});

	// important
	options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		Scheme = "bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header,
		Description = "Enter 'Bearer' [space] and then your token"
	});

	options.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			Array.Empty<string>()
		}
	});
});


//✅ Cloudinary Configuration
	DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
Cloudinary cloudinary = new Cloudinary(Environment.GetEnvironmentVariable("CLOUDINARY_URL"));
cloudinary.Api.Secure = true;

builder.Services.AddSingleton(new Cloudinary(Environment.GetEnvironmentVariable("CLOUDINARY_URL"))
{
	Api = { Secure = true }
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
		"default-src 'self'; " +
		"img-src 'self' data: blob: https://res.cloudinary.com; " +
		"style-src 'self' 'unsafe-inline' https://fonts.googleapis.com; " +
		"font-src 'self' https://fonts.gstatic.com; " +
		"script-src 'self' 'unsafe-inline' 'unsafe-eval'";

	await next();
});


// ✅ Dev / Prod Middleware
if (app.Environment.IsDevelopment())
{
	app.UseWebAssemblyDebugging();
	app.UseSwagger();
	app.UseSwaggerUI();
	app.UseDeveloperExceptionPage(); // ✅ active in Development
									 // ❌ Don't use this in Production! Use app.UseExceptionHandler("/error") instead
									 // to handle exceptions gracefully.
}
else
{
	app.UseSwagger();       // ✅ Swagger is still available in Production
	app.UseSwaggerUI();     // ✅ Swagger UI is still available in Production
	app.UseDeveloperExceptionPage(); // ❌ Don't use this in Production! Use app.UseExceptionHandler("/error") instead
}


app.UseHttpsRedirection();
app.UseResponseCompression();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseIpRateLimiting(); // before authentication to limit requests
app.Use(async (context, next) =>
{
	var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
	Console.WriteLine($"🔍 Swagger Authorization Header: {authHeader}");
	await next();
});
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
