namespace MoysIQPlatform.Shared.Models.Questions
{
	public class QuestionCreateDto
	{
		public string Text { get; set; } = string.Empty;
		public string Type { get; set; } = "MCQ";
		public double Weight { get; set; }
		public bool IsMandatory { get; set; }
		public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
		public int CreatedByEmployeeId { get; set; }
		public List<AnswerOptionDto> Options { get; set; } = new();
	}

	public class AnswerOptionDto
	{
		public string Text { get; set; } = string.Empty;
		public bool IsCorrect { get; set; }
	}
}
