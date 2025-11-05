using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Xunit;

namespace Prana.Common.UnitTesting.Prana.BusinessObjectsTest.NewLiveFeed
{
    public class FeedPriceChooserTests
    {
        public FeedPriceChooserTests()
        {
            FeedPriceChooser.SelectedFeedPrice = SelectedFeedPrice.Last;
            FeedPriceChooser.OptionSelectedFeedPrice = SelectedFeedPrice.Mid;
            FeedPriceChooser.UseClosingMark = false;
            FeedPriceChooser.UseDefaultDelta = false;
            FeedPriceChooser.OverrideWithOptions = SelectedFeedPrice.Ask;
            FeedPriceChooser.OverrideWithOthers = SelectedFeedPrice.Ask;
            FeedPriceChooser.XPercentOfAvgVolume = 100;
            FeedPriceChooser.OverrideCheckOptions = SelectedFeedPrice.Ask;
            FeedPriceChooser.OverrideCheckOthers = SelectedFeedPrice.Ask;
            FeedPriceChooser.OverrideConditionOptions = NumericConditionOperator.Equal;
            FeedPriceChooser.OverrideConditionOthers = NumericConditionOperator.Equal;
            FeedPriceChooser.PriceBarOptions = 0.0M;
            FeedPriceChooser.PriceBarOthers = 0.0M;
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "FeedPriceChooser")]
        public void SetSelectedFeedPrice_ShouldUseMarkPriceWhenUseClosingMarkIsTrue()
        {
            // Arrange
            SymbolData symbolData = new SymbolData
            {
                MarkPrice = 120.0,
                CategoryCode = AssetCategory.Equity
            };
            FeedPriceChooser.UseClosingMark = true;

            // Act
            FeedPriceChooser.SetSelectedFeedPrice(ref symbolData);

            // Assert
            Assert.Equal(120.0, symbolData.SelectedFeedPrice);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "FeedPriceChooser")]
        public void SetSelectedFeedPrice_ShouldUseOptionSelectedFeedPriceForEquityOption()
        {
            // Arrange
            SymbolData symbolData = new SymbolData
            {
                Ask = 100.0,
                LastPrice = 100.0,
                CategoryCode = AssetCategory.EquityOption,
            };

            // Act
            FeedPriceChooser.SetSelectedFeedPrice(ref symbolData);//here mid price returns

            // Assert
            Assert.Equal(50, symbolData.SelectedFeedPrice);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "FeedPriceChooser")]
        public void SetSelectedFeedPrice_ShouldUseBidPriceWhenOverrideConditionIsMet()
        {
            // Arrange
            SymbolData symbolData = new SymbolData
            {
                Ask = 150.0,
                Bid = 140.0,
                LastPrice = 100.0,
                CategoryCode = AssetCategory.Equity,
                Mid = 120.0
            };

            FeedPriceChooser.OverrideConditionOthers = NumericConditionOperator.GreaterThan;
            FeedPriceChooser.PriceBarOthers = 130.0M;
            FeedPriceChooser.OverrideWithOthers = SelectedFeedPrice.Bid;

            // Act
            FeedPriceChooser.SetSelectedFeedPrice(ref symbolData);

            // Assert
            Assert.Equal(140.0, symbolData.SelectedFeedPrice);
        }

        [Fact]
        [Trait("Prana.BusinessObjects", "FeedPriceChooser")]
        public void SetSelectedFeedPrice_ShouldNotOverrideIfConditionIsNotMet()
        {
            // Arrange
            SymbolData symbolData = new SymbolData
            {
                Ask = 150.0,
                Bid = 140.0,
                LastPrice = 100.0,
                CategoryCode = AssetCategory.Equity,
                Mid = 120.0
            };

            FeedPriceChooser.OverrideConditionOthers = NumericConditionOperator.LessThan;
            FeedPriceChooser.PriceBarOthers = 130.0M; 
            FeedPriceChooser.OverrideWithOthers = SelectedFeedPrice.Bid;

            // Act
            FeedPriceChooser.SetSelectedFeedPrice(ref symbolData);

            // Assert
            Assert.Equal(100.0, symbolData.SelectedFeedPrice);
        }
        
        [Fact]
        [Trait("Prana.BusinessObjects", "FeedPriceChooser")]
        public void SetLiveFeedPreferences_ShouldUpdateFeedChooserState()
        {
            // Arrange
            LiveFeedPreferences preferences = new LiveFeedPreferences
            {
                SelectedFeedPrice = SelectedFeedPrice.Bid,
                OptionSelectedFeedPrice = SelectedFeedPrice.Mid,
                OverrideWithOthers = SelectedFeedPrice.Last,
                OverrideWithOptions = SelectedFeedPrice.Ask,
                UseClosingMark = true,
                UseDefaultDelta = true,
                PriceBarOptions = 1.0M,
                PriceBarOthers = 2.0M,
                OverrideConditionOptions = NumericConditionOperator.GreaterThan,
                OverrideConditionOthers = NumericConditionOperator.LessThan,
                OverrideCheckOptions = SelectedFeedPrice.Previous,
                OverrideCheckOthers = SelectedFeedPrice.Previous
            };

            // Act
            FeedPriceChooser.SetLiveFeedPreferences(preferences);

            // Assert
            Assert.Equal(SelectedFeedPrice.Bid, FeedPriceChooser.SelectedFeedPrice);
            Assert.Equal(SelectedFeedPrice.Mid, FeedPriceChooser.OptionSelectedFeedPrice);
            Assert.True(FeedPriceChooser.UseClosingMark);
            Assert.True(FeedPriceChooser.UseDefaultDelta);
            Assert.Equal(1.0M, FeedPriceChooser.PriceBarOptions);
            Assert.Equal(2.0M, FeedPriceChooser.PriceBarOthers);
            Assert.Equal(NumericConditionOperator.GreaterThan, FeedPriceChooser.OverrideConditionOptions);
            Assert.Equal(NumericConditionOperator.LessThan, FeedPriceChooser.OverrideConditionOthers);
        }
    }
}
