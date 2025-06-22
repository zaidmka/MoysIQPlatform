using Microsoft.EntityFrameworkCore;
using MoysIQPlatform.Server.Data;
using MoysIQPlatform.Shared.Models;
using MoysIQPlatform.Shared.Models.Accounts;
using MoysIQPlatform.Shared.Models.Questions;
using System.Security.Claims;

namespace MoysIQPlatform.Server.Services.QuestionsServices
{
	public class QuestionsServices : IQuestionsServices
	{
		private readonly DataContext _context;
		private readonly IHttpContextAccessor _httpContext;

		public QuestionsServices(DataContext context, IHttpContextAccessor httpContext)
		{
			_context = context;
			_httpContext = httpContext;
		}

		public async Task<ServiceResponse<List<QuestionWithEmployeeDto>>> GetAllQuestions()
		{
			var questions = await _context.Questions
									.Include(q => q.Options)
									.Include(q => q.Employee)
									.ToListAsync();

			var result = questions.Select(q => new QuestionWithEmployeeDto
			{
				Id = q.Id,
				Text = q.Text,
				Type = q.Type,
				Weight = q.Weight,
				IsMandatory = q.IsMandatory,
				CreatedDate = q.CreatedDate,
				CreatedBy = q.Employee.FullName, //  q.Employee.Email
				Options = q.Options
			}).ToList();

			return new ServiceResponse<List<QuestionWithEmployeeDto>>
			{
				Data = result,
				Success = true
			};

		}

		public async Task<ServiceResponse<Question>> GetQuestionById(int id)
		{
			var question = await _context.Questions.Include(q => q.Options).FirstOrDefaultAsync(q => q.Id == id);
			if (question == null)
				return new ServiceResponse<Question> { Success = false, Message = "Question not found" };
			return new ServiceResponse<Question> { Data = question };
		}

		public async Task<ServiceResponse<QuestionWithEmployeeDto>> CreateQuestion(QuestionCreateDto dto)
		{
			try
			{
				var auth = await ValidateAuthUser();
				if (!auth.IsValid)
				{
					return new ServiceResponse<QuestionWithEmployeeDto>
					{
						Success = false,
						Message = auth.ErrorMessage
					};
				}
				// 🛡️ Check role explicitly
				if (!auth.Role.Split(',').Select(r => r.Trim()).Contains("Editor") &&
					!auth.Role.Split(',').Select(r => r.Trim()).Contains("Admin")) // optionally allow admins too
				{
					return new ServiceResponse<QuestionWithEmployeeDto>
					{
						Success = false,
						Message = "You are not authorized to create questions."
					};
				}

				var question = new Question
				{
					Text = dto.Text,
					Type = dto.Type,
					Weight = dto.Weight,
					IsMandatory = dto.IsMandatory,
					CreatedDate = dto.CreatedDate,
					CreatedByEmployeeId = auth.UserId!.Value // ✅ from auth context
				};

				question.Options = dto.Options.Select(o => new AnswerOption
				{
					Text = o.Text,
					IsCorrect = o.IsCorrect,
					Question = question
				}).ToList();

				_context.Questions.Add(question);
				await _context.SaveChangesAsync();

				return new ServiceResponse<QuestionWithEmployeeDto>
				{
					Data = new QuestionWithEmployeeDto
					{
						Text = question.Text,
						Type = question.Type,
						Weight = question.Weight,
						IsMandatory = question.IsMandatory,
						CreatedDate = question.CreatedDate,
						Options = question.Options.Select(o => new AnswerOption
						{
							Text = o.Text,
							IsCorrect = o.IsCorrect
						}).ToList()
					},
					Success = true,
					Message = "Question created successfully."
				};
			}
			catch (Exception ex)
			{
				return new ServiceResponse<QuestionWithEmployeeDto>
				{
					Success = false,
					Message = $"Error: {ex.Message}"
				};
			}
		}



		public async Task<ServiceResponse<bool>> UpdateQuestion(int id, Question question)
		{
			var dbQuestion = await _context.Questions
				.Include(q => q.Options)
				.FirstOrDefaultAsync(q => q.Id == id);

			if (dbQuestion == null)
				return new ServiceResponse<bool>
				{
					Success = false,
					Message = "Question not found"
				};

			dbQuestion.Text = question.Text;
			dbQuestion.Type = question.Type;
			dbQuestion.Weight = question.Weight;
			dbQuestion.IsMandatory = question.IsMandatory;
			dbQuestion.CreatedDate = question.CreatedDate;
			dbQuestion.CreatedByEmployeeId = question.CreatedByEmployeeId;

			// Remove old options
			_context.AnswerOptions.RemoveRange(dbQuestion.Options);

			// Add new options
			dbQuestion.Options = question.Options.Select(o => new AnswerOption
			{
				Text = o.Text,
				IsCorrect = o.IsCorrect,
				QuestionId = dbQuestion.Id // important to keep the FK consistent
			}).ToList();

			await _context.SaveChangesAsync();

			return new ServiceResponse<bool>
			{
				Data = true,
				Message = "Question updated successfully"
			};
		}

		public async Task<ServiceResponse<bool>> DeleteQuestion(int id)
		{
			var question = await _context.Questions.FirstOrDefaultAsync(q => q.Id == id);
			if (question == null)
				return new ServiceResponse<bool> { Success = false, Message = "Question not found" };

			_context.Questions.Remove(question);
			await _context.SaveChangesAsync();
			return new ServiceResponse<bool> { Data = true, Message = "Question deleted" };
		}

		private async Task<AuthUserContext> ValidateAuthUser()
		{
			var user = _httpContext.HttpContext?.User;

			if (!(user.Identity?.IsAuthenticated ?? false))
				return new AuthUserContext
				{
					IsValid = false,
					ErrorMessage = "Not authenticated."
				};

			var role = user.FindFirst(ClaimTypes.Role)?.Value ?? "";
			var idStr = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (!int.TryParse(idStr, out var userId))
				return new AuthUserContext
				{
					IsValid = false,
					ErrorMessage = "Invalid user ID in token."
				};

			var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == userId);
			if (employee == null)
				return new AuthUserContext
				{
					IsValid = false,
					ErrorMessage = "Employee not found."
				};

			if (!employee.IsApproved)
				return new AuthUserContext
				{
					IsValid = false,
					ErrorMessage = "Employee is not approved yet."
				};

			return new AuthUserContext
			{
				IsValid = true,
				UserId = userId,
				Role = role,
				EmployeeEntity = employee
			};
		}

	}
}
