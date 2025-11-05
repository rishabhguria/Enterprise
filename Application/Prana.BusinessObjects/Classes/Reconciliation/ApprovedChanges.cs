using Prana.BusinessObjects.AppConstants;


namespace Prana.BusinessObjects
{
    public class ApprovedChanges
    {

        private string _taxlotID;

        public string TaxlotID
        {
            get { return _taxlotID; }
            set { _taxlotID = value; }
        }

        private AmendedTaxLotStatus _taxlotStatus;

        public AmendedTaxLotStatus TaxlotStatus
        {
            get { return _taxlotStatus; }
            set { _taxlotStatus = value; }
        }

        private string _columnName;

        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }


        private string _oldValue;

        public string OldValue
        {
            get { return _oldValue; }
            set { _oldValue = value; }
        }

        private string _newValue;

        public string NewValue
        {
            get { return _newValue; }
            set { _newValue = value; }
        }

    }
}
