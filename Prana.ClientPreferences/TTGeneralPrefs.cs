using Prana.BusinessObjects.AppConstants;
using Prana.Global;

namespace Prana.ClientPreferences
{
    public class TTGeneralPrefs
    {
        private int _defaultSymbology = (int)ApplicationConstants.SymbologyCodes.TickerSymbol;
        public int DefaultSymbology
        {
            get { return _defaultSymbology; }
            set { _defaultSymbology = value; }
        }

        private int _defaultOptionType = (int)OptionType.CALL;
        public int DefaultOptionType
        {
            get { return _defaultOptionType; }
            set { _defaultOptionType = value; }
        }

        private bool _isShowOptionDetails = false;
        public bool IsShowOptionDetails
        {
            get { return _isShowOptionDetails; }
            set { _isShowOptionDetails = value; }
        }

        private bool _isSaveChecked = false;
        public bool IsSaveChecked
        {
            get { return _isSaveChecked; }
            set { _isSaveChecked = value; }
        }

        private bool _isPopulatelastPriceInPriceWhenAskORBidIsZero = false;
        public bool IsPopulatelastPriceInPriceWhenAskORBidIsZero
        {
            get { return _isPopulatelastPriceInPriceWhenAskORBidIsZero; }
            set { _isPopulatelastPriceInPriceWhenAskORBidIsZero = value; }
        }

        private bool _cleanDetailsAfterTrade = true;
        public bool CleanDetailsAfterTrade
        {
            get { return _cleanDetailsAfterTrade; }
        }

        private string _defaultInternalComments = string.Empty;
        public string DefaultInternalComments
        {
            get { return _defaultInternalComments; }
            set { _defaultInternalComments = value; }
        }

        private string _defaultBrokerComments = string.Empty;
        public string DefaultBrokerComments
        {
            get { return _defaultBrokerComments; }
            set { _defaultBrokerComments = value; }
        }

        private bool _isUseCustodianAsExecutingBroker = false;
        public bool IsUseCustodianAsExecutingBroker
        {
            get { return _isUseCustodianAsExecutingBroker; }
            set { _isUseCustodianAsExecutingBroker = value; }
        }
    }
}
