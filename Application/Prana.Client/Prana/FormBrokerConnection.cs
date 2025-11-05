#region Author Summary
///////////////////////////////////////////////////////////////////////////////
// AUTHOR   		 : Bharat Jangir
// CREATION DATE	 : 17 September 2013 
// PURPOSE	    	 : Show Counter Parties with Connected/Disconnected Status
// FILE NAME	     : $Workfile: CounterParty.cs $	
///////////////////////////////////////////////////////////////////////////////
#endregion

#region Using NameSpaces
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Windows.Forms;
#endregion

namespace Prana
{
    public partial class FormBrokerConnection : Form
    {
        public FormBrokerConnection()
        {
            InitializeComponent();
        }

        private void FormBrokerConnection_Load(object sender, EventArgs e)
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(this, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_CLOSING_WIZARD);
                usrCtrlBrokerConnectionStatusDetails.UseAppStyling = false;
                this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
                this.MaximizeBox = false;
                this.MinimizeBox = false;
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

        private void CounterParty_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.Hide();
                e.Cancel = true;
                //we hide the counterparty form instead of closing it

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