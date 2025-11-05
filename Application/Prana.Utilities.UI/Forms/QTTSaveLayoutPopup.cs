using System;
using System.Windows.Forms;

namespace Prana.Utilities.UI.Forms
{
    public partial class QTTSaveLayoutPopup : Form
    {
        public string InstanceName
        {
            get
            {
                return txtInstanceName.Text.Trim();
            }
        }
        public QTTSaveLayoutPopup()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            String name = txtInstanceName.Text.Trim();
            Boolean isValid = true;
            lblErrMsg1.Appearance.ForeColor = System.Drawing.Color.Gray;
            lblErrMsg2.Appearance.ForeColor = System.Drawing.Color.Gray;
            if (name.Length < 8 || name.Length > 50)
            {
                lblErrMsg1.Appearance.ForeColor = System.Drawing.Color.Red;
                isValid = false;
            }
            foreach (char c in name)
            {
                if (!(Char.IsLetterOrDigit(c) || c == '_' || c == ' ' || c == '-'))
                {
                    lblErrMsg2.Appearance.ForeColor = System.Drawing.Color.Red;
                    isValid = false;
                    break;
                }
            }
            if (isValid)
            {
                DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
