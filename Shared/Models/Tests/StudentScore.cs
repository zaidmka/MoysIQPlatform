using MoysIQPlatform.Shared.Models.Accounts;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MoysIQPlatform.Shared.Models.Tests
{
	public class StudentScore
	{
		[Key]
		public int Id { get; set; }

		public int StudentId { get; set; }
		public int TestId { get; set; }
		public int Score { get; set; }
		public DateTime Date { get; set; }
		public string? Comment { get; set; }

		// Navigation Properties
		[JsonIgnore]
		public Student Student { get; set; } = default!;


		public Test Test { get; set; } = default!;

		[JsonIgnore]
		public StudentAnswer? StudentAnswer { get; set; } 
	}
}
