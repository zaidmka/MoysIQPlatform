using MoysIQPlatform.Shared.Models.Questions;
using MoysIQPlatform.Shared.Models.Accounts;
using System.Text.Json.Serialization;

namespace MoysIQPlatform.Shared.Models.Questions;

public class Question
{
	public int Id { get; set; }
	public string Text { get; set; }
	public string Type { get; set; } // "MCQ", "Text"
	public double Weight { get; set; }
	public bool IsMandatory { get; set; }
	public DateTime CreatedDate { get; set; }

	public int CreatedByEmployeeId { get; set; }                 // Foreign Key

	[JsonIgnore]                                                 // ← To avoid circular loop
	public EmployeeProfile Employee { get; set; } = default!;    // Navigation Property

	public List<AnswerOption> Options { get; set; } = new();
}
