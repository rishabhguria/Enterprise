
namespace SoftVest.FinLib
{
    public class BondCalculatorAIResult
    {
        // AI percentage result 
        public BondCalculatorResult AIResult
        {
            get { return _AIResult; }
            set { _AIResult = value; }
        }

        // AI percentage result 
        public BondCalculatorResult AIValueResult
        {
            get { return _AIValueResult; }
            set { _AIValueResult = value; }
        }

        private BondCalculatorResult _AIResult;
        private BondCalculatorResult _AIValueResult;
    }
}
