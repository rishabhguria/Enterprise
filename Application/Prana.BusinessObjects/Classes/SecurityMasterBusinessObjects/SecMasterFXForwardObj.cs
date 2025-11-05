using Prana.LogManager;
using System;

namespace Prana.BusinessObjects.SecurityMasterBusinessObjects
{
    [Serializable]
    public class SecMasterFXForwardObj : SecMasterFutObj
    {
        private int _leadCurrencyID;

        public int LeadCurrencyID
        {
            get { return _leadCurrencyID; }
            set { _leadCurrencyID = value; }
        }
        private int _vsCurrencyID;

        public int VsCurrencyID
        {
            get { return _vsCurrencyID; }
            set { _vsCurrencyID = value; }
        }
        private bool _isNDF;
        public bool IsNDF
        {
            get { return _isNDF; }
            set { _isNDF = value; }
        }

        private DateTime _fixingDate = DateTimeConstants.MinValue;
        public DateTime FixingDate
        {
            get { return _fixingDate; }
            set { _fixingDate = value; }
        }

        public override void FillUIData(SecMasterUIObj uiObj)
        {
            base.FillUIData(uiObj);
            _longName = uiObj.LongName;
            _leadCurrencyID = uiObj.LeadCurrencyID;
            _vsCurrencyID = uiObj.VsCurrencyID;
            _multiplier = uiObj.Multiplier;
            _isNDF = uiObj.IsNDF;
            _fixingDate = uiObj.FixingDate;
            _delta = uiObj.Delta;

        }

        public override void FillData(object[] row, int offset)
        {
            base.FillData(row, 23);
            if (row != null)
            {
                int LeadCurrency = offset + 0;
                int VsCurrency = offset + 1;
                int LongName = offset + 2;
                int ExpirationDate = offset + 3;
                int Multipler = offset + 4;
                int isNDF = offset + 19;
                int fixingDate = offset + 20;
                try
                {
                    _leadCurrencyID = int.Parse(row[LeadCurrency].ToString());
                    _vsCurrencyID = int.Parse(row[VsCurrency].ToString());
                    _longName = row[LongName].ToString();
                    _multiplier = double.Parse(row[Multipler].ToString());
                    base.ExpirationDate = Convert.ToDateTime(row[ExpirationDate].ToString());
                    _isNDF = Convert.ToBoolean(row[isNDF].ToString());
                    _fixingDate = Convert.ToDateTime(row[fixingDate].ToString());
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
