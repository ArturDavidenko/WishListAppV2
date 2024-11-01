﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAPIWishList.Data;
using WebAPIWishList.Dto;
using WebAPIWishList.Models;
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

        public WishItemController(IWishListRepository wishListRepository, IMapper mapper)
        {
            _wishListRepository = wishListRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<WishItem>))]
        public IActionResult GetWishList()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var WishLists = _mapper.Map<List<WishItemDto>>(_wishListRepository.GetWishItems(userId));
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
        public IActionResult CreateWishItem([FromBody] WishItemDto wishItemCreate)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var wishItemMap = _mapper.Map<WishItem>(wishItemCreate);
                wishItemMap.UserId = userId;
                _wishListRepository.CreateWishItem(wishItemMap);
                return NoContent();
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
        public IActionResult UpdateWishItem([FromBody]WishItemDto updateWishItem, int wishItemId)
        {
            try
            {
                var wishItemMap = _mapper.Map<WishItem>(updateWishItem);
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                wishItemMap.UserId = userId;
                wishItemMap.Id = wishItemId;
                _wishListRepository.UpdateWishItem(wishItemMap);
                return NoContent();
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
        public IActionResult DeleteWishItem(int wishItemId) 
        {
            try
            {
                var wishItemToDelete = _wishListRepository.GetWishItem(wishItemId);
                _wishListRepository.DeleteWishItem(wishItemToDelete);
                return NoContent();
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
    }
}
