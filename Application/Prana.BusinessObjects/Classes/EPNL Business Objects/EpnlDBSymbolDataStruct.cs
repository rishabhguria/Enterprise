namespace Prana.BusinessObjects
{
    public struct EpnlDBSymbolDataStruct
    {
        private double _beta;

        public double Beta
        {
            get { return _beta; }
            set { _beta = value; }
        }

        private double _sharesOutstanding;

        public double SharesOutstanding
        {
            get { return _sharesOutstanding; }
            set { _sharesOutstanding = value; }
        }

        // mark prices and indictors to be added to this and the data need to be available on runtime


    }
}
