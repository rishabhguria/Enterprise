using Infragistics.Win.UltraWinGrid;
using System;
using System.Collections.Generic;

namespace Prana.PM.BLL
{
    [Serializable]
    public class GridColumnFilterDetails : IComparable<GridColumnFilterDetails>, IDisposable
    {
        public GridColumnFilterDetails()
        { }

        public GridColumnFilterDetails(string key)
        {
            _filterColumnKey = key;
        }

        public GridColumnFilterDetails(string key, List<FilterCondition> filterconditionList)
        {
            _filterColumnKey = key;
            _dynamicFilterConditionList = filterconditionList;
        }


        private string _filterColumnKey;
        public string FilterColumnKey
        {
            get { return _filterColumnKey; }
            set { _filterColumnKey = value; }
        }

        List<FilterCondition> _dynamicFilterConditionList = new List<FilterCondition>();
        public List<FilterCondition> DynamicFilterConditionList
        {
            get
            {
                return _dynamicFilterConditionList;
            }
            set
            {
                _dynamicFilterConditionList = value;
            }
        }

        #region IDisposable Members
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _filterColumnKey = null;
                _dynamicFilterConditionList = null;
            }
        }
        #endregion

        public int CompareTo(GridColumnFilterDetails other)
        {
            return String.Compare(_filterColumnKey, other._filterColumnKey, StringComparison.Ordinal);
        }
    }
}
