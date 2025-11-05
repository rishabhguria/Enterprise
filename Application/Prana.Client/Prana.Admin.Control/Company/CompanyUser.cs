#region Using

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Nirvana.Admin.BLL;



#endregion

namespace Nirvana.Admin.Controls
{
	/// <summary>
	/// Summary description for CompanyUser.
	/// </summary>
	public class CompanyUser : System.Windows.Forms.UserControl
	{
		#region private and protected members

		private int _companyID = int.MinValue;

		#endregion

		private System.Windows.Forms.ErrorProvider errorProvider1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox txtMailingAddress;
		private System.Windows.Forms.Label MailingAddress;
		private System.Windows.Forms.ComboBox cmbTradingPermission;
		private System.Windows.Forms.TextBox txtFirstName;
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
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.TextBox txtLastName;
		private System.Windows.Forms.TextBox txtLoginName;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.TextBox txtShortName;
		private System.Windows.Forms.TextBox txtTitle;
		private System.Windows.Forms.TextBox txtAddress1;
		private System.Windows.Forms.TextBox txtAddress2;
		private System.Windows.Forms.TextBox txtEMail;
		private System.Windows.Forms.TextBox txtTelephoneWork;
		private System.Windows.Forms.TextBox txtTelephoneCell;
		private System.Windows.Forms.TextBox txtPager;
		private System.Windows.Forms.TextBox txtTelephoneHome;
		private System.Windows.Forms.TextBox txtFax;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.Label label22;
		private System.Windows.Forms.Label label23;
		private System.Windows.Forms.Label label24;
		private System.Windows.Forms.Label label25;
		private System.Windows.Forms.Label label26;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public int CompanyID
		{
			//get{}
			set{_companyID = value;}
		}

