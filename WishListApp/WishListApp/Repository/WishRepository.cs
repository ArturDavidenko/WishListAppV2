using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using WishListApp.Models;
using WishListApp.Repository.Interfaces;

namespace WishListApp.Repository
{
    public class WishRepository : IWishRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _requestURL;

        public WishRepository(IOptions<ApiSettings> apiSettings, HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _requestURL = apiSettings.Value.RequestURL;
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task CreateWishItem(WishItem wishItem)
        {
            SetUpRequestHeaderAuthorization();
            var jsonContent = new StringContent(JsonSerializer.Serialize(wishItem), Encoding.UTF8, "application/json");
            await _httpClient.PostAsync(_requestURL, jsonContent);
        }

        public async Task DeleteWishItem(int id)
        {
            SetUpRequestHeaderAuthorization();
            await _httpClient.DeleteAsync($"{_requestURL}/{id}");
        }

        public async Task<WishItem> GetWishItem(int id)
        {
            SetUpRequestHeaderAuthorization();
            HttpResponseMessage response = await _httpClient.GetAsync($"{_requestURL}/{id}");

            string jsonResponse = await response.Content.ReadAsStringAsync();
            WishItem wishItem = JsonSerializer.Deserialize<WishItem>(jsonResponse);
            return wishItem;
        }

        public async Task<List<WishItem>> GetWishItems()
        {
            SetUpRequestHeaderAuthorization();
            HttpResponseMessage response = await _httpClient.GetAsync(_requestURL);
             
            string jsonResponse = await response.Content.ReadAsStringAsync();
            List<WishItem> wishItems = JsonSerializer.Deserialize<List<WishItem>>(jsonResponse);
            return wishItems;
        }

        public async Task<bool> isAdmin()
        {
            SetUpRequestHeaderAuthorization();
            HttpResponseMessage response = await _httpClient.GetAsync("https://localhost:7043/api/Admin/isAdmin");
            string jsonResponse = await response.Content.ReadAsStringAsync();
            bool isAdmin = JsonSerializer.Deserialize<bool>(jsonResponse);
            return isAdmin;
        }

        public async Task LogIn(string email, string password)
        {
            var loginViewModel = new LoginViewModel
            {
                Email = email,
                Password = password
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(loginViewModel), Encoding.UTF8, "application/json");
            var responseLogin = await _httpClient.PostAsync("https://localhost:7043/api/Auth/login", jsonContent);
            
            var token = await responseLogin.Content.ReadFromJsonAsync<JwtTokenResponse>();
            
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.Now.AddHours(1)
            };
            
            _httpContextAccessor.HttpContext.Response.Cookies.Append("authToken", token.token, cookieOptions);
            _httpContextAccessor.HttpContext.Session.SetString("authToken", token.token);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.token);
        }

        public async Task UpdateWishItem(WishItem item)
        {
            WishItem itemToSave = new WishItem
            {
                Title = item.Title,
                Description = item.Description
            };

            SetUpRequestHeaderAuthorization();
            var json = JsonSerializer.Serialize(itemToSave);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await _httpClient.PutAsync($"{_requestURL}/{item.Id}", content); 
        }

        public void SetUpRequestHeaderAuthorization()
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            if (token == null)
            {
                return;
            }
        }

        public async Task SetPopularityValue(int wishId)
        {
            SetUpRequestHeaderAuthorization();
            await _httpClient.PutAsync($"{_requestURL}/popularity-control/{wishId}", null);
        }


    }
}
