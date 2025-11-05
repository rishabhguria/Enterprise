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
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).BeginInit();
			this.SuspendLayout();
			// 
			// btnSave
			// 
			this.btnSave.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnSave.Location = new System.Drawing.Point(72, 88);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(72, 24);
			this.btnSave.TabIndex = 7;
			this.btnSave.Text = "Save";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.txtShortName);
			this.groupBox1.Controls.Add(this.txtTradingAccount);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.groupBox1.Location = new System.Drawing.Point(8, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(312, 70);
			this.groupBox1.TabIndex = 5;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Trading Accounts";
			// 
			// txtShortName
			// 
			this.txtShortName.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtShortName.Location = new System.Drawing.Point(148, 40);
			this.txtShortName.MaxLength = 50;
			this.txtShortName.Name = "txtShortName";
			this.txtShortName.Size = new System.Drawing.Size(148, 21);
			this.txtShortName.TabIndex = 2;
			this.txtShortName.Text = "";
			// 
			// txtTradingAccount
			// 
			this.txtTradingAccount.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtTradingAccount.Location = new System.Drawing.Point(148, 16);
			this.txtTradingAccount.MaxLength = 50;
			this.txtTradingAccount.Name = "txtTradingAccount";
			this.txtTradingAccount.Size = new System.Drawing.Size(148, 21);
			this.txtTradingAccount.TabIndex = 1;
			this.txtTradingAccount.Text = "";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label1.Location = new System.Drawing.Point(14, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(124, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Trading Account";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label2.Location = new System.Drawing.Point(14, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(124, 16);
			this.label2.TabIndex = 0;
			this.label2.Text = "Short Name";
			// 
			// btnClose
			// 
			this.btnClose.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnClose.Location = new System.Drawing.Point(160, 88);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(72, 24);
			this.btnClose.TabIndex = 6;
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
			this.stbCreateCompanyTradingAccounts.Size = new System.Drawing.Size(328, 22);
			this.stbCreateCompanyTradingAccounts.TabIndex = 8;
			this.stbCreateCompanyTradingAccounts.PanelClick += new System.Windows.Forms.StatusBarPanelClickEventHandler(this.stbCreateCompanyTradingAccounts_PanelClick);
			// 
			// statusBarPanel1
			// 
			this.statusBarPanel1.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.statusBarPanel1.Style = System.Windows.Forms.StatusBarPanelStyle.OwnerDraw;
			this.statusBarPanel1.Text = "statusBarPanel1";
			this.statusBarPanel1.Width = 312;
			// 
			// CreateCompanyTradingAccounts
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(328, 137);
			this.ControlBox = false;
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
//				int index = _tradingAccounts.IndexOf(_tradingAccountEdit);
//						
//				((Nirvana.Admin.BLL.TradingAccount)_tradingAccounts[index]).TradingAccountName = txtTradingAccount.Text.ToString();
//				((Nirvana.Admin.BLL.TradingAccount)_tradingAccounts[index]).TradingShortName = txtShortName.Text.ToString();
				
				_tradingAccountEdit.TradingAccountName = txtTradingAccount.Text.ToString();
				_tradingAccountEdit.TradingShortName = txtShortName.Text.ToString();
				SetStatusPanel(stbCreateCompanyTradingAccounts, "Stored!");
				
			}
			else
			{
				if(txtTradingAccount.Text.Trim() == "")
				{
					SetStatusPanel(stbCreateCompanyTradingAccounts, "Please enter Trading Account Name!");
					txtTradingAccount.Focus();
				}
				if(txtShortName.Text.Trim() == "")
				{
					SetStatusPanel(stbCreateCompanyTradingAccounts, "Please enter Short Name!");
					txtShortName.Focus();
				}
				else
				{
					Nirvana.Admin.BLL.TradingAccount tradingAccount = new Nirvana.Admin.BLL.TradingAccount();
						
					tradingAccount.TradingAccountName = txtTradingAccount.Text.ToString();
					tradingAccount.TradingShortName = txtShortName.Text.ToString();
					_tradingAccounts.Add(tradingAccount);		
					SetStatusPanel(stbCreateCompanyTradingAccounts, "Stored!");
				}
			}
		}

		public static void SetStatusPanel(StatusBar statusBar, string displayText)
		{
			StatusBarPanel pnlStatus = new StatusBarPanel();
			pnlStatus.BorderStyle = StatusBarPanelBorderStyle.Sunken;
			pnlStatus.Text = displayText;					
			pnlStatus.Alignment = HorizontalAlignment.Left;
			statusBar.Panels.Add(pnlStatus);	
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
				SetStatusPanel(stbCreateCompanyTradingAccounts, "");
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
	}
}
