using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Nirvana.Admin.Controls.Company
{
	/// <summary>
	/// Summary description for CompanyCounterPartyFundLevelTags.
	/// </summary>
	public class CompanyCounterPartyFundLevelTags : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cmbOnBehalfCompID;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtCVFIXTagFunds;
		private System.Windows.Forms.ComboBox cmbCounterPartyVenue;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.ComboBox cmbFunds;
		private System.Windows.Forms.ComboBox cmbCounterParty;
		private System.Windows.Forms.ComboBox cmbGiveUp;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtCMTA;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CompanyCounterPartyFundLevelTags()
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.cmbOnBehalfCompID = new System.Windows.Forms.ComboBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtCVFIXTagFunds = new System.Windows.Forms.TextBox();
			this.cmbCounterPartyVenue = new System.Windows.Forms.ComboBox();
			this.label9 = new System.Windows.Forms.Label();
			this.cmbFunds = new System.Windows.Forms.ComboBox();
			this.cmbCounterParty = new System.Windows.Forms.ComboBox();
			this.cmbGiveUp = new System.Windows.Forms.ComboBox();
			this.label10 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.txtCMTA = new System.Windows.Forms.TextBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.txtCMTA);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.cmbOnBehalfCompID);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.txtCVFIXTagFunds);
			this.groupBox1.Controls.Add(this.cmbCounterPartyVenue);
			this.groupBox1.Controls.Add(this.label9);
			this.groupBox1.Controls.Add(this.cmbFunds);
			this.groupBox1.Controls.Add(this.cmbCounterParty);
			this.groupBox1.Controls.Add(this.cmbGiveUp);
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Location = new System.Drawing.Point(4, 2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(440, 182);
			this.groupBox1.TabIndex = 15;
			this.groupBox1.TabStop = false;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(14, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 18);
			this.label1.TabIndex = 0;
			this.label1.Text = "CounterParty";
			// 
			// cmbOnBehalfCompID
			// 
			this.cmbOnBehalfCompID.Location = new System.Drawing.Point(256, 108);
			this.cmbOnBehalfCompID.Name = "cmbOnBehalfCompID";
			this.cmbOnBehalfCompID.Size = new System.Drawing.Size(174, 21);
			this.cmbOnBehalfCompID.TabIndex = 10;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(14, 132);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(100, 18);
			this.label8.TabIndex = 5;
			this.label8.Text = "CMTA";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(14, 46);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(138, 18);
			this.label2.TabIndex = 1;
			this.label2.Text = "CV-FIX Tag For Funds";
			// 
			// txtCVFIXTagFunds
			// 
			this.txtCVFIXTagFunds.Location = new System.Drawing.Point(256, 44);
			this.txtCVFIXTagFunds.Name = "txtCVFIXTagFunds";
			this.txtCVFIXTagFunds.Size = new System.Drawing.Size(174, 20);
			this.txtCVFIXTagFunds.TabIndex = 12;
			this.txtCVFIXTagFunds.Text = "";
			// 
			// cmbCounterPartyVenue
			// 
			this.cmbCounterPartyVenue.Location = new System.Drawing.Point(256, 64);
			this.cmbCounterPartyVenue.Name = "cmbCounterPartyVenue";
			this.cmbCounterPartyVenue.Size = new System.Drawing.Size(174, 21);
			this.cmbCounterPartyVenue.TabIndex = 8;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(14, 152);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(100, 18);
			this.label9.TabIndex = 6;
			this.label9.Text = "Give Up";
			// 
			// cmbFunds
			// 
			this.cmbFunds.Location = new System.Drawing.Point(256, 86);
			this.cmbFunds.Name = "cmbFunds";
			this.cmbFunds.Size = new System.Drawing.Size(174, 21);
			this.cmbFunds.TabIndex = 9;
			// 
			// cmbCounterParty
			// 
			this.cmbCounterParty.Location = new System.Drawing.Point(256, 22);
			this.cmbCounterParty.Name = "cmbCounterParty";
			this.cmbCounterParty.Size = new System.Drawing.Size(174, 21);
			this.cmbCounterParty.TabIndex = 7;
			// 
			// cmbGiveUp
			// 
			this.cmbGiveUp.Location = new System.Drawing.Point(256, 150);
			this.cmbGiveUp.Name = "cmbGiveUp";
			this.cmbGiveUp.Size = new System.Drawing.Size(174, 21);
			this.cmbGiveUp.TabIndex = 13;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(14, 110);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(116, 18);
			this.label10.TabIndex = 4;
			this.label10.Text = "On Behalf of Comp ID";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(14, 66);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(110, 18);
			this.label3.TabIndex = 2;
			this.label3.Text = "CounterPartyVenue";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(14, 88);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(138, 18);
			this.label4.TabIndex = 3;
			this.label4.Text = "Funds";
			// 
			// txtCMTA
			// 
			this.txtCMTA.Location = new System.Drawing.Point(256, 130);
			this.txtCMTA.Name = "txtCMTA";
			this.txtCMTA.Size = new System.Drawing.Size(174, 20);
			this.txtCMTA.TabIndex = 14;
			this.txtCMTA.Text = "";
			// 
			// CompanyCounterPartyFundLevelTags
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(450, 193);
			this.Controls.Add(this.groupBox1);
			this.MinimizeBox = false;
			this.Name = "CompanyCounterPartyFundLevelTags";
			this.Text = "CompanyCounterPartyFundLevelTags";
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
