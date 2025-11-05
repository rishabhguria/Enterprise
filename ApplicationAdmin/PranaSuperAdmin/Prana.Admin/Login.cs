#region Using Namespaces
using Castle.Core.Resource;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using Prana.Admin.BLL;
using Prana.Auth.Authentication.BLL;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Authorization;
using Prana.BusinessObjects.Enums;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
#endregion

namespace Prana.Admin
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
        static FileStream myFileStream = null;

        #endregion
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _Login_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _Login_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _Login_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _Login_UltraFormManager_Dock_Area_Bottom;
        private BackgroundWorker bgStartCaching;


        private IContainer components;

        /// <summary>
        /// Form Constructor.
        /// </summary>
        public Login()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            if (CustomThemeHelper.ApplyTheme)
            {
                var rc = new Rectangle(0, 20, this.ClientSize.Width,
                    this.ClientSize.Height);
                e.Graphics.DrawImage(WhiteLabelTheme.LoginBackGroundImage, rc);
            }
        }

        private void ApplyTheme()
        {
            try
            {
                //PranaReleaseViewType pranaReleaseType = CachedDataManager.GetInstance.GetPranaReleaseViewType();
                //if (CustomThemeHelper.APPLY_THEME)
                //{
                //this.BackgroundImageLayout = ImageLayout.Center;
                this.BackgroundImage = Properties.Resources.DefaultSplash;
                this.lblLogin.Location = new System.Drawing.Point(146, 178);
                this.lblPassword.Location = new System.Drawing.Point(146, 206);
                this.txtLogin.Location = new System.Drawing.Point(232, 176);
                this.txtPassword.Location = new System.Drawing.Point(232, 204);
                this.btnLogin.Location = new System.Drawing.Point(158, 238);
                this.btnClose.Location = new System.Drawing.Point(242, 238);
                this.btnClose.BackgroundImage = Properties.Resources.LoginExit_NewUI;
                this.btnLogin.BackgroundImage = Properties.Resources.LoginButton_NewUI;


                this.Icon = WhiteLabelTheme.AppIcon;
                this.Text = WhiteLabelTheme.AppTitle + " " + "Administration Login";
                this.BackgroundImage = WhiteLabelTheme.LoginBackGroundImage;

                this.lblLogin.Image = WhiteLabelTheme.LoginUserNameLabelImage;
                this.lblPassword.Image = WhiteLabelTheme.LoginPasswordLabelImage;

                //this.lblLogin.Text = string.Empty;
                //this.lblPassword.Text = string.Empty;
                this.btnLogin.BackgroundImage = WhiteLabelTheme.LoginBtnBackgroundImage;
                this.btnClose.BackgroundImage = WhiteLabelTheme.CloseBtnBackgroundImage;
                //this.btnLogin.Size = new System.Drawing.Size(78, 26);
                //this.btnClose.Size = new System.Drawing.Size(78, 26);
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_TRADING_TICKET);
                //}
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
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
                if (lblPassword != null)
                {
                    lblPassword.Dispose();
                }
                if (txtPassword != null)
                {
                    txtPassword.Dispose();
                }
                if (btnLogin != null)
                {
                    btnLogin.Dispose();
                }
                if (errorProvider1 != null)
                {
                    errorProvider1.Dispose();
                }
                if (_Login_UltraFormManager_Dock_Area_Bottom != null)
                {
                    _Login_UltraFormManager_Dock_Area_Bottom.Dispose();
                }
                if (_Login_UltraFormManager_Dock_Area_Left != null)
                {
                    _Login_UltraFormManager_Dock_Area_Left.Dispose();
                }
                if (_Login_UltraFormManager_Dock_Area_Right != null)
                {
                    _Login_UltraFormManager_Dock_Area_Right.Dispose();
                }
                if (_Login_UltraFormManager_Dock_Area_Top != null)
                {
                    _Login_UltraFormManager_Dock_Area_Top.Dispose();
                }
                if (txtLogin != null)
                {
                    txtLogin.Dispose();
                }
                if (ultraFormManager1 != null)
                {
                    ultraFormManager1.Dispose();
                }
                if (bgStartCaching != null)
                {
                    bgStartCaching.Dispose();
                }
                if (btnClose != null)
                {
                    btnClose.Dispose();
                }
                if (lblLogin != null)
                {
                    lblLogin.Dispose();
                }
            }
            base.Dispose(disposing);
        }


        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.lblLogin = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtLogin = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._Login_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._Login_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._Login_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._Login_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.bgStartCaching = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblLogin
            // 
            this.lblLogin.BackColor = System.Drawing.Color.Transparent;
            this.lblLogin.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogin.Location = new System.Drawing.Point(146, 178);
            this.lblLogin.Name = "lblLogin";
            this.lblLogin.Size = new System.Drawing.Size(80, 14);
            this.lblLogin.TabIndex = 0;
            // 
            // lblPassword
            // 
            this.lblPassword.BackColor = System.Drawing.Color.Transparent;
            this.lblPassword.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPassword.Location = new System.Drawing.Point(146, 206);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(76, 14);
            this.lblPassword.TabIndex = 1;
            // 
            // txtPassword
            // 
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtPassword.Location = new System.Drawing.Point(232, 204);
            this.txtPassword.MaxLength = 20;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '#';
            this.txtPassword.Size = new System.Drawing.Size(96, 21);
            this.txtPassword.TabIndex = 2;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            this.txtPassword.GotFocus += new System.EventHandler(this.txtPassword_GotFocus);
            this.txtPassword.LostFocus += new System.EventHandler(this.txtPassword_LostFocus);
            // 
            // txtLogin
            // 
            this.txtLogin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLogin.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtLogin.Location = new System.Drawing.Point(232, 176);
            this.txtLogin.MaxLength = 20;
            this.txtLogin.Name = "txtLogin";
            this.txtLogin.Size = new System.Drawing.Size(96, 21);
            this.txtLogin.TabIndex = 1;
            this.txtLogin.GotFocus += new System.EventHandler(this.txtLogin_GotFocus);
            this.txtLogin.LostFocus += new System.EventHandler(this.txtLogin_LostFocus);
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.Transparent;
            this.btnLogin.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnLogin.BackgroundImage")));
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnLogin.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnLogin.Location = new System.Drawing.Point(158, 238);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(78, 26);
            this.btnLogin.TabIndex = 3;
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnClose.Location = new System.Drawing.Point(242, 238);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(78, 26);
            this.btnClose.TabIndex = 4;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _Login_UltraFormManager_Dock_Area_Left
            // 
            this._Login_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._Login_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._Login_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._Login_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Login_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._Login_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 4;
            this._Login_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 26);
            this._Login_UltraFormManager_Dock_Area_Left.Name = "_Login_UltraFormManager_Dock_Area_Left";
            this._Login_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(4, 245);
            // 
            // _Login_UltraFormManager_Dock_Area_Right
            // 
            this._Login_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._Login_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._Login_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._Login_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Login_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._Login_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 4;
            this._Login_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(332, 26);
            this._Login_UltraFormManager_Dock_Area_Right.Name = "_Login_UltraFormManager_Dock_Area_Right";
            this._Login_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(4, 245);
            // 
            // _Login_UltraFormManager_Dock_Area_Top
            // 
            this._Login_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._Login_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._Login_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._Login_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Login_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._Login_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._Login_UltraFormManager_Dock_Area_Top.Name = "_Login_UltraFormManager_Dock_Area_Top";
            this._Login_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(336, 26);
            // 
            // _Login_UltraFormManager_Dock_Area_Bottom
            // 
            this._Login_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._Login_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._Login_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._Login_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Login_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._Login_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
            this._Login_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 271);
            this._Login_UltraFormManager_Dock_Area_Bottom.Name = "_Login_UltraFormManager_Dock_Area_Bottom";
            this._Login_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(336, 4);
            // 
            // bgStartCaching
            // 
            this.bgStartCaching.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgStartCaching_DoWork);
            // 
            // Login
            // 
            this.AcceptButton = this.btnLogin;
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
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
            this.Controls.Add(this._Login_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._Login_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._Login_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._Login_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Administration: User Login";
            this.Load += new System.EventHandler(this.Login_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // coomented by omshiv, 11 April 2014, for open multple instance of Admin
            //if (!Prana.Utilities.Win32Utilities.WinUtilities.SigletonCheck())
            //    return;

            try
            {
                //To Handle Exceptions
                Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

                IWindsorContainer container = new WindsorContainer(new XmlInterpreter(new ConfigResource("castle")));
                WindsorContainerManager.Container = container;
                // Initializing logging
                Logger.Initialize(container);

                // Initializing DatabaseManager
                DatabaseManager.DatabaseManager.Initialize(container);

                Application.Run(new Login());
            }
            catch (Exception ex)
            {
                //We are using try catch in catch block because in case of POLICY_LOGANDSHOW take time or not able to throw exception
                try
                {
                    // Invoke our policy that is responsible for making sure no secure information
                    // gets out of our layer.
                    bool rethrow = Logger.HandleException(new Exception("Error initializing application", ex), LoggingConstants.POLICY_LOGANDSHOW);

                    if (rethrow)
                    {
                        throw;
                    }
                }
                catch
                {
                    MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "Error initializing application");
                }
            }
        }

        #region Unhandled Exceptions
        /// <summary>
        /// The Application.ThreadException event fires when an exception is thrown from code that was ultimately called as a result of a Windows message (for example, a keyboard, mouse or "paint" message) – in short, nearly all code in a typical Windows Forms application. While this works perfectly, it lulls one into a false sense of security – that all exceptions will be caught by the central exception handler. Exceptions thrown on worker threads are a good example of exceptions not caught by Application.ThreadException (the code inside the Main method is another – including the main form's constructor, which executes before the Windows message loop begins).
        /// http://www.albahari.com/threading/
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ex"></param>
        public static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs ex)
        {
            try
            {
                throw new Exception("Caught Unhandled Exception", ex.Exception);
                //Here if the exception is caught it will be handled by the catch and it will log it 
            }
            catch (Exception e)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(e, LoggingConstants.POLICY_LOGONLY);

                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                GC.Collect();
            }
            //string formattedInfo = ex.Exception.StackTrace.ToString();
            //Logger.LoggerWrite(formattedInfo, Prana.Global.Common.LOG_CATEGORY_EXCEPTION, 1,     1, System.Diagnostics.TraceEventType.Error, FORM_NAME);

        }


        /// <summary>
        /// The .NET framework provides a lower-level event for global exception handling: AppDomain.UnhandledException. This event fires when there's an unhandled exception in any thread, and in any type of application (with or without a user interface). However, while it offers a good last-resort mechanism for logging untrapped exceptions, it provides no means of preventing the application from shutting down – and no means to suppress the .NET unhandled exception dialog.
        /// http://www.albahari.com/threading/
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                string formattedInfo = "Caught unhandled. IsTerminating : " + e.IsTerminating + " " + e.ExceptionObject.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, "Program");
                //Here if the exception is caught it will be handled by the catch and it will log it 
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);

                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                GC.Collect();
                Application.Exit();
            }
            //MessageBox.Show(sender.ToString() + " " + "Unhandled");

        }
        #endregion

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Validate User and set authorizedPrincipal
        /// modified by - omshiv, 11 april 2014
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, System.EventArgs e)
        {
            NirvanaPrincipal authorizedPrincipal;

            // Permissions userPermissions = null;
            try
            {
                authorizedPrincipal = NirvanaAuthenticationManager.GetInstance().ValidateUser(txtLogin.Text, txtPassword.Text);

                //userPermissions = UserManager.ValidateLogin(txtLogin.Text, txtPassword.Text);
                // if (userPermissions != null)

                // Modified by Ankit Gupta on 12 Dec, 2014.
                // http://jira.nirvanasolutions.com:8080/browse/CHMW-1831
                // As per the discussion in JIRA, concluded that a simple user must not be allowed to access the Admin.
                if (authorizedPrincipal.Role == NirvanaRoles.User)
                {
                    MessageBox.Show("You do not have permission to access Admin." + System.Environment.NewLine + "Please contact administrator", "Authentication Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (authorizedPrincipal != null && authorizedPrincipal.Identity.IsAuthenticated)
                {
                    bool IsAlreadyLoggedIn = CheckUserAlreadyLoggedIn(authorizedPrincipal.Identity.Name);
                    if (!IsAlreadyLoggedIn)
                    {
                        SetUserLoggedIn(authorizedPrincipal.Identity.Name);

                        AuthorizationManager.GetInstance()._authorizedPrincipal = authorizedPrincipal;

                        CompanyUser user = new CompanyUser();
                        user.CompanyUserID = authorizedPrincipal.UserId;
                        user.ShortName = authorizedPrincipal.Identity.Name;
                        user.CompanyID = authorizedPrincipal.CompanyID;
                        CachedDataManager.GetInstance.SetCompanyUser(user);
                        bgStartCaching.RunWorkerAsync(user);
                        SuperAdminMain mainApplication = new SuperAdminMain();
                        // mainApplication.UserPermissions = userPermissions;
                        mainApplication.Show();
                        mainApplication.SetUp(authorizedPrincipal);
                        this.Hide();
                    }
                }
                else
                {
                    MessageBox.Show(this, "Please enter valid Username and Password.", "Login Alert", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    txtLogin.Focus();
                }

            }
            catch (Exception ex)
            {
                //throw(ex);
                //string formattedInfo = ex.StackTrace.ToString();
                string formattedInfo = ex.Message + " " + ex.InnerException + " " + ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                            FORM_NAME);
                if (ex.Message.Equals(ErrorStatements.ENCRYPTION_ERROR))
                    Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                else
                    Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            finally
            {
                #region LogEntry

                Logger.LoggerWrite("btnLogin_Click",
                                                LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                                                FORM_NAME + "btnLogin_Click", null);


                #endregion
            }
        }

        private void SetUserLoggedIn(string UserName)
        {

            try
            {

                String path = Application.StartupPath + "\\LoggedInUsers\\" + UserName + ".txt";
                // Create the file if it exists. 

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
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private bool CheckUserAlreadyLoggedIn(string UserName)
        {
            bool isUserloggedIn = false;
            try
            {

                String path = Application.StartupPath + "\\LoggedInUsers\\" + UserName + ".txt";
                if (File.Exists(path))
                {
                    using (FileStream fs2 = File.Open(path, FileMode.Open))
                    {
                        // Do some task here.

                    }
                }
                return isUserloggedIn;
            }
            catch (Exception)
            {
                isUserloggedIn = true;
                MessageBox.Show("User: " + UserName + " is already logged in.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return isUserloggedIn;
                //// Invoke our policy that is responsible for making sure no secure information
                //// gets out of our layer.
                //bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                //if (rethrow)
                //{
                //    throw;
                //}
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

        private void Login_Load(object sender, EventArgs e)
        {
            try
            {
                btnLogin.Focus();
                ApplyTheme();
                if (CustomThemeHelper.ApplyTheme)
                    this.ultraFormManager1.FormStyleSettings.FormDisplayStyle = Infragistics.Win.UltraWinToolbars.FormDisplayStyle.RoundedFixed;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// Starts Creating cache 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bgStartCaching_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                CachedDataManager.GetInstance.StartCaching((CompanyUser)e.Argument);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
