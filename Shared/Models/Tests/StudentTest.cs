using MoysIQPlatform.Shared.Models.Tests;

namespace MoysIQPlatform.Shared.Models.Tests;

public class StudentTest
{
    public int TestId { get; set; }
    public int StudentId { get; set; }
    public List<StudentAnswer> Answers { get; set; } = new();
}