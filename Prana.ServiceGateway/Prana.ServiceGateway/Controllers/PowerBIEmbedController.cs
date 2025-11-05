using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Prana.ServiceGateway.Contracts;
using Prana.ServiceGateway.Services;
using Prana.ServiceGateway.ExceptionHandling;
using Prana.ServiceGateway.Constants;
using Prana.KafkaWrapper;
using Prana.ServiceGateway.Models.RequestDto;
using Azure.Core;
using Prana.KafkaWrapper.Extension.Classes;
using Microsoft.AspNetCore.Authorization;
using Serilog.Context;

namespace Prana.ServiceGateway.Controllers
{
    [ApiController]
    [Route(ControllerMethodConstants.CONST_CONTROLLER)]
    public class PowerBIEmbedController : BaseController
    {
        private readonly IPowerBiEmbedReportService _powerbiservice;
        private readonly ILogger<PowerBIEmbedController> logger;
        public PowerBIEmbedController(IValidationHelper validationHelper,
            ITokenService tokenService,
            IPowerBiEmbedReportService powerBIService,
            ILogger<PowerBIEmbedController> logger) : base(validationHelper, tokenService)
        {
            _powerbiservice = powerBIService;
            this.logger = logger;
        }

        [HttpGet]
        [Route(ControllerMethodConstants.CONST_GET_POWERBI_EMBED_INFO)] 
        public async Task<IActionResult> GetPowerBIReportEmbedInfo(string workspaceId, string reportId)
        {
            using var f = LogContext.PushProperty(LogConstant.CORRELATION_ID, CorrelationId);
            using var f1 = LogContext.PushProperty(LogConstant.USER_ID, UserDtoObj?.CompanyUserId);
            try
            {
                // Validate required parameters
                if (string.IsNullOrEmpty(workspaceId))
                    return BadRequest("WorkspaceId is required");

                if (string.IsNullOrEmpty(reportId))
                    return BadRequest("ReportId is required");

                var embedInfo = await _powerbiservice.GetPowerBIReportEmbedInfo(workspaceId, reportId);

                var respDto = new ResponseDto(CorrelationId, embedInfo, embedInfo != null);
                return Ok(respDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}