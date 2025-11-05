using Infragistics.Win.UltraWinGrid;
using Prana.Admin.BLL;
using Prana.LogManager;
using System;

namespace Prana.ClientCommon
{
    public class GridMarketDataColumnUtil
    {
        public static void hideNonPermitMarketDataColumns(string moduleName, UltraGrid grid)
        {
            try
            {
                foreach (UltraGridColumn column in grid.DisplayLayout.Bands[0].Columns)
                {
                    if (ModuleManager.isMDHideColumn(moduleName, column.Key))
                    {
                        column.Hidden = true;
                        column.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                        //grid.DisplayLayout.Bands[0].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    }
                    grid.Refresh();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
