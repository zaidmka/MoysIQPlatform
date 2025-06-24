using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoysIQPlatform.Shared.Models.Questions
{
	public class StudentAnswerOptionsDto
	{
		public int answerId { get; set; }
		public int questionId { get; set; }
		public string answerText { get; set; } = string.Empty;
	}
}
