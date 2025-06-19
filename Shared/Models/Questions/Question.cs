using MoysIQPlatform.Shared.Models.Accounts;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MoysIQPlatform.Shared.Models.Questions;

public class Question
{
	[Key]
	public int Id { get; set; }
	public string Text { get; set; } = string.Empty;
	public string Type { get; set; } = string.Empty; // "MCQ", "Text"
	public double Weight { get; set; }
	public bool IsMandatory { get; set; } = false;
	public DateTime CreatedDate { get; set; }

	public int CreatedByEmployeeId { get; set; }

	public Employee Employee { get; set; } = default!;

	public List<AnswerOption> Options { get; set; } = new(); // like to answers
}
