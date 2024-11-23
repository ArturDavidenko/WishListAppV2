namespace WebAPIWishList.Models
{
    public class PopularityWishItemsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public double CountOfView { get; set; }

        public string UserName { get; set; }
    }
}
