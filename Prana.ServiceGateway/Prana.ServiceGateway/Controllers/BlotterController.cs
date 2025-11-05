using Microsoft.AspNetCore.Mvc;
using Prana.KafkaWrapper;
using Prana.ServiceGateway.Constants;
using Prana.ServiceGateway.Contracts;
using Prana.ServiceGateway.Services;
using Prana.ServiceGateway.ExceptionHandling;
using Newtonsoft.Json;
using Prana.ServiceGateway.Models;
using Prana.ServiceGateway.Models.RequestDto;
using Prana.ServiceGateway.Utility.CustomAttributes;

namespace Prana.ServiceGateway.Controllers
{
    [Produces(ControllerMethodConstants.CONST_APPLICATION_JSON)]
    [Route(ControllerMethodConstants.CONST_CONTROLLER)]
    [ServiceHealthGate(ServiceNameConstants.CONST_BlotterData_Name, ServiceNameConstants.CONST_BlotterData_DisplayName)]
    [ApiController]
    public class BlotterController : BaseController
    {
        private readonly IBlotterService _blotterService;

        /// <summary>
        /// Constructor for API Controller including injection of parameters
        /// </summary>
        /// <param name="validationHelper"></param>
        /// <param name="tokenService"></param>
        /// <param name="blotterService"></param>
        public BlotterController(IValidationHelper validationHelper, ITokenService tokenService, IBlotterService blotterService) : base(validationHelper, tokenService)
        {
            _blotterService = blotterService;
        }

