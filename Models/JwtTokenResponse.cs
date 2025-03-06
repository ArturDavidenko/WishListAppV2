using System.Text.Json.Serialization;

namespace GraphQLWishList.Models
{
    public class JwtTokenResponse
    {
        [JsonPropertyName("token")]
        public string token { get; set; }
    }
}
