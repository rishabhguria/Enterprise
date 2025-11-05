using Prana.BusinessObjects.AppConstants;

namespace Prana.BusinessObjects
{
    public class UserSettingConstants
    {
        private static bool _isDebugModeEnabled = false;
        private static PriceUsedType _optPriceUsed = PriceUsedType.Mid;
        private static PriceUsedType _underLyingPriceUsed = PriceUsedType.Last;
        private static bool _optUseLastifMidZero = true;
        private static bool _underlyingUseLastifMidZero = true;
        private static bool _isRiskLoggingEnabled = false;
        private static int _RISKAPIEachCalDurationThreasholdToLog;

        public static bool IsDebugModeEnabled
        {
            set { _isDebugModeEnabled = value; }
            get
            {
                return _isDebugModeEnabled;
            }
        }

        public static PriceUsedType OptPriceUsed
        {
            set
            {
                _optPriceUsed = value;
            }
            get
            {
                return _optPriceUsed;
            }
        }

        public static PriceUsedType UnderLyingPriceUsed
        {
            set
            {
                _underLyingPriceUsed = value;
            }
            get
            {
                return _underLyingPriceUsed;
            }
        }

        public static bool OptUseLastifMidZero
        {
            set
            {
                _optUseLastifMidZero = value;
            }
            get
            {
                return _optUseLastifMidZero;
            }
        }

        public static bool UnderlyingUseLastifMidZero
        {
            set
            {
                _underlyingUseLastifMidZero = value;
            }
            get
            {
                return _underlyingUseLastifMidZero;
            }
        }

        public static bool IsRiskLoggingEnabled
        {
            set
            {
                _isRiskLoggingEnabled = value;
            }
            get
            {
                return _isRiskLoggingEnabled;
            }
        }

        public static int RISKAPIEachCalDurationThreasholdToLog
        {
            set
            {
                _RISKAPIEachCalDurationThreasholdToLog = value;
            }
            get
            {
                return _RISKAPIEachCalDurationThreasholdToLog;
            }
        }
    }
}
