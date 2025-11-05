using Prana.BusinessObjects;
using Prana.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.UtilitiesTest.MiscUtilities
{
    public class PranaExtensionHelperTests
    {
        [Fact]
        [Trait("Prana.Utilities", "PranaExtensionHelper")]
        public void AddThreadSafely_AddsItemToCollection()
        {
            // Arrange
            var collection = new List<int>();
            int itemToAdd = 1;

            // Act
            // Simulate concurrent access
            Parallel.Invoke(
                () => collection.AddThreadSafely(itemToAdd),
                () => collection.AddThreadSafely(itemToAdd));

            // Assert
            Assert.Equal(2, collection.Count);
            Assert.Contains(itemToAdd, collection);
        }

        [Fact]
        [Trait("Prana.Utilities", "PranaExtensionHelper")]
        public void AddRangeThreadSafely_AddsItemsToCollection()
        {
            // Arrange
            var collection = new List<int>();
            var itemsToAdd = new List<int> { 1, 2, 3 };

            // Act
            // Simulate concurrent access
            Parallel.Invoke(
                () => collection.AddRangeThreadSafely(itemsToAdd),
                () => collection.AddRangeThreadSafely(itemsToAdd));

            // Assert
            Assert.Equal(6, collection.Count);
            foreach (var item in itemsToAdd)
            {
                Assert.Contains(item, collection);
            }
        }

        [Fact]
        [Trait("Prana.Utilities", "PranaExtensionHelper")]
        public void ClearThreadSafely_ClearsCollection()
        {
            // Arrange
            var collection = new List<int> { 1, 2, 3 };

            // Act
            // Simulate concurrent access
            Parallel.Invoke(
                () => collection.ClearThreadSafely(),
                () => collection.ClearThreadSafely());

            // Assert
            Assert.Empty(collection);
        }

        [Fact]
        [Trait("Prana.Utilities", "PranaExtensionHelper")]
        public void Clone_CreatesDeepCopyOfList()
        {
            // Arrange
            var listToClone = new List<TaxLot>
            {
                new TaxLot { CumQty = 50}, /* Initialize properties as needed */ 
                new TaxLot { Symbol = "AAPL" }
            };

            // Act
            var clonedList = listToClone.Clone();

            // Assert
            Assert.NotSame(listToClone, clonedList);
            Assert.Equal(listToClone.Count, clonedList.Count);
            for (int i = 0; i < listToClone.Count; i++)
            {
                // Further assert that the properties are equal if something we need to check
                Assert.NotSame(listToClone[i], clonedList[i]);
                Assert.Equal(listToClone[i].CumQty, clonedList[i].CumQty);
                Assert.Equal(listToClone[i].Symbol, clonedList[i].Symbol);
            }
        }
    }
}
