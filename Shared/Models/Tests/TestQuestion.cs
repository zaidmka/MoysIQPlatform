using MoysIQPlatform.Shared.Models.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoysIQPlatform.Shared.Models.Tests
{
	public class TestQuestion
	{
		public int Id { get; set; }
		public int TestId { get; set; }
		public Test Test { get; set; } = default!;

		public int QuestionId { get; set; }
		public Question Question { get; set; } = default!;

		public bool IsMandatory { get; set; }
	}
}
