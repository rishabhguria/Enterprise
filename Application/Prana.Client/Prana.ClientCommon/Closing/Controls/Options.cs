using System;
using System.Windows.Forms;

namespace Prana.ClientCommon
{
    public partial class Options : UserControl
    {
        public event EventHandler ClosingOptionSelected;
        public Options()
        {
            InitializeComponent();
        }


        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void rdBtnRunClosing_CheckedChanged(object sender, EventArgs e)
        {

            if (rdBtnRunClosing.Checked)
            {
                if (ClosingOptionSelected != null)
                {
                    this.ClosingOptionSelected(null, null);
                }
            }
        }

        private void rdBtnPreviewData_CheckedChanged(object sender, EventArgs e)
        {
            if (rdBtnPreviewData.Checked)
            {
                if (ClosingOptionSelected != null)
                {
                    this.ClosingOptionSelected(null, null);
                }
            }
        }

        private void rdBtnAddUnwindingTemplate_CheckedChanged(object sender, EventArgs e)
        {

            if (rdBtnAddUnwindingTemplate.Checked)
            {
                if (ClosingOptionSelected != null)
                {
                    this.ClosingOptionSelected(null, null);
                }
            }
        }

        private void rdbtnEditTemplate_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbtnEditTemplate.Checked)
            {
                if (ClosingOptionSelected != null)
                {
                    this.ClosingOptionSelected(null, null);
                }
            }
        }

        private void rdBtnDeleteTemplate_CheckedChanged(object sender, EventArgs e)
        {

            if (rdBtnDeleteTemplate.Checked)
            {

                if (ClosingOptionSelected != null)
                {
                    this.ClosingOptionSelected(null, null);
                }
            }

        }

        private void rdbtnAddClosingTemplate_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbtnAddClosingTemplate.Checked)
            {
                if (ClosingOptionSelected != null)
                {
                    this.ClosingOptionSelected(null, null);
                }
            }
        }


    }
}