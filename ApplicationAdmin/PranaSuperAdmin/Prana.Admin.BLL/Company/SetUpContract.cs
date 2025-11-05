namespace Prana.Admin.BLL
{
    public class SetUpContract
    {
        #region Constructors

        public SetUpContract()
        {
        }

        public SetUpContract(string symbol, int auecID, double multiplier, int contractMonthID, string description)
        {
            _symbol = symbol;
            _auecID = auecID;
            _multiplier = multiplier;
            _contractMonthID = contractMonthID;
            _description = description;
        }

        #endregion

        #region Private members

        private string _symbol = string.Empty;
        private int _auecID = int.MinValue;
        private double _multiplier = double.MinValue;
        private string _contractMonthName = string.Empty;
        private int _contractMonthID = int.MinValue;
        private int _companyID = int.MinValue;
        private int _companySetUpContractID = int.MinValue;
        private string _description = string.Empty;

        #endregion

        #region Public Properties
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        public int AuecID
        {
            get { return _auecID; }
            set { _auecID = value; }
        }

        public double Multiplier
        {
            get { return _multiplier; }
            set { _multiplier = value; }
        }

        public string ContractMonthName
        {
            get { return _contractMonthName; }
            set { _contractMonthName = value; }
        }

        public int ContractMonthID
        {
            get { return _contractMonthID; }
            set { _contractMonthID = value; }
        }

        public int CompanyID
        {
            get { return _companyID; }
            set { _companyID = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public int CompanySetUpContractID
        {
            get { return _companySetUpContractID; }
            set { _companySetUpContractID = value; }
        }
        #endregion
    }
}
