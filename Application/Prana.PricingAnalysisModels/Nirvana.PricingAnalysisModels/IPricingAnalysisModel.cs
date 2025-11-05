namespace Prana.PricingAnalysisModels
{
    public interface IPricingAnalysisModel
    {
        #region Set/Get CurrentPricingAnalysisModel

        PricingAnalysisModelsEnum CurrentPricingAnalysisModel
        {
            get;
            set;
        }

        #endregion

        int BinomialSteps
        {
            get;
            set;
        }

        #region Pricing Greeks
        /// <summary>
        /// Computes the call price for the option using the values supplied to the method. 
        /// </summary>
        /// <param name="stockPrice">Double value supplying the current stock price. </param>
        /// <param name="exercisePrice">Double value supplying the exercise price for the stock</param>
        /// <param name="interestRate">Double value supplying the current risk-free interest rate [0.0 - 1.0]. </param>
        /// <param name="timeToMaturity">Double value supplying the number of years until the option expires. </param>
        /// <param name="volatility">Double value supplying the volatility of the stock [0.0 - 1.0]. </param>
        /// <param name="dividend">Double value supplying the dividend rate of the stock [0.0 - 1.0]. </param>
        /// <returns></returns>
        double GetCallPrice(double stockPrice, double exercisePrice, double interestRate, double timeToMaturity, double volatility, double dividend);
        double GetPutPrice(double stockPrice, double exercisePrice, double interestRate, double timeToMaturity, double volatility, double dividend);
        void GetDelta(double stockPrice, double exercisePrice, double interestRate, double timeToMaturity, double volatility, double dividend, ref double cDelta, ref double pDelta);
        void GetTheta(double stockPrice, double exercisePrice, double interestRate, double timeToMaturity, double volatility, double dividend, ref double cTheta, ref double pTheta);
        void GetGamma(double stockPrice, double exercisePrice, double interestRate, double timeToMaturity, double volatility, double dividend, ref double gamma);
        void GetVega(double stockPrice, double exercisePrice, double interestRate, double timeToMaturity, double volatility, double dividend, ref double vega);
        void GetRho(double stockPrice, double exercisePrice, double interestRate, double timeToMaturity, double volatility, double dividend, ref double cRho, ref double pRho);

        #endregion #region Pricing Greeks

        #region Implied Volitility

        /// <summary>
        /// Computes the Implied Volatility using the current model
        /// </summary>
        /// <param name="StockPrice">Double value supplying the current stock price. </param>
        /// <param name="ExercisePrice">Double value supplying the exercise price for the stock. </param>
        /// <param name="InterestRate">Double value supplying the current risk-free interest rate [0.0 - 1.0]. </param>
        /// <param name="TimeToMaturity">Double value supplying the number of years until the option expires. </param>
        /// <param name="Dividend">Double value supplying the dividend rate of the stock [0.0 - 1.0]. </param>
        /// <param name="CallPriceIV">Double value supplying the option Call value used to compute the implied volatility. </param>
        /// <param name="MaxIteration">Integer value setting the maximum number of iterations used to compute the implied volatility.</param>
        double GetImpliedVolatility(double StockPrice, double ExercisePrice, double InterestRate, double TimeToMaturity, double Dividend, double CallPriceIV, int MaxIteration);
        double GetImpliedVolatilityPut(double StockPrice, double ExercisePrice, double InterestRate, double TimeToMaturity, double Dividend, double PutPriceIV, int MaxIteration);

        #endregion

    }
}
