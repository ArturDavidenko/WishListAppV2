using GraphQLWishList.Models;
using GraphQLWishList.Repository.Interfaces;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text;

namespace GraphQLWishList.Repository
{
    public class EmployeerRepository : IEmployeerRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _adminURL;


        public EmployeerRepository(IOptions<ConnectionsOptions> connOption, IHttpClientFactory httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _adminURL = connOption.Value.AdminUrl;
            _httpClient = httpClient.CreateClient();
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task CreateEmployeer(RegisterEmployeerModel model)
        {
            
            var jsonContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

            await _httpClient.PostAsync($"{_adminURL}/register-new-employeer", jsonContent);
        }

        public async Task<List<Employeer>> GetAllEmployeersList()
        {
            var response = await _httpClient.GetAsync($"{_adminURL}/get-all-employeers");
            return await response.Content.ReadFromJsonAsync<List<Employeer>>();
        }
    }
}
