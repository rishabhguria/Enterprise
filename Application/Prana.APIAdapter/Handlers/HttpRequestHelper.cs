using Prana.APIAdapter.Interfaces;
using Prana.APIAdapter.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Prana.APIAdapter.HelperClasses
{
    internal class HttpRequestHelper
    {
        /// <summary>
        /// Get API Service Response
        /// </summary>
        /// <param name="httpRequestManager"></param>
        /// <param name="httpRequestParameters"></param>
        /// <returns></returns>
        internal async Task<HttpResponseMessage> GetAPIServiceResponse(IHttpRequestManager httpRequestManager, HTTPRequestParameters httpRequestParameters)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                switch (httpRequestParameters.HttpMethod.Method)
                {
                    case "POST":

                        httpResponse = await httpRequestManager.HttpPostAsync(httpRequestParameters);
                        break;

                    case "PUT":

                        httpResponse = await httpRequestManager.HttpPutAsync(httpRequestParameters);
                        break;

                    case "GET":
                        httpResponse = await httpRequestManager.HttpGetAsync(httpRequestParameters);
                        break;

                    case "DELETE":
                        httpResponse = await httpRequestManager.HttpDeleteAsync(httpRequestParameters);
                        break;
                }
            }
            catch (TaskCanceledException ex)
            {
                if (!ex.CancellationToken.IsCancellationRequested)
                    throw new Exception("Reqest got cancelled.", ex);
            }
            return httpResponse;
        }
    }
}
