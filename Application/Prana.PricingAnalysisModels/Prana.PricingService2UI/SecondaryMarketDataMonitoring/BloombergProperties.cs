using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Prana.PricingService2UI.SecondaryMarketDataMonitoring
{
    public partial class BloombergProperties : Form
    {
        public BloombergProperties()
        {
            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private async void BloombergProperties_Load(object sender, EventArgs e)
        {
            List<string> credentialsList = await PricingService2Manager.PricingService2Manager.GetInstance.SecondaryDataManagerSetup();
            txtBoxUsername.Text = credentialsList[0];
            txtBoxPassword.Text = credentialsList[1];
        }
        //public event EventHandler<EventArgs<List<string>>> CredentialsUpdated1;
        private async void buttonOk_Click(object sender, EventArgs e)
        {
            if (IsDataValid())
            {
                DialogResult response = MessageBox.Show("Clicking on OK will save connection properties & reset the Bloomberg connection. Do you want to continue?", "Connection properties save & reset connection", MessageBoxButtons.YesNo);
                if (response.Equals(DialogResult.Yes))
                {
                    await PricingService2Manager.PricingService2Manager.GetInstance.SecondaryUpdateLiveFeedDetails(txtBoxUsername.Text, txtBoxPassword.Text);
                    //CredentialsUpdated1(this, null);
                }

                this.Close();
            }

            else
                MessageBox.Show("Required fields missing or incorrect.");
        }
        private bool IsDataValid()
        {
            bool isDataValid = true;
            if (string.IsNullOrWhiteSpace(txtBoxUsername.Text) || string.IsNullOrWhiteSpace(txtBoxPassword.Text))
            {
                isDataValid = false;
            }

            return isDataValid;
        }
    }
}

