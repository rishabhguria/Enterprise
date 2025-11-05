using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects;
using Prana.Global;
using System.Xml.Serialization;
using System.Xml;
using System.ComponentModel;
using Prana.Utilities.DateTimeUtilities;

namespace Prana.Allocation.BLL
{
    /// <summary>
    /// Summary description for basketGroup.
    /// </summary>
    [Serializable]
    public class BasketGroup
    {
        #region Private Members

        private double _commission = 0;
        private double _fees = 0;
        private string _basketGroupID = string.Empty;
        private string _listID = string.Empty;
        private string _bidType = string.Empty;
        private int _baseCurrencyID = int.MinValue;
        private int _counterPartyID = int.MinValue;
        private int _venueID = int.MinValue;

        private int _assetID = int.MinValue;
        [XmlIgnore]
        private PranaInternalConstants.TYPE_OF_ALLOCATION _allocationType = PranaInternalConstants.TYPE_OF_ALLOCATION.FUND;
        [XmlIgnore]
        private PranaInternalConstants.ORDERSTATE_ALLOCATION _groupState = PranaInternalConstants.ORDERSTATE_ALLOCATION.UNALLOCATED;
        private int _auecID = int.MinValue;
        private double _cumQty = 0;
        private double  _quantity = 0;
        
        private string _asset = string.Empty;

        private int _underLyingID = int.MinValue;
        private string _underLying = string.Empty;

        private int _userID;
        private string _user=string.Empty;

        private int _tradingAccountID;
        private string _tradingAccount = string.Empty;
        //private System.Nullable<System.DateTime> _auecLocalDate = null;
        private DateTime _auecLocalDate = Prana.Utilities.DateTimeUtilities.DateTimeConstants.MinValue;

        private BasketCollection _groupedBaskets = new BasketCollection();




       

       
       
        //private double _basketValue = 0.0;
        
       
        #endregion

        private bool _updated = false;
        private AllocationFunds _funds;
        private string _basketName = string.Empty;
        private AllocationStrategies _companyStrategies;
        private double _allocatedQty = 0;
        
        public BasketGroup()
        {
           // if (_basketGroupID == string.Empty)
            //{
                _basketGroupID = AllocationIDGenerator.GenerateBasketGroupID();
           // }
            
        }

        public void AddBaskets(BasketCollection baskets)
        {
            if (baskets.Count == 0)
                return;
            if (baskets.Count == 1)
            {
                AddBasket(baskets[0]);
                return;
            }
           
            //_listID = Constants.MULTIPLE;
            _groupedBaskets = baskets;
            double quantity = Quantity;
            double exeQty = CumQty;
            double exeValue = ExeValue;
            _userID = baskets[0].UserID;
            _user = baskets[0].User;
            
            _assetID = baskets[0].AssetID;
            _asset = baskets[0].Asset;
            _basketName = baskets[0].BasketName;
            _tradingAccountID = baskets[0].TradingAccountID;

            _tradingAccount = baskets[0].TradingAccount;
            _underLyingID = baskets[0].UnderLyingID;
            _underLying = baskets[0].UnderLying;
            
            int i = 0;
            foreach (BasketDetail basket in baskets)
            {
                if (i > 0)
                {


                    _listID += "," + basket.BasketID;
                 
                    if (_assetID != basket.AssetID)
                    {
                        _assetID = int.MinValue;
                        _asset = Constants.MULTIPLE;
                    }
                    if (_basketName != basket.BasketName)
                    {
                        _basketName = Constants.MULTIPLE;
                    }
                    if (_underLyingID != basket.UnderLyingID)
                    {
                        _underLyingID = int.MinValue;
                        _underLying = Constants.MULTIPLE;
                    }
                    if (_tradingAccountID != basket.TradingAccountID)
                    {
                        _tradingAccountID = int.MinValue;
                        _tradingAccount = Constants.MULTIPLE;
                    }
                    if (_userID != basket.UserID)
                    {
                        _userID = int.MinValue;
                        _user = Constants.MULTIPLE;
                    }
                }
                i++;
            }
        }
        public void AddBasket(BasketDetail  basket)
        {
            _groupedBaskets.Add(basket);
            SetBasketDetails(basket);
           
          
        }
        private  void SetBasketDetails(BasketDetail basket)
        {
            double quantity = Quantity;
            double exeQty = CumQty;
            double exeValue = ExeValue;
            _userID = basket.UserID;
            _user = basket.User;
            _basketName = basket.BasketName;
            _assetID = basket.AssetID;
            _asset = basket.Asset;
            _listID = basket.TradedBasketID;
            _tradingAccountID = basket.TradingAccountID;

            _tradingAccount = basket.TradingAccount;
            _underLyingID = basket.UnderLyingID;
            _underLying = basket.UnderLying;
            _auecID = basket.BasketOrders[0].AUECID;
        }

        #region Properties

        public string BasketGroupID
        {
            set { _basketGroupID = value; }
            get { return _basketGroupID; }
        }
        public string ListID
        {
            set { _listID = value; }
            get { return _listID; }
        }
        public int AssetID
        {
            set { _assetID = value; }
            get { return _assetID; }
        }
        public int AUECID
        {
            set { _auecID = value; }
            get { return _auecID; }
        }
        public string  Asset
        {
            set { _asset = value; }
            get { return _asset; }
        }
        public int UnderLyingID
        {
            set { _underLyingID = value; }
            get { return _underLyingID; }
        }
        public string UnderLying
        {
            set { _underLying = value; }
            get { return _underLying; }
        }
        
