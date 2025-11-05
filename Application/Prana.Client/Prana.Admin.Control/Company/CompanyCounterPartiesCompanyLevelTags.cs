using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Nirvana.Admin.Controls
{
	/// <summary>
	/// Summary description for CompanyCounterPartiesCompanyLevelTags.
	/// </summary>
	public class CompanyCounterPartiesCompanyLevelTags : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cmbDeliverCompID;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cmbDeliverSubID;
		private System.Windows.Forms.TextBox txtCVFIXTagCompany;
		private System.Windows.Forms.ComboBox cmbCounterPartyVenue;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.ComboBox cmbClearingFirmPrimeBrokers;
		private System.Windows.Forms.ComboBox cmbCounterParty;
		private System.Windows.Forms.ComboBox cmbSenderCompID;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CompanyCounterPartiesCompanyLevelTags()
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
			this.cmbDeliverCompID = new System.Windows.Forms.ComboBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.cmbDeliverSubID = new System.Windows.Forms.ComboBox();
			this.txtCVFIXTagCompany = new System.Windows.Forms.TextBox();
			this.cmbCounterPartyVenue = new System.Windows.Forms.ComboBox();
			this.label9 = new System.Windows.Forms.Label();
			this.cmbClearingFirmPrimeBrokers = new System.Windows.Forms.ComboBox();
			this.cmbCounterParty = new System.Windows.Forms.ComboBox();
			this.cmbSenderCompID = new System.Windows.Forms.ComboBox();
			this.label10 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.cmbDeliverCompID);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.cmbDeliverSubID);
			this.groupBox1.Controls.Add(this.txtCVFIXTagCompany);
			this.groupBox1.Controls.Add(this.cmbCounterPartyVenue);
			this.groupBox1.Controls.Add(this.label9);
			this.groupBox1.Controls.Add(this.cmbClearingFirmPrimeBrokers);
			this.groupBox1.Controls.Add(this.cmbCounterParty);
			this.groupBox1.Controls.Add(this.cmbSenderCompID);
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Location = new System.Drawing.Point(4, 6);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(394, 184);
			this.groupBox1.TabIndex = 15;
			this.groupBox1.TabStop = false;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(14, 26);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 18);
			this.label1.TabIndex = 0;
			this.label1.Text = "CounterParty";
			// 
			// cmbDeliverCompID
			// 
			this.cmbDeliverCompID.Location = new System.Drawing.Point(212, 108);
			this.cmbDeliverCompID.Name = "cmbDeliverCompID";
			this.cmbDeliverCompID.Size = new System.Drawing.Size(174, 21);
			this.cmbDeliverCompID.TabIndex = 10;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(14, 133);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(100, 18);
			this.label8.TabIndex = 5;
			this.label8.Text = "Deliver To Sub ID";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(14, 46);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(138, 18);
			this.label2.TabIndex = 1;
			this.label2.Text = "CV-FIX Tag For Company";
			// 
			// cmbDeliverSubID
			// 
			this.cmbDeliverSubID.Location = new System.Drawing.Point(212, 130);
			this.cmbDeliverSubID.Name = "cmbDeliverSubID";
			this.cmbDeliverSubID.Size = new System.Drawing.Size(174, 21);
			this.cmbDeliverSubID.TabIndex = 11;
			// 
			// txtCVFIXTagCompany
			// 
			this.txtCVFIXTagCompany.Location = new System.Drawing.Point(212, 44);
			this.txtCVFIXTagCompany.Name = "txtCVFIXTagCompany";
			this.txtCVFIXTagCompany.Size = new System.Drawing.Size(174, 20);
			this.txtCVFIXTagCompany.TabIndex = 12;
			this.txtCVFIXTagCompany.Text = "";
			// 
			// cmbCounterPartyVenue
			// 
			this.cmbCounterPartyVenue.Location = new System.Drawing.Point(212, 64);
			this.cmbCounterPartyVenue.Name = "cmbCounterPartyVenue";
			this.cmbCounterPartyVenue.Size = new System.Drawing.Size(174, 21);
			this.cmbCounterPartyVenue.TabIndex = 8;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(14, 157);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(100, 18);
			this.label9.TabIndex = 6;
			this.label9.Text = "Sender Comp ID";
			// 
			// cmbClearingFirmPrimeBrokers
			// 
			this.cmbClearingFirmPrimeBrokers.Location = new System.Drawing.Point(212, 86);
			this.cmbClearingFirmPrimeBrokers.Name = "cmbClearingFirmPrimeBrokers";
			this.cmbClearingFirmPrimeBrokers.Size = new System.Drawing.Size(174, 21);
			this.cmbClearingFirmPrimeBrokers.TabIndex = 9;
			// 
			// cmbCounterParty
			// 
			this.cmbCounterParty.Location = new System.Drawing.Point(212, 22);
			this.cmbCounterParty.Name = "cmbCounterParty";
			this.cmbCounterParty.Size = new System.Drawing.Size(174, 21);
			this.cmbCounterParty.TabIndex = 7;
			// 
			// cmbSenderCompID
			// 
			this.cmbSenderCompID.Location = new System.Drawing.Point(212, 154);
			this.cmbSenderCompID.Name = "cmbSenderCompID";
			this.cmbSenderCompID.Size = new System.Drawing.Size(174, 21);
			this.cmbSenderCompID.TabIndex = 13;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(14, 111);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(100, 18);
			this.label10.TabIndex = 4;
			this.label10.Text = "Deliver to Comp ID";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(14, 67);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(110, 18);
			this.label3.TabIndex = 2;
			this.label3.Text = "CounterPartyVenue";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(14, 89);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(138, 18);
			this.label4.TabIndex = 3;
			this.label4.Text = "ClearingFirm/PrimeBroker";
			// 
			// CompanyCounterPartiesCompanyLevelTags
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(406, 195);
			this.Controls.Add(this.groupBox1);
			this.MaximizeBox = false;
			this.Name = "CompanyCounterPartiesCompanyLevelTags";
			this.Text = "CompanyCounterPartiesCompanyLevelTags";
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
