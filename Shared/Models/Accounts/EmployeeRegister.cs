using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoysIQPlatform.Shared.Models.Accounts
{
	public class EmployeeRegister
	{
		[Required, EmailAddress]
		public string Email { get; set; } = string.Empty;
		[Required, StringLength(100, MinimumLength = 6)]
		public string Password { get; set; } = string.Empty;
		[Compare("Password", ErrorMessage = "The Passwords do not Match!")]
		public string ConfirmPassword { get; set; } = string.Empty;
		[Required]
		public string FullName { get; set; } = string.Empty;
		public string Department { get; set; } = string.Empty;
		public string Role { get; set; } = string.Empty;
		public string WorkLocation { get; set; } = string.Empty;
		public string PhoneNumber { get; set; } = string.Empty;
		public string Gender { get; set; } = string.Empty;
	}
}
