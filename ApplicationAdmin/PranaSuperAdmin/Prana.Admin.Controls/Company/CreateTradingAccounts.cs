using System.Drawing;

namespace Prana.Admin.Controls.Company
{
    /// <summary>
    /// Summary description for CreateTradingAccounts.
    /// </summary>
    public class CreateTradingAccounts : System.Windows.Forms.UserControl
    {
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtShortName;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox txtTradingAccount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public CreateTradingAccounts()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call

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
                if (btnSave != null)
                {
                    btnSave.Dispose();
                }
                if (groupBox1 != null)
                {
                    groupBox1.Dispose();
                }
                if (label1 != null)
                {
                    label1.Dispose();
                }
                if (label2 != null)
                {
                    label2.Dispose();
                }
                if (txtShortName != null)
                {
                    txtShortName.Dispose();
                }
                if (btnClose != null)
                {
                    btnClose.Dispose();
                }
                if (txtTradingAccount != null)
                {
                    txtTradingAccount.Dispose();
                }
                if (label3 != null)
                {
                    label3.Dispose();
                }
                if (label4 != null)
                {
                    label4.Dispose();
                }
                if (btnSave != null)
                {
                    btnSave.Dispose();
                }
                if (groupBox1 != null)
                {
                    groupBox1.Dispose();
                }
                if (label1 != null)
                {
                    label1.Dispose();
                }
                if (label2 != null)
                {
                    label2.Dispose();
                }
                if (txtShortName != null)
                {
                    txtShortName.Dispose();
                }
                if (btnSave != null)
                {
                    btnSave.Dispose();
                }
                if (groupBox1 != null)
                {
                    groupBox1.Dispose();
                }
                if (label1 != null)
                {
                    label1.Dispose();
                }
                if (label2 != null)
                {
                    label2.Dispose();
                }
                if (txtShortName != null)
                {
                    txtShortName.Dispose();
                }
                if (btnClose != null)
                {
                    btnClose.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateTradingAccounts));
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTradingAccount = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtShortName = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSave.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnSave.Location = new System.Drawing.Point(75, 68);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtTradingAccount);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtShortName);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.groupBox1.Location = new System.Drawing.Point(2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(294, 66);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Trading Accounts";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // label3
            // 
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(108, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(12, 10);
            this.label3.TabIndex = 0;
            this.label3.Text = "*";
            // 
            // txtTradingAccount
            // 
            this.txtTradingAccount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTradingAccount.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtTradingAccount.Location = new System.Drawing.Point(126, 20);
            this.txtTradingAccount.MaxLength = 50;
            this.txtTradingAccount.Name = "txtTradingAccount";
            this.txtTradingAccount.Size = new System.Drawing.Size(148, 21);
            this.txtTradingAccount.TabIndex = 1;
            this.txtTradingAccount.GotFocus += new System.EventHandler(this.txtTradingAccount_GotFocus);
            this.txtTradingAccount.LostFocus += new System.EventHandler(this.txtTradingAccount_LostFocus);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label1.Location = new System.Drawing.Point(10, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Trading Account";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label2.Location = new System.Drawing.Point(10, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "Short Name";
            // 
            // txtShortName
            // 
            this.txtShortName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtShortName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtShortName.Location = new System.Drawing.Point(126, 42);
            this.txtShortName.MaxLength = 50;
            this.txtShortName.Name = "txtShortName";
            this.txtShortName.Size = new System.Drawing.Size(148, 21);
            this.txtShortName.TabIndex = 1;
            this.txtShortName.GotFocus += new System.EventHandler(this.txtShortName_GotFocus);
            this.txtShortName.LostFocus += new System.EventHandler(this.txtShortName_LostFocus);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnClose.Location = new System.Drawing.Point(151, 68);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.UseVisualStyleBackColor = false;
            // 
            // label4
            // 
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(86, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(12, 10);
            this.label4.TabIndex = 35;
            this.label4.Text = "*";
            // 
            // CreateTradingAccounts
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnClose);
            this.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.Name = "CreateTradingAccounts";
            this.Size = new System.Drawing.Size(300, 101);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion
        #region Focus Colors
        private void txtShortName_GotFocus(object sender, System.EventArgs e)
        {
            txtShortName.BackColor = Color.LemonChiffon;
        }
        private void txtShortName_LostFocus(object sender, System.EventArgs e)
        {
            txtShortName.BackColor = Color.White;
        }
        private void txtTradingAccount_GotFocus(object sender, System.EventArgs e)
        {
            txtTradingAccount.BackColor = Color.LemonChiffon;
        }
        private void txtTradingAccount_LostFocus(object sender, System.EventArgs e)
        {
            txtTradingAccount.BackColor = Color.White;
        }
        #endregion


        private void groupBox1_Enter(object sender, System.EventArgs e)
        {

        }
    }
}
