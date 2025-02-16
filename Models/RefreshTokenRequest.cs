
using System.Text.Json.Serialization;

namespace SnackUpAPI.Models
{

    public class RefreshTokenRequest
    {
        [JsonPropertyName("token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
    }

}
