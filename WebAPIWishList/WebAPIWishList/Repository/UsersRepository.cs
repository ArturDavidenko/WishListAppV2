using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebAPIWishList.Data;
using WebAPIWishList.Models;
using WebAPIWishList.Repository.Interfaces;

namespace WebAPIWishList.Repository
{
    public class UsersRepository : IUsersRepository
    {

        private readonly DBContext _context;
        public readonly UserManager<IdentityUser> _userManager;


        public UsersRepository(DBContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task AddRoleToUser(string id, string roleName)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == id);
            if (user != null) 
            {
                await _userManager.AddToRoleAsync(user, roleName);
            }
        }

        public void DeleteUser(string id)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        public ICollection<IdentityUser> GetUsers()
        {
            var users = _context.Users.OrderBy(x => x.Id).ToList();
            return users;
        }

        public ICollection<WishItem> GetUserWishList(string UserId)
        {
            var wishList = _context.wishItems.Where(x => x.UserId == UserId).OrderBy(x => x.UserId).ToList();
            return wishList;
        }

        public async Task AddUserWithRole(string email, string password, string roleName)
        {
            var user = new IdentityUser()
            {
                Email = email,
                UserName = email,
            };

            await _userManager.CreateAsync(user, password);

            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task DeleteRoleFromUser(string id, string roleName)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == id);

            if (user != null) 
            {
                await _userManager.RemoveFromRoleAsync(user, roleName);    
            }
        }

        public IdentityUser GetUser(string id)
        {
            return _context.Users.SingleOrDefault(u => u.Id == id);
        }

        public async Task<IList<string>> GetRolesOfUser(string id)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == id);

            var roles = await _userManager.GetRolesAsync(user);

            return roles;
        }
    }
}
