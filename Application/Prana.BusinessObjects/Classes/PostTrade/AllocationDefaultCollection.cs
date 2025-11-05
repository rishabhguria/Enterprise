using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
namespace Prana.BusinessObjects
{
    public class AllocationDefaultCollection
    {
        Dictionary<int, AllocationDefault> _defaults = new Dictionary<int, AllocationDefault>();
        List<int> removedDefaults = new List<int>();
        public void Add(AllocationDefault allocationDefault)
        {
            if (!_defaults.ContainsKey(allocationDefault.DefaultID))
            {
                _defaults.Add(allocationDefault.DefaultID, allocationDefault);
            }
        }
        public void Remove(int id)
        {
            if (_defaults.ContainsKey(id))
            {
                _defaults.Remove(id);
                removedDefaults.Add(id);
            }
        }
        public List<AllocationDefault> GetDefaults()
        {
            List<AllocationDefault> list = new List<AllocationDefault>();
            foreach (KeyValuePair<int, AllocationDefault> item in _defaults)
            {
                if (item.Key != int.MinValue)
                {
                    list.Add(item.Value);
                }
            }
            return list;
        }
        public List<int> GetRemovedDefaults()
        {
            return removedDefaults;
        }
        public void SetDefaults(List<AllocationDefault> allocationDefaultList)
        {
            try
            {
                _defaults.Clear();
                removedDefaults.Clear();
                AllocationDefault allocationDefaultSelect = new AllocationDefault();
                allocationDefaultSelect.DefaultName = Prana.Global.ApplicationConstants.C_COMBO_SELECT;
                allocationDefaultSelect.DefaultID = int.MinValue;
                _defaults.Add(allocationDefaultSelect.DefaultID, allocationDefaultSelect);
                foreach (AllocationDefault allocationDefault in allocationDefaultList)
                {
                    _defaults.Add(allocationDefault.DefaultID, allocationDefault);
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
        public AllocationDefault GetDefault(int id)
        {
            try
            {
                return _defaults[id];
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        public bool DoesDefaultExist(int id)
        {
            return _defaults.ContainsKey(id);
        }
        public DataTable GetDefaultsDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("Name");
            foreach (KeyValuePair<int, AllocationDefault> item in _defaults)
            {
                dt.Rows.Add(new object[] { item.Value.DefaultID, item.Value.DefaultName });
            }
            return dt;

        }

    }
}
