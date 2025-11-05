using Infragistics.Win;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.Utilities.UI.UIUtilities
{
    public partial class CustomMessageBox : Form
    {
        string messageForYesNo = string.Empty;
        string FOR_YES_NO = "YesNo";
        public CustomMessageBox(string title, string msg, bool enableLeftTitle = false, string leftTitle = CustomThemeHelper.REJECT_POPUP, FormStartPosition startPosition = FormStartPosition.CenterParent, MessageBoxButtons messageBoxButtons = MessageBoxButtons.OK)
        {
            InitializeComponent();
            if (CustomThemeHelper.ApplyTheme)
            {
                if (enableLeftTitle)
                {
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"Text-align:Left\">" + leftTitle + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(leftTitle, title, CustomThemeHelper.UsedFont);
                }
                else
                {
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"Text-align:Left\">" + title + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(title, string.Empty, CustomThemeHelper.UsedFont);
                }
            }
            this.MessageLabel.Text = msg;
            this.StartPosition = startPosition;

            if (messageBoxButtons == MessageBoxButtons.OK)
            {
                // tableLayoutPanel1.ColumnStyles[1].SizeType = SizeType.Absolute;
                //   tableLayoutPanel1.ColumnStyles[1].Width = 0;
                this.ultraCancelButton.Hide();
            }

            if (messageBoxButtons == MessageBoxButtons.YesNo)
            {
                messageForYesNo = messageBoxButtons.ToString();
                this.ultraOkButton.Visible = true;
                this.ultraCancelButton.Visible = true;
                this.ultraCancelButton.Text = "No";
                this.ultraOkButton.Text = "Yes";
                this.ultraOkButton.UseAppStyling = false;
                this.ultraOkButton.UseOsThemes = DefaultableBoolean.False;
                this.ultraOkButton.ButtonStyle = UIElementButtonStyle.ButtonSoft;
                this.ultraOkButton.BackColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
                this.ultraOkButton.ForeColor = Color.White;
                this.ultraCancelButton.UseAppStyling = false;
                this.ultraCancelButton.UseOsThemes = DefaultableBoolean.False;
                this.ultraCancelButton.ButtonStyle = UIElementButtonStyle.ButtonSoft;
                this.ultraCancelButton.BackColor = Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
                this.ultraCancelButton.ForeColor = Color.White;
            }
            if (messageBoxButtons == MessageBoxButtons.RetryCancel)
            {
                messageForYesNo = messageBoxButtons.ToString();
                this.ultraOkButton.Visible = true;
                this.ultraCancelButton.Visible = true;
                this.ultraCancelButton.Text = "Cancel";
                this.ultraOkButton.Text = "Proceed";
                this.ultraOkButton.UseAppStyling = false;
                this.ultraOkButton.UseOsThemes = DefaultableBoolean.False;
                this.ultraOkButton.ButtonStyle = UIElementButtonStyle.ButtonSoft;
                this.ultraOkButton.BackColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
                this.ultraOkButton.ForeColor = Color.White;
                this.ultraCancelButton.UseAppStyling = false;
                this.ultraCancelButton.UseOsThemes = DefaultableBoolean.False;
                this.ultraCancelButton.ButtonStyle = UIElementButtonStyle.ButtonSoft;
                this.ultraCancelButton.BackColor = Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
                this.ultraCancelButton.ForeColor = Color.White;
            }
            if (title.Equals("Warning"))
            {
                this.ultraCancelButton.Text = "Discard";
                this.ultraOkButton.Text = "Save Changes";
                this.ultraOkButton.Size = new System.Drawing.Size(110, 24);
                this.ultraCancelButton.Size = new System.Drawing.Size(110, 24);
            }
        }

        public void SetMessageBoxText(string messageText)
        {
            if (this.InvokeRequired)
            {
                MethodInvoker del = delegate
                {
                    SetMessageBoxText(messageText);
                };
                BeginInvoke(del);
            }
            else
            {
                this.MessageLabel.Text = messageText;
            }
        }

        private void ultraOkButton_Click(object sender, EventArgs e)
        {
            if (messageForYesNo.Equals(FOR_YES_NO))
            {
                this.DialogResult = DialogResult.Yes;
                this.Close();
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void ultraCancelButton_Click(object sender, EventArgs e)
        {
            if (messageForYesNo.Equals(FOR_YES_NO))
            {
                this.DialogResult = DialogResult.No;
                this.Close();
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }
    }
}
