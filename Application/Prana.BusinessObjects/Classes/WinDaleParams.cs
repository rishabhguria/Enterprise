namespace Prana.BusinessObjects
{
    public class WinDaleParams
    {
        private int _pricingModel;
        public int PricingModel
        {
            get { return _pricingModel; }
            set { _pricingModel = value; }
        }

        private int _binomialSteps;
        public int BinomialSteps
        {
            get { return _binomialSteps; }
            set { _binomialSteps = value; }
        }

        private int _volatilityIterations;
        public int VolatilityIterations
        {
            get { return _volatilityIterations; }
            set { _volatilityIterations = value; }
        }
    }
}
