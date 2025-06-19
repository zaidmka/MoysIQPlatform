using Microsoft.EntityFrameworkCore;
using MoysIQPlatform.Shared.Models.Accounts;
using MoysIQPlatform.Shared.Models.Questions;
using MoysIQPlatform.Shared.Models.Tests;

namespace MoysIQPlatform.Server.Data
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options) { }

		public DbSet<Employee> Employees { get; set; }
		//public DbSet<StudentProfile> Students { get; set; }
		public DbSet<Question> Questions { get; set; }
		public DbSet<AnswerOption> AnswerOptions { get; set; }
		//public DbSet<Test> Tests { get; set; }
		//public DbSet<StudentTest> StudentTests { get; set; }
		//public DbSet<StudentAnswer> StudentAnswers { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Force all column names to lowercase for PostgreSQL compatibility
			foreach (var entity in modelBuilder.Model.GetEntityTypes())
			{
				entity.SetTableName(entity.GetTableName()?.ToLower());
				foreach (var property in entity.GetProperties())
				{
					property.SetColumnName(property.Name.ToLower());
				}
			}

			modelBuilder.Entity<Question>()
				.HasOne(q => q.Employee)
				.WithMany(e => e.Questions)
				.HasForeignKey(q => q.CreatedByEmployeeId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<AnswerOption>()
				.HasOne(a => a.Question)
				.WithMany(q => q.Options)
				.HasForeignKey(a => a.QuestionId)
				.OnDelete(DeleteBehavior.Cascade);

			
		}
	}
}
