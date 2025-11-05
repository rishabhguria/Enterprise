using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Nirvana.Admin.Controls.Company
{
	/// <summary>
	/// Summary description for ClientFix.
	/// </summary>
	public class ClientFix : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label lblSenderCompID;
		private System.Windows.Forms.Label lblOnBehalfOfCompID;
		private System.Windows.Forms.Label lblTargetCompID;
		private System.Windows.Forms.Label lblIP;
		private System.Windows.Forms.Label lblPort;
		private System.Windows.Forms.TextBox txtSenderCompID;
		private System.Windows.Forms.TextBox txtOnBehalfOfCompID;
		private System.Windows.Forms.TextBox txtTargetCompID;
		private System.Windows.Forms.TextBox txtIP;
		private System.Windows.Forms.TextBox txtPort;
		private System.Windows.Forms.GroupBox Fix;
		private System.Windows.Forms.ComboBox cmbIdentifierID;
		private System.Windows.Forms.Label lblIdentifierID;
		private System.Windows.Forms.Label lblIdentifer;
		private System.Windows.Forms.TextBox txtIdentifer;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ClientFix()
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
			this.lblSenderCompID = new System.Windows.Forms.Label();
			this.lblOnBehalfOfCompID = new System.Windows.Forms.Label();
			this.lblTargetCompID = new System.Windows.Forms.Label();
			this.lblIP = new System.Windows.Forms.Label();
			this.lblPort = new System.Windows.Forms.Label();
			this.txtSenderCompID = new System.Windows.Forms.TextBox();
			this.txtOnBehalfOfCompID = new System.Windows.Forms.TextBox();
			this.txtTargetCompID = new System.Windows.Forms.TextBox();
			this.txtIP = new System.Windows.Forms.TextBox();
			this.txtPort = new System.Windows.Forms.TextBox();
			this.Fix = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.cmbIdentifierID = new System.Windows.Forms.ComboBox();
			this.lblIdentifierID = new System.Windows.Forms.Label();
			this.lblIdentifer = new System.Windows.Forms.Label();
			this.txtIdentifer = new System.Windows.Forms.TextBox();
			this.Fix.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblSenderCompID
			// 
			this.lblSenderCompID.Location = new System.Drawing.Point(20, 25);
			this.lblSenderCompID.Name = "lblSenderCompID";
			this.lblSenderCompID.Size = new System.Drawing.Size(106, 23);
			this.lblSenderCompID.TabIndex = 0;
			this.lblSenderCompID.Text = "SenderCompID";
			// 
			// lblOnBehalfOfCompID
			// 
			this.lblOnBehalfOfCompID.Location = new System.Drawing.Point(20, 53);
			this.lblOnBehalfOfCompID.Name = "lblOnBehalfOfCompID";
			this.lblOnBehalfOfCompID.Size = new System.Drawing.Size(108, 23);
			this.lblOnBehalfOfCompID.TabIndex = 1;
			this.lblOnBehalfOfCompID.Text = "OnBehalfOfCompID";
			// 
			// lblTargetCompID
			// 
			this.lblTargetCompID.Location = new System.Drawing.Point(20, 80);
			this.lblTargetCompID.Name = "lblTargetCompID";
			this.lblTargetCompID.Size = new System.Drawing.Size(108, 23);
			this.lblTargetCompID.TabIndex = 2;
			this.lblTargetCompID.Text = "TargetCompID";
			// 
			// lblIP
			// 
			this.lblIP.Location = new System.Drawing.Point(20, 110);
			this.lblIP.Name = "lblIP";
			this.lblIP.Size = new System.Drawing.Size(108, 23);
			this.lblIP.TabIndex = 3;
			this.lblIP.Text = "IP";
			// 
			// lblPort
			// 
			this.lblPort.Location = new System.Drawing.Point(20, 141);
			this.lblPort.Name = "lblPort";
			this.lblPort.Size = new System.Drawing.Size(108, 23);
			this.lblPort.TabIndex = 4;
			this.lblPort.Text = "Port";
			// 
			// txtSenderCompID
			// 
			this.txtSenderCompID.Location = new System.Drawing.Point(148, 26);
			this.txtSenderCompID.Name = "txtSenderCompID";
			this.txtSenderCompID.TabIndex = 5;
			this.txtSenderCompID.Text = "";
			// 
			// txtOnBehalfOfCompID
			// 
			this.txtOnBehalfOfCompID.Location = new System.Drawing.Point(148, 54);
			this.txtOnBehalfOfCompID.Name = "txtOnBehalfOfCompID";
			this.txtOnBehalfOfCompID.TabIndex = 6;
			this.txtOnBehalfOfCompID.Text = "";
			// 
			// txtTargetCompID
			// 
			this.txtTargetCompID.Location = new System.Drawing.Point(148, 86);
			this.txtTargetCompID.Name = "txtTargetCompID";
			this.txtTargetCompID.TabIndex = 7;
			this.txtTargetCompID.Text = "";
			// 
			// txtIP
			// 
			this.txtIP.Location = new System.Drawing.Point(148, 112);
			this.txtIP.Name = "txtIP";
			this.txtIP.TabIndex = 8;
			this.txtIP.Text = "";
			// 
			// txtPort
			// 
			this.txtPort.Location = new System.Drawing.Point(148, 144);
			this.txtPort.Name = "txtPort";
			this.txtPort.TabIndex = 9;
			this.txtPort.Text = "";
			// 
			// Fix
			// 
			this.Fix.Controls.Add(this.lblSenderCompID);
			this.Fix.Controls.Add(this.lblOnBehalfOfCompID);
			this.Fix.Controls.Add(this.lblTargetCompID);
			this.Fix.Controls.Add(this.lblIP);
			this.Fix.Controls.Add(this.lblPort);
			this.Fix.Controls.Add(this.txtSenderCompID);
			this.Fix.Controls.Add(this.txtOnBehalfOfCompID);
			this.Fix.Controls.Add(this.txtPort);
			this.Fix.Controls.Add(this.txtTargetCompID);
			this.Fix.Controls.Add(this.txtIP);
			this.Fix.Location = new System.Drawing.Point(6, 8);
			this.Fix.Name = "Fix";
			this.Fix.Size = new System.Drawing.Size(274, 192);
			this.Fix.TabIndex = 10;
			this.Fix.TabStop = false;
			this.Fix.Text = "Fix";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.txtIdentifer);
			this.groupBox2.Controls.Add(this.lblIdentifer);
			this.groupBox2.Controls.Add(this.lblIdentifierID);
			this.groupBox2.Controls.Add(this.cmbIdentifierID);
			this.groupBox2.Location = new System.Drawing.Point(10, 216);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(268, 92);
			this.groupBox2.TabIndex = 11;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Identifier";
			// 
			// cmbIdentifierID
			// 
			this.cmbIdentifierID.Location = new System.Drawing.Point(130, 26);
			this.cmbIdentifierID.Name = "cmbIdentifierID";
			this.cmbIdentifierID.Size = new System.Drawing.Size(121, 21);
			this.cmbIdentifierID.TabIndex = 0;
			// 
			// lblIdentifierID
			// 
			this.lblIdentifierID.Location = new System.Drawing.Point(22, 24);
			this.lblIdentifierID.Name = "lblIdentifierID";
			this.lblIdentifierID.TabIndex = 1;
			this.lblIdentifierID.Text = "IdentifierID";
			// 
			// lblIdentifer
			// 
			this.lblIdentifer.Location = new System.Drawing.Point(22, 58);
			this.lblIdentifer.Name = "lblIdentifer";
			this.lblIdentifer.TabIndex = 2;
			this.lblIdentifer.Text = "Identifer";
			// 
			// txtIdentifer
			// 
			this.txtIdentifer.Location = new System.Drawing.Point(130, 62);
			this.txtIdentifer.Name = "txtIdentifer";
			this.txtIdentifer.TabIndex = 3;
			this.txtIdentifer.Text = "";
			// 
			// ClientFix
			// 
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.Fix);
			this.Name = "ClientFix";
			this.Size = new System.Drawing.Size(296, 322);
			this.Fix.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
