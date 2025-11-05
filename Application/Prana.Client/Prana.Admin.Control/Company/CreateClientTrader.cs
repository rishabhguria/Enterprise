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
		private System.Windows.Forms.ErrorProvider errorProvider1;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label22;

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
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
			this.label17 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.label22 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label17);
			this.groupBox1.Controls.Add(this.label12);
			this.groupBox1.Controls.Add(this.label11);
			this.groupBox1.Controls.Add(this.label13);
			this.groupBox1.Controls.Add(this.label22);
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
			this.groupBox1.Location = new System.Drawing.Point(6, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(272, 268);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Client Trader";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label1.Location = new System.Drawing.Point(8, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(112, 18);
			this.label1.TabIndex = 0;
			this.label1.Text = "First Name";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label2.Location = new System.Drawing.Point(8, 42);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(112, 18);
			this.label2.TabIndex = 0;
			this.label2.Text = "Last Name";
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label3.Location = new System.Drawing.Point(8, 64);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(112, 18);
			this.label3.TabIndex = 0;
			this.label3.Text = "Short Name";
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label4.Location = new System.Drawing.Point(8, 86);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(112, 18);
			this.label4.TabIndex = 0;
			this.label4.Text = "Title";
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label5.Location = new System.Drawing.Point(8, 108);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(112, 18);
			this.label5.TabIndex = 0;
			this.label5.Text = "EMail";
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label6.Location = new System.Drawing.Point(8, 130);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(112, 18);
			this.label6.TabIndex = 0;
			this.label6.Text = "Telephone (Work)";
			// 
			// label7
			// 
			this.label7.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label7.Location = new System.Drawing.Point(8, 152);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(112, 18);
			this.label7.TabIndex = 0;
			this.label7.Text = "Telephone (Home)";
			// 
			// label8
			// 
			this.label8.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label8.Location = new System.Drawing.Point(8, 174);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(112, 18);
			this.label8.TabIndex = 0;
			this.label8.Text = "Telephone (Cell)";
			// 
			// label9
			// 
			this.label9.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label9.Location = new System.Drawing.Point(8, 196);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(112, 18);
			this.label9.TabIndex = 0;
			this.label9.Text = "Pager";
			// 
			// label10
			// 
			this.label10.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label10.Location = new System.Drawing.Point(8, 218);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(112, 18);
			this.label10.TabIndex = 0;
			this.label10.Text = "Fax";
			// 
			// txtFirstName
			// 
			this.txtFirstName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtFirstName.Location = new System.Drawing.Point(144, 17);
			this.txtFirstName.MaxLength = 50;
			this.txtFirstName.Name = "txtFirstName";
			this.txtFirstName.TabIndex = 1;
			this.txtFirstName.Text = "";
			this.txtFirstName.LostFocus += new System.EventHandler(this.txtFirstName_LostFocus);
			this.txtFirstName.GotFocus += new System.EventHandler(this.txtFirstName_GotFocus);
			// 
			// txtLastName
			// 
			this.txtLastName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtLastName.Location = new System.Drawing.Point(144, 39);
			this.txtLastName.MaxLength = 50;
			this.txtLastName.Name = "txtLastName";
			this.txtLastName.TabIndex = 1;
			this.txtLastName.Text = "";
			this.txtLastName.LostFocus += new System.EventHandler(this.txtLastName_LostFocus);
			this.txtLastName.GotFocus += new System.EventHandler(this.txtLastName_GotFocus);
			// 
			// txtShortName
			// 
			this.txtShortName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtShortName.Location = new System.Drawing.Point(144, 61);
			this.txtShortName.MaxLength = 50;
			this.txtShortName.Name = "txtShortName";
			this.txtShortName.TabIndex = 1;
			this.txtShortName.Text = "";
			this.txtShortName.LostFocus += new System.EventHandler(this.txtShortName_LostFocus);
			this.txtShortName.GotFocus += new System.EventHandler(this.txtShortName_GotFocus);
			// 
			// txtTitle
			// 
			this.txtTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtTitle.Location = new System.Drawing.Point(144, 83);
			this.txtTitle.MaxLength = 50;
			this.txtTitle.Name = "txtTitle";
			this.txtTitle.TabIndex = 1;
			this.txtTitle.Text = "";
			this.txtTitle.LostFocus += new System.EventHandler(this.txtTitle_LostFocus);
			this.txtTitle.GotFocus += new System.EventHandler(this.txtTitle_GotFocus);
			// 
			// txtEMail
			// 
			this.txtEMail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtEMail.Location = new System.Drawing.Point(144, 105);
			this.txtEMail.MaxLength = 50;
			this.txtEMail.Name = "txtEMail";
			this.txtEMail.TabIndex = 1;
			this.txtEMail.Text = "";
			this.txtEMail.LostFocus += new System.EventHandler(this.txtEMail_LostFocus);
			this.txtEMail.GotFocus += new System.EventHandler(this.txtEMail_GotFocus);
			// 
			// txtTelephoneWork
			// 
			this.txtTelephoneWork.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtTelephoneWork.Location = new System.Drawing.Point(144, 127);
			this.txtTelephoneWork.MaxLength = 50;
			this.txtTelephoneWork.Name = "txtTelephoneWork";
			this.txtTelephoneWork.TabIndex = 1;
			this.txtTelephoneWork.Text = "";
			this.txtTelephoneWork.LostFocus += new System.EventHandler(this.txtTelephoneWork_LostFocus);
			this.txtTelephoneWork.GotFocus += new System.EventHandler(this.txtTelephoneWork_GotFocus);
			// 
			// txtTelephoneHome
			// 
			this.txtTelephoneHome.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtTelephoneHome.Location = new System.Drawing.Point(144, 149);
			this.txtTelephoneHome.MaxLength = 50;
			this.txtTelephoneHome.Name = "txtTelephoneHome";
			this.txtTelephoneHome.TabIndex = 1;
			this.txtTelephoneHome.Text = "";
			this.txtTelephoneHome.LostFocus += new System.EventHandler(this.txtTelephoneHome_LostFocus);
			this.txtTelephoneHome.GotFocus += new System.EventHandler(this.txtTelephoneHome_GotFocus);
			// 
			// txtTelephoneCell
			// 
			this.txtTelephoneCell.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtTelephoneCell.Location = new System.Drawing.Point(144, 171);
			this.txtTelephoneCell.MaxLength = 50;
			this.txtTelephoneCell.Name = "txtTelephoneCell";
			this.txtTelephoneCell.TabIndex = 1;
			this.txtTelephoneCell.Text = "";
			this.txtTelephoneCell.LostFocus += new System.EventHandler(this.txtTelephoneCell_LostFocus);
			this.txtTelephoneCell.GotFocus += new System.EventHandler(this.txtTelephoneCell_GotFocus);
			// 
			// txtPager
			// 
			this.txtPager.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtPager.Location = new System.Drawing.Point(144, 193);
			this.txtPager.MaxLength = 50;
			this.txtPager.Name = "txtPager";
			this.txtPager.TabIndex = 1;
			this.txtPager.Text = "";
			this.txtPager.LostFocus += new System.EventHandler(this.txtPager_LostFocus);
			this.txtPager.GotFocus += new System.EventHandler(this.txtPager_GotFocus);
			// 
			// txtFax
			// 
			this.txtFax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtFax.Location = new System.Drawing.Point(144, 215);
			this.txtFax.MaxLength = 50;
			this.txtFax.Name = "txtFax";
			this.txtFax.TabIndex = 1;
			this.txtFax.Text = "";
			this.txtFax.LostFocus += new System.EventHandler(this.txtFax_LostFocus);
			this.txtFax.GotFocus += new System.EventHandler(this.txtFax_GotFocus);
			// 
			// btnSave
			// 
			this.btnSave.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(204)), ((System.Byte)(102)));
			this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSave.Location = new System.Drawing.Point(55, 280);
			this.btnSave.Name = "btnSave";
			this.btnSave.TabIndex = 1;
			this.btnSave.Text = "Save";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnClose
			// 
			this.btnClose.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(51)), ((System.Byte)(51)));
			this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnClose.Location = new System.Drawing.Point(133, 280);
			this.btnClose.Name = "btnClose";
			this.btnClose.TabIndex = 1;
			this.btnClose.Text = "Close";
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// stbClientTrader
			// 
			this.stbClientTrader.Location = new System.Drawing.Point(0, 307);
			this.stbClientTrader.Name = "stbClientTrader";
			this.stbClientTrader.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
																							   this.statusBarPanel1});
			this.stbClientTrader.Size = new System.Drawing.Size(282, 22);
			this.stbClientTrader.TabIndex = 2;
			// 
			// statusBarPanel1
			// 
			this.statusBarPanel1.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.statusBarPanel1.Style = System.Windows.Forms.StatusBarPanelStyle.OwnerDraw;
			this.statusBarPanel1.Text = "statusBarPanel1";
			// 
			// errorProvider1
			// 
			this.errorProvider1.ContainerControl = this;
			// 
			// label17
			// 
			this.label17.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label17.ForeColor = System.Drawing.Color.Red;
			this.label17.Location = new System.Drawing.Point(6, 252);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(98, 16);
			this.label17.TabIndex = 183;
			this.label17.Text = "* Required Field";
			// 
			// label12
			// 
			this.label12.ForeColor = System.Drawing.Color.Red;
			this.label12.Location = new System.Drawing.Point(130, 104);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(12, 14);
			this.label12.TabIndex = 182;
			this.label12.Text = "*";
			// 
			// label11
			// 
			this.label11.ForeColor = System.Drawing.Color.Red;
			this.label11.Location = new System.Drawing.Point(130, 62);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(12, 14);
			this.label11.TabIndex = 181;
			this.label11.Text = "*";
			// 
			// label13
			// 
			this.label13.ForeColor = System.Drawing.Color.Red;
			this.label13.Location = new System.Drawing.Point(130, 128);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(12, 14);
			this.label13.TabIndex = 179;
			this.label13.Text = "*";
			// 
			// label22
			// 
			this.label22.ForeColor = System.Drawing.Color.Red;
			this.label22.Location = new System.Drawing.Point(130, 18);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(12, 14);
			this.label22.TabIndex = 180;
			this.label22.Text = "*";
			// 
			// CreateClientTrader
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.ClientSize = new System.Drawing.Size(282, 329);
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
				errorProvider1.SetError(txtFirstName, "");
				errorProvider1.SetError(txtShortName, "");
				if(txtFirstName.Text.Trim() == "")
				{
					errorProvider1.SetError(txtFirstName, "Please enter Trader First Name!");
					txtFirstName.Focus();
				}
				else if(txtShortName.Text.Trim() == "")
				{
					errorProvider1.SetError(txtShortName, "Please enter Short Name!");
					txtShortName.Focus();
				}
				else
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
					Nirvana.Admin.Utility.Common.ResetStatusPanel(stbClientTrader);
					Nirvana.Admin.Utility.Common.SetStatusPanel(stbClientTrader, "Stored!");
				}
			}
			else
			{
			
				errorProvider1.SetError(txtFirstName, "");
				errorProvider1.SetError(txtShortName, "");
				if(txtFirstName.Text.Trim() == "")
				{
					errorProvider1.SetError(txtFirstName, "Please enter Trader First Name!");
					txtFirstName.Focus();
				}
				else if(txtShortName.Text.Trim() == "")
				{
					errorProvider1.SetError(txtShortName, "Please enter Short Name!");
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
					Nirvana.Admin.Utility.Common.ResetStatusPanel(stbClientTrader);
					Nirvana.Admin.Utility.Common.SetStatusPanel(stbClientTrader, "Stored!");
					this.Hide();
				}
			}
			
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
				Nirvana.Admin.Utility.Common.ResetStatusPanel(stbClientTrader);
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
		#region Focus Colors
		private void txtFirstName_GotFocus(object sender, System.EventArgs e)
		{
			txtFirstName.BackColor = Color.LemonChiffon;
		}
		private void txtFirstName_LostFocus(object sender, System.EventArgs e)
		{
			txtFirstName.BackColor = Color.White;
		}
		private void txtLastName_GotFocus(object sender, System.EventArgs e)
		{
			txtLastName.BackColor = Color.LemonChiffon;
		}
		private void txtLastName_LostFocus(object sender, System.EventArgs e)
		{
			txtLastName.BackColor = Color.White;
		}
		private void txtShortName_GotFocus(object sender, System.EventArgs e)
		{
			txtShortName.BackColor = Color.LemonChiffon;
		}
		private void txtShortName_LostFocus(object sender, System.EventArgs e)
		{
			txtShortName.BackColor = Color.White;
		}
		private void txtTitle_GotFocus(object sender, System.EventArgs e)
		{
			txtTitle.BackColor = Color.LemonChiffon;
		}
		private void txtTitle_LostFocus(object sender, System.EventArgs e)
		{
			txtTitle.BackColor = Color.White;
		}
		private void txtEMail_GotFocus(object sender, System.EventArgs e)
		{
			txtEMail.BackColor = Color.LemonChiffon;
		}
		private void txtEMail_LostFocus(object sender, System.EventArgs e)
		{
			txtEMail.BackColor = Color.White;
        }
		private void txtTelephoneWork_GotFocus(object sender, System.EventArgs e)
		{
			txtTelephoneWork.BackColor = Color.LemonChiffon;
		}
		private void txtTelephoneWork_LostFocus(object sender, System.EventArgs e)
		{
			txtTelephoneWork.BackColor = Color.White;
		}
		private void txtTelephoneHome_GotFocus(object sender, System.EventArgs e)
		{
			txtTelephoneHome.BackColor = Color.LemonChiffon;
		}
		private void txtTelephoneHome_LostFocus(object sender, System.EventArgs e)
		{
			txtTelephoneHome.BackColor = Color.White;
		}
		private void txtTelephoneCell_GotFocus(object sender, System.EventArgs e)
		{
			txtTelephoneCell.BackColor = Color.LemonChiffon;
		}
		private void txtTelephoneCell_LostFocus(object sender, System.EventArgs e)
		{
			txtTelephoneCell.BackColor = Color.White;
		}
		private void txtPager_GotFocus(object sender, System.EventArgs e)
		{
			txtPager.BackColor = Color.LemonChiffon;
		}
		private void txtPager_LostFocus(object sender, System.EventArgs e)
		{
			txtPager.BackColor = Color.White;
		}
		private void txtFax_GotFocus(object sender, System.EventArgs e)
		{
			txtFax.BackColor = Color.LemonChiffon;
		}
		private void txtFax_LostFocus(object sender, System.EventArgs e)
		{
			txtFax.BackColor = Color.White;
		}
		#endregion
	}
}
