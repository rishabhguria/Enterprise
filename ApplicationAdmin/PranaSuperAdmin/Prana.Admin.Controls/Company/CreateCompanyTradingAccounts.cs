using Prana.Admin.BLL;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.Admin.Controls
{
    /// <summary>
    /// Summary description for CreateCompanyTradingAccounts.
    /// </summary>
    public class CreateCompanyTradingAccounts : System.Windows.Forms.Form
    {
        private IContainer components;

        public CreateCompanyTradingAccounts()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
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
                if (txtTradingAccount != null)
                {
                    txtTradingAccount.Dispose();
                }
                if (label1 != null)
                {
                    label1.Dispose();
                }
                if (label2 != null)
                {
                    label2.Dispose();
                }
                if (btnClose != null)
                {
                    btnClose.Dispose();
                }
                if (txtShortName != null)
                {
                    txtShortName.Dispose();
                }
                if (errorProvider1 != null)
                {
                    errorProvider1.Dispose();
                }
                if (label3 != null)
                {
                    label3.Dispose();
                }
                if (label4 != null)
                {
                    label4.Dispose();
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateCompanyTradingAccounts));
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtShortName = new System.Windows.Forms.TextBox();
            this.txtTradingAccount = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSave.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnSave.Location = new System.Drawing.Point(61, 76);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtShortName);
            this.groupBox1.Controls.Add(this.txtTradingAccount);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.groupBox1.Location = new System.Drawing.Point(2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(270, 66);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Trading Accounts";
            // 
            // txtShortName
            // 
            this.txtShortName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtShortName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtShortName.Location = new System.Drawing.Point(102, 40);
            this.txtShortName.MaxLength = 50;
            this.txtShortName.Name = "txtShortName";
            this.txtShortName.Size = new System.Drawing.Size(148, 21);
            this.txtShortName.TabIndex = 1;
            this.txtShortName.GotFocus += new System.EventHandler(this.txtShortName_GotFocus);
            this.txtShortName.LostFocus += new System.EventHandler(this.txtShortName_LostFocus);
            // 
            // txtTradingAccount
            // 
            this.txtTradingAccount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTradingAccount.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtTradingAccount.Location = new System.Drawing.Point(102, 18);
            this.txtTradingAccount.MaxLength = 50;
            this.txtTradingAccount.Name = "txtTradingAccount";
            this.txtTradingAccount.Size = new System.Drawing.Size(148, 21);
            this.txtTradingAccount.TabIndex = 0;
            this.txtTradingAccount.GotFocus += new System.EventHandler(this.txtTradingAccount_GotFocus);
            this.txtTradingAccount.LostFocus += new System.EventHandler(this.txtTradingAccount_LostFocus);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label1.Location = new System.Drawing.Point(8, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label2.Location = new System.Drawing.Point(8, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "Short Name";
            // 
            // label3
            // 
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(50, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(12, 8);
            this.label3.TabIndex = 33;
            this.label3.Text = "*";
            // 
            // label4
            // 
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(82, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(12, 8);
            this.label4.TabIndex = 35;
            this.label4.Text = "*";
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnClose.Location = new System.Drawing.Point(138, 76);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // CreateCompanyTradingAccounts
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(274, 140);
            this.ControlBox = false;
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnClose);
            this.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(268, 142);
            this.Name = "CreateCompanyTradingAccounts";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Client Trading Accounts";
            this.Load += new System.EventHandler(this.CreateCompanyTradingAccounts_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtTradingAccount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox txtShortName;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;

        Prana.Admin.BLL.TradingAccount _tradingAccountEdit = null;

        public Prana.Admin.BLL.TradingAccount TradingAccountEdit
        {
            set { _tradingAccountEdit = value; }
        }

        public void BindForEdit()
        {
            if (_tradingAccountEdit != null)
            {
                txtTradingAccount.Text = _tradingAccountEdit.TradingAccountName;
                txtShortName.Text = _tradingAccountEdit.TradingShortName;
            }
        }
        private int _companyID = int.MinValue;
        public int CompanyID
        {
            set { _companyID = value; }
        }

        private Prana.Admin.BLL.TradingAccounts _tradingAccounts = new TradingAccounts();
        private void btnSave_Click(object sender, System.EventArgs e)
        {
            bool validTradingAccount = true;
            if (_noData == 1)
            {
                _tradingAccounts.Clear();
            }
            if (_tradingAccountEdit != null)
            {
                errorProvider1.SetError(txtTradingAccount, "");
                errorProvider1.SetError(txtShortName, "");
                if (txtTradingAccount.Text.Trim() == "")
                {
                    errorProvider1.SetError(txtTradingAccount, "Please enter Trading Account Name!");
                    txtTradingAccount.Focus();
                }
                else if (txtShortName.Text.Trim() == "")
                {
                    errorProvider1.SetError(txtShortName, "Please enter Short Name!");
                    txtShortName.Focus();
                }
                else
                {
                    foreach (TradingAccount checkTradingAccount in _tradingAccounts)
                    {
                        if (checkTradingAccount.TradingAccountName.Trim().ToUpper() == txtTradingAccount.Text.Trim().ToUpper() && checkTradingAccount.TradingAccountName != _tradingAccountEdit.TradingAccountName)
                        {
                            validTradingAccount = false;
                            MessageBox.Show(this, "The edited trading account with the same name already exists.", "Alert", MessageBoxButtons.OK);
                            break;
                        }
                        //Modified By faisal Shah
                        //Dated 01/07/14
                        //http://jira.nirvanasolutions.com:8080/browse/CHMW-911
                        if (checkTradingAccount.TradingShortName.Trim().ToUpper() == txtShortName.Text.Trim().ToUpper() && checkTradingAccount.TradingShortName != _tradingAccountEdit.TradingShortName)
                        {
                            validTradingAccount = false;
                            MessageBox.Show(this, "The edited trading account with the same short name already exists.", "Alert", MessageBoxButtons.OK);
                            break;
                        }
                    }
                    if (validTradingAccount == true)
                    {
                        _tradingAccountEdit.TradingAccountName = txtTradingAccount.Text.ToString().Trim();
                        _tradingAccountEdit.TradingShortName = txtShortName.Text.ToString().Trim();
                        //Prana.Admin.Utility.Common.ResetStatusPanel(stbCreateCompanyTradingAccounts);
                        //Prana.Admin.Utility.Common.SetStatusPanel(stbCreateCompanyTradingAccounts, "Stored!");
                        this.Hide();
                    }
                }
            }
            else
            {
                errorProvider1.SetError(txtTradingAccount, "");
                errorProvider1.SetError(txtShortName, "");
                if (txtTradingAccount.Text.Trim() == "")
                {
                    errorProvider1.SetError(txtTradingAccount, "Please enter Trading Account Name!");
                    txtTradingAccount.Focus();
                }
                else if (txtShortName.Text.Trim() == "")
                {
                    errorProvider1.SetError(txtShortName, "Please enter Short Name!");
                    txtShortName.Focus();
                }
                else
                {
                    foreach (TradingAccount checkTradingAccount in _tradingAccounts)
                    {
                        if (checkTradingAccount.TradingAccountName.Trim().ToUpper() == txtTradingAccount.Text.Trim().ToUpper())
                        {
                            validTradingAccount = false;
                            MessageBox.Show(this, "The trading account with the same name already exists.", "Alert", MessageBoxButtons.OK);
                            break;
                        }
                        if (checkTradingAccount.TradingShortName.Trim().ToUpper() == txtShortName.Text.Trim().ToUpper())
                        {
                            validTradingAccount = false;
                            MessageBox.Show(this, "The trading account with the same short name already exists.", "Alert", MessageBoxButtons.OK);
                            break;
                        }
                    }
                    if (validTradingAccount == true)
                    {
                        Prana.Admin.BLL.TradingAccount tradingAccount = new Prana.Admin.BLL.TradingAccount();

                        tradingAccount.TradingAccountName = txtTradingAccount.Text.ToString().Trim();
                        tradingAccount.TradingShortName = txtShortName.Text.ToString().Trim();
                        _tradingAccounts.Add(tradingAccount);
                        //Prana.Admin.Utility.Common.ResetStatusPanel(stbCreateCompanyTradingAccounts);
                        //Prana.Admin.Utility.Common.SetStatusPanel(stbCreateCompanyTradingAccounts, "Stored!");
                        this.Hide();
                    }
                }
            }
            //this.Close();
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            errorProvider1.SetError(txtTradingAccount, "");
            errorProvider1.SetError(txtShortName, "");
            this.Close();
        }

        private void CreateCompanyTradingAccounts_Load(object sender, System.EventArgs e)
        {
            if (_tradingAccountEdit != null)
            {
                BindForEdit();
            }
            else
            {
                Refresh(sender, e);
                //Prana.Admin.Utility.Common.ResetStatusPanel(stbCreateCompanyTradingAccounts);
            }
        }

        public void Refresh(object sender, System.EventArgs e)
        {
            txtTradingAccount.Text = "";
            txtShortName.Text = "";
        }

        private void stbCreateCompanyTradingAccounts_PanelClick(object sender, System.Windows.Forms.StatusBarPanelClickEventArgs e)
        {

        }

        private int _noData = int.MinValue;
        public int NoData
        {
            set
            {
                _noData = value;
            }
        }

        public TradingAccounts CurrentCompanyTradingAccounts
        {
            get
            {
                return _tradingAccounts;
            }
            set
            {
                if (value != null)
                {
                    _tradingAccounts = value;
                }
            }
        }
        #region Focus Colors
        private void txtTradingAccount_GotFocus(object sender, System.EventArgs e)
        {
            txtTradingAccount.BackColor = Color.LemonChiffon;
        }
        private void txtTradingAccount_LostFocus(object sender, System.EventArgs e)
        {
            txtTradingAccount.BackColor = Color.White;
        }
        private void txtShortName_GotFocus(object sender, System.EventArgs e)
        {
            txtShortName.BackColor = Color.LemonChiffon;
        }
        private void txtShortName_LostFocus(object sender, System.EventArgs e)
        {
            txtShortName.BackColor = Color.White;
        }
        #endregion
    }
}
