using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.LogManager;
using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class EPnLOrderFX : EPnlOrder
    {
        #region Properties
        private EPnLClassID _classID;
        public override EPnLClassID ClassID
        {
            get { return this._classID; }
        }

        private int _vsCurrencyID;
        public int VsCurrencyID
        {
            get { return _vsCurrencyID; }
            set { _vsCurrencyID = value; }
        }

        private int _leadCurrencyID;
        public int LeadCurrencyID
        {
            get { return _leadCurrencyID; }
            set { _leadCurrencyID = value; }
        }

        private string _contractType;
        public string ContractType
        {
            get { return _contractType; }
            set { _contractType = value; }
        }

        /// <summary>
        /// The counter currency identifier
        /// </summary>
        private int _counterCurrencyID;
        /// <summary>
        /// Gets or sets the counter currency identifier.
        /// </summary>
        /// <value>
        /// The counter currency identifier.
        /// </value>
        public int CounterCurrencyID
        {
            get { return _counterCurrencyID; }
            set { _counterCurrencyID = value; }
        }

        /// <summary>
        /// The counter currency amount
        /// </summary>
        private double _counterCurrencyAmount;
        /// <summary>
        /// Gets or sets the counter currency amount.
        /// </summary>
        /// <value>
        /// The counter currency amount.
        /// </value>
        public double CounterCurrencyAmount
        {
            get { return _counterCurrencyAmount; }
            set { _counterCurrencyAmount = value; }
        }

        /// <summary>
        /// The counter currency cost basis pnl
        /// </summary>
        private double _counterCurrencyCostBasisPnL;
        /// <summary>
        /// Gets or sets the counter currency cost basis pnl.
        /// </summary>
        /// <value>
        /// The counter currency cost basis pnl.
        /// </value>
        public double CounterCurrencyCostBasisPnL
        {
            get { return _counterCurrencyCostBasisPnL; }
            set { _counterCurrencyCostBasisPnL = value; }
        }

        /// <summary>
        /// The counter currency day pnl
        /// </summary>
        private double _counterCurrencyDayPnL;

        /// <summary>
        /// Gets or sets the counter currency day pnl.
        /// </summary>
        /// <value>
        /// The counter currency day pnl.
        /// </value>
        public double CounterCurrencyDayPnL
        {
            get { return _counterCurrencyDayPnL; }
            set { _counterCurrencyDayPnL = value; }
        }

        #endregion

        public EPnLOrderFX()
        {
            _classID = EPnLClassID.EPnLOrderFX;
            _vsCurrencyID = 0;
            _leadCurrencyID = 0;
            _contractType = string.Empty;
            _counterCurrencyID = 0;
            _counterCurrencyAmount = 0.0;
            _counterCurrencyCostBasisPnL = 0.0;
            _counterCurrencyDayPnL = 0.0;
        }

        public new EPnLOrderFX Clone()
        {
            EPnLOrderFX fxOrder = new EPnLOrderFX();

            base.Clone(fxOrder);
            fxOrder.VsCurrencyID = this._vsCurrencyID;
            fxOrder.LeadCurrencyID = this._leadCurrencyID;
            fxOrder.ContractType = this._contractType;
            fxOrder.CounterCurrencyID = this._counterCurrencyID;
            fxOrder.CounterCurrencyAmount = this._counterCurrencyAmount;
            fxOrder.CounterCurrencyCostBasisPnL = this._counterCurrencyCostBasisPnL;
            fxOrder.CounterCurrencyDayPnL = this._counterCurrencyDayPnL;
            return fxOrder;
        }

        public override void GetBindableObject(ExposurePnlCacheItem bindableObject)
        {
            base.GetBindableObject(bindableObject);
            bindableObject.VsCurrencyID = this._vsCurrencyID;
            bindableObject.ContractType = this.Asset.ToString();
            bindableObject.LeadCurrencyID = this._leadCurrencyID;
            bindableObject.CounterCurrencyID = this._counterCurrencyID;
            bindableObject.CounterCurrencyAmount = this._counterCurrencyAmount;
            bindableObject.CounterCurrencyCostBasisPnL = this._counterCurrencyCostBasisPnL;
            bindableObject.CounterCurrencyDayPnL = this._counterCurrencyDayPnL;
        }

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
                {
                    throw;
                }
            }
        }

        #region Overriden Methods to calculate Values using FX Rate
        public new void SetCostBasisUnrealizedPnlInBaseCurrency()
        {
        }

        public override void SetDayPnLInBaseCurrency()
        {
        }

        public override void SetMarketValueInBaseCurrency()
        {
        }
        public override void SetNetExposureInBaseCurrency()
        {
        }
        #endregion
    }
}
