using System.Collections.Generic;
using System.Net.Http;

namespace Prana.APIAdapter.Models
{

    /// <summary>
    /// HTTP Request Parameters Class
    /// </summary>
    public class HTTPRequestParameters
    {
        /// <summary>
        /// HttpMethod
        /// </summary>
        public HttpMethod HttpMethod { get; set; }

        /// <summary>
        /// URL
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// MediaType
        /// </summary>
        public string MediaType { get; set; }

        /// <summary>
        /// Content
        /// </summary>
        public HttpContent Content { get; set; }

        /// <summary>
        /// Is Request Header Value Static
        /// </summary>
        public bool IsRequestHeaderValueStatic { get; set; }

        /// <summary>
        /// Http Headers
        /// </summary>
        public Dictionary<string, string> HttpHeader { get; set; }
    }
}
