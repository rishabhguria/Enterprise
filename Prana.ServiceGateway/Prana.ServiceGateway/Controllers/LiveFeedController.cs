using Microsoft.AspNetCore.Mvc;
using Prana.KafkaWrapper;
using Prana.ServiceGateway.Constants;
using Prana.ServiceGateway.Contracts;
using Prana.ServiceGateway.ExceptionHandling;
using Prana.ServiceGateway.Models;
using Prana.ServiceGateway.Services;
using Prana.ServiceGateway.Utility.CustomAttributes;

namespace Prana.ServiceGateway.Controllers
{
    [ApiController]
    [Route(ControllerMethodConstants.CONST_CONTROLLER)]
    [ServiceHealthGate(ServiceNameConstants.CONST_LiveFeed_Name, ServiceNameConstants.CONST_LiveFeed_DisplayName)]
    public class LiveFeedController : BaseController
    {
        private readonly ILiveFeedService _liveFeedService;
        private readonly ILogger<LiveFeedController> _logger;

        public LiveFeedController(IValidationHelper validationHelper,
            ITokenService tokenService,
            ILiveFeedService liveFeedService,
            ILogger<LiveFeedController> logger) : base(validationHelper, tokenService)
        {
            _liveFeedService = liveFeedService;
            this._logger = logger;
        }

        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_SUBSCRIBE_LIVEFEED)]
        public async Task<IActionResult> SubscribeLiveFeed(string data)
        {
            if (UserDtoObj.CompanyUserId == 0)
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);

            try
            {
                _logger.LogInformation("Subscribing {data} to live feed...", data);
                await _liveFeedService.RequestSymbol(data, UserDtoObj.CompanyUserId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
            }
        }

        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_REQ_MULTIPLE_SYMBOLS_LIVE_FEED_SNAPSHOT_DATA)]
        public async Task<IActionResult> ReqMultipleSymbolsLiveFeedSnapshotData([FromBody] MultipleSymbolRequestDto request)
        {
            if (UserDtoObj.CompanyUserId == 0)
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);

            if (request?.RequestedSymbols == null || !request.RequestedSymbols.Any())
                return BadRequest("Symbol list cannot be empty.");

            if (string.IsNullOrWhiteSpace(request.RequestedInstance))
                return BadRequest("RequestedInstance is required.");

            try
            {
                _logger.LogInformation("Requesting multiple symbols livefeed Snapshot data: {@Symbols}, Instance: {Instance}", request.RequestedSymbols, request.RequestedInstance);

                await _liveFeedService.ReqMultipleSymbolsLiveFeedSnapshotData(request, UserDtoObj.CompanyUserId);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error requesting multiple symbols to live feed snapshot data.");
                return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
            }
        }


        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_UNSUBSCRIBE_LIVEFEED)]
        public async Task<IActionResult> UnSubscribeLiveFeed(string data)
        {
            if (UserDtoObj.CompanyUserId == 0)
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);

            _logger.LogInformation("UnSubscribing {data} to live feed", data);

            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, data, CorrelationId);
            using (LogKafkaId(requestResponseObj))
                await _liveFeedService.UnSubscribeLiveFeed(requestResponseObj);
            return Ok();

        }

        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_UPDATE_MARKET_DATA_TOKEN_REQUEST)]
        public async Task<IActionResult> UpdateMarketDataTokenRequest([FromBody] GenericDto data)
        {
            if (UserDtoObj.CompanyUserId == 0)
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);
            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, (data.Data), CorrelationId);

            using (LogKafkaId(requestResponseObj))
            {
                await _liveFeedService.UpdateMarketDataTokenRequest(requestResponseObj);
                return Ok();
            }

        }
    }
}