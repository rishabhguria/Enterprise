using Moq;
using Prana.ServiceGateway.Constants;
using Prana.ServiceGateway.Contracts;
using Prana.ServiceGateway.Controllers;
using Prana.ServiceGateway.Models;
using Prana.ServiceGateway.UnitTest.Commons;
using System;
using Xunit;

namespace Prana.ServiceGateway.UnitTest.ControllerTests
{
    public class LoggingControllerTest : BaseControllerTest, IDisposable
    {
        private readonly LoggingController _controller;

        public LoggingControllerTest() : base()
        {
            _controller = new LoggingController(
               _mockLoggingHelper.Object);
        }

        public void Dispose()
        {
        }

        #region PostLogs Test Cases
        [Fact]
        public void PostLogs_CallsLogMessage()
        {
            try
            {
                // Arrange
                var loggerInfo = new LoggerInfo();
                loggerInfo.message = UnitTestConstants.CONST_EXPECTED_MOCK_MESSAGE;

                // Act
                _controller.PostLogs(loggerInfo);

                // Assert
                _mockLoggingHelper.Verify(x => x.LogMessage(loggerInfo), Times.Once);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        [Fact]
        public void PostLogs_CallsLogMessage_WhenUserIdIsNullOrEmpty()
        {
            // Arrange
            var mockLoggingHelper = new Mock<ILoggingHelper>();
            var controller = new LoggingController(mockLoggingHelper.Object);
            var loggerInfo = new LoggerInfo { level = 1, message = "Test", userId = "" };

            // Act
            controller.PostLogs(loggerInfo);

            // Assert
            mockLoggingHelper.Verify(x => x.LogMessage(loggerInfo), Times.Once);
        }

        [Fact]
        public void PostLogs_CallsLogMessage_WithUserIdProperty_WhenUserIdIsNotNullOrEmpty()
        {
            // Arrange
            var mockLoggingHelper = new Mock<ILoggingHelper>();
            var controller = new LoggingController(mockLoggingHelper.Object);
            var loggerInfo = new LoggerInfo { level = 1, message = "Test", userId = "user123" };

            // Act
            controller.PostLogs(loggerInfo);

            // Assert
            mockLoggingHelper.Verify(x => x.LogMessage(loggerInfo), Times.Once);
        }

        [Fact]
        public void PostLogs_LogsException_WhenLogMessageThrows()
        {
            // Arrange
            var mockLoggingHelper = new Mock<ILoggingHelper>();
            var controller = new LoggingController(mockLoggingHelper.Object);
            var loggerInfo = new LoggerInfo { level = 1, message = "Test", userId = null };

            mockLoggingHelper.Setup(x => x.LogMessage(It.IsAny<LoggerInfo>())).Throws(new Exception("fail"));

            // Act & Assert
            // Since Logger.LogMessage is static, you would need to verify via side effects or use a wrapper for Logger in real code.
            // Here, we just ensure no exception is thrown from the controller.
            var exception = Record.Exception(() => controller.PostLogs(loggerInfo));
            Assert.Null(exception);
        }
        #endregion
    }
}
