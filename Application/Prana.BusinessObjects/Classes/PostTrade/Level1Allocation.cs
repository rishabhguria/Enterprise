using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class Level1Allocation
    {
        string _Level1Id = string.Empty;
        public virtual string AllocationId
        {
            set { _Level1Id = value; }
            get { return _Level1Id; }
        }

        string _groupID = string.Empty;
        public virtual string GroupID
        {
            set { _groupID = value; }
            get { return _groupID; }
        }

        double _orderQty = 0;
        public virtual double AllocatedQty
        {
            set
            {
                _orderQty = value;
            }
            get { return _orderQty; }
        }

        float _percentage = 100;
        public virtual float Percentage
        {
            set { _percentage = value; }
            get { return _percentage; }
        }

        int _AccountID = int.MinValue;
        public virtual int AccountID
        {
            set { _AccountID = value; }
            get { return _AccountID; }
        }

        private IList<TaxLot> _taxLotsH = new List<TaxLot>();
        [System.ComponentModel.Browsable(false)]
        [XmlIgnore]
        public virtual IList<TaxLot> TaxLotsH
        {
            get { return _taxLotsH; }
            set { _taxLotsH = value; }
        }

        public virtual string LevelnAllocationID
        {
            get { return _groupID + _Level1Id.ToString(); }
        }
        public override string ToString()
        {
            return "GroupID=" + _groupID + " LevelnID=" + _Level1Id + " QTy=" + _orderQty + " Percentage=" + _percentage;
        }
    }
}
