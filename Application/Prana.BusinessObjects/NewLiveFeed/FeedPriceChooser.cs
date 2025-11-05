using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prana.BusinessObjects
{
    public class FeedPriceChooser
    {
        /// <summary>
        /// The selected feed price
        /// </summary>
        static SelectedFeedPrice _selectedFeedPrice = SelectedFeedPrice.Last;

        /// <summary>
        /// Gets or sets the selected feed price.
        /// </summary>
        /// <value>
        /// The selected feed price.
        /// </value>
        public static SelectedFeedPrice SelectedFeedPrice
        {
            get { return _selectedFeedPrice; }
            set { _selectedFeedPrice = value; }
        }

        /// <summary>
        /// The option selected feed price
        /// </summary>
        static SelectedFeedPrice _optionSelectedFeedPrice = SelectedFeedPrice.Mid;

        /// <summary>
        /// Gets or sets the option selected feed price.
        /// </summary>
        /// <value>
        /// The option selected feed price.
        /// </value>
        public static SelectedFeedPrice OptionSelectedFeedPrice
        {
            get { return _optionSelectedFeedPrice; }
            set { _optionSelectedFeedPrice = value; }
        }

        /// <summary>
        /// The use closing mark
        /// </summary>
        static bool _useClosingMark = false;

        /// <summary>
        /// Gets or sets a value indicating whether [use closing mark].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use closing mark]; otherwise, <c>false</c>.
        /// </value>
        public static bool UseClosingMark
        {
            get { return _useClosingMark; }
            set
            {
                _useClosingMark = value;
                //if (_useClosingMark)
                //{
                //    _selectedFeedPrice = SelectedFeedPrice.UserMark;
                //}
            }
        }

        /// <summary>
        /// The use default delta
        /// </summary>
        static bool _useDefaultDelta = false;

        /// <summary>
        /// Gets or sets a value indicating whether [use default delta].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use default delta]; otherwise, <c>false</c>.
        /// </value>
        public static bool UseDefaultDelta
        {
            get { return _useDefaultDelta; }
            set { _useDefaultDelta = value; }
        }

        /// <summary>
        /// The override with options
        /// </summary>
        static SelectedFeedPrice _overrideWithOptions = SelectedFeedPrice.Ask;

        /// <summary>
        /// Gets or sets the override with options.
        /// </summary>
        /// <value>
        /// The override with options.
        /// </value>
        static public SelectedFeedPrice OverrideWithOptions
        {
            get { return _overrideWithOptions; }
            set { _overrideWithOptions = value; }
        }

        /// <summary>
        /// The override with others
        /// </summary>
        static SelectedFeedPrice _overrideWithOthers = SelectedFeedPrice.Ask;

        /// <summary>
        /// Gets or sets the override with others.
        /// </summary>
        /// <value>
        /// The override with others.
        /// </value>
        static public SelectedFeedPrice OverrideWithOthers
        {
            get { return _overrideWithOthers; }
            set { _overrideWithOthers = value; }
        }

        /// <summary>
        /// The xpercent of average volume
        /// </summary>
        static double _xpercentOfAvgVolume = 100;

        /// <summary>
        /// Gets or sets the x percent of average volume.
        /// </summary>
        /// <value>
        /// The x percent of average volume.
        /// </value>
        public static double XPercentOfAvgVolume
        {
            get { return _xpercentOfAvgVolume; }
            set { _xpercentOfAvgVolume = value; }
        }

        /// <summary>
        /// The override check options
        /// </summary>
        static SelectedFeedPrice _overrideCheckOptions = SelectedFeedPrice.Ask;

        /// <summary>
        /// Gets or sets the override check options.
        /// </summary>
        /// <value>
        /// The override check options.
        /// </value>
        public static SelectedFeedPrice OverrideCheckOptions
        {
            get { return _overrideCheckOptions; }
            set { _overrideCheckOptions = value; }
        }

        /// <summary>
        /// The override check others
        /// </summary>
        static SelectedFeedPrice _overrideCheckOthers = SelectedFeedPrice.Ask;

        /// <summary>
        /// Gets or sets the override check others.
        /// </summary>
        /// <value>
        /// The override check others.
        /// </value>
        public static SelectedFeedPrice OverrideCheckOthers
        {
            get { return _overrideCheckOthers; }
            set { _overrideCheckOthers = value; }
        }

        /// <summary>
        /// The override condition options
        /// </summary>
        private static NumericConditionOperator _overrideConditionOptions = NumericConditionOperator.Equal;

        /// <summary>
        /// Gets or sets the override condition options.
        /// </summary>
        /// <value>
        /// The override condition options.
        /// </value>
        public static NumericConditionOperator OverrideConditionOptions
        {
            get { return _overrideConditionOptions; }
            set { _overrideConditionOptions = value; }
        }

        /// <summary>
        /// The override condition others
        /// </summary>
        private static NumericConditionOperator _overrideConditionOthers = NumericConditionOperator.Equal;

        /// <summary>
        /// Gets or sets the override condition others.
        /// </summary>
        /// <value>
        /// The override condition others.
        /// </value>
        public static NumericConditionOperator OverrideConditionOthers
        {
            get { return _overrideConditionOthers; }
            set { _overrideConditionOthers = value; }
        }

        /// <summary>
        /// The price bar options
        /// </summary>
        private static decimal _priceBarOptions = 0.0M;

        /// <summary>
        /// Gets or sets the price bar options.
        /// </summary>
        /// <value>
        /// The price bar options.
        /// </value>
        public static decimal PriceBarOptions
        {
            get { return _priceBarOptions; }
            set { _priceBarOptions = value; }
        }

        /// <summary>
        /// The price bar others
        /// </summary>
        private static decimal _priceBarOthers = 0.0M;

        /// <summary>
        /// Gets or sets the price bar others.
        /// </summary>
        /// <value>
        /// The price bar others.
        /// </value>
        public static decimal PriceBarOthers
        {
            get { return _priceBarOthers; }
            set { _priceBarOthers = value; }
        }

        /// <summary>
        /// Sets the selected feed price.
        /// </summary>
        /// <param name="symbolData">The symbol data.</param>
        public static void SetSelectedFeedPrice(ref SymbolData symbolData)
        {
            try
            {
                if (UseClosingMark)
                {
                    symbolData.SelectedFeedPrice = symbolData.MarkPrice;
                }
                else
                {
                    if (symbolData.CategoryCode == AssetCategory.EquityOption || symbolData.CategoryCode == AssetCategory.FutureOption)
                    {
                        SetSelectedFeedPriceForAsset(symbolData, FeedPriceChooser.OptionSelectedFeedPrice, FeedPriceChooser.OverrideWithOptions, FeedPriceChooser.OverrideCheckOptions, FeedPriceChooser.OverrideConditionOptions, FeedPriceChooser.PriceBarOptions);
                    }
                    else
                    {
                        SetSelectedFeedPriceForAsset(symbolData, FeedPriceChooser.SelectedFeedPrice, FeedPriceChooser.OverrideWithOthers, FeedPriceChooser.OverrideCheckOthers, FeedPriceChooser.OverrideConditionOthers, FeedPriceChooser.PriceBarOthers);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets the selected feed price for asset.
        /// </summary>
        /// <param name="symbolData">The symbol data.</param>
        /// <param name="selectedPrice">The selected price.</param>
        /// <param name="overrideWith">The override with.</param>
        /// <param name="overrideCheck">The override check.</param>
        /// <param name="overrideCondition">The override condition.</param>
        /// <param name="priceBar">The price bar.</param>
        private static void SetSelectedFeedPriceForAsset(SymbolData symbolData, SelectedFeedPrice selectedPrice, SelectedFeedPrice overrideWith, SelectedFeedPrice overrideCheck, NumericConditionOperator overrideCondition, decimal priceBar)
        {
            try
            {
                double priceToUpdate = 0.0;
                priceToUpdate = GetPriceBasedOnFeedChooser(symbolData, selectedPrice).FirstOrDefault();
                switch (overrideWith)
                {
                    case SelectedFeedPrice.Ask:
                    case SelectedFeedPrice.Bid:
                    case SelectedFeedPrice.Last:
                    case SelectedFeedPrice.Previous:
                    case SelectedFeedPrice.Mid:
                    case SelectedFeedPrice.iMid:
                        List<double> checkingPrice = GetPriceBasedOnFeedChooser(symbolData, overrideCheck);
                        bool isOverride = false;
                        switch (overrideCondition)
                        {
                            case NumericConditionOperator.Equal:
                                isOverride = checkingPrice.Any(x => x == Convert.ToDouble(priceBar));
                                break;
                            case NumericConditionOperator.GreaterThan:
                                isOverride = checkingPrice.Any(x => x > Convert.ToDouble(priceBar));
                                break;
                            case NumericConditionOperator.GreaterThanOrEqual:
                                isOverride = checkingPrice.Any(x => x >= Convert.ToDouble(priceBar));
                                break;
                            case NumericConditionOperator.LessThan:
                                isOverride = checkingPrice.Any(x => x < Convert.ToDouble(priceBar));
                                break;
                            case NumericConditionOperator.LessThanOrEqual:
                                isOverride = checkingPrice.Any(x => x <= Convert.ToDouble(priceBar));
                                break;
                        }
                        if (isOverride)
                            priceToUpdate = GetPriceBasedOnFeedChooser(symbolData, overrideWith).FirstOrDefault();
                        break;

                    case SelectedFeedPrice.None:
                        break;
                }
                symbolData.SelectedFeedPrice = priceToUpdate;
                symbolData.PreferencedPrice = priceToUpdate;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Gets the price based on feed chooser.
        /// </summary>
        /// <param name="symbolData">The symbol data.</param>
        /// <param name="feedPriceChooser">The feed price chooser.</param>
        /// <returns></returns>
        private static List<double> GetPriceBasedOnFeedChooser(SymbolData symbolData, SelectedFeedPrice feedPriceChooser)
        {
            List<double> priceToUpdate = new List<double>();
            try
            {
                switch (feedPriceChooser)
                {
                    case SelectedFeedPrice.Ask:
                        priceToUpdate.Add(symbolData.Ask);
                        break;

                    case SelectedFeedPrice.Bid:
                        priceToUpdate.Add(symbolData.Bid);
                        break;

                    case SelectedFeedPrice.Last:
                        priceToUpdate.Add(symbolData.LastPrice);
                        break;

                    case SelectedFeedPrice.Previous:
                        priceToUpdate.Add(symbolData.Previous);
                        break;

                    case SelectedFeedPrice.Mid:
                        priceToUpdate.Add(symbolData.Mid);
                        break;

                    case SelectedFeedPrice.iMid:
                        priceToUpdate.Add(symbolData.iMid);
                        break;

                    case SelectedFeedPrice.AskOrBid:
                        priceToUpdate.Add(symbolData.Ask);
                        priceToUpdate.Add(symbolData.Bid);
                        break;

                    default:
                        priceToUpdate.Add(symbolData.LastPrice);
                        break;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return priceToUpdate;
        }

        /// <summary>
        /// Sets the live feed preferences.
        /// </summary>
        /// <param name="Preferences">The preferences.</param>
        public static void SetLiveFeedPreferences(LiveFeedPreferences Preferences)
        {
            try
            {
                _selectedFeedPrice = Preferences.SelectedFeedPrice;
                _optionSelectedFeedPrice = Preferences.OptionSelectedFeedPrice;
                _overrideWithOthers = Preferences.OverrideWithOthers;
                _overrideWithOptions = Preferences.OverrideWithOptions;
                _useClosingMark = Preferences.UseClosingMark;
                _useDefaultDelta = Preferences.UseDefaultDelta;
                _priceBarOptions = Preferences.PriceBarOptions;
                _priceBarOthers = Preferences.PriceBarOthers;
                _overrideConditionOptions = Preferences.OverrideConditionOptions;
                _overrideConditionOthers = Preferences.OverrideConditionOthers;
                _overrideCheckOptions = Preferences.OverrideCheckOptions;
                _overrideCheckOthers = Preferences.OverrideCheckOthers;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}