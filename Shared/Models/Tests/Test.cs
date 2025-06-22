using MoysIQPlatform.Shared.Models.Accounts;
using System.Text.Json.Serialization;

namespace MoysIQPlatform.Shared.Models.Tests;

public class Test
{
	public int Id { get; set; }
	public string Title { get; set; } = string.Empty;
	public DateTime StartTime { get; set; }
	public DateTime EndTime { get; set; }

	public int CreatedByEmployeeId { get; set; }
	[JsonIgnore]

	public Employee CreatedBy { get; set; } = default!;

	public List<TestQuestion> TestQuestions { get; set; } = new();
}