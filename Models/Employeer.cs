namespace GraphQLWishList.Models
{
    public class Employeer
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public string PasswordHash { get; set; }
        public string ImageId { get; set; }
        public List<string> Orders { get; set; }
    }
}
