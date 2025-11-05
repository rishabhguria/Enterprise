using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Prana.ServiceGateway.Contracts;
using Prana.ServiceGateway.Services;
using Prana.ServiceGateway.ExceptionHandling;
using Prana.ServiceGateway.Constants;
using Prana.ServiceGateway.Utility.CustomAttributes;

namespace Prana.ServiceGateway.Controllers
{
    [ApiController]
    [Route(ControllerMethodConstants.CONST_CONTROLLER)]
    [ServiceHealthGate(ServiceNameConstants.CONST_WatchlistData_Name, ServiceNameConstants.CONST_WatchlistData_DisplayName)]
    public class WatchListController : BaseController
    {
        private readonly ILiveFeedService _liveFeedService;
        private readonly IWatchlistDataService _watchlistDataService;

        public WatchListController(IValidationHelper validationHelper,
            ITokenService tokenService,
            ILiveFeedService liveFeedService,
            IWatchlistDataService watchlistDataService
            ) : base(validationHelper, tokenService)
        {
            _liveFeedService = liveFeedService;
            _watchlistDataService = watchlistDataService;
        }

        [HttpGet]
        [Route(ControllerMethodConstants.CONST_METHOD_GET_TAB_NAMES)]
        public async Task<IActionResult> GetTabNames(int userId)
        {
            Dictionary<string, int> tabNames = await _watchlistDataService.GetTabNames(userId);
            return Ok(JsonConvert.SerializeObject(tabNames));

        }

        [HttpGet]
        [Route(ControllerMethodConstants.CONST_METHOD_GET_TABWISE_SYMBOLS)]
        public async Task<IActionResult> GetTabWiseSymbols(int userId)
        {
            Dictionary<string, HashSet<string>> tabWiseSymbols = await _watchlistDataService.GetTabWiseSymbols(userId);
            return Ok(JsonConvert.SerializeObject(tabWiseSymbols));

        }

        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_ADD_TAB)]
        public async Task<IActionResult> AddTab(int userId, string tabName)
        {
            await _watchlistDataService.AddTab(userId, tabName);
            return Ok();
        }

        [HttpPut]
        [Route(ControllerMethodConstants.CONST_METHOD_RENAME_TAB)]
        public async Task<IActionResult> RenameTab(int userId, string oldTabName, string newTabName)
        {
            await _watchlistDataService.RenameTab(userId, oldTabName, newTabName);
            return Ok();
        }

        [HttpDelete]
        [Route(ControllerMethodConstants.CONST_METHOD_DELETE_TAB)]
        public async Task<IActionResult> DeleteTab(int userId, string tabName)
        {
            await _watchlistDataService.DeleteTab(userId, tabName);
            return Ok();

        }

        [HttpPost]
        [Route(ControllerMethodConstants.CONST_METHOD_ADD_SYMBOL_IN_TAB)]
        public async Task<IActionResult> AddSymbolInTab(int userId, string symbol, string tabName)
        {
            await _watchlistDataService.AddSymbolInTab(userId, symbol, tabName);
            return Ok();

        }

        [HttpDelete]
        [Route(ControllerMethodConstants.CONST_METHOD_REMOVE_SYMBOL_FROM_TAB)]
        public async Task<IActionResult> RemoveSymbolFromTab(int userId, string symbol, string tabName)
        {
            await _watchlistDataService.RemoveSymbolFromTab(userId, symbol, tabName);
            return Ok();

        }
    }
}