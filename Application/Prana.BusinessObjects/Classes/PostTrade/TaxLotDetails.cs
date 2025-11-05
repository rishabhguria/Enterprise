using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class TaxLotDetails : TaxLot
    {



        private SecMasterOTCData _otcParameters;
        public SecMasterOTCData OTCParams
        {
            get { return _otcParameters; }
            set { _otcParameters = value; }
        }

        private SwapParameters _swapParameters;

        public TaxLotDetails()
        {
            _putOrCall = int.MinValue;
            _contractMultiplier = 1;
            _companyName = string.Empty;

        }

        // adding new keyword as initial value for _putOrCall is differnt in base and derived
        new public int PutOrCall
        {
            get { return _putOrCall; }
            set { _putOrCall = value; }
        }

        public SwapParameters SwapParams
        {
            get { return _swapParameters; }
            set { _swapParameters = value; }
        }


    }
}
