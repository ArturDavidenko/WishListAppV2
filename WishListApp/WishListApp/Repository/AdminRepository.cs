using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using WishListApp.Models;
using WishListApp.Repository.Interfaces;

namespace WishListApp.Repository
{
    public class AdminRepository : IAdminRepository
    {

        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AdminRepository(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;  
        }

        public async Task<List<User>> GetUsers()
        {
            SetUpRequestHeaderAuthorization();

            HttpResponseMessage response = await _httpClient.GetAsync("https://localhost:7043/api/Admin");
            string jsonResponse = await response.Content.ReadAsStringAsync();
            List<User> usersList = JsonSerializer.Deserialize<List<User>>(jsonResponse);

            return usersList;
        }

        public async Task<List<WishItem>> GetUserWishList(string userId)
        {
            SetUpRequestHeaderAuthorization();

            HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:7043/api/Admin/get-user-wishlist/{userId}");


            string jsonResponse = await response.Content.ReadAsStringAsync();

            List<WishItem> wishItems = JsonSerializer.Deserialize<List<WishItem>>(jsonResponse);

            return wishItems;
        }

        public async Task<User> GetUserStats(string userId)
        {
            SetUpRequestHeaderAuthorization();

            HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:7043/api/Admin/get-user/{userId}");
            string jsonResponse = await response.Content.ReadAsStringAsync();

            User user = JsonSerializer.Deserialize<User>(jsonResponse);

            return user;

        }

        public async Task DeleteUser(string userId)
        {
            SetUpRequestHeaderAuthorization();

            await _httpClient.DeleteAsync($"https://localhost:7043/api/Admin/{userId}");
            
        }

        public async Task CreateUserWithRole(string email, string password, string roleName)
        {
            SetUpRequestHeaderAuthorization();

            await _httpClient.PostAsync($"https://localhost:7043/api/Admin/crete-user-withRole/{email}/{password}/{roleName}", null);
        }

        public async Task AddRoleToUser(AddRoleUserViewModel model)
        {
            SetUpRequestHeaderAuthorization();

            await _httpClient.PostAsync($"https://localhost:7043/api/Admin/add-role/{model.UserId}/{model.UserRoleNameToAdd}", null);
        }

        public async Task<List<string>> GetUsersRoles(string userId)
        {
            SetUpRequestHeaderAuthorization();

            var listresponse = await _httpClient.GetAsync($"https://localhost:7043/api/Admin/get-user-roles/{userId}");
            string jsonResponse = await listresponse.Content.ReadAsStringAsync();

            List<string> listOfRoles = JsonSerializer.Deserialize<List<string>>(jsonResponse);

            return listOfRoles;
        }

        public async Task DeleteUserRole(AddRoleUserViewModel model)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            await _httpClient.PostAsync($"https://localhost:7043/api/Admin/delete-role/{model.UserId}/{model.UserRoleNameToAdd}", null);
        }

        public void SetUpRequestHeaderAuthorization()
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

    }
}
