using Microsoft.AspNetCore.Identity;
using WebAPIWishList.Models;

namespace WebAPIWishList.Repository.Interfaces
{
    public interface IUsersRepository
    {
        public ICollection<IdentityUser> GetUsers();

        public ICollection<WishItem> GetUserWishList(string UserId);

        public void DeleteUser(string id);

        public Task AddRoleToUser(string id, string roleName);

        public Task AddUserWithRole(string email, string password, string roleName);

        public Task DeleteRoleFromUser(string id, string roleName);

        public IdentityUser GetUser(string id);

        public Task<IList<string>> GetRolesOfUser(string id);

    }
}
