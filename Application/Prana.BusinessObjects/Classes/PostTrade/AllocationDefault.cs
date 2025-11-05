using System;


namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for Default.
    /// </summary>

    [Serializable]
    public class AllocationDefault
    {
        private string _defaultName;
        public AllocationDefault()
        {
        }
        public AllocationDefault(string defaultname)
        {
            _defaultName = defaultname;
        }
        private int _defaultID = int.MinValue;
        public int DefaultID
        {
            get { return _defaultID; }
            set { _defaultID = value; }
        }
        public string DefaultName
        {
            set { _defaultName = value; }
            get { return _defaultName; }
        }
        private AllocationLevelList _defaultAllocation;
        public AllocationLevelList DefaultAllocationLevelList
        {
            get { return _defaultAllocation; }
            set { _defaultAllocation = value; }
        }
        private bool _isDefaultAllocationRule = true;

        public bool IsDefaultAllocationRule
        {
            get { return _isDefaultAllocationRule; }
            set { _isDefaultAllocationRule = value; }
        }

        public object Clone()
        {
            return Prana.Global.Utilities.DeepCopyHelper.Clone(this);
        }
    }
}
