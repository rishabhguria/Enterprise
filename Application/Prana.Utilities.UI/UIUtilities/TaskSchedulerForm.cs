using Prana.LogManager;
using System;
using System.Windows.Forms;

namespace Prana.Utilities.UI.UIUtilities
{
    public partial class TaskSchedulerForm : Form
    {
        String _cronExpression = string.Empty;
        //object[] execDates;

        public TaskSchedulerForm()
        {
            InitializeComponent();
        }

        public void GetCronToFill(String cronExpression)
        {
            try
            {
                taskScheduler1.FillUI(cronExpression);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            //execDates = new object[3];
            try
            {
                _cronExpression = taskScheduler1.GetCronExpression();
                this.DialogResult = DialogResult.OK;
                this.Hide();
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

        public String GetCronExpression()
        {
            return _cronExpression;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void TaskSchedulerForm_Load(object sender, EventArgs e)
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(sender as Form, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_TRADING_TICKET);
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
