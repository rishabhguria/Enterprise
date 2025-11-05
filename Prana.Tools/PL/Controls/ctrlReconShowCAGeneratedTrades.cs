using Prana.BusinessObjects;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Windows.Forms;

namespace Prana.Tools.PL.Controls
{
    public partial class ctrlReconShowCAGeneratedTrades : UserControl
    {
        //Making Custom event SaveCAGenerateTrade
        public event EventHandler<EventArgs<bool>> SaveCAGenerateTrade;

        public ctrlReconShowCAGeneratedTrades()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// This method is used for loading the checkstate from the template,
        /// which earlier gets its value from the database table "T_ReconPrefrences"
        /// </summary>
        /// <param name="template"></param>
        internal void LoadShowCAGeneratedTrades(ReconTemplate template)
        {
            try
            {

                chkShowCAGeneratedTrades.Checked = template.IsShowCAGeneratedTrades;

            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }


        /// <summary>
        /// This method is called whenever the "chkShowCAGeneratedTrades" is clicked.
        /// It simply updates the property, i.e IsShowCAGeneratedTrades
        /// </summary>
        /// <param name="template"></param>
        internal void UpdateShowCAGeneratedTrades(ReconTemplate template)
        {
            try
            {
                template.IsShowCAGeneratedTrades = chkShowCAGeneratedTrades.Checked;
                template.IsDirtyForSaving = true;
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }



        /// <summary>
        /// The main purpose of using this event is to save the checkstate in the current template.
        /// To do this another custom event has been created "SaveCAGenerateTrade", its wiring is done
        /// on the "ctrlReconTemplate"(User Control)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkShowCAGeneratedTrades_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (SaveCAGenerateTrade != null)
                {
                    SaveCAGenerateTrade(this, new EventArgs<bool>(chkShowCAGeneratedTrades.Checked));
                }
            }
            catch (Exception ex)
            {
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

    }
}