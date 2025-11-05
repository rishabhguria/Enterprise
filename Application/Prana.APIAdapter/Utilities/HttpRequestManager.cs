using Prana.APIAdapter.Interfaces;
using Prana.APIAdapter.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Prana.APIAdapter.Utilities
{
    class HttpRequestManager : IHttpRequestManager
    {
        /// <summary>
        /// The HTTP client
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        /// The semaphore slim
        /// </summary>
        static readonly SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequestManager"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="defaultTimeOut">The default time out.</param>
        public HttpRequestManager(HttpClient httpClient, double defaultTimeOut)
        {
            // To handle Certificate Validation errors.
            ServicePointManager.ServerCertificateValidationCallback +=
                (sender, cert, chain, sslPolicyErrors) => true;
            _httpClient = httpClient;
            _httpClient.Timeout = TimeSpan.FromSeconds(defaultTimeOut);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                      SecurityProtocolType.Tls11 |
                                      SecurityProtocolType.Tls12;
        }


        /// <summary>
        /// Sends the HTTP GET Request for input Target URL.
        /// </summary>
        /// <param name="httpRequestParameters">HTTPRequestParameters.</param>
        /// <returns>Returns HttpResponseMessage object.</returns>
        public async Task<HttpResponseMessage> HttpGetAsync(HTTPRequestParameters httpRequestParameters)
        {
            try
            {
                await SemaphoreSlim.WaitAsync();
                try
                {
                    _httpClient.DefaultRequestHeaders.Clear();

                    // Define request data format.
                    _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(httpRequestParameters.MediaType));

                    // Invoking the HTTP GET Type Request.
                    HttpResponseMessage response = await _httpClient.GetAsync(httpRequestParameters.URL);
                    return response;
                }
                finally
                {
                    SemaphoreSlim.Release();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Posts the HTTP POST Request for input Target URL.
        /// </summary>
        /// <param name="httpRequestParameters">HTTPRequestParameters.</param>
        /// <returns>Returns HttpResponseMessage object.</returns>
        public async Task<HttpResponseMessage> HttpPostAsync(HTTPRequestParameters httpRequestParameters)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Clear();

                if (httpRequestParameters.HttpHeader != null && httpRequestParameters.HttpHeader.Count > 0)
                {
                    foreach (var item in httpRequestParameters.HttpHeader)
                    {
                        _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(item.Key, item.Value);
                    }
                    //string userAndPasswordToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(key));

                }

                // Invoking the HTTP POST type Request.
                HttpResponseMessage httpResponse = await _httpClient.PostAsync(httpRequestParameters.URL, httpRequestParameters.Content);
                return httpResponse;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Posts the HTTP PUT Request for input Target URL.
        /// </summary>
        /// <param name="httpRequestParameters">HTTPRequestParameters.</param>
        /// <returns>Returns HttpResponseMessage object.</returns>
        public async Task<HttpResponseMessage> HttpPutAsync(HTTPRequestParameters httpRequestParameters)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Clear();

                // Invoking the HTTP PUT type Request.
                HttpResponseMessage httpResponse = await _httpClient.PutAsync(httpRequestParameters.URL, httpRequestParameters.Content);
                return httpResponse;
            }
            catch (Exception)
            {

                throw;
            }
        }


        /// <summary>
        /// HTTPs the delete asynchronous.
        /// </summary>
        /// <param name="httpRequestParameters">HTTPRequestParameters.</param>
        /// <returns></returns>
        public Task<HttpResponseMessage> HttpDeleteAsync(HTTPRequestParameters httpRequestParameters)
        {
            throw new NotImplementedException();
        }
    }
}
