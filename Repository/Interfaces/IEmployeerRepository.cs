using GraphQLWishList.Models;

namespace GraphQLWishList.Repository.Interfaces
{
    public interface IEmployeerRepository
    {
        public Task CreateEmployeer(RegisterEmployeerModel model);

        public Task<List<Employeer>> GetAllEmployeersList();
    }
}
