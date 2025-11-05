using Prana.KafkaWrapper;
using Prana.KafkaWrapper.Extension.Classes;
using Prana.ServiceGateway.Constants;
using Prana.ServiceGateway.Contracts;
using Prana.ServiceGateway.Services;
using Prana.ServiceGateway.UnitTest.Commons;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Prana.ServiceGateway.UnitTest.ServiceTests
{
    public class OpenfinManagerServiceTest : BaseControllerTest
    {
        private readonly IOpenfinManagerService _openfinManagerService;
        public OpenfinManagerServiceTest() : base()
        {
            _openfinManagerService = new OpenfinManagerService(_mockKafkaManager.Object, GetMockLogger<OpenfinManagerService>(), _mockHubServiceGateway.Object, _mockHubServiceGatewayRTPNL.Object);
        }

        #region GetOpenfinWorkspaceLayout Test Cases
        [Theory]
        [InlineData(1)]
        public async Task GetOpenfinWorkspaceLayout_ReturnsString(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock(requestResponseObj, UnitTestConstants.CONST_EXPECTED_MOCK_MESSAGE, KafkaConstants.TOPIC_GetOpenfinWorkspaceLayoutRequest, KafkaConstants.TOPIC_GetOpenfinWorkspaceLayoutResponse);

                // Act
                await _openfinManagerService.GetOpenfinWorkspaceLayout(requestResponseObj);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion

        #region SaveOpenfinWorkspaceLayout Test Cases
        [Theory]
        [InlineData(1)]
        public async Task SaveOpenfinWorkspaceLayout_ReturnsString(int companyUserID)
        {
            try
            {
                // Arrange
                var requestResponseObj = new RequestResponseModel(companyUserID, UnitTestConstants.CONST_REQUEST_DATA, CorrelationId);
                SetKafkaMock(requestResponseObj, UnitTestConstants.CONST_EXPECTED_MOCK_MESSAGE, KafkaConstants.TOPIC_SaveOpenfinWorkspaceLayoutRequest, KafkaConstants.TOPIC_SaveOpenfinWorkspaceLayoutResponse);

                // Act
                await _openfinManagerService.SaveOpenfinWorkspaceLayout(requestResponseObj);
            }
            catch (Exception ex)
            {
                throw new Exception(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message, ex);
            }
        }
        #endregion
    }
}
