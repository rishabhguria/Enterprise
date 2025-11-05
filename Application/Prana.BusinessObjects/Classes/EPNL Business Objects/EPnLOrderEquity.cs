using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.LogManager;
using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class EPnLOrderEquity : EPnlOrder
    {
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

        public new EPnLOrderEquity Clone()
        {
            EPnLOrderEquity equityOrder = new EPnLOrderEquity();
            base.Clone(equityOrder);
            equityOrder.ContractType = this._contractType;
            return equityOrder;
        }

        public new void Clone(EPnlOrder targetOrder)
        {
            base.Clone(targetOrder);
            ((EPnLOrderEquity)targetOrder).ContractType = this._contractType;
        }

        public EPnLOrderEquity()
        {
            _classID = EPnLClassID.EPnLOrderEquity;
            _contractType = string.Empty;
        }

        public override void GetBindableObject(ExposurePnlCacheItem bindableObject)
        {
            base.GetBindableObject(bindableObject);
            bindableObject.ContractType = this.Asset.ToString();

        }
        /// <summary>
        /// update sec master details
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
                {
                    throw;
                }
            }
        }
    }
}