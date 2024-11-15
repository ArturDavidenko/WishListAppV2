using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using StackExchange.Redis;
using System.Text.Json;
using WebAPIWishList.Dto;
using WebAPIWishList.Models;
using WebAPIWishList.Repository.Interfaces;

namespace WebAPIWishList.Repository
{
    public class RedisRepository
    {
        private readonly IDistributedCache _distributedCashe;
        private readonly IWishListRepository _wishListRepository;
        private readonly IDatabase _redisDb;
        private readonly StackExchange.Redis.IServer _redisServer;
        string PopularWisheskey = "popular_wishes";

        public RedisRepository(IDistributedCache distributedCache, IWishListRepository wishListRepository, IConnectionMultiplexer connectionMultiplexer)
        {
            _distributedCashe = distributedCache;
            _wishListRepository = wishListRepository;
            _redisDb = connectionMultiplexer.GetDatabase();
            _redisServer = connectionMultiplexer.GetServer("redis-16632.c274.us-east-1-3.ec2.redns.redis-cloud.com", 16632);
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

        public async Task AddWishViewToSortedSetAndIncrementAsync(int wishId)
        {
            await _redisDb.SortedSetIncrementAsync(PopularWisheskey, $"wish:{wishId}", 1); 
        }

        public async Task<SortedSetEntry[]> GetPopularsWishes()
        {
            return await _redisDb.SortedSetRangeByScoreWithScoresAsync("popular_wishes", order: Order.Descending, take: 10);
        }

        public async Task DeleteSortedSet(int wishId)
        {
            await _redisDb.SortedSetRemoveAsync(PopularWisheskey, $"wish:{wishId}");
        }

        public async Task DeleteAllCache()
        {
            var keys = _redisServer.Keys(pattern: "user-wish-items*");

            foreach (var key in keys) 
            {
                await _redisDb.KeyDeleteAsync(key);
            }
        }

        public async Task DeleteCacheKey(int wishId, string userId)
        {
            var listKey = $"user-wish-items{userId}";

            string json = await _distributedCashe.GetStringAsync(listKey);

            var wishItems = JsonSerializer.Deserialize<List<WishItem>>(json);

            if (wishItems == null)
            {
                return; 
            }

            var itemToRemove = wishItems.FirstOrDefault(w => w.Id == wishId);
            if (itemToRemove != null)
            {
                wishItems.Remove(itemToRemove);
            }

            await _distributedCashe.SetStringAsync(listKey, JsonSerializer.Serialize(wishItems));
        }

        public async Task UpdateWishItemsCacheAsync(int wishId, string userId, WishItemDto updatedWishItem)
        {
            var listKey = $"user-wish-items{userId}";

            string json = await _distributedCashe.GetStringAsync(listKey);

            var wishItems = JsonSerializer.Deserialize<List<WishItem>>(json);

            var itemToUpdate = wishItems.FirstOrDefault(w => w.Id == wishId);

            itemToUpdate.Description = updatedWishItem.Description;
            itemToUpdate.Title = updatedWishItem.Title;

            await _distributedCashe.SetStringAsync(listKey, JsonSerializer.Serialize(wishItems));
        }


    }
}
