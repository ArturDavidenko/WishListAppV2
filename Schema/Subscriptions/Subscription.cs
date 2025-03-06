using GraphQLWishList.Models;

namespace GraphQLWishList.Schema.Subscriptions
{
    public class Subscription
    {
        [Subscribe]
        public RegisterEmployeerModel EmployeerAdded([EventMessage] RegisterEmployeerModel model) => model;
    }
}
