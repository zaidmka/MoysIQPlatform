using MoysIQPlatform.Shared.Models.Questions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MoysIQPlatform.Shared.Models.Tests
{
	public class StudentAnswerSnapshot
	{
		[Key]
		public int Id { get; set; }
		public int StudentId { get; set; }
		public int TestId { get; set; }
		public int QuestionId { get; set; }

		public string? StudentAnswerText { get; set; }
		public int? StudentAnswerOptionId { get; set; }

		public string? CorrectAnswerTextAtSubmission { get; set; }
		public int? CorrectAnswerOptionIdAtSubmission { get; set; }

		public bool IsCorrect { get; set; }
		public double QuestionWeight { get; set; }

		public DateTime SubmittedAt { get; set; }

		public Question questions { get; set; }


		[JsonIgnore]
		public Test test { get; set; }
	}
}
