using Prana.LogManager;
using System;
using System.Windows.Forms;

namespace Prana.Admin.Controls
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class SymbolLevelAccrualUtility : Form
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message">The message.</param>
        internal delegate void ShowMessage(string message);

        /// <summary>
        /// The disp message
        /// </summary>
        private ShowMessage _dispMessage;

        /// <summary>
        /// Gets the disp message.
        /// </summary>
        /// <value>
        /// The disp message.
        /// </value>
        internal ShowMessage DispMessage
        {
            get { return _dispMessage; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolLevelAccrualUtility"/> class.
        /// </summary>
        public SymbolLevelAccrualUtility()
        {
            InitializeComponent();
            _dispMessage = new ShowMessage(DisplayMessage);
        }

        /// <summary>
        /// Displays the message.
        /// </summary>
        /// <param name="message">The message.</param>
        internal void EnableFinish()
        {
            try
            {
                if (finishButton.InvokeRequired)
                {
                    finishButton.BeginInvoke(new System.Action(() => EnableFinish()));
                }
                else
                {
                    finishButton.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Displays the message.
        /// </summary>
        /// <param name="message">The message.</param>
        private void DisplayMessage(string message)
        {
            try
            {
                if (MsgListBox.InvokeRequired)
                {
                    MsgListBox.BeginInvoke(new System.Action(() => DisplayMessage(message)));
                }
                else
                {
                    MsgListBox.Items.Add("[" + DateTime.Now + "]: " + message);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the finishButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void finishButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
