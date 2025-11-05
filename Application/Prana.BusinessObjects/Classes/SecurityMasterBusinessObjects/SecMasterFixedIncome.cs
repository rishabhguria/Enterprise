using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using System;

namespace Prana.BusinessObjects.SecurityMasterBusinessObjects
{
    [Serializable]
    public class SecMasterFixedIncome : SecMasterBaseObj
    {
        private CollateralType _collateralType;
        public CollateralType CollateralType
        {
            get { return _collateralType; }
            set { _collateralType = value; }
        }
        private int _collateralTypeID;
        public int CollateralTypeID
        {
            get { return _collateralTypeID; }
            set { _collateralTypeID = value; }
        }
        private string _bondDescription;
        public string BondDescription
        {
            get { return _bondDescription; }
            set { _bondDescription = value; }
        }

        private int _daysToSettlement = int.MinValue;
        public int DaysToSettlement
        {
            get { return _daysToSettlement; }
            set { _daysToSettlement = value; }
        }

        private AccrualBasis _accrualBasis;
        public AccrualBasis AccrualBasis
        {
            get { return _accrualBasis; }
            set { _accrualBasis = value; }
        }

        private SecurityType _bondType;
        public SecurityType BondType
        {
            get { return _bondType; }
            set { _bondType = value; }
        }

        private CouponFrequency _frequency;
        public CouponFrequency Frequency
        {
            get { return _frequency; }
            set { _frequency = value; }
        }

        private int _bondTypeID;
        public int BondTypeID
        {
            get { return _bondTypeID; }
            set { _bondTypeID = value; }
        }

        private int _accrualBasisID;
        public int AccrualBasisID
        {
            get { return _accrualBasisID; }
            set { _accrualBasisID = value; }
        }

        private int _couponFrequencyID;
        public int CouponFrequencyID
        {
            get { return _couponFrequencyID; }
            set { _couponFrequencyID = value; }
        }

        private DateTime _dateMaturity = DateTimeConstants.MinValue;
        // MaturityDate - security maturity date
        public DateTime MaturityDate
        {
            get { return _dateMaturity; }
            set { _dateMaturity = value; }
        }

        private double _price;
        // Price - market price of the security, not necessary for accrued interest percentage
        // Either price and marketvalue or par are needed for accrued interest value
        public double Price
        {
            get { return _price; }
            set { _price = value; }
        }

        private double _par;
        // Par - par value of this bond
        // Either price and marketvalue or par are needed for accrued interest value
        public double Par
        {
            get { return _par; }
            set { _par = value; }
        }

        private double _holdingValue;
        // HoldingValue - holding value (price/100 * par) of the this bond
        // Either price and marketvalue or par are needed for accrued interest value
        public double HoldingValue
        {
            get { return _holdingValue; }
            set { _holdingValue = value; }
        }

        private double _coupon;
        // Coupon - decimal value of the annual coupon, eg 6.0 means 6%
        public double Coupon
        {
            get { return _coupon; }
            set { _coupon = value; }
        }

        private bool _isZero;
        // IsZero - set true if zero coupon bond, can also set coupon above to 0
        public bool IsZero
        {
            get { return _isZero; }
            set { _isZero = value; }
        }

        private DateTime _firstCouponDate = DateTimeConstants.MinValue;
        // FirstCouponDate - first coupon date from the date of issue 
        public DateTime FirstCouponDate
        {
            get { return _firstCouponDate; }
            set { _firstCouponDate = value; }
        }

        private DateTime _dateIssue = DateTimeConstants.MinValue;
        // IssueDate - issue date of the security
        public DateTime IssueDate
        {
            get { return _dateIssue; }
            set { _dateIssue = value; }
        }

        private string _countryCode;
        // CountryCode - 2 digit country code, such as Thomson Reuters codes, blank defaults to US
        public string CountryCode
        {
            get { return _countryCode; }
            set { _countryCode = value; }
        }

        public override void FillUIData(SecMasterUIObj uiObj)
        {
            base.FillUIData(uiObj);
            _dateMaturity = uiObj.ExpirationDate.Date;
            _dateIssue = uiObj.IssueDate.Date;
            _firstCouponDate = uiObj.FirstCouponDate.Date;
            _bondTypeID = uiObj.BondTypeID;
            _bondType = (SecurityType)(uiObj.BondTypeID);
            _accrualBasisID = uiObj.AccrualBasisID;
            _accrualBasis = (AccrualBasis)uiObj.AccrualBasisID;
            _coupon = uiObj.Coupon;
            _couponFrequencyID = uiObj.CouponFrequencyID;
            _frequency = (CouponFrequency)(uiObj.CouponFrequencyID);
            _isZero = uiObj.IsZero;
            _daysToSettlement = uiObj.DaysToSettlement;
            // Kuldeep A.: Here Delta refers to Leveraged Factor.
            _delta = uiObj.Delta;
            _collateralTypeID = uiObj.CollateralTypeID;
            _collateralType = (CollateralType)uiObj.CollateralTypeID;
        }

