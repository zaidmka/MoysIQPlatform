namespace MoysIQPlatform.Client.Shared
{
	public class UserAuthState
	{
		public bool IsAuthenticated { get; set; }
		public string Role { get; set; } = string.Empty;
		public string FullName { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string IsApproved { get; set; } = string.Empty;

		public string? ErrorMessage { get; set; }

		public bool HasRole(params string[] roles) =>
			roles.Any(r => Role.Split(',').Contains(r, StringComparer.OrdinalIgnoreCase));
	}
}
