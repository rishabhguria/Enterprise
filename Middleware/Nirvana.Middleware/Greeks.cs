using System;
using System.Collections.Generic;
using Windale.Options;

namespace Nirvana.Middleware
{
    public class RiskAdjustModel
    {
        public double dOne { get; set; }
        public double dOneCumulativeDistribution { get; set; }
        public double dOneNegativeCumulativeDistribution { get; set; }
        public double dOneProbabilityDensity { get; set; }
        public double dTwo { get; set; }
        public double dTwoCumulativeDistribution { get; set; }
        public double dTwoNegativeCumulativeDistribution { get; set; }
    }

    public class RiskAdjustBlackScholesModel : RiskAdjustModel
    {
        public RiskAdjustBlackScholesModel(double _UnderlyingPrice, double _ExercisePrice, double _Interest, double _Dividend, double _Time, double _ImpliedVolatility)
        {
            dOne = (Math.Log(_UnderlyingPrice / _ExercisePrice) +
                (_Interest - _Dividend + Math.Pow(_ImpliedVolatility, 2) / 2) * _Time) / (_ImpliedVolatility * (Math.Sqrt(_Time)));
            dTwo = dOne - (_ImpliedVolatility * Math.Sqrt(_Time));
            dOneProbabilityDensity = dOne.ProbabilityDensity();
            dOneCumulativeDistribution = dOne.CumulativeDistribution();
            dOneNegativeCumulativeDistribution = (-dOne).CumulativeDistribution();
            dTwoCumulativeDistribution = dTwo.CumulativeDistribution();
            dTwoNegativeCumulativeDistribution = (-dTwo).CumulativeDistribution();
        }
    }

    public class RiskAdjustBlack76Model : RiskAdjustModel
    {
        public RiskAdjustBlack76Model(double _UnderlyingPrice, double _ExercisePrice, double _Interest, double _Dividend, double _Time, double _ImpliedVolatility)
        {
            dOne = (Math.Log(_UnderlyingPrice / _ExercisePrice) +
                (Math.Pow(_ImpliedVolatility, 2) / 2) * _Time) / (_ImpliedVolatility * (Math.Sqrt(_Time)));
            dTwo = dOne - (_ImpliedVolatility * Math.Sqrt(_Time));
            dOneProbabilityDensity = dOne.ProbabilityDensity();
            dOneCumulativeDistribution = dOne.CumulativeDistribution();
            dOneNegativeCumulativeDistribution = (-dOne).CumulativeDistribution();
            dTwoCumulativeDistribution = dTwo.CumulativeDistribution();
            dTwoNegativeCumulativeDistribution = (-dTwo).CumulativeDistribution();
        }
    }

    public class GreeksModelEquality : IEqualityComparer<GreeksModel>
    {
        public bool Equals(GreeksModel x, GreeksModel y)
        {
            return x.Symbol == y.Symbol && x.Underlying == y.Underlying;
        }

        public int GetHashCode(GreeksModel obj)
        {
            return obj.Symbol.GetHashCode() ^ obj.Symbol.GetHashCode();
        }
    }


    public class GreeksModel
    {

        public virtual void Calculate(bool UseWinDaleToCalculateImpliedVol, bool UseUserPrefToCalculateImpliedVol) { }

        public string Asset { get; set; }

        public double? Delta { get; set; }

        public double? Theta { get; set; }

        public double? Rho { get; set; }

        public double? Vega { get; set; }

        public double? Gamma { get; set; }

        public double? OptionValue { get; set; }

        public double ImpliedVolatility { get; set; }

        public string Symbol { get; set; }

        public double? SymbolPrice { get; set; }

        public string Underlying { get; set; }

        public double? UnderlyingPrice { get; set; }

        public string PutOrCall { get; set; }

        public double? ExercisePrice { get; set; }

        public double Time { get; set; }

        public double Interest { get; set; }

        public double Dividend { get; set; }
    }

    public class BlackScholesModel : GreeksModel
    {

        public BlackScholesModel(GreeksModel _Model)
        {
            Asset = _Model.Asset;
            Symbol = _Model.Symbol;
            SymbolPrice = _Model.SymbolPrice;
            Underlying = _Model.Underlying;
            UnderlyingPrice = _Model.UnderlyingPrice;
            PutOrCall = _Model.PutOrCall;
            ExercisePrice = _Model.ExercisePrice;
            Time = _Model.Time;
            Interest = _Model.Interest;
            Dividend = _Model.Dividend;
        }

     

