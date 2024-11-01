using System.Text.Json.Serialization;

namespace WishListApp.Models
{
    public class User
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("userName")]
        public string UserName { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("NormalizedUserName")]
        public string NormalizedUserName { get; set; }

        [JsonPropertyName("EmailConfirmed")]
        public bool EmailConfirmed { get; set; }

        [JsonPropertyName("PasswordHash")]
        public string PasswordHash { get; set; }

        [JsonPropertyName("PhoneNumberConfirmed")]
        public bool PhoneNumberConfirmed { get; set; }

        [JsonPropertyName("TwoFactorEnabled")]
        public bool TwoFactorEnabled { get; set; }

        [JsonPropertyName("LockoutEnd")]
        public TimeSpan LockoutEnd { get; set; }

        [JsonPropertyName("LockoutEnabled")]
        public bool LockoutEnabled { get; set; }

        [JsonPropertyName("AccessFailedCount")]
        public int AccessFailedCount { get; set; }


    }
}
