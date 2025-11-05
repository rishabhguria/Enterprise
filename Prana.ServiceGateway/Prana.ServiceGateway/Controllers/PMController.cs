using Microsoft.AspNetCore.Mvc;
using Prana.ServiceGateway.Constants;
using Prana.ServiceGateway.Contracts;

namespace Prana.ServiceGateway.Controllers
{
    [ApiController]
    [Route(ControllerMethodConstants.CONST_CONTROLLER_ACTION)]
    public class PMController : BaseController
    {
        public PMController(IValidationHelper validationHelper, ITokenService tokenService) : base(validationHelper, tokenService)
        {
        }

        [HttpPost(Name = ControllerMethodConstants.CONST_REQUEST_CONTINUOUS_DATA)]
        public void RequestContinousData()
        {

        }
    }
}