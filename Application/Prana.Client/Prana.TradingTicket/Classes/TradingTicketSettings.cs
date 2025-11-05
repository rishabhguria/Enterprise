namespace Prana.TradingTicket
{
    /// <summary>
    /// Summary description for TradingTicket.
    /// </summary>
    public class TradingTicketSettings
    {
        #region Constructor

        public TradingTicketSettings()
        {
        }

        #endregion

        #region Private members
        private int _companyUserID = int.MinValue;
        private string _ticketSettingsID = string.Empty;
        private int _auecID = int.MinValue;
        private string _buttonPosition = string.Empty;

        private string _name = string.Empty;
        private string _description = string.Empty;
        private string _buttonColor = string.Empty;
        private bool _isHotButton = true;

        private int _assetID = int.MinValue;
        private int _underlyingID = int.MinValue;
        private int _counterpartyID = int.MinValue;
        private int _venueID = int.MinValue;
        private string _sideID = string.Empty;
        private int _quantity = int.MinValue;
        private string _orderTypeID = string.Empty;
        private int _limitType = int.MinValue;    //Ask,Bid or Last
        private double _limitOffset = double.MinValue;

        private int _tif = int.MinValue;
        private string _handlingInstructionID = string.Empty;
        private string _executionInstructionID = string.Empty;
        private int _tradingAccountID = int.MinValue;
        private int _accountID = int.MinValue;
        private int _strategyID = int.MinValue;

        private double _peg = double.MinValue;
        private double _discreationOffset = double.MinValue;
        private int _displayQuantity = int.MinValue;
        private int _random = int.MinValue;
        private int _pnp = int.MinValue;

        //private int _clientTraderID = int.MinValue;
        //private int _clientAccountID = int.MinValue;
        //private int _principal = int.MinValue;
        //private int _shortExmpt = int.MinValue;
        //private int _clearingFirmID = int.MinValue;


        #endregion

        #region Properties
        public int CompanyUserID
        {
            get { return _companyUserID; }
            set { _companyUserID = value; }
        }
        public string TicketSettingsID
        {
            get { return _ticketSettingsID; }
            set { _ticketSettingsID = value; }
        }
        public int AUECID
        {
            get { return _auecID; }
            set { _auecID = value; }
        }
        public string ButtonPosition
        {
            get { return _buttonPosition; }
            set { _buttonPosition = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        public string ButtonColor
        {
            get { return _buttonColor; }
            set { _buttonColor = value; }
        }
        public bool IsHotButton
        {
            get { return _isHotButton; }
            set { _isHotButton = value; }
        }

        public int AssetID
        {
            get { return _assetID; }
            set { _assetID = value; }
        }
        public int UnderLyingID
        {
            get { return _underlyingID; }
            set { _underlyingID = value; }
        }
        public int CounterpartyID
        {
            get { return _counterpartyID; }
            set { _counterpartyID = value; }
        }
        public int VenueID
        {
            get { return _venueID; }
            set { _venueID = value; }
        }

        public string SideID
        {
            get { return _sideID; }
            set { _sideID = value; }
        }
        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }
        public string OrderTypeID
        {
            get { return _orderTypeID; }
            set { _orderTypeID = value; }
        }
        public int LimitType
        {
            get { return _limitType; }
            set { _limitType = value; }
        }
        public double LimitOffset
        {
            get { return _limitOffset; }
            set { _limitOffset = value; }
        }

        public int TIF
        {
            get { return _tif; }
            set { _tif = value; }
        }
        public string HandlingInstructionID
        {
            get { return _handlingInstructionID; }
            set { _handlingInstructionID = value; }
        }
        public string ExecutionInstructionID
        {
            get { return _executionInstructionID; }
            set { _executionInstructionID = value; }
        }
        public int TradingAccountID
        {
            get { return _tradingAccountID; }
            set { _tradingAccountID = value; }
        }
        public int StrategyID
        {
            get { return _strategyID; }
            set { _strategyID = value; }
        }
        public int AccountID
        {
            get { return _accountID; }
            set { _accountID = value; }
        }
        public double Peg
        {
            get { return _peg; }
            set { _peg = value; }
        }
        public double DiscreationOffset
        {
            get { return _discreationOffset; }
            set { _discreationOffset = value; }
        }
        public int DisplayQuantity
        {
            get { return _displayQuantity; }
            set { _displayQuantity = value; }
        }
        public int Random
        {
            get { return _random; }
            set { _random = value; }
        }
        public int PNP
        {
            get { return _pnp; }
            set { _pnp = value; }
        }

        //public int ClientTraderID
        //{
        //    get{return _clientTraderID;}
        //    set{_clientTraderID = value;}
        //}
        //public int ClientAccountID
        //{
        //    get{return _clientAccountID;}
        //    set{_clientAccountID = value;}
        //}

        //public int Principal 
        //{
        //    get{return _principal;}
        //    set{_principal = value;}
        //}

        //public int ShortExempt  
        //{
        //    get{return _shortExmpt;}
        //    set{_shortExmpt = value;}
        //}
        //public int ClearingFirmID
        //{
        //    get{return _clearingFirmID;}
        //    set{_clearingFirmID = value;}
        //}



        #endregion





    }
}
