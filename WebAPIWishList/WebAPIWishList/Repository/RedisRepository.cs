using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using WebAPIWishList.Models;
using WebAPIWishList.Repository.Interfaces;

namespace WebAPIWishList.Repository
{
    public class RedisRepository
    {
        private readonly IDistributedCache _distributedCashe;
        private readonly IWishListRepository _wishListRepository;

        public RedisRepository(IDistributedCache distributedCache, IWishListRepository wishListRepository)
        {
            _distributedCashe = distributedCache;
            _wishListRepository = wishListRepository;
        }

        public async Task<ICollection<WishItem>> GetWishItemsListAsync(string userId)
        {
            string key = $"user-wish-items{userId}";

            string? cachedWishList = await _distributedCashe.GetStringAsync(key);

            ICollection<WishItem> WishList;
            
            if (string.IsNullOrEmpty(cachedWishList))

            {
                WishList = await _wishListRepository.GetWishItems(userId);

                if (WishList is null)
                {
                    return WishList;   
                }

                await _distributedCashe.SetStringAsync(key, JsonSerializer.Serialize(WishList));

                return WishList;
            }

            WishList = JsonSerializer.Deserialize<ICollection<WishItem>>(cachedWishList);

            return WishList;
        }

    }
}
