using MoysIQPlatform.Shared.Models;
using MoysIQPlatform.Shared.Models.Accounts;

namespace MoysIQPlatform.Server.Services.StudentService;

public interface IStudentService
{
	Task<ServiceResponse<string>> Register(Student student, string password);
	Task<ServiceResponse<string>> Login(string email, string password);
	Task<bool> StudentExists(string email);
}
