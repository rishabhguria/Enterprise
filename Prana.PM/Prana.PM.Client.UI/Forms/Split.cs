using Prana.BusinessObjects.PositionManagement;
using Prana.LogManager;
using System;
using System.Windows.Forms;

namespace Prana.PM.Client.UI.Forms
{
    public partial class Split : Form
    {
        public Split()
        {
            InitializeComponent();
        }


        public void SetGridDataSource(AllocatedTradesList alloactedTradelist)
        {
            try
            {
                ctrlSplit1.SetGridDataSource(alloactedTradelist);

            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        public void SetGridDataSourceForModify(AllocatedTradesList alloactedTradelist)
        {
            try
            {
                ctrlSplit1.SetGridDataSourceForModify(alloactedTradelist);

            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        private void Split_FormClosed(object sender, FormClosedEventArgs e)
        {

            this.Dispose();

        }


    }
}