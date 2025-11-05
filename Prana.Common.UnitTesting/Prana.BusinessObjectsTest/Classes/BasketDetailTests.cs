using Xunit;
using Prana.BusinessObjects;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes
{
    public class BasketDetailTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "BasketDetail")]
        public void AddWave_ShouldAddWaveToCollection()
        {
            // Arrange
            var basketDetail = new BasketDetail();
            var wave = new Wave { WaveID = "wave1", Percentage = 50 };

            // Act
            basketDetail.AddWave(wave);

            // Assert
            Assert.Equal(wave, basketDetail.GetWave("wave1"));
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "BasketDetail")]
        public void AddWaves_ShouldAddMultipleWaves()
        {
            // Arrange
            var basketDetail = new BasketDetail();
            var waves = new Waves
            {
                new Wave { WaveID = "wave1", Percentage = 50 },
                new Wave { WaveID = "wave2", Percentage = 50 }
            };

            // Act
            basketDetail.AddWaves(waves);

            // Assert
            Assert.NotNull(basketDetail.GetWave("wave1"));
            Assert.NotNull(basketDetail.GetWave("wave2"));
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "BasketDetail")]
        public void FillOrdersInWave_WithSubOrdersCreation_CreatesExpectedOrders()
        {
            // Arrange
            var basketDetail = new BasketDetail();

            var waves = new Waves();
            var wave1 = new Wave { WaveID = "1", Percentage = 40 };
            var wave2 = new Wave { WaveID = "2", Percentage = 60 };
            waves.Add(wave1);
            waves.Add(wave2);

            var order = new Order { Quantity = 100, Price = 10, UnsentQty = 100 };

            basketDetail.BasketOrders.Add(order);

            // Act
            basketDetail.FillOrdersInWave(waves);

            // Assert
            Assert.Equal(40, wave1.WaveOrders[0].Quantity);
            Assert.Equal(60, wave2.WaveOrders[0].Quantity);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "BasketDetail")]
        public void FillOrdersInWave_NoSubOrdersCreated_WhenCannotCreateSubOrders()
        {
            // Arrange
            var basketDetail = new BasketDetail();

            var waves = new Waves();
            var wave1 = new Wave { WaveID = "1", Percentage = 50 };
            var wave2 = new Wave { WaveID = "2", Percentage = 50 };
            waves.Add(wave1);
            waves.Add(wave2);

            // Not setting the unsent quantity which would set it to default 0
            var order = new Order { Quantity = 100, Price = 10 };

            basketDetail.BasketOrders.Add(order);

            // Act
            basketDetail.FillOrdersInWave(waves);

            // Assert
            Assert.Empty(wave1.WaveOrders);
            Assert.Empty(wave2.WaveOrders);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "BasketDetail")]
        public void FillOrdersInWave_SubOrdersHaveCorrectParentAndClientOrderID()
        {
            // Arrange
            var basketDetail = new BasketDetail();
            var waves = new Waves();
            var wave = new Wave { WaveID = "1", Percentage = 100 };
            waves.Add(wave);

            var order = new Order { Quantity = 100, Price = 10, UnsentQty = 100 };
            order.ClientOrderID = "ParentOrder123";

            basketDetail.BasketOrders.Add(order);

            // Act
            basketDetail.FillOrdersInWave(waves);

            // Assert
            var subOrder = wave.WaveOrders[0];
            Assert.Equal(order, subOrder.Parent);
            Assert.NotNull(subOrder.ClientOrderID);
            Assert.Equal("ParentOrder123", subOrder.ParentClientOrderID);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "BasketDetail")]
        public void GetOrder_ShouldReturnCorrectOrder()
        {
            // Arrange
            var basketDetail = new BasketDetail();
            var order = new Order { ClientOrderID = "order1", Quantity = 10, Price = 20 };
            basketDetail.BasketOrders.Add(order);

            // Act
            var retrievedOrder = basketDetail.GetOrder("order1");

            // Assert
            Assert.Equal(order, retrievedOrder);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "BasketDetail")]
        public void Clone_ShouldCreateDeepCopyOfBasketDetail()
        {
            // Arrange
            var basketDetail = new BasketDetail();
            basketDetail.BasketName = "Test Basket";
            basketDetail.AssetID = 1;
            var order = new Order { ClientOrderID = "order1", Quantity = 10, Price = 20 };
            basketDetail.BasketOrders.Add(order);

            // Act
            var clonedBasket = basketDetail.Clone();

            // Assert
            Assert.NotEqual(basketDetail, clonedBasket);
            Assert.Equal(basketDetail.BasketName, clonedBasket.BasketName);
            Assert.Equal(basketDetail.AssetID, clonedBasket.AssetID);
            Assert.NotEqual(order, clonedBasket.BasketOrders[0]);
        }
    }
}
