using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using Prana.ServiceGateway.Constants;
using Prana.ServiceGateway.Controllers;
using Prana.ServiceGateway.Contracts;
using Prana.ServiceGateway.Services;
using Prana.ServiceGateway.UnitTest.Commons;
using Xunit;

namespace Prana.ServiceGateway.UnitTest.ControllerTests
{
    public class WatchListControllerTest : BaseControllerTest, IDisposable
    {
        private readonly WatchListController _controller;

        public WatchListControllerTest() : base()
        {
            _controller = new WatchListController(
                _mockValidationHelper.Object,
                _mockTokenService.Object,
                _mockLiveFeedService.Object,
                _mockWatchlistDataService.Object
            );
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) { }

        [Theory]
        [InlineData(1)]
        public async Task GetTabNames_ReturnsOkResult(int userId)
        {
            var tabNames = new Dictionary<string, int> { { "Tab1", 1 } };
            _mockWatchlistDataService.Setup(s => s.GetTabNames(userId)).ReturnsAsync(tabNames);

            var result = await _controller.GetTabNames(userId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<string>(okResult.Value);
            Assert.Equal(JsonConvert.SerializeObject(tabNames), response);
        }

        [Theory]
        [InlineData(1)]
        public async Task GetTabWiseSymbols_ReturnsOkResult(int userId)
        {
            var tabWiseSymbols = new Dictionary<string, HashSet<string>> { { "Tab1", new HashSet<string> { "AAPL" } } };
            _mockWatchlistDataService.Setup(s => s.GetTabWiseSymbols(userId)).ReturnsAsync(tabWiseSymbols);

            var result = await _controller.GetTabWiseSymbols(userId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<string>(okResult.Value);
            Assert.Equal(JsonConvert.SerializeObject(tabWiseSymbols), response);
        }

        [Theory]
        [InlineData(1, "NewTab")]
        public async Task AddTab_ReturnsOkResult(int userId, string tabName)
        {
            _mockWatchlistDataService.Setup(s => s.AddTab(userId, tabName)).Returns(Task.CompletedTask);

            var result = await _controller.AddTab(userId, tabName);

            var okResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Theory]
        [InlineData(1, "OldTab", "NewTab")]
        public async Task RenameTab_ReturnsOkResult(int userId, string oldTabName, string newTabName)
        {
            _mockWatchlistDataService.Setup(s => s.RenameTab(userId, oldTabName, newTabName)).Returns(Task.CompletedTask);

            var result = await _controller.RenameTab(userId, oldTabName, newTabName);

            var okResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Theory]
        [InlineData(1, "TabToDelete")]
        public async Task DeleteTab_ReturnsOkResult(int userId, string tabName)
        {
            _mockWatchlistDataService.Setup(s => s.DeleteTab(userId, tabName)).Returns(Task.CompletedTask);

            var result = await _controller.DeleteTab(userId, tabName);

            var okResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Theory]
        [InlineData(1, "AAPL", "Favorites")]
        public async Task AddSymbolInTab_ReturnsOkResult(int userId, string symbol, string tabName)
        {
            _mockWatchlistDataService.Setup(s => s.AddSymbolInTab(userId, symbol, tabName)).Returns(Task.CompletedTask);

            var result = await _controller.AddSymbolInTab(userId, symbol, tabName);

            var okResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Theory]
        [InlineData(1, "AAPL", "Favorites")]
        public async Task RemoveSymbolFromTab_ReturnsOkResult(int userId, string symbol, string tabName)
        {
            _mockWatchlistDataService.Setup(s => s.RemoveSymbolFromTab(userId, symbol, tabName)).Returns(Task.CompletedTask);

            var result = await _controller.RemoveSymbolFromTab(userId, symbol, tabName);

            var okResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }
    }
}