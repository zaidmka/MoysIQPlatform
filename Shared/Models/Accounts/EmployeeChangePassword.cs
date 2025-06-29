﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoysIQPlatform.Shared.Models.Accounts
{
	public class EmployeeChangePassword
	{
		[Required, StringLength(100, MinimumLength = 6)]
		public string Password { get; set; } = string.Empty;
		[Compare("Password", ErrorMessage = "The passwords do not match.")]
		public string ConfirmPassword { get; set; } = string.Empty;
	}
}
