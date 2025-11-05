using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Prana.ServiceGateway.Constants;
using Prana.ServiceGateway.Contracts;
using Prana.ServiceGateway.Controllers;
using Prana.ServiceGateway.ExceptionHandling;
using Prana.ServiceGateway.Models;
using Prana.ServiceGateway.Services;
using System.Text.Json;

namespace ServiceGateway.Controllers
{
    [Produces(ControllerMethodConstants.CONST_APPLICATION_JSON)]
    [Route(ControllerMethodConstants.CONST_CONTROLLER)]
    [ApiController]
    public class ReportsPortalController : BaseController
    {
        private static readonly HttpClient _client = new HttpClient { Timeout = TimeSpan.FromSeconds(1200) };
        private readonly IReportsPortalService _reportsPortalService;
        private IConfiguration _configuration;
        private readonly ILogger<ReportsPortalController> logger;
        private string _touchUrl;

        /// <summary>
        /// Constructor for API Controller including injection of parameters
        /// </summary>
        /// <param name="validationHelper"></param>
        /// <param name="tokenService"></param>
        /// <param name="reportsPortalService"></param>
        /// <param name="configuration"></param>
        /// <param name="_logger"></param>

        public ReportsPortalController(IValidationHelper validationHelper, ITokenService tokenService,
          IReportsPortalService reportsPortalService,
          IConfiguration configuration,
          ILogger<ReportsPortalController> _logger) : base(validationHelper, tokenService)
        {
            _reportsPortalService = reportsPortalService;
            _configuration = configuration;
            logger = _logger;
            _touchUrl = _configuration[ControllerMethodConstants.CONST_TOUCH_BASE_URL];
            if (!string.IsNullOrWhiteSpace(_touchUrl))
                _touchUrl = _touchUrl?.TrimEnd('/');   //remove ending slash if exists
        }


        [HttpGet]
        [Route(ControllerMethodConstants.CONST_METHOD_GET_CUTOFFDATE)]
        public async Task<IActionResult> GetCutOffDate()
        {
            var url = _touchUrl + ControllerMethodConstants.CONST_REPORT_PORTAL_API + ControllerContext.ActionDescriptor.ActionName;
            string res = await _reportsPortalService.GetApi(url);
            return Ok(res);


        }

        [HttpGet]
        [Route(ControllerMethodConstants.CONST_METHOD_GET_USER_PREFRENCES)]
        public async Task<IActionResult> GetUserPreferences()
        {
            var url = _touchUrl + ControllerMethodConstants.CONST_REPORT_PORTAL_API + ControllerContext.ActionDescriptor.ActionName;
            string res = await _reportsPortalService.GetApi(url);
            return Ok(res);
        }

