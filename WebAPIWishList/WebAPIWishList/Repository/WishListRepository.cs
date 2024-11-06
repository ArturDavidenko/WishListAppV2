using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using WebAPIWishList.Data;
using WebAPIWishList.Models;
using WebAPIWishList.Repository.Interfaces;

namespace WebAPIWishList.Repository
{
    public class WishListRepository : IWishListRepository
    {
        private readonly DBContext _context;
        
   
        public WishListRepository(DBContext context) 
        { 
            _context = context;
        }

        public WishItem GetWishItem(int id)
        {
            return _context.wishItems.Where(x => x.Id == id).SingleOrDefault(); 
        }

        public WishItem GetWishItem(string title)
        {
            return _context.wishItems.Where(x => x.Title == title).SingleOrDefault(); 
        }

        public bool WishItemExists(int id)
        {
            return _context.wishItems.Any(x => x.Id == id);
        }

        public async Task<ICollection<WishItem>> GetWishItems(string userId) 
        {
            return _context.wishItems.Where(w => w.UserId == userId).ToList();
        }

        public bool CreateWishItem(WishItem wishItem)
        {
            _context.wishItems.Add(wishItem);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool UpdateWishItem(WishItem updateWishItem)
        {
            _context.wishItems.Update(updateWishItem);
            return Save();
        }

        public bool DeleteWishItem(WishItem wishItem)
        {
            _context.wishItems.Remove(wishItem);
            return Save();
        }
    }
}
