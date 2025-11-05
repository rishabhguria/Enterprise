using Prana.LogManager;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.Analytics
{
    public partial class RiskReportGraphUI : Form
    {
        public RiskReportGraphUI()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void SetUp(DataTable dt)
        {
            try
            {
                chartPortfolioGrouping.BackColor = Color.White;
                chartPortfolioGrouping.DataSource = dt;
                chartPortfolioGrouping.Data.DataBind();
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}