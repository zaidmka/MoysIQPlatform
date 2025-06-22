using MoysIQPlatform.Shared.Models;
using MoysIQPlatform.Shared.Models.Accounts;

namespace MoysIQPlatform.Client.Services.StudentService
{
	public interface IStudentService
	{
		Task<ServiceResponse<string>> Register(StudentRegister request);
		Task<ServiceResponse<string>> Login(StudentLogin request);
		Task Logout();
	}
}
