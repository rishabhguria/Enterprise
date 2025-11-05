using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using System;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.SecurityMasterBusinessObjects
{
    public class SecMasterFutObjTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "SecMasterFutObj")]
        public void UpDateData_UpdatesPropertiesCorrectly()
        {
            // Arrange
            var secMasterFutObj = new SecMasterFutObj
            {
                Multiplier = 10,
                ExpirationDate = new DateTime(2023, 12, 31),
                LongName = "Test Future",
                IsCurrencyFuture = true
            };
            var secMasterFutObj1 = new SecMasterFutObj();

            // Act
            secMasterFutObj1.UpDateData(secMasterFutObj);

            // Assert
            Assert.Equal(10, secMasterFutObj1.Multiplier);
            Assert.Equal(new DateTime(2023, 12, 31), secMasterFutObj1.ExpirationDate);
            Assert.Equal(202312, secMasterFutObj1.MaturityMonth);
            Assert.Equal("Test Future", secMasterFutObj1.LongName);
            Assert.True(secMasterFutObj1.IsCurrencyFuture);
        }     
    }
}

