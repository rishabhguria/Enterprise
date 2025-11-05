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
	/// Summary description for CreateCompanyClearingFirmsPrimeBrokers.
	/// </summary>
	public class CreateCompanyClearingFirmsPrimeBrokers : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox txtClearingFirmsPrimeBrokers;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtShortName;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.StatusBarPanel statusBarPanel1;
		private System.Windows.Forms.StatusBar stbCreateCompanyClearingFirmsPrimeBrokers;
		private System.Windows.Forms.ErrorProvider errorProvider1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CreateCompanyClearingFirmsPrimeBrokers()
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
			this.btnClose = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.txtClearingFirmsPrimeBrokers = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtShortName = new System.Windows.Forms.TextBox();
			this.btnSave = new System.Windows.Forms.Button();
			this.stbCreateCompanyClearingFirmsPrimeBrokers = new System.Windows.Forms.StatusBar();
			this.statusBarPanel1 = new System.Windows.Forms.StatusBarPanel();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).BeginInit();
			this.SuspendLayout();
			// 
			// btnClose
			// 
			this.btnClose.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(51)), ((System.Byte)(51)));
			this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnClose.Location = new System.Drawing.Point(183, 90);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(72, 23);
			this.btnClose.TabIndex = 13;
			this.btnClose.Text = "Close";
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.txtClearingFirmsPrimeBrokers);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.txtShortName);
			this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
			this.groupBox1.Location = new System.Drawing.Point(6, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(366, 76);
			this.groupBox1.TabIndex = 11;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Clearing Firms/Prime Brokers";
			// 
			// txtClearingFirmsPrimeBrokers
			// 
			this.txtClearingFirmsPrimeBrokers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtClearingFirmsPrimeBrokers.Location = new System.Drawing.Point(190, 20);
			this.txtClearingFirmsPrimeBrokers.MaxLength = 50;
			this.txtClearingFirmsPrimeBrokers.Name = "txtClearingFirmsPrimeBrokers";
			this.txtClearingFirmsPrimeBrokers.Size = new System.Drawing.Size(148, 21);
			this.txtClearingFirmsPrimeBrokers.TabIndex = 1;
			this.txtClearingFirmsPrimeBrokers.Text = "";
			this.txtClearingFirmsPrimeBrokers.LostFocus += new System.EventHandler(this.txtClearingFirmsPrimeBrokers_LostFocus);
			this.txtClearingFirmsPrimeBrokers.GotFocus += new System.EventHandler(this.txtClearingFirmsPrimeBrokers_GotFocus);
			// 
			// label1
			// 
			this.label1.AllowDrop = true;
			this.label1.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label1.Location = new System.Drawing.Point(8, 21);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(140, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Name";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label2.Location = new System.Drawing.Point(8, 49);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(140, 16);
			this.label2.TabIndex = 0;
			this.label2.Text = "Short Name";
			// 
			// txtShortName
			// 
			this.txtShortName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtShortName.Location = new System.Drawing.Point(190, 48);
			this.txtShortName.MaxLength = 50;
			this.txtShortName.Name = "txtShortName";
			this.txtShortName.Size = new System.Drawing.Size(148, 21);
			this.txtShortName.TabIndex = 1;
			this.txtShortName.Text = "";
			this.txtShortName.LostFocus += new System.EventHandler(this.txtShortName_LostFocus);
			this.txtShortName.GotFocus += new System.EventHandler(this.txtShortName_GotFocus);
			// 
			// btnSave
			// 
			this.btnSave.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(204)), ((System.Byte)(102)));
			this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSave.Location = new System.Drawing.Point(103, 90);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(72, 23);
			this.btnSave.TabIndex = 12;
			this.btnSave.Text = "Save";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// stbCreateCompanyClearingFirmsPrimeBrokers
			// 
			this.stbCreateCompanyClearingFirmsPrimeBrokers.Location = new System.Drawing.Point(0, 123);
			this.stbCreateCompanyClearingFirmsPrimeBrokers.Name = "stbCreateCompanyClearingFirmsPrimeBrokers";
			this.stbCreateCompanyClearingFirmsPrimeBrokers.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
																														 this.statusBarPanel1});
			this.stbCreateCompanyClearingFirmsPrimeBrokers.ShowPanels = true;
			this.stbCreateCompanyClearingFirmsPrimeBrokers.Size = new System.Drawing.Size(376, 22);
			this.stbCreateCompanyClearingFirmsPrimeBrokers.TabIndex = 14;
			// 
			// statusBarPanel1
			// 
			this.statusBarPanel1.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.statusBarPanel1.Style = System.Windows.Forms.StatusBarPanelStyle.OwnerDraw;
			this.statusBarPanel1.Text = "statusBarPanel1";
			this.statusBarPanel1.Width = 360;
			// 
			// errorProvider1
			// 
			this.errorProvider1.ContainerControl = this;
			// 
			// label3
			// 
			this.label3.ForeColor = System.Drawing.Color.Red;
			this.label3.Location = new System.Drawing.Point(184, 30);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(12, 14);
			this.label3.TabIndex = 33;
			this.label3.Text = "*";
			// 
			// label4
			// 
			this.label4.ForeColor = System.Drawing.Color.Red;
			this.label4.Location = new System.Drawing.Point(184, 58);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(12, 14);
			this.label4.TabIndex = 34;
			this.label4.Text = "*";
			// 
			// CreateCompanyClearingFirmsPrimeBrokers
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.ClientSize = new System.Drawing.Size(376, 145);
			this.ControlBox = false;
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.stbCreateCompanyClearingFirmsPrimeBrokers);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnSave);
			this.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "CreateCompanyClearingFirmsPrimeBrokers";
			this.Text = "CreateCompanyClearingFirmsPrimeBrokers";
			this.Load += new System.EventHandler(this.CreateCompanyClearingFirmsPrimeBrokers_Load);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		Nirvana.Admin.BLL.ClearingFirmPrimeBroker _clearingFirmPrimeBrokerEdit = null;

		public Nirvana.Admin.BLL.ClearingFirmPrimeBroker ClearingFirmPrimeBrokerEdit
		{
			set{_clearingFirmPrimeBrokerEdit = value;}
		}

		public void BindForEdit()
		{
			if(_clearingFirmPrimeBrokerEdit != null)
			{
				txtClearingFirmsPrimeBrokers.Text = _clearingFirmPrimeBrokerEdit.ClearingFirmsPrimeBrokersName;
				txtShortName.Text = _clearingFirmPrimeBrokerEdit.ClearingFirmsPrimeBrokersShortName;
			}
		}
		
		private int _companyID = int.MinValue;
		public int CompanyID
		{
			set{_companyID = value;}
		}
		
		private Nirvana.Admin.BLL.ClearingFirmsPrimeBrokers _clearingFirmsPrimeBrokers = new ClearingFirmsPrimeBrokers();
		private void btnSave_Click(object sender, System.EventArgs e)
		{
			if(_clearingFirmPrimeBrokerEdit != null)
			{
				errorProvider1.SetError(txtClearingFirmsPrimeBrokers, "");
				errorProvider1.SetError(txtShortName, "");
				if(txtClearingFirmsPrimeBrokers.Text.Trim() == "")
				{
					errorProvider1.SetError(txtClearingFirmsPrimeBrokers, "Please enter ClearingFirmsPrimeBrokers Name!");
					txtClearingFirmsPrimeBrokers.Focus();
				}
				else if(txtShortName.Text.Trim() == "")
				{
					errorProvider1.SetError(txtShortName, "Please enter Short Name!");
					txtShortName.Focus();
				}
				else
				{
					_clearingFirmPrimeBrokerEdit.ClearingFirmsPrimeBrokersName = txtClearingFirmsPrimeBrokers.Text.ToString();
					_clearingFirmPrimeBrokerEdit.ClearingFirmsPrimeBrokersShortName = txtShortName.Text.ToString();

					Nirvana.Admin.Utility.Common.ResetStatusPanel(stbCreateCompanyClearingFirmsPrimeBrokers);
					Nirvana.Admin.Utility.Common.SetStatusPanel(stbCreateCompanyClearingFirmsPrimeBrokers, "Stored!");
				}
				
			}
			else
			{
				errorProvider1.SetError(txtClearingFirmsPrimeBrokers, "");
				errorProvider1.SetError(txtShortName, "");
				if(txtClearingFirmsPrimeBrokers.Text.Trim() == "")
				{
					errorProvider1.SetError(txtClearingFirmsPrimeBrokers, "Please enter ClearingFirmsPrimeBrokers Name!");
					txtClearingFirmsPrimeBrokers.Focus();
				}
				else if(txtShortName.Text.Trim() == "")
				{
					errorProvider1.SetError(txtShortName, "Please enter Short Name!");
					txtShortName.Focus();
				}
				else
				{
					Nirvana.Admin.BLL.ClearingFirmPrimeBroker clearingFirmPrimeBroker = new Nirvana.Admin.BLL.ClearingFirmPrimeBroker();
						
					clearingFirmPrimeBroker.ClearingFirmsPrimeBrokersName = txtClearingFirmsPrimeBrokers.Text.ToString();
					clearingFirmPrimeBroker.ClearingFirmsPrimeBrokersShortName = txtShortName.Text.ToString();
					_clearingFirmsPrimeBrokers.Add(clearingFirmPrimeBroker);		
					Nirvana.Admin.Utility.Common.ResetStatusPanel(stbCreateCompanyClearingFirmsPrimeBrokers);
					Nirvana.Admin.Utility.Common.SetStatusPanel(stbCreateCompanyClearingFirmsPrimeBrokers, "Stored!");
					this.Hide();
				}
			}
			
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Hide();
		}

		private void CreateCompanyClearingFirmsPrimeBrokers_Load(object sender, System.EventArgs e)
		{
			if(_clearingFirmPrimeBrokerEdit != null)
			{
				BindForEdit();
			}
			else
			{
				Refresh(sender, e);
				Nirvana.Admin.Utility.Common.ResetStatusPanel(stbCreateCompanyClearingFirmsPrimeBrokers);
			}
		}

		public void Refresh(object sender, System.EventArgs e)
		{
			txtClearingFirmsPrimeBrokers.Text = "";
			txtShortName.Text = "";
		}
		
		//private Nirvana.Admin.BLL.ClearingFirmsPrimeBrokers _clearingFirmsPrimeBrokers = new ClearingFirmsPrimeBrokers();
		public ClearingFirmsPrimeBrokers CurrentClearingFirmsPrimeBrokers
		{
			get 
			{
				return _clearingFirmsPrimeBrokers; 
			}
			set
			{
				if(value != null)
				{
					_clearingFirmsPrimeBrokers = value;
				}
			}			
		}
		#region Focus Colors
		private void txtClearingFirmsPrimeBrokers_GotFocus(object sender, System.EventArgs e)
		{
			txtClearingFirmsPrimeBrokers.BackColor = Color.LemonChiffon;
		}
		private void txtClearingFirmsPrimeBrokers_LostFocus(object sender, System.EventArgs e)
		{
			txtClearingFirmsPrimeBrokers.BackColor = Color.White;
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
