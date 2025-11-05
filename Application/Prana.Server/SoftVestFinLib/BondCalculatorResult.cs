
namespace SoftVest.FinLib
{
    public class BondCalculatorResult
    {
        // constructors
        public BondCalculatorResult()
        {
        }

        public BondCalculatorResult(BondCalculatorResult bondCalcResult)
        {
            _bError = bondCalcResult.IsError;
            _resultVal = bondCalcResult.ResultVal;
            _warnMsg = bondCalcResult.WarningMessage;
            _errMsg = bondCalcResult.ErrorMessage;
        }

        // the result of the calculation, eg AI or AI Value
        public double ResultVal
        {
            get { return _resultVal; }
            set { _resultVal = value; }
        }

        // an error message when the calculation fails
        public string ErrorMessage
        {
            get { return _errMsg; }
            set { _errMsg = value; }
        }

        // a possible warning message when the calculation is OK
        public string WarningMessage
        {
            get { return _warnMsg; }
            set { _warnMsg = value; }
        }

        // IsError - returns true if error in calculation
        public bool IsError
        {
            get { return _bError; }
            set { _bError = value; }
        }

        private double _resultVal = Constants.NA_Value;
        private string _errMsg = "";
        private string _warnMsg = "";
        private bool _bError = false;
    }
}
