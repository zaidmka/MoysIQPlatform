using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoysIQPlatform.Shared.Models.Tests
{
	public class StudentAnswerDto
	{
		public int StudentId { get; set; }
		public int TestId { get; set; }
		public int QuestionId { get; set; }
		public int? AnswerOptionId { get; set; }
		public string? WrittenAnswer { get; set; }
		public DateTime AnsweredAt { get; set; }
	}
}
