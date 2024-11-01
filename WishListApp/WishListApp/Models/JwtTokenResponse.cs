using System.Text.Json.Serialization;

namespace WishListApp.Models
{
    public class JwtTokenResponse
    {
        [JsonPropertyName("token")]
        public string token { get; set; }
    }
}
