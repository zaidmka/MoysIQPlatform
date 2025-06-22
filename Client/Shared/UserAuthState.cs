namespace MoysIQPlatform.Client.Shared;

public class UserAuthState
{
	public bool IsAuthenticated { get; set; }
	public string Role { get; set; } = string.Empty;
	public bool IsApproved { get; set; }

	public string Status =>
		!IsAuthenticated ? "unauthenticated" :
		IsApproved && Role == "Employee" ? "active_employee" :
		IsApproved && Role == "Student" ? "active_student" :
		!IsApproved && Role == "Employee" ? "inactive_employee" :
		!IsApproved && Role == "Student" ? "inactive_student" :
		"unknown";
	public string? ErrorMessage { get; set; }
	public string FullName { get; set; } = string.Empty;
	public bool HasRole(params string[] roles) =>
	roles.Any(r => Role.Split(',').Contains(r, StringComparer.OrdinalIgnoreCase));


}
