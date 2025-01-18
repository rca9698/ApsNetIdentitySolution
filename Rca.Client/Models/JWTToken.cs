using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Rca.Client.Models
{
    public class JWTToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("expires_at")]
        public DateTime ExpiresAt { get; set; }     
    }
}
