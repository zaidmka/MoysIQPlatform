using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoysIQPlatform.Shared.Models.Accounts
{
	public class AuthUserContext
	{
		public bool IsValid { get; set; }
		public string? ErrorMessage { get; set; }
		public int? UserId { get; set; }
		public string Role { get; set; } = "";
		public Employee? EmployeeEntity { get; set; }
	}

}
