using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prana.KafkaWrapper;
using Prana.ServiceGateway.Constants;
using Prana.ServiceGateway.Contracts;
using Prana.ServiceGateway.ExceptionHandling;
using Prana.ServiceGateway.Models;
using Prana.ServiceGateway.Models.RequestDto;
using Prana.ServiceGateway.Services;
using Prana.ServiceGateway.Utility;
using Prana.ServiceGateway.Utility.CustomAttributes;

namespace Prana.ServiceGateway.Controllers
{
    [ApiController]
    [Route(ControllerMethodConstants.CONST_CONTROLLER)]
    [ServiceHealthGate(ServiceNameConstants.CONST_Trading_Name, ServiceNameConstants.CONST_Trading_DisplayName)]
    public class TradingController : BaseController
    {
        private readonly ILiveFeedService _liveFeedService;
        private readonly ITradingService _tradingservice;
        private readonly ILogger<TradingController> logger;
        private readonly ISecurityValidationService _securityValidationService;


        public TradingController(IValidationHelper validationHelper,
            ITokenService tokenService,
            ILiveFeedService liveFeedService,
            ISecurityValidationService securityValidationService,
            ITradingService tradingService,
            ILogger<TradingController> logger) : base(validationHelper, tokenService)
        {
            _liveFeedService = liveFeedService;
            _securityValidationService = securityValidationService;
            _tradingservice = tradingService;
            this.logger = logger;
        }

        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_SM_SAVE_NEW_SYMBOL)]
        public IActionResult SMSaveNewSymbol(SmSymbolDto requestDto)
        {//{"AUECID":1,"AssetID":1,"ExchangeID":1,"UnderlyingID":1,"CurrencyID":1,"TickerSymbol":"AAPL3","Multiplier":"5","Description":"AAPL3","UnderlyingSymbol":"AAPL3","BloombergSymbol":"AAPL3","FactsetSymbol":"AAPL3","ActivSymbol":"AAPL3","PutAndCall":"","StrikePrice":0,"RoundLot":1,"ExpirationDate":"1800-01-01T18:07:00.000Z","SecurityAction":"0","Symbol_PK":"","SedolSymbol":"AAPL3"}
            if (UserDtoObj.CompanyUserId == 0)
            {
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);
            }
            var data = JsonConvert.SerializeObject(requestDto);
            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, data, CorrelationId);
            using (LogKafkaId(requestResponseObj))
            {
                _securityValidationService.SMSaveNewSymbol(requestResponseObj);
                return Ok();
            }
        }

        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_CREATE_POPUP_TEXT)]
        public async Task<IActionResult> CreatePopUpText([FromBody] TradeOrderDto data)
        {

            if (UserDtoObj.CompanyUserId == 0)
            {
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);
            }
            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, data.Data, CorrelationId);
            using (LogKafkaId(requestResponseObj))
            {
                await _tradingservice.CreatePopUpText(requestResponseObj);
                return Ok();
            }
        }
        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_CREATE_OPTION_SYMBOL)]
        public async Task<IActionResult> CreateOptionSymbol(CreateOptionSymbolRequestDto requestDto)
        {
            if (UserDtoObj.CompanyUserId == 0)
            {
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);
            }
            var data = JsonConvert.SerializeObject(requestDto);
            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, data, CorrelationId);
            using (LogKafkaId(requestResponseObj))
            {
                await _tradingservice.CreateOptionSymbol(requestResponseObj);
                return Ok();
            }
        }

        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_UNSUBSCRIBE_SYBMOL_COMPRESSION_FEED)]
        public async Task<IActionResult> UnSubscribeSymbolCompressionFeed(UnSubscribeSymbolComprFeedRequestDto requestDto)
        {
            //{"RequestedInstance":"17_internal-generated-view-fb5619af-76ed-47ad-8c41-1f20e40a29fc"}
            //{"RequestedInstance":"17_internal-generated-view-f6e10f94-3a13-412a-b8d3-5845ac774b30"}
            if (UserDtoObj.CompanyUserId == 0)
            {
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);
            }
            var data = JsonConvert.SerializeObject(requestDto);
            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, data, CorrelationId);
            using (LogKafkaId(requestResponseObj))
            {
                await _tradingservice.UnsubscribeSymbolCompressionFeed(requestResponseObj);
                return Ok();
            }
        }

        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_SM_SYMBOL_SEARCH)]
        public async Task<IActionResult> SMSymbolSearch([FromBody] GenericDto data)
        {
            if (UserDtoObj.CompanyUserId == 0)
            {
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);
            }

            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, data.Data, CorrelationId);
            using (LogKafkaId(requestResponseObj))
            {
                await _securityValidationService.SMSymbolSearch(requestResponseObj);
                return Ok();
            }
        }

        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_GET_SYMBOL_WISE_SHORT_LOCATE_ORDERS)]
        public async Task<IActionResult> GetSymbolWiseShortLocateOrders([FromBody] GenericDto data)
        {
            if (UserDtoObj.CompanyUserId == 0)
            {
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);
            }
            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, data.Data, CorrelationId);
            using (LogKafkaId(requestResponseObj))
            {
                await _tradingservice.GetSymbolWiseShortLocateOrders(requestResponseObj);
                return Ok();
            }
        }

        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_DETERMINE_SECURITY_BORROW_TYPE)]
        public async Task<IActionResult> DetermineSecurityBorrowType([FromBody] GenericDto data)
        {
            if (UserDtoObj.CompanyUserId == 0)
            {
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);
            }
            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, data.Data, CorrelationId);
            using (LogKafkaId(requestResponseObj))
            {
                await _tradingservice.DetermineSecurityBorrowType(requestResponseObj);
                return Ok();
            }
        }

        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_SYMBOL_SEARCH)]
        public async Task<IActionResult> SymbolSearch(SymbolSearchRequestDto requestDto)
        {
            try
            {
                var data = JsonConvert.SerializeObject(requestDto);
                var requestResponseObj = new RequestResponseModel(UserDtoObj.CompanyUserId, data, CorrelationId);
                await _securityValidationService.SymbolSearch(requestResponseObj);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
            }
        }

        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_GET_CUSTOM_ALLOCATION_DETAILS)]
        public async Task<IActionResult> GetCustomAllocationDetails([FromBody] GenericDto data)
        {

            if (UserDtoObj.CompanyUserId == 0)
            {
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);
            }
            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, data.Data, CorrelationId);
            using (LogKafkaId(requestResponseObj))
            {
                try
                {
                    await _tradingservice.GetCustomAllocationDetails(requestResponseObj);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
                }
            }
        }

        [HttpGet]
        [Route(ControllerMethodConstants.CONST_METHOD_GET_SAVED_CUSTOM_ALLOCATION_DETAILS)]
        public async Task<IActionResult> GetSavedCustomAllocationDetails(string preferenceId, string tradingTicketId)
        {

            if (UserDtoObj.CompanyUserId == 0)
            {
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);
            }
            var data = new { preferenceId = preferenceId, tradingTicketId = tradingTicketId };
            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, JsonConvert.SerializeObject(data), CorrelationId);
            using (LogKafkaId(requestResponseObj))
            {
                try
                {
                    await _tradingservice.GetSavedCustomAllocationDetails(requestResponseObj);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
                }
            }
        }

        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_DETERMINE_MULTIPLE_SECURITY_BORROW_TYPE)]
        public async Task<IActionResult> DetermineMultipleSecurityBorrowType([FromBody] GenericDto data)
        {
            if (UserDtoObj.CompanyUserId == 0)
            {
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);
            }
            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, data.Data, CorrelationId);
            using (LogKafkaId(requestResponseObj))
            {
                await _tradingservice.DetermineMultipleSecurityBorrowType(requestResponseObj);
                return Ok();
            }
        }

        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_GET_SAVED_CUSTOM_ALLOCATION_DETAILS_BULK)]
        public async Task<IActionResult> GetSavedCustomAllocationDetailsBulk([FromBody] AllocationBulkRequest request)
        {
            if (UserDtoObj.CompanyUserId == 0)
            {
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);
            }
            var data = new
            {
                preferenceIds = request.PreferenceIds,
                tradingTicketId = request.TradingTicketId
            };
            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, JsonConvert.SerializeObject(data), CorrelationId);
            using (LogKafkaId(requestResponseObj))
            {
                try
                {
                    await _tradingservice.GetSavedCustomAllocationDetailsBulk(requestResponseObj);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
                }
            }
        }


        [HttpGet]
        [Route(ControllerMethodConstants.CONST_METHOD_GET_SM_DATA)]
        public async Task<IActionResult> GetSMData(string viewId)
        {

            if (UserDtoObj.CompanyUserId == 0)
            {
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);
            }
            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, viewId, CorrelationId);
            using (LogKafkaId(requestResponseObj))
            {
                await _tradingservice.GetSMData(requestResponseObj);
                return Ok();
            }
        }

        [HttpGet]
        [Route(ControllerMethodConstants.CONST_METHOD_BROKER_CONNECTION_AND_VENUES)]
        public async Task<IActionResult> GetBrokerConnectionAndVenuesData(string tradingTicketId)
        {
            if (UserDtoObj.CompanyUserId == 0)
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);

            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, tradingTicketId, CorrelationId);
            using (LogKafkaId(requestResponseObj))
            {
                await _tradingservice.GetBrokerConnectionAndVenuesData(requestResponseObj);
                return Ok();
            }
        }

        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_BOOK_AS_SWAP_REPLACE)]
        public async Task<IActionResult> BookAsSwapReplace([FromBody] TradeOrderDto data)
        {
            if (UserDtoObj.CompanyUserId == 0)
            {
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);
            }

            if (!string.IsNullOrEmpty(data?.Data))
            {
                RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, data.Data, CorrelationId);
                using (LogKafkaId(requestResponseObj))
                {
                    try
                    {
                        await _tradingservice.BookAsSwapReplace(requestResponseObj);
                        return Ok();
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
                    }
                }
            }
            return BadRequest();

        }

        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_SEND_REPLACE_ORDER)]
        public async Task<IActionResult> SendReplaceOrder([FromBody] TradeOrderDto data)
        {
            if (UserDtoObj.CompanyUserId == 0)
            {
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);
            }
            if (data.Data != null)
            {
                logger.LogInformation("Sending ReplaceOrder to kafka...");
                RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, data.Data, CorrelationId);
                using (LogKafkaId(requestResponseObj))
                {
                    try
                    {
                        await _tradingservice.SendReplaceOrderFromTT(requestResponseObj);
                        return Ok();
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
                    }
                }
            }

            return BadRequest();
        }

        /// <summary>
        /// This endpoint is used to send Live Order
        /// </summary>
        /// <remarks>
        /// Example Value | Schema :
        /// 
        ///     {"OrderId":"string",
        ///     "TransactionTime":DateTime,
        ///     "Symbol":"string",
        ///     "Side":"string",
        ///     "Status":"string",
        ///     "Quantity":Number,
        ///     "Broker":"string",
        ///     "Venue":Number,
        ///     "Algo":"string",
        ///     "TIF":"string",
        ///     "WorkingQuantity":Number,
        ///     "ExecutedQuantity":Number,
        ///     "AvgPrice":Number,
        ///     "Stop":Number,
        ///     "Level1Id":Number,
        ///     "Limit":Number,
        ///     "OrderType":"string",
        ///     "ExchangeID": Number,
        ///     "UnderlyingID": Number}
        /// </remarks>

        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_SEND_LIVE_ORDER)]
        public async Task<IActionResult> SendLiveOrderFromTT([FromBody] TradeOrderDto data)
        {

            if (UserDtoObj.CompanyUserId == 0)
            {
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);
            }
            if (!string.IsNullOrEmpty(data.Data))
            {
                RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, data.Data, CorrelationId);
                using (LogKafkaId(requestResponseObj))
                {
                    try
                    {
                        await _tradingservice.SendLiveOrderFromTT(requestResponseObj);
                        return Ok();
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
                    }
                }
            }

            return BadRequest();
        }

        /// <summary>
        /// This endpoint is used to get Algo Strategies from broker
        /// </summary>
        /// <remarks>
        ///  Example Value | Schema : 
        ///     {
        ///     "counterPartyID": integer,
        ///     "auecId": integer,
        ///     }
        /// </remarks>
        /// <response code="200">
        /// 
        /// Example Value | Schema :
        ///     
        ///     {
        ///         "ChangeStrategyOnCxlRpl":null,
        ///         "Description":null,
        ///         "DraftFlagIdentifierTag":null,
        ///         "ImageLocation":"UBS.PNG",
        ///         "StrategyIdentifierTag":{"_value":6061},
        ///         "VersionIdentifierTag":null,
        ///         "Tag957Support":null,
        ///         "Count":1,
        ///         "Strategies":[{"Description":null,
        ///                 "DisclosureDoc":null,
        ///                 "Edits":[],
        ///                 "FixMsgType":null,
        ///                 "ImageLocation":null,
        ///                 "Markets":[],
        ///                 "Name":"680",
        ///                 "OrderSequenceTag":null,
        ///                 "Parameters":[{"Value":{"LocalMktTz":null,"MaxValue":null,"MinValue":null,"ConstValue":null},"DefinedByFix":null,"EnumPairs":[],"HasEnumPairs":false,"FixTag":{"_value":6062},"MutableOnCxlRpl":true,"Name":"StartTime","RevertOnCxlRpl":null,"Type":"UTCTimestamp_t","Use":1},
        ///                 "ProviderId":"23",
        ///                 "ProviderSubId":null,
        ///                 "Regions":[],
        ///                 "RepeatingGroup":null,
        ///                 "SecurityTypes":[],
        ///                 "SentOrderLink":null,
        ///                 "StrategyEdits":[],
        ///                 "StrategyLayout":{
        ///                 "StrategyPanel":
        ///                 {
        ///                     "Border":null,
        ///                     "Collapsed":true,
        ///                     "Collapsible":false,
        ///                     "Color":null,"Orientation":1,
        ///                     "Title":null,
        ///                     "StrategyPanels":[],
        ///                     "Controls":[{ "Type":"Clock_t","LocalMktTz":null,"InitValueMode":null,"InitValue":null,"DisableForTemplate":null,"Id":"StartTimeClock","InitFixField":null,"InitPolicy":null,"Label":"Start Time","ParameterRef":"StartTime","ToolTip":null,"IsToggleable":false,"StateRules":[] }]
        ///                 }
        ///                 "TotalLegs":null,
        ///                 "TotalOrders":null,
        ///                 "UiRep":"VWAP",
        ///                 "Version":"1",
        ///                 "WireValue":"VWAP"}],
        ///         "Edits":[]}
        ///     }
        ///     
        /// </response>
        /// <response code="400">
        /// 
        /// Example Value | Schema :
        ///     
        ///      "CompanyUserId cannot be Blank or 0"
        ///      
        /// </response>
        [HttpGet]
        [Route(ControllerMethodConstants.CONST_METHOD_GET_ALGO_STRATEGIES_FROM_BROKER)]
        public async Task<IActionResult> GetAlgoStrategiesFromBroker(string Data)
        {
            if (UserDtoObj.CompanyUserId == 0)
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);

            if (!string.IsNullOrEmpty(Data))
            {
                RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, Data, CorrelationId);
                using (LogKafkaId(requestResponseObj))
                {
                    try
                    {
                        await _tradingservice.GetAlgoStrategiesFromBroker(requestResponseObj);
                        var resp = new RequestResponseModel(requestResponseObj.CompanyUserID, null, requestResponseObj.CorrelationId);
                        return Ok(resp);    //pass correlationId to UI
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
                    }
                }
            }
            return BadRequest();
        }


        /// <summary>
        /// This endpoint is used to send Manual Order
        /// </summary>
        /// <remarks>
        /// Example Value | Schema :
        /// 
        ///     {"OrderId":"string",
        ///     "TransactionTime":DateTime,
        ///     "Symbol":"string",
        ///     "Side":"string",
        ///     "Status":"string",
        ///     "Quantity":Number,
        ///     "Broker":"string",
        ///     "Venue":Number,
        ///     "Algo":"string",
        ///     "TIF":"string",
        ///     "WorkingQuantity":Number,
        ///     "ExecutedQuantity":Number,
        ///     "AvgPrice":Number,
        ///     "Stop":Number,
        ///     "Level1Id":Number,
        ///     "Limit":Number,
        ///     "OrderType":"string",
        ///     "ExchangeID": Number,
        ///     "UnderlyingID": Number}
        /// </remarks>
        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_SEND_MANUAL_ORDER)]
        public async Task<IActionResult> SendManualOrderFromTT([FromBody] TradeOrderDto data)
        {
            if (UserDtoObj.CompanyUserId == 0)
            {
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);
            }
            if (!string.IsNullOrEmpty(data.Data))
            {
                RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, data.Data, CorrelationId);
                using (LogKafkaId(requestResponseObj))
                {
                    try
                    {
                        await _tradingservice.SendManualOrderFromTT(requestResponseObj);
                        return Ok();
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
                    }
                }
            }

            Console.WriteLine(MessageConstants.CONST_BRACKET_OPEN + DateTime.Now.ToString(MessageConstants.CONST_DATET_TIME_FORMAT) + MessageConstants.CONST_BRACKET_CLOSE + MessageConstants.CONST_BAD_REQUEST_FORMAT + ControllerMethodConstants.CONST_METHOD_SEND_MANUAL_ORDER);
            return BadRequest();
        }

        /// <summary>
        /// This endpoint is used to send Stage Order
        /// </summary>
        /// <remarks>
        /// Example Value | Schema :
        /// 
        ///     {"OrderId":"string",
        ///     "TransactionTime":DateTime,
        ///     "Symbol":"string",
        ///     "Side":"string",
        ///     "Status":"string",
        ///     "Quantity":Number,
        ///     "Broker":"string",
        ///     "Venue":Number,
        ///     "Algo":"string",
        ///     "TIF":"string",
        ///     "WorkingQuantity":Number,
        ///     "ExecutedQuantity":Number,
        ///     "AvgPrice":Number,
        ///     "Stop":Number,
        ///     "Level1Id":Number,
        ///     "Limit":Number,
        ///     "OrderType":"string",
        ///     "ExchangeID": Number,
        ///     "UnderlyingID": Number}
        /// </remarks>
        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_SEND_STAGE_ORDER)]
        public async Task<IActionResult> SendStageOrderFromTT([FromBody] TradeOrderDto data)
        {
            if (UserDtoObj.CompanyUserId == 0)
            {
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);
            }

            if (data.Data != null)
            {
                TradingTicketInfo info = JsonConvert.DeserializeObject<TradingTicketInfo>(data.Data);
                logger.LogInformation($"Sending Stage order to kafka with Symbol: {info.StageTradeHandler[0]["Symbol"]}, Broker: {info.StageTradeHandler[0]["Broker"]}");

                RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, data.Data, CorrelationId);
                using (LogKafkaId(requestResponseObj))
                {
                    try
                    {
                        await _tradingservice.SendStageOrderFromTT(requestResponseObj);
                        return Ok();
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
                    }
                }
            }
            return BadRequest();
        }

        /// <summary>
        /// This endpoint is used to get Symbol-Account wise Position.
        /// </summary>
        /// <response code="200">
        /// 
        /// Example Value | Schema :
        ///     
        /// "{\"InstanceNames\":[\"Quick TT(1)\",\"Quick TT(2)\",\"Quick TT(3)\",\"Quick TT(4)\",\"Quick TT(5)\",\"Quick TT(6)\",\"Quick TT(7)\",\"Quick TT(8)\",\"Quick TT(9)\",\"Quick TT(10)\"],\"InstanceForeColors\":[\"155, 187, 89\",\"155, 187, 89\",\"155, 187, 89\",\"155, 187, 89\",\"155, 187, 89\",\"155, 187, 89\",\"155, 187, 89\",\"155, 187, 89\",\"155, 187, 89\",\"155, 187, 89\"],\"InstanceBackColors\":[\"33, 44, 57\",\"33, 44, 57\",\"33, 44, 57\",\"33, 44, 57\",\"33, 44, 57\",\"33, 44, 57\",\"33, 44, 57\",\"33, 44, 57\",\"33, 44, 57\",\"33, 44, 57\"],\"HotButtonQuantities\":[100,1000,10000],\"UseAccountForLinking\":false,\"UseVenueForLinking\":false}"/// 
        /// </response>
        /// <response code="400">
        /// 
        /// Example Value | Schema :
        ///     
        ///      "CompanyUserId cannot be Blank or 0"
        ///      
        /// </response>
        /// <response code="401">
        /// 
        /// Example Value | Schema :
        ///     
        ///     "Unauthorized"
        ///     
        /// </response>
        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_GET_SYMBOL_ACCOUNT_POSITION)]
        public async Task<IActionResult> GET_SymbolAccountWisePosition(SymbolAccountPositionRequestDto requestDto)
        {
            //{"Symbol":"AAPL","CurrencyID":1,"RequestID":"17_internal-generated-view-b538e294-dd0f-4728-92aa-ccd72bb17ab0"}
            //{"Symbol":"AAPL","CurrencyID":1,"RequestID":"17_internal-generated-view-48769661-d529-4b6c-a7bf-aae7e413a8fb"}
            if (UserDtoObj.CompanyUserId == 0)
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);
            var data = JsonConvert.SerializeObject(requestDto);
            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, data, CorrelationId);
            using (LogKafkaId(requestResponseObj))
            {
                try
                {
                    await _tradingservice.GetSymbolAccountWisePosition(requestResponseObj);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
                }
            }
        }

        /// <summary>
        /// This endpoint is used to get company user hot key preferences
        /// </summary>
        /// <remarks>
        /// Example Value | Schema :
        /// 
        ///     {"CompanyUserHotKeyPreferenceID":Number,
        ///     "CompanyUserID":Number,
        ///     "HotKeyPreferenceElements":"string",
        ///     "EnableBookMarkIcon":bool}
        /// </remarks>
        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_GET_COMPANY_USER_HOT_KEY_PREFERENCES)]
        public async Task<IActionResult> GET_CompanyUserHotKeyPreferences(BaseRequestDto requestDto)
        {

            if (UserDtoObj.CompanyUserId == 0)
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);

            requestDto.CompanyUserID = UserDtoObj.CompanyUserId;
            string data = JsonConvert.SerializeObject(requestDto);
            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, data);
            await _tradingservice.GetCompanyUserHotKeyPreferences(requestResponseObj);

            return Ok();
        }

        /// <summary>
        /// This endpoint is used to update company user hot key preferences
        /// </summary>
        /// <remarks>
        /// Example Value | Schema :
        /// 
        ///     {"companyUserID":Number,
        ///     "hotKeyPreferenceElements":"string",
        ///     "enableBookMarkIcon":bool}
        /// </remarks>
        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_UPDATE_COMPANY_USER_HOT_KEY_PREFERENCES)]
        public async Task<IActionResult> Update_CompanyUserHotKeyPreferences(UserHotKeyPreferencesRequestDto requestDto)
        {
            //{"CompanyUserID":"17","HotKeyPreferenceElements":"Symbol^Broker^Venue^TIF^Order Type^Execution Type","EnableBookMarkIcon":false,"HotKeyOrderChanged":false}
            try
            {
                if (UserDtoObj.CompanyUserId == 0 ||
                    requestDto == null || string.IsNullOrEmpty(requestDto.HotKeyPreferenceElements))
                {
                    return BadRequest(MessageConstants.MSG_INVALID_REQUEST_PARAMETERS);
                }

                requestDto.CompanyUserID = UserDtoObj.CompanyUserId;
                string data = JsonConvert.SerializeObject(requestDto);
                RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, data);
                await _tradingservice.UpdateCompanyUserHotKeyPreferences(requestResponseObj);

                GenerateConsoleAndLog(MessageConstants.CONST_TRADING_CONTROLLER, ControllerMethodConstants.CONST_METHOD_UPDATE_COMPANY_USER_HOT_KEY_PREFERENCES, MessageConstants.CONST_METHOD_EXECUTION_ENDED);
                return Ok();

            }
            catch (Exception ex)
            {
                Logger.LogMessage(ex, LogEnums.LogPolicy.LogAndShow);
                return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
            }
        }

        /// <summary>
        /// This endpoint is used to get company user hot key preferences details
        /// </summary>
        /// <remarks>
        /// Example Value | Schema :
        /// 
        ///     {"CompanyUserHotKeyID":Number,
        ///     "CompanyUserID":Number,
        ///     "CompanyUserHotKeyName":"string",
        ///     "HotKeyPreferenceNameValue":"string",
        ///     "IsFavourites":bool,
        ///     "HotKeySequence":Number}
        /// </remarks>
        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_GET_COMPANY_USER_HOT_KEY_PREFERENCES_DETAILS)]
        public async Task<IActionResult> GET_CompanyUserHotKeyPreferencesDetails(BaseRequestDto requestDto)
        {
            try
            {
                if (UserDtoObj.CompanyUserId == 0)
                {
                    return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);
                }

                requestDto.CompanyUserID = UserDtoObj.CompanyUserId;
                string data = JsonConvert.SerializeObject(requestDto);
                RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, data);
                await _tradingservice.GetCompanyUserHotKeyPreferencesDetails(requestResponseObj);

                return Ok();
            }
            catch (Exception ex)
            {
                Logger.LogMessage(ex, LogEnums.LogPolicy.LogAndShow);
                return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
            }
        }

        /// <summary>
        /// This endpoint is used to update company user hot key preferences details
        /// </summary>
        /// <remarks>
        /// Example Value | Schema :
        /// 
        ///     {"companyUserID":Number,
        ///     "companyUserHotKeyName":"string",
        ///     "hotKeyPreferenceNameValue":"string",
        ///     "isFavourites":bool,
        ///     "hotKeySequence":Number}
        /// </remarks>
        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_UPDATE_COMPANY_USER_HOT_KEY_PREFERENCES_DETAILS)]
        public async Task<IActionResult> Update_CompanyUserHotKeyPreferencesDetails(SaveHotKeyPreferencesRequestDto dto)
        {
            try
            {
                GenerateConsoleAndLog(MessageConstants.CONST_TRADING_CONTROLLER, ControllerMethodConstants.CONST_METHOD_UPDATE_COMPANY_USER_HOT_KEY_PREFERENCES_DETAILS, MessageConstants.CONST_METHOD_EXECUTION_STARTED);

                if (UserDtoObj.CompanyUserId == 0 || dto == null)
                    return BadRequest(MessageConstants.MSG_INVALID_REQUEST_PARAMETERS);

                var data = JsonConvert.SerializeObject(dto);
                RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, data);
                await _tradingservice.UpdateCompanyUserHotKeyPreferencesDetails(requestResponseObj);

                GenerateConsoleAndLog(MessageConstants.CONST_TRADING_CONTROLLER, ControllerMethodConstants.CONST_METHOD_UPDATE_COMPANY_USER_HOT_KEY_PREFERENCES_DETAILS, MessageConstants.CONST_METHOD_EXECUTION_ENDED);
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.LogMessage(ex, LogEnums.LogPolicy.LogAndShow);
                return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
            }
        }

        /// <summary>
        /// This endpoint is used to save new company user hot key preferences details
        /// </summary>
        /// <remarks>
        /// Example Value | Schema :
        /// 
        ///     {"companyUserID":Number,
        ///     "companyUserHotKeyName":"string",
        ///     "hotKeyPreferenceNameValue":"string",
        ///     "isFavourites":bool,
        ///     "hotKeySequence":Number}
        /// </remarks>
        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_SAVE_COMPANY_USER_HOT_KEY_PREFERENCES_DETAILS)]
        public async Task<IActionResult> Save_CompanyUserHotKeyPreferencesDetails(SaveHotKeyPreferencesRequestDto dto)
        {
            try
            {
                if (UserDtoObj.CompanyUserId == 0 || dto == null)
                    return BadRequest(MessageConstants.MSG_INVALID_REQUEST_PARAMETERS);

                string data = JsonConvert.SerializeObject(dto);
                RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, data);

                using (LogKafkaId(requestResponseObj))
                {
                    logger.LogInformation("Received Save QTT request");
                    await _tradingservice.SaveCompanyUserHotKeyPreferencesDetails(requestResponseObj);
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                Logger.LogMessage(ex, LogEnums.LogPolicy.LogAndShow);
                return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
            }
        }

        /// <summary>
        /// This endpoint is used to delete company user hot key preferences details
        /// </summary>
        /// <remarks>
        /// Example Value | Schema :
        /// 
        ///     {"companyUserID":Number,
        ///     "companyUserHotKeyName":"string"}
        /// </remarks>
        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_DELETE_COMPANY_USER_HOT_KEY_PREFERENCES_DETAILS)]
        public async Task<IActionResult> Delete_CompanyUserHotKeyPreferencesDetails([FromBody] DeleteHotKeyRequestDto requestDto)
        {
            try
            {
                if (UserDtoObj.CompanyUserId == 0 ||
                    requestDto == null
                    //|| string.IsNullOrWhiteSpace(requestDto.CompanyUserHotKeyName
                    )
                    return BadRequest(MessageConstants.MSG_INVALID_REQUEST_PARAMETERS);

                string data = JsonConvert.SerializeObject(requestDto);
                RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, data);
                await _tradingservice.DeleteCompanyUserHotKeyPreferencesDetails(requestResponseObj);
                return Ok();

            }
            catch (Exception ex)
            {
                Logger.LogMessage(ex, LogEnums.LogPolicy.LogAndShow);
                return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
            }
        }

        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_PST_ACCOUNTS_NAV)]
        public async Task<IActionResult> PstAccountsNav(RequestDto<List<PstAccountNavsRequestDto>> pstRequestDto)
        {
            try
            {
                var correlationId = pstRequestDto.Data.First()?.CorrelationId;
                pstRequestDto.CorrelationId = correlationId;
                logger.LogInformation("Received PST account Nav request with correlationId " + correlationId);

                var reqRespModel = new RequestResponseModel(UserDtoObj.CompanyUserId, JsonConvert.SerializeObject(pstRequestDto.Data), correlationId);

                await _tradingservice.GetPstAccountNav(reqRespModel);
                await Task.CompletedTask;
                return Ok(new { IsSuccess = true, CorrelationId = correlationId });
            }
            catch (Exception ex)
            {
                Logger.LogMessage(ex, LogEnums.LogPolicy.LogAndShow);
                return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
            }
        }


        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_SEND_PST_ORDERS)]
        public async Task<IActionResult> SendPstOrders(RequestDto<string> requestDto)
        {
            try
            {
                if (UserDtoObj.CompanyUserId == 0)
                {
                    return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);
                }
                if (!string.IsNullOrEmpty(requestDto.Data))
                {
                    RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, requestDto.Data);
                    requestResponseObj.CorrelationId = requestDto.CorrelationId;
                    await _tradingservice.SendPstOrders(requestResponseObj);
                    return Ok();
                }
                return BadRequest(MessageConstants.MSG_INVALID_REQUEST_PARAMETERS);
            }
            catch (Exception ex)
            {
                Logger.LogMessage(ex, LogEnums.LogPolicy.LogAndShow);
                return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
            }
        }

        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_SEND_ORDERS_TO_MARKET)]
        public async Task<IActionResult> SendOrdersToMarket(RequestDto<string> requestDto)
        {
            try
            {
                if (UserDtoObj.CompanyUserId == 0)
                {
                    return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);
                }
                if (!string.IsNullOrEmpty(requestDto.Data))
                {
                    RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, requestDto.Data);
                    requestResponseObj.CorrelationId = requestDto.CorrelationId;
                    await _tradingservice.SendOrdersToMarket(requestResponseObj);
                    return Ok();
                }
                return BadRequest(MessageConstants.MSG_INVALID_REQUEST_PARAMETERS);
            }
            catch (Exception ex)
            {
                Logger.LogMessage(ex, LogEnums.LogPolicy.LogAndShow);
                return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
            }
        }


        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_CREATE_PST_ALLOCATION_PREF)]
        public async Task<IActionResult> CreatePstAllocatonPreference(RequestDto<string> requestDto)
        {
            try
            {
                if (UserDtoObj.CompanyUserId == 0)
                {
                    return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);
                }
                if (!string.IsNullOrEmpty(requestDto.Data))
                {
                    RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, requestDto.Data);
                    requestResponseObj.CorrelationId = requestDto.CorrelationId;
                    await _tradingservice.CreatePstAllocatonPreference(requestResponseObj);
                    return Ok();
                }
                return BadRequest("Invalid request paramters");
            }
            catch (Exception ex)
            {
                Logger.LogMessage(ex, LogEnums.LogPolicy.LogAndShow);
                return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
            }
        }

        [HttpPost]
        [Route(ControllerMethodConstants.CONST_GET_TRADE_ATTRIBUTES_LABELS)]
        public async Task<IActionResult> GetTradeAttributesLabels([FromBody] TradeAttributesDto requestDto)
        {
            try
            {
                if (requestDto.CompanyUserID == 0)
                {
                    return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);
                }

                var data = JsonConvert.SerializeObject(new { RequestId = requestDto.RequestId });

                RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, data, requestDto.CorrelationId);
                await _tradingservice.ProduceTradeAttributeLabelsEvent(requestResponseObj);
                return Ok(new { IsSuccess = true, CorrelationId = requestDto.CorrelationId });
            }
            catch (Exception ex)
            {
                Logger.LogMessage(ex, LogEnums.LogPolicy.LogAndShow);
                return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
            }
        }

        [HttpPost]
        [Route(ControllerMethodConstants.CONST_GET_TRADE_ATTRIBUTES_VALUES)]
        public async Task<IActionResult> GetTradeAttributesValues([FromBody] TradeAttributesDto requestDto)
        {
            try
            {
                if (requestDto.CompanyUserID == 0)
                {
                    return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);
                }

                var data = JsonConvert.SerializeObject(new { RequestId = requestDto.RequestId });

                RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, data, requestDto.CorrelationId);
                await _tradingservice.ProduceTradeAttributeValuesEvent(requestResponseObj);
                return Ok(new { IsSuccess = true, CorrelationId = requestDto.CorrelationId });
            }
            catch (Exception ex)
            {
                Logger.LogMessage(ex, LogEnums.LogPolicy.LogAndShow);
                return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
            }
        }

        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_VALIDATE_SYMBOL_UNIFIED)]
        public async Task<IActionResult> ValidateSymbolUnified([FromBody] ValidateSymbolUnifiedRequestDto request)
        {
            if (UserDtoObj.CompanyUserId == 0)
            {
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);
            }

            try
            {
                // Case 1: ValidateSymbol
                if (!string.IsNullOrEmpty(request.Symbol))
                {
                    await _securityValidationService.ValidateSymbolUnifiedAsync(new List<string> { request.Symbol }, UserDtoObj.CompanyUserId, CorrelationId, request.RequestID, false, "", request.Symbology);
                }
                // Case 2: ValidateOptionSymbol
                else if (!string.IsNullOrEmpty(request.OptionSymbol))
                {
                    await _securityValidationService.ValidateSymbolUnifiedAsync(new List<string> { request.OptionSymbol }, UserDtoObj.CompanyUserId, CorrelationId, request.RequestID, true, request.UnderLyingSymbol ?? string.Empty);
                }
                // Case 3: ValidateMultipleSymbols
                else if (request.Symbols != null && request.Symbols.Count > 0)
                {
                    await _securityValidationService.ValidateSymbolUnifiedAsync(request.Symbols, UserDtoObj.CompanyUserId, CorrelationId, string.Empty);
                }
                else
                {
                    return BadRequest(MessageConstants.MSG_INVALID_INPUT);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
            }
        }

    }
}