using Prana.LogManager;
using System;
using System.Windows.Forms;

namespace Prana.Tools
{
    public partial class frmAdHocRecon : Form
    {
        public frmAdHocRecon()
        {
            try
            {
                InitializeComponent();
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