		public CompanyUser()
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
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label26 = new System.Windows.Forms.Label();
			this.label25 = new System.Windows.Forms.Label();
			this.label24 = new System.Windows.Forms.Label();
			this.label23 = new System.Windows.Forms.Label();
			this.label22 = new System.Windows.Forms.Label();
			this.label21 = new System.Windows.Forms.Label();
			this.label20 = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.label18 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.txtMailingAddress = new System.Windows.Forms.TextBox();
			this.MailingAddress = new System.Windows.Forms.Label();
			this.cmbTradingPermission = new System.Windows.Forms.ComboBox();
			this.txtFirstName = new System.Windows.Forms.TextBox();
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
			this.label11 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.txtLastName = new System.Windows.Forms.TextBox();
			this.txtLoginName = new System.Windows.Forms.TextBox();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.txtShortName = new System.Windows.Forms.TextBox();
			this.txtTitle = new System.Windows.Forms.TextBox();
			this.txtAddress1 = new System.Windows.Forms.TextBox();
			this.txtAddress2 = new System.Windows.Forms.TextBox();
			this.txtEMail = new System.Windows.Forms.TextBox();
			this.txtTelephoneWork = new System.Windows.Forms.TextBox();
			this.txtTelephoneCell = new System.Windows.Forms.TextBox();
			this.txtPager = new System.Windows.Forms.TextBox();
			this.txtTelephoneHome = new System.Windows.Forms.TextBox();
			this.txtFax = new System.Windows.Forms.TextBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// errorProvider1
			// 
			this.errorProvider1.ContainerControl = this;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label26);
			this.groupBox1.Controls.Add(this.label25);
			this.groupBox1.Controls.Add(this.label24);
			this.groupBox1.Controls.Add(this.label23);
			this.groupBox1.Controls.Add(this.label22);
			this.groupBox1.Controls.Add(this.label21);
			this.groupBox1.Controls.Add(this.label20);
			this.groupBox1.Controls.Add(this.label19);
			this.groupBox1.Controls.Add(this.label18);
			this.groupBox1.Controls.Add(this.label17);
			this.groupBox1.Controls.Add(this.label16);
			this.groupBox1.Controls.Add(this.txtMailingAddress);
			this.groupBox1.Controls.Add(this.MailingAddress);
			this.groupBox1.Controls.Add(this.cmbTradingPermission);
			this.groupBox1.Controls.Add(this.txtFirstName);
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
			this.groupBox1.Controls.Add(this.label11);
			this.groupBox1.Controls.Add(this.label12);
			this.groupBox1.Controls.Add(this.label13);
			this.groupBox1.Controls.Add(this.label14);
			this.groupBox1.Controls.Add(this.label15);
			this.groupBox1.Controls.Add(this.txtLastName);
			this.groupBox1.Controls.Add(this.txtLoginName);
			this.groupBox1.Controls.Add(this.txtPassword);
			this.groupBox1.Controls.Add(this.txtShortName);
			this.groupBox1.Controls.Add(this.txtTitle);
			this.groupBox1.Controls.Add(this.txtAddress1);
			this.groupBox1.Controls.Add(this.txtAddress2);
			this.groupBox1.Controls.Add(this.txtEMail);
			this.groupBox1.Controls.Add(this.txtTelephoneWork);
			this.groupBox1.Controls.Add(this.txtTelephoneCell);
			this.groupBox1.Controls.Add(this.txtPager);
			this.groupBox1.Controls.Add(this.txtTelephoneHome);
			this.groupBox1.Controls.Add(this.txtFax);
			this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
			this.groupBox1.Location = new System.Drawing.Point(2, 2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(352, 378);
			this.groupBox1.TabIndex = 17;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "User Detail";
			// 
			// label26
			// 
			this.label26.ForeColor = System.Drawing.Color.Red;
			this.label26.Location = new System.Drawing.Point(138, 262);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(12, 14);
			this.label26.TabIndex = 59;
			this.label26.Text = "*";
			// 
			// label25
			// 
			this.label25.ForeColor = System.Drawing.Color.Red;
			this.label25.Location = new System.Drawing.Point(138, 240);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(12, 14);
			this.label25.TabIndex = 58;
			this.label25.Text = "*";
			// 
			// label24
			// 
			this.label24.ForeColor = System.Drawing.Color.Red;
			this.label24.Location = new System.Drawing.Point(138, 218);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(12, 14);
			this.label24.TabIndex = 57;
			this.label24.Text = "*";
			// 
			// label23
			// 
			this.label23.ForeColor = System.Drawing.Color.Red;
			this.label23.Location = new System.Drawing.Point(138, 174);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(12, 14);
			this.label23.TabIndex = 56;
			this.label23.Text = "*";
			// 
			// label22
			// 
			this.label22.ForeColor = System.Drawing.Color.Red;
			this.label22.Location = new System.Drawing.Point(138, 152);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(12, 14);
			this.label22.TabIndex = 55;
			this.label22.Text = "*";
			// 
			// label21
			// 
			this.label21.ForeColor = System.Drawing.Color.Red;
			this.label21.Location = new System.Drawing.Point(138, 130);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(12, 14);
			this.label21.TabIndex = 54;
			this.label21.Text = "*";
			// 
			// label20
			// 
			this.label20.ForeColor = System.Drawing.Color.Red;
			this.label20.Location = new System.Drawing.Point(138, 108);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(12, 14);
			this.label20.TabIndex = 53;
			this.label20.Text = "*";
			// 
			// label19
			// 
			this.label19.ForeColor = System.Drawing.Color.Red;
			this.label19.Location = new System.Drawing.Point(138, 86);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(12, 14);
			this.label19.TabIndex = 52;
			this.label19.Text = "*";
			// 
			// label18
			// 
			this.label18.ForeColor = System.Drawing.Color.Red;
			this.label18.Location = new System.Drawing.Point(138, 64);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(12, 14);
			this.label18.TabIndex = 51;
			this.label18.Text = "*";
			// 
			// label17
			// 
			this.label17.ForeColor = System.Drawing.Color.Red;
			this.label17.Location = new System.Drawing.Point(138, 42);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(12, 14);
			this.label17.TabIndex = 50;
			this.label17.Text = "*";
			// 
			// label16
			// 
			this.label16.ForeColor = System.Drawing.Color.Red;
			this.label16.Location = new System.Drawing.Point(138, 20);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(12, 14);
			this.label16.TabIndex = 49;
			this.label16.Text = "*";
			// 
			// txtMailingAddress
			// 
			this.txtMailingAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtMailingAddress.Enabled = false;
			this.txtMailingAddress.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtMailingAddress.Location = new System.Drawing.Point(150, 152);
			this.txtMailingAddress.MaxLength = 50;
			this.txtMailingAddress.Name = "txtMailingAddress";
			this.txtMailingAddress.Size = new System.Drawing.Size(176, 21);
			this.txtMailingAddress.TabIndex = 39;
			this.txtMailingAddress.Text = "";
			this.txtMailingAddress.LostFocus += new System.EventHandler(this.txtMailingAddress_LostFocus);
			this.txtMailingAddress.GotFocus += new System.EventHandler(this.txtMailingAddress_GotFocus);
			// 
			// MailingAddress
			// 
			this.MailingAddress.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.MailingAddress.Location = new System.Drawing.Point(10, 152);
			this.MailingAddress.Name = "MailingAddress";
			this.MailingAddress.Size = new System.Drawing.Size(116, 22);
			this.MailingAddress.TabIndex = 35;
			this.MailingAddress.Text = "Mailing Address";
			this.MailingAddress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cmbTradingPermission
			// 
			this.cmbTradingPermission.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbTradingPermission.Enabled = false;
			this.cmbTradingPermission.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.cmbTradingPermission.Location = new System.Drawing.Point(150, 350);
			this.cmbTradingPermission.Name = "cmbTradingPermission";
			this.cmbTradingPermission.Size = new System.Drawing.Size(176, 21);
			this.cmbTradingPermission.TabIndex = 48;
			this.cmbTradingPermission.GotFocus += new System.EventHandler(this.cmbTradingPermission_GotFocus);
			this.cmbTradingPermission.LostFocus += new System.EventHandler(this.cmbTradingPermission_LostFocus);
			// 
			// txtFirstName
			// 
			this.txtFirstName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtFirstName.Enabled = false;
			this.txtFirstName.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtFirstName.Location = new System.Drawing.Point(150, 20);
			this.txtFirstName.MaxLength = 50;
			this.txtFirstName.Name = "txtFirstName";
			this.txtFirstName.Size = new System.Drawing.Size(176, 21);
			this.txtFirstName.TabIndex = 32;
			this.txtFirstName.Text = "";
			this.txtFirstName.LostFocus += new System.EventHandler(this.txtFirstName_LostFocus);
			this.txtFirstName.GotFocus += new System.EventHandler(this.txtFirstName_GotFocus);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label1.Location = new System.Drawing.Point(10, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(116, 22);
			this.label1.TabIndex = 28;
			this.label1.Text = "First Name";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label2.Location = new System.Drawing.Point(10, 42);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(116, 22);
			this.label2.TabIndex = 27;
			this.label2.Text = "Last Name";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label3.Location = new System.Drawing.Point(10, 64);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(116, 22);
			this.label3.TabIndex = 29;
			this.label3.Text = "Login Name";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label4.Location = new System.Drawing.Point(10, 86);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(116, 22);
			this.label4.TabIndex = 31;
			this.label4.Text = "Password";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label5.Location = new System.Drawing.Point(10, 108);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(116, 22);
			this.label5.TabIndex = 30;
			this.label5.Text = "Short Name";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label6.Location = new System.Drawing.Point(10, 130);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(116, 22);
			this.label6.TabIndex = 26;
			this.label6.Text = "Title";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label7
			// 
			this.label7.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label7.Location = new System.Drawing.Point(10, 174);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(116, 22);
			this.label7.TabIndex = 19;
			this.label7.Text = "Address1";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label8
			// 
			this.label8.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label8.Location = new System.Drawing.Point(10, 196);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(116, 22);
			this.label8.TabIndex = 20;
			this.label8.Text = "Address2";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label9
			// 
			this.label9.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label9.Location = new System.Drawing.Point(10, 218);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(116, 22);
			this.label9.TabIndex = 17;
			this.label9.Text = "E-Mail";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label10
			// 
			this.label10.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label10.Location = new System.Drawing.Point(10, 240);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(116, 22);
			this.label10.TabIndex = 18;
			this.label10.Text = "Work Telephone #";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label11
			// 
			this.label11.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label11.Location = new System.Drawing.Point(10, 262);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(116, 22);
			this.label11.TabIndex = 21;
			this.label11.Text = "Cell #";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label12
			// 
			this.label12.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label12.Location = new System.Drawing.Point(10, 284);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(116, 22);
			this.label12.TabIndex = 24;
			this.label12.Text = "Pager #";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label13
			// 
			this.label13.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label13.Location = new System.Drawing.Point(10, 306);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(116, 22);
			this.label13.TabIndex = 25;
			this.label13.Text = "Home #";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label14
			// 
			this.label14.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label14.Location = new System.Drawing.Point(10, 328);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(116, 22);
			this.label14.TabIndex = 22;
			this.label14.Text = "Fax #";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label15
			// 
			this.label15.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label15.Location = new System.Drawing.Point(10, 348);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(116, 22);
			this.label15.TabIndex = 23;
			this.label15.Text = "Trading Permission";
			this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtLastName
			// 
			this.txtLastName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtLastName.Enabled = false;
			this.txtLastName.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtLastName.Location = new System.Drawing.Point(150, 42);
			this.txtLastName.MaxLength = 50;
			this.txtLastName.Name = "txtLastName";
			this.txtLastName.Size = new System.Drawing.Size(176, 21);
			this.txtLastName.TabIndex = 33;
			this.txtLastName.Text = "";
			this.txtLastName.LostFocus += new System.EventHandler(this.txtLastName_LostFocus);
			this.txtLastName.GotFocus += new System.EventHandler(this.txtLastName_GotFocus);
			// 
			// txtLoginName
			// 
			this.txtLoginName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtLoginName.Enabled = false;
			this.txtLoginName.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtLoginName.Location = new System.Drawing.Point(150, 64);
			this.txtLoginName.MaxLength = 50;
			this.txtLoginName.Name = "txtLoginName";
			this.txtLoginName.Size = new System.Drawing.Size(176, 21);
			this.txtLoginName.TabIndex = 34;
			this.txtLoginName.Text = "";
			this.txtLoginName.LostFocus += new System.EventHandler(this.txtLoginName_LostFocus);
			this.txtLoginName.GotFocus += new System.EventHandler(this.txtLoginName_GotFocus);
			// 
			// txtPassword
			// 
			this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtPassword.Enabled = false;
			this.txtPassword.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtPassword.Location = new System.Drawing.Point(150, 86);
			this.txtPassword.MaxLength = 50;
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.PasswordChar = '*';
			this.txtPassword.Size = new System.Drawing.Size(176, 21);
			this.txtPassword.TabIndex = 36;
			this.txtPassword.Text = "";
			this.txtPassword.LostFocus += new System.EventHandler(this.txtPassword_LostFocus);
			this.txtPassword.GotFocus += new System.EventHandler(this.txtPassword_GotFocus);
			// 
			// txtShortName
			// 
			this.txtShortName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtShortName.Enabled = false;
			this.txtShortName.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtShortName.Location = new System.Drawing.Point(150, 108);
			this.txtShortName.MaxLength = 50;
			this.txtShortName.Name = "txtShortName";
			this.txtShortName.Size = new System.Drawing.Size(176, 21);
			this.txtShortName.TabIndex = 37;
			this.txtShortName.Text = "";
			this.txtShortName.LostFocus += new System.EventHandler(this.txtShortName_LostFocus);
			this.txtShortName.GotFocus += new System.EventHandler(this.txtShortName_GotFocus);
			// 
			// txtTitle
			// 
			this.txtTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtTitle.Enabled = false;
			this.txtTitle.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtTitle.Location = new System.Drawing.Point(150, 130);
			this.txtTitle.MaxLength = 50;
			this.txtTitle.Name = "txtTitle";
			this.txtTitle.Size = new System.Drawing.Size(176, 21);
			this.txtTitle.TabIndex = 38;
			this.txtTitle.Text = "";
			this.txtTitle.LostFocus += new System.EventHandler(this.txtTitle_LostFocus);
			this.txtTitle.GotFocus += new System.EventHandler(this.txtTitle_GotFocus);
			// 
			// txtAddress1
			// 
			this.txtAddress1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtAddress1.Enabled = false;
			this.txtAddress1.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtAddress1.Location = new System.Drawing.Point(150, 174);
			this.txtAddress1.MaxLength = 50;
			this.txtAddress1.Name = "txtAddress1";
			this.txtAddress1.Size = new System.Drawing.Size(176, 21);
			this.txtAddress1.TabIndex = 40;
			this.txtAddress1.Text = "";
			this.txtAddress1.LostFocus += new System.EventHandler(this.txtAddress1_LostFocus);
			this.txtAddress1.GotFocus += new System.EventHandler(this.txtAddress1_GotFocus);
			// 
			// txtAddress2
			// 
			this.txtAddress2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtAddress2.Enabled = false;
			this.txtAddress2.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtAddress2.Location = new System.Drawing.Point(150, 196);
			this.txtAddress2.MaxLength = 50;
			this.txtAddress2.Name = "txtAddress2";
			this.txtAddress2.Size = new System.Drawing.Size(176, 21);
			this.txtAddress2.TabIndex = 41;
			this.txtAddress2.Text = "";
			this.txtAddress2.LostFocus += new System.EventHandler(this.txtAddress2_LostFocus);
			this.txtAddress2.GotFocus += new System.EventHandler(this.txtAddress2_GotFocus);
			// 
			// txtEMail
			// 
			this.txtEMail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtEMail.Enabled = false;
			this.txtEMail.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtEMail.Location = new System.Drawing.Point(150, 218);
			this.txtEMail.MaxLength = 50;
			this.txtEMail.Name = "txtEMail";
			this.txtEMail.Size = new System.Drawing.Size(176, 21);
			this.txtEMail.TabIndex = 42;
			this.txtEMail.Text = "";
			this.txtEMail.LostFocus += new System.EventHandler(this.txtEMail_LostFocus);
			this.txtEMail.GotFocus += new System.EventHandler(this.txtEMail_GotFocus);
			// 
			// txtTelephoneWork
			// 
			this.txtTelephoneWork.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtTelephoneWork.Enabled = false;
			this.txtTelephoneWork.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtTelephoneWork.Location = new System.Drawing.Point(150, 240);
			this.txtTelephoneWork.MaxLength = 50;
			this.txtTelephoneWork.Name = "txtTelephoneWork";
			this.txtTelephoneWork.Size = new System.Drawing.Size(176, 21);
			this.txtTelephoneWork.TabIndex = 43;
			this.txtTelephoneWork.Text = "";
			this.txtTelephoneWork.LostFocus += new System.EventHandler(this.txtTelephoneWork_LostFocus);
			this.txtTelephoneWork.GotFocus += new System.EventHandler(this.txtTelephoneWork_GotFocus);
			// 
			// txtTelephoneCell
			// 
			this.txtTelephoneCell.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtTelephoneCell.Enabled = false;
			this.txtTelephoneCell.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtTelephoneCell.Location = new System.Drawing.Point(150, 262);
			this.txtTelephoneCell.MaxLength = 50;
			this.txtTelephoneCell.Name = "txtTelephoneCell";
			this.txtTelephoneCell.Size = new System.Drawing.Size(176, 21);
			this.txtTelephoneCell.TabIndex = 44;
			this.txtTelephoneCell.Text = "";
			this.txtTelephoneCell.LostFocus += new System.EventHandler(this.txtTelephoneCell_LostFocus);
			this.txtTelephoneCell.GotFocus += new System.EventHandler(this.txtTelephoneCell_GotFocus);
			// 
			// txtPager
			// 
			this.txtPager.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtPager.Enabled = false;
			this.txtPager.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtPager.Location = new System.Drawing.Point(150, 284);
			this.txtPager.MaxLength = 50;
			this.txtPager.Name = "txtPager";
			this.txtPager.Size = new System.Drawing.Size(176, 21);
			this.txtPager.TabIndex = 45;
			this.txtPager.Text = "";
			this.txtPager.LostFocus += new System.EventHandler(this.txtPager_LostFocus);
			this.txtPager.GotFocus += new System.EventHandler(this.txtPager_GotFocus);
			// 
			// txtTelephoneHome
			// 
			this.txtTelephoneHome.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtTelephoneHome.Enabled = false;
			this.txtTelephoneHome.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtTelephoneHome.Location = new System.Drawing.Point(150, 306);
			this.txtTelephoneHome.MaxLength = 50;
			this.txtTelephoneHome.Name = "txtTelephoneHome";
			this.txtTelephoneHome.Size = new System.Drawing.Size(176, 21);
			this.txtTelephoneHome.TabIndex = 46;
			this.txtTelephoneHome.Text = "";
			this.txtTelephoneHome.LostFocus += new System.EventHandler(this.txtTelephoneHome_LostFocus);
			this.txtTelephoneHome.GotFocus += new System.EventHandler(this.txtTelephoneHome_GotFocus);
			// 
			// txtFax
			// 
			this.txtFax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtFax.Enabled = false;
			this.txtFax.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtFax.Location = new System.Drawing.Point(150, 328);
			this.txtFax.MaxLength = 50;
			this.txtFax.Name = "txtFax";
			this.txtFax.Size = new System.Drawing.Size(176, 21);
			this.txtFax.TabIndex = 47;
			this.txtFax.Text = "";
			this.txtFax.LostFocus += new System.EventHandler(this.txtFax_LostFocus);
			this.txtFax.GotFocus += new System.EventHandler(this.txtFax_GotFocus);
			// 
			// CompanyUser
			// 
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.Controls.Add(this.groupBox1);
			this.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.Name = "CompanyUser";
			this.Size = new System.Drawing.Size(358, 384);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Public Properties and Methods 

		public User User
		{
			get{return GetUserDetail();}
			set{SetUserDetail(value);}
		}

		public void RefreshUserDetail()
		{
			txtAddress1.Text = "";
			txtAddress2.Text = "";
			txtEMail.Text = "";
			txtFax.Text = "";
			txtFirstName.Text = "";
			txtLastName.Text = "";
			txtLoginName.Text = "";
			txtLoginName.Tag = int.MinValue;
			txtPassword.Text = "";
			txtShortName.Text = "";
			txtTelephoneHome.Text = "";
			txtTelephoneCell.Text = "";
			txtPager.Text = "";
			txtTelephoneWork.Text = "";
			txtTitle.Text = "";		
			txtMailingAddress.Text = "";

		}
		
		//Bind Permission
		
		private StatusBar _statusBar = null;
		public StatusBar ParentStatusBar
		{
			set{_statusBar = value;}
		}
		
		public int SaveCompanyUser()
		{
			int result = int.MinValue;

			if (txtFirstName.Text == "")
			{
				//Nirvana.Admin.Utility.Common.ResetStatusPanel(_statusBar);
				//Nirvana.Admin.Utility.Common.SetStatusPanel(_statusBar, "Please enter First name!");
				txtFirstName.Focus();
			}
			else if (txtLastName.Text == "")
			{
				//Nirvana.Admin.Utility.Common.ResetStatusPanel(_statusBar);
				//Nirvana.Admin.Utility.Common.SetStatusPanel(_statusBar, "Please enter Last name!");
				txtLastName.Focus();
			}
			else if (txtLoginName.Text == "")
			{
				//Nirvana.Admin.Utility.Common.ResetStatusPanel(_statusBar);
				//Nirvana.Admin.Utility.Common.SetStatusPanel(_statusBar, "Stored!");
				txtLoginName.Focus();
			}
			else if (txtPassword.Text == "")
			{
				//Nirvana.Admin.Utility.Common.ResetStatusPanel(_statusBar);
				//Nirvana.Admin.Utility.Common.SetStatusPanel(_statusBar, "Please enter Password name!");
				txtPassword.Focus();
			}
			else if (txtShortName.Text == "")
			{
				//Nirvana.Admin.Utility.Common.ResetStatusPanel(_statusBar);
				//Nirvana.Admin.Utility.Common.SetStatusPanel(_statusBar, "Please enter Short name!");
				txtShortName.Focus();
			}
			else if (txtTitle.Text == "")
			{
				//Nirvana.Admin.Utility.Common.ResetStatusPanel(_statusBar);
				//Nirvana.Admin.Utility.Common.SetStatusPanel(_statusBar, "Please enter Title!");
				txtTitle.Focus();
			}
			else if (txtMailingAddress.Text == "")
			{
				//Nirvana.Admin.Utility.Common.ResetStatusPanel(_statusBar);
				//Nirvana.Admin.Utility.Common.SetStatusPanel(_statusBar, "Please enter Mailing Address!");
				txtMailingAddress.Focus();
			}
			else if (txtAddress1.Text == "")
			{
				//Nirvana.Admin.Utility.Common.ResetStatusPanel(_statusBar);
				//Nirvana.Admin.Utility.Common.SetStatusPanel(_statusBar, "Please enter Address1!");
				txtAddress1.Focus();
			}
			else if (txtAddress2.Text == "")
			{
				//Nirvana.Admin.Utility.Common.ResetStatusPanel(_statusBar);
				//Nirvana.Admin.Utility.Common.SetStatusPanel(_statusBar, "Please enter Address2!");
				txtAddress2.Focus();
			}
			else if (txtEMail.Text == "")
			{
				//Nirvana.Admin.Utility.Common.ResetStatusPanel(_statusBar);
				//Nirvana.Admin.Utility.Common.SetStatusPanel(_statusBar, "Please enter Email!");
				txtEMail.Focus();
			}
			else if (txtTelephoneWork.Text == "")
			{
				//Nirvana.Admin.Utility.Common.ResetStatusPanel(_statusBar);
				//Nirvana.Admin.Utility.Common.SetStatusPanel(_statusBar, "Please enter Work Telephone!");
				txtTelephoneWork.Focus();
			}
			else if (txtTelephoneCell.Text == "")
			{
				//Nirvana.Admin.Utility.Common.ResetStatusPanel(_statusBar);
				//Nirvana.Admin.Utility.Common.SetStatusPanel(_statusBar, "Please enter Cell no!");
				txtTelephoneCell.Focus();
			}
			else if (txtPager.Text == "")
			{
				//Nirvana.Admin.Utility.Common.ResetStatusPanel(_statusBar);
				//Nirvana.Admin.Utility.Common.SetStatusPanel(_statusBar, "Please enter Pager no!");
				txtPager.Focus();
			}
			else if (txtTelephoneHome.Text == "")
			{
				//Nirvana.Admin.Utility.Common.ResetStatusPanel(_statusBar);
				//Nirvana.Admin.Utility.Common.SetStatusPanel(_statusBar, "Please enter Home Telephone!");
				txtTelephoneHome.Focus();
			}
			else if (txtFax.Text == "")
			{
				//Nirvana.Admin.Utility.Common.ResetStatusPanel(_statusBar);
				//Nirvana.Admin.Utility.Common.SetStatusPanel(_statusBar, "Please enter Fax no!");
				txtFax.Focus();
			}
//			else if(int.Parse(cmbTradingPermission.SelectedValue.ToString()) == int.MinValue)
//			{
//				//Nirvana.Admin.Utility.Common.ResetStatusPanel(_statusBar);
//				//Nirvana.Admin.Utility.Common.SetStatusPanel(_statusBar, "Please select Trading Permission!");
//				cmbTradingPermission.Focus();
//			}
			else
			{
				User user = new User();				
				//user.UserID
				user.Address1 = txtAddress1.Text.Trim();
				user.Address2 = txtAddress2.Text.Trim();
				user.EMail = txtEMail.Text.Trim();
				user.Fax = txtFax.Text.Trim();
				user.FirstName = txtFirstName.Text.Trim();
				user.LastName = txtLastName.Text.Trim();
				user.LoginName = txtLoginName.Text.Trim();
				
				user.Password = txtPassword.Text.Trim();
				user.ShortName = txtShortName.Text.Trim();
				user.TelephoneHome = txtTelephoneHome.Text.Trim();
				user.TelephoneMobile = txtTelephoneCell.Text.Trim();
				user.TelephoneWork = txtTelephoneWork.Text.Trim();
				user.Title = txtTitle.Text.Trim();
				user.MailingAddress = txtMailingAddress.Text.Trim();
				
				
				result = 1;
			}			
			return result;
			
			//result = UserManager.SaveCompanyUser(_companyID, user);

		
			
		}
		
		
		#region savecompany user with goto function
//		public static int SaveCompanyUser()
//		{
//			int result = int.MinValue;

//			User user = new User();
//			user.UserID = int.Parse(txtLoginName.Tag.ToString());
//			if(txtFirstName.Text.Trim().Length == 0)
//			{
//				//Message
//				goto ExitFunction;
//			}
//			user.FirstName = txtFirstName.Text;
//			user.LastName = txtLastName.Text;
//			if(txtLoginName.Text.Trim().Length == 0)
//			{
//				//Message
//				goto ExitFunction;
//			}
//			user.LoginName = txtLoginName.Text;
//			if(txtPassword.Text.Trim().Length == 0)
//			{
//				//Message
//				goto ExitFunction;
//			}
//			user.Password = txtPassword.Text;
//			user.ShortName = txtShortName.Text;
//			user.Title = txtTitle.Text;
//			user.Address1 = txtAddress1.Text;
//			user.Address2 = txtAddress2.Text;
//			if(txtEMail.Text.Trim().Length == 0)
//			{
//				//Message
//				goto ExitFunction;
//			}
//			user.EMail = txtEMail.Text;
//			user.TelephoneWork = txtTelephoneWork.Text;			
//			user.TelephoneMobile = txtTelephoneCell.Text;
//			user.TelephonePager = txtPager.Text;
//			user.TelephoneHome = txtTelephoneHome.Text;
//			user.Fax = txtFax.Text;
//			
//			result = UserManager.SaveCompanyUser(_companyID, user);

//		
//			return result;
//		}
		#endregion
		
		#endregion

		#region Private methods

		private void SetUserDetail(User user)
		{
			
			txtAddress1.Text = user.Address1;
			txtAddress2.Text = user.Address2;
			txtEMail.Text = user.EMail;
			txtFax.Text = user.Fax;
			txtFirstName.Text = user.FirstName;
			txtLastName.Text = user.LastName;
			txtLoginName.Text = user.LoginName;
			
			txtPassword.Text = user.Password;
			txtShortName.Text = user.ShortName;
			txtTelephoneHome.Text = user.TelephoneHome;
			txtTelephoneCell.Text = user.TelephoneMobile;
			txtPager.Text = user.TelephonePager;
			txtTelephoneWork.Text = user.TelephoneWork;
			txtTitle.Text = user.Title;						
			txtMailingAddress.Text = user.MailingAddress;
		}

		private User GetUserDetail()
		{
			User user = null;
			
			errorProvider1.SetError(txtFirstName, "");
			errorProvider1.SetError(txtLastName, "");
			errorProvider1.SetError(txtLoginName, "");
			errorProvider1.SetError(txtPassword, "");
			errorProvider1.SetError(txtShortName, "");
			errorProvider1.SetError(txtTitle, "");
			errorProvider1.SetError(txtMailingAddress, "");
			errorProvider1.SetError(txtAddress1, "");
			errorProvider1.SetError(txtEMail, "");
			errorProvider1.SetError(txtTelephoneWork, "");
			errorProvider1.SetError(txtTelephoneCell, "");
			if (txtFirstName.Text == "")
			{
				errorProvider1.SetError(txtFirstName, "Please enter Address 1 in details!");
				txtFirstName.Focus();
			}
			else if (txtLastName.Text == "")
			{
				errorProvider1.SetError(txtLastName, "Please enter Address 1 in details!");
				txtLastName.Focus();
			}
			else if (txtLoginName.Text == "")
			{
				errorProvider1.SetError(txtLoginName, "Please enter Login Name!");
				txtLoginName.Focus();
			}
			else if (txtPassword.Text == "")
			{
				errorProvider1.SetError(txtPassword, "Please enter Password!");
				txtPassword.Focus();
			}
			else if (txtShortName.Text == "")
			{
				errorProvider1.SetError(txtShortName, "Please enter Short name!");
				txtShortName.Focus();
			}
			else if (txtTitle.Text == "")
			{
				errorProvider1.SetError(txtTitle, "Please enter Title!");
				txtTitle.Focus();
			}
			else if (txtMailingAddress.Text == "")
			{
				errorProvider1.SetError(txtMailingAddress, "Please enter Mailing Address!");
				txtMailingAddress.Focus();
			}
			else if (txtAddress1.Text == "")
			{
				errorProvider1.SetError(txtAddress1, "Please enter Address1");
				txtAddress1.Focus();
			}
			
			else if (txtEMail.Text == "")
			{
				errorProvider1.SetError(txtEMail, "Please enter Email!");
				txtEMail.Focus();
			}
			else if (txtTelephoneWork.Text == "")
			{
				errorProvider1.SetError(txtTelephoneWork, "Please enter Work Telephone!");
				txtTelephoneWork.Focus();
			}
			else if (txtTelephoneCell.Text == "")
			{
				errorProvider1.SetError(txtTelephoneCell, "Please enter Cell no!");
				txtTelephoneCell.Focus();
			}
			
//			else if(int.Parse(cmbTradingPermission.SelectedValue.ToString()) == int.MinValue)
//			{
//				//Nirvana.Admin.Utility.Common.ResetStatusPanel(_statusBar);
//				//Nirvana.Admin.Utility.Common.SetStatusPanel(_statusBar, "Please select Trading Permission!");
//				cmbTradingPermission.Focus();
//			}
			else
			{
				user = new User();
				user.Address1 = txtAddress1.Text;
				user.Address2 = txtAddress2.Text;
				user.EMail = txtEMail.Text;
				user.Fax = txtFax.Text;
				user.FirstName = txtFirstName.Text;
				user.LastName = txtLastName.Text;
				user.LoginName = txtLoginName.Text;
				user.Password = txtPassword.Text;
				user.ShortName = txtShortName.Text;
				user.TelephoneHome = txtTelephoneHome.Text;
				user.TelephoneMobile = txtTelephoneCell.Text;
				user.TelephonePager = txtPager.Text;
				user.TelephoneWork = txtTelephoneWork.Text;
				user.Title = txtTitle.Text;
				user.MailingAddress = txtMailingAddress.Text;
			}
			return user;
		}

		#endregion

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
		private void txtLoginName_GotFocus(object sender, System.EventArgs e)
		{
			txtLoginName.BackColor = Color.LemonChiffon;
		}
		private void txtLoginName_LostFocus(object sender, System.EventArgs e)
		{
			txtLoginName.BackColor = Color.White;
		}
		private void txtPassword_GotFocus(object sender, System.EventArgs e)
		{
			txtPassword.BackColor = Color.LemonChiffon;
		}
		private void txtPassword_LostFocus(object sender, System.EventArgs e)
		{
			txtPassword.BackColor = Color.White;
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
		private void txtMailingAddress_GotFocus(object sender, System.EventArgs e)
		{
			txtMailingAddress.BackColor = Color.LemonChiffon;
		}
		private void txtMailingAddress_LostFocus(object sender, System.EventArgs e)
		{
			txtMailingAddress.BackColor = Color.White;
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
		private void txtTelephoneHome_GotFocus(object sender, System.EventArgs e)
		{
			txtTelephoneHome.BackColor = Color.LemonChiffon;
		}
		private void txtTelephoneHome_LostFocus(object sender, System.EventArgs e)
		{
			txtTelephoneHome.BackColor = Color.White;
		}
		private void txtFax_GotFocus(object sender, System.EventArgs e)
		{
			txtFax.BackColor = Color.LemonChiffon;
		}
		private void txtFax_LostFocus(object sender, System.EventArgs e)
		{
			txtFax.BackColor = Color.White;
		}
		private void cmbTradingPermission_GotFocus(object sender, System.EventArgs e)
		{
			cmbTradingPermission.BackColor = Color.LemonChiffon;
		}
		private void cmbTradingPermission_LostFocus(object sender, System.EventArgs e)
		{
			cmbTradingPermission.BackColor = Color.White;
		}
		#endregion
	}
}
