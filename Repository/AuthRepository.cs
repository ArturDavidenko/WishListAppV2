using GraphQLWishList.Models;
using GraphQLWishList.Repository.Interfaces;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;

namespace GraphQLWishList.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _authURL;


        public AuthRepository(IOptions<ConnectionsOptions> connOption, HttpClient httpClient, IHttpContextAccessor httpContextAccessor) 
        {
            _authURL = connOption.Value.AuthURL;
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Login()
        {
            var email = "Davidenko";
            var password = "admin";

            var model = new LoginModel { employeerLastName = email, employeerPassword = password };


            var jsonContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var responseLogin = await _httpClient.PostAsync(_authURL, jsonContent);

            var token = await responseLogin.Content.ReadFromJsonAsync<JwtTokenResponse>();

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.Now.AddHours(1)
            };

            _httpContextAccessor.HttpContext.Response.Cookies.Append("authToken", token.token, cookieOptions);
            //_httpContextAccessor.HttpContext.Session.SetString("authToken", token.token);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.token);
        }


    }
}
