namespace Prana.BusinessObjects
{
    public class ClosingInfo
    {
        private string _closingID;

        public string ClosingID
        {
            get { return _closingID; }
            set { _closingID = value; }
        }

        private string _positionalTaxlotID;

        public string PositionalTaxlotID
        {
            get { return _positionalTaxlotID; }
            set { _positionalTaxlotID = value; }
        }

        private string _closingTaxlotID;

        public string ClosingTaxlotID
        {
            get { return _closingTaxlotID; }
            set { _closingTaxlotID = value; }
        }


        // Added By: Ankit Gupta
        // On May 30, 2014 
        // For Bulk Unwinding from CA
        private string _closingTradeDate;

        public string ClosingTradeDate
        {
            get { return _closingTradeDate; }
            set { _closingTradeDate = value; }
        }

    }
}