        public override void Calculate(bool UseWinDaleToCalculateImpliedVol, bool UseUserPrefToCalculateImpliedVol)
        {
            if (UseUserPrefToCalculateImpliedVol)
            {
                if (ImpliedVolatility == 0 && UseWinDaleToCalculateImpliedVol)
                {
                    int maximumIterations = Nirvana.Middleware.Properties.Settings.Default.ImpliedVolatiltyIterations;
                    ImpliedVolatility = CalculateImpliedVolatility(PutOrCall, UnderlyingPrice.Value, ExercisePrice.Value, Interest, Dividend, Time, SymbolPrice.Value, maximumIterations);
                }
            }
            else if (UseWinDaleToCalculateImpliedVol)
            {
                int maximumIterations = Nirvana.Middleware.Properties.Settings.Default.ImpliedVolatiltyIterations;
                ImpliedVolatility = CalculateImpliedVolatility(PutOrCall, UnderlyingPrice.Value, ExercisePrice.Value, Interest, Dividend, Time, SymbolPrice.Value, maximumIterations);
            }
            else
                ImpliedVolatility = CalculateImpliedVolatility(PutOrCall, UnderlyingPrice.Value, ExercisePrice.Value, Interest, Dividend, Time, SymbolPrice.Value);
            RiskAdjustBlackScholesModel model = new RiskAdjustBlackScholesModel(UnderlyingPrice.Value, ExercisePrice.Value, Interest, Dividend, Time, ImpliedVolatility);
            Delta = CalculateDelta(model, PutOrCall, UnderlyingPrice.Value, ExercisePrice.Value, Interest, Dividend, Time, ImpliedVolatility);
            Rho = CalculateRho(model, PutOrCall, UnderlyingPrice.Value, ExercisePrice.Value, Interest, Dividend, Time, ImpliedVolatility);
            Vega = CalculateVega(model, PutOrCall, UnderlyingPrice.Value, ExercisePrice.Value, Interest, Dividend, Time, ImpliedVolatility);
            Gamma = CalculateGamma(model, PutOrCall, UnderlyingPrice.Value, ExercisePrice.Value, Interest, Dividend, Time, ImpliedVolatility);
            Theta = CalculateTheta(model, PutOrCall, UnderlyingPrice.Value, ExercisePrice.Value, Interest, Dividend, Time, ImpliedVolatility);
        }

        public double CalculateImpliedVolatility(string _PutOrCall, double _UnderlyingPrice, double _ExercisePrice, double _Interest, double _Dividend, double _Time, double _SymbolPrice)
        {
            double hi = 10;
            double lo = 0;

            while (hi - lo > 0.0001)
            {
                if (CalculateOptionValue(_PutOrCall, _UnderlyingPrice, _ExercisePrice, _Interest, _Dividend, _Time, (hi + lo) / 2) > _SymbolPrice)
                    hi = (hi + lo) / 2.00;
                else
                    lo = (hi + lo) / 2.00;
            }

            return (hi + lo) / 2.00;
        }

