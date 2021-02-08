using System.Collections.Generic;
using System.Threading.Tasks;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.Library.API
{
	public interface IUserEndpoint
	{
		Task<List<UserModel>> GetAll();
		Task<Dictionary<string, string>> GetAllRoles();
		Task AddUserToRole(string userId, string role);
		Task RemoveUserFromRole(string userId, string role);
	}
}