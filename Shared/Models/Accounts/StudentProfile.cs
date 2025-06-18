using MoysIQPlatform.Shared.Models.Enums;

namespace MoysIQPlatform.Shared.Models.Accounts;

public class StudentProfile
{
	public int Id { get; set; }
	public string FullName { get; set; }
	public string Email { get; set; }
	public string PasswordHash { get; set; }
	public string SchoolName { get; set; }
	public StudyStage Stage { get; set; }
	public DateTime BirthDate { get; set; }
	public string Gender { get; set; }
	public bool IsApproved { get; set; }
}
