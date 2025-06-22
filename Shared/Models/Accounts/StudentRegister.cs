using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoysIQPlatform.Shared.Models.Accounts
{
	public class StudentRegister
	{
		public string FullName { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
		public string SchoolName { get; set; } = string.Empty;
		public string Grade { get; set; } = string.Empty;
		public string Gender { get; set; } = string.Empty;
		public DateTime BirthDay { get; set; } = DateTime.Today;
	}
}
