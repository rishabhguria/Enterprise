#region Using

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Nirvana.Admin.BLL;
using Nirvana.Admin.Utility;

using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Sinks;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tracing;

#endregion

namespace Nirvana.Admin.Controls
{
	/// <summary>
	/// Summary description for ClientCompany.
	/// </summary>
	public class ClientCompany : System.Windows.Forms.UserControl
	{
		const string C_COMBO_SELECT = "- Select -";
		private const string FORM_NAME = "ClientCompany : ";
		#region Private and Protected members.

		private System.Windows.Forms.GroupBox gpbDetails;
		private System.Windows.Forms.ComboBox cmbCompanyTYpe;
		private System.Windows.Forms.TextBox txtTelephone;
		private System.Windows.Forms.TextBox txtFax;
		private System.Windows.Forms.TextBox txtAddress2;
		private System.Windows.Forms.TextBox txtAddress1;
		private System.Windows.Forms.TextBox txtCompanyName;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox gpbSecondaryContact;
		private System.Windows.Forms.TextBox txtPC2Telephone;
		private System.Windows.Forms.TextBox txtPC2Cell;
		private System.Windows.Forms.TextBox txtPC2Title;
		private System.Windows.Forms.TextBox txtPC2LastName;
		private System.Windows.Forms.TextBox txtPC2FirstName;
		private System.Windows.Forms.Label label30;
		private System.Windows.Forms.Label label31;
		private System.Windows.Forms.Label label32;
		private System.Windows.Forms.Label label33;
		private System.Windows.Forms.Label label34;
		private System.Windows.Forms.Label label35;
		private System.Windows.Forms.TextBox txtPC2Email;
		private System.Windows.Forms.GroupBox gpbPrimaryContact;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.TextBox txtPC1FirstName;
		private System.Windows.Forms.TextBox txtPC1LastName;
		private System.Windows.Forms.TextBox txtPC1Title;
		private System.Windows.Forms.TextBox txtPC1Cell;
		private System.Windows.Forms.TextBox txtPC1Telephone;
		private System.Windows.Forms.TextBox txtPC1Email;
		private System.Windows.Forms.Label label19;

		private int _companyID = int.MinValue;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#endregion

		public int CompanyID
		{
			set{_companyID = value;}
		}

