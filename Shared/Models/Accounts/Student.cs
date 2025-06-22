using MoysIQPlatform.Shared.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace MoysIQPlatform.Shared.Models.Accounts;

public class Student
{
	[Key]
	public int Id { get; set; }

	[Required]
	public string FullName { get; set; } = string.Empty;

	[Required, EmailAddress]
	public string Email { get; set; } = string.Empty;

	[Required]
	public string PasswordHash { get; set; } = string.Empty;

	public string SchoolName { get; set; } = string.Empty;

	public string Grade { get; set; } = string.Empty;

	public string Gender { get; set; } = string.Empty;

	// Indicates whether the student account is approved by an admin
	public bool IsApproved { get; set; } = false;

	// The UTC date and time when the student was created
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	public DateTime BirthDay { get; set; }
}
