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
	/// Summary description for CreateClientTrader.
	/// </summary>
	public class CreateClientTrader : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.TextBox txtFirstName;
		private System.Windows.Forms.TextBox txtLastName;
		private System.Windows.Forms.TextBox txtShortName;
		private System.Windows.Forms.TextBox txtTitle;
		private System.Windows.Forms.TextBox txtEMail;
		private System.Windows.Forms.TextBox txtTelephoneWork;
		private System.Windows.Forms.TextBox txtTelephoneHome;
		private System.Windows.Forms.TextBox txtTelephoneCell;
		private System.Windows.Forms.TextBox txtPager;
		private System.Windows.Forms.TextBox txtFax;

		
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.StatusBar stbClientTrader;
		private System.Windows.Forms.StatusBarPanel statusBarPanel1;

		Nirvana.Admin.BLL.Trader _traderEdit = null;
		public Nirvana.Admin.BLL.Trader TraderEdit
		{
			set{_traderEdit = value;}
		}
		
		public CreateClientTrader()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			BindForEdit();
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.txtFirstName = new System.Windows.Forms.TextBox();
			this.txtLastName = new System.Windows.Forms.TextBox();
			this.txtShortName = new System.Windows.Forms.TextBox();
			this.txtTitle = new System.Windows.Forms.TextBox();
			this.txtEMail = new System.Windows.Forms.TextBox();
			this.txtTelephoneWork = new System.Windows.Forms.TextBox();
			this.txtTelephoneHome = new System.Windows.Forms.TextBox();
			this.txtTelephoneCell = new System.Windows.Forms.TextBox();
			this.txtPager = new System.Windows.Forms.TextBox();
			this.txtFax = new System.Windows.Forms.TextBox();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.stbClientTrader = new System.Windows.Forms.StatusBar();
			this.statusBarPanel1 = new System.Windows.Forms.StatusBarPanel();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Controls.Add(this.label9);
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.txtFirstName);
			this.groupBox1.Controls.Add(this.txtLastName);
			this.groupBox1.Controls.Add(this.txtShortName);
			this.groupBox1.Controls.Add(this.txtTitle);
			this.groupBox1.Controls.Add(this.txtEMail);
			this.groupBox1.Controls.Add(this.txtTelephoneWork);
			this.groupBox1.Controls.Add(this.txtTelephoneHome);
			this.groupBox1.Controls.Add(this.txtTelephoneCell);
			this.groupBox1.Controls.Add(this.txtPager);
			this.groupBox1.Controls.Add(this.txtFax);
			this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
			this.groupBox1.Location = new System.Drawing.Point(2, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(240, 242);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Client Trader";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(6, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(126, 18);
			this.label1.TabIndex = 0;
			this.label1.Text = "First Name";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(6, 42);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(126, 18);
			this.label2.TabIndex = 0;
			this.label2.Text = "Last Name";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(6, 64);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(126, 18);
			this.label3.TabIndex = 0;
			this.label3.Text = "Short Name";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(6, 86);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(126, 18);
			this.label4.TabIndex = 0;
			this.label4.Text = "Title";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(6, 108);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(126, 18);
			this.label5.TabIndex = 0;
			this.label5.Text = "EMail";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(6, 130);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(126, 18);
			this.label6.TabIndex = 0;
			this.label6.Text = "Telephone (Work)";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(6, 152);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(126, 18);
			this.label7.TabIndex = 0;
			this.label7.Text = "Telephone (Home)";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(6, 174);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(126, 18);
			this.label8.TabIndex = 0;
			this.label8.Text = "Telephone (Cell)";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(6, 196);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(126, 18);
			this.label9.TabIndex = 0;
			this.label9.Text = "Pager";
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(6, 218);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(126, 18);
			this.label10.TabIndex = 0;
			this.label10.Text = "Fax";
			// 
			// txtFirstName
			// 
			this.txtFirstName.Location = new System.Drawing.Point(134, 17);
			this.txtFirstName.MaxLength = 50;
			this.txtFirstName.Name = "txtFirstName";
			this.txtFirstName.TabIndex = 1;
			this.txtFirstName.Text = "";
			// 
			// txtLastName
			// 
			this.txtLastName.Location = new System.Drawing.Point(134, 39);
			this.txtLastName.MaxLength = 50;
			this.txtLastName.Name = "txtLastName";
			this.txtLastName.TabIndex = 1;
			this.txtLastName.Text = "";
			// 
			// txtShortName
			// 
			this.txtShortName.Location = new System.Drawing.Point(134, 61);
			this.txtShortName.MaxLength = 50;
			this.txtShortName.Name = "txtShortName";
			this.txtShortName.TabIndex = 1;
			this.txtShortName.Text = "";
			// 
			// txtTitle
			// 
			this.txtTitle.Location = new System.Drawing.Point(134, 83);
			this.txtTitle.MaxLength = 50;
			this.txtTitle.Name = "txtTitle";
			this.txtTitle.TabIndex = 1;
			this.txtTitle.Text = "";
			// 
			// txtEMail
			// 
			this.txtEMail.Location = new System.Drawing.Point(134, 105);
			this.txtEMail.MaxLength = 50;
			this.txtEMail.Name = "txtEMail";
			this.txtEMail.TabIndex = 1;
			this.txtEMail.Text = "";
			// 
			// txtTelephoneWork
			// 
			this.txtTelephoneWork.Location = new System.Drawing.Point(134, 127);
			this.txtTelephoneWork.MaxLength = 50;
			this.txtTelephoneWork.Name = "txtTelephoneWork";
			this.txtTelephoneWork.TabIndex = 1;
			this.txtTelephoneWork.Text = "";
			// 
			// txtTelephoneHome
			// 
			this.txtTelephoneHome.Location = new System.Drawing.Point(134, 149);
			this.txtTelephoneHome.MaxLength = 50;
			this.txtTelephoneHome.Name = "txtTelephoneHome";
			this.txtTelephoneHome.TabIndex = 1;
			this.txtTelephoneHome.Text = "";
			// 
			// txtTelephoneCell
			// 
			this.txtTelephoneCell.Location = new System.Drawing.Point(134, 171);
			this.txtTelephoneCell.MaxLength = 50;
			this.txtTelephoneCell.Name = "txtTelephoneCell";
			this.txtTelephoneCell.TabIndex = 1;
			this.txtTelephoneCell.Text = "";
			// 
			// txtPager
			// 
			this.txtPager.Location = new System.Drawing.Point(134, 193);
			this.txtPager.MaxLength = 50;
			this.txtPager.Name = "txtPager";
			this.txtPager.TabIndex = 1;
			this.txtPager.Text = "";
			// 
			// txtFax
			// 
			this.txtFax.Location = new System.Drawing.Point(134, 215);
			this.txtFax.MaxLength = 50;
			this.txtFax.Name = "txtFax";
			this.txtFax.TabIndex = 1;
			this.txtFax.Text = "";
			// 
			// btnSave
			// 
			this.btnSave.Location = new System.Drawing.Point(47, 260);
			this.btnSave.Name = "btnSave";
			this.btnSave.TabIndex = 1;
			this.btnSave.Text = "Save";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnClose
			// 
			this.btnClose.Location = new System.Drawing.Point(125, 260);
			this.btnClose.Name = "btnClose";
			this.btnClose.TabIndex = 1;
			this.btnClose.Text = "Close";
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// stbClientTrader
			// 
			this.stbClientTrader.Location = new System.Drawing.Point(0, 283);
			this.stbClientTrader.Name = "stbClientTrader";
			this.stbClientTrader.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
																							   this.statusBarPanel1});
			this.stbClientTrader.Size = new System.Drawing.Size(246, 22);
			this.stbClientTrader.TabIndex = 2;
			// 
			// statusBarPanel1
			// 
			this.statusBarPanel1.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.statusBarPanel1.Style = System.Windows.Forms.StatusBarPanelStyle.OwnerDraw;
			this.statusBarPanel1.Text = "statusBarPanel1";
			// 
			// CreateClientTrader
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(246, 305);
			this.ControlBox = false;
			this.Controls.Add(this.stbClientTrader);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnClose);
			this.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.Name = "CreateClientTrader";
			this.Text = "Create Client Trader";
			this.Load += new System.EventHandler(this.CreateClientTrader_Load);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private int _companyID = int.MinValue;
		public int CompanyID
		{
			set{_companyID = value;}
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Hide();
		}

		public void BindForEdit()
		{
			if(_traderEdit != null)
			{
				txtEMail.Text = _traderEdit.EMail;
				txtFax.Text = _traderEdit.Fax;
				txtFirstName.Text = _traderEdit.FirstName;
				txtLastName.Text = _traderEdit.LastName;
				txtPager.Text = _traderEdit.Pager;
				txtShortName.Text = _traderEdit.ShortName;
				txtTelephoneCell.Text = _traderEdit.TelephoneCell;
				txtTelephoneHome.Text = _traderEdit.TelephoneHome;
				txtTelephoneWork.Text = _traderEdit.TelephoneWork;
				txtTitle.Text = _traderEdit.Title;
			}
			
		}
		
		private void btnSave_Click(object sender, System.EventArgs e)
		{
			if(_traderEdit != null)
			{
				_traderEdit.EMail = txtEMail.Text.ToString();
				_traderEdit.Fax = txtFax.Text.ToString();
				_traderEdit.FirstName = txtFirstName.Text.ToString();
				_traderEdit.LastName = txtLastName.Text.ToString();
				_traderEdit.Pager = txtPager.Text.ToString();
				_traderEdit.ShortName = txtShortName.Text.ToString();
				_traderEdit.TelephoneCell = txtTelephoneCell.Text.ToString();
				_traderEdit.TelephoneHome = txtTelephoneHome.Text.ToString();
				_traderEdit.TelephoneWork = txtTelephoneWork.Text.ToString();
				_traderEdit.Title = txtTitle.Text.ToString();
				SetStatusPanel(stbClientTrader, "Stored!");				
			}
			else
			{
			
				if(txtFirstName.Text.Trim() == "")
				{
					SetStatusPanel(stbClientTrader, "Please enter Trader First Name!");
					txtFirstName.Focus();
				}
				if(txtShortName.Text.Trim() == "")
				{
					SetStatusPanel(stbClientTrader, "Please enter Short Name!");
					txtShortName.Focus();
				}
				else
				{
					Trader trader = new Trader();
			
					trader.EMail = txtEMail.Text.ToString();
					trader.Fax = txtFax.Text.ToString();
					trader.FirstName = txtFirstName.Text.ToString();
					trader.LastName = txtLastName.Text.ToString();
					trader.Pager = txtPager.Text.ToString();
					trader.ShortName = txtShortName.Text.ToString();
					trader.TelephoneCell = txtTelephoneCell.Text.ToString();
					trader.TelephoneHome = txtTelephoneHome.Text.ToString();
					trader.TelephoneWork = txtTelephoneWork.Text.ToString();
					trader.Title = txtTitle.Text.ToString();
			
					_traders.Add(trader);		
					SetStatusPanel(stbClientTrader, "Stored!");
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

		public void Refresh(object sender, System.EventArgs e)
		{
			txtEMail.Text = "";
			txtFax.Text = "";
			txtFirstName.Text = "";
			txtLastName.Text = "";
			txtPager.Text = "";
			txtShortName.Text = "";
			txtTelephoneCell.Text = "";
			txtTelephoneHome.Text = "";
			txtTelephoneWork.Text = "";
			txtTitle.Text = "";
		}
		
		private void CreateClientTrader_Load(object sender, System.EventArgs e)
		{
			if(_traderEdit != null)
			{
				BindForEdit();
			}
			else
			{
				Refresh(sender, e);
				SetStatusPanel(stbClientTrader, "");
			}
		}
	
		private Nirvana.Admin.BLL.Traders _traders = new Traders();
		public Traders CurrentTraders
		{
			get 
			{
				return _traders; 
			}
			set
			{
				if(value != null)
				{
					_traders = value;
				}
			}			
		}
	}
}
