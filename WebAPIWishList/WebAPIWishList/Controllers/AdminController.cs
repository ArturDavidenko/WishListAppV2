using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAPIWishList.Dto;
using WebAPIWishList.Models;
using WebAPIWishList.Repository.Interfaces;

namespace WebAPIWishList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        public readonly IUsersRepository _usersRepository;
        private readonly IMapper _mapper;
        public readonly UserManager<IdentityUser> _userManager;

        public AdminController(IUsersRepository usersRepository, UserManager<IdentityUser> userManager, IMapper mapper)
        {
            _usersRepository = usersRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<IdentityUser>))]
        public IActionResult GetUsersList()
        {
            try
            {
                var usersList = _usersRepository.GetUsers().ToList();
                return Ok(usersList);
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        [HttpGet("get-user-wishlist/{UserId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<WishItem>))]
        public IActionResult GetUserWishList(string UserId)
        {
            try
            {
                var userWishList = _mapper.Map<List<WishItemDto>>(_usersRepository.GetUserWishList(UserId).ToList());
                return Ok(userWishList);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("isAdmin")]
        public async Task<IActionResult> IsAdmin()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var isAdmin = roles.Contains("Admin");

            return Ok(new { IsAdmin = isAdmin });
        }


        [HttpDelete("{userId}")]
        public IActionResult DeleteUser(string userId)
        {
            try
            {
                _usersRepository.DeleteUser(userId);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("add-role/{userId}/{roleNameToAdd}")]
        public async Task<IActionResult> AddRoleToUser(string userId, string roleNameToAdd)
        {
            try
            {
                await _usersRepository.AddRoleToUser(userId, roleNameToAdd);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("crete-user-withRole/{userEmail}/{userPassword}/{roleName}")]
        public async Task<IActionResult> CreateUserWithRole(string userEmail,string userPassword, string roleName)
        {
            try
            {
                await _usersRepository.AddUserWithRole(userEmail, userPassword, roleName);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            } 
        }


        [HttpPost("delete-role/{userId}/{roleNameToDelete}")]
        public async Task<IActionResult> DeleteRoleFromUser(string userId, string roleNameToDelete)
        {
            try
            {
                await _usersRepository.DeleteRoleFromUser(userId, roleNameToDelete);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        [HttpGet("get-user/{UserId}")]
        public IActionResult GetUser(string UserId) 
        {
            try
            {
                var user =  _usersRepository.GetUser(UserId);
                return Ok(user);
            }
            catch (Exception)
            {
                return BadRequest(); 
            }
        }

        [HttpGet("get-user-roles/{UserId}")]
        public async Task<IActionResult> GetUserRoles(string UserId)
        {
            try
            {
                var roles =  await _usersRepository.GetRolesOfUser(UserId);
                return Ok(roles);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

    }
}
