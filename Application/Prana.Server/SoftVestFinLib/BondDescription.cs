using Prana.LogManager;
using System;
using System.Collections;

namespace SoftVest.FinLib
{
    public class BondDescription
    {
        // accrual basis, if set as UseDefault, FillInDefaultFields() can figure out values
        public enum Accrual
        {
            Accrual_UseDefault = Constants.UseDefaultValue,
            Accrual_Actual_365,
            Accrual_Actual_360,
            Accrual_30_360,
            Accrual_30E_360,
            Accrual_Actual_Actual
        }

        public enum BondSecurityType
        {
            SecurityType_UseDefault = Constants.UseDefaultValue,
            SecurityType_Treasury,
            SecurityType_Municipal,
            SecurityType_Agency,
            SecurityType_Corporate,
            SecurityType_Other_Government
        }

        public enum Frequency
        {
            Frequency_UseDefault = Constants.UseDefaultValue,
            Frequency_Monthly,
            Frequency_Quarterly,
            Frequency_SemiAnnually,
            Frequency_Annually,
            Frequency_None
        }

        // constructors
        //
        public BondDescription()
        {
        }

        public BondDescription(BondDescription bondDesc)
        {
            Init(bondDesc._price, bondDesc._par, bondDesc._holdingValue, bondDesc._coupon, bondDesc._isZero, bondDesc._freq, bondDesc._dateMaturity,
            bondDesc._dateCall, bondDesc._datePut, bondDesc._dateFirst, bondDesc._dateIssue, bondDesc._accrualBasis, bondDesc._securityType,
            bondDesc._countryCode);

        }

        // basic details needed for AI calculation
        public BondDescription(double coupon, Frequency freq, DateTime dateMaturity, DateTime dateFirst, DateTime dateIssue, Accrual accrualBasis)
        {
            Init(Constants.NA_Value, Constants.NA_Value, Constants.NA_Value, coupon, false, freq, dateMaturity,
            Constants.NullDate, Constants.NullDate, dateFirst, dateIssue, accrualBasis, BondDescription.BondSecurityType.SecurityType_UseDefault, "");

        }

        public BondDescription(double coupon, bool isZero, Frequency freq, DateTime dateMaturity, DateTime dateCall, DateTime datePut,
            DateTime dateFirst, DateTime dateIssue, Accrual accrualBasis, BondSecurityType securityType, string countryCode)
        {
            Init(Constants.NA_Value, Constants.NA_Value, Constants.NA_Value, coupon, isZero, freq, dateMaturity,
           dateCall, datePut, dateFirst, dateIssue, accrualBasis, securityType, countryCode);

        }

        // details for all fields in bond description object
        public BondDescription(double price, double par, double holdingValue, double coupon, bool isZero, Frequency freq, DateTime dateMaturity,
            DateTime dateCall, DateTime datePut, DateTime dateFirst, DateTime dateIssue, Accrual accrualBasis, BondSecurityType securityType,
            string countryCode)
        {
            Init(price, par, holdingValue, coupon, isZero, freq, dateMaturity,
            dateCall, datePut, dateFirst, dateIssue, accrualBasis, securityType, countryCode);
        }

