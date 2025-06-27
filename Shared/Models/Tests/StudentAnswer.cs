using MoysIQPlatform.Shared.Models.Accounts;
using MoysIQPlatform.Shared.Models.Questions;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
	[JsonIgnore]
	public Student Student { get; set; }
	[JsonIgnore]
	public Test Test { get; set; }
	[JsonIgnore]
	public Question Question { get; set; }
	[JsonIgnore]
	public AnswerOption? AnswerOption { get; set; }
}