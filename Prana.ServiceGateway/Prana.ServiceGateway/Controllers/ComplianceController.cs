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
    [Produces(ControllerMethodConstants.CONST_APPLICATION_JSON)]
    [Route(ControllerMethodConstants.CONST_CONTROLLER)]
    [ServiceHealthGate(ServiceNameConstants.CONST_ComplianceAlerts_Name, ServiceNameConstants.CONST_ComplianceAlerts_DisplayName)]
    [ApiController]
    public class ComplianceController : BaseController
    {
        private readonly IComplianceService _complianceService;

        public ComplianceController(IValidationHelper validationHelper, ITokenService tokenService, IComplianceService complianceService) : base(validationHelper, tokenService)
        {
            _complianceService = complianceService;
        }

        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_SEND_COMPLIANCE_DATA)]
        public async Task<IActionResult> SendComplianceData([FromBody] GenericDto data)
        {
            if (UserDtoObj.CompanyUserId == 0)
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);

            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, data.Data, CorrelationId);
            using (LogKafkaId(requestResponseObj))
            {
                try
                {
                    await _complianceService.SendComplianceData(requestResponseObj);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
                }
            }
        }

        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_SEND_COMPLIANCE_DATA_FOR_STAGE)]
        public async Task<IActionResult> SendComplianceDataForStage([FromBody] GenericDto data)
        {
            if (UserDtoObj.CompanyUserId == 0)
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);

            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, data.Data, CorrelationId);
            using (LogKafkaId(requestResponseObj))
            {
                try
                {
                    await _complianceService.SendComplianceDataForStage(requestResponseObj);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
                }
            }
        }


        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_CHECK_COMPLIANCE_FR_BASKET)]
        public async Task<IActionResult> CheckComplianceFrBasket([FromBody] RequestResponseModel requestResponseObj)
        {
            if (UserDtoObj.CompanyUserId == 0)
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);

            if (requestResponseObj.Data == "[]")
                return BadRequest("Parameter Data is misssing");

            requestResponseObj.CompanyUserID = UserDtoObj.CompanyUserId;
            using (LogKafkaId(requestResponseObj))
            {
                await _complianceService.CheckComplianceFrBasket(requestResponseObj);
                return Ok();
            }
        }
    }
}
