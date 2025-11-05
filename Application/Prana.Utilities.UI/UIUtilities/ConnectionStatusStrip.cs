using Prana.BusinessObjects;
using Prana.LogManager;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.Utilities.UI.UIUtilities
{
    public partial class ConnectionStatusStrip : UserControl
    {
        private delegate void UIThreadMarsheller(PranaInternalConstants.ConnectionStatus status);
        public ConnectionStatusStrip()
        {
            InitializeComponent();
            ultraLabel1.Appearance.BackColor = Color.Red;
        }
        public void SetStatus(PranaInternalConstants.ConnectionStatus status)
        {
            try
            {
                UIThreadMarsheller mi = new UIThreadMarsheller(SetStatus);
                if (UIValidation.GetInstance().validate(ultraLabel1))
                {
                    if (ultraLabel1.InvokeRequired)
                    {
                        this.BeginInvoke(mi, new object[] { status });
                    }
                    else
                    {

                        ultraLabel1.Text = status.ToString();
                        switch (status)
                        {
                            case PranaInternalConstants.ConnectionStatus.CONNECTED:
                                ultraLabel1.Appearance.BackColor = Color.Green;
                                break;
                            case PranaInternalConstants.ConnectionStatus.DISCONNECTED:
                                ultraLabel1.Appearance.BackColor = Color.Red;
                                break;
                            case PranaInternalConstants.ConnectionStatus.NOSERVER:
                                ultraLabel1.Appearance.BackColor = Color.Red;
                                break;
                        }
                    }
                }
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

        public string Status
        {
            get { return ultraLabel1.Text; }
        }
    }
}
