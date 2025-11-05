using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.UtilitiesTest.MiscUtilities
{
    public class DeliveryFactoryTests
    {
        [Fact]
        [Trait("Prana.Utilities", "DeliveryFactory")]
        public void GetInstance_Always_ReturnsSameInstance()
        {
            // Act
            var instance1 = DeliveryFactory.GetInstance();
            var instance2 = DeliveryFactory.GetInstance();

            // Assert
            Assert.Same(instance1, instance2);
        }

        [Fact]
        [Trait("Prana.Utilities", "DeliveryFactory")]
        public void GetDeliveryClass_WithFTP_ReturnsFTPDelivery()
        {
            // Arrange
            var factory = DeliveryFactory.GetInstance();

            // Act
            var delivery = factory.GetDeliveryClass("ftp");

            // Assert
            Assert.NotNull(delivery);
            Assert.IsType<FTPDelivery>(delivery);
        }

        [Fact]
        [Trait("Prana.Utilities", "DeliveryFactory")]
        public void GetDeliveryClass_WithMail_ReturnsMailDelivery()
        {
            // Arrange
            var factory = DeliveryFactory.GetInstance();

            // Act
            var delivery = factory.GetDeliveryClass("mail");

            // Assert
            Assert.NotNull(delivery);
            Assert.IsType<MailDelivery>(delivery);
        }

        [Fact]
        [Trait("Prana.Utilities", "DeliveryFactory")]
        public void GetDeliveryClass_WithUnknownType_ReturnsNull()
        {
            // Arrange
            var factory = DeliveryFactory.GetInstance();

            // Act
            var delivery = factory.GetDeliveryClass("unknown");

            // Assert
            Assert.Null(delivery);
        }
    }
}
