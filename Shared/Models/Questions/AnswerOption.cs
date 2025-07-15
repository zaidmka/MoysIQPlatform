using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MoysIQPlatform.Shared.Models.Questions;

public class AnswerOption
{
	[Key]
	public int Id { get; set; }
	public int QuestionId { get; set; }
	public string Text { get; set; }
	public bool IsCorrect { get; set; }=false;
	public string? ImageUrl { get; set; } // if the answer option has an image

	[JsonIgnore]
	public Question Question { get; set; }
}
