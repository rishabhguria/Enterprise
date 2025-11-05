using Prana.Global;
using Prana.LiveFeed.UI.Controls;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prana.LiveFeed.UI.Forms
{
    public partial class FactSetAuthenticationForm : Form
    {
        /// <summary>
        /// Event to sets token for the factset user
        /// </summary>
        public event EventHandler<EventArgs<string, string>> SetTokenFactsetUser;
        /// <summary>
        /// Event to close the Factset Authentication form.
        // </summary>
        public event EventHandler CloseTheFactSetForm;
        public FactSetAuthenticationForm()
        {
            InitializeComponent();
            factSetUserControl.SetTokenFactsetUser += FactSetUserControl_SetTokenFactsetUser;
            factSetUserControl.CloseTheFactSetForm += FactSetUserControl_CloseFactsetForm;
        }

        private void FactSetUserControl_SetTokenFactsetUser(object sender, EventArgs<string, string> e)
        {
            try
            {
                if (SetTokenFactsetUser != null)
                    SetTokenFactsetUser(this, e);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        private void FactSetUserControl_CloseFactsetForm(object sender, EventArgs e)
        {
            try
            {
                if (CloseTheFactSetForm != null)
                    CloseTheFactSetForm(this, e);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

    }
}
