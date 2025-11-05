using System;
using System.ComponentModel;
using System.Data;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class AllocationLevelClass
    {
        //[NonSerialized]
        AllocationLevelList _childs = null;
        int _ID = int.MinValue;
        string _name = string.Empty;
        double _orderQty = 0;
        float _percentage = 100;
        string _groupID = string.Empty;
        decimal _targetPercentage = 0;

        public AllocationLevelClass(string groupID)
        {
            _groupID = groupID;
        }

        public AllocationLevelClass()
        {

        }

        public AllocationLevelClass(string groupID, DataRow dr)
        {
            _groupID = groupID;
            if (!string.IsNullOrEmpty(dr["L1AllocatedQty"].ToString()))
                _orderQty = Convert.ToDouble(dr["L1AllocatedQty"]);
            if (!string.IsNullOrEmpty(dr["L1FundID"].ToString()))
                _ID = Convert.ToInt32(dr["L1FundID"]);
            if (!string.IsNullOrEmpty(dr["L1Percentage"].ToString()))
                _percentage = float.Parse(dr["L1Percentage"].ToString());
        }

        public virtual string Name
        {
            set { _name = value; }
            get { return _name; }
        }

        [Browsable(false)]
        public virtual int LevelnID
        {
            set { _ID = value; }
            get { return _ID; }
        }

        public virtual float Percentage
        {
            set { _percentage = value; }
            get { return _percentage; }
        }

        [Browsable(false)]
        public virtual decimal TargetPercentage
        {
            set { _targetPercentage = value; }
            get { return _targetPercentage; }
        }

        public virtual double AllocatedQty
        {
            set
            {
                _orderQty = value;
            }
            get { return _orderQty; }
        }

        [Browsable(false)]
        public virtual string GroupID
        {
            set { _groupID = value; }
            get { return _groupID; }
        }

        [Browsable(false)]
        public virtual string LevelnAllocationID
        {
            get { return _groupID + _ID.ToString(); }
        }

        public AllocationLevelList Childs
        {
            get
            {
                if (_childs != null)
                    return _childs;
                else
                    return null;
            }

        }

        public void AddChilds(AllocationLevelClass child)
        {
            if (_childs == null)
            {
                _childs = new AllocationLevelList();
            }
            _childs.Add(child);
        }



        public object Clone()
        {
            return Prana.Global.Utilities.DeepCopyHelper.Clone(this);
        }

    }
}
