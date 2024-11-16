using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAPIWishList.Data;
using WebAPIWishList.Dto;
using WebAPIWishList.Models;
using WebAPIWishList.Repository;
using WebAPIWishList.Repository.Interfaces;

namespace WebAPIWishList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WishItemController : Controller
    {
        private readonly IWishListRepository _wishListRepository;
        private readonly IMapper _mapper;
        private readonly RedisRepository _redisRepository;

        public WishItemController(IWishListRepository wishListRepository, IMapper mapper, RedisRepository redisRepository)
        {
            _wishListRepository = wishListRepository;
            _mapper = mapper;
            _redisRepository = redisRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<WishItem>))]
        public async Task<IActionResult> GetWishList()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var WishLists = _mapper.Map<List<WishItemDto>>(await _redisRepository.GetWishItemsListAsync(userId));
                return Ok(WishLists);
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        [HttpGet("{wishId}")]
        [ProducesResponseType(200, Type = typeof(WishItem))]
        [ProducesResponseType(400)]
        public IActionResult GetWishListItem(int wishId)
        {
            try
            {
                var wishItem = _mapper.Map<WishItemDto>(_wishListRepository.GetWishItem(wishId));
                return Ok(wishItem);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateWishItem([FromBody] WishItemDto wishItemCreate)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var wishItemMap = _mapper.Map<WishItem>(wishItemCreate);
                wishItemMap.UserId = userId;
                await _redisRepository.DeleteAllCache();
                _wishListRepository.CreateWishItem(wishItemMap);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut("{wishItemId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateWishItem([FromBody]WishItemDto updateWishItem, int wishItemId)
        {
            try
            {
                var wishItemMap = _mapper.Map<WishItem>(updateWishItem);
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                wishItemMap.UserId = userId;
                wishItemMap.Id = wishItemId;
                await _redisRepository.UpdateWishItemsCacheAsync(wishItemId, userId, updateWishItem);
                _wishListRepository.UpdateWishItem(wishItemMap);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }


        }

        [HttpDelete("{wishItemId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteWishItem(int wishItemId) 
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _redisRepository.DeleteSortedSet(wishItemId);
                await _redisRepository.DeleteCacheKey(wishItemId, userId);
                var wishItemToDelete = _wishListRepository.GetWishItem(wishItemId);
                _wishListRepository.DeleteWishItem(wishItemToDelete);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut("popularity-control/{wishItemId}")]
        public async Task<IActionResult> PopularityAction(int wishItemId)
        {
            try
            {
                await _redisRepository.AddWishViewToSortedSetAndIncrementAsync(wishItemId);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
            
        }


        [HttpGet("popular-wishes-list")]
        [ProducesResponseType(200, Type = typeof(List<PopularityWishItemsViewModel>))]
        public async Task<IActionResult> GetPopularityWishItemsList()
        {
            try
            {
                var PopularWishes = await _redisRepository.GetPopularsWishes();
                return Ok(PopularWishes);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

    }
}
