using Infragistics.Win.UltraWinGrid;
using Prana.LogManager;
using System;
using System.Windows.Forms;

namespace Prana.PM.Client.UI.Controls
{
    public partial class CtrlTabularUI : UserControl
    {
        public CtrlTabularUI()
        {
            InitializeComponent();
        }

        private void grdPivotDisplay_CellDataError(object sender, CellDataErrorEventArgs e)
        {
            try
            {
                e.RestoreOriginalValue = true;
                e.RaiseErrorEvent = false;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
