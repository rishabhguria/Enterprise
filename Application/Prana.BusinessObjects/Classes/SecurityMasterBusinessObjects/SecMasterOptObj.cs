using Prana.LogManager;
using System;

namespace Prana.BusinessObjects.SecurityMasterBusinessObjects
{
    [Serializable]
    public class SecMasterOptObj : SecMasterBaseObj
    {
        //private SecMasterCoreObject _secMasterCoreObject = null;
        int _settlementdate = int.MinValue;
        double _strikePirce = double.Epsilon;
        int _maturityMonth = int.MinValue;
        int _maturityDay = int.MinValue;

        DateTime _expirationDate = DateTimeConstants.MinValue;




        // string _type = string.Empty;

        # region properties

        public DateTime ExpirationDate
        {
            get { return _expirationDate; }
            set { _expirationDate = value; }
        }

        public int SettleMentDate
        {
            get { return _settlementdate; }
            set { _settlementdate = value; }
        }
        public int MaturityMonth
        {
            get { return _maturityMonth; }
            set { _maturityMonth = value; }

        }

        public int MaturityDay
        {
            get { return _maturityDay; }
            set { _maturityDay = value; }
        }

        public double StrikePrice
        {
            get { return _strikePirce; }
            set { _strikePirce = value; }
        }


        // commenting as same defination in base and derived
        //public string LongName
        //{
        //    get { return _longName; }
        //    set { _longName = value; }
        //}


        private int _putOrCall = int.MinValue;
        /// <summary>
        /// Same int as used in Prana.BusinessObjects.FIXConstants class
        /// </summary>
        public int PutOrCall
        {
            get { return _putOrCall; }
            set { _putOrCall = value; }
        }

        private bool _isCurrencyFuture;
        public bool IsCurrencyFuture
        {
            get { return _isCurrencyFuture; }
            set { _isCurrencyFuture = value; }
        }
        # endregion

