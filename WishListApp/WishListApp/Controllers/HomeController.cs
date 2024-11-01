using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using WishListApp.Models;
using WishListApp.Repository;
using WishListApp.Repository.Interfaces;

namespace WishListApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWishRepository _repository;
        private readonly IAdminRepository _adminRepository;

        public HomeController(IWishRepository repository, IAdminRepository adminRepository)
        {
            _repository = repository;
            _adminRepository = adminRepository;
        }
        public IActionResult Index()
        {
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
            return View();
        }

        public IActionResult WishItemAddPage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> HomePage(string email, string password)
        {
            await _repository.LogIn(email, password);
            var wishItems = await _repository.GetWishItems();
            return View("HomePage", wishItems);
        }

        public async Task<IActionResult> WishItemsPage()
        {
            var wishItems = await _repository.GetWishItems();
            if (wishItems == null)
            {
                return View("UnauthorizedPageError");
            }
            return View("WishItemsPage", wishItems);
        }

        public async Task<IActionResult> GetWishItemView(int id)
        {
            var wishItem = await _repository.GetWishItem(id);
            return View("WishItemViewPage", wishItem);
        }

        public async Task<IActionResult> EditWishItemPage(int id)
        {
            var wishItem = await _repository.GetWishItem(id);
            return View("EditWishItemPage", wishItem);
        }

        public async Task<IActionResult> EditWishItem(WishItem wishitem)
        {
            await _repository.UpdateWishItem(wishitem);
            return RedirectToAction("WishItemsPage");
        }

        public async Task<IActionResult> DeleteWishItem(int id)
        {
            await _repository.DeleteWishItem(id);
            return RedirectToAction("WishItemsPage");
        }

        public async Task<IActionResult> CreateWishItem(WishItem wishItem)
        {
            await _repository.CreateWishItem(wishItem);
            return RedirectToAction("WishItemsPage");
        }
 
        public async Task<IActionResult> AdminPage()
        {
            var users = await _adminRepository.GetUsers();
            return View(users);
        }

        public async Task<IActionResult> UserWishListPage(string Id)
        {
            var wishItems = await _adminRepository.GetUserWishList(Id);
            return View("AdminUsersWishItemsList", wishItems);
        }

        public async Task<IActionResult> AllStatsOfUser(string id)
        {
            var users = await _adminRepository.GetUserStats(id);
            return View(users);
        }

        public async Task<IActionResult> DeleteUser(string id) 
        { 
            await _adminRepository.DeleteUser(id);
            var users =  await _adminRepository.GetUsers();
            return View("AdminPage", users);
        }

        public IActionResult CreateUserPage()
        {
            return View();
        }

        public async Task<IActionResult> CreateUser(string email, string password, string roleName)
        {
            await _adminRepository.CreateUserWithRole(email, password, roleName);
            var users = await _adminRepository.GetUsers();
            return View("AdminPage", users);
        }

        public async Task<IActionResult> AddRoleToUserPage(string id)
        {
            var listOfRoles = await _adminRepository.GetUsersRoles(id);

            var model = new AddRoleUserViewModel()
            {
                UserId = id,
                UserRoleNameToAdd = null,
                UserRoles = listOfRoles
            };

            return View(model);
        }

        public async Task<IActionResult> AddRoleToUser(AddRoleUserViewModel model)
        {
            await _adminRepository.AddRoleToUser(model);
            var users = await _adminRepository.GetUsers();
            return View("AdminPage", users);
        }

        public async Task<IActionResult> DeleteUserRolePage(string id)
        {
            var listOfRoles = await _adminRepository.GetUsersRoles(id);

            var model = new AddRoleUserViewModel()
            {
                UserId = id,
                UserRoleNameToAdd = null,
                UserRoles = listOfRoles
            };

            return View(model);
        }

        public async Task<IActionResult> DeleteRoleUser(AddRoleUserViewModel model)
        {
            await _adminRepository.DeleteUserRole(model);
            var users = await _adminRepository.GetUsers();
            return View("AdminPage", users);
        }


    }
}