        public void Init(double price, double par, double holdingValue, double coupon, bool isZero, Frequency freq, DateTime dateMaturity,
            DateTime dateCall, DateTime datePut, DateTime dateFirst, DateTime dateIssue, Accrual accrualBasis, BondSecurityType securityType,
            string countryCode)
        {
            try
            {
                _price = price;
                _par = par;
                _holdingValue = holdingValue;
                _coupon = coupon;
                _isZero = isZero;
                _freq = freq;
                _dateMaturity = dateMaturity;
                _dateCall = dateCall;
                _datePut = datePut;
                _dateFirst = dateFirst;
                _dateIssue = dateIssue;
                _accrualBasis = accrualBasis;
                _securityType = securityType;
                _countryCode = countryCode;

                if (!_bBondMarketDefaultsSetup)
                {
                    SetupBondMarketDefaults();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        // Price - market price of the security, not necessary for accrued interest percentage
        // Either price and marketvalue or par are needed for accrued interest value
        public double Price
        {
            get { return _price; }
            set { _price = value; }
        }

        // Par - par value of this bond
        // Either price and marketvalue or par are needed for accrued interest value
        public double Par
        {
            get { return _par; }
            set { _par = value; }
        }

        // HoldingValue - holding value (price/100 * par) of the this bond
        // Either price and marketvalue or par are needed for accrued interest value
        public double HoldingValue
        {
            get { return _holdingValue; }
            set { _holdingValue = value; }
        }

        // Coupon - decimal value of the annual coupon, eg 6.0 means 6%
        public double Coupon
        {
            get { return _coupon; }
            set { _coupon = value; }
        }

        // IsZero - set true if zero coupon bond, can also set coupon above to 0
        public bool IsZero
        {
            get { return _isZero; }
            set { _isZero = value; }
        }

        // Freq - number of times per year coupon is paid 
        public Frequency Freq
        {
            get { return _freq; }
            set { _freq = value; }
        }

        // MaturityDate - security maturity date
        public DateTime MaturityDate
        {
            get { return _dateMaturity; }
            set { _dateMaturity = value; }
        }

        // CallDate - next call date, not necessary for accrued interest
        public DateTime CallDate
        {
            get { return _dateCall; }
            set { _dateCall = value; }
        }

        // PutDate - next put date, not necessary for accrued interest
        public DateTime PutDate
        {
            get { return _datePut; }
            set { _datePut = value; }
        }

        // FirstCouponDate - first coupon date from the date of issue 
        public DateTime FirstCouponDate
        {
            get { return _dateFirst; }
            set { _dateFirst = value; }
        }

        // IssueDate - issue date of the security
        public DateTime IssueDate
        {
            get { return _dateIssue; }
            set { _dateIssue = value; }
        }

        // AccrualBasis - accrual basis, if set as UseDefault, FillInDefaultFields() can figure out values
        public Accrual AccrualBasis
        {
            get { return _accrualBasis; }
            set { _accrualBasis = value; }
        }

        // SecurityType - general type of security, BondDescription may use this type to help fill in default values
        public BondSecurityType SecurityType
        {
            get { return _securityType; }
            set { _securityType = value; }
        }

        // CountryCode - 2 digit country code, such as Thomson Reuters codes, blank defaults to US
        public string CountryCode
        {
            get { return _countryCode; }
            set { _countryCode = value; }
        }

        // function to fill in default values for fields that have UseDefaultValue
        // can either be called explicitly by user or can be called by BondCalculator 
        //
        public void FillInDefaultFields()
        {
            try
            {
                if (_countryCode.Equals("") || _countryCode.ToLower().Equals("us"))
                {
                    // US security

                    // if not mortgage backed, then use semiannually, unless is a zero type
                    if (_freq == Frequency.Frequency_UseDefault && !_isZero)
                        _freq = Frequency.Frequency_SemiAnnually;

                    if (_securityType == BondSecurityType.SecurityType_Treasury)
                    {
                        // treasury
                        if (_accrualBasis == Accrual.Accrual_UseDefault)
                            _accrualBasis = Accrual.Accrual_Actual_Actual;

                    }
                    else if (_securityType != BondSecurityType.SecurityType_UseDefault)
                    {
                        // non treasury
                        if (_accrualBasis == Accrual.Accrual_UseDefault)
                            _accrualBasis = Accrual.Accrual_30_360;
                    }

                }
                else
                {
                    // international security

                    // based on Fabozzi "Fixed Income Securities", p. 346, non US government bonds
                    if (_securityType == BondSecurityType.SecurityType_Other_Government)
                    {
                        if (_accrualBasis == Accrual.Accrual_UseDefault || _freq == Frequency.Frequency_UseDefault)
                        {
                            // look up the accrual basis, frequency defaults for some foreign countries
                            for (int i = 0; i < _arBondMarketDefaults.Count; i++)
                            {
                                BondMarketDefaultsArrayObject bondObj = (BondMarketDefaultsArrayObject)_arBondMarketDefaults[i];
                                if (_accrualBasis == Accrual.Accrual_UseDefault)
                                    _accrualBasis = bondObj.accrual;
                                if (_freq == Frequency.Frequency_UseDefault)
                                    _freq = bondObj.freq;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        // some functions to convert from enum to text, get text for BondDescription info
        //
        public static string GetFrequencyText(Frequency freq)
        {
            try
            {
                string sFreq = "UseDefault";
                if (freq == Frequency.Frequency_UseDefault)
                    sFreq = "UseDefault";
                else if (freq == Frequency.Frequency_Monthly)
                    sFreq = "Monthly";
                else if (freq == Frequency.Frequency_Quarterly)
                    sFreq = "Quarterly";
                else if (freq == Frequency.Frequency_SemiAnnually)
                    sFreq = "SemiAnnually";
                else if (freq == Frequency.Frequency_Annually)
                    sFreq = "Annually";
                else if (freq == Frequency.Frequency_None)
                    sFreq = "None";
                return sFreq;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }

                return string.Empty;
            }
        }

        public static Frequency GetFrequency(string sFreq)
        {
            Frequency freq = Frequency.Frequency_UseDefault;
            try
            {
                if (sFreq.Equals("UseDefault"))
                    freq = Frequency.Frequency_UseDefault;
                else if (sFreq.Equals("Monthly"))
                    freq = Frequency.Frequency_Monthly;
                else if (sFreq.Equals("Quarterly"))
                    freq = Frequency.Frequency_Quarterly;
                else if (sFreq.Equals("SemiAnnually"))
                    freq = Frequency.Frequency_SemiAnnually;
                else if (sFreq.Equals("Annually"))
                    freq = Frequency.Frequency_Annually;
                else if (sFreq.Equals("None"))
                    freq = Frequency.Frequency_None;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return freq;
        }

        public static int FrequencyPerYear(Frequency freq)
        {
            try
            {
                int nFreq = 0;
                switch (freq)
                {
                    case Frequency.Frequency_Monthly:
                        {
                            nFreq = 12;
                            break;
                        }
                    case Frequency.Frequency_Quarterly:
                        {
                            nFreq = 4;
                            break;
                        }
                    case Frequency.Frequency_SemiAnnually:
                        {
                            nFreq = 2;
                            break;
                        }
                    case Frequency.Frequency_Annually:
                        {
                            nFreq = 1;
                            break;
                        }
                }
                return nFreq;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }

                return Int32.MinValue;
            }
        }

        public string GetFrequencyText()
        {
            try
            {
                return GetFrequencyText(_freq);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }

                return string.Empty;
            }
        }

        public int FrequencyPerYear()
        {
            try
            {
                return FrequencyPerYear(_freq);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }

                return Int32.MinValue;
            }
        }

        public string GetBondSecurityTypeText(BondSecurityType bondSecurityType)
        {
            try
            {
                string sBondSecurityType = "UseDefault";
                if (bondSecurityType == BondSecurityType.SecurityType_UseDefault)
                    sBondSecurityType = "UseDefault";
                else if (bondSecurityType == BondSecurityType.SecurityType_Treasury)
                    sBondSecurityType = "Treasury";
                else if (bondSecurityType == BondSecurityType.SecurityType_Municipal)
                    sBondSecurityType = "Municipal";
                else if (bondSecurityType == BondSecurityType.SecurityType_Agency)
                    sBondSecurityType = "Agency";
                else if (bondSecurityType == BondSecurityType.SecurityType_Corporate)
                    sBondSecurityType = "Corporate";
                else if (bondSecurityType == BondSecurityType.SecurityType_Other_Government)
                    sBondSecurityType = "Other_Government";
                return sBondSecurityType;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }

                return string.Empty;
            }
        }

        public BondSecurityType GetBondSecurityType(string sBondSecurityType)
        {
            BondSecurityType bondSecurityType = BondSecurityType.SecurityType_UseDefault;
            try
            {
                if (sBondSecurityType.Equals("UseDefault"))
                    bondSecurityType = BondSecurityType.SecurityType_UseDefault;
                else if (sBondSecurityType.Equals("Treasury"))
                    bondSecurityType = BondSecurityType.SecurityType_Treasury;
                else if (sBondSecurityType.Equals("Municipal"))
                    bondSecurityType = BondSecurityType.SecurityType_Municipal;
                else if (sBondSecurityType.Equals("Agency"))
                    bondSecurityType = BondSecurityType.SecurityType_Agency;
                else if (sBondSecurityType.Equals("Corporate"))
                    bondSecurityType = BondSecurityType.SecurityType_Corporate;
                else if (sBondSecurityType.Equals("Other_Government"))
                    bondSecurityType = BondSecurityType.SecurityType_Other_Government;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return bondSecurityType;
        }

        public string GetAccrualText(Accrual accrual)
        {
            try
            {
                string sAccrual = "UseDefault";
                if (accrual == Accrual.Accrual_UseDefault)
                    sAccrual = "Accrual_UseDefault";
                else if (accrual == Accrual.Accrual_Actual_365)
                    sAccrual = "Accrual_Actual_365";
                else if (accrual == Accrual.Accrual_Actual_360)
                    sAccrual = "Accrual_Actual_360";
                else if (accrual == Accrual.Accrual_30_360)
                    sAccrual = "Accrual_30_360";
                else if (accrual == Accrual.Accrual_30E_360)
                    sAccrual = "Accrual_30E_360";
                else if (accrual == Accrual.Accrual_Actual_Actual)
                    sAccrual = "Accrual_Actual_Actual";
                return sAccrual;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }

                return string.Empty;
            }
        }

        public Accrual GetAccrual(string sAccrual)
        {
            Accrual accrual = Accrual.Accrual_UseDefault;
            try
            {
                if (sAccrual.Equals("Accrual_UseDefault"))
                    accrual = Accrual.Accrual_UseDefault;
                else if (sAccrual.Equals("Accrual_Actual_365"))
                    accrual = Accrual.Accrual_Actual_365;
                else if (sAccrual.Equals("Accrual_Actual_360"))
                    accrual = Accrual.Accrual_Actual_360;
                else if (sAccrual.Equals("Accrual_30_360"))
                    accrual = Accrual.Accrual_30_360;
                else if (sAccrual.Equals("Accrual_30E_360"))
                    accrual = Accrual.Accrual_30E_360;
                else if (sAccrual.Equals("Accrual_Actual_Actual"))
                    accrual = Accrual.Accrual_Actual_Actual;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return accrual;
        }


        public string ToXml()
        {
            try
            {
                string sXml = "<BondDescription>";
                sXml += Utils.GetXmlElement("Price", _price.ToString());
                sXml += Utils.GetXmlElement("Par", _par.ToString());
                sXml += Utils.GetXmlElement("HoldingValue", _holdingValue.ToString());
                sXml += Utils.GetXmlElement("Coupon", _coupon.ToString());
                sXml += Utils.GetXmlElement("IsZero", _isZero.ToString());
                sXml += Utils.GetXmlElement("Frequency", GetFrequencyText(_freq));
                sXml += Utils.GetXmlElement("DateMaturity", _dateMaturity.ToString());
                sXml += Utils.GetXmlElement("DateCall", _dateCall.ToString());
                sXml += Utils.GetXmlElement("DatePut", _datePut.ToString());
                sXml += Utils.GetXmlElement("DateFirst", _dateFirst.ToString());
                sXml += Utils.GetXmlElement("DateIssue", _dateIssue.ToString());
                sXml += Utils.GetXmlElement("AccrualBasis", GetAccrualText(_accrualBasis));
                sXml += Utils.GetXmlElement("SecurityType", GetBondSecurityTypeText(_securityType));
                sXml += Utils.GetXmlElement("CountryCode", _countryCode);
                sXml += "</BondDescription>";
                return sXml;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }

                return string.Empty;
            }
        }

        private static void SetupBondMarketDefaults()
        {
            try
            {
                // no synchronization done here, any problem with not being thread safe?
                if (!_bBondMarketDefaultsSetup)
                {
                    _bBondMarketDefaultsSetup = true;

                    // based on Fabozzi "Fixed Income Securities", p. 346, non US government bonds
                    // accrual basis and coupon frequency for major countries
                    // how do we add new countries as needed, make this more flexible?
                    _arBondMarketDefaults.Add(new BondMarketDefaultsArrayObject("at", Frequency.Frequency_Annually, Accrual.Accrual_30E_360));           // austria
                    _arBondMarketDefaults.Add(new BondMarketDefaultsArrayObject("au", Frequency.Frequency_SemiAnnually, Accrual.Accrual_Actual_Actual)); // australia
                    _arBondMarketDefaults.Add(new BondMarketDefaultsArrayObject("be", Frequency.Frequency_SemiAnnually, Accrual.Accrual_30E_360));       // belgium
                    _arBondMarketDefaults.Add(new BondMarketDefaultsArrayObject("bg", Frequency.Frequency_SemiAnnually, Accrual.Accrual_Actual_365));    // great britain
                    _arBondMarketDefaults.Add(new BondMarketDefaultsArrayObject("ca", Frequency.Frequency_SemiAnnually, Accrual.Accrual_Actual_Actual)); // canada
                    _arBondMarketDefaults.Add(new BondMarketDefaultsArrayObject("ch", Frequency.Frequency_Annually, Accrual.Accrual_30E_360));           // switzerland
                    _arBondMarketDefaults.Add(new BondMarketDefaultsArrayObject("de", Frequency.Frequency_Annually, Accrual.Accrual_30E_360));           // germany
                    _arBondMarketDefaults.Add(new BondMarketDefaultsArrayObject("dk", Frequency.Frequency_Annually, Accrual.Accrual_30E_360));           // denmark
                    _arBondMarketDefaults.Add(new BondMarketDefaultsArrayObject("es", Frequency.Frequency_SemiAnnually, Accrual.Accrual_Actual_Actual)); // spain
                    _arBondMarketDefaults.Add(new BondMarketDefaultsArrayObject("fr", Frequency.Frequency_Annually, Accrual.Accrual_Actual_Actual));     // france
                    _arBondMarketDefaults.Add(new BondMarketDefaultsArrayObject("ie", Frequency.Frequency_Annually, Accrual.Accrual_Actual_365));        // ireland
                    _arBondMarketDefaults.Add(new BondMarketDefaultsArrayObject("it", Frequency.Frequency_Annually, Accrual.Accrual_30E_360));           // italy
                    _arBondMarketDefaults.Add(new BondMarketDefaultsArrayObject("nl", Frequency.Frequency_Annually, Accrual.Accrual_30E_360));           // netherlands
                    _arBondMarketDefaults.Add(new BondMarketDefaultsArrayObject("no", Frequency.Frequency_SemiAnnually, Accrual.Accrual_Actual_365));    // norway
                    _arBondMarketDefaults.Add(new BondMarketDefaultsArrayObject("nz", Frequency.Frequency_SemiAnnually, Accrual.Accrual_Actual_Actual)); // new zealand
                    _arBondMarketDefaults.Add(new BondMarketDefaultsArrayObject("se", Frequency.Frequency_Annually, Accrual.Accrual_30E_360));           // sweden
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        private class BondMarketDefaultsArrayObject
        {
            public BondMarketDefaultsArrayObject(string name, Frequency freq, Accrual accrual)
            {
                this.name = name;
                this.freq = freq;
                this.accrual = accrual;
            }
            public string name;
            public Frequency freq;
            public Accrual accrual;
        }

        private double _price;
        private double _par;
        private double _holdingValue;
        private double _coupon;
        private bool _isZero;
        private Frequency _freq;
        private DateTime _dateMaturity;
        private DateTime _dateCall;
        private DateTime _datePut;
        private DateTime _dateFirst;
        private DateTime _dateIssue;
        private Accrual _accrualBasis;
        private BondSecurityType _securityType;
        private string _countryCode;

        private static ArrayList _arBondMarketDefaults = new ArrayList();
        private static bool _bBondMarketDefaultsSetup = false;
    }
}
