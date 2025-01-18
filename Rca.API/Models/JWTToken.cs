using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Rca.API.Models
{
    public class JWTToken
    {
        [JsonProperty("acces_token")]
        public string AccessToken { get; set; }
        [JsonProperty("expires_at")]
        public string ExpiresAt { get; set; }     
    }
}
