using GraphQLWishList.Models;
using GraphQLWishList.Repository.Interfaces;
using GraphQLWishList.Schema.Subscriptions;
using HotChocolate.Subscriptions;

namespace GraphQLWishList.Schema
{
    public class Mutation
    {
        private readonly IEmployeerRepository _employeerRepository;
        public Mutation(IEmployeerRepository employeerRepository) 
        { 
            _employeerRepository = employeerRepository;
        }

        public async Task<bool> CreateEmployeer(RegisterEmployeerModel model, [Service] ITopicEventSender topicEventSender)
        {
            try
            {
                await _employeerRepository.CreateEmployeer(model);
                await topicEventSender.SendAsync(nameof(Subscription.EmployeerAdded), model);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
