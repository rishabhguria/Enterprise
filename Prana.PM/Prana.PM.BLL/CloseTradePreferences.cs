//using System;
//using System.Collections.Generic;
//using System.Text;

//using System.ComponentModel;

//using Csla;
//using Csla.Validation;
//using Prana.BusinessObjects.PositionManagement;

//namespace Prana.PM.BLL
//{
//    [Serializable()]
//    public class CloseTradePreferences:BusinessBase<CloseTradePreferences>
//    {


//        private DateTime _closeTradeDate;

//        /// <summary>
//        /// Gets or sets the date time value to Todate.
//        /// </summary>
//        /// <value>The date time value.</value>
//        public DateTime CloseTradeDate
//        {
//            get { return _closeTradeDate; }
//            set
//            {
//                _closeTradeDate = value;
//                PropertyHasChanged();
//            }
//        }

//        private int _id;




//        private DateTime _fromDate;

//        /// <summary>
//        /// Gets or sets the date time value.
//        /// </summary>
//        /// <value>The date time value.</value>
//        public DateTime FromDate
//        {
//            get { return _fromDate; }
//            set
//            {
//                _fromDate = value;
//                PropertyHasChanged();
//            }
//        }
//        private CloseTradeMethodology _defaultMethodology;

//        /// <summary>
//        /// Gets or sets the default methodology.
//        /// </summary>
//        /// <value>The default methodology.</value>
//        public CloseTradeMethodology DefaultMethodology
//        {
//            get { return _defaultMethodology; }
//            set
//            {
//                _defaultMethodology = value;
//                PropertyHasChanged();
//            }
//        }

//        private CloseTradeAlogrithm _algorithm;

//        /// <summary>
//        /// Gets or sets the algorithm.
//        /// </summary>
//        /// <value>The algorithm.</value>
//        public CloseTradeAlogrithm Algorithm
//        {
//            get { return _algorithm; }
//            set
//            {
//                _algorithm = value;
//                PropertyHasChanged();
//            }
//        }

//        private bool _isShortWithBuyAndBuyToCover;

//        /// <summary>
//        /// Gets or sets a value indicating whether this instance is short with buy and buy to cover.
//        /// </summary>
//        /// <value>
//        /// 	<c>true</c> if this instance is short with buy and buy to cover; otherwise, <c>false</c>.
//        /// </value>
//        public bool IsShortWithBuyAndBuyToCover
//        {
//            get { return _isShortWithBuyAndBuyToCover; }
//            set { _isShortWithBuyAndBuyToCover = value; }
//        }

//        private bool _isCurrentDateClosing = true;

//        public bool IsCurrentDateClosing
//        {
//            get { return _isCurrentDateClosing; }
//            set { _isCurrentDateClosing = value; }
//        }


//        protected override object GetIdValue()
//        {
//            return _id;
//        }

//        private string _comments;

//        /// <summary>
//        /// Gets or sets the Comments value.
//        /// </summary>
//        /// <value>The Comments value.</value>
//        public string Comments
//        {
//            get { return _comments; }
//            set
//            {
//                _comments = value;
//                PropertyHasChanged();
//            }
//        }
//    }
//}
