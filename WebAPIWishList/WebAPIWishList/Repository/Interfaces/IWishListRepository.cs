using WebAPIWishList.Models;

namespace WebAPIWishList.Repository.Interfaces
{
    public interface IWishListRepository
    {
        Task<ICollection<WishItem>> GetWishItems(string userId);

        WishItem GetWishItem(int id);

        WishItem GetWishItem(string title);

        bool WishItemExists(int id);

        bool CreateWishItem(WishItem wishItem);

        bool Save();

        bool UpdateWishItem(WishItem wishItem);

        bool DeleteWishItem(WishItem wishItem);

    }
}
