namespace MoysIQPlatform.Shared.Models.Questions
{
	public class QuestionCreateDto
	{
		public string Text { get; set; } = string.Empty;

		// Question type: "MCQ" or "Text"
		public string Type { get; set; } = "MCQ";

		// Weight/score of the question
		public double Weight { get; set; }

		// Whether this question is mandatory
		public bool IsMandatory { get; set; }

		// Date when the question is created (stored as UTC)
		public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

		// Optional image for the question (e.g. diagram, visual pattern)
		public string? ImageBase64 { get; set; }

		// List of answer options (if applicable)
		public List<AnswerOptionDto> Options { get; set; } = new();
	}

	public class AnswerOptionDto
	{
		// Text content of the option
		public string Text { get; set; } = string.Empty;

		// Indicates whether this option is the correct answer
		public bool IsCorrect { get; set; }

		// Optional image for this option (e.g. visual-based choice)
		public string? ImageBase64 { get; set; }
	}
}
