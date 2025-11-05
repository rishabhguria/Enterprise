using Prana.LogManager;
using System;
using System.Windows.Forms;

namespace Prana.Tools
{
    public partial class ctrlReport : UserControl
    {


        public ctrlReport()
        {
            try
            {
                InitializeComponent();
                //_dictReconTemplates = ReconPrefManager.ReconPreferences.DictReconTemplates;
                //BindClientCombo();
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


        internal void InitializeDataSources()
        {
            try
            {
                ctrlAuditTrail1.drawsthedefaultgridstructure();
                ctrlExcepptionReport1.InitializeDataSources();
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
