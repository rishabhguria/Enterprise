using Prana.LogManager;
using System;
using System.Windows.Forms;

namespace Prana.Tools.PL
{
    public partial class CompanyNameMapper : Form
    {

        public CompanyNameMapper()
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

        //private void btnFileOpen_Click(object sender, EventArgs e)
        //{
        //}


    }
}