        public override void FillData(SymbolData level1Data)
        {
            base.FillData(level1Data);
            FixedIncomeSymbolData fixedIncomeData = level1Data as FixedIncomeSymbolData;
            if (fixedIncomeData != null)
            {
                this._accrualBasis = fixedIncomeData.AccrualBasis;
                this._accrualBasisID = fixedIncomeData.AccrualBasisID;
                this._bondDescription = fixedIncomeData.BondDescription;
                this._bondType = fixedIncomeData.BondType;
                this._coupon = fixedIncomeData.Coupon;
                this._couponFrequencyID = fixedIncomeData.CouponFrequencyID;
                this._firstCouponDate = fixedIncomeData.FirstCouponDate;
                this._frequency = fixedIncomeData.Frequency;
                this._dateIssue = fixedIncomeData.IssueDate;
                this._isZero = fixedIncomeData.IsZero;
                this._dateMaturity = fixedIncomeData.MaturityDate;
                this._collateralType = fixedIncomeData.CollateralType;
                this._collateralTypeID = fixedIncomeData.CollateralTypeID;
                this._bondTypeID = fixedIncomeData.BondTypeID;
            }
        }

        public override void FillData(object[] row, int offset)
        {
            base.FillData(row, 0);
            if (row != null)
            {
                int IssueDate = offset + 0;
                int Coupon = offset + 1;
                int MaturityDate = offset + 2;
                int BondType = offset + 3;
                int AccrualBasis = offset + 4;
                int BondDescription = offset + 5;
                int FirstCouponDate = offset + 6;
                int IsZero = offset + 7;
                int frequency = offset + 8;
                int DaysToSettlement = offset + 9;
                int multiplier = offset + 10;
                int CollateralType = offset + 45;

                try
                {
                    if (row[IssueDate] != System.DBNull.Value)
                    {
                        _dateIssue = DateTime.Parse(row[IssueDate].ToString());
                    }
                    if (row[Coupon] != System.DBNull.Value)
                    {
                        _coupon = double.Parse(row[Coupon].ToString());
                    }
                    if (row[MaturityDate] != System.DBNull.Value)
                    {
                        _dateMaturity = Convert.ToDateTime(row[MaturityDate].ToString());
                    }
                    if (row[BondType] != System.DBNull.Value)
                    {
                        _bondType = (SecurityType)(Convert.ToInt32(row[BondType].ToString()));//new DateTime(Convert.ToInt32(row[MaturityMonth].ToString().Substring(0, 4)), Convert.ToInt32(row[MaturityMonth].ToString().Substring(4, 2)), Convert.ToInt32(row[SettleMentDate].ToString())); //,23,59,59);
                    }
                    if (row[BondType] != System.DBNull.Value)
                    {
                        _bondTypeID = (Convert.ToInt32(row[BondType].ToString()));
                    }
                    if (row[AccrualBasis] != System.DBNull.Value)
                    {
                        _accrualBasis = (AccrualBasis)(Convert.ToInt32(row[AccrualBasis].ToString()));
                    }
                    if (row[AccrualBasis] != System.DBNull.Value)
                    {
                        _accrualBasisID = (Convert.ToInt32(row[AccrualBasis].ToString()));
                    }
                    if (row[BondDescription] != System.DBNull.Value)
                    {
                        _bondDescription = row[BondDescription].ToString();
                    }
                    if (row[FirstCouponDate] != System.DBNull.Value)
                    {
                        _firstCouponDate = DateTime.Parse(row[FirstCouponDate].ToString());
                    }
                    if (row[IsZero] != System.DBNull.Value)
                    {
                        _isZero = Convert.ToBoolean(row[IsZero].ToString());
                    }
                    if (row[frequency] != System.DBNull.Value)
                    {
                        _frequency = (CouponFrequency)(Convert.ToInt32(row[frequency].ToString()));
                    }
                    if (row[frequency] != System.DBNull.Value)
                    {
                        _couponFrequencyID = (Convert.ToInt32(row[frequency].ToString()));
                    }
                    if (row[DaysToSettlement] != System.DBNull.Value)
                    {
                        _daysToSettlement = Convert.ToInt32(row[DaysToSettlement].ToString());
                    }
                    if (row[multiplier] != System.DBNull.Value)
                    {
                        _multiplier = Convert.ToDouble(row[multiplier].ToString());
                    }
                    if (_longName.Equals(string.Empty))
                    {
                        _longName = _bondDescription;
                    }
                    if (row[CollateralType] != System.DBNull.Value)
                    {
                        _collateralType = (CollateralType)(Convert.ToInt32(row[CollateralType].ToString()));
                    }
                    if (row[CollateralType] != System.DBNull.Value)
                    {
                        _collateralTypeID = (Convert.ToInt32(row[CollateralType].ToString()));
                    }

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
}
