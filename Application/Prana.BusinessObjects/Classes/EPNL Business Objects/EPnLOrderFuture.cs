using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.LogManager;
using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class EPnLOrderFuture : EPnlOrder
    {
        #region Properties
        private EPnLClassID _classID;
        public override EPnLClassID ClassID
        {
            get { return this._classID; }
        }

        private string _contractType;
        public string ContractType
        {
            get { return _contractType; }
            set { _contractType = value; }
        }

        private string _expirationDate;
        public string ExpirationDate
        {
            get { return _expirationDate; }
            set { _expirationDate = value; }
        }

        private double _strikePrice;
        public double StrikePrice
        {
            get { return _strikePrice; }
            set { _strikePrice = value; }
        }

        private bool _isCurrencyFuture;
        public bool IsCurrencyFuture
        {
            get { return _isCurrencyFuture; }
            set { _isCurrencyFuture = value; }
        }
        #endregion

        public EPnLOrderFuture()
        {
            _classID = EPnLClassID.EPnLOrderFuture;
            _contractType = string.Empty;
            _expirationDate = string.Empty;
            _strikePrice = 0.0;
            _isCurrencyFuture = false;
        }

        public new EPnLOrderFuture Clone()
        {
            EPnLOrderFuture futureOrder = new EPnLOrderFuture();
            base.Clone(futureOrder);
            futureOrder.ContractType = this._contractType;
            futureOrder.StrikePrice = this._strikePrice;
            futureOrder.ExpirationDate = this._expirationDate;
            futureOrder.IsCurrencyFuture = this._isCurrencyFuture;
            return futureOrder;
        }

        public override void GetBindableObject(ExposurePnlCacheItem bindableObject)
        {
            base.GetBindableObject(bindableObject);
            bindableObject.ContractType = this.Asset.ToString();
            bindableObject.StrikePrice = this._strikePrice;
            DateTime convertedDateTime;
            if (DateTime.TryParse(this.ExpirationDate, out convertedDateTime))
            {
                bindableObject.ExpirationDate = convertedDateTime;
            }
            else
                bindableObject.ExpirationDate = null;
        }

        public override void CopyBasicDetails(SecMasterBaseObj secMasterObject, bool isUnderlyingData)
        {
            try
            {
                base.CopyBasicDetails(secMasterObject, isUnderlyingData);
                if (!isUnderlyingData)
                {
                    SecMasterFutObj futObj = (SecMasterFutObj)secMasterObject;
                    if (futObj != null)
                    {
                        _expirationDate = futObj.ExpirationDate.ToShortDateString();
                        _isCurrencyFuture = futObj.IsCurrencyFuture;
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
    }
}