        public double CalculateImpliedVolatility(string _PutOrCall, double _UnderlyingPrice, double _ExercisePrice, double _Interest, double _Dividend, double _Time, double _SymbolPrice, int VolatilityIterations)
        {
            try
            {
                OptionsNET OptionsNet1 = new OptionsNET();
                if (_SymbolPrice == double.MinValue || _SymbolPrice == 0 || ExercisePrice.Value == double.MinValue || !ExercisePrice.HasValue || _Interest == double.MinValue || _Interest == 0
                    || _Time == double.MinValue || _Time == 0 || Dividend == double.MinValue || _UnderlyingPrice == double.MinValue || _UnderlyingPrice == 0 || VolatilityIterations == int.MinValue || _SymbolPrice == -9999.0)
                    return 0;

                if (_PutOrCall.Trim().ToUpper().StartsWith("C"))
                {
                    return OptionsNet1.BSImpliedVolatility(_UnderlyingPrice, ExercisePrice.Value, _Interest, _Time, Dividend, _SymbolPrice, VolatilityIterations);
                }
                else if (_PutOrCall.Trim().ToUpper().StartsWith("P"))
                {
                    return OptionsNet1.BSImpliedVolatilityPut(_UnderlyingPrice, ExercisePrice.Value, _Interest, _Time, Dividend, _SymbolPrice, VolatilityIterations);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return 0;
        }
        public double? CalculateOptionValue(string _PutOrCall, double _UnderlyingPrice, double _ExercisePrice, double _Interest, double _Dividend, double _Time, double _ImpliedVolatility)
        {
            RiskAdjustBlackScholesModel model = new RiskAdjustBlackScholesModel(_UnderlyingPrice, _ExercisePrice, _Interest, _Dividend, _Time, _ImpliedVolatility);

            if (_PutOrCall.Trim().ToUpper().StartsWith("P"))
            {
                return Math.Exp(-_Interest * _Time) * _ExercisePrice * model.dTwoNegativeCumulativeDistribution -
                Math.Exp(-_Dividend * _Time) * _UnderlyingPrice * model.dOneNegativeCumulativeDistribution;
            }
            else if (_PutOrCall.Trim().ToUpper().StartsWith("C"))
            {
                return Math.Exp(-_Dividend * _Time) * _UnderlyingPrice * model.dOneCumulativeDistribution -
                Math.Exp(-_Interest * _Time) * _ExercisePrice * model.dTwoCumulativeDistribution;
            }
            else
                return null;
        }

        public double? CalculateDelta(RiskAdjustModel model, string _PutOrCall, double _UnderlyingPrice, double _ExercisePrice, double _Interest, double _Dividend, double _Time, double _ImpliedVolatility)
        {
            if (_PutOrCall.Trim().ToUpper().StartsWith("P"))
            {
                return -Math.Exp(-_Dividend * _Time) * model.dOneNegativeCumulativeDistribution;
            }
            else if (_PutOrCall.Trim().ToUpper().StartsWith("C"))
            {
                return Math.Exp(-_Dividend * _Time) * model.dOneCumulativeDistribution;
            }
            else
                return null;
        }

        public double? CalculateGamma(RiskAdjustModel model, string _PutOrCall, double _UnderlyingPrice, double _ExercisePrice, double _Interest, double _Dividend, double _Time, double _ImpliedVolatility)
        {
            if (_PutOrCall.Trim().ToUpper().StartsWith("P") || _PutOrCall.Trim().ToUpper().StartsWith("C"))
            {
                double gamma = Math.Exp(-_Dividend * _Time) * (model.dOneProbabilityDensity / (_UnderlyingPrice * (_ImpliedVolatility * Math.Sqrt(_Time))));
                return gamma;
            }
            else
                return null;
        }

        public double? CalculateVega(RiskAdjustModel model, string _PutOrCall, double _UnderlyingPrice, double _ExercisePrice, double _Interest, double _Dividend, double _Time, double _ImpliedVolatility)
        {
            if (_PutOrCall.Trim().ToUpper().StartsWith("P") || _PutOrCall.Trim().ToUpper().StartsWith("C"))
            {
                double vega = _UnderlyingPrice * Math.Exp(-_Dividend * _Time) * Math.Sqrt(_Time) * model.dOneProbabilityDensity;
                return vega / 100.0d;
            }
            else
                return null;
        }

        public double? CalculateTheta(RiskAdjustModel model, string _PutOrCall, double _UnderlyingPrice, double _ExercisePrice, double _Interest, double _Dividend, double _Time, double _ImpliedVolatility)
        {
            if (_PutOrCall.Trim().ToUpper().StartsWith("P"))
            {
                double theta = (-Math.Exp(-_Dividend * _Time) * (_UnderlyingPrice * _ImpliedVolatility * model.dOneProbabilityDensity) /
                    (2.00 * Math.Sqrt(_Time)) + _Interest * _ExercisePrice * Math.Exp(-_Interest * _Time) *
                    model.dTwoNegativeCumulativeDistribution -
                    _Dividend * _UnderlyingPrice * Math.Exp(-_Dividend * _Time) *
                    model.dOneNegativeCumulativeDistribution);
                return theta / 365.0d;
            }
            else if (_PutOrCall.Trim().ToUpper().StartsWith("C"))
            {
                double theta = (-Math.Exp(-_Dividend * _Time) * (_UnderlyingPrice * _ImpliedVolatility * model.dOneProbabilityDensity) /
                    (2.00 * Math.Sqrt(_Time)) - _Interest * _ExercisePrice * Math.Exp(-_Interest * _Time) *
                    model.dTwoCumulativeDistribution +
                    _Dividend * _UnderlyingPrice * Math.Exp(-_Dividend * _Time) * model.dOneCumulativeDistribution);
                return theta / 365.0d;
            }
            else
                return null;
        }

        public double? CalculateRho(RiskAdjustModel model, string _PutOrCall, double _UnderlyingPrice, double _ExercisePrice, double _Interest, double _Dividend, double _Time, double _ImpliedVolatility)
        {
            if (_PutOrCall.Trim().ToUpper().StartsWith("P"))
            {
                double rho = -_ExercisePrice * _Time * Math.Exp(-_Interest * _Time) *
                    model.dTwoNegativeCumulativeDistribution;
                return rho / 100.0d;
            }
            else if (_PutOrCall.Trim().ToUpper().StartsWith("C"))
            {
                double rho = _ExercisePrice * _Time * Math.Exp(-_Interest * _Time) *
                    model.dTwoCumulativeDistribution;
                return rho / 100.0d;
            }
            else
                return null;
        }
    }

    public class Black76Model : GreeksModel
    {

        
        public Black76Model(GreeksModel _Model)
        {
            Asset = _Model.Asset;
            Symbol = _Model.Symbol;
            SymbolPrice = _Model.SymbolPrice;
            Underlying = _Model.Underlying;
            UnderlyingPrice = _Model.UnderlyingPrice;
            PutOrCall = _Model.PutOrCall;
            ExercisePrice = _Model.ExercisePrice;
            Time = _Model.Time;
            Interest = _Model.Interest;
            Dividend = _Model.Dividend;
        }

      

        public override void Calculate(bool UseWinDaleToCalculateImpliedVol, bool UseUserPrefToCalculateImpliedVol)
        {
            if (UseWinDaleToCalculateImpliedVol)
            {
                int maximumIterations = Nirvana.Middleware.Properties.Settings.Default.ImpliedVolatiltyIterations;
                ImpliedVolatility = CalculateImpliedVolatility("BlackScholes", PutOrCall, UnderlyingPrice.Value * Math.Exp(-Interest * Time), ExercisePrice.Value, Interest, Dividend, Time, SymbolPrice.Value, maximumIterations);
            }
            else
                ImpliedVolatility = CalculateImpliedVolatility("BlackScholes", PutOrCall, UnderlyingPrice.Value * Math.Exp(-Interest * Time), ExercisePrice.Value, Interest, Dividend, Time, SymbolPrice.Value);
            RiskAdjustBlackScholesModel model = new RiskAdjustBlackScholesModel(UnderlyingPrice.Value * Math.Exp(-Interest * Time), ExercisePrice.Value, Interest, Dividend, Time, ImpliedVolatility);
            Delta = CalculateDelta(model, PutOrCall, UnderlyingPrice.Value * Math.Exp(-Interest * Time), ExercisePrice.Value, Interest, Dividend, Time, ImpliedVolatility);
            Vega = CalculateVega(model, PutOrCall, UnderlyingPrice.Value * Math.Exp(-Interest * Time), ExercisePrice.Value, Interest, Dividend, Time, ImpliedVolatility);
            Gamma = CalculateGamma(model, PutOrCall, UnderlyingPrice.Value * Math.Exp(-Interest * Time), ExercisePrice.Value, Interest, Dividend, Time, ImpliedVolatility);

            double _ImpliedVolatility = ImpliedVolatility;
            //double _ImpliedVolatility = CalculateImpliedVolatility("Black76", PutOrCall, UnderlyingPrice.Value, ExercisePrice.Value, Interest, Dividend, Time, SymbolPrice.Value);
            RiskAdjustBlack76Model _model = new RiskAdjustBlack76Model(UnderlyingPrice.Value, ExercisePrice.Value, Interest, Dividend, Time, _ImpliedVolatility);

            Rho = CalculateRho(_model, PutOrCall, UnderlyingPrice.Value, ExercisePrice.Value, Interest, Dividend, Time, _ImpliedVolatility);
            Theta = CalculateTheta(_model, PutOrCall, UnderlyingPrice.Value, ExercisePrice.Value, Interest, Dividend, Time, _ImpliedVolatility);
        }

        public double CalculateImpliedVolatility(string _RiskAdjustModel, string _PutOrCall, double _UnderlyingPrice, double _ExercisePrice, double _Interest, double _Dividend, double _Time, double _SymbolPrice, int VolatilityIterations)
        {
            try
            {
                OptionsNET OptionsNet1 = new OptionsNET();
                if (_SymbolPrice == double.MinValue || _SymbolPrice == 0 || ExercisePrice.Value == double.MinValue || !ExercisePrice.HasValue || _Interest == double.MinValue || _Interest == 0
                    || _Time == double.MinValue || _Time == 0 || Dividend == double.MinValue || _UnderlyingPrice == double.MinValue || _UnderlyingPrice == 0 || VolatilityIterations == int.MinValue || _SymbolPrice == -9999.0)
                    return 0;


                _UnderlyingPrice = _UnderlyingPrice * Math.Exp((-1) * _Time * _Interest);

                if (_PutOrCall.Trim().ToUpper().StartsWith("C"))
                {
                    return OptionsNet1.BSImpliedVolatility(_UnderlyingPrice, ExercisePrice.Value, _Interest, _Time, Dividend, _SymbolPrice, VolatilityIterations);
                }
                else if (_PutOrCall.Trim().ToUpper().StartsWith("P"))
                {
                    return OptionsNet1.BSImpliedVolatilityPut(_UnderlyingPrice, ExercisePrice.Value, _Interest, _Time, Dividend, _SymbolPrice, VolatilityIterations);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return 0;
        }

        public double CalculateImpliedVolatility(string _RiskAdjustModel, string _PutOrCall, double _UnderlyingPrice, double _ExercisePrice, double _Interest, double _Dividend, double _Time, double _SymbolPrice)
        {
            double hi = 10;
            double lo = 0;

            while (hi - lo > 0.0001)
            {
                if (CalculateOptionValue(_RiskAdjustModel, _PutOrCall, _UnderlyingPrice, _ExercisePrice, _Interest, _Dividend, _Time, (hi + lo) / 2) > _SymbolPrice)
                    hi = (hi + lo) / 2.00;
                else
                    lo = (hi + lo) / 2.00;
            }

            return (hi + lo) / 2.00;
        }

        public double? CalculateOptionValue(string _RiskAdjustModel, string _PutOrCall, double _UnderlyingPrice, double _ExercisePrice, double _Interest, double _Dividend, double _Time, double _ImpliedVolatility)
        {
            RiskAdjustModel model;

            switch (_RiskAdjustModel)
            {
                case "Black76":
                    model = new RiskAdjustBlack76Model(_UnderlyingPrice, _ExercisePrice, _Interest, _Dividend, _Time, _ImpliedVolatility);

                    if (_PutOrCall.Trim().ToUpper().StartsWith("P"))
                    {
                        return Math.Exp(-_Interest * _Time) * _ExercisePrice * model.dTwoNegativeCumulativeDistribution -
                        Math.Exp(-_Interest * _Time) * _UnderlyingPrice * model.dOneNegativeCumulativeDistribution;
                    }
                    else if (_PutOrCall.Trim().ToUpper().StartsWith("C"))
                    {
                        return Math.Exp(-_Interest * _Time) * _UnderlyingPrice * model.dOneCumulativeDistribution -
                        Math.Exp(-_Interest * _Time) * _ExercisePrice * model.dTwoCumulativeDistribution;
                    }
                    else
                        return null;
                default:
                    model = new RiskAdjustBlackScholesModel(_UnderlyingPrice, _ExercisePrice, _Interest, _Dividend, _Time, _ImpliedVolatility);

                    if (_PutOrCall.Trim().ToUpper().StartsWith("P"))
                    {
                        return Math.Exp(-_Interest * _Time) * _ExercisePrice * model.dTwoNegativeCumulativeDistribution -
                        Math.Exp(-_Dividend * _Time) * _UnderlyingPrice * model.dOneNegativeCumulativeDistribution;
                    }
                    else if (_PutOrCall.Trim().ToUpper().StartsWith("C"))
                    {
                        return Math.Exp(-_Dividend * _Time) * _UnderlyingPrice * model.dOneCumulativeDistribution -
                        Math.Exp(-_Interest * _Time) * _ExercisePrice * model.dTwoCumulativeDistribution;
                    }
                    else
                        return null;
            }
        }

        public double? CalculateDelta(RiskAdjustModel model, string _PutOrCall, double _UnderlyingPrice, double _ExercisePrice, double _Interest, double _Dividend, double _Time, double _ImpliedVolatility)
        {
            if (_PutOrCall.Trim().ToUpper().StartsWith("P"))
            {
                return -Math.Exp(-_Dividend * _Time) * model.dOneNegativeCumulativeDistribution;
            }
            else if (_PutOrCall.Trim().ToUpper().StartsWith("C"))
            {
                return Math.Exp(-_Dividend * _Time) * model.dOneCumulativeDistribution;
            }
            else
                return null;
        }

        public double? CalculateGamma(RiskAdjustModel model, string _PutOrCall, double _UnderlyingPrice, double _ExercisePrice, double _Interest, double _Dividend, double _Time, double _ImpliedVolatility)
        {
            if (_PutOrCall.Trim().ToUpper().StartsWith("P") || _PutOrCall.Trim().ToUpper().StartsWith("C"))
            {
                double gamma = Math.Exp(-_Dividend * _Time) * (model.dOneProbabilityDensity / (_UnderlyingPrice * (_ImpliedVolatility * Math.Sqrt(_Time))));
                return gamma;
            }
            else
                return null;
        }

        public double? CalculateVega(RiskAdjustModel model, string _PutOrCall, double _UnderlyingPrice, double _ExercisePrice, double _Interest, double _Dividend, double _Time, double _ImpliedVolatility)
        {
            if (_PutOrCall.Trim().ToUpper().StartsWith("P") || _PutOrCall.Trim().ToUpper().StartsWith("C"))
            {
                double vega = _UnderlyingPrice * Math.Exp(-_Dividend * _Time) * Math.Sqrt(_Time) * model.dOneProbabilityDensity;
                return vega / 100.0d;
            }
            else
                return null;
        }

        public double? CalculateTheta(RiskAdjustModel model, string _PutOrCall, double _UnderlyingPrice, double _ExercisePrice, double _Interest, double _Dividend, double _Time, double _ImpliedVolatility)
        {
            if (_PutOrCall.Trim().ToUpper().StartsWith("P"))
            {
                double theta = -_UnderlyingPrice * Math.Exp(-_Interest * _Time) * model.dOneProbabilityDensity * _ImpliedVolatility / (2 * Math.Sqrt(_Time))
                    - _Interest * _UnderlyingPrice * Math.Exp(-_Interest * _Time) * model.dOneNegativeCumulativeDistribution
                    + _Interest * _ExercisePrice * Math.Exp(-_Interest * _Time) * model.dTwoNegativeCumulativeDistribution;
                return theta / 365.0d;
            }
            else if (_PutOrCall.Trim().ToUpper().StartsWith("C"))
            {
                double theta = -_UnderlyingPrice * Math.Exp(-_Interest * _Time) * model.dOneProbabilityDensity * _ImpliedVolatility / (2 * Math.Sqrt(_Time))
                    + _Interest * _UnderlyingPrice * Math.Exp(-_Interest * _Time) * model.dOneCumulativeDistribution
                    - _Interest * _ExercisePrice * Math.Exp(-_Interest * _Time) * model.dTwoCumulativeDistribution;
                return theta / 365.0d;
            }
            else
                return null;
        }

        public double? CalculateRho(RiskAdjustModel model, string _PutOrCall, double _UnderlyingPrice, double _ExercisePrice, double _Interest, double _Dividend, double _Time, double _ImpliedVolatility)
        {
            if (_PutOrCall.Trim().ToUpper().StartsWith("P"))
            {
                double rho = -_Time * ((_ExercisePrice * Math.Exp(-_Interest * _Time) * model.dTwoNegativeCumulativeDistribution)
                    - (_UnderlyingPrice * Math.Exp(-_Interest * _Time) * model.dOneNegativeCumulativeDistribution));
                return rho / 100.0d;
            }
            else if (_PutOrCall.Trim().ToUpper().StartsWith("C"))
            {
                double rho = -_Time * ((_UnderlyingPrice * Math.Exp(-_Interest * _Time) * model.dOneCumulativeDistribution)
                    - (_ExercisePrice * Math.Exp(-_Interest * _Time) * model.dTwoCumulativeDistribution));
                return rho / 100.0d;
            }
            else
                return null;
        }
    }
}
