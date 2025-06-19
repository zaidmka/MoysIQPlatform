using Microsoft.EntityFrameworkCore;
using MoysIQPlatform.Server.Data;
using MoysIQPlatform.Shared.Models.Questions;

namespace MoysIQPlatform.Server.Services.QuestionsServices
{
	public class QuestionsServices : IQuestionsServices
	{
		private readonly DataContext _context;
		public QuestionsServices(DataContext context)
		{
			_context = context;
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
				CreatedBy = q.Employee.FullName, // أو q.Employee.Email
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

		public async Task<ServiceResponse<Question>> CreateQuestion(QuestionCreateDto dto)
		{
			try
			{
				var question = new Question
				{
					Text = dto.Text,
					Type = dto.Type,
					Weight = dto.Weight,
					IsMandatory = dto.IsMandatory,
					CreatedDate = dto.CreatedDate,
					CreatedByEmployeeId = dto.CreatedByEmployeeId,
				};

				question.Options = dto.Options.Select(o => new AnswerOption
				{
					Text = o.Text,
					IsCorrect = o.IsCorrect,
					Question = question // real connection to the question
				}).ToList();

				_context.Questions.Add(question);
				await _context.SaveChangesAsync();

				return new ServiceResponse<Question>
				{
					Data = question,
					Success = true,
					Message = "Question created successfully."
				};
			}
			catch (Exception ex)
			{
				return new ServiceResponse<Question>
				{
					Success = false,
					Message = ex.Message
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
	}
}
