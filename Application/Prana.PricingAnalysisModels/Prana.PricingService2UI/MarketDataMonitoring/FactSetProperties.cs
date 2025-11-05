using Prana.Global;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Prana.PricingService2UI.MarketDataMonitoring
{
    public partial class FactSetProperties : Form
    {
        public event EventHandler<EventArgs<List<string>>> CredentialsUpdated;

        public FactSetProperties()
        {
            InitializeComponent();
        }

        private async void FactSetProperties_Load(object sender, EventArgs e)
        {
            List<string> credentialsList = await PricingService2Manager.PricingService2Manager.GetInstance.DataManagerSetup();

            txtBoxClientUsername.Text = credentialsList[0];
            txtBoxClientPassword.Text = credentialsList[1];
            txtBoxClientHost.Text = credentialsList[2];
            txtBoxClientPort.Text = credentialsList[3];

            txtBoxSupportUserame.Text = credentialsList[4];
            txtBoxSupportPassword.Text = credentialsList[5];
        }

        private async void buttonOk_Click(object sender, EventArgs e)
        {
            if (IsDataValid())
            {
                DialogResult response = MessageBox.Show("Clicking on OK will save connection properties & reset the FactSet connection. Do you want to continue?", "Connection properties save & reset connection", MessageBoxButtons.YesNo);
                if (response.Equals(DialogResult.Yes))
                {
                    await PricingService2Manager.PricingService2Manager.GetInstance.UpdateLiveFeedDetails(txtBoxClientUsername.Text, txtBoxClientPassword.Text, txtBoxClientHost.Text, txtBoxClientPort.Text,
                        txtBoxSupportUserame.Text, txtBoxSupportPassword.Text);
                    CredentialsUpdated(this, null);
                }

                this.Close();
            }
            else
                MessageBox.Show("Required fields missing or incorrect.");
        }

        private bool IsDataValid()
        {
            bool isDataValid = true;
            if (string.IsNullOrWhiteSpace(txtBoxClientUsername.Text) || string.IsNullOrWhiteSpace(txtBoxClientPassword.Text) || string.IsNullOrWhiteSpace(txtBoxClientHost.Text))
            {
                isDataValid = false;
            }
            else if (!string.IsNullOrWhiteSpace(txtBoxClientPort.Text))
            {
                int port = 0;
                isDataValid = (int.TryParse(txtBoxClientPort.Text, out port) && port > 0);
            }
            else if (!((string.IsNullOrWhiteSpace(txtBoxSupportUserame.Text) && string.IsNullOrWhiteSpace(txtBoxSupportPassword.Text)) ||
                (!string.IsNullOrWhiteSpace(txtBoxSupportUserame.Text) && !string.IsNullOrWhiteSpace(txtBoxSupportPassword.Text))))
            {
                isDataValid = false;
            }

            return isDataValid;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ultraExpandableGroupBox1_ExpandedStateChanging(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ultraExpandableGroupBoxSupportDetails.Expanded)
            {
                this.Size = new System.Drawing.Size(447, 253);
                this.MaximumSize = new System.Drawing.Size(447, 253);
                this.MinimumSize = new System.Drawing.Size(447, 253);
            }
            else
            {
                this.Size = new System.Drawing.Size(447, 317);
                this.MaximumSize = new System.Drawing.Size(447, 317);
                this.MinimumSize = new System.Drawing.Size(447, 317);
            }
        }
    }
}
