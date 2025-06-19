using MoysIQPlatform.Shared.Models;
using MoysIQPlatform.Shared.Models.Accounts;

namespace MoysIQPlatform.Client.Services.EmployeeService
{
	public interface IEmployeeService
	{
		Task<ServiceResponse<int>> EmployeeRegister(EmployeeRegister request);
		Task<ServiceResponse<string>> EmployeeLogin(EmployeeLogin request);
		Task<ServiceResponse<bool>> ChangePassword(EmployeeChangePassword request);
	}
}
