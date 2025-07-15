using MoysIQPlatform.Server.Data;
using MoysIQPlatform.Shared.Models;
using MoysIQPlatform.Shared.Models.Questions;
using MoysIQPlatform.Shared.Models.Tests;
using System.Data;

namespace MoysIQPlatform.Server.Services.TestService
{
	public class TestService : ITestService
	{
		private readonly DataContext _context;

		public TestService(DataContext context)
		{
			_context = context;
		}

		public async Task AttachQuestionsAsync(AttachQuestionsDto dto, int employeeId, string role)
		{

			var allowedRoles = new[] { "Admin", "Editor" };
			if (!allowedRoles.Contains(role))
				throw new UnauthorizedAccessException("You are not authorized to attach questions.");

			var test = await _context.Tests
				.Include(t => t.TestQuestions)
				.FirstOrDefaultAsync(t => t.Id == dto.TestId);

			if (test == null)
				throw new Exception("Test not found.");

			// Delete existing questions 
			_context.TestQuestions.RemoveRange(test.TestQuestions);

			foreach (var q in dto.Questions)
			{
				test.TestQuestions.Add(new TestQuestion
				{
					QuestionId = q.QuestionId,
					IsMandatory = q.IsMandatory
				});
			}

			await _context.SaveChangesAsync();
		}



		public async Task<int> CreateTestAsync(CreateTestDto dto, int employeeId, string employeeRole)

		{
			var employee = await _context.Employees
				.FirstOrDefaultAsync(e => e.Id == employeeId);

			if (employee == null)
				throw new Exception("Employee not found.");

			var allowedRoles = new[] { "Editor", "Admin" };

			if (!allowedRoles.Contains(employee.Role))
				throw new UnauthorizedAccessException("You are not authorized to create tests.");

			var test = new Test
			{
				Title = dto.Title,
				StartTime = dto.StartTime,
				EndTime = dto.EndTime,
				CreatedByEmployeeId = employeeId
			};

			_context.Tests.Add(test);
			await _context.SaveChangesAsync();
			return test.Id;
		}

		public async Task<List<TestDto>> GetAvailableTestsAsync()
		{
			try
			{
				var now = DateTime.UtcNow;

				var tests = await _context.Tests
					.Where(t => t.StartTime <= now && t.EndTime > now)
					.Select(t => new TestDto
					{
						Id = t.Id,
						Title = t.Title,
						StartTime = t.StartTime,
						EndTime = t.EndTime
					})
					.ToListAsync();
				if (tests == null || !tests.Any())
					return new List<TestDto>();

				return (tests);

			}
			catch (Exception ex)
			{
				// Log the exception (not shown here)
				throw new Exception("An error occurred while retrieving available tests.", ex);
			}
		}

		public async Task<List<QuestionSelection>> GetQuestionsForTestAsync(int testId)
		{
			var testQuestions = await _context.TestQuestions
				.Where(tq => tq.TestId == testId)
				.Select(tq => new QuestionSelection
				{
					QuestionId = tq.QuestionId,
					IsMandatory = tq.IsMandatory
				})
				.ToListAsync();

			return testQuestions;
		}

		public async Task<ServiceResponse<List<StudentAnswerSnapshot>>> GetStudentAnswerSnapshotAsync(int studentId)
		{
			var studentCheck = await _context.Students
				.Where(s => s.Id == studentId)
				.FirstOrDefaultAsync();
			if (studentCheck == null || !studentCheck.IsApproved)
				{
				return new ServiceResponse<List<StudentAnswerSnapshot>>
				{
					Data = null,
					Message = "Student not found or not active.",
					Success = false
				};

			}

			try
			{
				var response = await _context.StudentAnswerSnapshots
					.Where(s => s.StudentId == studentId)
					.Include(s => s.questions)
					.ThenInclude(s=>s.Options)
					.Include(s => s.test)
					.ToListAsync();
				return new ServiceResponse<List<StudentAnswerSnapshot>>
				{
					Data = response,
					Message = response.Any() ? "Snapshots retrieved successfully." : "No snapshots found for this student.",
					Success = true
				};
			}
			catch (Exception ex)
			{
				return new ServiceResponse<List<StudentAnswerSnapshot>>
				{
					Data = null,
					Message = $"An error occurred while retrieving snapshots: {ex.Message}",
					Success = false
				};
			}
		}

		public async Task<ServiceResponse<List<StudentScore>>> GetStudentScoreAsync(int studentId)
		{
			var studentCheck = await _context.Students
				.Where(s => s.Id == studentId)
				.FirstOrDefaultAsync();
			if (studentCheck == null || !studentCheck.IsApproved)
			{
				return new ServiceResponse<List<StudentScore>>
				{
					Data = null,
					Message = "Student not found or not active.",
					Success = false
				};
			}
				try
			{
				var response = await _context.StudentScores
					.Where(s => s.StudentId == studentId)
					.Include(s => s.Test)
					.ToListAsync();
				return new ServiceResponse<List<StudentScore>>
				{
					Data = response,
					Message = response.Any() ? "Scores retrieved successfully." : "No scores found for this student.",
					Success = true
				};
			}
			catch (Exception ex)
			{
				return new ServiceResponse<List<StudentScore>>
				{
					Data = null,
					Message = $"An error occurred while retrieving scores: {ex.Message}",
					Success = false
				};
			}

		}