        public override void FillData(object[] row, int offset)
        {


            base.FillData(row, 0);

            //SecMasterOptObj secMasterObj = new SecMasterOptObj();
            if (row != null)
            {
                // int MaturityMonth=offset+1;
                int Multiplier = offset + 0;
                int PutOrCall = offset + 1;
                int StrikePrice = offset + 2;

                int SettleMentDate = offset + 3;
                int Longname = offset + 4;
                int IsCurrencyFuture = offset + 62;


                try
                {

                    if (row[Multiplier] != System.DBNull.Value)
                    {
                        _multiplier = double.Parse(row[Multiplier].ToString());
                    }
                    if (row[PutOrCall] != System.DBNull.Value)
                    {
                        _putOrCall = Convert.ToInt32(row[PutOrCall].ToString());
                    }
                    if (row[StrikePrice] != System.DBNull.Value)
                    {
                        _strikePirce = Convert.ToDouble(row[StrikePrice].ToString());
                    }
                    if (row[SettleMentDate] != System.DBNull.Value)
                    {
                        // _settlementdate = int.Parse(row[SettleMentDate].ToString());
                        _expirationDate = Convert.ToDateTime(row[SettleMentDate].ToString());//new DateTime(Convert.ToInt32(row[MaturityMonth].ToString().Substring(0, 4)), Convert.ToInt32(row[MaturityMonth].ToString().Substring(4, 2)), Convert.ToInt32(row[SettleMentDate].ToString())); //,23,59,59);
                        _maturityDay = _expirationDate.Day;
                        _maturityMonth = GetMaturityMonth(_expirationDate);
                    }
                    // order.AllocatedQty = int.Parse(row[AllocatedQty].ToString());

                    if (row[Longname] != System.DBNull.Value)
                    {
                        _longName = row[Longname].ToString();
                    }

                    if (row[IsCurrencyFuture] != System.DBNull.Value)
                    {
                        _isCurrencyFuture = Convert.ToBoolean(row[IsCurrencyFuture].ToString());
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
        public override void FillData(SymbolData level1Data)
        {
            base.FillData(level1Data);
            OptionSymbolData opdata = level1Data as OptionSymbolData;
            if (opdata != null)
            {

                _strikePirce = ((OptionSymbolData)level1Data).StrikePrice;
                if (_strikePirce == double.MinValue)
                {
                    _strikePirce = 0.0;
                }
                //TODO : Check this out, why settlement is assigned by expirationdate.
                _settlementdate = opdata.ExpirationDate.Day;
                // _maturityMonth = ((OptionSymbolData)level1Data).ExpirationMonth;
                _maturityDay = opdata.ExpirationDate.Day;
                _putOrCall = (int)(opdata.PutOrCall);
                if (opdata.ExpirationDate != DateTimeConstants.MinValue)
                {
                    _expirationDate = opdata.ExpirationDate;
                    _maturityDay = _expirationDate.Day;
                    _maturityMonth = GetMaturityMonth(_expirationDate);
                    // DateTime.TryParse(((OptionSymbolData)level1Data).ExpirationMonth.ToString().Substring(4, 2) + "/" + ((OptionSymbolData)level1Data).ExpirationDate.ToString() + "/" + ((OptionSymbolData)level1Data).ExpirationMonth.ToString().Substring(0, 4), out _expirationDate);
                }

                if (opdata.Multiplier != 0)
                    _multiplier = opdata.Multiplier;

                if (level1Data.FullCompanyName != null)
                    _longName = level1Data.FullCompanyName;

            }
        }

        public static int GetMaturityMonth(DateTime expirationDate)
        {
            String strDate = expirationDate.ToString("yyyyMMdd");
            string maturityMonth = string.Empty;
            if (strDate.Length == 8)
            {
                maturityMonth = strDate.Substring(0, 6);
            }
            return Convert.ToInt32(maturityMonth);
        }

        public override void FillUIData(SecMasterUIObj uiObj)
        {
            base.FillUIData(uiObj);
            _strikePirce = uiObj.StrikePrice;
            //  _settlementdate = uiObj.SettleMentDate;
            //  _maturityMonth = uiObj.MaturityMonth;
            if (uiObj.PutOrCall == (int)AppConstants.OptionType.CALL)
            {
                _putOrCall = 1;
            }
            else
            {
                _putOrCall = 0;
            }


            //_multiplier = uiObj.Multiplier;
            if (uiObj.ExpirationDate != DateTimeConstants.MinValue)
            {
                _expirationDate = uiObj.ExpirationDate;
                _maturityDay = _expirationDate.Day;
                _maturityMonth = GetMaturityMonth(_expirationDate);
                // DateTime.TryParse(((OptionSymbolData)level1Data).ExpirationMonth.ToString().Substring(4, 2) + "/" + ((OptionSymbolData)level1Data).ExpirationDate.ToString() + "/" + ((OptionSymbolData)level1Data).ExpirationMonth.ToString().Substring(0, 4), out _expirationDate);
            }
            if (uiObj.LongName != null)
                _longName = uiObj.LongName;
            _delta = uiObj.Delta;
            _isCurrencyFuture = uiObj.IsCurrencyFuture;

        }
        public override string ToString()
        {
            return base.ToString() + " Maturity Month=" + _maturityMonth + "Multiplier = " + _multiplier + "Strike Price =" + _strikePirce + "SettleMent Date =" + _settlementdate + " Option Type=" + _putOrCall + " Long Name=" + _longName + " Currency Future =" + _isCurrencyFuture;
            //+ _secMasterCoreObject.ToString()
        }
        public override void UpDateData(object secMasterObj)
        {

            base.UpDateData(secMasterObj);
            SecMasterOptObj secMasterOptObj = (SecMasterOptObj)secMasterObj;

            _strikePirce = secMasterOptObj.StrikePrice;

            //if (_settlementdate == int.MinValue)
            //{
            //    _settlementdate = secMasterOptObj.SettleMentDate;
            //}
            //if (_maturityMonth == int.MinValue)
            //{
            //    _maturityMonth = secMasterOptObj.MaturityMonth;
            //}

            _longName = secMasterOptObj.LongName;

            _multiplier = secMasterOptObj.Multiplier;
            _expirationDate = secMasterOptObj.ExpirationDate;
            _isCurrencyFuture = secMasterOptObj.IsCurrencyFuture;
        }


    }
}
