using WishListApp.Models;

namespace WishListApp.Repository.Interfaces
{
    public interface IAdminRepository
    {
        public Task<List<User>> GetUsers();

        public Task<List<WishItem>> GetUserWishList(string userId);

        public Task<User> GetUserStats(string userId);

        public Task DeleteUser(string userId);

        public Task CreateUserWithRole(string email, string password, string roleName);

        public Task AddRoleToUser(AddRoleUserViewModel model);

        public Task<List<string>> GetUsersRoles(string userId);

        public Task DeleteUserRole(AddRoleUserViewModel model);
    }
}
