using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Prana.ServiceGateway.Constants;
using Prana.ServiceGateway.Controllers;
using Prana.ServiceGateway.Contracts;
using Prana.ServiceGateway.Models;
using Prana.ServiceGateway.UnitTest.Commons;
using ServiceGateway.Controllers;
using Xunit;
using Prana.ServiceGateway.Services;

namespace Prana.ServiceGateway.UnitTest.ControllerTests
{
    public class ReportsPortalControllerTest : BaseControllerTest, IDisposable
    {
        private readonly ReportsPortalController _controller;
        private readonly Mock<IReportsPortalService> _mockReportsPortalService;

        public ReportsPortalControllerTest() : base()
        {
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(config => config[ControllerMethodConstants.CONST_TOUCH_BASE_URL])
                             .Returns("https://mock-touch-service/api/");

            _mockReportsPortalService = new Mock<IReportsPortalService>();

            _controller = new ReportsPortalController(
                _mockValidationHelper.Object,
                _mockTokenService.Object,
                _mockReportsPortalService.Object,
                mockConfiguration.Object,
                GetMockLogger<ReportsPortalController>()
            );
        }

        public void Dispose() { }

        [Fact]
        public async Task GetNewlyApprovedReportsAndStatus_ReturnsBadRequest_WhenNull()
        {
            var result = await _controller.GetNewlyApprovedReportsAndStatus(null);
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(MessageConstants.MSG_CONST_CANNOT_BE_NULL, badResult.Value);
        }

        [Fact]
        public async Task ZipReportFiles_ReturnsBadRequest_WhenNull()
        {
            var result = await _controller.ZipReportFiles(null);
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(MessageConstants.MSG_CONST_CANNOT_BE_NULL, badResult.Value);
        }

        [Fact]
        public async Task DownloadReportsZip_ReturnsBadRequest_WhenNull()
        {
            var result = await _controller.DownloadReportsZip(null);
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(MessageConstants.MSG_CONST_CANNOT_BE_NULL, badResult.Value);
        }

        [Fact]
        public async Task DownloadExcelFile_ReturnsBadRequest_WhenNull()
        {
            var result = await _controller.DownloadExcelFile(null);
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(MessageConstants.MSG_CONST_CANNOT_BE_NULL, badResult.Value);
        }

        [Fact]
        public async Task EntitySelect_ReturnsBadRequest_WhenNull()
        {
            var result = await _controller.EntitySelect(null);
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(MessageConstants.MSG_CONST_CANNOT_BE_NULL, badResult.Value);
        }

        [Fact]
        public async Task SetUserPreferences_ReturnsBadRequest_WhenNull()
        {
            var result = await _controller.SetUserPreferences(null);
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(MessageConstants.MSG_CONST_CANNOT_BE_NULL, badResult.Value);
        }

        [Fact]
        public async Task SetUserDefaultLayoutPreferences_ReturnsBadRequest_WhenNull()
        {
            var result = await _controller.SetUserDefaultLayoutPreferences(null);
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(MessageConstants.MSG_CONST_CANNOT_BE_NULL, badResult.Value);
        }

        [Fact]
        public void DefaultLayout_ReturnsOk()
        {
            var layout = new SaveDefaultLayoutDto();
            _mockReportsPortalService.Setup(s => s.FetchDefaultLayout(layout)).Returns("LayoutJson");
            var result = _controller.DefaultLayout(layout);
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("LayoutJson", okResult.Value);
        }

        [Fact]
        public async Task GenerateReport_ReturnsBadRequest_WhenSelectedReportsNull()
        {
            var dto = new ReportFilesDto { selectedReports = null };
            var result = await _controller.GenerateReport(dto);
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(MessageConstants.MSG_CONST_CANNOT_BE_NULL, badResult.Value);
        }

        [Fact]
        public async Task RemoveReport_ReturnsBadRequest_WhenSelectedReportsNull()
        {
            var dto = new ReportFilesDto { selectedReports = null };
            var result = await _controller.RemoveReport(dto);
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(MessageConstants.MSG_CONST_CANNOT_BE_NULL, badResult.Value);
        }

        [Fact]
        public async Task CancelApproval_ReturnsBadRequest_WhenNull()
        {
            var result = await _controller.CancelApproval(null);
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(MessageConstants.MSG_CONST_CANNOT_BE_NULL, badResult.Value);
        }
    }
}
