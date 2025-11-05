#region Using Namespaces

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Nirvana.Admin.BLL;
using Nirvana.Admin.Utility;
using Nirvana.Global;

using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Sinks;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tracing;

#endregion

namespace Nirvana.Admin
{
	/// <summary>
	/// Summary description for Login.
	/// </summary>
	public class Login : System.Windows.Forms.Form
	{
		private const string FORM_NAME = "Login : ";
		#region Private and Protected members

		private System.Windows.Forms.Label lblPassword;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.Button btnLogin;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Label lblLogin;
		private System.Windows.Forms.TextBox txtLogin;
		private System.Windows.Forms.ErrorProvider errorProvider1;	

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#endregion
		
		/// <summary>
		/// Form Constructor.
		/// </summary>
		public Login()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			btnLogin.Focus();
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Login));
			this.lblLogin = new System.Windows.Forms.Label();
			this.lblPassword = new System.Windows.Forms.Label();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.txtLogin = new System.Windows.Forms.TextBox();
			this.btnLogin = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
			this.SuspendLayout();
			// 
			// lblLogin
			// 
			this.lblLogin.BackColor = System.Drawing.Color.White;
			this.lblLogin.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblLogin.Location = new System.Drawing.Point(146, 168);
			this.lblLogin.Name = "lblLogin";
			this.lblLogin.Size = new System.Drawing.Size(80, 14);
			this.lblLogin.TabIndex = 0;
			this.lblLogin.Text = "User Name";
			// 
			// lblPassword
			// 
			this.lblPassword.BackColor = System.Drawing.Color.White;
			this.lblPassword.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblPassword.Location = new System.Drawing.Point(146, 196);
			this.lblPassword.Name = "lblPassword";
			this.lblPassword.Size = new System.Drawing.Size(76, 14);
			this.lblPassword.TabIndex = 1;
			this.lblPassword.Text = "Password";
			// 
			// txtPassword
			// 
			this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtPassword.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtPassword.Location = new System.Drawing.Point(232, 194);
			this.txtPassword.MaxLength = 20;
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.PasswordChar = '#';
			this.txtPassword.Size = new System.Drawing.Size(96, 21);
			this.txtPassword.TabIndex = 2;
			this.txtPassword.Text = "password";
			this.txtPassword.LostFocus += new System.EventHandler(this.txtPassword_LostFocus);
			this.txtPassword.GotFocus += new System.EventHandler(this.txtPassword_GotFocus);
			this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
			// 
			// txtLogin
			// 
			this.txtLogin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtLogin.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtLogin.Location = new System.Drawing.Point(232, 166);
			this.txtLogin.MaxLength = 20;
			this.txtLogin.Name = "txtLogin";
			this.txtLogin.Size = new System.Drawing.Size(96, 21);
			this.txtLogin.TabIndex = 1;
			this.txtLogin.Text = "user";
			this.txtLogin.LostFocus += new System.EventHandler(this.txtLogin_LostFocus);
			this.txtLogin.GotFocus += new System.EventHandler(this.txtLogin_GotFocus);
			// 
			// btnLogin
			// 
			this.btnLogin.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(204)), ((System.Byte)(102)));
			this.btnLogin.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnLogin.BackgroundImage")));
			this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnLogin.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.btnLogin.Location = new System.Drawing.Point(158, 238);
			this.btnLogin.Name = "btnLogin";
			this.btnLogin.Size = new System.Drawing.Size(78, 26);
			this.btnLogin.TabIndex = 3;
			this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
			// 
			// btnClose
			// 
			this.btnClose.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(51)), ((System.Byte)(51)));
			this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnClose.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.btnClose.Location = new System.Drawing.Point(242, 238);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(78, 26);
			this.btnClose.TabIndex = 4;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// errorProvider1
			// 
			this.errorProvider1.ContainerControl = this;
			// 
			// Login
			// 
			this.AcceptButton = this.btnLogin;
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(336, 275);
			this.ControlBox = false;
			this.Controls.Add(this.txtLogin);
			this.Controls.Add(this.txtPassword);
			this.Controls.Add(this.btnLogin);
			this.Controls.Add(this.lblLogin);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.lblPassword);
			this.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Login";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Nirvana: Login";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
				if(!Global.Common.SigletonCheck())
					return;

				Application.Run(new Login());
			
			
		}


		private void btnClose_Click(object sender, System.EventArgs e)
		{
			Application.Exit();
		}
		
		private void btnLogin_Click(object sender, System.EventArgs e)
		{
			Permissions userPermissions = null;
			try
			{				
//				if(int.Parse(txtPassword.Text.Trim().Length.ToString()) < 4)
//				{
//					//errorProvider1.SetError(txtPassword, "Please enter password having at least four characters !");
//					MessageBox.Show(this, "Please enter password having at least four characters.", "Login Alert", MessageBoxButtons.RetryCancel);
//					txtLogin.Focus();				
//				}
//				else
//				{
					userPermissions = UserManager.ValidateLogin(txtLogin.Text, txtPassword.Text);
					if(userPermissions != null)
					{
						SuperAdminMain mainApplication = new SuperAdminMain();
						mainApplication.UserPermissions = userPermissions;
						mainApplication.Show();
						this.Hide();
						//throw new Exception("Test Exception");
					}
					else
					{
						MessageBox.Show(this, "Please enter valid Username and Password.", "Login Alert", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
						txtLogin.Focus(); 							
					}
					//				throw new Exception("Test");
//				}
			}
			catch(Exception ex)
			{
				throw(ex);
				//string formattedInfo = ex.StackTrace.ToString();
				string formattedInfo = ex.Message + " " + ex.InnerException + " " + ex.StackTrace.ToString() ;
				Logger.Write(formattedInfo, Nirvana.Admin.Utility.Common.LOG_CATEGORY_EXCEPTION, 1, 1, Severity.Error, 
							FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler();
				appMessageExceptionHandler.HandleException(new Exception(Nirvana.Admin.Utility.Common.ERROR_STATEMENT), Nirvana.Admin.Utility.Common.POLICY_GLOBAL, System.Guid.NewGuid());
			}
			finally
			{
				#region LogEntry

				LogEntry logEntry = new LogEntry("btnLogin_Click", 
												Nirvana.Admin.Utility.Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
												FORM_NAME + "btnLogin_Click"); 
				Logger.Write(logEntry); 

				#endregion
			}
		}
		#region Focus Colors
		private void txtLogin_GotFocus(object sender, System.EventArgs e)
		{
			txtLogin.BackColor = Color.LemonChiffon;
		}
		private void txtLogin_LostFocus(object sender, System.EventArgs e)
		{
			txtLogin.BackColor = Color.White;
		}
		private void txtPassword_GotFocus(object sender, System.EventArgs e)
		{
			txtPassword.BackColor = Color.LemonChiffon;
		}
		private void txtPassword_LostFocus(object sender, System.EventArgs e)
		{
			txtPassword.BackColor = Color.White;
		}
		#endregion

		private void txtPassword_TextChanged(object sender, System.EventArgs e)
		{
		
		}
	}
}
