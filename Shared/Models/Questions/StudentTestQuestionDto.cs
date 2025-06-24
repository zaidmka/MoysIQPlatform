using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoysIQPlatform.Shared.Models.Questions
{
	public class StudentTestQuestionDto
	{
		public int TestId { get; set; }
		public int QuestionId { get; set; }
		public string QuestionText { get; set; } = string.Empty;
		public string QuestionType { get; set; } = string.Empty; // "MCQ" or "Text"
		public double Weight { get; set; }
		public bool IsMandatory { get; set; } = false;
		public List<StudentAnswerOptionsDto> Answers { get; set; } = new();
	}
}
