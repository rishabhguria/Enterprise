using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Prana.KafkaWrapper;
using Prana.ServiceGateway.Constants;
using Prana.ServiceGateway.Contracts;
using Prana.ServiceGateway.ExceptionHandling;
using Prana.ServiceGateway.Hubs;
using Prana.ServiceGateway.Models;
using Prana.ServiceGateway.Models.RequestDto;
using Prana.ServiceGateway.Services;

namespace Prana.ServiceGateway.Controllers
{
    [Produces(ControllerMethodConstants.CONST_APPLICATION_JSON)]
    [Route(ControllerMethodConstants.CONST_CONTROLLER)]
    [ApiController]
    public class RtpnlController : BaseController
    {
        private readonly IRtpnlService _rtpnlService;
        private readonly ILogger<RtpnlController> _logger;

        /// <summary>
        /// Constructor for API Controller including injection of parameters
        /// </summary>
        /// <param name="validationHelper"></param>
        /// <param name="tokenService"></param>
        /// <param name="rtpnlService"></param>
        /// <param name="logger"></param>
        public RtpnlController(IValidationHelper validationHelper, ITokenService tokenService, IRtpnlService rtpnlService, ILogger<RtpnlController> logger) : base(validationHelper, tokenService)
        {
            _rtpnlService = rtpnlService;
            _logger = logger;
        }