		public ClientCompany()
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
			this.gpbDetails = new System.Windows.Forms.GroupBox();
			this.cmbCompanyTYpe = new System.Windows.Forms.ComboBox();
			this.txtTelephone = new System.Windows.Forms.TextBox();
			this.txtFax = new System.Windows.Forms.TextBox();
			this.txtAddress2 = new System.Windows.Forms.TextBox();
			this.txtAddress1 = new System.Windows.Forms.TextBox();
			this.txtCompanyName = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.gpbSecondaryContact = new System.Windows.Forms.GroupBox();
			this.txtPC2Telephone = new System.Windows.Forms.TextBox();
			this.txtPC2Cell = new System.Windows.Forms.TextBox();
			this.txtPC2Title = new System.Windows.Forms.TextBox();
			this.txtPC2LastName = new System.Windows.Forms.TextBox();
			this.txtPC2FirstName = new System.Windows.Forms.TextBox();
			this.label30 = new System.Windows.Forms.Label();
			this.label31 = new System.Windows.Forms.Label();
			this.label32 = new System.Windows.Forms.Label();
			this.label33 = new System.Windows.Forms.Label();
			this.label34 = new System.Windows.Forms.Label();
			this.label35 = new System.Windows.Forms.Label();
			this.txtPC2Email = new System.Windows.Forms.TextBox();
			this.gpbPrimaryContact = new System.Windows.Forms.GroupBox();
			this.label13 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.label18 = new System.Windows.Forms.Label();
			this.txtPC1FirstName = new System.Windows.Forms.TextBox();
			this.txtPC1LastName = new System.Windows.Forms.TextBox();
			this.txtPC1Title = new System.Windows.Forms.TextBox();
			this.txtPC1Cell = new System.Windows.Forms.TextBox();
			this.txtPC1Telephone = new System.Windows.Forms.TextBox();
			this.txtPC1Email = new System.Windows.Forms.TextBox();
			this.label19 = new System.Windows.Forms.Label();
			this.gpbDetails.SuspendLayout();
			this.gpbSecondaryContact.SuspendLayout();
			this.gpbPrimaryContact.SuspendLayout();
			this.SuspendLayout();
			// 
			// gpbDetails
			// 
			this.gpbDetails.Controls.Add(this.cmbCompanyTYpe);
			this.gpbDetails.Controls.Add(this.txtTelephone);
			this.gpbDetails.Controls.Add(this.txtFax);
			this.gpbDetails.Controls.Add(this.txtAddress2);
			this.gpbDetails.Controls.Add(this.txtAddress1);
			this.gpbDetails.Controls.Add(this.txtCompanyName);
			this.gpbDetails.Controls.Add(this.label6);
			this.gpbDetails.Controls.Add(this.label5);
			this.gpbDetails.Controls.Add(this.label4);
			this.gpbDetails.Controls.Add(this.label3);
			this.gpbDetails.Controls.Add(this.label2);
			this.gpbDetails.Controls.Add(this.label1);
			this.gpbDetails.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
			this.gpbDetails.Location = new System.Drawing.Point(2, 6);
			this.gpbDetails.Name = "gpbDetails";
			this.gpbDetails.Size = new System.Drawing.Size(320, 168);
			this.gpbDetails.TabIndex = 8;
			this.gpbDetails.TabStop = false;
			this.gpbDetails.Text = "Details";
			// 
			// cmbCompanyTYpe
			// 
			this.cmbCompanyTYpe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbCompanyTYpe.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.cmbCompanyTYpe.Location = new System.Drawing.Point(128, 88);
			this.cmbCompanyTYpe.Name = "cmbCompanyTYpe";
			this.cmbCompanyTYpe.Size = new System.Drawing.Size(184, 21);
			this.cmbCompanyTYpe.TabIndex = 11;
			this.cmbCompanyTYpe.GotFocus += new System.EventHandler(this.cmbCompanyTYpe_GotFocus);
			this.cmbCompanyTYpe.LostFocus += new System.EventHandler(this.cmbCompanyTYpe_LostFocus);
			// 
			// txtTelephone
			// 
			this.txtTelephone.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtTelephone.Location = new System.Drawing.Point(128, 112);
			this.txtTelephone.MaxLength = 50;
			this.txtTelephone.Name = "txtTelephone";
			this.txtTelephone.Size = new System.Drawing.Size(184, 21);
			this.txtTelephone.TabIndex = 10;
			this.txtTelephone.Text = "34";
			this.txtTelephone.LostFocus += new System.EventHandler(this.txtTelephone_LostFocus);
			this.txtTelephone.GotFocus += new System.EventHandler(this.txtTelephone_GotFocus);
			// 
			// txtFax
			// 
			this.txtFax.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtFax.Location = new System.Drawing.Point(128, 136);
			this.txtFax.MaxLength = 50;
			this.txtFax.Name = "txtFax";
			this.txtFax.Size = new System.Drawing.Size(184, 21);
			this.txtFax.TabIndex = 9;
			this.txtFax.Text = "34";
			this.txtFax.LostFocus += new System.EventHandler(this.txtFax_LostFocus);
			this.txtFax.GotFocus += new System.EventHandler(this.txtFax_GotFocus);
			// 
			// txtAddress2
			// 
			this.txtAddress2.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtAddress2.Location = new System.Drawing.Point(128, 64);
			this.txtAddress2.MaxLength = 50;
			this.txtAddress2.Name = "txtAddress2";
			this.txtAddress2.Size = new System.Drawing.Size(184, 21);
			this.txtAddress2.TabIndex = 8;
			this.txtAddress2.Text = "34";
			this.txtAddress2.LostFocus += new System.EventHandler(this.txtAddress2_LostFocus);
			this.txtAddress2.GotFocus += new System.EventHandler(this.txtAddress2_GotFocus);
			// 
			// txtAddress1
			// 
			this.txtAddress1.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtAddress1.Location = new System.Drawing.Point(128, 40);
			this.txtAddress1.MaxLength = 50;
			this.txtAddress1.Name = "txtAddress1";
			this.txtAddress1.Size = new System.Drawing.Size(184, 21);
			this.txtAddress1.TabIndex = 7;
			this.txtAddress1.Text = "34";
			this.txtAddress1.LostFocus += new System.EventHandler(this.txtAddress1_LostFocus);
			this.txtAddress1.GotFocus += new System.EventHandler(this.txtAddress1_GotFocus);
			// 
			// txtCompanyName
			// 
			this.txtCompanyName.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtCompanyName.Location = new System.Drawing.Point(128, 16);
			this.txtCompanyName.MaxLength = 50;
			this.txtCompanyName.Name = "txtCompanyName";
			this.txtCompanyName.Size = new System.Drawing.Size(184, 21);
			this.txtCompanyName.TabIndex = 6;
			this.txtCompanyName.Text = "34";
			this.txtCompanyName.LostFocus += new System.EventHandler(this.txtCompanyName_LostFocus);
			this.txtCompanyName.GotFocus += new System.EventHandler(this.txtCompanyName_GotFocus);
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label6.Location = new System.Drawing.Point(8, 144);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(120, 16);
			this.label6.TabIndex = 5;
			this.label6.Text = "Fax";
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label5.Location = new System.Drawing.Point(8, 120);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(120, 16);
			this.label5.TabIndex = 4;
			this.label5.Text = "Telephone";
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label4.Location = new System.Drawing.Point(8, 96);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(120, 16);
			this.label4.TabIndex = 3;
			this.label4.Text = "Company Type";
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label3.Location = new System.Drawing.Point(8, 72);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(120, 16);
			this.label3.TabIndex = 2;
			this.label3.Text = "Mailing Address 2";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label2.Location = new System.Drawing.Point(8, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(120, 16);
			this.label2.TabIndex = 1;
			this.label2.Text = "Mailing Address 1";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label1.Location = new System.Drawing.Point(8, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(120, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Company Name";
			// 
			// gpbSecondaryContact
			// 
			this.gpbSecondaryContact.Controls.Add(this.txtPC2Telephone);
			this.gpbSecondaryContact.Controls.Add(this.txtPC2Cell);
			this.gpbSecondaryContact.Controls.Add(this.txtPC2Title);
			this.gpbSecondaryContact.Controls.Add(this.txtPC2LastName);
			this.gpbSecondaryContact.Controls.Add(this.txtPC2FirstName);
			this.gpbSecondaryContact.Controls.Add(this.label30);
			this.gpbSecondaryContact.Controls.Add(this.label31);
			this.gpbSecondaryContact.Controls.Add(this.label32);
			this.gpbSecondaryContact.Controls.Add(this.label33);
			this.gpbSecondaryContact.Controls.Add(this.label34);
			this.gpbSecondaryContact.Controls.Add(this.label35);
			this.gpbSecondaryContact.Controls.Add(this.txtPC2Email);
			this.gpbSecondaryContact.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
			this.gpbSecondaryContact.Location = new System.Drawing.Point(330, 6);
			this.gpbSecondaryContact.Name = "gpbSecondaryContact";
			this.gpbSecondaryContact.Size = new System.Drawing.Size(320, 168);
			this.gpbSecondaryContact.TabIndex = 9;
			this.gpbSecondaryContact.TabStop = false;
			this.gpbSecondaryContact.Text = "Secondary Contact";
			// 
			// txtPC2Telephone
			// 
			this.txtPC2Telephone.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtPC2Telephone.Location = new System.Drawing.Point(128, 112);
			this.txtPC2Telephone.MaxLength = 50;
			this.txtPC2Telephone.Name = "txtPC2Telephone";
			this.txtPC2Telephone.Size = new System.Drawing.Size(184, 21);
			this.txtPC2Telephone.TabIndex = 22;
			this.txtPC2Telephone.Text = "34";
			this.txtPC2Telephone.LostFocus += new System.EventHandler(this.txtPC2Telephone_LostFocus);
			this.txtPC2Telephone.GotFocus += new System.EventHandler(this.txtPC2Telephone_GotFocus);
			// 
			// txtPC2Cell
			// 
			this.txtPC2Cell.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtPC2Cell.Location = new System.Drawing.Point(128, 136);
			this.txtPC2Cell.MaxLength = 50;
			this.txtPC2Cell.Name = "txtPC2Cell";
			this.txtPC2Cell.Size = new System.Drawing.Size(184, 21);
			this.txtPC2Cell.TabIndex = 21;
			this.txtPC2Cell.Text = "34";
			this.txtPC2Cell.LostFocus += new System.EventHandler(this.txtPC2Cell_LostFocus);
			this.txtPC2Cell.GotFocus += new System.EventHandler(this.txtPC2Cell_GotFocus);
			// 
			// txtPC2Title
			// 
			this.txtPC2Title.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtPC2Title.Location = new System.Drawing.Point(128, 64);
			this.txtPC2Title.MaxLength = 50;
			this.txtPC2Title.Name = "txtPC2Title";
			this.txtPC2Title.Size = new System.Drawing.Size(184, 21);
			this.txtPC2Title.TabIndex = 20;
			this.txtPC2Title.Text = "34";
			this.txtPC2Title.LostFocus += new System.EventHandler(this.txtPC2Title_LostFocus);
			this.txtPC2Title.GotFocus += new System.EventHandler(this.txtPC2Title_GotFocus);
			// 
			// txtPC2LastName
			// 
			this.txtPC2LastName.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtPC2LastName.Location = new System.Drawing.Point(128, 40);
			this.txtPC2LastName.MaxLength = 50;
			this.txtPC2LastName.Name = "txtPC2LastName";
			this.txtPC2LastName.Size = new System.Drawing.Size(184, 21);
			this.txtPC2LastName.TabIndex = 19;
			this.txtPC2LastName.Text = "34";
			this.txtPC2LastName.LostFocus += new System.EventHandler(this.txtPC2LastName_LostFocus);
			this.txtPC2LastName.GotFocus += new System.EventHandler(this.txtPC2LastName_GotFocus);
			// 
			// txtPC2FirstName
			// 
			this.txtPC2FirstName.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtPC2FirstName.Location = new System.Drawing.Point(128, 16);
			this.txtPC2FirstName.MaxLength = 50;
			this.txtPC2FirstName.Name = "txtPC2FirstName";
			this.txtPC2FirstName.Size = new System.Drawing.Size(184, 21);
			this.txtPC2FirstName.TabIndex = 18;
			this.txtPC2FirstName.Text = "34";
			this.txtPC2FirstName.LostFocus += new System.EventHandler(this.txtPC2FirstName_LostFocus);
			this.txtPC2FirstName.GotFocus += new System.EventHandler(this.txtPC2FirstName_GotFocus);
			// 
			// label30
			// 
			this.label30.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label30.Location = new System.Drawing.Point(8, 96);
			this.label30.Name = "label30";
			this.label30.Size = new System.Drawing.Size(120, 16);
			this.label30.TabIndex = 15;
			this.label30.Text = "EMail";
			// 
			// label31
			// 
			this.label31.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label31.Location = new System.Drawing.Point(8, 120);
			this.label31.Name = "label31";
			this.label31.Size = new System.Drawing.Size(120, 16);
			this.label31.TabIndex = 16;
			this.label31.Text = "Telephone #";
			// 
			// label32
			// 
			this.label32.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label32.Location = new System.Drawing.Point(8, 144);
			this.label32.Name = "label32";
			this.label32.Size = new System.Drawing.Size(120, 16);
			this.label32.TabIndex = 17;
			this.label32.Text = "Cell #";
			// 
			// label33
			// 
			this.label33.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label33.Location = new System.Drawing.Point(8, 24);
			this.label33.Name = "label33";
			this.label33.Size = new System.Drawing.Size(120, 16);
			this.label33.TabIndex = 12;
			this.label33.Text = "First Name";
			// 
			// label34
			// 
			this.label34.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label34.Location = new System.Drawing.Point(8, 48);
			this.label34.Name = "label34";
			this.label34.Size = new System.Drawing.Size(120, 16);
			this.label34.TabIndex = 13;
			this.label34.Text = "Last Name";
			// 
			// label35
			// 
			this.label35.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label35.Location = new System.Drawing.Point(8, 72);
			this.label35.Name = "label35";
			this.label35.Size = new System.Drawing.Size(120, 16);
			this.label35.TabIndex = 14;
			this.label35.Text = "Title";
			// 
			// txtPC2Email
			// 
			this.txtPC2Email.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtPC2Email.Location = new System.Drawing.Point(128, 88);
			this.txtPC2Email.MaxLength = 50;
			this.txtPC2Email.Name = "txtPC2Email";
			this.txtPC2Email.Size = new System.Drawing.Size(184, 21);
			this.txtPC2Email.TabIndex = 20;
			this.txtPC2Email.Text = "34";
			this.txtPC2Email.LostFocus += new System.EventHandler(this.txtPC2Email_LostFocus);
			this.txtPC2Email.GotFocus += new System.EventHandler(this.txtPC2Email_GotFocus);
			// 
			// gpbPrimaryContact
			// 
			this.gpbPrimaryContact.Controls.Add(this.label13);
			this.gpbPrimaryContact.Controls.Add(this.label14);
			this.gpbPrimaryContact.Controls.Add(this.label15);
			this.gpbPrimaryContact.Controls.Add(this.label16);
			this.gpbPrimaryContact.Controls.Add(this.label17);
			this.gpbPrimaryContact.Controls.Add(this.label18);
			this.gpbPrimaryContact.Controls.Add(this.txtPC1FirstName);
			this.gpbPrimaryContact.Controls.Add(this.txtPC1LastName);
			this.gpbPrimaryContact.Controls.Add(this.txtPC1Title);
			this.gpbPrimaryContact.Controls.Add(this.txtPC1Cell);
			this.gpbPrimaryContact.Controls.Add(this.txtPC1Telephone);
			this.gpbPrimaryContact.Controls.Add(this.txtPC1Email);
			this.gpbPrimaryContact.Controls.Add(this.label19);
			this.gpbPrimaryContact.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
			this.gpbPrimaryContact.Location = new System.Drawing.Point(2, 174);
			this.gpbPrimaryContact.Name = "gpbPrimaryContact";
			this.gpbPrimaryContact.Size = new System.Drawing.Size(320, 168);
			this.gpbPrimaryContact.TabIndex = 10;
			this.gpbPrimaryContact.TabStop = false;
			this.gpbPrimaryContact.Text = "Primary Contact";
			// 
			// label13
			// 
			this.label13.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label13.Location = new System.Drawing.Point(8, 24);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(120, 16);
			this.label13.TabIndex = 12;
			this.label13.Text = "First Name";
			// 
			// label14
			// 
			this.label14.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label14.Location = new System.Drawing.Point(8, 48);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(120, 16);
			this.label14.TabIndex = 13;
			this.label14.Text = "Last Name";
			// 
			// label15
			// 
			this.label15.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label15.Location = new System.Drawing.Point(8, 72);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(120, 16);
			this.label15.TabIndex = 14;
			this.label15.Text = "Title";
			// 
			// label16
			// 
			this.label16.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label16.Location = new System.Drawing.Point(8, 96);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(120, 16);
			this.label16.TabIndex = 15;
			this.label16.Text = "EMail";
			// 
			// label17
			// 
			this.label17.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label17.Location = new System.Drawing.Point(8, 120);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(120, 16);
			this.label17.TabIndex = 16;
			this.label17.Text = "Telephone #";
			// 
			// label18
			// 
			this.label18.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label18.Location = new System.Drawing.Point(8, 144);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(120, 16);
			this.label18.TabIndex = 17;
			this.label18.Text = "Cell #";
			// 
			// txtPC1FirstName
			// 
			this.txtPC1FirstName.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtPC1FirstName.Location = new System.Drawing.Point(128, 16);
			this.txtPC1FirstName.MaxLength = 50;
			this.txtPC1FirstName.Name = "txtPC1FirstName";
			this.txtPC1FirstName.Size = new System.Drawing.Size(184, 21);
			this.txtPC1FirstName.TabIndex = 18;
			this.txtPC1FirstName.Text = "34";
			this.txtPC1FirstName.LostFocus += new System.EventHandler(this.txtPC1FirstName_LostFocus);
			this.txtPC1FirstName.GotFocus += new System.EventHandler(this.txtPC1FirstName_GotFocus);
			// 
			// txtPC1LastName
			// 
			this.txtPC1LastName.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtPC1LastName.Location = new System.Drawing.Point(128, 40);
			this.txtPC1LastName.MaxLength = 50;
			this.txtPC1LastName.Name = "txtPC1LastName";
			this.txtPC1LastName.Size = new System.Drawing.Size(184, 21);
			this.txtPC1LastName.TabIndex = 19;
			this.txtPC1LastName.Text = "34";
			this.txtPC1LastName.LostFocus += new System.EventHandler(this.txtPC1LastName_LostFocus);
			this.txtPC1LastName.GotFocus += new System.EventHandler(this.txtPC1LastName_GotFocus);
			// 
			// txtPC1Title
			// 
			this.txtPC1Title.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtPC1Title.Location = new System.Drawing.Point(128, 64);
			this.txtPC1Title.MaxLength = 50;
			this.txtPC1Title.Name = "txtPC1Title";
			this.txtPC1Title.Size = new System.Drawing.Size(184, 21);
			this.txtPC1Title.TabIndex = 20;
			this.txtPC1Title.Text = "34";
			this.txtPC1Title.LostFocus += new System.EventHandler(this.txtPC1Title_LostFocus);
			this.txtPC1Title.GotFocus += new System.EventHandler(this.txtPC1Title_GotFocus);
			// 
			// txtPC1Cell
			// 
			this.txtPC1Cell.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtPC1Cell.Location = new System.Drawing.Point(128, 136);
			this.txtPC1Cell.MaxLength = 50;
			this.txtPC1Cell.Name = "txtPC1Cell";
			this.txtPC1Cell.Size = new System.Drawing.Size(184, 21);
			this.txtPC1Cell.TabIndex = 21;
			this.txtPC1Cell.Text = "34";
			this.txtPC1Cell.LostFocus += new System.EventHandler(this.txtPC1Cell_LostFocus);
			this.txtPC1Cell.GotFocus += new System.EventHandler(this.txtPC1Cell_GotFocus);
			// 
			// txtPC1Telephone
			// 
			this.txtPC1Telephone.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtPC1Telephone.Location = new System.Drawing.Point(128, 112);
			this.txtPC1Telephone.MaxLength = 50;
			this.txtPC1Telephone.Name = "txtPC1Telephone";
			this.txtPC1Telephone.Size = new System.Drawing.Size(184, 21);
			this.txtPC1Telephone.TabIndex = 22;
			this.txtPC1Telephone.Text = "34";
			this.txtPC1Telephone.LostFocus += new System.EventHandler(this.txtFax_LostFocus);
			this.txtPC1Telephone.GotFocus += new System.EventHandler(this.txtFax_GotFocus);
			// 
			// txtPC1Email
			// 
			this.txtPC1Email.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtPC1Email.Location = new System.Drawing.Point(128, 88);
			this.txtPC1Email.MaxLength = 50;
			this.txtPC1Email.Name = "txtPC1Email";
			this.txtPC1Email.Size = new System.Drawing.Size(184, 21);
			this.txtPC1Email.TabIndex = 20;
			this.txtPC1Email.Text = "34";
			this.txtPC1Email.LostFocus += new System.EventHandler(this.txtPC1Email_LostFocus);
			this.txtPC1Email.GotFocus += new System.EventHandler(this.txtPC1Email_GotFocus);
			// 
			// label19
			// 
			this.label19.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label19.Location = new System.Drawing.Point(8, 48);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(120, 16);
			this.label19.TabIndex = 12;
			this.label19.Text = "First Name";
			// 
			// ClientCompany
			// 
			this.Controls.Add(this.gpbPrimaryContact);
			this.Controls.Add(this.gpbSecondaryContact);
			this.Controls.Add(this.gpbDetails);
			this.Name = "ClientCompany";
			this.Size = new System.Drawing.Size(654, 350);
			this.Load += new System.EventHandler(this.ClientCompany_Load);
			this.gpbDetails.ResumeLayout(false);
			this.gpbSecondaryContact.ResumeLayout(false);
			this.gpbPrimaryContact.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Public Properties
		
		public CompanyClient CompanyClient
		{
			get{return GetCompanyClient();}
			set{SetCompanyClient(value);}			
		}

		public void RefreshCompanyClientDetail()
		{
			txtAddress1.Text = "";
			txtAddress2.Text = "";
			txtCompanyName.Text = "";
			txtFax.Text = "";
			txtPC1Cell.Text = "";
			txtPC1Email.Text = "";
			txtPC1FirstName.Text = "";
			txtPC1LastName.Text = "";
			txtPC1Telephone.Text = "";
			txtPC1Title.Text = "";
			txtPC2Cell.Text = "";
			txtPC2Email.Text = "";
			txtPC2FirstName.Text = "";
			txtPC2LastName.Text ="";
			txtPC2Telephone.Text = "";
			txtPC2Title.Text = "";
			txtTelephone.Text = "";
		}
		
		private void SetCompanyClient(CompanyClient companyClient)
		{
			if(companyClient != null)
			{
				txtAddress1.Text = companyClient.Address1;
				txtAddress2.Text = companyClient.Address2;
				cmbCompanyTYpe.SelectedValue = companyClient.CompanyTypeID;
				txtTelephone.Text = companyClient.Telephone;
				txtFax.Text = companyClient.Fax;
				txtCompanyName.Text = companyClient.Name;			
				txtPC1Cell.Text = companyClient.PrimaryContactCell;
				txtPC1FirstName.Text = companyClient.PrimaryContactFirstName;
				txtPC1LastName.Text = companyClient.PrimaryContactLastName;
				txtPC1Email.Text = companyClient.PrimaryContactEMail;
				txtPC1Telephone.Text = companyClient.PrimaryContactTelephone;
				txtPC1Title.Text = companyClient.PrimaryContactTitle;			
				txtPC2Cell.Text = companyClient.SecondaryContactCell;
				txtPC2FirstName.Text = companyClient.SecondaryContactFirstName;
				txtPC2LastName.Text = companyClient.SecondaryContactLastName;
				txtPC2Email.Text = companyClient.SecondaryContactEMail;
				txtPC2Telephone.Text = companyClient.SecondaryContactTelephone;
				txtPC2Title.Text = companyClient.SecondaryContactTitle;			
			}
		}

		private CompanyClient GetCompanyClient()
		{
			CompanyClient companyClient = new CompanyClient();

			companyClient.Address1 = txtAddress1.Text;
			companyClient.Address2 = txtAddress2.Text;
			companyClient.CompanyTypeID = int.Parse(cmbCompanyTYpe.SelectedValue.ToString());
			companyClient.Telephone = txtTelephone.Text;
			
			companyClient.Fax = txtFax.Text;
			companyClient.Name = txtCompanyName.Text;			
			companyClient.PrimaryContactCell = txtPC1Cell.Text;
			companyClient.PrimaryContactFirstName = txtPC1FirstName.Text;
			companyClient.PrimaryContactLastName = txtPC1LastName.Text;
			companyClient.PrimaryContactEMail = txtPC1Email.Text;
			companyClient.PrimaryContactTelephone = txtPC1Telephone.Text;
			companyClient.PrimaryContactTitle = txtPC1Title.Text;			
			companyClient.SecondaryContactCell = txtPC2Cell.Text;
			companyClient.SecondaryContactFirstName = txtPC2FirstName.Text;
			companyClient.SecondaryContactLastName = txtPC2LastName.Text;
			companyClient.SecondaryContactEMail = txtPC2Email.Text;
			companyClient.SecondaryContactTelephone = txtPC2Telephone.Text;
			companyClient.SecondaryContactTitle = txtPC2Title.Text;			
			
			return companyClient;
			//return null;
		}

		#endregion

		public int CompanyClientSave(CompanyClient companyClient)
		{
			int result = int.MinValue;
			if (txtCompanyName.Text == "")
			{
				//_statusBar.Text = "Please enter Client Company name!";
				txtCompanyName.Focus();
			}
			else if (txtAddress1.Text == "")
			{
				//_statusBar.Text = "Please enter Address 1 in details!";
				txtAddress1.Focus();
			}
			else if(int.Parse(cmbCompanyTYpe.SelectedValue.ToString()) == int.MinValue)
			{
				//_statusBar.Text = "Please select Company Type!";
				cmbCompanyTYpe.Focus();
			}
			else if (txtPC1LastName.Text == "")
			{
				//_statusBar.Text = "Please enter Primary contact First name in details!";
				txtAddress1.Focus();
			}
			else if (txtPC1Email.Text == "")
			{
				//_statusBar.Text = "Please enter Primary contact Email in details!";
				txtPC1Email.Focus();
			}
			else
			{
				companyClient.Address1 = txtAddress1.Text.Trim();
				companyClient.Address2 = txtAddress2.Text.Trim();
				companyClient.CompanyTypeID = int.Parse(cmbCompanyTYpe.SelectedValue.ToString());
				companyClient.Telephone = txtTelephone.Text.Trim();
			
				companyClient.Fax = txtFax.Text.Trim();
				companyClient.Name = txtCompanyName.Text.Trim();			
				companyClient.PrimaryContactCell = txtPC1Cell.Text.Trim();
				companyClient.PrimaryContactFirstName = txtPC1FirstName.Text.Trim();
				companyClient.PrimaryContactLastName = txtPC1LastName.Text.Trim();
				companyClient.PrimaryContactEMail = txtPC1Email.Text.Trim();
				companyClient.PrimaryContactTelephone = txtPC1Telephone.Text.Trim();
				companyClient.PrimaryContactTitle = txtPC1Title.Text.Trim();			
				companyClient.SecondaryContactCell = txtPC2Cell.Text.Trim();
				companyClient.SecondaryContactFirstName = txtPC2FirstName.Text.Trim();
				companyClient.SecondaryContactLastName = txtPC2LastName.Text.Trim();
				companyClient.SecondaryContactEMail = txtPC2Email.Text.Trim();
				companyClient.SecondaryContactTelephone = txtPC2Telephone.Text.Trim();
				companyClient.SecondaryContactTitle = txtPC2Title.Text.Trim();
				
				result = 1;
			}			
			return result;			
		}
		
		private void ClientCompany_Load(object sender, System.EventArgs e)
		{
			try
			{
				BindCompanyType();
			}
			catch(Exception ex)
			{
				string formattedInfo = ex.StackTrace.ToString();
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, Severity.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler();
				appMessageExceptionHandler.HandleException(new Exception(Common.ERROR_STATEMENT), Common.POLICY_GLOBAL, System.Guid.NewGuid());
			}
			finally
			{
				#region LogEntry

				LogEntry logEntry = new LogEntry("btnLogin_Click", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "btnLogin_Click"); 
				Logger.Write(logEntry); 

				#endregion
			}
		}

		#region Private Properties

		private void BindCompanyType()
		{
			CompanyTypes companyTypes = CompanyManager.GetCompanyTypes();
			companyTypes.Insert(0, new CompanyType(int.MinValue, C_COMBO_SELECT));
			cmbCompanyTYpe.DataSource = companyTypes;
			cmbCompanyTYpe.DisplayMember = "Type";
			cmbCompanyTYpe.ValueMember = "CompanyTypeID";
		}

		#endregion

		# region Controls Focus Colors
		private void txtTelephone_GotFocus(object sender, System.EventArgs e)
		{
			txtTelephone.BackColor = Color.LemonChiffon;
		}
		private void txtTelephone_LostFocus(object sender, System.EventArgs e)
		{
			txtTelephone.BackColor = Color.White;
		}
		
		private void txtAddress1_GotFocus(object sender, System.EventArgs e)
		{
			txtAddress1.BackColor = Color.LemonChiffon;
		}
		private void txtAddress1_LostFocus(object sender, System.EventArgs e)
		{
			txtAddress1.BackColor = Color.White;
		}
		private void txtAddress2_GotFocus(object sender, System.EventArgs e)
		{
			txtAddress2.BackColor = Color.LemonChiffon;
		}
		private void txtAddress2_LostFocus(object sender, System.EventArgs e)
		{
			txtAddress2.BackColor = Color.White;
		}
		private void txtFax_GotFocus(object sender, System.EventArgs e)
		{
			txtFax.BackColor = Color.LemonChiffon;
		}
		private void txtFax_LostFocus(object sender, System.EventArgs e)
		{
			txtFax.BackColor = Color.White;
        }
		private void txtCompanyName_GotFocus(object sender, System.EventArgs e)
		{
			txtCompanyName.BackColor = Color.LemonChiffon;
		}
		private void txtCompanyName_LostFocus(object sender, System.EventArgs e)
		{
			txtCompanyName.BackColor = Color.White;
		}
		private void txtPC1Cell_GotFocus(object sender, System.EventArgs e)
		{
			txtPC1Cell.BackColor = Color.LemonChiffon;
		}
		private void txtPC1Cell_LostFocus(object sender, System.EventArgs e)
		{
			txtPC1Cell.BackColor = Color.White;
		}
		private void txtPC1Email_GotFocus(object sender, System.EventArgs e)
		{
			txtPC1Email.BackColor = Color.LemonChiffon;
		}
		private void txtPC1Email_LostFocus(object sender, System.EventArgs e)
		{
			txtPC1Email.BackColor = Color.White;
		}
		private void txtPC1FirstName_GotFocus(object sender, System.EventArgs e)
		{
			txtPC1FirstName.BackColor = Color.LemonChiffon;
		}
		private void txtPC1FirstName_LostFocus(object sender, System.EventArgs e)
		{
			txtPC1FirstName.BackColor = Color.White;
		}
		private void txtPC1LastName_GotFocus(object sender, System.EventArgs e)
		{
			txtPC1LastName.BackColor = Color.LemonChiffon;
		}
		private void txtPC1LastName_LostFocus(object sender, System.EventArgs e)
		{
			txtPC1LastName.BackColor = Color.White;
		}
		private void txtPC1Telephone_GotFocus(object sender, System.EventArgs e)
		{
			txtPC1Telephone.BackColor = Color.LemonChiffon;
		}
		private void txtPC1Telephone_LostFocus(object sender, System.EventArgs e)
		{
			txtPC1Telephone.BackColor = Color.White;
		}
		private void txtPC1Title_GotFocus(object sender, System.EventArgs e)
		{
			txtPC1Title.BackColor = Color.LemonChiffon;
		}
		private void txtPC1Title_LostFocus(object sender, System.EventArgs e)
		{
			txtPC1Title.BackColor = Color.White;
		}
		private void txtPC2Cell_GotFocus(object sender, System.EventArgs e)
		{
			txtPC2Cell.BackColor = Color.LemonChiffon;
		}
		private void txtPC2Cell_LostFocus(object sender, System.EventArgs e)
		{
			txtPC2Cell.BackColor = Color.White;
		}
		private void txtPC2Email_GotFocus(object sender, System.EventArgs e)
		{
			txtPC2Email.BackColor = Color.LemonChiffon;
		}
		private void txtPC2Email_LostFocus(object sender, System.EventArgs e)
		{
			txtPC2Email.BackColor = Color.White;
		}
		private void txtPC2FirstName_GotFocus(object sender, System.EventArgs e)
		{
			txtPC2FirstName.BackColor = Color.LemonChiffon;
		}
		private void txtPC2FirstName_LostFocus(object sender, System.EventArgs e)
		{
			txtPC2FirstName.BackColor = Color.White;
		}
		private void txtPC2LastName_GotFocus(object sender, System.EventArgs e)
		{
			txtPC2LastName.BackColor = Color.LemonChiffon;
		}
		private void txtPC2LastName_LostFocus(object sender, System.EventArgs e)
		{
			txtPC2LastName.BackColor = Color.White;
		}
		private void txtPC2Telephone_GotFocus(object sender, System.EventArgs e)
		{
			txtPC2Telephone.BackColor = Color.LemonChiffon;
		}
		private void txtPC2Telephone_LostFocus(object sender, System.EventArgs e)
		{
			txtPC2Telephone.BackColor = Color.White;
		}
		private void txtPC2Title_GotFocus(object sender, System.EventArgs e)
		{
			txtPC2Title.BackColor = Color.LemonChiffon;
		}
		private void txtPC2Title_LostFocus(object sender, System.EventArgs e)
		{
			txtPC2Title.BackColor = Color.White;
		}
		private void cmbCompanyTYpe_GotFocus(object sender, System.EventArgs e)
		{
			cmbCompanyTYpe.BackColor = Color.LemonChiffon;
		}
		private void cmbCompanyTYpe_LostFocus(object sender, System.EventArgs e)
		{
			cmbCompanyTYpe.BackColor = Color.White;
		}
		
		#endregion
	}
}
