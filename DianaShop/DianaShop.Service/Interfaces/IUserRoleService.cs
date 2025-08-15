using DianaShop.Data.Entities;
using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;

namespace DianaShop.Service.Interfaces
{
    public interface IUserRoleService
    {
        Task AcceptRoleRequestAsync(int userId, int roleId);
        Task AddRoleAsync(UserRoleRequestModels role);
        Task<IEnumerable<UserRole>> GetAllPendingRoleRequestsAsync();
        Task<IEnumerable<UserRoleResponseModel>> GetRolesOfUserAsync(int userId);
        Task RemoveRoleAsync(int userId, int roleId);
        Task RequestRoleAsync(int userId, int roleId);
        Task UpdateRolesAsync(int userId, List<int> newRoleIds);
        bool UserHasRole(int userId, string roleName);
    }
}