        [HttpGet]
        [Route(ControllerMethodConstants.CONST_IS_SESSION_ALIVE)]
        public async Task<object> IsAlive()
        {
            var url = _touchUrl + ControllerMethodConstants.CONST_SSO + ControllerContext.ActionDescriptor.ActionName; 
            var res = await _reportsPortalService.GetApi(url);

            logger.LogInformation($"IsAlive method called in service gateway and response is {res} for the endpoint {url}", LogEnums.LogPolicy.LogAndShow);
            return res;
        }

        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_POST_GetNewlyApprovedReportsAndStatus)]
        public async Task<IActionResult> GetNewlyApprovedReportsAndStatus([FromBody] GetNewlyApprovedReportsDto bodyJSON)
        {
            if (bodyJSON == null)
                return BadRequest(MessageConstants.MSG_CONST_CANNOT_BE_NULL);

            var url = _touchUrl + ControllerMethodConstants.CONST_REPORT_PORTAL_API + ControllerContext.ActionDescriptor.ActionName;
            var res = await _reportsPortalService.PostApi(url: url, BodyJSON: bodyJSON);
            return Ok(res);

        }

        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_POST_ZipReportFiles)]
        public async Task<IActionResult> ZipReportFiles([FromBody] ZipReportFilesDto bodyJSON)
        {
            if (bodyJSON == null)
                return BadRequest(MessageConstants.MSG_CONST_CANNOT_BE_NULL);

            var url = _touchUrl +
                ControllerMethodConstants.CONST_REPORT_PORTAL_API + ControllerContext.ActionDescriptor.ActionName;
            var res = await _reportsPortalService.PostApi(url: url, BodyJSON: bodyJSON);
            return Ok(res);

        }

        [HttpGet]
        [Route(ControllerMethodConstants.CONST_METHOD_POST_DownloadReportsZip)]
        public async Task<IActionResult> DownloadReportsZip([FromQuery] DownloadReportsZipDto bodyJSON)
        {
            if (bodyJSON == null)
                return BadRequest(MessageConstants.MSG_CONST_CANNOT_BE_NULL);

            var url = _touchUrl + ControllerMethodConstants.CONST_REPORT_PORTAL_API + ControllerContext.ActionDescriptor.ActionName;
            var res = await _reportsPortalService.PostApi(url: url, BodyJSON: bodyJSON);
            return Ok(res);

        }

        [HttpGet]
        [Route(ControllerMethodConstants.CONST_METHOD_POST_DownloadExcelFile)]
        public async Task<IActionResult> DownloadExcelFile([FromQuery] DownloadExcelFileDto bodyJSON)
        {
            if (bodyJSON == null)
                return BadRequest(MessageConstants.MSG_CONST_CANNOT_BE_NULL);

            var url = _touchUrl + ControllerMethodConstants.CONST_REPORT_PORTAL_API + ControllerContext.ActionDescriptor.ActionName;
            var res = await _reportsPortalService.PostApi(url: url, BodyJSON: bodyJSON);
            return Ok(res);

        }

        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_POST_EntitySelect)]
        public async Task<IActionResult> EntitySelect([FromBody] EntitySelectDto bodyJSON)
        {
            if (bodyJSON == null)
                return BadRequest(MessageConstants.MSG_CONST_CANNOT_BE_NULL);

            var url = _touchUrl + ControllerMethodConstants.CONST_REPORT_PORTAL_API + ControllerContext.ActionDescriptor.ActionName;
            var res = await _reportsPortalService.PostApi(url: url, BodyJSON: bodyJSON);
            return Ok(res);

        }

        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_SET_USER_PREFRENCES)]
        public async Task<IActionResult> SetUserPreferences([FromBody] SetUserPreferencesDto bodyJSON)
        {
            if (bodyJSON == null)
                return BadRequest(MessageConstants.MSG_CONST_CANNOT_BE_NULL);

            var url = _touchUrl + ControllerMethodConstants.CONST_REPORT_PORTAL_API + ControllerContext.ActionDescriptor.ActionName;
            var res = await _reportsPortalService.PostApi(url: url, BodyJSON: bodyJSON);
            return Ok(res);

        }

        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_SET_USER_LAYOUT_PREFERENCES)]
        public async Task<IActionResult> SetUserDefaultLayoutPreferences([FromBody] SetLayoutUserPreferncesDto bodyJSON)
        {
            if (bodyJSON == null)
                return BadRequest(MessageConstants.MSG_CONST_CANNOT_BE_NULL);

            var url = _touchUrl + ControllerMethodConstants.CONST_REPORT_PORTAL_API + ControllerContext.ActionDescriptor.ActionName;
            var res = await _reportsPortalService.PostApi(url: url, BodyJSON: bodyJSON);
            return Ok(res);

        }

        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_POST_GetAllMasterfunds)]
        public async Task<IActionResult> GetAllMasterfunds()
        {
            var url = _touchUrl + ControllerMethodConstants.CONST_REPORT_PORTAL_API + ControllerContext.ActionDescriptor.ActionName;
            var res = await _reportsPortalService.PostApi(url: url, string.Empty);
            return Ok(res);

        }

        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_POST_SaveDefaultLayout)]
        public IActionResult SaveDefaultLayout(SaveDefaultLayoutDto layout)
        {
            var res = _reportsPortalService.StoreLayout(layout);
            return Ok(new ResponseData(res));
        }

        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_DEFAULT_LAYOUT)]
        public IActionResult DefaultLayout(SaveDefaultLayoutDto layout)
        {
            var res = _reportsPortalService.FetchDefaultLayout(layout);
            return Ok(res);


        }

        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_POST_GenerateReport)]
        public async Task<IActionResult> GenerateReport([FromBody] ReportFilesDto bodyJSON)
        {
            if (bodyJSON.selectedReports == null)
                return BadRequest(MessageConstants.MSG_CONST_CANNOT_BE_NULL);

            var url = _touchUrl + ControllerMethodConstants.CONST_REPORT_PORTAL_API + ControllerContext.ActionDescriptor.ActionName;
            var res = await _reportsPortalService.PostApi(url: url, BodyJSON: bodyJSON);
            return Ok(res);


        }

        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_POST_RemoveReport)]
        public async Task<IActionResult> RemoveReport([FromBody] ReportFilesDto bodyJSON)
        {
            if (bodyJSON.selectedReports == null)
                return BadRequest(MessageConstants.MSG_CONST_CANNOT_BE_NULL);

            var url = _touchUrl + ControllerMethodConstants.CONST_REPORT_PORTAL_API + ControllerContext.ActionDescriptor.ActionName;
            var res = await _reportsPortalService.PostApi(url: url, BodyJSON: bodyJSON);
            return Ok(res);


        }

        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_POST_CancelApproval)]
        public async Task<IActionResult> CancelApproval([FromBody] GetNewlyApprovedReportsDto bodyJSON)
        {
            if (bodyJSON == null)
                return BadRequest(MessageConstants.MSG_CONST_CANNOT_BE_NULL);

            var url = _touchUrl + ControllerMethodConstants.CONST_REPORT_PORTAL_API + ControllerContext.ActionDescriptor.ActionName;
            var res = await _reportsPortalService.PostApi(url: url, BodyJSON: bodyJSON);
            return Ok(res);
        }

        [HttpGet]
        [Route(ControllerMethodConstants.CONST_METHOD_REPORT_APPROVAL_LOG)]
        public async Task<IActionResult> ReportsApprovalLogData()
        {
            var url = _touchUrl + ControllerMethodConstants.CONST_REPORT_PORTAL_API + ControllerContext.ActionDescriptor.ActionName;
            string res = await _reportsPortalService.GetApi(url);
            return Ok(res);
        }
    }
}