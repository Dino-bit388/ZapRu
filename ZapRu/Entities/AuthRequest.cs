using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZapRu.Entities
{
    public class AuthRequest
    {
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; } = string.Empty;
        [JsonPropertyName("lastName")]
        public string LastName { get; set; } = string.Empty;
        [JsonPropertyName("middleName")]
        public string MiddleName { get; set; } = string.Empty;
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;

        [JsonPropertyName("phone")]
        public string Phone { get; set; } = string.Empty;

        [JsonPropertyName("agree")]
        public bool Agree { get; set; } = false;

        [JsonPropertyName("rememberMe")]
        public bool RememberMe { get; set; } = false;
    }
}
