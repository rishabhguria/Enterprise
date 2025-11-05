using Prana.BusinessObjects;
using System;
using Xunit;
using static Prana.BusinessObjects.AutomationEnum;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Risk_Management
{
    public class RiskPathCreatorTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "RiskPathCreator")]
        public void GetClientPath_ReturnsCorrectPath()
        {
            // Arrange
            var clientSettings = new ClientSettings
            {
                BaseSettings = new BaseSettings { FilePath = "C:\\Clients" },
                ClientName = "ClientA"
            };

            // Act
            var result = RiskPathCreator.GetClientPath(clientSettings);

            // Assert
            Assert.Equal("C:\\Clients\\ClientA", result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "RiskPathCreator")]
        public void GetThirdPartyPath_ReturnsCorrectPath()
        {
            // Arrange
            var clientSettings = new ClientSettings
            {
                BaseSettings = new BaseSettings { FilePath = "C:\\Clients" },
                ClientName = "ClientA"
            };
            string thirdPartyName = "ThirdPartyX";

            // Act
            var result = RiskPathCreator.GetThirdPartyPath(clientSettings, thirdPartyName);

            // Assert
            Assert.Equal("C:\\Clients\\ClientA\\ThirdPartyX", result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "RiskPathCreator")]
        public void GetRiskReportPath_ReturnsCorrectPath()
        {
            // Arrange
            var clientSettings = new ClientSettings
            {
                BaseSettings = new BaseSettings { FilePath = "C:\\Clients" },
                ClientName = "ClientA"
            };

            // Act
            var result = RiskPathCreator.GetRiskReportPath(clientSettings);

            // Assert
            Assert.Equal("C:\\Clients\\ClientA\\RiskReport", result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "RiskPathCreator")]
        public void GetDatePath_ReturnsCorrectPath()
        {
            // Arrange
            var clientSettings = new ClientSettings
            {
                BaseSettings = new BaseSettings { FilePath = "C:\\Clients" },
                ClientName = "ClientA",
                Date = new DateTime(2023, 12, 25)
            };
            string thirdPartyName = "ThirdPartyX";

            // Act
            var result = RiskPathCreator.GetDatePath(clientSettings, thirdPartyName);

            // Assert
            Assert.Equal("C:\\Clients\\ClientA\\ThirdPartyX\\25122023", result);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "RiskPathCreator")]
        public void GetRiskReportFileName_ReturnsCorrectFileName()
        {
            // Arrange
            var clientSettings = new ClientSettings
            {
                BaseSettings = new BaseSettings { FilePath = "C:\\Clients" },
                ClientName = "ClientA",
                ReportType = ReprotTypeEnum.Internal,
                Date = new DateTime(2023, 12, 25),
                FileFormatter = FileFormat.pdf,
            };

            // Act
            var result = RiskPathCreator.GetRiskReportFileName(clientSettings);

            // Assert
            Assert.Equal("C:\\Clients\\ClientA\\RiskReport\\Internal_25122023.pdf", result);
        }

    }
}

