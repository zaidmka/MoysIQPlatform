using System.ComponentModel.DataAnnotations;

namespace MoysIQPlatform.Shared.Models.Accounts
{
	public class EmployeeRegister
	{
		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage = "Invalid Email address")]
		public string Email { get; set; } = string.Empty;

		[Required(ErrorMessage = "Password is required")]
		[StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters")]
		public string Password { get; set; } = string.Empty;

		[Required(ErrorMessage = "Please confirm your password")]
		[Compare("Password", ErrorMessage = "The passwords do not match")]
		public string ConfirmPassword { get; set; } = string.Empty;

		[Required(ErrorMessage = "Full name is required")]
		public string FullName { get; set; } = string.Empty;

		[Required(ErrorMessage = "Department is required")]
		public string Department { get; set; } = string.Empty;

		[Required(ErrorMessage = "Role is required")]
		public string Role { get; set; } = string.Empty;

		[Required(ErrorMessage = "Work location is required")]
		public string WorkLocation { get; set; } = string.Empty;

		[Required(ErrorMessage = "Phone number is required")]
		[Phone(ErrorMessage = "Invalid phone number")]
		public string PhoneNumber { get; set; } = string.Empty;

		[Required(ErrorMessage = "Gender is required")]
		public string Gender { get; set; } = string.Empty;
	}
}
