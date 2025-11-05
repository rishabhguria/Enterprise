using Prana.BusinessObjects;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.TradeServiceUI
{
    public partial class UsrCtrlConnectionStatus : UserControl
    {
        private PranaInternalConstants.ConnectionStatus _buySideState = PranaInternalConstants.ConnectionStatus.DISCONNECTED;
        private delegate void SetTextCallback(PranaInternalConstants.ConnectionStatus connState);
        private int _connectionID = int.MinValue;
        private string _partyName = string.Empty;

        public UsrCtrlConnectionStatus(int connectionID, string partyName)
        {
            try
            {
                InitializeComponent();

                this.btnConnect.Name = this.btnConnect.Name + "_" + partyName.ToString();
                _connectionID = connectionID;
                _partyName = partyName;

                lblPartyName.Text = _partyName;

                SetBuySideStatus(PranaInternalConstants.ConnectionStatus.Connecting);
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

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (_buySideState != PranaInternalConstants.ConnectionStatus.CONNECTED)
                {
                    SetBuySideStatus(PranaInternalConstants.ConnectionStatus.Connecting);
                    await TradeServiceManager.TradeServiceManager.GetInstance.FixEngineConnectBuySide(_connectionID);
                }
                else
                {
                    DialogResult result = MessageBox.Show("Do you Want to Disconnect this FIX Connection", "Prana Warning", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        await TradeServiceManager.TradeServiceManager.GetInstance.FixEngineDisconnectBuySide(_connectionID);
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

        private void SetBuySideStatus(PranaInternalConstants.ConnectionStatus constatus)
        {
            try
            {
                if (UIValidation.GetInstance().validate(btnConnect))
                {
                    if (btnConnect.InvokeRequired)
                    {
                        SetTextCallback callback = new SetTextCallback(SetBuySideStatus);
                        btnConnect.Invoke(callback, new object[] { constatus });
                    }
                    else
                    {
                        if (constatus == PranaInternalConstants.ConnectionStatus.Connecting)
                        {
                            btnConnect.ForeColor = Color.Red;
                            btnConnect.Enabled = false;
                            lblStatus.Text = "Connecting...";
                            btnConnect.Text = "Connecting...";
                            lblPartyName.ForeColor = Color.Red;
                            lblStatus.ForeColor = Color.Red;
                        }
                        else if (constatus == PranaInternalConstants.ConnectionStatus.CONNECTED)
                        {
                            btnConnect.Text = "Disconnect";
                            lblPartyName.ForeColor = Color.Green;
                            lblStatus.ForeColor = Color.Green;
                            btnConnect.ForeColor = Color.Red;
                            btnConnect.Enabled = true;
                        }
                        else if (constatus == PranaInternalConstants.ConnectionStatus.DISCONNECTED)
                        {
                            btnConnect.Enabled = true;
                            lblStatus.Text = "NotConnected";
                            btnConnect.Text = "Connect";

                            btnConnect.ForeColor = Color.Black;
                            lblPartyName.ForeColor = Color.Red;
                            lblStatus.ForeColor = Color.Red;
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

        private void SetSellSideStatus(PranaInternalConstants.ConnectionStatus constatus)
        {
            try
            {
                if (UIValidation.GetInstance().validate(btnConnect))
                {
                    if (btnConnect.InvokeRequired)
                    {
                        SetTextCallback callback = new SetTextCallback(SetSellSideStatus);
                        btnConnect.Invoke(callback, new object[] { constatus });
                    }
                    else
                    {
                        if (constatus == PranaInternalConstants.ConnectionStatus.CONNECTED)
                        {
                            lblStatus.Text = "Connected";
                            lblStatus.ForeColor = Color.Green;
                            lblPartyName.ForeColor = Color.Green;
                        }
                        else if (constatus == PranaInternalConstants.ConnectionStatus.DISCONNECTED)
                        {
                            if (_buySideState == PranaInternalConstants.ConnectionStatus.CONNECTED) // if buy side is up then only show if sell side is down or net
                            {
                                lblStatus.Text = "NoSellServer";
                                lblPartyName.ForeColor = Color.Red;
                                lblStatus.ForeColor = Color.Red;
                            }
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

        public void UserCtrlConnectionStatusUpdate(FixPartyDetails fixPartyDetails)
        {
            try
            {
                if (fixPartyDetails.ConnectionID == _connectionID)
                {
                    _buySideState = fixPartyDetails.BuySideStatus;
                    SetBuySideStatus(fixPartyDetails.BuySideStatus);
                    SetSellSideStatus(fixPartyDetails.BuyToSellSideStatus);
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
    }
}