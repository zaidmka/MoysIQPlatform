using MoysIQPlatform.Shared.Models;
using MoysIQPlatform.Shared.Models.Accounts;

namespace MoysIQPlatform.Server.Services.EmployeeService
{
	public interface IEmployeeService
	{
		Task<ServiceResponse<UserDto>> EmployeeRegister(Employee employee, string password);
		Task<bool> EmployeeExists(string email);
		Task<ServiceResponse<string>> Login(string email, string password);
	}
}
