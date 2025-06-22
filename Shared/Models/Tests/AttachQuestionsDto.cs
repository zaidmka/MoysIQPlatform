using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoysIQPlatform.Shared.Models.Tests
{
	public class AttachQuestionsDto
	{
		public int TestId { get; set; }
		public List<QuestionSelection> Questions { get; set; } = new();
	}

	public class QuestionSelection
	{
		public int QuestionId { get; set; }
		public bool IsMandatory { get; set; }
		public bool IsSelected { get; set; }
	}
}
