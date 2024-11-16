using WishListApp.Models;

namespace WishListApp.Repository.Interfaces
{
    public interface IWishRepository
    {
        public Task<List<WishItem>> GetWishItems();
        
        public Task<WishItem> GetWishItem(int id);

        public Task DeleteWishItem(int id);

        public Task CreateWishItem(WishItem wishItem);

        public Task UpdateWishItem(WishItem item);

        public Task LogIn(string email, string password);

        public Task<bool> isAdmin();

        public Task SetPopularityValue(int wishId);

        public Task<List<PopularityWishItemsViewModel>> GetPopularityWishesList();
    }
}
