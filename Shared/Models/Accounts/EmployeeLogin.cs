using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoysIQPlatform.Shared.Models.Accounts
{
	public class EmployeeLogin
	{
		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage = "Invalid Email Address")] // 
		public string Email { get; set; } = string.Empty;

		[Required(ErrorMessage = "Password is required")]
		[MinLength(3, ErrorMessage = "Password too short")]
		public string Password { get; set; } = string.Empty;
	}
}
