using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Prana.KafkaWrapper;
using Prana.ServiceGateway.Constants;
using Prana.ServiceGateway.Contracts;
using Prana.ServiceGateway.Services;
using Prana.ServiceGateway.ExceptionHandling;
using Prana.ServiceGateway.Models;
using Prana.ServiceGateway.Utility.CustomAttributes;

namespace Prana.ServiceGateway.Controllers
{
    [Produces(ControllerMethodConstants.CONST_APPLICATION_JSON)]
    [Route(ControllerMethodConstants.CONST_CONTROLLER)]
    [ServiceHealthGate(ServiceNameConstants.CONST_Layout_Name, ServiceNameConstants.CONST_Layout_DisplayName)]
    [ApiController]
    public class LayoutController : BaseController
    {
        private readonly ILayoutService _layoutService;
        private readonly ILogger<LayoutController> _logger;

        public LayoutController(IValidationHelper validationHelper,
            ITokenService tokenService,
            ILayoutService layoutService,
            ILogger<LayoutController> logger) : base(validationHelper, tokenService)
        {
            _layoutService = layoutService;
            this._logger = logger;
        }

        /// <summary>
        /// This endpoint is used to save the layout of grid
        /// </summary>
        /// <response code="200">
        /// 
        /// Example Value | Schema :
        ///     
        ///     "File Saved"
        ///     
        /// </response>
        /// <response code="400">
        /// 
        /// Example Value | Schema :
        ///     
        ///      "Either SavedLayoutText or GridName is Blank"
        ///     
        /// </response>
        ///  <response code="401">  
        /// 
        /// Example Value | Schema :
        ///     
        ///     "Status Code: 401; Unauthorized"
        ///        
        /// </response>
        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_SAVE_LAYOUT)]
        public async Task<IActionResult> SaveLayout([FromBody] ViewInfo viewInfo)
        {
            if (UserDtoObj.CompanyUserId == 0)
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);
            string viewInfoJson = JsonConvert.SerializeObject(viewInfo);
            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, viewInfoJson, CorrelationId);
            using (LogKafkaId(requestResponseObj))
            {
                await _layoutService.SaveLayout(requestResponseObj);
                return Ok();
            }

        }

        /// <summary>
        /// This endpoint is used to load the All grids layout
        /// </summary>
        /// <response code="200">
        /// 
        /// Example Value | Schema :
        ///     
        ///     "{Layout JSON file}"
        ///     
        ///     "File does not exist"
        ///     
        /// </response>
        /// <response code="400">
        /// 
        /// Example Value | Schema :
        ///     
        ///      "GridName cannot be Blank"
        ///      
        /// </response>
        ///  <response code="401">  
        /// 
        /// Example Value | Schema :
        ///     
        ///     "Status Code: 401; Unauthorized"
        ///        
        /// </response>
        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_LOAD_BLOTTER_ALL_GRIDS_LAYOUT)]
        public async Task<IActionResult> LoadBlotterAllGridsLayoutAsync([FromBody] GenericDto viewInfo)
        {
            if (UserDtoObj.CompanyUserId == 0)
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);

            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, viewInfo.Data, CorrelationId);
            using (LogKafkaId(requestResponseObj))
            {
                await _layoutService.LoadLayout(requestResponseObj);
                return Ok();
            }

        }

        /// <summary>
        /// This endpoint is used to save the RTPNL views.
        /// </summary>
        /// <response code="200">
        /// 
        /// Example Value | Schema :
        ///     "File Saved"
        /// </response>
        ///  <response code="401">  
        /// Example Value | Schema :
        ///     "Status Code: 401; Unauthorized"
        /// </response>
        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_SAVE_OR_UPDATE_RTPNL_LAYOUT)]
        public async Task<IActionResult> SaveOrUpdateRtpnlLayout([FromBody] GenericDto data)
        {
            if (string.IsNullOrEmpty(data.Data))
                return BadRequest(MessageConstants.MSG_CONST_LAYOUTTITLE_OR_LAYOUT_CANNOT_BLANK);

            if (UserDtoObj.CompanyUserId == 0)
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);

            RtpnlLayoutInfo layoutDetails = JsonConvert.DeserializeObject<RtpnlLayoutInfo>(data.Data);
            WidgetConfigAndOldWidgetIds widgetConfigAndOldWidgetIds = JsonConvert.DeserializeObject<WidgetConfigAndOldWidgetIds>(data.Data);

            if (layoutDetails.internalPageInfo.Any(x => string.IsNullOrEmpty(x.viewId) || string.IsNullOrEmpty(x.viewName)))
                return BadRequest(MessageConstants.MSG_CONST_LAYOUTTITLE_OR_LAYOUT_CANNOT_BLANK);

            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId,
                data.Data, CorrelationId);
            using (LogKafkaId(requestResponseObj))
            {
                try
                {
                    await _layoutService.SaveOrUpdateRtpnlViews(requestResponseObj);
                    GenerateConsoleAndLog(MessageConstants.CONST_LAYOUT_CONTROLLER, ControllerMethodConstants.CONST_METHOD_SAVE_OR_UPDATE_RTPNL_LAYOUT, MessageConstants.CONST_METHOD_EXECUTION_ENDED);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
                }
            }
        }

        /// <summary>
        /// This endpoint is used to load the RTPNL views.
        /// </summary>
        /// <response code="200">
        /// 
        /// Example Value | Schema :
        ///     
        ///     "{Layout JSON file}"
        ///     
        ///     "File does not exist"
        ///     
        /// </response>
        ///  <response code="401">  
        /// 
        /// Example Value | Schema :
        ///     
        ///     "Status Code: 401; Unauthorized"
        ///        
        /// </response>
        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_LOAD_RTPNL_LAYOUT)]
        public async Task<IActionResult> LoadRtpnlLayout([FromBody] LoadRtpnlLayout loadRtpnlLayout)
        {
            if (UserDtoObj.CompanyUserId == 0)
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);

            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, JsonConvert.SerializeObject(loadRtpnlLayout), CorrelationId);
            using (LogKafkaId(requestResponseObj))
            {
                try
                {
                    await _layoutService.LoadRtpnlViews(requestResponseObj);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
                }
            }
        }

        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_DELETE_OPENFIN_PAGES)]
        public async Task<IActionResult> DeletePageInformationForUser([FromBody] DeletePageDTO openfinPageInformation)
        {
            if (UserDtoObj.CompanyUserId == 0)
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);

            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, JsonConvert.SerializeObject(openfinPageInformation), CorrelationId);
            using (LogKafkaId(requestResponseObj))
            {
                await _layoutService.DeleteOpenfinPages(requestResponseObj);
                return Ok();
            }

        }


        [HttpGet]
        [Route(ControllerMethodConstants.REMOVE_PAGES_FOR_AN_USER)]
        public async Task<IActionResult> RemovePagesForAnUser(string moduleName = "")
        {
            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, moduleName, CorrelationId);
            using (LogKafkaId(requestResponseObj))
            {
                try
                {
                    await _layoutService.RemovePagesForAnUser(requestResponseObj);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
                }
            }
        }

    }
}
