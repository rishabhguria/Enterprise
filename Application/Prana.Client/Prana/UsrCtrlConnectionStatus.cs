using Infragistics.Win.Misc;
using Prana.BusinessObjects;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Drawing;

namespace Prana
{
    public partial class UsrCtrlConnectionStatus : UltraPanel
    {
        delegate void SetTextCallback(PranaInternalConstants.ConnectionStatus connState);
        string _name = string.Empty;
        public UsrCtrlConnectionStatus(string name)
        {
            InitializeComponent();
            _name = name;
            lblCpName.Text = _name;
        }

        public void UserCtrlConnectionStatusUpdate(PranaInternalConstants.ConnectionStatus connState)
        {
            try
            {
                if (UIValidation.GetInstance().validate(lblCpName))
                {
                    if (this.InvokeRequired)
                    {
                        SetTextCallback mi = new SetTextCallback(UserCtrlConnectionStatusUpdate);
                        this.BeginInvoke(mi, connState);
                    }
                    else
                    {
                        if (connState == PranaInternalConstants.ConnectionStatus.CONNECTED)
                        {
                            lblCpName.ForeColor = Color.Green;
                        }
                        if (connState == PranaInternalConstants.ConnectionStatus.DISCONNECTED)
                        {
                            lblCpName.ForeColor = Color.Red;
                        }
                    }
                }
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
    }
}
