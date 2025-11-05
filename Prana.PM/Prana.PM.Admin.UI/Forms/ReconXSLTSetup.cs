using Prana.LogManager;
using System;
using System.Windows.Forms;

namespace Prana.PM.Admin.UI.Forms
{
    public partial class ReconXSLTSetup : Form
    {
        public ReconXSLTSetup()
        {
            InitializeComponent();
        }

        private void ReconXSLTSetup_Load(object sender, EventArgs e)
        {
            try
            {
                this.ctrlReconXSLTSetup1.PopulateReconSetUpDetails();
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ctrlReconXSLTSetup1.SaveReconXSLTDetails();
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

        private void btnRemove_Click(object sender, EventArgs e)
        {
            ctrlReconXSLTSetup1.RemoveReconXSLTDetails();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ctrlReconXSLTSetup1.AddReconXSLTDetails();
        }
    }
}