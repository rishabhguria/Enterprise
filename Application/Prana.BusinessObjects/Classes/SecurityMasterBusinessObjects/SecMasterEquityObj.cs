using Prana.LogManager;
using System;

namespace Prana.BusinessObjects.SecurityMasterBusinessObjects
{
    [Serializable]
    public class SecMasterEquityObj : SecMasterBaseObj
    {

        public override void FillData(object[] row, int offset)
        {
            base.FillData(row, 0);
            if (row != null)
            {
                int delta = offset + 0;
                int multiplier = offset + 18;
                try
                {
                    if (row[delta] != System.DBNull.Value)
                    {
                        //modified by omshiv, parse to float as DB cloumn type is float, on parsing to int it gives error for float values of delta 
                        _delta = float.Parse(row[delta].ToString());
                    }
                    if (row[multiplier] != System.DBNull.Value)
                    {
                        _multiplier = double.Parse(row[multiplier].ToString());
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
            _divDistributionDate = level1Data.DivDistributionDate;
            _dividend = (float)(level1Data.Dividend);
            _dividendAmtRate = level1Data.DividendAmtRate;
            _dividendYield = level1Data.DividendYield;

            base.FillData(level1Data);
            _multiplier = 1;
        }
        public override void FillUIData(SecMasterUIObj uiObj)
        {
            base.FillUIData(uiObj);
            _delta = uiObj.Delta;
            _multiplier = uiObj.Multiplier;
        }
        public override void UpDateData(object secMasterObj)
        {

            base.UpDateData(secMasterObj);
            SecMasterEquityObj secMasterEquityObj = (SecMasterEquityObj)secMasterObj;
            _delta = secMasterEquityObj.Delta;

        }


        public override string ToString()
        {
            return base.ToString() + " Delta=" + _delta;// +_secMasterCoreObject.ToString();
        }

        private double _dividendYield;

        public double DividendYield
        {
            get { return _dividendYield; }
            set { _dividendYield = value; }
        }
        private float _dividend;

        public float Dividend
        {
            get { return _dividend; }
            set { _dividend = value; }
        }
        private float _dividendAmtRate;

        public float DividendAmtRate
        {
            get { return _dividendAmtRate; }
            set { _dividendAmtRate = value; }
        }
        private DateTime _divDistributionDate;

        public DateTime DivDistributionDate
        {
            get { return _divDistributionDate; }
            set { _divDistributionDate = value; }
        }

    }
}
