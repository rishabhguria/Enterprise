using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Prana.KafkaWrapper;
using Prana.KafkaWrapper.Contracts;
using Prana.ServiceGateway.Constants;
using Prana.ServiceGateway.Contracts;
using Prana.ServiceGateway.Controllers;
using Prana.ServiceGateway.Hubs;
using Prana.ServiceGateway.Services;
using System;

namespace Prana.ServiceGateway.UnitTest
{
    public class BaseControllerTest
    {
        protected readonly Mock<IValidationHelper> _mockValidationHelper;
        protected readonly Mock<ITokenService> _mockTokenService;
        protected readonly Mock<IKafkaManager> _mockKafkaManager;
        protected readonly Mock<IWatchlistDataService> _mockWatchlistDataService;
        protected readonly Mock<ILiveFeedService> _mockLiveFeedService;
        protected readonly Mock<IHubContext> _mockIHubContext;
        protected readonly Mock<IHubManager> _mockHubServiceGateway;
        protected readonly Mock<IHubManagerRTPNL> _mockHubServiceGatewayRTPNL;
        protected readonly Mock<IHubManagerRTPNLUpdates> _mockHubServiceGatewayRTPNLUpdates;
        protected readonly Mock<IBlotterService> _mockBlotterService;
        protected readonly Mock<IAuthenticateUserService> _mockAuthenticateUserService;
        protected readonly Mock<ICommonDataService> _mockCommonDataService;
        protected readonly Mock<ITradingService> _mockTradingService;
        protected readonly Mock<IComplianceService> _mockComplianceService;
        protected readonly Mock<ISecurityValidationService> _mockSecurityValidationService;
        protected readonly Mock<ILoggingHelper> _mockLoggingHelper;
        protected readonly Mock<ILayoutService> _mockLayoutService;
        protected readonly Mock<IRtpnlService> _mockRtpnlService;
        protected readonly Mock<IOpenfinManagerService> _mockOpenfinManagerService;

        protected readonly string CorrelationId = Guid.NewGuid().ToString();

        protected Mock<IConfiguration> _mockConfiguration;  //removed readonly, as derived class is initializing it

        public BaseControllerTest()
        {
            _mockValidationHelper = new Mock<IValidationHelper>();
            _mockTokenService = new Mock<ITokenService>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockKafkaManager = new Mock<IKafkaManager>();
            _mockWatchlistDataService = new Mock<IWatchlistDataService>();
            _mockLiveFeedService = new Mock<ILiveFeedService>();
            _mockIHubContext = new Mock<IHubContext>();
            _mockHubServiceGateway = new Mock<IHubManager>();
            _mockHubServiceGatewayRTPNL = new Mock<IHubManagerRTPNL>();
            _mockHubServiceGatewayRTPNLUpdates = new Mock<IHubManagerRTPNLUpdates>();
            _mockBlotterService = new Mock<IBlotterService>();
            _mockAuthenticateUserService = new Mock<IAuthenticateUserService>();
            _mockCommonDataService = new Mock<ICommonDataService>();
            _mockTradingService = new Mock<ITradingService>();
            _mockComplianceService = new Mock<IComplianceService>();
            _mockSecurityValidationService = new Mock<ISecurityValidationService>();
            _mockLoggingHelper = new Mock<ILoggingHelper>();
            _mockLayoutService = new Mock<ILayoutService>();
            _mockRtpnlService = new Mock<IRtpnlService>();
            _mockOpenfinManagerService = new Mock<IOpenfinManagerService>();
        }

        #region Setup MockKafkaManager
        public void SetKafkaMock(RequestResponseModel requestResponseObj, string expected, string produceTopic, string consumeTopic)
        {
            try
            {
                var consumerMock = new Mock<NirvanaConsumer>();
                _mockKafkaManager.Setup(x => x.ProduceAndConsume(It.IsAny<RequestResponseModel>(), produceTopic, consumeTopic, It.IsAny<bool>())).ReturnsAsync(expected);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        public void SetKafkaMock_ToThrowException(RequestResponseModel requestResponseObj, string consumeTopic, string produceTopic)
        {
            try
            {
                var consumerMock = new Mock<NirvanaConsumer>();
                _mockKafkaManager.Setup(x => x.Produce(It.IsAny<string>(), It.IsAny<RequestResponseModel>(), true)).Throws(new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED));
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        public void SetKafkaMock_ProduceThrowException()
        {
            try
            {
                _mockKafkaManager.Setup(x => x.Produce(It.IsAny<string>(), It.IsAny<RequestResponseModel>(), true)).Throws(new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED));
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }

        public void SetKafkaMock_ToThrowTestException(RequestResponseModel requestResponseObj, string consumeTopic, string produceTopic)
        {
            try
            {
                var consumerMock = new Mock<NirvanaConsumer>();
                _mockKafkaManager.Setup(x => x.ProduceAndConsume(It.IsAny<RequestResponseModel>(), produceTopic, consumeTopic, It.IsAny<bool>())).ThrowsAsync(new Exception("Test exception"));
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion


        protected ILogger<T> GetMockLogger<T>()
        {
            return new Mock<ILogger<T>>().Object;
        }

        protected Mock<ILogger<T>> CreateMockLogger<T>()
        {
            return new Mock<ILogger<T>>();
        }
    }
}
