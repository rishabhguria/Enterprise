using Prana.ServiceGateway.Contracts;
using Prana.ServiceGateway.Constants;
using Microsoft.Identity.Client;
using Prana.KafkaWrapper.Contracts;
using Microsoft.PowerBI.Api;
using Microsoft.Rest;
using Microsoft.PowerBI.Api.Models;
using Prana.ServiceGateway.Models.RequestDto;

namespace Prana.ServiceGateway.Services
{
    public class PowerBiEmbedReportService : IPowerBiEmbedReportService
    {
        private readonly IConfiguration _configuration;
        private readonly IKafkaManager _kafkaManager;
        private readonly IHubManager _hubManager;
        private readonly ILogger<PowerBiEmbedReportService> _logger;
        private const string API_URL = "https://api.powerbi.com/";
        private const string POWER_BI_SCOPE = "https://analysis.windows.net/powerbi/api/.default";

        public PowerBiEmbedReportService(IConfiguration configuration,
            IKafkaManager kafkaManager,
            IHubManager hubManager,
            ILogger<PowerBiEmbedReportService> logger)
        {
            _configuration = configuration;
            _kafkaManager = kafkaManager;
            _hubManager = hubManager;
            _logger = logger;
        }



        public async Task<PowerBIDto> GetPowerBIReportEmbedInfo(string workspaceId, string reportId)
        {
            try
            {
                // Get configuration values 
                var clientId =_configuration[UtilityConstants.CONST_CLIENT_CLIENT_ID];    
                var clientSecret = _configuration[UtilityConstants.CONST_CLIENT_CLIENT_SECRET];

                // Get access token
                string accessToken = await GetAccessToken(clientId, clientSecret);

                // Create Power BI Client
                var tokenCredentials = new TokenCredentials(accessToken, GlobalConstants.BEARER);
                var client = new PowerBIClient(new Uri(API_URL), tokenCredentials);

                var report = await client.Reports.GetReportInGroupAsync(Guid.Parse(workspaceId), Guid.Parse(reportId));

                // Generate embed token with user context
                var generateTokenRequestParameters = new GenerateTokenRequest(
                    accessLevel: "View"
                );

                var tokenResponse = await client.Reports.GenerateTokenInGroupAsync(
                    Guid.Parse(workspaceId),
                    Guid.Parse(reportId),
                    generateTokenRequestParameters);

                var expireTime = tokenResponse.Expiration.Subtract(DateTime.UtcNow).TotalMilliseconds.ToString();

                return new PowerBIDto
                {
                    ReportId = reportId,
                    EmbedUrl = report.EmbedUrl,
                    EmbedAccessToken = tokenResponse.Token,
                    TokenExpiration = expireTime
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getPowerBiReportEmbededInfo");
                return null;
            }
        }

        public async Task<string> GetAccessToken(string clientId, string clientSecret)
        {
            var issuers = _configuration[UtilityConstants.CONST_ISSUERS];
            // Configure the MSAL client
            IConfidentialClientApplication app = ConfidentialClientApplicationBuilder
                .Create(clientId)
                .WithClientSecret(clientSecret)
                .WithAuthority(new Uri(issuers))
                .Build();

            // Define the resource scope
            string[] scopes = new string[] { POWER_BI_SCOPE };

            try
            {
                // Acquire token using client credentials flow
                var result = await app.AcquireTokenForClient(scopes).ExecuteAsync();
                return result.AccessToken;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAccessToken of power BI");
                return string.Empty;
            }
        }
    }
}