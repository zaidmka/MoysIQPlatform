using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoysIQPlatform.Shared.Models.Tests
{
	public class StudentTestDto
	{
		public int testId { get; set; }
		public string testName { get; set; }=string.Empty;
		public DateTime startTime { get; set; }
		public DateTime endTime { get; set; }


	}
}
