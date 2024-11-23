using System.Text.Json.Serialization;

namespace WishListApp.Models
{
    public class PopularityWishItemsViewModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        [JsonPropertyName("countOfView")]
        public double CountOfView { get; set; }

        [JsonPropertyName("userName")]
        public string UserName { get; set; }
    }
}
