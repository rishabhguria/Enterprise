using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects.AppConstants;
using Csla;
using System.ComponentModel;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Used in corporate action.
    /// </summary>
    [Serializable]
    public class TaxlotBase : BusinessBase<TaxlotBase>
    {

        public TaxlotBase()
        {
            MarkAsChild();
        }

        private string _groupID = string.Empty;

        public string GroupID
        {
            get { return _groupID; }
            set { _groupID = value; }
        }

        private string _strategy = string.Empty;

        public string Strategy
        {
            get { return _strategy; }
            set { _strategy = value; }
        }

        private string _l1TaxlotID = string.Empty;

        [Browsable(false)]
        public string L1TaxlotID
        {
            get { return _l1TaxlotID; }
            set { _l1TaxlotID = value; }
        }

        private string _l2TaxlotID = string.Empty;

        public string L2TaxlotID
        {
            get { return _l2TaxlotID; }
            set { _l2TaxlotID = value; }
        }

        private int _fundID;
        [Browsable(false)]
        public int Level1ID
        {
            get { return _fundID; }
            set { _fundID = value; }
        }

        private int _l2ID;
        [Browsable(false)]
        public int Level2ID
        {
            get { return _l2ID; }
            set { _l2ID = value; }
        }

        private string _symbol = string.Empty;

        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private AssetCategory _assetCategory = AssetCategory.None;
        [Browsable(false)]
        public AssetCategory AssetCategory
        {
            get { return _assetCategory; }
            set { _assetCategory = value; }
        }

        private Underlying _underlying = Underlying.None;
        [Browsable(false)]
        public Underlying Underlying
        {
            get { return _underlying; }
            set { _underlying = value; }
        }

        private int _exchangeID;
        [Browsable(false)]
        public int ExchangeID
        {
            get { return _exchangeID; }
            set { _exchangeID = value; }
        }

        private int _currencyID;
        [Browsable(false)]
        public int CurrencyID
        {
            get { return _currencyID; }
            set { _currencyID = value; }
        }

        private string _orderSideTagValue = string.Empty;
        [Browsable(false)]
        public string OrderSideTagValue
        {
            get { return _orderSideTagValue; }
            set { _orderSideTagValue = value; }
        }


        private string _orderSide = string.Empty;

        public string Side
        {
            get { return _orderSide; }
            set { _orderSide = value; }
        }

        private string _fund = string.Empty;

        public string Fund
        {
            get { return _fund; }
            set { _fund = value; }
        }

        private double _avgPrice = double.MinValue;

        public double AvgPrice
        {
            get { return _avgPrice; }
            set { _avgPrice = value; }
        }

        
        private float _openQty;

        public float OpenQty
        {
            get { return _openQty; }
            set { _openQty = value; }
        }

        private int _auecID = int.MinValue;
        [Browsable(false)]
        public int AUECID
        {
            get { return _auecID; }
            set { _auecID = value; }
        }


        private string _auecDate = string.Empty;

        public string AUECDate
        {
            get { return _auecDate; }
            set { _auecDate = value; }
        }


        private string _UTCDate = string.Empty;

        public string UTCDate
        {
            get { return _UTCDate; }
            set { _UTCDate = value; }
        }


        private PositionTag _positionTag ;

        public PositionTag PositionTag
        {
            get { return _positionTag; }
            set { _positionTag = value; }
        }


        private double _totalCommissionandFees = 0.0;

        public double TotalCommissionandFees
        {
            get { return _totalCommissionandFees; }
            set { _totalCommissionandFees = value; }
        }

        //private double _otherBrokerfees = 0.0;

        //public double Fees
        //{
        //    get { return _otherBrokerfees; }
        //    set { _otherBrokerfees = value; }
        //}


        //public double OtherFees
        //{
        //    get { return _stampDuty + _clearingFee + _miscFees + _taxOnCommissions + _transactionLevy; }
        //}
        //private double _stampDuty;

        //public double StampDuty
        //{
        //    get { return _stampDuty; }
        //    set { _stampDuty = value; }
        //}
        //private double _transactionLevy;

        //public double TransactionLevy
        //{
        //    get { return _transactionLevy; }
        //    set { _transactionLevy = value; }
        //}

        //private double _clearingFee;

        //public double ClearingFee
        //{
        //    get { return _clearingFee; }
        //    set { _clearingFee = value; }
        //}

        //private double _taxOnCommissions;

        //public double TaxOnCommissions
        //{
        //    get { return _taxOnCommissions; }
        //    set { _taxOnCommissions = value; }
        //}

        //private double _miscFees;

        //public double MiscFees
        //{
        //    get { return _miscFees; }
        //    set { _miscFees = value; }
        //}
	

        //private float _groupCumQty;

        //public float GroupCumQty
        //{
        //    get { return _groupCumQty; }
        //    set { _groupCumQty = value; }
        //}

        //private float _groupAllocatedQty;

        //public float GroupAllocatedQty
        //{
        //    get { return _groupAllocatedQty; }
        //    set { _groupAllocatedQty = value; }
        //}


        //private float _l1AllocatedQty;

        //public float L1AllocatedQty
        //{
        //    get { return _l1AllocatedQty; }
        //    set { _l1AllocatedQty = value; }
        //}

        //private float _l2AllocatedQty;

        //public float L2AllocatedQty
        //{
        //    get { return _l2AllocatedQty; }
        //    set { _l2AllocatedQty = value; }
        //}

        private DateTime _AUECLocalDate;

        public DateTime AUECLocalDate
        {
            get { return _AUECLocalDate; }
            set { _AUECLocalDate = value; }
        }


        private string _corpActionId = string.Empty;

        public string CorpActionID
        {
            get { return _corpActionId; }
            set { _corpActionId = value; }
        }
        
        private string _newCompanyName = string.Empty;

        public string NewCompanyName
        {
            get { return _newCompanyName; }
            set { _newCompanyName = value; }
        }


        private string _newTaxlotOpenQty = string.Empty;

        public string NewTaxlotOpenQty
        {
            get { return _newTaxlotOpenQty; }
            set { _newTaxlotOpenQty = value; }
        }


        private string _newAvgPrice = string.Empty;

        public string NewAvgPrice
        {
            get { return _newAvgPrice; }
            set { _newAvgPrice = value; }
        }

        
        private PositionType _positionType;

        [Browsable(false)]
        public PositionType PositionType
        {
            get { return _positionType; }
            set { _positionType = value; }
        }

        private string _taxLotPK = string.Empty;

        [Browsable(false)]
        public string TaxLotPK
        {
            get { return _taxLotPK; }
            set { _taxLotPK = value; }
        }

        private Guid _closingTaxLotID = Guid.Empty;
        [Browsable(false)]
        public Guid ClosingTaxlotID
        {
            get { return _closingTaxLotID ; }
            set { _closingTaxLotID  = value; }
        }


        private float _dividend;
        /// <summary>
        /// Dividend received on this taxlot
        /// </summary>
        public float Dividend
        {
            get { return _dividend; }
            set { _dividend = value; }
        }

        private DateTime _divPayoutDate;
        /// <summary>
        /// date when dividend will be accrued.
        /// </summary>
        public DateTime DivPayoutDate
        {
            get { return _divPayoutDate; }
            set { _divPayoutDate = value; }
        }


        protected override object GetIdValue()
        {
            StringBuilder id = new StringBuilder();
            id.Append(_groupID);
            if (!String.IsNullOrEmpty(_l1TaxlotID))
            {
                id.Append(_l1TaxlotID);
            }
            if (!String.IsNullOrEmpty(_l2TaxlotID))
            {
                id.Append(_l2TaxlotID);
            }

            return id.ToString();
        }
    }
}
