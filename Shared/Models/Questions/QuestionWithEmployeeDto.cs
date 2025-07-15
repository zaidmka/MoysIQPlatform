namespace MoysIQPlatform.Shared.Models.Questions;

public class QuestionWithEmployeeDto
{
	public int Id { get; set; }
	public string Text { get; set; } = string.Empty;
	public string Type { get; set; } = "MCQ";
	public double Weight { get; set; }
	public bool IsMandatory { get; set; }
	public DateTime CreatedDate { get; set; }

	public string CreatedBy { get; set; } = string.Empty; // ← FullName أو Email

	public string? ImageUrl { get; set; } // if the question has an image
	public List<AnswerOption> Options { get; set; } = new();
}
