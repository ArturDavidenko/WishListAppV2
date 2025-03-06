using GraphQLWishList.Models;
using GraphQLWishList.Repository.Interfaces;
using Microsoft.Extensions.Options;

namespace GraphQLWishList.Schema
{
    public class Query
    {
        private readonly IAuthRepository _authRepository;
        private readonly IEmployeerRepository _employeerRepository;
        public Query(IAuthRepository authRepository, IEmployeerRepository employeerRepository)
        {
            _authRepository = authRepository;
            _employeerRepository = employeerRepository;
        }

        public async Task<string> Login()
        {
            await _authRepository.Login();
            return "Login successful";
        }

        public async Task<List<Employeer>> GetAllEmployeersList()
        {
           return await _employeerRepository.GetAllEmployeersList();
        }


        [GraphQLDeprecated("Ussless one!")]
        public string Test => ("HAJ DEBIL!");
    }
}
