namespace Prana.BusinessObjects
{
    public struct KeyReturns
    {
        public KeyReturns(double mtdPnL, double qtdPnL, double ytdPnL, double mtdReturn, double qtdReturn, double ytdReturn)
        {
            _mtdPnL = mtdPnL;
            _qtdPnL = qtdPnL;
            _ytdPnL = ytdPnL;
            _mtdReturn = mtdReturn;
            _qtdReturn = qtdReturn;
            _ytdReturn = ytdReturn;
        }

        private double _mtdReturn;
        public double MTDReturn
        {
            get { return _mtdReturn; }
            set { _mtdReturn = value; }
        }

        private double _qtdReturn;
        public double QTDReturn
        {
            get { return _qtdReturn; }
            set { _qtdReturn = value; }
        }

        private double _ytdReturn;
        public double YTDReturn
        {
            get { return _ytdReturn; }
            set { _ytdReturn = value; }
        }

        private double _mtdPnL;
        public double MTDPnL
        {
            get { return _mtdPnL; }
            set { _mtdPnL = value; }
        }

        private double _qtdPnL;
        public double QTDPnL
        {
            get { return _qtdPnL; }
            set { _qtdPnL = value; }
        }

        private double _ytdPnL;
        public double YTDPnL
        {
            get { return _ytdPnL; }
            set { _ytdPnL = value; }
        }
    }
}