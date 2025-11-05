using Prana.BusinessObjects.AppConstants;

namespace Prana.BusinessObjects
{
    public static class SettlementCachePreferences
    {
        static SettlementAutoCalculateField _settlementAutoCalculateField = 0;
        public static SettlementAutoCalculateField SettlementAutoCalculateField
        {
            get { return _settlementAutoCalculateField; }
            set { _settlementAutoCalculateField = value; }
        }
    }
}
