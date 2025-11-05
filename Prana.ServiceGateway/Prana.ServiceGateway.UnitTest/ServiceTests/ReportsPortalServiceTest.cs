using Microsoft.Extensions.Logging;
using Moq;
using Prana.ServiceGateway.Constants;
using Prana.ServiceGateway.Contracts;
using Prana.ServiceGateway.Models;
using Prana.ServiceGateway.Services;
using Prana.ServiceGateway.UnitTest.Commons; // Assuming BaseControllerTest and UnitTestConstants are here
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Http; // For IHttpContextAccessor
using Microsoft.Extensions.Configuration; // For IConfiguration
using System.Text;
using Newtonsoft.Json; // For JsonConvert

namespace Prana.ServiceGateway.UnitTest.ServiceTests
{
    // Inherit from BaseControllerTest to reuse common mocks and setup
    public class ReportsPortalServiceTest : BaseControllerTest
    {
        private readonly IReportsPortalService _reportsPortalService;
        private readonly Mock<ILogger<ReportsPortalService>> _mockLogger;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly Mock<HttpContext> _mockHttpContext;
        private readonly Mock<HttpRequest> _mockHttpRequest;
        private readonly Mock<IHeaderDictionary> _mockRequestHeaders;

        // A custom HttpMessageHandler for mocking HttpClient responses.
        // This is necessary because HttpClient is static and cannot be directly mocked by Moq.
        // In a real-world scenario, you would typically inject HttpClient via IHttpClientFactory.
        public class MockHttpMessageHandler : HttpMessageHandler
        {
            private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _sendAsyncFunc;

            public MockHttpMessageHandler(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncFunc)
            {
                _sendAsyncFunc = sendAsyncFunc;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return _sendAsyncFunc(request, cancellationToken);
            }
        }

        public ReportsPortalServiceTest() : base()
        {
            _mockLogger = CreateMockLogger<ReportsPortalService>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _mockHttpContext = new Mock<HttpContext>();
            _mockHttpRequest = new Mock<HttpRequest>();
            _mockRequestHeaders = new Mock<IHeaderDictionary>();

            // Setup HttpContextAccessor mocks
            _mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(_mockHttpContext.Object);
            _mockHttpContext.Setup(c => c.Request).Returns(_mockHttpRequest.Object);
            _mockHttpRequest.Setup(r => r.Headers).Returns(_mockRequestHeaders.Object);

            // Default setup for headers to avoid NullReferenceException in getSession
            _mockRequestHeaders.Setup(h => h[ServicesMethodConstants.CONST_CONKIE]).Returns("mockCookie1");
            _mockRequestHeaders.Setup(h => h[ServicesMethodConstants.CONST_CONKIE2]).Returns("mockCookie2");

            // Initialize the service with mocked dependencies
            _reportsPortalService = new ReportsPortalService(
                _mockConfiguration.Object,
                _mockHttpContextAccessor.Object,
                _mockLogger.Object
            );
        }

        #region getSession Test Cases (Indirectly Covered)
        // The getSession method is private and its logic is covered by GetApi and PostApi tests.
        // We can add a specific test for it if we make it public or use reflection,
        // but for simplicity, we'll rely on its coverage via public methods.

        [Fact]
        public void GetSession_CombinesHeadersCorrectly()
        {
            // Arrange
            string mockCookie1 = "cookieValue1";
            string mockCookie2 = "cookieValue2";

            _mockRequestHeaders.Setup(h => h[ServicesMethodConstants.CONST_CONKIE]).Returns(mockCookie1);
            _mockRequestHeaders.Setup(h => h[ServicesMethodConstants.CONST_CONKIE2]).Returns(mockCookie2);

            // Use reflection to call the private getSession method
            var getSessionMethod = typeof(ReportsPortalService).GetMethod("getSession", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Act
            var result = getSessionMethod.Invoke(_reportsPortalService, null) as string;

            // Assert
            string expectedSession = ServicesMethodConstants.CONST_GATEWAY_SESSION
                                    + mockCookie1
                                    + ServicesMethodConstants.CONST_GATEWAY_AUTH
                                    + mockCookie2
                                    + ServicesMethodConstants.CONST_SEMICOLON;
            Assert.Equal(expectedSession, result);
        }

        [Fact]
        public void GetSession_HandlesMissingHeaders()
        {
            // Arrange
            // Simulate missing headers by not setting up the mock for them
            _mockRequestHeaders.Setup(h => h[ServicesMethodConstants.CONST_CONKIE]).Returns((string)null);
            _mockRequestHeaders.Setup(h => h[ServicesMethodConstants.CONST_CONKIE2]).Returns((string)null);

            var getSessionMethod = typeof(ReportsPortalService).GetMethod("getSession", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Act
            var result = getSessionMethod.Invoke(_reportsPortalService, null) as string;

            // Assert
            string expectedSession = ServicesMethodConstants.CONST_GATEWAY_SESSION
                                    + "" // null header will become empty string when concatenated
                                    + ServicesMethodConstants.CONST_GATEWAY_AUTH
                                    + "" // null header will become empty string when concatenated
                                    + ServicesMethodConstants.CONST_SEMICOLON;
            Assert.Equal(expectedSession, result);
        }
        #endregion

        #region ResponseData Class Test Cases
        [Fact]
        public void ResponseData_Constructor_ErrorMessageAndStatusCode_SetsPropertiesCorrectly()
        {
            // Arrange
            string errorMessage = "Test Error";
            int statusCode = 500;

            // Act
            var response = new ResponseData(errorMessage, statusCode);

            // Assert
            Assert.False(response.isSuccess);
            Assert.Equal(errorMessage, response.errorMessage);
            Assert.Equal(statusCode, response.statusCode);
            Assert.Null(response.data);
        }

        [Fact]
        public void ResponseData_Constructor_Data_SetsPropertiesCorrectly()
        {
            // Arrange
            var testData = new { Id = 1, Name = "Test Data" };

            // Act
            var response = new ResponseData(testData);

            // Assert
            Assert.True(response.isSuccess);
            Assert.Equal(string.Empty, response.errorMessage);
            Assert.Equal(200, response.statusCode);
            Assert.Equal(testData, response.data);
        }
        #endregion
    }
}
