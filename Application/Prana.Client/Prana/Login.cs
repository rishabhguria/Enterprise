using Prana.Authentication.Common;
using Prana.BusinessObjects;
using Prana.CommonDatabaseAccess;
using Prana.CommonDataCache;
using Prana.CoreService.Interfaces;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Prana
{
    /// <summary>
    /// Summary description for Login.
    /// </summary>
    public class Login : System.Windows.Forms.Form
    {
        #region Private variables

        private RoundCornerTextBox txtLoginID;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnClose;
        private RoundCornerTextBox txtPassword;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.ErrorProvider errorProvider2;

        #endregion

        private IContainer components;

        Constants.LoginStatus _loginStatus = Constants.LoginStatus.NotSet;
        private Label lblUserName;
        private Label lblPassword;
        int _companyUserID = int.MinValue;
        static FileStream myFileStream = null;
        bool _isUserNotMapped = false;

        // <summary>
        /// proxy to authenticate login user from server
        /// </summary>
        private ProxyBase<IAuthenticateUser> _authenticateUserService = null;

        // <summary>
        /// proxy to authenticate login user from server
        /// </summary>
        private ProxyBase<IAuthService> _greenFieldAuthService = null;

        /// <summary>
        /// Fetch Samsara Azure Id from RDP Session
        /// </summary>
        public string SamsaraAzureId
        {
            get
            {
                string sessionName = Environment.GetEnvironmentVariable("SESSIONNAME");

                if (!_isUserNotMapped && !string.IsNullOrEmpty(sessionName) && sessionName.Contains("RDP"))
                {
                    string domain = Constants.AzureRDPServer;
                    return Environment.UserName.ToLower() + domain;
                }
                else
                    return string.Empty;
            }
        }

        /// <summary>
        /// User Login Status
        /// </summary>
        public Constants.LoginStatus LoginStatus
        {
            get
            {
                return _loginStatus;
            }
        }

        public int CompanyUserID
        {
            get
            {
                return _companyUserID;
            }
        }

        public Login()
        {

            InitializeComponent();
            if (_authenticateUserService == null)
            {
                _authenticateUserService = new ProxyBase<IAuthenticateUser>(EndPointAddressConstants.CONST_TradeAuthenticateUserServiceEndpoint);
            }
            if (_greenFieldAuthService == null)
            {
                _greenFieldAuthService = new ProxyBase<IAuthService>(EndPointAddressConstants.CONST_AuthServiceEndpoint);
            }
            if (!string.IsNullOrEmpty(SamsaraAzureId))
                AuthenticateUser(true);
        }


        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (txtLoginID != null)
                {
                    txtLoginID.Dispose();
                }
                if (btnLogin != null)
                {
                    btnLogin.Dispose();
                }
                if (btnClose != null)
                {
                    btnClose.Dispose();
                }
                if (txtPassword != null)
                {
                    txtPassword.Dispose();
                }
                if (errorProvider1 != null)
                {
                    errorProvider1.Dispose();
                }
                if (errorProvider2 != null)
                {
                    errorProvider2.Dispose();
                }
                if (lblUserName != null)
                {
                    lblUserName.Dispose();
                }
                if (lblPassword != null)
                {
                    lblPassword.Dispose();
                }
                if (_authenticateUserService != null)
                {
                    _authenticateUserService.Dispose();
                }
                if (_greenFieldAuthService != null)
                {
                    _greenFieldAuthService.Dispose();
                }
            }
            base.Dispose(disposing);
        }


        //[STAThread]
        //static void Main()
        //{
        //    Application.Run(new Login(null));
        //}


        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.txtLoginID = new RoundCornerTextBox();
            this.txtPassword = new RoundCornerTextBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.errorProvider2 = new System.Windows.Forms.ErrorProvider(this.components);
            this.lblUserName = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.txtLoginID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider2)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.Transparent;
            this.btnLogin.BackgroundImage = global::Prana.Properties.Resources.LoginButton_NewUI;
            this.btnLogin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLogin.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnLogin.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnLogin.ForeColor = System.Drawing.Color.Transparent;
            this.btnLogin.Location = new System.Drawing.Point(74, 219);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(92, 40);
            this.btnLogin.TabIndex = 4;
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.MouseEnter += btnLogin_MouseEnter;
            this.btnLogin.MouseLeave += btnLogin_MouseLeave;
            this.btnLogin.GotFocus += btnLogin_GotFocus;
            this.btnLogin.LostFocus += btnLogin_LostFocus;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Prana.Properties.Resources.LoginExit_NewUI;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.ForeColor = System.Drawing.Color.Transparent;
            this.btnClose.Location = new System.Drawing.Point(169, 219);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(92, 40);
            this.btnClose.TabIndex = 1;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.MouseEnter += btnClose_MouseEnter;
            this.btnClose.MouseLeave += btnClose_MouseLeave;
            this.btnClose.GotFocus += btnClose_GotFocus;
            this.btnClose.LostFocus += btnClose_LostFocus;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtLoginID
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.BackColor2 = System.Drawing.Color.Transparent;
            appearance1.BorderColor = System.Drawing.Color.Transparent;
            appearance1.BorderColor2 = System.Drawing.Color.Transparent;
            appearance1.TextHAlign = Infragistics.Win.HAlign.Center;
            appearance1.FontData.SizeInPoints = 11F;
            appearance1.ForeColor = System.Drawing.Color.White;
            this.txtLoginID.Appearance = appearance1;
            this.txtLoginID.AutoSize = false;
            this.txtLoginID.BackColor = System.Drawing.Color.Transparent;
            this.txtLoginID.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.txtLoginID.Location = new System.Drawing.Point(67, 92);
            //this.txtLoginID.Font = new System.Drawing.Font("Verdana", 80F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Regular))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLoginID.Name = "txtLoginID";
            this.txtLoginID.Size = new System.Drawing.Size(200, 43);
            this.txtLoginID.TabIndex = 2;
            this.txtLoginID.WordWrap = false;
            this.txtLoginID.NullText = "Username";
            appearance2.BackColor = System.Drawing.Color.Transparent;
            appearance2.BackColor2 = System.Drawing.Color.Transparent;
            appearance2.AlphaLevel = 100;
            this.txtLoginID.NullTextAppearance = appearance2;
            this.txtLoginID.AfterEnterEditMode += new System.EventHandler(this.txtLoginID_AfterEnterEditMode);
            this.txtLoginID.AfterExitEditMode += new System.EventHandler(this.txtLoginID_AfterExitEditMode);
            this.txtLoginID.KeyDown += textBox_KeyDown;
            // 
            // txtPassword
            // 
            appearance3.BackColor = System.Drawing.Color.Transparent;
            appearance3.BackColor2 = System.Drawing.Color.Transparent;
            appearance3.BorderColor = System.Drawing.Color.Transparent;
            appearance3.BorderColor2 = System.Drawing.Color.Transparent;
            appearance3.FontData.SizeInPoints = 11F;
            appearance3.TextHAlign = Infragistics.Win.HAlign.Center;
            appearance3.ForeColor = System.Drawing.Color.White;
            this.txtPassword.Appearance = appearance3;
            this.txtPassword.AutoSize = false;
            this.txtPassword.BackColor = System.Drawing.Color.Transparent;
            this.txtPassword.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.txtPassword.Location = new System.Drawing.Point(67, 146);
            //this.txtPassword.Font = new System.Drawing.Font("Verdana", 80F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Regular))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(200, 43);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.NullText = "Password";
            appearance4.BackColor = System.Drawing.Color.Transparent;
            appearance4.BackColor2 = System.Drawing.Color.Transparent;
            appearance4.AlphaLevel = 100;
            this.txtPassword.NullTextAppearance = appearance4;
            this.txtPassword.AfterEnterEditMode += new System.EventHandler(this.txtPassword_AfterEnterEditMode);
            this.txtPassword.AfterExitEditMode += new System.EventHandler(this.txtPassword_AfterExitEditMode);
            this.txtPassword.KeyDown += textBox_KeyDown;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // errorProvider2
            // 
            this.errorProvider2.ContainerControl = this;
            // 
            // lblUserName
            // 
            this.lblUserName.BackColor = System.Drawing.Color.Transparent;
            this.lblUserName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblUserName.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserName.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblUserName.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblUserName.Location = new System.Drawing.Point(63, 118);
            this.lblUserName.Margin = new System.Windows.Forms.Padding(0);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(93, 20);
            this.lblUserName.TabIndex = 5;
            this.lblUserName.Text = "";
            // 
            // lblPassword
            // 
            this.lblPassword.BackColor = System.Drawing.Color.Transparent;
            this.lblPassword.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblPassword.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPassword.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblPassword.Location = new System.Drawing.Point(56, 147);
            this.lblPassword.Margin = new System.Windows.Forms.Padding(0);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(100, 23);
            this.lblPassword.TabIndex = 6;
            this.lblPassword.Text = "";
            // 
            // Login
            // 
            //this.AcceptButton = this.btnLogin;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(87)))), ((int)(((byte)(95)))), ((int)(((byte)(107)))));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(334, 334);
            this.ControlBox = false;
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblUserName);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.txtLoginID);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.Transparent;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Nirvana: User Login";
            this.Load += new System.EventHandler(this.Login_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtLoginID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider2)).EndInit();
            this.ResumeLayout(false);
        }

        void btnClose_LostFocus(object sender, EventArgs e)
        {
            this.btnClose.BackgroundImage = global::Prana.Properties.Resources.LoginExit_NewUI;
        }

        void btnClose_GotFocus(object sender, EventArgs e)
        {
            this.btnClose.BackgroundImage = global::Prana.Properties.Resources.LoginExit_NewUIHover;
        }

        void btnLogin_LostFocus(object sender, EventArgs e)
        {
            this.btnLogin.BackgroundImage = global::Prana.Properties.Resources.LoginButton_NewUI;
        }

        void btnLogin_GotFocus(object sender, EventArgs e)
        {
            this.btnLogin.BackgroundImage = global::Prana.Properties.Resources.LoginButton_NewUIHover;
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                btnLogin_Click(null, null);
            }
        }

        private void btnLogin_MouseLeave(object sender, EventArgs e)
        {
            this.btnLogin.BackgroundImage = global::Prana.Properties.Resources.LoginButton_NewUI;
        }

        private void btnLogin_MouseEnter(object sender, EventArgs e)
        {
            this.btnLogin.BackgroundImage = global::Prana.Properties.Resources.LoginButton_NewUIHover;
        }

        void btnClose_MouseLeave(object sender, EventArgs e)
        {
            this.btnClose.BackgroundImage = global::Prana.Properties.Resources.LoginExit_NewUI;
        }

        void btnClose_MouseEnter(object sender, EventArgs e)
        {
            this.btnClose.BackgroundImage = global::Prana.Properties.Resources.LoginExit_NewUIHover;
        }
        #endregion

        /// <summary>
        /// Close the Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, System.EventArgs e)
        {
            _loginStatus = Constants.LoginStatus.NotSet;
            Application.Exit();
        }

        /// <summary>
        /// Validate User Input
        /// </summary>
        /// <returns></returns>
        bool validate()
        {
            bool result = true;
            if (txtLoginID.Text == string.Empty)
            {
                result = false;
                errorProvider1.SetError(txtLoginID, "Enter Username");
            }
            else
            {
                errorProvider1.Clear();
            }

            if (txtPassword.Text == string.Empty)
            {
                result = false;
                errorProvider2.SetError(txtPassword, "Enter Password");
            }
            else
            {
                errorProvider2.Clear();
            }
            return result;
        }

        /// <summary>
        /// Login User
        /// Register User with Server(Core)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, System.EventArgs e)
        {
            AuthenticateUser();
        }

        /// <summary>
        /// Login User
        /// Register User with Server(Core)
        /// </summary>
        private void AuthenticateUser(bool is2FALogin = false)
        {
            try
            {
                AuthenticatedUserInfo authUser = new AuthenticatedUserInfo();
                if (is2FALogin)
                {
                    if (string.IsNullOrEmpty(SamsaraAzureId))
                        return;
                    DialogResult = DialogResult.OK;
                    var applyTheme = WhiteLabelTheme.ApplyTheme;
                    authUser = _authenticateUserService.InnerChannel.ValidateCompanyUserLogin(string.Empty, string.Empty, false, SamsaraAzureId);
                }
                else
                {
                    if (!validate())
                    {
                        return;
                    }
                    string login = txtLoginID.Text;
                    string password = txtPassword.Text;
                    authUser = _authenticateUserService.InnerChannel.ValidateCompanyUserLogin(login, password, false);
                }
                #region Validation for users
                _companyUserID = authUser.CompanyUserId;
                if (!(_companyUserID > 0))
                {
                    if (is2FALogin)
                        _isUserNotMapped = true;
                    else
                        MessageBox.Show(this, "Invalid User or Password", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _loginStatus = Constants.LoginStatus.InValidUser;
                    CachedDataManager.GetInstance.SetCompanyUser(null);
                }
                else
                {
                    string alreadyLoggedInErrMsg = authUser.ErrorMessage;
                    if (string.IsNullOrWhiteSpace(alreadyLoggedInErrMsg))
                    {
                        try
                        {
                            _loginStatus = Constants.LoginStatus.ValidUser;
                            CachedDataManager.GetInstance.SetCompanyUser(authUser.CompanyUser);
                            if (!is2FALogin)
                                this.Close();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Server Not Running.");
                        }
                    }
                    else
                    {
                        if (authUser.AuthenticationType == AuthenticationTypes.WebLoggedIn)
                        {
                            DialogResult userChoice = new CustomMessageBox("Alert", alreadyLoggedInErrMsg, false, string.Empty, FormStartPosition.CenterParent, MessageBoxButtons.RetryCancel).ShowDialog();
                            if (userChoice == DialogResult.OK)
                            {
                                try
                                {
                                    bool result = _greenFieldAuthService.InnerChannel.CompanyUserLogout(_companyUserID.ToString(), false, true);
                                    if (result)
                                    {
                                        _loginStatus = Constants.LoginStatus.ValidUser;
                                        CachedDataManager.GetInstance.SetCompanyUser(authUser.CompanyUser);
                                        if (!is2FALogin)
                                            this.Close();
                                        return;
                                    }
                                    else
                                    {
                                        DialogResult userChoice1 = new CustomMessageBox("Alert", AuthenticationConstants.MSG_FAILED_LOGGED_OUT, false, string.Empty, FormStartPosition.CenterParent, MessageBoxButtons.OK).ShowDialog();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    if (ex.Message.Contains(AuthenticationConstants.SERVICE_NOT_AVAILABLE_ERROR))
                                    {
                                        MessageBox.Show(this, AuthenticationConstants.MSG_CORE_SERVICES_NOT_RUNNING, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                            }
                        }
                        else
                            MessageBox.Show(alreadyLoggedInErrMsg, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        _loginStatus = Constants.LoginStatus.InValidUser;
                        CachedDataManager.GetInstance.SetCompanyUser(null);
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                bool rethrow;
                if (ex.Message.Equals(ErrorStatements.ENCRYPTION_ERROR))
                {
                    rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                }
                else if (ex.Message.Contains(AuthenticationConstants.SERVICE_NOT_AVAILABLE_ERROR))
                {
                    MessageBox.Show(this, AuthenticationConstants.MSG_CORE_SERVICES_NOT_RUNNING, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    rethrow = false;
                }
                else
                {
                    rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                }

                if (rethrow)
                {
                    throw;
                }
            }
        }


        private void Login_Load(object sender, System.EventArgs e)
        {
            this.Icon = WhiteLabelTheme.AppIcon;
            this.Text = WhiteLabelTheme.AppTitle + ": " + "User Login";
            this.BackgroundImage = WhiteLabelTheme.LoginBackGroundImage;

            this.lblUserName.AutoSize = true;
            this.lblUserName.Font = new System.Drawing.Font("Segoe UI", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserName.Location = new System.Drawing.Point(47, 118);
            this.lblUserName.Size = new System.Drawing.Size(93, 21);
            //this.lblUserName.Text = "Username :";

            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("Segoe UI", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPassword.Location = new System.Drawing.Point(52, 147);
            this.lblPassword.Size = new System.Drawing.Size(88, 21);
            //this.lblPassword.Text = "Password :";
        }

        private void SetUserLoggedIn(string UserName)
        {
            try
            {
                String path = Application.StartupPath + "\\LoggedInUsers\\" + UserName + ".txt";
                if (!Directory.Exists(Application.StartupPath + "\\LoggedInUsers"))
                {
                    Directory.CreateDirectory(Application.StartupPath + "\\LoggedInUsers");
                }
                if (!File.Exists(path))
                {
                    // Create the file. 
                    using (FileStream fs = File.Create(path))
                    {
                        Byte[] info = new UTF8Encoding(true).GetBytes("\n---------------------------\n User name: " + UserName + "\n-------------------------------");

                        // Add some information to the file.
                        fs.Write(info, 0, info.Length);
                    }
                }
                myFileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void SetUserLoggedOut()
        {
            try
            {
                myFileStream.Close();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void txtLoginID_AfterEnterEditMode(object sender, EventArgs e)
        {
            (sender as RoundCornerTextBox).Appearance.BackColor = Color.FromArgb(87, 95, 107);
        }

        private void txtLoginID_AfterExitEditMode(object sender, EventArgs e)
        {
            (sender as RoundCornerTextBox).Appearance.BackColor = Color.Transparent;
        }

        private void txtPassword_AfterEnterEditMode(object sender, EventArgs e)
        {
            (sender as RoundCornerTextBox).Appearance.BackColor = Color.FromArgb(87, 95, 107);
        }

        private void txtPassword_AfterExitEditMode(object sender, EventArgs e)
        {
            (sender as RoundCornerTextBox).Appearance.BackColor = Color.Transparent;
        }
    }
}
