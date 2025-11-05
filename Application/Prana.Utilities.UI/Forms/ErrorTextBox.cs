using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Windows.Forms;

namespace Prana.Utilities.UI
{
    public partial class ErrorTextBox : Form
    {
        public ErrorTextBox()
        {
            InitializeComponent();
        }

        private void errortextbox_Load(object sender, EventArgs e)
        {
            okbutton.Select();
        }

        public void SetUp(string labelmessage, string message)
        {
            try
            {
                ErrorMessageBox.Text = message;
                MessageLabel.Text = labelmessage;
                CustomThemeHelper.AddUltraFormManagerToDynamicForm(this.FindForm());
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

        private void okbutton_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
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
    }
}
