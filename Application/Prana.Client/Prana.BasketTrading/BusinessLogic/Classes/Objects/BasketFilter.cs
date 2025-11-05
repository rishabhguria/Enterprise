using System;
using System.Collections;
using System.Text;


namespace Prana.BasketTrading
{
    [Serializable]
    public class BasketFilter
    {
        private string _FilterID = string.Empty;
        private int _filterTypeID = int.MinValue;
        private int _benchmarkID = int.MinValue;
        private int _operatorID = int.MinValue;
        private float _percentage = float.MinValue;
        
        public string FilterID
        {
            set { _FilterID = value; }
            get { return _FilterID; }
        }

        public int FilterTypeID
        {
            set { _filterTypeID = value; }
            get { return _filterTypeID; }
        }
        public int BenchmarkID
        {
            set { _benchmarkID = value; }
            get { return _benchmarkID; }
        }
        public int OperatorID
        {
            set { _operatorID = value; }
            get { return _operatorID; }
        }
        public float Percentage
        {
            set { _percentage = value; }
            get { return _percentage; }
        }

    }

    }
