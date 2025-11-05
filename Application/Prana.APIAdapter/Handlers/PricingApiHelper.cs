using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prana.APIAdapter.Interfaces;
using Prana.APIAdapter.Models;
using Prana.APIAdapter.Sessions;
using Prana.APIAdapter.Utilities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Prana.APIAdapter.HelperClasses
{
    public class PricingApiHelper : IPriceAPIService, IDisposable
    {
        /// <summary>
        /// httpRequestManager instance
        /// </summary>
        IHttpRequestManager _httpRequestManager;

        /// <summary>
        /// httpRequestManager instance
        /// </summary>
        HttpRequestHelper _httpRequestHelper;

        /// <summary>
        /// Pricing Api Helper
        /// </summary>
        public PricingApiHelper()
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                //TimeOut, TODO set config
                int timeOut = 30000;
                _httpRequestManager = _httpRequestManager ?? new HttpRequestManager(httpClient, timeOut);
                _httpRequestHelper = _httpRequestHelper ?? new HttpRequestHelper();
            }
            catch (Exception)
            {

                throw;
            }

        }

        ///<inheritdoc/>
        public async Task<AuthSession> Authenticate()
        {
            try
            {
                var httpParams = SessionManager.Instance.AuthHTTPRequestParameters;

                AuthSession auth = new AuthSession();

                //Get httpResponse from authentication API
                HttpResponseMessage httpResponse = await _httpRequestHelper.GetAPIServiceResponse(_httpRequestManager, httpParams);

                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    var responseString = await httpResponse.Content.ReadAsStringAsync();
                    var jAuth = JsonConvert.DeserializeObject<JObject>(responseString);

                    var config = SessionManager.Instance.AuthenticationColumnMappingConfig;

                    auth = config.CreateMapper().Map<AuthSession>(jAuth);

                    if (auth.isAuthenticated)
                    {
                        SessionManager.Instance.Session = auth;
                    }
                    else
                    {

                    }
                }

                return auth;
            }
            catch (Exception)
            {
                throw;
            }

        }

        ///<inheritdoc/>
        public async Task<string> GetPrice()
        {
            try
            {
                var httpParams = SessionManager.Instance.PriceDataHTTPRequestParameters;

                //if static value of authentication false then set authentication code
                if (httpParams.HttpHeader != null && httpParams.HttpHeader.Count == 1)
                {
                    Dictionary<string, string> header = new Dictionary<string, string>();
                    foreach (var item in httpParams.HttpHeader)
                    {
                        header.Add(item.Key, SessionManager.Instance.Session.key);
                    }
                    httpParams.HttpHeader = header;
                }

                //Get httpResponse from authentication API
                HttpResponseMessage httpResponse = await _httpRequestHelper.GetAPIServiceResponse(_httpRequestManager, httpParams);

                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    var responseString = await httpResponse.Content.ReadAsStringAsync();
                    // var prices = JsonConvert.DeserializeObject<InstrumentPrice>(responseString);
                    return responseString;
                }
                else
                {
                    //TODO; Print/log the response
                    return string.Empty;
                }

            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose all unmanaged resources
        /// </summary>
        /// <param name="isDisposing"></param>
        protected virtual void Dispose(bool isDisposing)
        {

            if (isDisposing)
            {
                if (_httpRequestManager != null)
                {
                    _httpRequestManager = null;
                }
                if (_httpRequestHelper != null)
                {
                    _httpRequestHelper = null;
                }
            }
        }

    }
}
