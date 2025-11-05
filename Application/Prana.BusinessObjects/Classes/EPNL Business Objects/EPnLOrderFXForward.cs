using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.LogManager;
using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class EPnLOrderFXForward : EPnLOrderFuture
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
        private string _expirationDate;
        private double _strikePrice;

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
        /// The counter currency cost basis pn l.
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

        public EPnLOrderFXForward()
        {
            _classID = EPnLClassID.EPnLOrderFXForward;
            _vsCurrencyID = 0;
            _leadCurrencyID = 0;
            _contractType = string.Empty;
            _expirationDate = string.Empty;
            _strikePrice = 0.0;
            _counterCurrencyID = 0;
            _counterCurrencyAmount = 0.0;
            _counterCurrencyCostBasisPnL = 0.0;
            _counterCurrencyDayPnL = 0.0;
        }

        public new EPnLOrderFXForward Clone()
        {
            EPnLOrderFXForward fxForwardOrder = new EPnLOrderFXForward();

            base.Clone(fxForwardOrder);
            fxForwardOrder.VsCurrencyID = this._vsCurrencyID;
            fxForwardOrder.LeadCurrencyID = this._leadCurrencyID;
            fxForwardOrder.ContractType = this._contractType;
            fxForwardOrder.ExpirationDate = this._expirationDate;
            fxForwardOrder.StrikePrice = this._strikePrice;
            fxForwardOrder.CounterCurrencyID = this._counterCurrencyID;
            fxForwardOrder.CounterCurrencyAmount = this._counterCurrencyAmount;
            fxForwardOrder.CounterCurrencyCostBasisPnL = this._counterCurrencyCostBasisPnL;
            fxForwardOrder.CounterCurrencyDayPnL = this._counterCurrencyDayPnL;
            return fxForwardOrder;
        }

        public override void GetBindableObject(ExposurePnlCacheItem bindableObject)
        {
            base.GetBindableObject(bindableObject);
            bindableObject.VsCurrencyID = this._vsCurrencyID;
            bindableObject.ContractType = this.Asset.ToString();
            bindableObject.StrikePrice = this._strikePrice;
            DateTime convertedDateTime;
            if (DateTime.TryParse(this.ExpirationDate, out convertedDateTime))
            {
                bindableObject.ExpirationDate = convertedDateTime;
            }
            else
                bindableObject.ExpirationDate = null;
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
                if (!isUnderlyingData)
                {
                    SecMasterFXForwardObj forwardObj = (SecMasterFXForwardObj)secMasterObject;
                    if (forwardObj != null)
                    {
                        _expirationDate = forwardObj.ExpirationDate.ToShortDateString();
                    }
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

        #region Overriden Methods to calculate Values using FX Rate
        public override void SetCostBasisUnrealizedPnlInBaseCurrency()
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