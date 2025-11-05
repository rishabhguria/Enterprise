using Infragistics.Win.UltraWinGrid;
using System;

namespace Prana.ClientCommon
{
    [Serializable]
    public class SortedColumnData
    {
        public SortedColumnData()
        {
        }
        private string _key = String.Empty;
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }
        private SortIndicator _sortIndicator;
        public SortIndicator SortIndicator
        {
            get { return _sortIndicator; }
            set { _sortIndicator = value; }
        }
    }
}
