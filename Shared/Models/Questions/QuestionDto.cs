using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoysIQPlatform.Shared.Models.Questions
{
	public class QuestionDto
	{
		public int Id { get; set; }
		public string Text { get; set; } = string.Empty;
		public string Type { get; set; } = string.Empty; // "MCQ" or "Text"
	}
}
