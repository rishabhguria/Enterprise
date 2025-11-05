using Prana.PricingAnalysisModels;

namespace Prana.OptionCalculator.CalculationComponent
{
    public class PricingAnalysisModelFactory
    {
        static PricingAnalysisVendorEnum _sPricingAnalysisModelVendor = PricingAnalysisVendorEnum.WinDale;
        static IPricingAnalysisModel _currentAdapter;

        public PricingAnalysisModelFactory()
        {
            ///Get the configuration from app.config for the vendor information

        }

        public static IPricingAnalysisModel GetPricingAnalysisModel()
        {
            switch (_sPricingAnalysisModelVendor)
            {
                case PricingAnalysisVendorEnum.Numerix:
                    return null;
                case PricingAnalysisVendorEnum.WinDale:
                    _currentAdapter = new WinDalePricingAnalysisAdapter();
                    return _currentAdapter;
                default:
                    return null;
            }
        }


    }
}
