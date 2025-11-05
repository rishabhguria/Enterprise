using Prana.LogManager;
using System;
namespace Prana.BusinessObjects
{
    [Serializable]
    public class StepAnalysisResponse
    {
        InputParametersForGreeks _inputParameter = null;
        OptionGreeks _greeks = null;
        StepParameter _stepParam1;
        private string _symbol = string.Empty;
        string _underlyingSymbol = string.Empty;
        string _userID = string.Empty;
        public StepAnalysisResponse()
        {
            _inputParameter = new InputParametersForGreeks();
            _greeks = new OptionGreeks();
        }
        public StepAnalysisResponse(InputParametersForGreeks inputParameter)
        {
            _inputParameter = inputParameter;
            _symbol = _inputParameter.Symbol;
        }
        public StepAnalysisResponse(string data)
        {
            string[] dataArray = data.Split(Seperators.SEPERATOR_3);
            if (dataArray[0] != string.Empty)
                _inputParameter = new InputParametersForGreeks(dataArray[0]);
            if (dataArray[1] != string.Empty)
                _greeks = new OptionGreeks(dataArray[1]);
        }
        public override string ToString()
        {
            try
            {
                return _inputParameter.ToString() + Seperators.SEPERATOR_3 + _greeks.ToString();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return string.Empty;
        }

        public StepAnalysisResponse(string userId, string symbol, string underlyingSymbol, StepParameter stepParam1)
        {
            _stepParam1 = stepParam1;
            _symbol = symbol;
            _underlyingSymbol = underlyingSymbol;
            _userID = userId;
        }
        public StepParameter StepParameter_X
        {
            get { return _stepParam1; }
            set { _stepParam1 = value; }
        }
        public string Symbol
        {
            set { _symbol = value; }
            get { return _symbol; }
        }
        public OptionGreeks Greeks
        {
            get { return _greeks; }
            set { _greeks = value; }
        }
        public InputParametersForGreeks InputParameters
        {
            get { return _inputParameter; }
            set { _inputParameter = value; }
        }
        public void SetXParameters(double changedValue)
        {

            if (_stepParam1.ParameterCode == StepAnalParameterCode.UnderlyingPrice)
            {
                if (changedValue == -100)
                {
                    _inputParameter.SimulatedUnderlyingStockPrice = _inputParameter.SimulatedUnderlyingStockPrice * .000099999999999988987d;
                }
                else
                {
                    _inputParameter.SimulatedUnderlyingStockPrice = _inputParameter.SimulatedUnderlyingStockPrice * (1 + changedValue / 100);
                }
            }
            else if (_stepParam1.ParameterCode == StepAnalParameterCode.Volatility)
            {
                if (changedValue == -100)
                {
                    _inputParameter.Volatility = _inputParameter.Volatility * .000099999999999988987d;
                }
                else
                {
                    _inputParameter.Volatility = _inputParameter.Volatility * (1 + changedValue / 100);
                }
            }
            else if (_stepParam1.ParameterCode == StepAnalParameterCode.InterestRate)
            {
                if (changedValue == -100)
                {
                    _inputParameter.InterestRate = _inputParameter.InterestRate * .000099999999999988987d;
                }
                else
                {
                    _inputParameter.InterestRate = _inputParameter.InterestRate * (1 + changedValue / 100);
                }
            }
            else if (_stepParam1.ParameterCode == StepAnalParameterCode.DaysToExpiration)
            {
                _inputParameter.ExpirationDate = _inputParameter.ExpirationDate.AddDays(changedValue * (-1));
            }
        }

        public string UnderlyingSymbol
        {
            get { return _underlyingSymbol; }
            set { _underlyingSymbol = value; }
        }
        public string UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }
        public object Clone()
        {
            return Prana.Global.Utilities.DeepCopyHelper.Clone(this);
        }
    }
}
