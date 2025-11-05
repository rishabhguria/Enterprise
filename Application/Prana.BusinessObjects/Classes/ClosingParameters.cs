using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    public class ClosingParameters
    {

        public List<TaxLot> BuyTaxLotsAndPositions { get; set; }

        public List<TaxLot> SellTaxLotsAndPositions { get; set; }

        public PostTradeEnums.CloseTradeAlogrithm Algorithm { get; set; }

        public bool IsShortWithBuyAndBuyToCover { get; set; }

        public bool IsSellWithBuyToClose { get; set; }

        public bool IsManual { get; set; }

        public bool IsDragDrop { get; set; }

        public bool IsFromServer { get; set; }

        public PostTradeEnums.SecondarySortCriteria SecondarySort { get; set; }

        public bool IsVirtualClosingPopulate { get; set; }

        public bool IsOverrideWithUserClosing { get; set; }

        public bool IsMatchStrategy { get; set; }


        public List<string> VirtualUnwidedTaxlots { get; set; }

        public PostTradeEnums.ClosingField ClosingField { get; set; }

        public bool IsCopyOpeningTradeAttributes { get; set; }
    }
}
