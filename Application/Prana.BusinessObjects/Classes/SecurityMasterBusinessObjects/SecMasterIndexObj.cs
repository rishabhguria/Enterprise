using Prana.LogManager;
using System;


namespace Prana.BusinessObjects.SecurityMasterBusinessObjects
{
    [Serializable]
    public class SecMasterIndexObj : SecMasterBaseObj
    {
        // vommenting as same defination in base and derived
        // public string LongName
        //{
        //    get { return _longName; }
        //    set { _longName = value; }
        //}
        private string _shortName;

        public string ShortName
        {
            get { return _shortName; }
            set { _shortName = value; }
        }

        public override void FillData(object[] row, int offset)
        {


            base.FillData(row, 0);

            //SecMasterOptObj secMasterObj = new SecMasterOptObj();
            if (row != null)
            {



                try
                {



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

        }
        public override void FillUIData(SecMasterUIObj uiObj)
        {
            base.FillUIData(uiObj);
            _longName = uiObj.LongName;
            _delta = uiObj.Delta;
        }
    }
}