		public async Task<TestDto> GetTestByIdAsync(int testId, int employeeId, string employeeRole)
		{
			var allowedRoles = new[] { "Admin", "Editor","Student" };
			if (!allowedRoles.Contains(employeeRole))
				throw new UnauthorizedAccessException("You are not authorized to attach questions.");

			var test = await _context.Tests
				.Where(t => t.Id == testId)
				.FirstOrDefaultAsync();
			if (test == null)
				throw new Exception("Test not found.");
			return new TestDto
			{
				Id = test.Id,
				Title = test.Title,
				StartTime = test.StartTime,
				EndTime = test.EndTime
			};

		}

		public async Task<ServiceResponse<List<StudentTestQuestionDto>>> GetValidTest(int testId, int studentId)
		{
			var studentCheck = await _context.Students
				.Where(t => t.Id == studentId).FirstOrDefaultAsync();
			if (studentCheck == null || !studentCheck.IsApproved)
			{
				return new ServiceResponse<List<StudentTestQuestionDto>>
				{
					Data = null,
					Message = "Student not found , or not active",
					Success = true
				};

			}
			var testQuestions = await _context.TestQuestions
				.Include(t => t.Test)
				.Include(q => q.Question)
				.ThenInclude(q => q.Options)
				.Where(t => t.TestId == testId)
				.ToListAsync();

			if (!testQuestions.Any())
			{
				return new ServiceResponse<List<StudentTestQuestionDto>>
				{
					Data = null,
					Message = "no message found",
					Success = true
				};
			};
			int totalQuestions = 20;
			int mandatoryCount = (int)Math.Ceiling(totalQuestions * 0.3);

			// choose the mandatory and optional questions
			var mandatoryQuestions = testQuestions.Where(q => q.IsMandatory).ToList();
			var optionalQuestions = testQuestions.Where(q => !q.IsMandatory).ToList();

			// choose random mandatory questions
			var selectedMandatory = mandatoryQuestions.OrderBy(_ => Guid.NewGuid())
													  .Take(mandatoryCount)
													  .ToList();

			// calculate remaining count for optional questions
			var remainingCount = totalQuestions - selectedMandatory.Count;
			var selectedOptional = optionalQuestions.OrderBy(_ => Guid.NewGuid())
													.Take(remainingCount)
													.ToList();

			// merge selected mandatory and optional questions
			var finalSelected = selectedMandatory.Concat(selectedOptional).ToList();
			List<StudentTestQuestionDto> _studentTestQuestionDto = new List<StudentTestQuestionDto>();

			foreach (var tq in finalSelected)
			{
				_studentTestQuestionDto.Add(new StudentTestQuestionDto
				{
					TestId = tq.Test.Id,
					QuestionId = tq.Question.Id,
					IsMandatory = tq.IsMandatory,
					Weight = tq.Question.Weight,
					QuestionType = tq.Question.Type.ToString(), // Assuming Type is an enum in Question model
					QuestionText = tq.Question.Text,
					ImageUrl = tq.Question.ImageUrl, // Assuming ImageUrl is a property in Question model
					Answers = tq.Question.Options.Select(o => new StudentAnswerOptionsDto
					{
						 answerId= o.Id,
						answerText = o.Text,
						questionId = tq.Question.Id,
						ImageUrl = o.ImageUrl, // Assuming ImageUrl is a property in AnswerOption model

					}).ToList()
				});
			}
			
			return new ServiceResponse<List<StudentTestQuestionDto>>
			{
				Data = _studentTestQuestionDto,
				Message = "Test questions retrieved successfully.",
				Success = true
			};
				


		}

		public async Task<ServiceResponse<bool>> IsTestSubmit(int testId, int studentId)
		{
			var response = await _context.StudentAnswers
				.AnyAsync(sa => sa.TestId == testId && sa.StudentId == studentId);
			if (response) {
				return new ServiceResponse<bool>
				{
					Data = true,
					Message = "Test already submitted.",
					Success = true
				};

					}
			else
				{
				return new ServiceResponse<bool>
				{
					Data = false,
					Message = "Test not submitted yet.",
					Success = true
				};
			}
		}

