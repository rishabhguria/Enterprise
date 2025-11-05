using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Nirvana.Admin.BLL;

namespace Nirvana.Admin.Controls
{
	/// <summary>
	/// Summary description for CreateCompanyTradingAccounts.
	/// </summary>
	public class CreateCompanyTradingAccounts : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

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
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnSave = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.txtShortName = new System.Windows.Forms.TextBox();
			this.txtTradingAccount = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.btnClose = new System.Windows.Forms.Button();
			this.stbCreateCompanyTradingAccounts = new System.Windows.Forms.StatusBar();
			this.statusBarPanel1 = new System.Windows.Forms.StatusBarPanel();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).BeginInit();
			this.SuspendLayout();
			// 
			// btnSave
			// 
			this.btnSave.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(204)), ((System.Byte)(102)));
			this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSave.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnSave.Location = new System.Drawing.Point(85, 80);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(72, 24);
			this.btnSave.TabIndex = 6;
			this.btnSave.Text = "Save";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.txtShortName);
			this.groupBox1.Controls.Add(this.txtTradingAccount);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
			this.groupBox1.Location = new System.Drawing.Point(10, 2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(318, 70);
			this.groupBox1.TabIndex = 5;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Trading Accounts";
			// 
			// txtShortName
			// 
			this.txtShortName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtShortName.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtShortName.Location = new System.Drawing.Point(142, 40);
			this.txtShortName.MaxLength = 50;
			this.txtShortName.Name = "txtShortName";
			this.txtShortName.Size = new System.Drawing.Size(148, 21);
			this.txtShortName.TabIndex = 2;
			this.txtShortName.Text = "";
			this.txtShortName.LostFocus += new System.EventHandler(this.txtShortName_LostFocus);
			this.txtShortName.GotFocus += new System.EventHandler(this.txtShortName_GotFocus);
			// 
			// txtTradingAccount
			// 
			this.txtTradingAccount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtTradingAccount.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtTradingAccount.Location = new System.Drawing.Point(142, 16);
			this.txtTradingAccount.MaxLength = 50;
			this.txtTradingAccount.Name = "txtTradingAccount";
			this.txtTradingAccount.Size = new System.Drawing.Size(148, 21);
			this.txtTradingAccount.TabIndex = 1;
			this.txtTradingAccount.Text = "";
			this.txtTradingAccount.LostFocus += new System.EventHandler(this.txtTradingAccount_LostFocus);
			this.txtTradingAccount.GotFocus += new System.EventHandler(this.txtTradingAccount_GotFocus);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label1.Location = new System.Drawing.Point(8, 21);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(112, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Name";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label2.Location = new System.Drawing.Point(8, 45);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(112, 16);
			this.label2.TabIndex = 0;
			this.label2.Text = "Short Name";
			// 
			// btnClose
			// 
			this.btnClose.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(51)), ((System.Byte)(51)));
			this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnClose.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnClose.Location = new System.Drawing.Point(163, 80);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(72, 24);
			this.btnClose.TabIndex = 7;
			this.btnClose.Text = "Close";
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// stbCreateCompanyTradingAccounts
			// 
			this.stbCreateCompanyTradingAccounts.Location = new System.Drawing.Point(0, 115);
			this.stbCreateCompanyTradingAccounts.Name = "stbCreateCompanyTradingAccounts";
			this.stbCreateCompanyTradingAccounts.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
																											   this.statusBarPanel1});
			this.stbCreateCompanyTradingAccounts.ShowPanels = true;
			this.stbCreateCompanyTradingAccounts.Size = new System.Drawing.Size(330, 22);
			this.stbCreateCompanyTradingAccounts.TabIndex = 8;
			this.stbCreateCompanyTradingAccounts.PanelClick += new System.Windows.Forms.StatusBarPanelClickEventHandler(this.stbCreateCompanyTradingAccounts_PanelClick);
			// 
			// statusBarPanel1
			// 
			this.statusBarPanel1.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.statusBarPanel1.Style = System.Windows.Forms.StatusBarPanelStyle.OwnerDraw;
			this.statusBarPanel1.Text = "statusBarPanel1";
			this.statusBarPanel1.Width = 314;
			// 
			// errorProvider1
			// 
			this.errorProvider1.ContainerControl = this;
			// 
			// label3
			// 
			this.label3.ForeColor = System.Drawing.Color.Red;
			this.label3.Location = new System.Drawing.Point(140, 20);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(12, 14);
			this.label3.TabIndex = 33;
			this.label3.Text = "*";
			// 
			// label4
			// 
			this.label4.ForeColor = System.Drawing.Color.Red;
			this.label4.Location = new System.Drawing.Point(140, 44);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(12, 14);
			this.label4.TabIndex = 35;
			this.label4.Text = "*";
			// 
			// CreateCompanyTradingAccounts
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.ClientSize = new System.Drawing.Size(330, 137);
			this.ControlBox = false;
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.stbCreateCompanyTradingAccounts);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnClose);
			this.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "CreateCompanyTradingAccounts";
			this.Text = "CreateCompanyTradingAccounts";
			this.Load += new System.EventHandler(this.CreateCompanyTradingAccounts_Load);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).EndInit();
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
		private System.Windows.Forms.StatusBarPanel statusBarPanel1;
		private System.Windows.Forms.StatusBar stbCreateCompanyTradingAccounts;
		private System.Windows.Forms.ErrorProvider errorProvider1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;

		Nirvana.Admin.BLL.TradingAccount _tradingAccountEdit = null;

		public Nirvana.Admin.BLL.TradingAccount TradingAccountEdit
		{
			set{_tradingAccountEdit = value;}
		}

		public void BindForEdit()
		{
			if(_tradingAccountEdit != null)
			{
				txtTradingAccount.Text = _tradingAccountEdit.TradingAccountName;
				txtShortName.Text = _tradingAccountEdit.TradingShortName;
			}
		}
		private int _companyID = int.MinValue;
		public int CompanyID
		{
			set{_companyID = value;}
		}

		private Nirvana.Admin.BLL.TradingAccounts _tradingAccounts = new TradingAccounts();
		private void btnSave_Click(object sender, System.EventArgs e)
		{
			if(_tradingAccountEdit != null)
			{
				errorProvider1.SetError(txtTradingAccount, "");
				errorProvider1.SetError(txtShortName, "");
				if(txtTradingAccount.Text.Trim() == "")
				{
					errorProvider1.SetError(txtTradingAccount, "Please enter Trading Account Name!");
					txtTradingAccount.Focus();
				}
				else if(txtShortName.Text.Trim() == "")
				{
					errorProvider1.SetError(txtShortName, "Please enter Short Name!");
					txtShortName.Focus();
				}
				else
				{
					_tradingAccountEdit.TradingAccountName = txtTradingAccount.Text.ToString();
					_tradingAccountEdit.TradingShortName = txtShortName.Text.ToString();
					Nirvana.Admin.Utility.Common.ResetStatusPanel(stbCreateCompanyTradingAccounts);
					Nirvana.Admin.Utility.Common.SetStatusPanel(stbCreateCompanyTradingAccounts, "Stored!");
				}
			}
			else
			{
				errorProvider1.SetError(txtTradingAccount, "");
				errorProvider1.SetError(txtShortName, "");
				if(txtTradingAccount.Text.Trim() == "")
				{
					errorProvider1.SetError(txtTradingAccount, "Please enter Trading Account Name!");
					txtTradingAccount.Focus();
				}
				else if(txtShortName.Text.Trim() == "")
				{
					errorProvider1.SetError(txtShortName, "Please enter Short Name!");
					txtShortName.Focus();
				}
				else
				{
					Nirvana.Admin.BLL.TradingAccount tradingAccount = new Nirvana.Admin.BLL.TradingAccount();
						
					tradingAccount.TradingAccountName = txtTradingAccount.Text.ToString();
					tradingAccount.TradingShortName = txtShortName.Text.ToString();
					_tradingAccounts.Add(tradingAccount);		
					Nirvana.Admin.Utility.Common.ResetStatusPanel(stbCreateCompanyTradingAccounts);
					Nirvana.Admin.Utility.Common.SetStatusPanel(stbCreateCompanyTradingAccounts, "Stored!");
						this.Hide();
				}
			}
		
		}
		
		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Hide();
		}

		private void CreateCompanyTradingAccounts_Load(object sender, System.EventArgs e)
		{
			if(_tradingAccountEdit != null)
			{
				BindForEdit();
			}
			else
			{
				Refresh(sender, e);
				Nirvana.Admin.Utility.Common.ResetStatusPanel(stbCreateCompanyTradingAccounts);
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
	
		public TradingAccounts CurrentCompanyTradingAccounts
		{
			get 
			{
				return _tradingAccounts; 
			}
			set
			{
				if(value != null)
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
