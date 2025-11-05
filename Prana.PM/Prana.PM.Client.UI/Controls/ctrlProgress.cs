using Prana.LogManager;
using System;
using System.Text;
using System.Windows.Forms;

namespace Prana.PM.Client.UI
{
    public partial class ctrlProgress : UserControl
    {
        public ctrlProgress()
        {
            InitializeComponent();
        }

        private void panelProgress_Paint(object sender, PaintEventArgs e)
        {

        }

        private string _progressingText;
        public string ProgressingText
        {
            get { return lblProgress.Text = _progressingText; }
            set { _progressingText = lblProgress.Text = value; }
        }

        private string _importingText;
        public string ImportingText
        {
            get { return lblImporting.Text = _importingText; }
            set { _importingText = lblImporting.Text = value; }
        }

        private int _progressValue;
        public int ProgressValue
        {
            get { return pBarProgressing.Value = _progressValue; }
            set { _progressValue = pBarProgressing.Value = value; }
        }

        private void timerProgress_Tick(object sender, EventArgs e)
        {
            try
            {
                this.lblProgress.Margin = new System.Windows.Forms.Padding(20, 3, 0, 2);
                StringBuilder progress = new StringBuilder(lblProgress.Text);
                if (progress.Length < 10)
                {
                    progress.Append(". ");
                    lblProgress.Text = progress.ToString();
                }
                else
                    lblProgress.Text = String.Empty;
            }
            catch (Exception ex)
            {
                lblImporting.Text = String.Empty;
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }

        }

    }
}
