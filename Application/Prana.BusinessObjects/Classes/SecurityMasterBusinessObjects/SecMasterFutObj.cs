using Prana.LogManager;
using System;
using System.Text;


namespace Prana.BusinessObjects.SecurityMasterBusinessObjects
{
    [Serializable]
    public class SecMasterFutObj : SecMasterBaseObj
    {
        //private SecMasterCoreObject _secMasterCoreObject = null;
        private DateTime _expirationDate = DateTimeConstants.MinValue;
        int _maturityMonth = int.MinValue;

        #region properties
        public DateTime ExpirationDate
        {
            get { return _expirationDate; }
            set { _expirationDate = value; }
        }
        public int MaturityMonth
        {
            get { return _maturityMonth; }
            set { _maturityMonth = value; }

        }
        // hiding using new as defination same in base and derived
        new public string LongName
        {
            get { return _longName; }
            set { _longName = value; }
        }

        private string _cutOffTime = string.Empty;
        /// <summary>
        /// Cut off time for the future root on the basis of which process date is determined. This is used only
        /// when _isFutureCutOffTimeUsed is true in Secmasterdatacache. This is further copied into the respective future
        /// sybmol's data.
        /// </summary>
        public string CutOffTime
        {
            get { return _cutOffTime; }
            set { _cutOffTime = value; }
        }

        private bool _isCurrencyFuture;
        public bool IsCurrencyFuture
        {
            get { return _isCurrencyFuture; }
            set { _isCurrencyFuture = value; }
        }

        #endregion 

        public override void FillData(object[] row, int offset)
        {
            //.SecMasterFutObj secMasterObj = new SecMasterFutObj();
            if (row != null)
            {
                int Multiplier = offset + 0;
                int Expirationdate = offset + 1;
                int LONGName = offset + 2;
                int CutOffTime = offset + 3;
                int IsCurrencyFuture = offset + 57;

                try
                {
                    base.FillData(row, 0);

                    if (row[Multiplier] != System.DBNull.Value)
                    {
                        _multiplier = double.Parse(row[Multiplier].ToString());
                    }
                    if (row[Expirationdate] != System.DBNull.Value && row[Expirationdate].ToString() != int.MinValue.ToString())
                    {
                        _expirationDate = Convert.ToDateTime(row[Expirationdate].ToString());
                        _maturityMonth = GetMaturityMonth(_expirationDate);
                    }

                    if (row[LONGName] != System.DBNull.Value)
                    {
                        _longName = row[LONGName].ToString();
                    }
                    if (row[CutOffTime] != System.DBNull.Value && row[CutOffTime] != null && row[CutOffTime].ToString() != String.Empty)
                    {
                        _cutOffTime = Convert.ToDateTime(row[CutOffTime].ToString()).ToString("HH:mm:ss");
                    }

                    if (row[IsCurrencyFuture] != System.DBNull.Value && row[IsCurrencyFuture] != null && row[IsCurrencyFuture].ToString() != String.Empty)
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
            try
            {
                base.FillData(level1Data);
                FutureSymbolData futuredata = level1Data as FutureSymbolData;
                if (futuredata != null)
                {

                    //_expirationDate = level1Data.ExpirationDate;
                    if (futuredata.ExpirationDate != DateTimeConstants.MinValue)
                    {
                        _maturityMonth = GetMaturityMonth(_expirationDate);
                        _expirationDate = futuredata.ExpirationDate;
                        // DateTime.TryParse(((OptionSymbolData)level1Data).ExpirationMonth.ToString().Substring(4, 2) + "/" + ((OptionSymbolData)level1Data).ExpirationDate.ToString() + "/" + ((OptionSymbolData)level1Data).ExpirationMonth.ToString().Substring(0, 4), out _expirationDate);
                        _multiplier = level1Data.Multiplier;
                    }
                    _longName = level1Data.FullCompanyName;
                    if (level1Data.CategoryCode == Prana.BusinessObjects.AppConstants.AssetCategory.FXForward)
                    {
                        _multiplier = 1;
                    }
                }
                //_secMasterCoreObject.FillData(level1Data);
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
        public override void FillUIData(SecMasterUIObj uiObj)
        {
            try
            {
                base.FillUIData(uiObj);
                _expirationDate = uiObj.ExpirationDate;
                _maturityMonth = GetMaturityMonth(_expirationDate);
                _longName = uiObj.LongName;
                _delta = uiObj.Delta;
                _isCurrencyFuture = uiObj.IsCurrencyFuture;
                // _multiplier = uiObj.Multiplier;
                //_secMasterCoreObject.FillData(level1Data);
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
        public override void UpDateData(object secMasterObj)
        {
            try
            {
                // base.UpDateData(secMasterObj);
                SecMasterFutObj secMasterFutObj = (SecMasterFutObj)secMasterObj;
                //if(_expirationDate==int.MinValue)
                //_expirationDate = secMasterFutObj.Expirationdate;
                _multiplier = secMasterFutObj.Multiplier;
                _expirationDate = secMasterFutObj.ExpirationDate;
                _maturityMonth = GetMaturityMonth(_expirationDate);
                _longName = secMasterFutObj.LongName;
                _isCurrencyFuture = secMasterFutObj.IsCurrencyFuture;
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
        private int GetMaturityMonth(DateTime expirationDate)
        {
            try
            {
                String strDate = expirationDate.ToString("yyyyMMdd");
                string maturityMonth = string.Empty;
                if (strDate.Length == 8)
                {
                    maturityMonth = strDate.Substring(0, 6);
                }
                return Convert.ToInt32(maturityMonth);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return 0;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.Append(base.ToString());
                if (_longName != null)
                {
                    sb.Append(" Long Name=" + _longName.ToString());
                }

                sb.Append(" ExpirationDate=" + _expirationDate.ToString());
                sb.Append(" Multiplier=" + _multiplier.ToString());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return sb.ToString();
        }

    }
}
