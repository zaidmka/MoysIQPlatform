using MoysIQPlatform.Shared.Models.Accounts;
using MoysIQPlatform.Shared.Models.Questions;
using System.ComponentModel.DataAnnotations;

namespace MoysIQPlatform.Shared.Models.Tests;

public class StudentAnswer
{
	[Key]
	public int Id { get; set; }

	// Foreign Keys
	public int StudentId { get; set; }
	public int TestId { get; set; }
	public int QuestionId { get; set; }
	public int? AnswerOptionId { get; set; }

	public string? WrittenAnswer { get; set; }
	public DateTime AnsweredAt { get; set; } = DateTime.UtcNow;

	// Navigation Properties
	public Student Student { get; set; } = default!;
	public Test Test { get; set; } = default!;
	public Question Question { get; set; } = default!;
	public AnswerOption? AnswerOption { get; set; }
}