        public string BasketName
        {
            set { _basketName = value; }
            get { return _basketName; }
        }       
        //public string BidType
        //{
        //    set { _bidType = value; }
        //    get { return _bidType; }
        //}
        public int BaseCurrencyID
        {
            set { _baseCurrencyID = value; }
            get { return _baseCurrencyID; }

        }
        public int UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }
        public string User
        {
            set { _user = value; }
            get { return _user; }
        }
        public DateTime AUECLocalDate
        {
            get { return _auecLocalDate; }
            set { _auecLocalDate = value; }
        }
        //public DateTime AUECLocalDate
        //{
        //    get { return _auecLocalDate.Value; }
        //    set { _auecLocalDate = value; }
        //}
        public int TradingAccountID
        {
            get { return _tradingAccountID; }
            set { _tradingAccountID = value; }
        }
        public string TradingAccount
        {
            set { _tradingAccount = value; }
            get { return _tradingAccount; }
        }
        public PranaInternalConstants.TYPE_OF_ALLOCATION AllocationType
        {
            set { _allocationType = value; }
            get { return _allocationType; }
        }
        public PranaInternalConstants.ORDERSTATE_ALLOCATION GroupState
        {
            set { _groupState = value; }
            get { return _groupState; }
        }
        public double ExeValue
        {
            get
            {
                double exeValue = 0.0;
                foreach (BasketDetail basket in _groupedBaskets)
                {
                    exeValue = basket.ExeValue + exeValue;
                }
                return exeValue;
            }
        }
        public double CumQty
        {
            get
            {
                double exeQty = 0;
                foreach (BasketDetail basket in _groupedBaskets)
                {
                    exeQty = basket.CumQty + exeQty;
                }
                _cumQty = exeQty;
                return _cumQty;
            }
            set
            {
                _cumQty = value;
            }
        }
        public double  AllocatedQty
        {
            get
            {
                return _allocatedQty;
            }
            set 
            {
                 _allocatedQty =value;
            }
        }
        public double Quantity
        {
            get
            {

                double totalQty = 0;
                foreach (BasketDetail basket in _groupedBaskets)
                {
                    totalQty = basket.Quantity + totalQty;
                }
                _quantity = totalQty;
                return _quantity;
            }
            set
            {
                _quantity = value;
            }
        }
        public BasketCollection AddedBaskets
        {
            get { return _groupedBaskets; }
        }

        #endregion
        public void AllocateGroupToFund(AllocationFunds funds, double allocatedQty)
        {

            _funds = funds;
            //foreach (AllocationFund allocationFund in _funds)
            //{
            //    allocationFund.ParentBasketGroup = this;
            //}
            _allocatedQty = allocatedQty;
            // SetEntityValues();



        }
        public void AllocateGroupToStrategy(AllocationStrategies companyStrategies, double  allocatedQty)
        {
            
            _companyStrategies = companyStrategies;
            _allocatedQty = allocatedQty;
           // SetEntityValues();

        }
        public AllocationFunds AllocationFunds
        {
            set { _funds = value; }
            get { return _funds; }

        }
        public AllocationStrategies Strategies
        {
            set { _companyStrategies = value; }
            get { return _companyStrategies; }

        }
        public bool Updated
        {
            set { _updated = value;
            if (!_updated)
            {
                foreach (BasketDetail basket in _groupedBaskets)
                {
                    basket.Updated = value;
                }
            }
            }
            get {
                bool updated = false;
                foreach (BasketDetail basket in _groupedBaskets)
                {
                    if (basket.Updated)
                    {
                        updated = true;
                        break;
                    }
                }
                
                return updated ; }
        }
        public double Commission
        {
            get
            {
                return _commission;
            }
            set
            {
                _commission = value;
                //if (_commissionCalculationTime.Equals(false))
                //{
                //    CalculateTotalCommission();
                //}
            }
        }

        public void CalculateTotalCommission()
        {
            foreach (AllocationFund allocationFund in _funds)
            {
                //allocationFund.Commission = (allocationFund.AllocatedQty / _allocatedQty) * _commission;
                allocationFund.Commission = (allocationFund.AllocatedQty / _quantity) * _commission;
            }
            _isCommissionCalculated = false;
        }



        public double Fees
        {
            get { return _fees; }
            set
            {
                _fees = value;
                //if (_commissionCalculationTime.Equals(false))
                //{
                //    CalculateTotalFees();
                //}
            }
        }

        public void CalculateTotalFees()
        {
            foreach (AllocationFund allocationFund in _funds)
            {
                //allocationFund.Fees = (allocationFund.AllocatedQty / _allocatedQty) * _fees;
                allocationFund.Fees = (allocationFund.AllocatedQty / _quantity) * _fees;
            }
            _isCommissionCalculated = false;
        }

        [XmlIgnore]
        private bool _commissionCalculationTime = false;

        [Browsable(false)]
        public bool CommissionCalculationTime
        {
            get { return _commissionCalculationTime; }
            set { _commissionCalculationTime = value; }
        }

        private bool _isCommissionCalculated = false;

        public bool IsCommissionCalculated
        {
            get { return _isCommissionCalculated; }
            set { _isCommissionCalculated = value; }
        }
        public int CounterPartyID
        {
            set { _counterPartyID = value; }
            get { return _counterPartyID; }

        }
        public int VenueID
        {
            get { return _venueID; }
            set { _venueID = value; }
        }

        public void RecalculateGroupCommAndFee_FromTaxlotComm()
        {
            double new_commission = 0;
            double new_Fees = 0;

            if (_funds != null)
            {
                foreach (AllocationFund fund in _funds)
                {
                    new_commission += fund.Commission;
                    new_Fees += fund.Fees;
                }
            }
            _commission = new_commission;
            _fees = new_Fees;
            _isCommissionCalculated = false;
        }
    }
}
