using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.PositionManagement;
using Prana.LogManager;
using System;
using System.Text;


namespace Prana.BusinessObjects
{
    [Serializable]
    public class BizDataFilter
    {
        private int _userID = int.MinValue;

        public int UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }

        private DateTime _startDate = DateTimeConstants.MinValue;

        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }

        private DateTime _endDate = DateTimeConstants.MinValue;

        public DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }

        private ThirdPartyNameID _dataSourceValue;

        public ThirdPartyNameID DataSourceValue
        {
            get { return _dataSourceValue; }
            set { _dataSourceValue = value; }
        }

        private Account _accountValue;

        public Account AccountValue
        {
            get { return _accountValue; }
            set { _accountValue = value; }
        }

        private AssetCategory _assetValue = AssetCategory.None;

        public AssetCategory AssetValue
        {
            get { return _assetValue; }
            set { _assetValue = value; }
        }

        private Underlying _underlyingValue = Underlying.None;

        public Underlying UnderlyingValue
        {
            get { return _underlyingValue; }
            set { _underlyingValue = value; }
        }

        private string _exchange;

        public string Exchange
        {
            get { return _exchange; }
            set { _exchange = value; }
        }

        private string _symbol;

        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private string _orderSideTagValue;

        public string OrderSideTagValue
        {
            get { return _orderSideTagValue; }
            set { _orderSideTagValue = value; }
        }

        private string _side;

        public string Side
        {
            get { return _side; }
            set { _side = value; }
        }

        private string _counterparty;

        public string Counterparty
        {
            get { return _counterparty; }
            set { _counterparty = value; }
        }

        private string _venue;

        public string Venue
        {
            get { return _venue; }
            set { _venue = value; }
        }

        public BizDataFilter()
        {

        }

        public BizDataFilter(string message)
        {
            try
            {
                string[] str = message.Split(Seperators.SEPERATOR_1);
                _userID = int.Parse(str[0]);
                _startDate = DateTime.Parse(str[1]);
                _dataSourceValue.ID = int.Parse(str[2]);
                _accountValue.AccountID = int.Parse(str[4]);
                //_assetValue = Enum.GetName(AssetCategory, str[5]);
                //_underlyingValue = Enum.GetName(Underlying,(object)str[6]);
                _exchange = str[7];
                _symbol = str[8];
                _orderSideTagValue = str[9];
                _counterparty = str[10];
                _venue = str[11];
            }
            catch (Exception ex)
            {
                throw new Exception("Filter Properties not set correctly", ex);
            }
        }
        /// <summary>
        /// The fields used in tostring method are complulsory for setting in the biz objects
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(_userID.ToString());
            sb.Append(Seperators.SEPERATOR_1);
            sb.Append(_startDate.ToString());
            sb.Append(Seperators.SEPERATOR_1);
            sb.Append(_dataSourceValue.ID);
            sb.Append(Seperators.SEPERATOR_1);
            sb.Append(_accountValue.AccountID);
            sb.Append(Seperators.SEPERATOR_1);
            sb.Append(_assetValue.ToString());
            sb.Append(Seperators.SEPERATOR_1);
            sb.Append(_underlyingValue.ToString());
            sb.Append(Seperators.SEPERATOR_1);
            sb.Append(_exchange.ToString());
            sb.Append(Seperators.SEPERATOR_1);
            sb.Append(_symbol.ToString());
            sb.Append(Seperators.SEPERATOR_1);
            sb.Append(_orderSideTagValue.ToString());
            sb.Append(Seperators.SEPERATOR_1);
            sb.Append(_counterparty.ToString());
            sb.Append(Seperators.SEPERATOR_1);
            sb.Append(_venue.ToString());


            return sb.ToString();
        }


        public void Clear()
        {
            try
            {
                this.AssetValue = AssetCategory.None;
                this.Counterparty = string.Empty;
                this.OrderSideTagValue = string.Empty;
                this.Side = string.Empty;
                this.UnderlyingValue = Underlying.None;
                this.Venue = string.Empty;
                this.Symbol = string.Empty;
                this.Exchange = string.Empty;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
