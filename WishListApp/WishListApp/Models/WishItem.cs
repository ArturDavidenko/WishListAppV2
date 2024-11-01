using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WishListApp.Models
{
    public class WishItem
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

    }
}
