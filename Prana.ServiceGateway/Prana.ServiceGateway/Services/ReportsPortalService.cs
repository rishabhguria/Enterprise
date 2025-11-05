using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Prana.ServiceGateway.Constants;
using Prana.ServiceGateway.Contracts;
using Prana.ServiceGateway.ExceptionHandling;
using Prana.ServiceGateway.Models;
using System.Text;

namespace Prana.ServiceGateway.Services
{
    public class ReportsPortalService : IReportsPortalService
    {
        //private static readonly  HttpClientHandler handler =new HttpClientHandler() { UseCookies = false };
        private static readonly HttpClient client = new HttpClient();
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ReportsPortalService> _logger;
        private IConfiguration _configuration;

        public ReportsPortalService(IConfiguration configuration,
            IHttpContextAccessor IhttpContextAccessor,
            ILogger<ReportsPortalService> logger)
        {
            _configuration = configuration;
            _httpContextAccessor = IhttpContextAccessor;
            this._logger = logger;
        }
        public async Task<string> GetApi(string url)
        {
            try
            {
                //var fullUrl = _configuration[ControllerMethodConstants.CONST_TOUCH_BASE_URL] + url;
                var message = new HttpRequestMessage(HttpMethod.Get, url);
                message.Headers.Clear();
                message.Headers.Add(ServicesMethodConstants.CONST_COOKIE, getSession());
                var result = await client.SendAsync(message);

                _logger.LogDebug("Touch GET Api called, endpoint: ${url}, isSuccess reponse: {code}", url, result.IsSuccessStatusCode);

                var responseData = await result.Content.ReadAsStringAsync();

                _logger.LogTrace("Touch response received for endpoint: ${url}, reponse: {resp}", url, responseData);

                return responseData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Service");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        public async Task<object> PostApi(string url, object BodyJSON)
        {
            try
            {
                using StringContent jsonContent = new(System.Text.Json.JsonSerializer.Serialize(BodyJSON), Encoding.UTF8, "application/json");
                var message = new HttpRequestMessage(HttpMethod.Post, url);
                message.Content = jsonContent;
                message.Headers.Clear();
                message.Headers.Add(ServicesMethodConstants.CONST_COOKIE, getSession());
                var result = await client.SendAsync(message);

                _logger.LogDebug("Touch GET Api called, endpoint: ${url}, isSuccess reponse: {code}", url, result.IsSuccessStatusCode);

                var responseData = await result.Content.ReadAsStreamAsync();

                _logger.LogTrace("Touch response received for endpoint: ${url}, reponse: {resp}", url, responseData);

                return responseData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Service");
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        public Task<string> StoreLayout(SaveDefaultLayoutDto layout)
        {
            string layoutStr = layout.groupIds.ToString();
            string filePath = Path.Combine("App_Data/Persistence", layout.userName + "_" + layout.companyUserId + "_" + "ReportDefault.txt");

            // Check if the file exists and delete it
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            // Create directory if it doesn't exist
            string directory = System.IO.Path.GetDirectoryName(filePath);
            if (!System.IO.Directory.Exists(directory))
                System.IO.Directory.CreateDirectory(directory);

            // Write layoutStr to the file
            System.IO.File.WriteAllText(filePath, layoutStr);
            return Task.FromResult("Saved");
        }

        public string FetchDefaultLayout(SaveDefaultLayoutDto layout)
        {
            string filePath = Path.Combine("App_Data/Persistence", layout.userName + "_" + layout.companyUserId + "_" + "ReportDefault.txt");
            string layoutStr = "";
            if (System.IO.File.Exists(filePath))
            {
                layoutStr = System.IO.File.ReadAllText(filePath);
            }
            return layoutStr;
        }

        private string getSession()
        {
            return ServicesMethodConstants.CONST_GATEWAY_SESSION
                + _httpContextAccessor.HttpContext.Request.Headers[ServicesMethodConstants.CONST_CONKIE]
                + ServicesMethodConstants.CONST_GATEWAY_AUTH
                + _httpContextAccessor.HttpContext.Request.Headers[ServicesMethodConstants.CONST_CONKIE2]
                + ServicesMethodConstants.CONST_SEMICOLON;
        }
    }

    public class ResponseData
    {
        public bool isSuccess { get; set; }
        public string errorMessage { get; set; }
        public int statusCode { get; set; }
        public object data { get; set; }

        public ResponseData(string errorMessage, int statusCode)
        {
            this.isSuccess = false;
            this.errorMessage = errorMessage;
            this.statusCode = statusCode;
            this.data = null;
        }
        public ResponseData(object data)
        {
            this.isSuccess = true;
            this.errorMessage = string.Empty;
            this.statusCode = 200;
            this.data = data;
        }
    }

}
