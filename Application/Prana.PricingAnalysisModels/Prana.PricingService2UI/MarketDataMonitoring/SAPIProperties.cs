using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Prana.PricingService2UI.MarketDataMonitoring
{
    public partial class SAPIProperties : Form
    {
        public event EventHandler<EventArgs<List<string>>> CredentialsUpdated;
        /// <summary>
        /// To initilize the Component of UI 
        /// </summary>
        public SAPIProperties()
        {
            InitializeComponent();
        }
        /// <summary>
        /// To load the values of server address, password and port number
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SAPIProperties_Load(object sender, EventArgs e)
        {
            try
            {
                List<string> credentialsList = await PricingService2Manager.PricingService2Manager.GetInstance.DataManagerSetup();

                txtBoxServerAddress.Text = credentialsList[0];
                txtBoxPassword.Text = credentialsList[1];
                txtBoxPortNumber.Text = credentialsList[2];
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
        /// <summary>
        /// implement of OK button 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void buttonOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsDataValid())
                {
                    DialogResult response = MessageBox.Show("Clicking on OK will save connection properties & reset the Bloomberg connection. Do you want to continue?", "Connection properties save & reset connection", MessageBoxButtons.YesNo);
                    if (response.Equals(DialogResult.Yes))
                    {
                        await PricingService2Manager.PricingService2Manager.GetInstance.UpdateLiveFeedDetails(txtBoxServerAddress.Text, txtBoxPassword.Text, port: txtBoxPortNumber.Text);
                        if (CredentialsUpdated != null)
                            CredentialsUpdated(this, null);
                    }

                    this.Close();
                }
                else
                    MessageBox.Show("Required fields missing or incorrect.");
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
        /// <summary>
        /// To check the data of all textboxes are valid or not
        /// </summary>
        /// <returns></returns>
        private bool IsDataValid()
        {
            bool isDataValid = true;
            try
            {
                if (string.IsNullOrWhiteSpace(txtBoxServerAddress.Text) || string.IsNullOrWhiteSpace(txtBoxPassword.Text) || string.IsNullOrWhiteSpace(txtBoxPortNumber.Text))
                {
                    isDataValid = false;
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
            return isDataValid;

        }
        /// <summary>
        /// implementation of cancel button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
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
