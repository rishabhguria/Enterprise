using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Prana.KafkaWrapper;
using Prana.ServiceGateway.Constants;
using Prana.ServiceGateway.Contracts;
using Prana.ServiceGateway.ExceptionHandling;
using Prana.ServiceGateway.Models;

namespace Prana.ServiceGateway.Controllers
{
    [Produces(ControllerMethodConstants.CONST_APPLICATION_JSON)]
    [Route(ControllerMethodConstants.CONST_CONTROLLER_ACTION)]
    [ApiController]
    public class OpenfinManagementController : BaseController
    {
        private readonly IOpenfinManagerService _openfinManagerService;

        public OpenfinManagementController(IValidationHelper validationHelper, ITokenService tokenService, IOpenfinManagerService openfinManagerService) : base(validationHelper, tokenService)
        {
            _openfinManagerService = openfinManagerService;
        }

        [HttpGet]
        [Route(ControllerMethodConstants.CONST_METHOD_GET_DEFAULT_OPENFIN_WORKSPACE)]
        public async Task<IActionResult> GetSavedWorkspaceInformationForUser()
        {
            if (UserDtoObj.CompanyUserId == 0)
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);

            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, "", CorrelationId);
            using (LogKafkaId(requestResponseObj))
            {
                try
                {
                    await _openfinManagerService.GetOpenfinWorkspaceLayout(requestResponseObj);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
                }
            }
        }


        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_SAVE_DEFAULT_OPENFIN_WORKSPACE)]
        public async Task<IActionResult> SaveWorkspaceInformationForUser([FromBody] GenericDto openfinWorkspaceInformation)
        {
            if (UserDtoObj.CompanyUserId == 0)
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);

            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, (openfinWorkspaceInformation.Data), CorrelationId);

            using (LogKafkaId(requestResponseObj))
            {
                try
                {
                    await _openfinManagerService.SaveOpenfinWorkspaceLayout(requestResponseObj);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
                }
            }
        }

        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_DELETE_OPENFIN_WORKSPACE)]
        public async Task<IActionResult> DeleteWorkspaceInformationForUser([FromBody] GenericDto openfinWorkspaceInformation)
        {
            if (UserDtoObj.CompanyUserId == 0)
                return BadRequest(MessageConstants.MSG_CONST_COMPANYUSERID_CANNOT_BLANK);

            RequestResponseModel requestResponseObj = new(UserDtoObj.CompanyUserId, (openfinWorkspaceInformation.Data), CorrelationId);

            using (LogKafkaId(requestResponseObj))
            {
                await _openfinManagerService.DeleteOpenfinWorkspaceLayout(requestResponseObj);
                return Ok();
            }
        }
    }
}
