using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WishListApp.Controllers;
using WishListApp.Models;
using WishListApp.Repository;
using WishListApp.Repository.Interfaces;

namespace WishListUnitTestes.HomeControllerTest
{
    public class HomeControllerTestes
    {
        private readonly IWishRepository _wishRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly HomeController _homeController;


        public HomeControllerTestes()
        {

            //dependencies
            _wishRepository = A.Fake<IWishRepository>();
            _adminRepository = A.Fake<IAdminRepository>();

            //SUT
            _homeController = new HomeController(_wishRepository, _adminRepository);

        }


        [Fact]
        public void HomePage_ReturnSuccesss()
        {
            var email = "asdasd";
            var password = "test1asdasd";

            A.CallTo(() => _wishRepository.LogIn(email, password));

            var wishItems = A.Fake<List<WishItem>>();

            A.CallTo(() => _wishRepository.GetWishItems()).Returns(wishItems);

            // Создаем фейковый HttpContext
            var fakeHttpContext = A.Fake<HttpContext>();

            // Создаем фейковый заголовок авторизации
            fakeHttpContext.Request.Headers["Authorization"] = "Bearer faketoken123";

            // Присваиваем HttpContext контроллеру
            _homeController.ControllerContext = new ControllerContext
            {
                HttpContext = fakeHttpContext
            };


            var result = _homeController.HomePage(email, password);

            result.Should().BeOfType<Task<IActionResult>>();

        }

        [Fact]
        public void WishItemsPage_ReturnSuccess()
        {
            var expectedWishItems = A.Fake<List<WishItem>>();

            A.CallTo(() => _wishRepository.GetWishItems()).Returns(expectedWishItems);

            var result = _homeController.WishItemsPage();

            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void GetWishItemView_ReturnSuccess()
        {
            var expectedWishItem = A.Fake<WishItem>();

            A.CallTo(() => _wishRepository.GetWishItem(1)).Returns(expectedWishItem);

            var result = _homeController.GetWishItemView(1);

            result.Should().BeOfType<Task<IActionResult>>();
        }


        [Fact]
        public void EditWishItemPage_ReturnSuccess()
        {
            var expectedWishItem = A.Fake<WishItem>();

            A.CallTo(() => _wishRepository.GetWishItem(1)).Returns(expectedWishItem);

            var result = _homeController.EditWishItemPage(1);

            result.Should().BeOfType<Task<IActionResult>>();
        }


        [Fact]
        public void EditWishItem_ReturnSuccess()
        {
            var expectedWishItem = A.Fake<WishItem>();

            A.CallTo(() => _wishRepository.UpdateWishItem(expectedWishItem));

            var result = _homeController.EditWishItem(expectedWishItem);

            result.Should().BeOfType<Task<IActionResult>>();
        }


        [Fact]
        public void DeleteWishItem_ReturnSuccess()
        {
            A.CallTo(() => _wishRepository.DeleteWishItem(1));

            var result = _homeController.DeleteWishItem(1);

            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void CreateWishItem_ReturnSuccess()
        {
            var expectedWishItem = A.Fake<WishItem>();

            A.CallTo(() => _wishRepository.CreateWishItem(expectedWishItem));

            var result = _homeController.CreateWishItem(expectedWishItem);

            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void AdminPage_ReturnSuccess()
        {
            var expectedUsers = A.Fake<List<User>>();

            A.CallTo(() => _adminRepository.GetUsers()).Returns(expectedUsers);

            var result = _homeController.AdminPage();

            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void UserWishListPage_ReturnSuccess()
        {
            var expectedWishList = A.Fake<List<WishItem>>();
            
            A.CallTo(() => _adminRepository.GetUserWishList("asdasdas")).Returns(expectedWishList);

            var result = _homeController.UserWishListPage("asdasdsa");

            result.Should().BeOfType<Task<IActionResult>>();

        }

        [Fact]
        public void AllStatsOfUser_ReturnSuccess()
        {
            var expectedUser = A.Fake<User>();

            A.CallTo(() => _adminRepository.GetUserStats("asdasd")).Returns(expectedUser);

            var result = _homeController.AllStatsOfUser("asdasd");

            result.Should().BeOfType<Task<IActionResult>>();

        }


        [Fact]
        public void DeleteUser_ReturnSuccess()
        {
            var expectedUsers = A.Fake<List<User>>();

            A.CallTo(() => _adminRepository.DeleteUser("asdasd"));

            A.CallTo(() => _adminRepository.GetUsers()).Returns(expectedUsers);

            var result = _homeController.DeleteUser("asdasd");

            result.Should().BeOfType<Task<IActionResult>>();
        }


        [Fact]
        public void CreateUser_ReturnSuccess()
        {
            var expectedUsers = A.Fake<List<User>>();
            var email = "asdasdasd";
            var password = "asdsad123123";
            var roleName = "asdasd";

            A.CallTo(() => _adminRepository.CreateUserWithRole(email, password, roleName));

            A.CallTo(() => _adminRepository.GetUsers()).Returns(expectedUsers);

            var result = _homeController.CreateUser(email, password, roleName);

            result.Should().BeOfType<Task<IActionResult>>();

        }


        [Fact]
        public void AddRoleToUserPage_ReturnSuccess()
        {
            var expectedlistOfroles = A.Fake<List<string>>();

            A.CallTo(() => _adminRepository.GetUsersRoles("asdsad")).Returns(expectedlistOfroles);

            var result = _homeController.AddRoleToUserPage("asdasd");

            result.Should().BeOfType<Task<IActionResult>>();
        }


        [Fact]
        public void AddRoleToUser()
        {
            var expectedModel = A.Fake<AddRoleUserViewModel>();

            var expectedUsers = A.Fake<List<User>>();

            A.CallTo(() => _adminRepository.AddRoleToUser(expectedModel));

            A.CallTo(() => _adminRepository.GetUsers()).Returns(expectedUsers);

            var result = _homeController.AddRoleToUser(expectedModel);


            result.Should().BeOfType<Task<IActionResult>>();

        }


        [Fact]
        public void DeleteUserRolePage_ReturnSuccess()
        {
            var expectedlistOfroles = A.Fake<List<string>>();

            A.CallTo(() => _adminRepository.GetUsersRoles("asdsad")).Returns(expectedlistOfroles);

            var result = _homeController.DeleteUserRolePage("asd");

            result.Should().BeOfType<Task<IActionResult>>();
        }


        [Fact]
        public void DeleteRoleUser_ReturnSuccess()
        {
            var expectedModel = A.Fake<AddRoleUserViewModel>();

            var expectedUsers = A.Fake<List<User>>();

            A.CallTo(() => _adminRepository.DeleteUserRole(expectedModel));

            A.CallTo(() => _adminRepository.GetUsers()).Returns(expectedUsers);

            var result = _homeController.DeleteRoleUser(expectedModel);

            result.Should().BeOfType<Task<IActionResult>>();
        }


    }
}
