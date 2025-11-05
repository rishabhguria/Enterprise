using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.LogManager;
using System;


namespace Prana.BusinessObjects
{
    [Serializable]
    public class EPnLOrderFixedIncome : EPnlOrder
    {
        private EPnLClassID _classID;

        public override EPnLClassID ClassID
        {
            get { return this._classID; }
        }

        private double _delta;

        public double Delta
        {
            get { return _delta; }
            set { _delta = value; }
        }

        private string _contractType;

        public string ContractType
        {
            get { return _contractType; }
            set { _contractType = value; }
        }

        private double _accruedInterest;

        public double AccruedInterest
        {
            get { return _accruedInterest; }
            set { _accruedInterest = value; }
        }
        private string _expirationDate;

        public string ExpirationDate
        {
            get { return _expirationDate; }
            set { _expirationDate = value; }
        }

        public new EPnLOrderFixedIncome Clone()
        {
            EPnLOrderFixedIncome fixedIncomeOrder = new EPnLOrderFixedIncome();
            base.Clone(fixedIncomeOrder);
            fixedIncomeOrder.Delta = this._delta;
            fixedIncomeOrder.ContractType = this._contractType;
            fixedIncomeOrder.ExpirationDate = this._expirationDate;
            return fixedIncomeOrder;
        }

        public void Clone(EPnLOrderFixedIncome targetOrder)
        {
            base.Clone(targetOrder);
            ((EPnLOrderFixedIncome)targetOrder).Delta = this._delta;
            ((EPnLOrderFixedIncome)targetOrder).ContractType = this._contractType;
            ((EPnLOrderFixedIncome)targetOrder).ExpirationDate = this._expirationDate;
        }

        public EPnLOrderFixedIncome()
        {
            _classID = EPnLClassID.EPnLOrderFixedIncome;
            _delta = 1;
            _contractType = string.Empty;
        }

        public override void GetBindableObject(ExposurePnlCacheItem bindableObject)
        {
            base.GetBindableObject(bindableObject);
            bindableObject.Delta = this._delta;
            bindableObject.ContractType = this.Asset.ToString();
            if (!string.IsNullOrEmpty(this.ExpirationDate))
            {
                bindableObject.ExpirationDate = Convert.ToDateTime(this._expirationDate);
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
                    SecMasterFixedIncome fixedIncomeObj = (SecMasterFixedIncome)secMasterObject;
                    if (fixedIncomeObj != null)
                    {
                        _expirationDate = fixedIncomeObj.MaturityDate.ToShortDateString();
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
