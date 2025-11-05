using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Nirvana.Admin.Controls.Company
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnSave = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.txtTradingAccount = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtShortName = new System.Windows.Forms.TextBox();
			this.btnClose = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnSave
			// 
			this.btnSave.Location = new System.Drawing.Point(91, 102);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(72, 26);
			this.btnSave.TabIndex = 4;
			this.btnSave.Text = "Save";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.txtTradingAccount);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.txtShortName);
			this.groupBox1.Location = new System.Drawing.Point(29, 22);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(312, 70);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Trading Accounts";
			// 
			// txtTradingAccount
			// 
			this.txtTradingAccount.Location = new System.Drawing.Point(148, 16);
			this.txtTradingAccount.MaxLength = 50;
			this.txtTradingAccount.Name = "txtTradingAccount";
			this.txtTradingAccount.Size = new System.Drawing.Size(148, 20);
			this.txtTradingAccount.TabIndex = 1;
			this.txtTradingAccount.Text = "";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(14, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(124, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Trading Account";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(14, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(124, 16);
			this.label2.TabIndex = 0;
			this.label2.Text = "Short Name";
			// 
			// txtShortName
			// 
			this.txtShortName.Location = new System.Drawing.Point(148, 44);
			this.txtShortName.MaxLength = 50;
			this.txtShortName.Name = "txtShortName";
			this.txtShortName.Size = new System.Drawing.Size(148, 20);
			this.txtShortName.TabIndex = 1;
			this.txtShortName.Text = "";
			// 
			// btnClose
			// 
			this.btnClose.Location = new System.Drawing.Point(177, 102);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(72, 26);
			this.btnClose.TabIndex = 3;
			this.btnClose.Text = "Close";
			// 
			// CreateTradingAccounts
			// 
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnClose);
			this.Name = "CreateTradingAccounts";
			this.Size = new System.Drawing.Size(372, 138);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
