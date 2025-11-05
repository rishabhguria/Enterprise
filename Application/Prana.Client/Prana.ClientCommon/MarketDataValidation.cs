using Prana.CommonDataCache;
using Prana.Utilities.UI.UIUtilities;
using System.Windows.Forms;

namespace Prana.ClientCommon
{
    public static class MarketDataValidation
    {
        public static bool CheckMarketDataPermissioning()
        {
            if (!CachedDataManager.GetInstance.IsMarketDataPermissionEnabled)
            {
                string messageHeader = ClientLevelConstants.HEADER_MARKET_DATA_ALERT;
                string messageText = ClientLevelConstants.MSG_MARKET_DATA_NOT_AVAILABLE;

                CustomMessageBox popUpMessage = new CustomMessageBox(messageHeader, messageText, false, string.Empty, FormStartPosition.CenterScreen);
                popUpMessage.ShowDialog();
                return false;
            }
            else
                return true;
        }
    }
}
