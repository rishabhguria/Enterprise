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
		private System.Windows.Forms.TextBox txtFirstName;
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
		private System.Windows.Forms.ComboBox cmbTradingPermission;
		private int _companyID = int.MinValue;

		#endregion
		private System.Windows.Forms.Label MailingAddress;
		private System.Windows.Forms.TextBox txtMailingAddress;

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
			this.txtFirstName = new System.Windows.Forms.TextBox();
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
			this.cmbTradingPermission = new System.Windows.Forms.ComboBox();
			this.MailingAddress = new System.Windows.Forms.Label();
			this.txtMailingAddress = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label1.Location = new System.Drawing.Point(2, 6);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(128, 22);
			this.label1.TabIndex = 0;
			this.label1.Text = "First Name";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label2.Location = new System.Drawing.Point(2, 28);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(128, 22);
			this.label2.TabIndex = 0;
			this.label2.Text = "Last Name";
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label3.Location = new System.Drawing.Point(2, 50);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(128, 22);
			this.label3.TabIndex = 0;
			this.label3.Text = "Login Name";
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label4.Location = new System.Drawing.Point(2, 72);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(128, 22);
			this.label4.TabIndex = 0;
			this.label4.Text = "Password";
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label5.Location = new System.Drawing.Point(2, 94);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(128, 22);
			this.label5.TabIndex = 0;
			this.label5.Text = "Short Name";
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label6.Location = new System.Drawing.Point(2, 116);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(128, 22);
			this.label6.TabIndex = 0;
			this.label6.Text = "Title";
			// 
			// label7
			// 
			this.label7.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label7.Location = new System.Drawing.Point(2, 160);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(128, 22);
			this.label7.TabIndex = 0;
			this.label7.Text = "Address1";
			// 
			// label8
			// 
			this.label8.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label8.Location = new System.Drawing.Point(2, 182);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(128, 22);
			this.label8.TabIndex = 0;
			this.label8.Text = "Address2";
			// 
			// label9
			// 
			this.label9.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label9.Location = new System.Drawing.Point(2, 204);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(128, 22);
			this.label9.TabIndex = 0;
			this.label9.Text = "E-Mail";
			// 
			// label10
			// 
			this.label10.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label10.Location = new System.Drawing.Point(2, 226);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(128, 22);
			this.label10.TabIndex = 0;
			this.label10.Text = "Work Telephone #";
			// 
			// label11
			// 
			this.label11.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label11.Location = new System.Drawing.Point(2, 248);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(128, 22);
			this.label11.TabIndex = 0;
			this.label11.Text = "Cell #";
			// 
			// label12
			// 
			this.label12.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label12.Location = new System.Drawing.Point(2, 270);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(128, 22);
			this.label12.TabIndex = 0;
			this.label12.Text = "Pager #";
			// 
			// label13
			// 
			this.label13.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label13.Location = new System.Drawing.Point(2, 292);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(128, 22);
			this.label13.TabIndex = 0;
			this.label13.Text = "Home #";
			// 
			// label14
			// 
			this.label14.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label14.Location = new System.Drawing.Point(2, 314);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(128, 22);
			this.label14.TabIndex = 0;
			this.label14.Text = "Fax #";
			// 
			// label15
			// 
			this.label15.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label15.Location = new System.Drawing.Point(2, 336);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(128, 22);
			this.label15.TabIndex = 0;
			this.label15.Text = "Trading Permission";
			// 
			// txtFirstName
			// 
			this.txtFirstName.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtFirstName.Location = new System.Drawing.Point(130, 2);
			this.txtFirstName.MaxLength = 50;
			this.txtFirstName.Name = "txtFirstName";
			this.txtFirstName.Size = new System.Drawing.Size(176, 21);
			this.txtFirstName.TabIndex = 1;
			this.txtFirstName.Text = "";
			// 
			// txtLastName
			// 
			this.txtLastName.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtLastName.Location = new System.Drawing.Point(130, 24);
			this.txtLastName.MaxLength = 50;
			this.txtLastName.Name = "txtLastName";
			this.txtLastName.Size = new System.Drawing.Size(176, 21);
			this.txtLastName.TabIndex = 1;
			this.txtLastName.Text = "";
			// 
			// txtLoginName
			// 
			this.txtLoginName.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtLoginName.Location = new System.Drawing.Point(130, 46);
			this.txtLoginName.MaxLength = 50;
			this.txtLoginName.Name = "txtLoginName";
			this.txtLoginName.Size = new System.Drawing.Size(176, 21);
			this.txtLoginName.TabIndex = 1;
			this.txtLoginName.Text = "";
			// 
			// txtPassword
			// 
			this.txtPassword.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtPassword.Location = new System.Drawing.Point(130, 68);
			this.txtPassword.MaxLength = 50;
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.Size = new System.Drawing.Size(176, 21);
			this.txtPassword.TabIndex = 1;
			this.txtPassword.Text = "";
			// 
			// txtShortName
			// 
			this.txtShortName.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtShortName.Location = new System.Drawing.Point(130, 90);
			this.txtShortName.MaxLength = 50;
			this.txtShortName.Name = "txtShortName";
			this.txtShortName.Size = new System.Drawing.Size(176, 21);
			this.txtShortName.TabIndex = 1;
			this.txtShortName.Text = "";
			// 
			// txtTitle
			// 
			this.txtTitle.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtTitle.Location = new System.Drawing.Point(130, 112);
			this.txtTitle.MaxLength = 50;
			this.txtTitle.Name = "txtTitle";
			this.txtTitle.Size = new System.Drawing.Size(176, 21);
			this.txtTitle.TabIndex = 1;
			this.txtTitle.Text = "";
			// 
			// txtAddress1
			// 
			this.txtAddress1.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtAddress1.Location = new System.Drawing.Point(130, 156);
			this.txtAddress1.MaxLength = 50;
			this.txtAddress1.Name = "txtAddress1";
			this.txtAddress1.Size = new System.Drawing.Size(176, 21);
			this.txtAddress1.TabIndex = 1;
			this.txtAddress1.Text = "";
			// 
			// txtAddress2
			// 
			this.txtAddress2.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtAddress2.Location = new System.Drawing.Point(130, 178);
			this.txtAddress2.MaxLength = 50;
			this.txtAddress2.Name = "txtAddress2";
			this.txtAddress2.Size = new System.Drawing.Size(176, 21);
			this.txtAddress2.TabIndex = 1;
			this.txtAddress2.Text = "";
			// 
			// txtEMail
			// 
			this.txtEMail.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtEMail.Location = new System.Drawing.Point(130, 200);
			this.txtEMail.MaxLength = 50;
			this.txtEMail.Name = "txtEMail";
			this.txtEMail.Size = new System.Drawing.Size(176, 21);
			this.txtEMail.TabIndex = 1;
			this.txtEMail.Text = "";
			// 
			// txtTelephoneWork
			// 
			this.txtTelephoneWork.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtTelephoneWork.Location = new System.Drawing.Point(130, 222);
			this.txtTelephoneWork.MaxLength = 50;
			this.txtTelephoneWork.Name = "txtTelephoneWork";
			this.txtTelephoneWork.Size = new System.Drawing.Size(176, 21);
			this.txtTelephoneWork.TabIndex = 1;
			this.txtTelephoneWork.Text = "";
			// 
			// txtTelephoneCell
			// 
			this.txtTelephoneCell.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtTelephoneCell.Location = new System.Drawing.Point(130, 244);
			this.txtTelephoneCell.MaxLength = 50;
			this.txtTelephoneCell.Name = "txtTelephoneCell";
			this.txtTelephoneCell.Size = new System.Drawing.Size(176, 21);
			this.txtTelephoneCell.TabIndex = 1;
			this.txtTelephoneCell.Text = "";
			// 
			// txtPager
			// 
			this.txtPager.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtPager.Location = new System.Drawing.Point(130, 266);
			this.txtPager.MaxLength = 50;
			this.txtPager.Name = "txtPager";
			this.txtPager.Size = new System.Drawing.Size(176, 21);
			this.txtPager.TabIndex = 1;
			this.txtPager.Text = "";
			// 
			// txtTelephoneHome
			// 
			this.txtTelephoneHome.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtTelephoneHome.Location = new System.Drawing.Point(130, 288);
			this.txtTelephoneHome.MaxLength = 50;
			this.txtTelephoneHome.Name = "txtTelephoneHome";
			this.txtTelephoneHome.Size = new System.Drawing.Size(176, 21);
			this.txtTelephoneHome.TabIndex = 1;
			this.txtTelephoneHome.Text = "";
			// 
			// txtFax
			// 
			this.txtFax.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtFax.Location = new System.Drawing.Point(130, 310);
			this.txtFax.MaxLength = 50;
			this.txtFax.Name = "txtFax";
			this.txtFax.Size = new System.Drawing.Size(176, 21);
			this.txtFax.TabIndex = 1;
			this.txtFax.Text = "";
			// 
			// cmbTradingPermission
			// 
			this.cmbTradingPermission.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.cmbTradingPermission.Location = new System.Drawing.Point(130, 330);
			this.cmbTradingPermission.Name = "cmbTradingPermission";
			this.cmbTradingPermission.Size = new System.Drawing.Size(176, 21);
			this.cmbTradingPermission.TabIndex = 2;
			// 
			// MailingAddress
			// 
			this.MailingAddress.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.MailingAddress.Location = new System.Drawing.Point(2, 138);
			this.MailingAddress.Name = "MailingAddress";
			this.MailingAddress.Size = new System.Drawing.Size(128, 22);
			this.MailingAddress.TabIndex = 3;
			this.MailingAddress.Text = "Mailing Address";
			// 
			// txtMailingAddress
			// 
			this.txtMailingAddress.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtMailingAddress.Location = new System.Drawing.Point(130, 134);
			this.txtMailingAddress.MaxLength = 50;
			this.txtMailingAddress.Name = "txtMailingAddress";
			this.txtMailingAddress.Size = new System.Drawing.Size(176, 21);
			this.txtMailingAddress.TabIndex = 4;
			this.txtMailingAddress.Text = "";
			// 
			// CompanyUser
			// 
			this.Controls.Add(this.txtMailingAddress);
			this.Controls.Add(this.MailingAddress);
			this.Controls.Add(this.cmbTradingPermission);
			this.Controls.Add(this.txtFirstName);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.label13);
			this.Controls.Add(this.label14);
			this.Controls.Add(this.label15);
			this.Controls.Add(this.txtLastName);
			this.Controls.Add(this.txtLoginName);
			this.Controls.Add(this.txtPassword);
			this.Controls.Add(this.txtShortName);
			this.Controls.Add(this.txtTitle);
			this.Controls.Add(this.txtAddress1);
			this.Controls.Add(this.txtAddress2);
			this.Controls.Add(this.txtEMail);
			this.Controls.Add(this.txtTelephoneWork);
			this.Controls.Add(this.txtTelephoneCell);
			this.Controls.Add(this.txtPager);
			this.Controls.Add(this.txtTelephoneHome);
			this.Controls.Add(this.txtFax);
			this.Name = "CompanyUser";
			this.Size = new System.Drawing.Size(310, 356);
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
		
		public int SaveCompanyUser()
		{
			int result = int.MinValue;

			if (txtFirstName.Text == "")
			{
				//_statusBar.Text = "Please enter First name!";
				txtFirstName.Focus();
			}
			if (txtLastName.Text == "")
			{
				//_statusBar.Text = "Please enter Last name!";
				txtLastName.Focus();
			}
			if (txtLoginName.Text == "")
			{
				//_statusBar.Text = "Please enter Login name!";
				txtLoginName.Focus();
			}
			if (txtPassword.Text == "")
			{
				//_statusBar.Text = "Please enter Password name!";
				txtPassword.Focus();
			}
			if (txtShortName.Text == "")
			{
				//_statusBar.Text = "Please enter Short name!";
				txtShortName.Focus();
			}
			if (txtTitle.Text == "")
			{
				//_statusBar.Text = "Please enter Title!";
				txtTitle.Focus();
			}
			if (txtMailingAddress.Text == "")
			{
				//_statusBar.Text = "Please enter Mailing Address!";
				txtMailingAddress.Focus();
			}
			if (txtAddress1.Text == "")
			{
				//_statusBar.Text = "Please enter Address1!";
				txtAddress1.Focus();
			}
			if (txtAddress2.Text == "")
			{
				//_statusBar.Text = "Please enter Address2!";
				txtAddress2.Focus();
			}
			if (txtEMail.Text == "")
			{
				//_statusBar.Text = "Please enter Email!";
				txtEMail.Focus();
			}
			if (txtTelephoneWork.Text == "")
			{
				//_statusBar.Text = "Please enter Work Telephone!";
				txtTelephoneWork.Focus();
			}
			if (txtTelephoneCell.Text == "")
			{
				//_statusBar.Text = "Please enter Cell no!";
				txtTelephoneCell.Focus();
			}
			if (txtPager.Text == "")
			{
				//_statusBar.Text = "Please enter Pager no!";
				txtPager.Focus();
			}
			if (txtTelephoneHome.Text == "")
			{
				//_statusBar.Text = "Please enter Home Telephone!";
				txtTelephoneHome.Focus();
			}
			if (txtFax.Text == "")
			{
				//_statusBar.Text = "Please enter Fax no!";
				txtFax.Focus();
			}
			else if(int.Parse(cmbTradingPermission.SelectedValue.ToString()) == int.MinValue)
			{
				//_statusBar.Text = "Please select Trading Permission!";
				cmbTradingPermission.Focus();
			}
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
			User user = new User();
			
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
			
			return user;
		}

		#endregion
	}
}
