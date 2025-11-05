using Prana.ClientCommon;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Prana.Analytics
{
    [Serializable]
    public class StepAnalLayout
    {
        private List<ColumnData> _stepAnalysisColumns = new List<ColumnData>();
        [XmlArray("StepAnalysisColumns"), XmlArrayItem("ColumnData", typeof(ColumnData))]
        public List<ColumnData> StepAnalysisColumns
        {
            get { return _stepAnalysisColumns; }
            set { _stepAnalysisColumns = value; }
        }

        private List<SortedColumnData> _groupByColumns = new List<SortedColumnData>();
        [XmlArray("GroupByColumnsCollection"), XmlArrayItem("GroupByColumn", typeof(SortedColumnData))]
        public List<SortedColumnData> GroupByColumnsCollection
        {
            get { return _groupByColumns; }
            set { _groupByColumns = value; }
        }

        private int _splitterPosition = 50;
        public int SplitterPosition
        {
            get { return _splitterPosition; }
            set { _splitterPosition = value; }
        }

        private int _viewOrder = 0;
        public int viewOrder
        {
            get { return _viewOrder; }
            set { _viewOrder = value; }
        }

        private bool _isCheckedinStressTestCombo = true;
        public bool IsCheckedinStressTestCombo
        {
            get { return _isCheckedinStressTestCombo; }
            set { _isCheckedinStressTestCombo = value; }
        }
    }
}