        /// <summary>
        /// This endpoint is used to Get Blotter data for the User
        /// </summary>
        /// <param name="isComTransTradeRulesRequired"></param>
        /// <param name="blotterId"></param>
        /// <response code="400">
        /// Example Value | Schema :
        ///      "CompanyUserId cannot be Blank or 0"
        /// </response>
        ///  <response code="401">  
        /// Example Value | Schema :
        ///        "Status Code: 401; Unauthorized"
        /// </response>
        [HttpGet]
        [ActionName(ControllerMethodConstants.CONST_ACTION_GET_BLOTTER_DATA)]
        [Route(ControllerMethodConstants.CONST_METHOD_GET_BLOTTER_DATA)]
        public async Task<IActionResult> GetBlotterData(string blotterId, bool? isComTransTradeRulesRequired = false)
        {
            if (UserDtoObj.CompanyUserId == 0)
            {
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);
            }
            var data = new { blotterId = blotterId, isComTransTradeRulesRequired = isComTransTradeRulesRequired };
            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, JsonConvert.SerializeObject(data), CorrelationId);
            using (LogKafkaId(requestResponseObj))
            {
                try
                {
                    await _blotterService.GetBlotterData(requestResponseObj);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
                }
            }
        }
        /// <summary>
        /// This endpoint is used to Get PST data for the User
        /// </summary>
        /// <param name="allocationPrefID"></param>
        /// <param name="symbol"></param>
        /// <param name="OrderSideId"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName(ControllerMethodConstants.CONST_ACTION_GET_PST_DATA)]
        [Route(ControllerMethodConstants.CONST_METHOD_GET_PST_DATA)]
        public async Task<IActionResult> GetPstData(int allocationPrefID, string symbol, string OrderSideId)
        {
            if (UserDtoObj.CompanyUserId == 0)
            {
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);
            }
            var data = new { allocationPrefID = allocationPrefID, symbol = symbol, OrderSideId = OrderSideId };
            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, JsonConvert.SerializeObject(data), CorrelationId);
            using (LogKafkaId(requestResponseObj))
            {
                await _blotterService.GetPstData(requestResponseObj);
                return Ok();
            }
        }

        /// <summary>
        /// This endpoint is used to Cancel All Sub(s) for the User
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
        [HttpPut]
        [Route(ControllerMethodConstants.CONST_METHOD_CANCEL_ALL_SUBS)]
        public async Task<IActionResult> CancelAllSubOrders([FromBody] OrdersActionRequestDto requestDto)
        {
            if (UserDtoObj.CompanyUserId == 0)
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);

            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, requestDto.CommaSeparatedParentClOrderIds, CorrelationId);
            using (LogKafkaId(requestResponseObj))
            {
                try
                {
                    await _blotterService.CancelAllSubOrders(requestResponseObj);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
                }
            }
        }

        /// <summary>
        /// This endpoint is used to Save allocation details.
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
        [HttpPut]
        [Route(ControllerMethodConstants.CONST_METHOD_TRANSFER_USER)]
        public async Task<IActionResult> TransferUser([FromBody] TransferUserRequestDto requestDto)
        {
            if (UserDtoObj.CompanyUserId == 0)
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);

            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, JsonConvert.SerializeObject(requestDto), CorrelationId);
            using (LogKafkaId(requestResponseObj))
            {
                await _blotterService.TransferUser(requestResponseObj);
                return Ok();
            }
        }

        /// <summary>
        /// This endpoint is used to Rollover All Sub(s) for the User
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
        [HttpPut]
        [Route(ControllerMethodConstants.CONST_METHOD_ROLLOVER_ALL_SUBS)]
        public async Task<IActionResult> RolloverAllSubOrders([FromBody] OrdersActionRequestDto requestDto)
        {
            if (UserDtoObj.CompanyUserId == 0)
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);

            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, requestDto.CommaSeparatedParentClOrderIds, CorrelationId);
            using (LogKafkaId(requestResponseObj))
            {
                try
                {
                    await _blotterService.RolloverAllSubOrders(requestResponseObj);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
                }
            }
        }

        /// <summary>
        /// This endpoint is used to Remove order(s).
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
        [HttpDelete]
        [Route(ControllerMethodConstants.CONST_METHOD_REMOVE_ORDERS)]
        public async Task<IActionResult> RemoveOrders(string commaSaperateParentClOrderId)
        {
            if (UserDtoObj.CompanyUserId == 0)
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);

            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, commaSaperateParentClOrderId, CorrelationId);
            using (LogKafkaId(requestResponseObj))
            {
                try
                {
                    await _blotterService.RemoveOrders(requestResponseObj);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
                }
            }
        }

        /// <summary>
        /// This endpoint is used to freeze order(s) rows in Pending Compliance UI.
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
        [Route(ControllerMethodConstants.CONST_METHOD_FREEZE_ORDERS_PCUI)]
        public async Task<IActionResult> FreezeOrdersInPendingComplianceUI([FromBody] OrdersActionRequestDto requestDto)
        {

            if (UserDtoObj.CompanyUserId == 0)
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);

            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, requestDto.CommaSeparatedParentClOrderIds, CorrelationId);
            using (LogKafkaId(requestResponseObj))
            {
                try
                {
                    await _blotterService.FreezeOrdersInPendingComplianceUI(requestResponseObj);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
                }
            }

        }

        /// <summary>
        /// This endpoint is used to unfreeze order(s) rows in Pending Compliance UI.
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
        [Route(ControllerMethodConstants.CONST_METHOD_UNFREEZE_ORDERS_PCUI)]
        public async Task<IActionResult> UnfreezeOrdersInPendingComplianceUI([FromBody] OrdersActionRequestDto requestDto)
        {
            if (UserDtoObj.CompanyUserId == 0)
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);

            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, requestDto.CommaSeparatedParentClOrderIds, CorrelationId);
            using (LogKafkaId(requestResponseObj))
            {
                try
                {
                    await _blotterService.UnfreezeOrdersInPendingComplianceUI(requestResponseObj);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
                }
            }
        }

        /// <summary>
        /// This endpoint is used to Remove manual execution.
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
        [HttpDelete]
        [Route(ControllerMethodConstants.CONST_METHOD_REMOVE_MANUAL_EXECUTION)]
        public async Task<IActionResult> RemoveManualExecution(string parentClOrderId)
        {

            if (UserDtoObj.CompanyUserId == 0)
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);

            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, parentClOrderId, CorrelationId);

            using (LogKafkaId(requestResponseObj))
            {
                try
                {
                    await _blotterService.RemoveManualExecution(requestResponseObj);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
                }
            }
        }

        /// <summary>
        /// This endpoint is used to Get manual fills of a manual order.
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
        [Route(ControllerMethodConstants.CONST_METHOD_GET_BLOTTER_MANUAL_FILLS)]
        public async Task<IActionResult> GetBlotterManualFills(string parentClOrderId)
        {
            if (UserDtoObj.CompanyUserId == 0)
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);

            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, parentClOrderId, CorrelationId);
            using (LogKafkaId(requestResponseObj))
            {
                try
                {
                    await _blotterService.GetBlotterManualFills(requestResponseObj);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
                }
            }
        }

        /// <summary>
        /// This endpoint is used to Save add/modified manual fills.
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
        [HttpPut]
        [Route(ControllerMethodConstants.CONST_METHOD_SAVE_ADD_MODIFY_FILLS)]
        public async Task<IActionResult> SaveManualFills([FromBody] SaveManualFillsDto requestDto)
        {
            if (UserDtoObj.CompanyUserId == 0)
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);

            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, requestDto.FillsDataTable, CorrelationId);
            using (LogKafkaId(requestResponseObj))
            {
                try
                {
                    await _blotterService.SaveManualFills(requestResponseObj);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
                }
            }
        }

        /// <summary>
        /// This endpoint is used to Get allocation details.
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
        [Route(ControllerMethodConstants.CONST_METHOD_GET_ALLOCATION_DETAILS)]
        public async Task<IActionResult> GetAllocateDetails(string parentClOrderId)
        {
            if (UserDtoObj.CompanyUserId == 0)
            {
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);
            }
            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, parentClOrderId, CorrelationId);

            using (LogKafkaId(requestResponseObj))
            {
                try
                {
                    await _blotterService.GetAllocationDetails(requestResponseObj);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
                }
            }
        }

        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_GET_PST_ALLOCATION_DETAILS)]
        public async Task<IActionResult> GetPstAllocationDetails(RequestDto<string> requestDto)
        {
            if (UserDtoObj.CompanyUserId == 0)
            {
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);
            }
            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, requestDto.Data);
            requestResponseObj.CorrelationId = requestDto.CorrelationId;

            using (LogKafkaId(requestResponseObj))
            {
                await _blotterService.GetPstAllocationDetails(requestResponseObj);
                return Ok();
            }
        }

        /// <summary>
        /// This endpoint is used to Save allocation details.
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
        [HttpPut]
        [Route(ControllerMethodConstants.CONST_METHOD_SAVE_ALLOCATION_DETAILS)]
        public async Task<IActionResult> SaveAllocationDetails([FromBody] SaveAllocationDetailsDto requestDto)
        {
            if (UserDtoObj.CompanyUserId == 0)
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);

            // Serialize the requestDto object to JSON string to match the expected parameter type  
            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, requestDto.AllocationDetails, CorrelationId);
            using (LogKafkaId(requestResponseObj))
            {
                try
                {
                    await _blotterService.SaveAllocationDetails(requestResponseObj);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
                }
            }
        }

        /// <summary>
        /// This endpoint is used to Rename custom tab.
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
        [HttpPut]
        [Route(ControllerMethodConstants.CONST_METHOD_RENAME_BLOTTER_CUSTOM_TAB)]
        public async Task<IActionResult> RenameBlotterCustomTab([FromBody] BlotterTabRenameInfo blotterTabRenameInfo)
        {
            if (UserDtoObj.CompanyUserId == 0)
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);

            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, JsonConvert.SerializeObject(blotterTabRenameInfo), CorrelationId);


            using (LogKafkaId(requestResponseObj))
            {
                await _blotterService.RenameBlotterCustomTab(requestResponseObj);
                return Ok();
            }

        }

        /// <summary>
        /// This endpoint is used to Remove custom tab.
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
        [HttpDelete]
        [Route(ControllerMethodConstants.CONST_METHOD_REMOVE_BLOTTER_CUSTOM_TAB)]
        public async Task<IActionResult> RemoveBlotterCustomTab([FromBody] BlotterTabRemoveInfo customTabsDetails)
        {
            if (UserDtoObj.CompanyUserId == 0)
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);

            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, JsonConvert.SerializeObject(customTabsDetails), CorrelationId);

            using (LogKafkaId(requestResponseObj))
            {
                await _blotterService.RemoveBlotterCustomTab(requestResponseObj);
                return Ok();
            }
        }

        /// <summary>
        /// Handles HTTP GET request to fetch order details required for editing trade attributes
        /// based on the provided parent ClOrderID.
        /// </summary>
        /// <param name="parentClOrderId">The parent client order ID of the sub-order.</param>
        [HttpGet]
        [Route(ControllerMethodConstants.CONST_METHOD_GET_ORDER_DETAILS_FOR_EDIT_TRADE_ATTRIBUTES)]
        public async Task<IActionResult> FetchOrderDetailsForEditTradeAttributes([FromQuery] string parentClOrderId)
        {
            if (UserDtoObj.CompanyUserId == 0)
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);

            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, parentClOrderId, CorrelationId);

            using (LogKafkaId(requestResponseObj))
            {
                try
                {
                    await _blotterService.GetOrderDetailsForEditTradeAttributes(requestResponseObj);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
                }
            }
        }

        /// <summary>
        /// Handles the API request to save edited trade attributes.
        /// </summary>
        /// <param name="data">The trade order data containing updated trade attributes.</param>
        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_SAVE_EDITED_TRADE_ATTRIBUTES)]
        public async Task<IActionResult> SaveEditedTradeAttributes([FromBody] TradeOrderDto data)
        {
            if (UserDtoObj.CompanyUserId == 0)
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);

            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, data.Data, CorrelationId);

            using (LogKafkaId(requestResponseObj))
            {
                try
                {
                    await _blotterService.SaveEditedTradeAttributes(requestResponseObj);
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