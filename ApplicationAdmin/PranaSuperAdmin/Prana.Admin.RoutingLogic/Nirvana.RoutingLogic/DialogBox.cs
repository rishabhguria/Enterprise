using System.Windows.Forms;

namespace Prana.Admin.RoutingLogic.Forms
{
    /// <summary>
    /// Summary description for DialogBox.
    /// </summary>
    public class DialogBox : System.Windows.Forms.Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.Windows.Forms.Label labelMessage;
        private Infragistics.Win.Misc.UltraButton btnNo;
        private Infragistics.Win.Misc.UltraButton btnYes;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;


        public DialogBox(string strMessage)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            this.labelMessage.Text = strMessage;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (labelMessage != null)
                {
                    labelMessage.Dispose();
                }
                if (btnNo != null)
                {
                    btnNo.Dispose();
                }
                if (btnYes != null)
                {
                    btnYes.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DialogBox));
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            this.labelMessage = new System.Windows.Forms.Label();
            this.btnNo = new Infragistics.Win.Misc.UltraButton();
            this.btnYes = new Infragistics.Win.Misc.UltraButton();
            this.SuspendLayout();
            // 
            // labelMessage
            // 
            this.labelMessage.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.labelMessage.Location = new System.Drawing.Point(28, 6);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(440, 48);
            this.labelMessage.TabIndex = 3;
            this.labelMessage.Text = "Message";
            this.labelMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnNo
            // 
            this.btnNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.ImageBackground = ((System.Drawing.Image)(resources.GetObject("appearance1.ImageBackground")));
            this.btnNo.Appearance = appearance1;
            this.btnNo.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnNo.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.btnNo.Location = new System.Drawing.Point(296, 88);
            this.btnNo.Name = "btnNo";
            appearance2.ImageBackground = ((System.Drawing.Image)(resources.GetObject("appearance2.ImageBackground")));
            this.btnNo.PressedAppearance = appearance2;
            this.btnNo.TabIndex = 4;
            this.btnNo.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.btnNo.Click += new System.EventHandler(this.DialogResultNo);
            // 
            // btnYes
            // 
            this.btnYes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance3.BackColor = System.Drawing.Color.Transparent;
            appearance3.ImageBackground = ((System.Drawing.Image)(resources.GetObject("appearance3.ImageBackground")));
            this.btnYes.Appearance = appearance3;
            this.btnYes.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.btnYes.Location = new System.Drawing.Point(120, 88);
            this.btnYes.Name = "btnYes";
            this.btnYes.TabIndex = 5;
            this.btnYes.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.btnYes.Click += new System.EventHandler(this.DialogResultYes);
            // 
            // DialogBox
            // 
            this.AcceptButton = this.btnYes;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
            this.CancelButton = this.btnNo;
            this.ClientSize = new System.Drawing.Size(526, 150);
            this.ControlBox = false;
            this.Controls.Add(this.btnYes);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.labelMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(528, 152);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(528, 152);
            this.Name = "DialogBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Are You Sure ?";
            this.TopMost = true;
            this.ResumeLayout(false);

        }
        #endregion

        #region EventHandler for Yes

        private void DialogResultYes(object sender, System.EventArgs e)
        {
            this.Hide();
            this.DialogResult = DialogResult.Yes;
        }
        #endregion

        #region EventHandler for No
        private void DialogResultNo(object sender, System.EventArgs e)
        {
            this.Hide();
            this.DialogResult = DialogResult.No;
        }
        #endregion

    }
}
