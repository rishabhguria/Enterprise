using Bloomberglp.Blpapi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.SAPIAdapter.Models
{
    public class BloombergAccessTokenDTO
    {
        public string GrantType { get; set; }
        public string Code { get; set; }
        public string CodeVerifier { get; set; }
        public string ClientId { get; set; }
        public string RedirectUri { get; set; }
        public int UserId { get; set; }
    }

    public class TokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }

    public class UserIdentity
    {
        public string Topic { get; set; }

        public Identity Identity { get; set; }

        public UserIdentity(string topic, Identity identity)
        {
            this.Topic = topic;
            this.Identity = identity;
        }
    }

}