		public async Task<ServiceResponse<List<StudentAnswerDto>>> StudentAnswerSubmit(List<StudentAnswerDto> studentAnswers)
		{
			// Check if the input list is null or empty
			if (studentAnswers == null || !studentAnswers.Any())
			{
				return new ServiceResponse<List<StudentAnswerDto>>
				{
					Data = null,
					Message = "No answers provided.",
					Success = false
				};
			}

			var studentId = studentAnswers[0].StudentId;
			var testId = studentAnswers[0].TestId;

			// Verify student exists and is approved
			var studentCheck = await _context.Students.FirstOrDefaultAsync(t => t.Id == studentId);
			if (studentCheck == null || !studentCheck.IsApproved)
			{
				return new ServiceResponse<List<StudentAnswerDto>>
				{
					Data = null,
					Message = "Student not found or not approved.",
					Success = false
				};
			}

			// Verify test exists
			var testCheck = await _context.Tests.FirstOrDefaultAsync(t => t.Id == testId);
			if (testCheck == null)
			{
				return new ServiceResponse<List<StudentAnswerDto>>
				{
					Data = null,
					Message = "Test not found.",
					Success = false
				};
			}

			// Validate all answers belong to the same student and test
			bool mixedStudent = studentAnswers.Any(a => a.StudentId != studentId);
			bool mixedTest = studentAnswers.Any(a => a.TestId != testId);

			if (mixedStudent || mixedTest)
			{
				return new ServiceResponse<List<StudentAnswerDto>>
				{
					Data = null,
					Message = "Answers must belong to one student and one test only.",
					Success = false
				};
			}

			// Ensure every answer has either option or written text
			foreach (var answer in studentAnswers)
			{
				if (answer.AnswerOptionId == null && string.IsNullOrWhiteSpace(answer.WrittenAnswer))
				{
					return new ServiceResponse<List<StudentAnswerDto>>
					{
						Data = null,
						Message = $"Answer for question {answer.QuestionId} is missing.",
						Success = false
					};
				}
			}

			// Convert DTOs to actual StudentAnswer entities
			var entityAnswers = studentAnswers.Select(dto => new StudentAnswer
			{
				StudentId = dto.StudentId,
				TestId = dto.TestId,
				QuestionId = dto.QuestionId,
				AnswerOptionId = dto.AnswerOptionId,
				WrittenAnswer = dto.WrittenAnswer,
				AnsweredAt = dto.AnsweredAt
			}).ToList();

			_context.StudentAnswers.AddRange(entityAnswers);

			try
			{
				await _context.SaveChangesAsync();

				await CalculateAndStoreStudentScoreAsync(studentId, testId);

				return new ServiceResponse<List<StudentAnswerDto>>
				{
					Data = studentAnswers,
					Message = "Answers submitted successfully.",
					Success = true
				};
			}
			catch (Exception ex)
			{
				return new ServiceResponse<List<StudentAnswerDto>>
				{
					Data = null,
					Message = $"An error occurred while submitting answers: {ex.Message}",
					Success = false
				};
			}
		}

		private async Task<ServiceResponse<StudentScore>> CalculateAndStoreStudentScoreAsync(int studentId, int testId)
		{
			var testQuestions = await _context.TestQuestions
				.Where(tq => tq.TestId == testId)
				.Include(tq => tq.Question)
					.ThenInclude(q => q.Options)
				.ToListAsync();

			var studentAnswers = await _context.StudentAnswers
				.Where(a => a.StudentId == studentId && a.TestId == testId)
				.ToListAsync();

			double totalScore = 0;
			var snapshots = new List<StudentAnswerSnapshot>();

			foreach (var tq in testQuestions)
			{
				var question = tq.Question;
				var studentAnswer = studentAnswers.FirstOrDefault(a => a.QuestionId == question.Id);
				if (studentAnswer == null)
					continue;

				var correctOption = question.Options.FirstOrDefault(o => o.IsCorrect);
				bool isCorrect = false;

				if (question.Type == "MCQ" && correctOption != null && studentAnswer.AnswerOptionId == correctOption.Id)
				{
					totalScore += question.Weight;
					isCorrect = true;
				}
				else if (question.Type == "Text" && correctOption != null)
				{
					var correct = correctOption.Text?.Trim().ToLower();
					var student = studentAnswer.WrittenAnswer?.Trim().ToLower();

					if (!string.IsNullOrWhiteSpace(student) && student == correct)
					{
						totalScore += question.Weight;
						isCorrect = true;
					}
				}

				snapshots.Add(new StudentAnswerSnapshot
				{
					StudentId = studentId,
					TestId = testId,
					QuestionId = question.Id,
					StudentAnswerText = studentAnswer.WrittenAnswer,
					StudentAnswerOptionId = studentAnswer.AnswerOptionId,
					CorrectAnswerTextAtSubmission = correctOption?.Text,
					CorrectAnswerOptionIdAtSubmission = correctOption?.Id,
					IsCorrect = isCorrect,
					QuestionWeight = question.Weight,
					SubmittedAt = DateTime.UtcNow
				});
			}

			var studentScore = new StudentScore
			{
				StudentId = studentId,
				TestId = testId,
				Score = (int)Math.Round(totalScore),
				Date = DateTime.UtcNow
			};

			_context.StudentScores.Add(studentScore);
			_context.StudentAnswerSnapshots.AddRange(snapshots); // 🆕 Add answer snapshots
			await _context.SaveChangesAsync();

			Console.WriteLine($"Score for student {studentId} in test {testId}: {studentScore.Score}");

			return new ServiceResponse<StudentScore>
			{
				Data = studentScore,
				Message = "Score calculated and saved successfully.",
				Success = true
			};
		}



	}
}