        /// <summary>
        /// This endpoint is used to save and update Configuration details
        /// </summary>
        /// <response code="400">
        /// 
        /// Example Value | Schema :
        ///     
        ///      "CompanyUserId cannot be Blank or 0"
        ///     
        /// </response>
        ///  <response code="401">  
        /// 
        /// Example Value | Schema :
        ///     
        ///        "Status Code: 401; Unauthorized"
        ///        
        /// </response>
        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_SAVEUPDATE_CONFIG_DETAILS)]
        public async Task<IActionResult> SaveUpdateConfigDetails([FromBody] ConfigDetailsInfo[] configDetails)
        {
            if (UserDtoObj.CompanyUserId == 0)
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);

            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, JsonConvert.SerializeObject(configDetails), CorrelationId);
            using (LogKafkaId(requestResponseObj))
            {
                await _rtpnlService.SaveUpdateConfigDetails(requestResponseObj);
                return Ok();
            }
        }


        /// <summary>
        /// This endpoint is used to save and update Configuration details
        /// </summary>
        /// <response code="400">
        /// 
        /// Example Value | Schema :
        ///     
        ///      "CompanyUserId cannot be Blank or 0"
        ///     
        /// </response>
        ///  <response code="401">  
        /// 
        /// Example Value | Schema :
        ///     
        ///        "Status Code: 401; Unauthorized"
        ///        
        /// </response>
        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_SAVE_CONFIG_DATA_FOR_EXTRACT)]
        public async Task<IActionResult> SaveConfigDataForExtract([FromBody] ConfigDetailsInfo[] data)
        {
            if (UserDtoObj.CompanyUserId == 0)
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);

            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, JsonConvert.SerializeObject(data), CorrelationId);

            using (LogKafkaId(requestResponseObj))
            {
                await _rtpnlService.SaveConfigDataForExtract(requestResponseObj);
                return Ok();
            }
        }

        /// <summary>
        /// This endpoint is used to Get Rtpnl widget data for the User
        /// </summary>
        /// <response code="400">
        /// 
        /// Example Value | Schema :
        ///     
        ///      "CompanyUserId cannot be Blank or 0"
        ///     
        /// </response>
        ///  <response code="401">  
        /// 
        /// Example Value | Schema :
        ///     
        ///        "Status Code: 401; Unauthorized"
        ///        
        /// </response>
        [HttpGet]
        [Route(ControllerMethodConstants.CONST_METHOD_GET_RTPNL_WIDGET_DATA)]
        public async Task<IActionResult> GetRtpnlWidgetData()
        {
            if (UserDtoObj.CompanyUserId == 0)
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);
            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, null, CorrelationId);
            using (LogKafkaId(requestResponseObj))
            {
                try
                {
                    await _rtpnlService.GetRtpnlWidgetData(requestResponseObj);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
                }
            }
        }

        /// <summary>
        /// This endpoint is used to Get Rtpnl widget Config data for the User
        /// </summary>
        /// <response code="400">
        /// 
        /// Example Value | Schema :
        ///     
        ///      "CompanyUserId cannot be Blank or 0"
        ///     
        /// </response>
        ///  <response code="401">  
        /// 
        /// Example Value | Schema :
        ///     
        ///        "Status Code: 401; Unauthorized"
        ///        
        /// </response>
        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_GET_RTPNL_WIDGET_CONFIG_DATA)]
        public async Task<IActionResult> GetRtpnlWidgetConfigData([FromBody] RtpnlWidgetConfigRequestDto data)
        {
            if (UserDtoObj.CompanyUserId == 0)
            {
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);
            }
            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, JsonConvert.SerializeObject(data), CorrelationId);
            using (LogKafkaId(requestResponseObj))
            {
                await _rtpnlService.GetRtpnlWidgetConfigData(requestResponseObj);
                return Ok();
            }
        }

        /// <summary>
        /// This endpoint is used to check the calculation service is running
        /// </summary>
        /// <response code="400">
        /// 
        /// Example Value | Schema :
        ///     
        ///      "CompanyUserId cannot be Blank or 0"
        ///     
        /// </response>
        ///  <response code="401">  
        /// 
        /// Example Value | Schema :
        ///     
        ///        "Status Code: 401; Unauthorized"
        ///        
        /// </response>
        [HttpGet]
        [ActionName(ControllerMethodConstants.CONST_ACTION__CHECK_CALCULATION_SERVICE_RUNNING)]
        [Route(ControllerMethodConstants.CONST_ACTION__CHECK_CALCULATION_SERVICE_RUNNING)]
        public async Task<IActionResult> CheckCalculationServiceRunning()
        {
            if (UserDtoObj.CompanyUserId == 0)
            {
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);
            }
            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, String.Empty, CorrelationId);
            using (LogKafkaId(requestResponseObj))
            {
                await _rtpnlService.CheckCalculationServiceRunning(requestResponseObj);
                return Ok();
            }

        }

        /// <summary>
        /// This endpoint is used to delete removed widget details during save operation
        /// </summary>
        /// <response code="400">
        /// 
        /// Example Value | Schema :
        ///     
        ///      "CompanyUserId cannot be Blank or 0"
        ///     
        /// </response>
        ///  <response code="401">  
        /// 
        /// Example Value | Schema :
        ///     
        ///        "Status Code: 401; Unauthorized"
        ///        
        /// </response>
        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_DELETE_REMOVED_WIDGET_CONFIG_DETAILS)]
        public async Task<IActionResult> DeleteRemovedWidgetConfigDetails([FromBody] List<WidgetConfigDto> widgetConfigs)
        {
            if (UserDtoObj.CompanyUserId == 0)
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);

            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, JsonConvert.SerializeObject(widgetConfigs), CorrelationId);
            await _rtpnlService.DeleteRemovedWidgetConfigDetails(requestResponseObj);

            return Ok();

        }


        /// <summary>
        /// This endpoint is used to register the user for getting rtpnl updates
        /// </summary>
        /// <response code="400">
        /// 
        /// Example Value | Schema :
        ///     
        ///      "CompanyUserId cannot be Blank or 0"
        ///     
        /// </response>
        ///  <response code="401">  
        /// 
        /// Example Value | Schema :
        ///     
        ///        "Status Code: 401; Unauthorized"
        ///        
        /// </response>
        [HttpGet]
        [ActionName(ControllerMethodConstants.CONST_ACTION_REGISTERRTPNLUSER)]
        [Route(ControllerMethodConstants.CONST_ACTION_REGISTERRTPNLUSER)]
        public async Task<IActionResult> RegisterRTPNLUser()
        {
            try
            {
                _logger.LogInformation("Trying to register rtpnl user to the list : " + UserDtoObj.CompanyUserId);
                if (UserDtoObj.CompanyUserId == 0)
                {
                    return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);
                }

                //Here , we are trying to insert the user information in the dictionary
                HubClientConnectionManagerRTPNL.AddUserToRTPNLUpdatesList(UserDtoObj.CompanyUserId);
                HubClientConnectionManagerRTPNLUpdates.AddUserToRTPNLUpdatesList(UserDtoObj.CompanyUserId);
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while adding client details to the list in controller : " + ex);
            }
            return Ok();

        }

        /// <summary>
        /// This endpoint is used to de-register the user for getting rtpnl updates
        /// </summary>
        /// <response code="400">
        /// 
        /// Example Value | Schema :
        ///     
        ///      "CompanyUserId cannot be Blank or 0"
        ///     
        /// </response>
        ///  <response code="401">  
        /// 
        /// Example Value | Schema :
        ///     
        ///        "Status Code: 401; Unauthorized"
        ///        
        /// </response>
        [HttpGet]
        [ActionName(ControllerMethodConstants.CONST_ACTION_DEREGISTERRTPNLUSER)]
        [Route(ControllerMethodConstants.CONST_ACTION_DEREGISTERRTPNLUSER)]
        public async Task<IActionResult> DeRegisterRTPNLUser()
        {
            try
            {
                _logger.LogTrace("Trying to de register rtpnl user to the list :{0} ", UserDtoObj.CompanyUserId);
                if (UserDtoObj.CompanyUserId == 0)
                {
                    return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);
                }

                //Here , we are trying to remove the user information from the dictionary , after this the user will not get the rtpnl updates.
                HubClientConnectionManagerRTPNL.RemoveUserFromRTPNLUpdatesList(UserDtoObj.CompanyUserId);
                HubClientConnectionManagerRTPNLUpdates.RemoveUserFromRTPNLUpdatesList(UserDtoObj.CompanyUserId);
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while removing client details from the list in controller : " + ex);
            }
            return Ok();

        }

        /// <summary>
        /// This endpoint is used to get the trade attributes labels
        /// </summary>
        /// <response code="400">
        /// 
        /// Example Value | Schema :
        ///     
        ///      "CompanyUserId cannot be Blank or 0"
        ///     
        /// </response>
        ///  <response code="401">  
        /// 
        /// Example Value | Schema :
        ///     
        ///        "Status Code: 401; Unauthorized"
        ///        
        /// </response>
        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_RTPNL_TRADE_ATTRIBUTES_LABELS_REQUEST)]
        public async Task<IActionResult> GetTradeAttributesLabelsRtpnl(BaseRequestDto requestDto)
        {
            try
            {
                if (requestDto.CompanyUserID == 0)
                {
                    return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);
                }
                RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, null, requestDto.CorrelationId);
                await _rtpnlService.GetRtpnlTradeAttributeLabels(requestResponseObj);
                return Ok(new { IsSuccess = true, CorrelationId = requestDto.CorrelationId });
            }
            catch (Exception ex)
            {
                Logger.LogMessage(ex, LogEnums.LogPolicy.LogAndShow);
                return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
            }
        }
    }
}
