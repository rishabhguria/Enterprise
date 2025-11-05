using Prana.APIAdapter.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace Prana.APIAdapter.Interfaces
{
    /// <summary>
    /// IHttpRequestManager interface
    /// </summary>
    public interface IHttpRequestManager
    {
        /// <summary>
        /// HTTPs the get asynchronous.
        /// </summary>
        /// <param name="httpRequestParameters">HTTPRequestParameters.</param>
        /// <returns></returns>
        Task<HttpResponseMessage> HttpGetAsync(HTTPRequestParameters httpRequestParameters);

        /// <summary>
        /// HTTPs the post asynchronous.
        /// </summary>
        /// <param name="httpRequestParameters">HTTPRequestParameters.</param>
        /// <returns></returns>
        Task<HttpResponseMessage> HttpPostAsync(HTTPRequestParameters httpRequestParameters);

        /// <summary>
        /// HTTPs the put asynchronous.
        /// </summary>
        /// <param name="httpRequestParameters">HTTPRequestParameters.</param>
        /// <returns></returns>
        Task<HttpResponseMessage> HttpPutAsync(HTTPRequestParameters httpRequestParameters);

        /// <summary>
        /// HTTPs the delete asynchronous.
        /// </summary>
        /// <param name="httpRequestParameters">HTTPRequestParameters.</param>
        /// <returns></returns>
        Task<HttpResponseMessage> HttpDeleteAsync(HTTPRequestParameters httpRequestParameters);
    }

}
