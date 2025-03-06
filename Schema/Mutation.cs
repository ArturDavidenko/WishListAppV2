using GraphQLWishList.Models;
using GraphQLWishList.Repository.Interfaces;

namespace GraphQLWishList.Schema
{
    public class Mutation
    {
        private readonly IEmployeerRepository _employeerRepository;
        public Mutation(IEmployeerRepository employeerRepository) 
        { 
            _employeerRepository = employeerRepository;
        }

        public async Task<bool> CreateEmployeer(string password, string firstname, string lastname, string role, string phonenumber)
        {
            try
            {
                var model = new RegisterEmployeerModel
                {
                    Password = password,
                    FirstName = firstname,
                    LastName = lastname,
                    Role = role,
                    PhoneNumber = phonenumber
                };

                await _employeerRepository.CreateEmployeer(model);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
