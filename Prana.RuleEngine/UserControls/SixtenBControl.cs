using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;

namespace Prana.RuleEngine
{
    public delegate void SixtenBEventDelegate(object sender, String symbol, DateTime date);
    public partial class SixtenBControl : System.Windows.Forms.UserControl
    {
        public event EventHandler cancelClick;
        public event SixtenBEventDelegate submitClick;

        public SixtenBControl()
        {
            InitializeComponent();
        }

        private void btSubmit_Click(object sender, EventArgs e)
        {

            //if (txtDate.Text != String.Empty && txtSymbol.Text != String.Empty)
            //{

            //    MessageBox.Show(txtSymbol.Text + " rule " + txtDate.Text);
            //}

            try
            {
                if (submitClick != null)
                    submitClick(this, txtSymbol.Text, txtDate.DateTime);
                txtSymbol.Clear();
                txtDate.DateTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (cancelClick != null)
                    cancelClick(this, null);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        internal void SetSixteenBRule(string symbol, DateTime date)
        {
            
            txtSymbol.Text = symbol;
            txtDate.DateTime = date;
        }
    }
}
