using Prana.BusinessObjects.LiveFeed;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.Classes.LiveFeed
{
    public class LeafContainerTests
    {
        [Fact]
        [Trait("Prana.BusinessObjects", "LeafContainer")]
        public void ProcessMMID_ShouldInsertMarketMaker_WhenNotExists()
        {
            // Arrange
            var leafContainer = new LeafContainer("TestContainer");
            var marketMaker = new MarketMaker
            {
                Price = 100,
                Size = 50,
            };

            // Act
            leafContainer.ProcessMMID(marketMaker);

            // Assert
            Assert.Equal(1, leafContainer.DataCollection.Count);
            Assert.Equal(100, leafContainer.DataCollection[0].Price);
            Assert.Equal(50, leafContainer.DataCollection[0].Size);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "LeafContainer")]
        public void ProcessDataUsingIBindingList_UpdateFlag_U_UpdatesExistingItem()
        {
            // Arrange
            var leafContainer = new LeafContainer("TestContainer");
            var marketMaker = new MarketMaker
            {
                UpdateFlag = 'U',
                Price = 100,
                Size = 50,
            };
            leafContainer.ProcessMMID(marketMaker);

            // Act
            marketMaker.Price = 150;
            marketMaker.UpdateFlag = 'U';
            leafContainer.ProcessMMID(marketMaker);

            // Assert
            Assert.Equal(1, leafContainer.DataCollection.Count);
            Assert.Equal(150, leafContainer.DataCollection[0].Price);
            Assert.Equal(50, leafContainer.DataCollection[0].Size);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "LeafContainer")]
        public void ProcessDataUsingIBindingList_UpdateFlag_D_RemovesExistingItem()
        {
            // Arrange
            var mmidCollection = new MarketMakerCollection();
            var marketMaker = new MarketMaker
            {
                Price = 110,
                Size = 55,
                Time = "12:00:00",
                UpdateFlag = 'D'
            };
            var leafContainer = new LeafContainer("TestContainer", mmidCollection);
            mmidCollection.Insert(0, marketMaker);

            // Act
            leafContainer.ProcessMMID(marketMaker);

            // Assert
            Assert.Equal(0, leafContainer.DataCollection.Count);
            Assert.Empty(leafContainer);
        }
    }
}
