using Prana.BusinessObjects.AppConstants;
using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class LiveFeedPreferences
    {
        /// <summary>
        /// The selected feed price
        /// </summary>
        private SelectedFeedPrice _selectedFeedPrice = SelectedFeedPrice.Last;

        /// <summary>
        /// Gets or sets the selected feed price.
        /// </summary>
        /// <value>
        /// The selected feed price.
        /// </value>
        public SelectedFeedPrice SelectedFeedPrice
        {
            get { return _selectedFeedPrice; }
            set { _selectedFeedPrice = value; }
        }

        /// <summary>
        /// The option selected feed price
        /// </summary>
        private SelectedFeedPrice _optionSelectedFeedPrice = SelectedFeedPrice.Mid;

        /// <summary>
        /// Gets or sets the option selected feed price.
        /// </summary>
        /// <value>
        /// The option selected feed price.
        /// </value>
        public SelectedFeedPrice OptionSelectedFeedPrice
        {
            get { return _optionSelectedFeedPrice; }
            set { _optionSelectedFeedPrice = value; }
        }

        /// <summary>
        /// The override with options
        /// </summary>
        SelectedFeedPrice _overrideWithOptions = SelectedFeedPrice.Ask;

        /// <summary>
        /// Gets or sets the override with options.
        /// </summary>
        /// <value>
        /// The override with options.
        /// </value>
        public SelectedFeedPrice OverrideWithOptions
        {
            get { return _overrideWithOptions; }
            set { _overrideWithOptions = value; }
        }

        /// <summary>
        /// The override with others
        /// </summary>
        SelectedFeedPrice _overrideWithOthers = SelectedFeedPrice.Ask;

        /// <summary>
        /// Gets or sets the override with others.
        /// </summary>
        /// <value>
        /// The override with others.
        /// </value>
        public SelectedFeedPrice OverrideWithOthers
        {
            get { return _overrideWithOthers; }
            set { _overrideWithOthers = value; }
        }

        /// <summary>
        /// The use closing mark
        /// </summary>
        private bool _useClosingMark = false;

        /// <summary>
        /// Gets or sets a value indicating whether [use closing mark].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use closing mark]; otherwise, <c>false</c>.
        /// </value>
        public bool UseClosingMark
        {
            get { return _useClosingMark; }
            set
            {
                _useClosingMark = value;
            }
        }

        /// <summary>
        /// The use default delta
        /// </summary>
        private bool _useDefaultDelta = false;

        /// <summary>
        /// Gets or sets a value indicating whether [use default delta].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use default delta]; otherwise, <c>false</c>.
        /// </value>
        public bool UseDefaultDelta
        {
            get { return _useDefaultDelta; }
            set { _useDefaultDelta = value; }
        }

        /// <summary>
        /// The override check options
        /// </summary>
        SelectedFeedPrice _overrideCheckOptions = SelectedFeedPrice.Ask;

        /// <summary>
        /// Gets or sets the override check options.
        /// </summary>
        /// <value>
        /// The override check options.
        /// </value>
        public SelectedFeedPrice OverrideCheckOptions
        {
            get { return _overrideCheckOptions; }
            set { _overrideCheckOptions = value; }
        }

        /// <summary>
        /// The override check others
        /// </summary>
        SelectedFeedPrice _overrideCheckOthers = SelectedFeedPrice.Ask;

        /// <summary>
        /// Gets or sets the override check others.
        /// </summary>
        /// <value>
        /// The override check others.
        /// </value>
        public SelectedFeedPrice OverrideCheckOthers
        {
            get { return _overrideCheckOthers; }
            set { _overrideCheckOthers = value; }
        }

        /// <summary>
        /// The feed px condition options
        /// </summary>
        private NumericConditionOperator _overrideConditionOptions = NumericConditionOperator.Equal;

        /// <summary>
        /// Gets or sets the feed px condition options.
        /// </summary>
        /// <value>
        /// The feed px condition options.
        /// </value>
        public NumericConditionOperator OverrideConditionOptions
        {
            get { return _overrideConditionOptions; }
            set { _overrideConditionOptions = value; }
        }

        /// <summary>
        /// The feed px condition others
        /// </summary>
        private NumericConditionOperator _overrideConditionOthers = NumericConditionOperator.Equal;

        /// <summary>
        /// Gets or sets the feed px condition others.
        /// </summary>
        /// <value>
        /// The feed px condition others.
        /// </value>
        public NumericConditionOperator OverrideConditionOthers
        {
            get { return _overrideConditionOthers; }
            set { _overrideConditionOthers = value; }
        }

        /// <summary>
        /// The price bar options
        /// </summary>
        private decimal _priceBarOptions = 0.0M;

        /// <summary>
        /// Gets or sets the price bar options.
        /// </summary>
        /// <value>
        /// The price bar options.
        /// </value>
        public decimal PriceBarOptions
        {
            get { return _priceBarOptions; }
            set { _priceBarOptions = value; }
        }

        /// <summary>
        /// The price bar others
        /// </summary>
        private decimal _priceBarOthers = 0.0M;

        /// <summary>
        /// Gets or sets the price bar others.
        /// </summary>
        /// <value>
        /// The price bar others.
        /// </value>
        public decimal PriceBarOthers
        {
            get { return _priceBarOthers; }
            set { _priceBarOthers = value; }
        }
    }
}