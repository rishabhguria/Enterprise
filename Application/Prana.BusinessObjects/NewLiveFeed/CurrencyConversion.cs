namespace Prana.BusinessObjects.NewLiveFeed
{
    public class CurrencyConversions
    {
        private string higherCurrency;
        private string operatorToApply;
        private double adjustValue;

        public string HigherCurrency
        {
            get { return higherCurrency; }
            set { higherCurrency = value; }
        }
        public string OperatorToApply
        {
            get { return operatorToApply; }
            set { operatorToApply = value; }
        }
        public double AdjustValue
        {
            get
            {
                if (operatorToApply == "M")
                {
                    return 1 / adjustValue;
                }
                else
                {
                    return adjustValue;
                }
            }
            set { adjustValue = value; }
        }
    }
}
