using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.LogManager;
using System;



namespace Prana.BusinessObjects
{
    [Serializable]
    public class EPnLOrderEquitySwap : EPnLOrderEquity
    {
        #region Properties
        private EPnLClassID _classID;
        public override EPnLClassID ClassID
        {
            get { return this._classID; }
        }

        private SwapParameters _swapParameters;
        public SwapParameters SwapParameters
        {
            get { return _swapParameters; }
            set { _swapParameters = value; }
        }

        private double _dayInterest;
        public double DayInterest
        {
            get { return _dayInterest; }
            set { _dayInterest = value; }
        }

        private double _totalInterest;
        public double TotalInterest
        {
            get { return _totalInterest; }
            set { _totalInterest = value; }
        }

        private string _contractType;

        // commenting as base and derived have same defination value and no initial value
        //public string ContractType
        //{
        //    get { return _contractType; }
        //    set { _contractType = value; }
        //}
        #endregion 

        public EPnLOrderEquitySwap()
        {
            _classID = EPnLClassID.EPnLOrderEquitySwap;
            _swapParameters = new SwapParameters();
            _dayInterest = 0.0;
            _totalInterest = 0.0;
            _contractType = string.Empty;
        }

        public new EPnLOrderEquitySwap Clone()
        {
            EPnLOrderEquitySwap equitySwapOrder = new EPnLOrderEquitySwap();
            base.Clone(equitySwapOrder);
            equitySwapOrder.SwapParameters = this._swapParameters.Clone();
            equitySwapOrder.DayInterest = this._dayInterest;
            equitySwapOrder.TotalInterest = this._totalInterest;
            equitySwapOrder.ContractType = this._contractType;
            return equitySwapOrder;
        }

        public override void GetBindableObject(ExposurePnlCacheItem bindableObject)
        {
            base.GetBindableObject(bindableObject);
            bindableObject.DayInterest = this._dayInterest;
            bindableObject.TotalInterest = this._totalInterest;
            bindableObject.ContractType = this.Asset.ToString();
        }

        /// <summary>
        /// Update sec master details
        /// </summary>
        /// <param name="secMasterObject"></param>
        /// <param name="isUnderlyingData"></param>
        public override void CopyBasicDetails(SecMasterBaseObj secMasterObject, bool isUnderlyingData)
        {
            try
            {
                base.CopyBasicDetails(secMasterObject, isUnderlyingData);

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
    }
}
