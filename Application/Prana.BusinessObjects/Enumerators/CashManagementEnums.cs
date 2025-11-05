using Prana.BusinessObjects.AppConstants;

namespace Prana.BusinessObjects.Enumerators
{
    public class CashManagementEnums
    {
        public enum ActivitySource
        {
            [EnumDescriptionAttribute("Non Trading Transaction")]
            Non_Trading_Transaction,
            [EnumDescriptionAttribute("Trading Transaction")]
            Trading_Transaction,
            [EnumDescriptionAttribute("Dividend")]
            Dividend,
            [EnumDescriptionAttribute("Revaluation")]
            Revaluation,
            [EnumDescriptionAttribute("Opening Balance")]
            Opening_Balance
        }

        public enum OperationMode
        {
            DailyProcess = 1,
            Implementation = 2
        }
    }
}
