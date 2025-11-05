using Csla;
using Prana.BusinessObjects;
using System;

namespace Prana.PostTrade
{
    [Serializable()]
    public class CloseTradePreferences : BusinessBase<CloseTradePreferences>
    {


        private DateTime _closeTradeDate;

        /// <summary>
        /// Gets or sets the date time value to Todate.
        /// </summary>
        /// <value>The date time value.</value>
        public DateTime CloseTradeDate
        {
            get { return _closeTradeDate; }
            set
            {
                _closeTradeDate = value;
                PropertyHasChanged();
            }
        }

        //Not assigned and not used so removing discussed with Narendra
        //private int _id;




        private DateTime _fromDate;

        /// <summary>
        /// Gets or sets the date time value.
        /// </summary>
        /// <value>The date time value.</value>
        public DateTime FromDate
        {
            get { return _fromDate; }
            set
            {
                _fromDate = value;
                PropertyHasChanged();
            }
        }
        private PostTradeEnums.CloseTradeMethodology _defaultMethodology;

        /// <summary>
        /// Gets or sets the default methodology.
        /// </summary>
        /// <value>The default methodology.</value>
        public PostTradeEnums.CloseTradeMethodology DefaultMethodology
        {
            get { return _defaultMethodology; }
            set
            {
                _defaultMethodology = value;
                PropertyHasChanged();
            }
        }

        private PostTradeEnums.CloseTradeAlogrithm _algorithm;

        /// <summary>
        /// Gets or sets the algorithm.
        /// </summary>
        /// <value>The algorithm.</value>
        public PostTradeEnums.CloseTradeAlogrithm Algorithm
        {
            get { return _algorithm; }
            set
            {
                _algorithm = value;
                PropertyHasChanged();
            }
        }

        private bool _isShortWithBuyAndBuyToCover = false;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is short with buy and buy to cover.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is short with buy and buy to cover; otherwise, <c>false</c>.
        /// </value>
        public bool IsShortWithBuyAndBuyToCover
        {
            get { return _isShortWithBuyAndBuyToCover; }
            set { _isShortWithBuyAndBuyToCover = value; }
        }

        private bool _isCurrentDateClosing = true;

        public bool IsCurrentDateClosing
        {
            get { return _isCurrentDateClosing; }
            set { _isCurrentDateClosing = value; }
        }

        //As discussed with narendra not used anywhere so returning new object.
        protected override object GetIdValue()
        {
            return new object();
        }

        private string _comments;

        /// <summary>
        /// Gets or sets the Comments value.
        /// </summary>
        /// <value>The Comments value.</value>
        public string Comments
        {
            get { return _comments; }
            set
            {
                _comments = value;
                PropertyHasChanged();
            }
        }
    }
}
