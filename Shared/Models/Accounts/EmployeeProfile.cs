using MoysIQPlatform.Shared.Models.Questions;
using System.Text.Json.Serialization;

namespace MoysIQPlatform.Shared.Models.Accounts;

public class EmployeeProfile
{
	public int Id { get; set; }
	public string FullName { get; set; }
	public string Email { get; set; }
	public string PasswordHash { get; set; }
	public string Department { get; set; }
	public string Position { get; set; }
	public string WorkLocation { get; set; }
	public string PhoneNumber { get; set; }
	public string Gender { get; set; }
	public bool IsApproved { get; set; }

	[JsonIgnore]
	public List<Question> Questions { get; set; } = new();
}
