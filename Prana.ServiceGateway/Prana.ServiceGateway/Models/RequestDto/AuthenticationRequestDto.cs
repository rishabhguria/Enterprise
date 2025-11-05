using System.Text.Json.Serialization;

namespace Prana.ServiceGateway.Models.RequestDto
{

    public class AuthenticationRequestDto : BaseRequestDto
    {
        public string UserName { get; set; }

        public object Password { get; set; }

        public string MsalToken { get; set; }



        /// <summary>
        /// Can be multiple clientids in csv from config
        /// </summary>
        [JsonIgnore]
        public string ClientIds { get; set; }



        /// <summary>
        /// Can be multiple Issuers in csv from config
        /// </summary>
        [JsonIgnore]
        public string Issuers { get; set; }
    }
}
