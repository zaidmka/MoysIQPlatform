using MoysIQPlatform.Shared.Models;
using MoysIQPlatform.Shared.Models.Accounts;

namespace MoysIQPlatform.Client.Services.EmployeeService
{
	public interface IEmployeeService
	{
		Task<ServiceResponse<UserDto>> Register(EmployeeRegister request);
		Task<ServiceResponse<string>> Login(EmployeeLogin request);
		Task Logout();
	}
}
