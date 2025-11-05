using Prana.LogManager;
using Prana.PricingAnalysisModels;
using System;
using Windale.Options;

namespace Prana.OptionCalculator.CalculationComponent
{
    class WinDalePricingAnalysisAdapter : IPricingAnalysisModel, IDisposable
    {
        #region Private variables
        //TODO : To create multiple instance of OptionNET and do the object pooling
        private OptionsNET OptionsNet1 = new OptionsNET();
        #endregion

        #region IPricingAnalysisModel Members
        PricingAnalysisModelsEnum _currentPricingAnalysisModel = PricingAnalysisModelsEnum.None;

        /// <summary>
        /// This is the critical property to set before calling any of the function
        /// </summary>
        PricingAnalysisModelsEnum IPricingAnalysisModel.CurrentPricingAnalysisModel
        {
            get
            {
                return _currentPricingAnalysisModel;
            }
            set
            {
                if (value == PricingAnalysisModelsEnum.None)
                {
                    ///Raise the exception by giving user a message to set CurrentPricingAnalysisModel property first before calling any method
                    ///and return from here itself
                    throw (new ArgumentException("Value of PricingAnalysisModelsEnum can not be set to 'None'."));
                }
                _currentPricingAnalysisModel = value;
            }
        }

        int _binomialSteps = 10;
        /// <summary>
        /// This is the critical property to set before calling any of the function
        /// </summary>
        int IPricingAnalysisModel.BinomialSteps
        {
            get
            {
                return _binomialSteps;
            }
            set
            {
                if (value == 0)
                {
                    ///Raise the exception by giving user a message to set CurrentPricingAnalysisModel property first before calling any method
                    ///and return from here itself
                    throw (new ArgumentException("Value of BinomialSteps can not be set to '0'."));
                }
                _binomialSteps = value;
            }
        }

        /// <summary>
        /// Used name hiding
        /// TODO : Make it Asynchronous & Apply strategy pattern
        /// </summary>
        public double GetCallPrice(double StockPrice, double ExercisePrice, double InterestRate, double TimeToMaturity, double Volatility, double Dividend)
        {
            try
            {
                if (_currentPricingAnalysisModel == PricingAnalysisModelsEnum.None)
                {
                    ///Raise the exception by giving user a message to set CurrentPricingAnalysisModel property first before calling any method
                    ///and return from here itself
                    throw (new ArgumentException("Value of PricingAnalysisModelsEnum must be set before calculating any value."));
                }
                if (StockPrice == double.MinValue || ExercisePrice == double.MinValue || InterestRate == double.MinValue || TimeToMaturity == double.MinValue || Volatility == double.MinValue || Dividend == double.MinValue)
                    throw (new ArgumentException("Set the value of the arguments."));

                switch (_currentPricingAnalysisModel)
                {
                    case PricingAnalysisModelsEnum.Black_Scholes:
                        return OptionsNet1.BSCallPrice(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Volatility, Dividend);
                    case PricingAnalysisModelsEnum.AmericanBinomialSteps:
                        double CPrice = 0.0;
                        OptionsNet1.BinCallPrice(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Volatility, Dividend, Convert.ToInt32(Convert.ToInt32(ModelType.StockWithContinuousDividend)), 0, _binomialSteps, Convert.ToInt32(Convert.ToInt32(FlagEurAm.American)), ref CPrice);
                        return CPrice;
                    case PricingAnalysisModelsEnum.Bjerksund_Stensland:
                        return OptionsNet1.BJECallPrice(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Volatility, Dividend, Convert.ToInt32(Convert.ToInt32(ModelType.StockWithContinuousDividend)), 0);
                    case PricingAnalysisModelsEnum.Barone_Adesi_Whaley:
                        return OptionsNet1.BAWCallPrice(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Volatility, Dividend, Convert.ToInt32(Convert.ToInt32(ModelType.StockWithContinuousDividend)), 0);
                    case PricingAnalysisModelsEnum.Black76:
                        StockPrice = StockPrice * Math.Exp((-1) * TimeToMaturity * InterestRate);
                        return OptionsNet1.BSCallPrice(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Volatility, Dividend);
                    default:
                        return 0;
                }
            }
            catch (Exception ex)
            {
                Logger.LoggerWrite(ex.ToString());
            }
            return 0;
        }

