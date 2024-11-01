namespace WishListApp.Models
{
    public class AddRoleUserViewModel
    {
        public string UserId { get; set; }
        public string UserRoleNameToAdd { get; set; }
        public List<string> UserRoles { get; set; }
    }
}
