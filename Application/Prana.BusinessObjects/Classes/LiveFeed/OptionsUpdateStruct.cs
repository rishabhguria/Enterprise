using System;

namespace Prana.BusinessObjects.LiveFeed
{
    /// <summary>
    /// We save the updates coming through the InernationLong event for the options. We save and forward
    /// this structure to the UI
    /// </summary>
    [Serializable]
    public class OptionsUpdateStruct
    {
        public string Symbol;
        public double BidPrice;
        public long BidSize;
        public string ExchangeBid;

        public double AskPrice;
        public long AskSize;
        public string ExchangeAsk;

        public string ExchangeListed;
        public double Change;
        public double Last;
        public double High;
        public double Low;
        public long Volume; // (Cumulative)
        public double Open;
        public double Previous; //Same as close
        public DateTime UpdateTime; //Same as close
        public long OpenInterest;
        // added by Sandeep 17/07/07
        //public string AssetCategoryCode;
        public AppConstants.AssetCategory AssetCategoryCode = AppConstants.AssetCategory.None;
    }
}
