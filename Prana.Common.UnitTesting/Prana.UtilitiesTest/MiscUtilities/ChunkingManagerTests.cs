using Prana.BusinessObjects;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.UtilitiesTest.MiscUtilities
{
    public class ChunkingManagerTests
    {
        [Fact]
        [Trait("Prana.Utilities", "ChunkingManager")]
        public void CreateChunksForClosing_WithTaxLotWithDifferentChunkSize_CreatesCorrectChunks()
        {
            // Arrange
            List<TaxLot> taxLots = new List<TaxLot>
            {
                new TaxLot { TaxLotClosingId = "1" },
                new TaxLot { TaxLotClosingId = "2" },
                new TaxLot { TaxLotClosingId = "3" },
                new TaxLot { TaxLotClosingId = "4" },
                new TaxLot { TaxLotClosingId = "5" }
            };
            int chunkSize = 2;

            // Act
            //Error - Array dimensions exceeded supported range.
            var chunks = ChunkingManager.CreateChunksForClosing<TaxLot>(taxLots, chunkSize);

            // Assert
            // Expecting 5 chunks based on the TaxLotClosingId grouping
            Assert.Equal(5, chunks.Count);
        }

        [Fact]
        [Trait("Prana.Utilities", "ChunkingManager")]
        public void CreateChunksForClosing_WithTaxLotAsChunkSize_CreatesCorrectChunks()
        {
            // Arrange
            List<TaxLot> taxLots = new List<TaxLot>
            {
                new TaxLot { TaxLotClosingId = "1" },
                new TaxLot { TaxLotClosingId = "1" },
                new TaxLot { TaxLotClosingId = "2" },
                new TaxLot { TaxLotClosingId = "2" },
                new TaxLot { TaxLotClosingId = "3" }
            };
            int chunkSize = 2;

            // Act
            var chunks = ChunkingManager.CreateChunksForClosing<TaxLot>(taxLots, chunkSize);

            // Assert
            // Expecting 3 chunks based on the TaxLotClosingId grouping
            Assert.Equal(3, chunks.Count);
        }

        [Fact]
        [Trait("Prana.Utilities", "ChunkingManager")]
        public void CreateChunksForClosing_WithInteger_CreatesCorrectChunks()
        {
            // Arrange
            List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            int chunkSize = 3;

            // Act
            var chunks = ChunkingManager.CreateChunksForClosing<int>(numbers, chunkSize);

            // Assert
            // Expecting 4 chunks
            Assert.Equal(4, chunks.Count);
            // First 3 chunks should have 3 elements each
            Assert.Equal(3, chunks[0].Count);
            // Last chunk should have 1 element
            Assert.Single(chunks[3]);
        }

        [Fact]
        [Trait("Prana.Utilities", "ChunkingManager")]
        public void CreateChunks_WithIntegers_CreatesCorrectChunks()
        {
            // Arrange
            List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            int chunkSize = 3;

            // Act
            var chunks = ChunkingManager.CreateChunks<int>(numbers, chunkSize);

            // Assert
            // Expecting 4 chunks
            Assert.Equal(4, chunks.Count);
            // First 3 chunks should have 3 elements each
            Assert.Equal(3, chunks[0].Count);
            // Last chunk should have 1 element
            Assert.Single(chunks[3]); 
        }

        [Fact]
        [Trait("Prana.Utilities", "ChunkingManager")]
        public void CreateChunks_WithEmptyCollection_ReturnsEmptyList()
        {
            // Arrange
            List<int> emptyList = new List<int>();
            int chunkSize = 3;

            // Act
            var chunks = ChunkingManager.CreateChunks<int>(emptyList, chunkSize);

            // Assert
            Assert.Empty(chunks);
        }

        [Fact]
        [Trait("Prana.Utilities", "ChunkingManager")]
        public void CreateChunks_WithChunkSizeLargerThanCollection_ReturnsSingleChunk()
        {
            // Arrange
            List<int> numbers = new List<int> { 1, 2, 3 };
            int chunkSize = 5;

            // Act
            var chunks = ChunkingManager.CreateChunks<int>(numbers, chunkSize);

            // Assert
            // Only one chunk should be created
            Assert.Single(chunks);
            // The chunk should contain all elements
            Assert.Equal(3, chunks[0].Count); 
        }
    }
}