        /// <summary>
        /// Used name hiding
        /// TODO : Make it Asynchronous & Apply strategy pattern
        /// </summary>
        public double GetPutPrice(double StockPrice, double ExercisePrice, double InterestRate, double TimeToMaturity, double Volatility, double Dividend)
        {
            try
            {
                if (_currentPricingAnalysisModel == PricingAnalysisModelsEnum.None)
                {
                    ///Raise the exception by giving user a message to set CurrentPricingAnalysisModel property first before calling any method
                    ///and return from here itself
                    throw (new ArgumentException("Value of PricingAnalysisModelsEnum must be set before calculating any value."));
                }
                if (StockPrice == double.MinValue || ExercisePrice == double.MinValue || InterestRate == double.MinValue || TimeToMaturity == double.MinValue || Volatility == double.MinValue || Dividend == double.MinValue)
                    throw (new ArgumentException("Set the value of the arguments."));

                switch (_currentPricingAnalysisModel)
                {
                    case PricingAnalysisModelsEnum.Black_Scholes:
                        return OptionsNet1.BSPutPrice(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Volatility, Dividend);
                    case PricingAnalysisModelsEnum.AmericanBinomialSteps:
                        double PPrice = 0.0;
                        OptionsNet1.BinPutPrice(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Volatility, Dividend, Convert.ToInt32(ModelType.StockWithContinuousDividend), 0, _binomialSteps, Convert.ToInt32(FlagEurAm.American), ref PPrice);
                        return PPrice;
                    case PricingAnalysisModelsEnum.Bjerksund_Stensland:
                        return OptionsNet1.BJEPutPrice(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Volatility, Dividend, Convert.ToInt32(ModelType.StockWithContinuousDividend), 0);
                    case PricingAnalysisModelsEnum.Barone_Adesi_Whaley:
                        return OptionsNet1.BAWPutPrice(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Volatility, Dividend, Convert.ToInt32(ModelType.StockWithContinuousDividend), 0);
                    case PricingAnalysisModelsEnum.Black76:
                        StockPrice = StockPrice * Math.Exp((-1) * TimeToMaturity * InterestRate);
                        return OptionsNet1.BSPutPrice(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Volatility, Dividend);
                    default:
                        return 0;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                Logger.LoggerWrite(ex.ToString());
            }
            return 0;
        }

        /// <summary>
        /// Used name hiding
        /// TODO : Make it Asynchronous & Apply strategy pattern
        /// </summary>
        public void GetDelta(double StockPrice, double ExercisePrice, double InterestRate, double TimeToMaturity, double Volatility, double Dividend, ref double CDelta, ref double PDelta)
        {
            try
            {
                if (_currentPricingAnalysisModel == PricingAnalysisModelsEnum.None)
                {
                    ///Raise the exception by giving user a message to set CurrentPricingAnalysisModel property first before calling any method
                    ///and return from here itself
                    throw (new ArgumentException("Value of PricingAnalysisModelsEnum must be set before calculating any value."));
                }

                if (StockPrice == double.MinValue || StockPrice == 0 || ExercisePrice == double.MinValue || ExercisePrice == 0 || InterestRate == double.MinValue || InterestRate == 0 || TimeToMaturity == double.MinValue || TimeToMaturity == 0 || Dividend == double.MinValue || Volatility == double.MinValue || Volatility == 0)
                    return;

                switch (_currentPricingAnalysisModel)
                {
                    case PricingAnalysisModelsEnum.Black_Scholes:
                        OptionsNet1.BSDelta(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Volatility, Dividend, ref CDelta, ref PDelta);
                        break;
                    case PricingAnalysisModelsEnum.AmericanBinomialSteps:
                        OptionsNet1.BinDelta(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Volatility, Dividend, Convert.ToInt32(ModelType.StockWithContinuousDividend), 0, _binomialSteps, Convert.ToInt32(FlagEurAm.American), ref CDelta, ref PDelta);
                        break;
                    case PricingAnalysisModelsEnum.Bjerksund_Stensland:
                        OptionsNet1.BJEDelta(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Volatility, Dividend, Convert.ToInt32(ModelType.StockWithContinuousDividend), 0, ref CDelta, ref PDelta);
                        break;
                    case PricingAnalysisModelsEnum.Barone_Adesi_Whaley:
                        OptionsNet1.BAWDelta(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Volatility, Dividend, Convert.ToInt32(ModelType.StockWithContinuousDividend), 0, ref CDelta, ref PDelta);
                        break;
                    case PricingAnalysisModelsEnum.Black76:
                        StockPrice = StockPrice * Math.Exp((-1) * TimeToMaturity * InterestRate);
                        OptionsNet1.BSDelta(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Volatility, Dividend, ref CDelta, ref PDelta);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.LoggerWrite(ex.ToString());
            }
        }

        /// <summary>
        /// Used name hiding
        /// TODO : Make it Asynchronous & Apply strategy pattern
        /// </summary>
        public void GetTheta(double StockPrice, double ExercisePrice, double InterestRate, double TimeToMaturity, double Volatility, double Dividend, ref double CTheta, ref double PTheta)
        {
            try
            {
                if (_currentPricingAnalysisModel == PricingAnalysisModelsEnum.None)
                {
                    ///Raise the exception by giving user a message to set CurrentPricingAnalysisModel property first before calling any method
                    ///and return from here itself
                    throw (new ArgumentException("Value of PricingAnalysisModelsEnum must be set before calculating any value."));
                }
                if (StockPrice == double.MinValue || ExercisePrice == double.MinValue || InterestRate == double.MinValue || TimeToMaturity == double.MinValue || Volatility == double.MinValue || Dividend == double.MinValue)
                    throw (new ArgumentException("Set the value of the arguments."));

                switch (_currentPricingAnalysisModel)
                {
                    case PricingAnalysisModelsEnum.Black_Scholes:
                        OptionsNet1.BSTheta(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Volatility, Dividend, ref CTheta, ref PTheta);
                        break;
                    case PricingAnalysisModelsEnum.AmericanBinomialSteps:
                        OptionsNet1.BinTheta(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Volatility, Dividend, Convert.ToInt32(ModelType.StockWithContinuousDividend), 0, _binomialSteps, Convert.ToInt32(FlagEurAm.American), ref CTheta, ref PTheta);
                        break;
                    case PricingAnalysisModelsEnum.Bjerksund_Stensland:
                        OptionsNet1.BJETheta(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Volatility, Dividend, Convert.ToInt32(ModelType.StockWithContinuousDividend), 0, ref CTheta, ref PTheta);
                        break;
                    case PricingAnalysisModelsEnum.Barone_Adesi_Whaley:
                        OptionsNet1.BAWTheta(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Volatility, Dividend, Convert.ToInt32(ModelType.StockWithContinuousDividend), 0, ref CTheta, ref PTheta);
                        break;
                    case PricingAnalysisModelsEnum.Black76:
                        //OptionsNet1.BLTheta(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Volatility, ref CTheta, ref PTheta);
                        GetBLTheta(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Volatility, Dividend, ref CTheta, ref PTheta);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.LoggerWrite(ex.ToString());
            }
        }

        /// <summary>
        /// Used name hiding
        /// TODO : Make it Asynchronous & Apply strategy pattern
        /// </summary>
        public void GetGamma(double StockPrice, double ExercisePrice, double InterestRate, double TimeToMaturity, double Volatility, double Dividend, ref double Gamma)
        {
            try
            {
                if (_currentPricingAnalysisModel == PricingAnalysisModelsEnum.None)
                {
                    ///Raise the exception by giving user a message to set CurrentPricingAnalysisModel property first before calling any method
                    ///and return from here itself
                    throw (new ArgumentException("Value of PricingAnalysisModelsEnum must be set before calculating any value."));
                }

                if (StockPrice == double.MinValue || ExercisePrice == double.MinValue || InterestRate == double.MinValue || TimeToMaturity == double.MinValue || Volatility == double.MinValue || Dividend == double.MinValue)
                    throw (new ArgumentException("Set the value of the arguments."));

                switch (_currentPricingAnalysisModel)
                {
                    case PricingAnalysisModelsEnum.Black_Scholes:
                        OptionsNet1.BSGamma(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Volatility, Dividend, ref Gamma);
                        break;
                    case PricingAnalysisModelsEnum.AmericanBinomialSteps:
                        OptionsNet1.BinGamma(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Volatility, Dividend, Convert.ToInt32(ModelType.StockWithContinuousDividend), 0, _binomialSteps, Convert.ToInt32(FlagEurAm.American), ref Gamma);
                        break;
                    case PricingAnalysisModelsEnum.Bjerksund_Stensland:
                        OptionsNet1.BJEGamma(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Volatility, Dividend, Convert.ToInt32(ModelType.StockWithContinuousDividend), 0, ref Gamma);
                        break;
                    case PricingAnalysisModelsEnum.Barone_Adesi_Whaley:
                        OptionsNet1.BAWGamma(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Volatility, Dividend, Convert.ToInt32(ModelType.StockWithContinuousDividend), 0, ref Gamma);
                        break;
                    case PricingAnalysisModelsEnum.Black76:
                        StockPrice = StockPrice * Math.Exp((-1) * TimeToMaturity * InterestRate);
                        OptionsNet1.BSGamma(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Volatility, Dividend, ref Gamma);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.LoggerWrite(ex.ToString());
            }
        }

        /// <summary>
        /// Used name hiding
        /// TODO : Make it Asynchronous & Apply strategy pattern
        /// </summary>
        public void GetVega(double StockPrice, double ExercisePrice, double InterestRate, double TimeToMaturity, double Volatility, double Dividend, ref double Vega)
        {
            try
            {
                if (_currentPricingAnalysisModel == PricingAnalysisModelsEnum.None)
                {
                    ///Raise the exception by giving user a message to set CurrentPricingAnalysisModel property first before calling any method
                    ///and return from here itself
                    throw (new ArgumentException("Value of PricingAnalysisModelsEnum must be set before calculating any value."));
                }

                if (StockPrice == double.MinValue || ExercisePrice == double.MinValue || InterestRate == double.MinValue || TimeToMaturity == double.MinValue || Volatility == double.MinValue || Dividend == double.MinValue)
                    throw (new ArgumentException("Set the value of the arguments."));

                switch (_currentPricingAnalysisModel)
                {
                    case PricingAnalysisModelsEnum.Black_Scholes:
                        OptionsNet1.BSVega(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Volatility, Dividend, ref Vega);
                        break;
                    case PricingAnalysisModelsEnum.AmericanBinomialSteps:
                        OptionsNet1.BinVega(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Volatility, Dividend, Convert.ToInt32(ModelType.StockWithContinuousDividend), 0, _binomialSteps, Convert.ToInt32(FlagEurAm.American), ref Vega);
                        break;
                    case PricingAnalysisModelsEnum.Bjerksund_Stensland:
                        OptionsNet1.BJEVega(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Volatility, Dividend, Convert.ToInt32(ModelType.StockWithContinuousDividend), 0, ref Vega);
                        break;
                    case PricingAnalysisModelsEnum.Barone_Adesi_Whaley:
                        OptionsNet1.BAWVega(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Volatility, Dividend, Convert.ToInt32(ModelType.StockWithContinuousDividend), 0, ref Vega);
                        break;
                    case PricingAnalysisModelsEnum.Black76:
                        StockPrice = StockPrice * Math.Exp((-1) * TimeToMaturity * InterestRate);
                        OptionsNet1.BSVega(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Volatility, Dividend, ref Vega);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.LoggerWrite(ex.ToString());
            }
        }

        /// <summary>
        /// Used name hiding
        /// TODO : Make it Asynchronous & Apply strategy pattern
        /// </summary>
        public void GetRho(double StockPrice, double ExercisePrice, double InterestRate, double TimeToMaturity, double Volatility, double Dividend, ref double CRho, ref double PRho)
        {
            try
            {
                if (_currentPricingAnalysisModel == PricingAnalysisModelsEnum.None)
                {
                    ///Raise the exception by giving user a message to set CurrentPricingAnalysisModel property first before calling any method
                    ///and return from here itself
                    throw (new ArgumentException("Value of PricingAnalysisModelsEnum must be set before calculating any value."));
                }

                if (StockPrice == double.MinValue || ExercisePrice == double.MinValue || InterestRate == double.MinValue || TimeToMaturity == double.MinValue || Volatility == double.MinValue || Dividend == double.MinValue)
                    throw (new ArgumentException("Set the value of the arguments."));

                switch (_currentPricingAnalysisModel)
                {
                    case PricingAnalysisModelsEnum.Black_Scholes:
                        OptionsNet1.BSRho(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Volatility, Dividend, ref CRho, ref PRho);
                        break;
                    case PricingAnalysisModelsEnum.AmericanBinomialSteps:
                        OptionsNet1.BinRho(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Volatility, Dividend, Convert.ToInt32(ModelType.StockWithContinuousDividend), 0, _binomialSteps, Convert.ToInt32(FlagEurAm.American), ref CRho, ref PRho);
                        break;
                    case PricingAnalysisModelsEnum.Bjerksund_Stensland:
                        OptionsNet1.BJERho(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Volatility, Dividend, Convert.ToInt32(ModelType.StockWithContinuousDividend), 0, ref CRho, ref PRho);
                        break;
                    case PricingAnalysisModelsEnum.Barone_Adesi_Whaley:
                        OptionsNet1.BAWRho(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Volatility, Dividend, Convert.ToInt32(ModelType.StockWithContinuousDividend), 0, ref CRho, ref PRho);
                        break;
                    case PricingAnalysisModelsEnum.Black76:
                        GetBLRho(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Volatility, Dividend, ref CRho, ref PRho);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.LoggerWrite(ex.ToString());
            }
        }

        /// <summary>
        /// Used name hiding
        /// TODO : Make it Asynchronous & Apply strategy pattern
        /// </summary>
        public double GetImpliedVolatility(double StockPrice, double ExercisePrice, double InterestRate, double TimeToMaturity, double Dividend, double CallPriceIV, int MaxIteration)
        {
            try
            {
                if (_currentPricingAnalysisModel == PricingAnalysisModelsEnum.None)
                {
                    ///Raise the exception by giving user a message to set CurrentPricingAnalysisModel property first before calling any method
                    ///and return from here itself
                    throw (new ArgumentException("Value of PricingAnalysisModelsEnum must be set before calculating any value."));
                }

                if (StockPrice == double.MinValue || StockPrice == 0 || ExercisePrice == double.MinValue || ExercisePrice == 0 || InterestRate == double.MinValue || InterestRate == 0 || TimeToMaturity == double.MinValue || TimeToMaturity == 0 || Dividend == double.MinValue || CallPriceIV == double.MinValue || CallPriceIV == 0 || MaxIteration == int.MinValue || StockPrice == -9999.0)
                    return 0;

                switch (_currentPricingAnalysisModel)
                {
                    case PricingAnalysisModelsEnum.Black_Scholes:
                        return OptionsNet1.BSImpliedVolatility(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Dividend, CallPriceIV, MaxIteration);
                    case PricingAnalysisModelsEnum.AmericanBinomialSteps:
                        return OptionsNet1.BinImpliedVolatility(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Dividend, Convert.ToInt32(ModelType.StockWithContinuousDividend), 0, _binomialSteps, Convert.ToInt32(FlagEurAm.American), CallPriceIV, MaxIteration);
                    case PricingAnalysisModelsEnum.Bjerksund_Stensland:
                        return OptionsNet1.BJEImpliedVolatility(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Dividend, Convert.ToInt32(ModelType.StockWithContinuousDividend), 0, CallPriceIV, MaxIteration);
                    case PricingAnalysisModelsEnum.Barone_Adesi_Whaley:
                        return OptionsNet1.BAWImpliedVolatility(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Dividend, Convert.ToInt32(ModelType.StockWithContinuousDividend), 0, CallPriceIV, MaxIteration);
                    case PricingAnalysisModelsEnum.Black76:
                        StockPrice = StockPrice * Math.Exp((-1) * TimeToMaturity * InterestRate);
                        return OptionsNet1.BSImpliedVolatility(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Dividend, CallPriceIV, MaxIteration);
                    default:
                        return 0;
                }
            }
            catch (Exception ex)
            {
                Logger.LoggerWrite(ex.ToString());
            }
            return 0;
        }

        /// <summary>
        /// Used name hiding
        /// TODO : Make it Asynchronous & Apply strategy pattern
        /// </summary>
        public double GetImpliedVolatilityPut(double StockPrice, double ExercisePrice, double InterestRate, double TimeToMaturity, double Dividend, double PutPriceIV, int MaxIteration)
        {
            try
            {
                if (_currentPricingAnalysisModel == PricingAnalysisModelsEnum.None)
                {
                    ///Raise the exception by giving user a message to set CurrentPricingAnalysisModel property first before calling any method
                    ///and return from here itself
                    throw (new ArgumentException("Value of PricingAnalysisModelsEnum must be set before calculating any value."));
                }

                if (StockPrice == double.MinValue || StockPrice == 0 || ExercisePrice == double.MinValue || ExercisePrice == 0 || InterestRate == double.MinValue || InterestRate == 0 || TimeToMaturity == double.MinValue || TimeToMaturity == 0 || Dividend == double.MinValue || PutPriceIV == double.MinValue || PutPriceIV == 0 || MaxIteration == int.MinValue || StockPrice == -9999.0)
                    return 0;

                switch (_currentPricingAnalysisModel)
                {
                    case PricingAnalysisModelsEnum.Black_Scholes:
                        return OptionsNet1.BSImpliedVolatilityPut(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Dividend, PutPriceIV, MaxIteration);
                    case PricingAnalysisModelsEnum.AmericanBinomialSteps:
                        return OptionsNet1.BinImpliedVolatilityPut(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Dividend, Convert.ToInt32(ModelType.StockWithContinuousDividend), 0, _binomialSteps, Convert.ToInt32(FlagEurAm.American), PutPriceIV, MaxIteration);
                    case PricingAnalysisModelsEnum.Bjerksund_Stensland:
                        return OptionsNet1.BJEImpliedVolatilityPut(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Dividend, Convert.ToInt32(ModelType.StockWithContinuousDividend), 0, PutPriceIV, MaxIteration);
                    case PricingAnalysisModelsEnum.Barone_Adesi_Whaley:
                        return OptionsNet1.BAWImpliedVolatilityPut(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Dividend, Convert.ToInt32(ModelType.StockWithContinuousDividend), 0, PutPriceIV, MaxIteration);
                    case PricingAnalysisModelsEnum.Black76:
                        StockPrice = StockPrice * Math.Exp((-1) * TimeToMaturity * InterestRate);
                        return OptionsNet1.BSImpliedVolatilityPut(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Dividend, PutPriceIV, MaxIteration);
                    default:
                        return 0;
                }
            }
            catch (Exception ex)
            {
                Logger.LoggerWrite(ex.ToString());
            }
            return 0;
        }

        private void GetBLRho(double StockPrice, double ExercisePrice, double InterestRate, double TimeToMaturity, double Volatility, double Dividend, ref double CRho, ref double PRho)
        {
            try
            {
                double d1 = 0;
                double d2 = 0;

                GetBSd1d2(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Dividend, Volatility, ref d1, ref d2);

                CRho = -TimeToMaturity * ((StockPrice * Math.Exp(-InterestRate * TimeToMaturity) * NormalSDist(d1)) - (ExercisePrice * Math.Exp(-InterestRate * TimeToMaturity) * NormalSDist(d2)));

                PRho = -TimeToMaturity * ((ExercisePrice * Math.Exp(-InterestRate * TimeToMaturity) * NormalSDist(-d2)) - (StockPrice * Math.Exp(-InterestRate * TimeToMaturity) * NormalSDist(-d1)));
            }
            catch (Exception ex)
            {
                Logger.LoggerWrite(ex.ToString());
            }
        }

        private void GetBLTheta(double StockPrice, double ExercisePrice, double InterestRate, double TimeToMaturity, double Volatility, double Dividend, ref double CTheta, ref double PTheta)
        {
            try
            {
                double d1 = 0;
                double d2 = 0;

                GetBSd1d2(StockPrice, ExercisePrice, InterestRate, TimeToMaturity, Dividend, Volatility, ref d1, ref d2);

                CTheta = -((StockPrice * (Math.Exp(-1 * InterestRate * TimeToMaturity) * NDF(d1) * Volatility)) / (2 * Math.Sqrt(TimeToMaturity))) + (InterestRate * StockPrice * Math.Exp(-1 * InterestRate * TimeToMaturity) * NormalSDist(d1)) - (InterestRate * ExercisePrice * Math.Exp(-1 * InterestRate * TimeToMaturity) * NormalSDist(d2));

                PTheta = -((StockPrice * (Math.Exp(-1 * InterestRate * TimeToMaturity) * NDF(d1) * Volatility)) / (2 * Math.Sqrt(TimeToMaturity))) - (InterestRate * StockPrice * Math.Exp(-1 * InterestRate * TimeToMaturity) * NormalSDist(-d1)) + (InterestRate * ExercisePrice * Math.Exp(-1 * InterestRate * TimeToMaturity) * NormalSDist(-d2));
            }
            catch (Exception ex)
            {
                Logger.LoggerWrite(ex.ToString());
            }
        }

        private double NDF(double var)
        {
            try
            {
                return 0.398942280401433 * Math.Exp(-var * var * 0.5);
            }
            catch (Exception ex)
            {
                Logger.LoggerWrite(ex.ToString());
            }
            return 0;
        }

        private void GetBSd1d2(double StockPrice, double ExercisePrice, double InterestRate, double TimeToMaturity, double Dividend, double Volatility, ref double d1, ref double d2)
        {
            try
            {
                //Bharat Kumar Jangir (21 February 2014)
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-3432
                //Verified with Windale Sample Application
                d1 = (1 / (Volatility * Math.Sqrt(TimeToMaturity))) * ((Math.Log(StockPrice / ExercisePrice) + ((0.5 * Math.Pow(Volatility, 2)) * TimeToMaturity)));
                d2 = d1 - Volatility * Math.Sqrt(TimeToMaturity);
            }
            catch (Exception ex)
            {
                Logger.LoggerWrite(ex.ToString());
            }
        }

        private double NormalSDist(double var)
        {
            try
            {
                if (var < 0)
                {
                    return 1.00 - NormalSDist(-1.00 * var);
                }
                else
                {
                    double num1;
                    double num2;
                    double num3;
                    double num4;
                    double num5;
                    double num6;
                    double num7;
                    double num8;

                    num1 = NDF(var);
                    num2 = 0.2316419;
                    num3 = 0.31938153;
                    num4 = -0.356563782;
                    num5 = 1.781477937;
                    num6 = -1.821255978;
                    num7 = 1.330274429;
                    num8 = 1.0 / (1.0 + num2 * var);

                    double result = 1 - num1 * (num8 * num3 + num4 * Math.Pow(num8, 2) + num5 * Math.Pow(num8, 3) + num6 * Math.Pow(num8, 4) + num7 * Math.Pow(num8, 5));
                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.LoggerWrite(ex.ToString());
            }
            return 0;
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            OptionsNet1.Dispose();
        }
        #endregion
    }
}
