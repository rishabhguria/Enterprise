using Prana.Admin.BLL;
using Prana.BusinessObjects;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;


namespace Prana.Preferences
{

    /// <summary>
    /// Summary description for PreferencesMain.
    /// </summary>
    public class PreferencesMain : System.Windows.Forms.Form
    {
        private const string GENERAL = "General";
        private const string ROOT_NODE = "Preferences Modules";
        private Infragistics.Win.Misc.UltraPanel pnlLeft;
        private Infragistics.Win.Misc.UltraPanel pnlRight;
        private Infragistics.Win.UltraWinTree.UltraTree ultraTreePrefs;
        private Infragistics.Win.Misc.UltraPanel pnlBottom;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton btnRestoreDefault;
        private IContainer components = new System.ComponentModel.Container();

        /// <summary>
        /// PNLClosed will be fired if PNL is closed
        /// </summary>
        public event EventHandler PreferencesClosed;
        /// <summary>
        /// LaunchPreferences to launch the preferences
        /// </summary>
        private Hashtable _availableModulesDetails;
        private System.Windows.Forms.UserControl prefsForm = null;
        private string _prefsModuleName = string.Empty;
        private Prana.BusinessObjects.CompanyUser _loginUser;
        private Infragistics.Win.Misc.UltraButton btnApply;

        /// <summary>
        /// Holds the references to the PNL UserControl
        /// </summary>
        private IPreferences prefsInstance;
        private IPreferenceData _prefsData;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        private Infragistics.Win.Misc.UltraPanel PreferencesMain_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _PreferencesMain_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _PreferencesMain_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _PreferencesMain_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _PreferencesMain_UltraFormManager_Dock_Area_Bottom;
        //private CtrlImageListButtons ctrlImageListButtons1;

        public event ApplyPreferenceHandler ApplyPrefsClick;
        //public event SaveClickHandler SaveClick;
        public static ISecurityMasterServices securityMasterObject;
        /// <summary>
        /// The security master
        /// </summary>
        public ISecurityMasterServices _securityMaster;
        #region Properties

        /// <summary>
        /// Gets or sets the security master.
        /// </summary>
        /// <value>
        /// The security master.
        /// </value>
        public ISecurityMasterServices SecurityMaster
        {
            get
            {
                return _securityMaster;
            }
            set
            {
                _securityMaster = value;
                securityMasterObject = value;
            }
        }
        public Hashtable AvailableModulesDetails
        {
            get
            {
                return this._availableModulesDetails;
            }

            set
            {
                this._availableModulesDetails = value;
            }
        }

        public string PrefsModuleName
        {
            get
            {
                return _prefsModuleName;
            }

            set
            {
                this._prefsModuleName = value;
            }
        }

        public Prana.BusinessObjects.CompanyUser LoginUser
        {
            get { return _loginUser; }
            set { _loginUser = value; }
        }

        // added by Sandeep as on 26-Dec-2007 
        private int _read_write = 0;

        public int Read_Write
        {
            get { return _read_write; }
            set { _read_write = value; }
        }

        #endregion

        private PreferencesMain()
        {
            InitializeComponent();
        }

        private PreferencesMain(Hashtable availableModulesDetails)
        {
            this._availableModulesDetails = availableModulesDetails;

            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        private static PreferencesMain _instPreferencesMain;


        private static object _lockerObj = new object();

        /// <summary>
        /// Singleton instance for the newsStory form
        /// </summary>
        /// <returns></returns>
        public static PreferencesMain GetInstance()
        {
            if (_instPreferencesMain == null || _instPreferencesMain.IsDisposed)
            {
                lock (_lockerObj)
                {
                    if (_instPreferencesMain == null || _instPreferencesMain.IsDisposed)
                    {
                        _instPreferencesMain = new PreferencesMain();
                    }
                }
            }
            return _instPreferencesMain;
        }

        public void LoadControl()
        {
            LoadLeftTreeControl();

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
                    //dispose the pnl prefs control
                    if (prefsForm != null && !prefsForm.IsDisposed)
                        prefsForm.Dispose();

                    if (_instPreferencesMain != null && !_instPreferencesMain.IsDisposed)
                        _instPreferencesMain.Dispose(false);

                    components.Dispose();
                }

                if (pnlLeft != null)
                {
                    pnlLeft.Dispose();
                }

                if (pnlRight != null)
                {
                    pnlRight.Dispose();
                }

                if (ultraTreePrefs != null)
                {
                    ultraTreePrefs.Dispose();
                }

                if (pnlBottom != null)
                {
                    pnlBottom.Dispose();
                }

                if (btnApply != null)
                {
                    btnApply.Dispose();
                }

                if (statusStrip1 != null)
                {
                    statusStrip1.Dispose();
                }

                if (btnSave != null)
                {
                    btnSave.Dispose();
                }

                if (btnClose != null)
                {
                    btnClose.Dispose();
                }

                if (btnRestoreDefault != null)
                {
                    btnRestoreDefault.Dispose();
                }

                if (toolStripStatusLabel1 != null)
                {
                    toolStripStatusLabel1.Dispose();
                }

                if (inboxControlStyler1 != null)
                {
                    inboxControlStyler1.Dispose();
                }

                if (PreferencesMain_Fill_Panel != null)
                {
                    PreferencesMain_Fill_Panel.Dispose();
                }

                if (ultraFormManager1 != null)
                {
                    ultraFormManager1.Dispose();
                }

                if (_PreferencesMain_UltraFormManager_Dock_Area_Left != null)
                {
                    _PreferencesMain_UltraFormManager_Dock_Area_Left.Dispose();
                }

                if (_PreferencesMain_UltraFormManager_Dock_Area_Right != null)
                {
                    _PreferencesMain_UltraFormManager_Dock_Area_Right.Dispose();
                }

                if (_PreferencesMain_UltraFormManager_Dock_Area_Top != null)
                {
                    _PreferencesMain_UltraFormManager_Dock_Area_Top.Dispose();
                }

                if (_PreferencesMain_UltraFormManager_Dock_Area_Bottom != null)
                {
                    _PreferencesMain_UltraFormManager_Dock_Area_Bottom.Dispose();
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
            Infragistics.Win.UltraWinTree.Override _override1 = new Infragistics.Win.UltraWinTree.Override();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this.pnlLeft = new Infragistics.Win.Misc.UltraPanel();
            this.ultraTreePrefs = new Infragistics.Win.UltraWinTree.UltraTree();
            this.pnlRight = new Infragistics.Win.Misc.UltraPanel();
            this.pnlBottom = new Infragistics.Win.Misc.UltraPanel();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.btnApply = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnRestoreDefault = new Infragistics.Win.Misc.UltraButton();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            this.PreferencesMain_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._PreferencesMain_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._PreferencesMain_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._PreferencesMain_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._PreferencesMain_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.pnlLeft.ClientArea.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTreePrefs)).BeginInit();
            this.pnlRight.SuspendLayout();
            this.pnlBottom.ClientArea.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            this.PreferencesMain_Fill_Panel.ClientArea.SuspendLayout();
            this.PreferencesMain_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlLeft
            // 
            // 
            // pnlLeft.ClientArea
            // 
            this.pnlLeft.ClientArea.Controls.Add(this.ultraTreePrefs);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(182, 525);
            this.pnlLeft.TabIndex = 0;
            // 
            // ultraTreePrefs
            // 
            this.ultraTreePrefs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraTreePrefs.Location = new System.Drawing.Point(0, 0);
            this.ultraTreePrefs.Name = "ultraTreePrefs";
            appearance1.BackColor = System.Drawing.Color.Blue;
            appearance1.ForeColor = System.Drawing.Color.White;
            _override1.ActiveNodeAppearance = appearance1;
            this.ultraTreePrefs.Override = _override1;
            this.ultraTreePrefs.Size = new System.Drawing.Size(182, 525);
            this.ultraTreePrefs.TabIndex = 0;
            this.ultraTreePrefs.ViewStyle = Infragistics.Win.UltraWinTree.ViewStyle.Standard;
            this.ultraTreePrefs.AfterActivate += new Infragistics.Win.UltraWinTree.AfterNodeChangedEventHandler(this.ultraTreePrefs_AfterActivate);
            this.ultraTreePrefs.AfterSelect += new Infragistics.Win.UltraWinTree.AfterNodeSelectEventHandler(this.ultraTreePrefs_AfterSelect);
            this.ultraTreePrefs.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ultraTreePrefs_MouseUp);
            // 
            // pnlRight
            // 
            this.pnlRight.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(182, 0);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(807, 525);
            this.pnlRight.TabIndex = 1;
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            // 
            // pnlBottom.ClientArea
            // 
            this.pnlBottom.ClientArea.Controls.Add(this.btnClose);
            this.pnlBottom.ClientArea.Controls.Add(this.btnApply);
            this.pnlBottom.ClientArea.Controls.Add(this.btnSave);
            this.pnlBottom.ClientArea.Controls.Add(this.statusStrip1);
            this.pnlBottom.ClientArea.Controls.Add(this.btnRestoreDefault);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 525);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(989, 52);
            this.pnlBottom.TabIndex = 2;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnClose.Location = new System.Drawing.Point(612, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnApply
            // 
            this.btnApply.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnApply.Location = new System.Drawing.Point(424, 3);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 8;
            this.btnApply.Text = "Apply";
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSave.Location = new System.Drawing.Point(516, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 30);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(989, 22);
            this.inboxControlStyler1.SetStyleSettings(this.statusStrip1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.statusStrip1.TabIndex = 9;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(943, 17);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnRestoreDefault
            // 
            this.btnRestoreDefault.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnRestoreDefault.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnRestoreDefault.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnRestoreDefault.Location = new System.Drawing.Point(301, 3);
            this.btnRestoreDefault.Name = "btnRestoreDefault";
            this.btnRestoreDefault.Size = new System.Drawing.Size(100, 23);
            this.btnRestoreDefault.TabIndex = 7;
            this.btnRestoreDefault.Text = "Restore Defaults";
            this.btnRestoreDefault.Click += new System.EventHandler(this.btnRestoreDefault_Click);
            // 
            // PreferencesMain_Fill_Panel
            // 
            // 
            // PreferencesMain_Fill_Panel.ClientArea
            // 
            this.PreferencesMain_Fill_Panel.ClientArea.Controls.Add(this.pnlRight);
            this.PreferencesMain_Fill_Panel.ClientArea.Controls.Add(this.pnlLeft);
            this.PreferencesMain_Fill_Panel.ClientArea.Controls.Add(this.pnlBottom);
            this.PreferencesMain_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.PreferencesMain_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PreferencesMain_Fill_Panel.Location = new System.Drawing.Point(8, 32);
            this.PreferencesMain_Fill_Panel.Name = "PreferencesMain_Fill_Panel";
            this.PreferencesMain_Fill_Panel.Size = new System.Drawing.Size(989, 577);
            this.PreferencesMain_Fill_Panel.TabIndex = 0;
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _PreferencesMain_UltraFormManager_Dock_Area_Left
            // 
            this._PreferencesMain_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._PreferencesMain_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._PreferencesMain_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._PreferencesMain_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._PreferencesMain_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._PreferencesMain_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._PreferencesMain_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 32);
            this._PreferencesMain_UltraFormManager_Dock_Area_Left.Name = "_PreferencesMain_UltraFormManager_Dock_Area_Left";
            this._PreferencesMain_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 577);
            // 
            // _PreferencesMain_UltraFormManager_Dock_Area_Right
            // 
            this._PreferencesMain_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._PreferencesMain_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._PreferencesMain_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._PreferencesMain_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._PreferencesMain_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._PreferencesMain_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._PreferencesMain_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(997, 32);
            this._PreferencesMain_UltraFormManager_Dock_Area_Right.Name = "_PreferencesMain_UltraFormManager_Dock_Area_Right";
            this._PreferencesMain_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 577);
            // 
            // _PreferencesMain_UltraFormManager_Dock_Area_Top
            // 
            this._PreferencesMain_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._PreferencesMain_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._PreferencesMain_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._PreferencesMain_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._PreferencesMain_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._PreferencesMain_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._PreferencesMain_UltraFormManager_Dock_Area_Top.Name = "_PreferencesMain_UltraFormManager_Dock_Area_Top";
            this._PreferencesMain_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(1005, 32);
            // 
            // _PreferencesMain_UltraFormManager_Dock_Area_Bottom
            // 
            this._PreferencesMain_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._PreferencesMain_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._PreferencesMain_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._PreferencesMain_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._PreferencesMain_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._PreferencesMain_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._PreferencesMain_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 609);
            this._PreferencesMain_UltraFormManager_Dock_Area_Bottom.Name = "_PreferencesMain_UltraFormManager_Dock_Area_Bottom";
            this._PreferencesMain_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(1005, 8);
            // 
            // PreferencesMain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(1005, 617);
            this.Controls.Add(this.PreferencesMain_Fill_Panel);
            this.Controls.Add(this._PreferencesMain_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._PreferencesMain_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._PreferencesMain_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._PreferencesMain_UltraFormManager_Dock_Area_Bottom);
            this.MinimumSize = new System.Drawing.Size(1005, 617);
            this.Name = "PreferencesMain";
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Text = "Preferences";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.PreferencesMain_Closing);
            this.Load += new System.EventHandler(this.PreferencesMain_Load);
            this.pnlLeft.ClientArea.ResumeLayout(false);
            this.pnlLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraTreePrefs)).EndInit();
            this.pnlRight.ResumeLayout(false);
            this.pnlBottom.ClientArea.ResumeLayout(false);
            this.pnlBottom.ClientArea.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            this.PreferencesMain_Fill_Panel.ClientArea.ResumeLayout(false);
            this.PreferencesMain_Fill_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        #region Load controls

        /// <summary>
        /// LoadLeftTreeControl() method loads the list of item in the left menu.
        /// </summary>
        private void LoadLeftTreeControl()
        {
            Infragistics.Win.UltraWinTree.UltraTreeNode rootNode = this.ultraTreePrefs.Nodes.Add(ROOT_NODE);
            rootNode.Tag = ROOT_NODE;
            Infragistics.Win.UltraWinTree.UltraTreeNode newNode;

            if (ModuleManager.CheckModulePermissioning(PranaModules.PERCENT_TRADING_TOOL, PranaModules.PERCENT_TRADING_TOOL))
            {
                newNode = rootNode.Nodes.Add(PranaModules.PERCENT_TRADING_TOOL.ToString());
                newNode.Tag = PranaModules.PERCENT_TRADING_TOOL;
                if (this._prefsModuleName.Equals(PranaModules.PERCENT_TRADING_TOOL))
                {
                    this.ultraTreePrefs.ActiveNode = newNode;
                    newNode.Selected = true;
                }
            }

            if (ModuleManager.CheckModulePermissioning(PranaModules.BLOTTER_MODULE, PranaModules.BLOTTER_MODULE))
            {
                newNode = rootNode.Nodes.Add(PranaModules.BLOTTER_MODULE.ToString());
                newNode.Tag = PranaModules.BLOTTER_MODULE;
                if (this._prefsModuleName.Equals(PranaModules.BLOTTER_MODULE))
                {
                    this.ultraTreePrefs.ActiveNode = newNode;
                    newNode.Selected = true;
                }
            }

            if (ModuleManager.CheckModulePermissioning(PranaModules.CLOSE_POSITIONS_MODULE, PranaModules.CLOSE_POSITIONS_MODULE))
            {
                newNode = rootNode.Nodes.Add(PranaModules.CLOSE_POSITIONS_MODULE.ToString());
                newNode.Tag = PranaModules.CLOSING_PREFS_MODULE;
                if (this._prefsModuleName.Equals(PranaModules.CLOSING_PREFS_MODULE))
                {
                    this.ultraTreePrefs.ActiveNode = newNode;
                    newNode.Selected = true;
                }
            }

            if (ModuleManager.CheckModulePermissioning(PranaModules.PORTFOLIO_MANAGEMENT_MODULE, PranaModules.PORTFOLIO_MANAGEMENT_MODULE))
            {
                newNode = rootNode.Nodes.Add(PranaModules.PORTFOLIO_MANAGEMENT_MODULE.ToString());
                newNode.Tag = PranaModules.PORTFOLIO_MANAGEMENT_MODULE;
                if (this._prefsModuleName.Equals(PranaModules.PORTFOLIO_MANAGEMENT_MODULE))
                {
                    this.ultraTreePrefs.ActiveNode = newNode;
                    newNode.Selected = true;
                }

            }

            if (ModuleManager.CheckModulePermissioning(PranaModules.POSITION_MANAGEMENT_MODULE, PranaModules.POSITION_MANAGEMENT_MODULE))
            {
                newNode = rootNode.Nodes.Add(PranaModules.POSITION_MANAGEMENT_MODULE.ToString());
                newNode.Tag = PranaModules.POSITION_MANAGEMENT_PREFS_MODULE;
                if (this._prefsModuleName.Equals(PranaModules.POSITION_MANAGEMENT_PREFS_MODULE))
                {
                    this.ultraTreePrefs.ActiveNode = newNode;
                    newNode.Selected = true;
                }
            }

            if (ModuleManager.CheckModulePermissioning(PranaModules.SHORTLOCATE_MODULE, PranaModules.SHORTLOCATE_MODULE))
            {
                newNode = rootNode.Nodes.Add(PranaModules.SHORTLOCATE_MODULE.ToString());
                newNode.Tag = PranaModules.SHORTLOCATE_MODULE;
                if (this._prefsModuleName.Equals(PranaModules.SHORTLOCATE_MODULE))
                {
                    this.ultraTreePrefs.ActiveNode = newNode;
                    newNode.Selected = true;
                }
            }

            if (ModuleManager.CheckModulePermissioning(PranaModules.TRADING_TICKET_MODULE, PranaModules.TRADING_TICKET_MODULE))
            {
                newNode = rootNode.Nodes.Add(ApplicationConstants.CONST_Trading);
                newNode.Tag = PranaModules.TRADING_TICKET_MODULE;
                if (this._prefsModuleName.Equals(PranaModules.TRADING_TICKET_MODULE))
                {
                    this.ultraTreePrefs.ActiveNode = newNode;
                    newNode.Selected = true;
                }
            }
            newNode = rootNode.Nodes.Add(GENERAL);
            newNode.Tag = GENERAL;
            if (this._prefsModuleName.Equals(GENERAL))
            {
                this.ultraTreePrefs.ActiveNode = newNode;
                newNode.Selected = true;
            }
            this.ultraTreePrefs.ExpandAll();
            this.ultraTreePrefs.AfterSelect += new Infragistics.Win.UltraWinTree.AfterNodeSelectEventHandler(ultraTreePrefs_AfterSelect);
        }

        private void EnableDisableButtons()
        {
            btnApply.Enabled = true;
            btnClose.Enabled = true;
            btnRestoreDefault.Enabled = true;
            btnSave.Enabled = true;
        }

        void ultraTreePrefs_AfterSelect(object sender, Infragistics.Win.UltraWinTree.SelectEventArgs e)
        {
            try
            {
                if (e.NewSelections.Count > 0)
                {
                    string SelectedPrefNode = (string)e.NewSelections[0].Tag;
                    if (!SelectedPrefNode.Equals(ROOT_NODE))
                        this.PrefsModuleName = SelectedPrefNode;
                }
                toolStripStatusLabel1.Text = "";
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

        private void LaunchGeneralPreferences()
        {
            try
            {
                prefsInstance = null;
                prefsInstance = (IPreferences)new GeneralPreferenceControl();

                prefsInstance.SetUp(_loginUser);

                prefsForm = prefsInstance.Reference();
                prefsForm.Dock = DockStyle.Fill;
                this._prefsData = this.prefsInstance.GetPrefs();
                this.pnlRight.ClientArea.Controls.Add(prefsForm);
                EnableDisableButtons();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
        }

        /// <summary>
        ///  Check Availability for PNL Module in availableModulesDetails.
        ///  Launch PNL Pref control.
        /// </summary>
        private void LaunchPreferences(string moduleName)
        {
            try
            {
                DynamicClass formToLoad;
                formToLoad = (DynamicClass)_availableModulesDetails[moduleName];

                Assembly asmAssemblyContainingForm = Assembly.LoadFrom(formToLoad.Location);

                Type typeToLoad = asmAssemblyContainingForm.GetType(formToLoad.PrefControlType);

                prefsInstance = null;
                prefsInstance = (IPreferences)Activator.CreateInstance(typeToLoad);

                prefsInstance.SetUp(_loginUser);

                //Special Handing for Position Management UI and Trading ticket because they do not use the SaveCliked event
                //modified by: Raturi, 24 Apr 2015
                //added the blotter module in the ignored module list as this was causing issue
                if (moduleName != PranaModules.PORTFOLIO_MANAGEMENT_MODULE && moduleName != PranaModules.TRADING_TICKET_MODULE && moduleName != PranaModules.BLOTTER_MODULE && moduleName != PranaModules.PERCENT_TRADING_TOOL && moduleName != PranaModules.SHORTLOCATE_MODULE && moduleName != GENERAL)
                {
                    ((IPreferencesSavedClicked)prefsInstance).SaveClicked += new EventHandler(prefsInstance_SaveClicked);
                }

                prefsForm = prefsInstance.Reference();
                prefsForm.Dock = DockStyle.Fill;
                this._prefsData = this.prefsInstance.GetPrefs();
                this.pnlRight.ClientArea.Controls.Add(prefsForm);
                EnableDisableButtons();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
        }

        void prefsInstance_SaveClicked(object sender, EventArgs e)
        {
            btnSave_Click(sender, null);
        }

        /// <summary>
        /// Unloads the PNL UserControl
        /// </summary>
        private void UnloadPrefControl()
        {
            if (prefsInstance != null)
            {
                //if(MessageBox.Show("Do you want to save the Prefs changes?", "Confirmation",System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == DialogResult.Yes)
                //	{
                //		prefsInstance.Save();
                //	}
                this.pnlRight.ClientArea.Controls.Remove(prefsForm);
                prefsForm.Dispose();
            }
            prefsInstance = null;
        }

        #endregion

        private void ultraTreePrefs_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {

        }

        private void ultraTreePrefs_AfterActivate(object sender, Infragistics.Win.UltraWinTree.NodeEventArgs e)
        {
            if (e.TreeNode.Level == 0 || e.TreeNode.Tag == null) return;

            switch ((string)e.TreeNode.Tag)
            {
                case PranaModules.BLOTTER_MODULE:
                case PranaModules.TRADING_TICKET_MODULE:
                case Global.ApplicationConstants.APP_NAME:
                case PranaModules.PORTFOLIO_MANAGEMENT_MODULE:
                case PranaModules.GENERAL_LEDGER_MODULE:
                case PranaModules.CLOSING_PREFS_MODULE:
                case PranaModules.PERCENT_TRADING_TOOL:
                case PranaModules.SHORTLOCATE_MODULE:
                case PranaModules.POSITION_MANAGEMENT_PREFS_MODULE:
                    UnloadPrefControl();
                    LaunchPreferences((string)e.TreeNode.Tag);
                    break;
                case GENERAL:
                    UnloadPrefControl();
                    LaunchGeneralPreferences();
                    break;
            }
            if (((string)e.TreeNode.Tag).Equals(PranaModules.TRADING_TICKET_MODULE) || ((string)e.TreeNode.Tag).Equals(PranaModules.PERCENT_TRADING_TOOL) || ((string)e.TreeNode.Tag).Equals(PranaModules.SHORTLOCATE_MODULE) || ((string)e.TreeNode.Tag).Equals(GENERAL))
            {
                btnRestoreDefault.Visible = false;
                btnApply.Visible = false;
            }
            else
            {
                btnRestoreDefault.Visible = true;
                btnApply.Visible = true;
            }

            CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_PREFERENCES);
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            try
            {
                //Ask user to save the prefs and close the window
                if (ApplyPrefsClick != null && _prefsData != null)
                {
                    ApplyPrefsClick(this, new EventArgs<string, IPreferenceData>((string)this.ultraTreePrefs.ActiveNode.Tag, this._prefsData));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
            finally
            {
                this.Close();
            }
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            //Save Prefs
            try
            {
                if (this.prefsInstance != null)
                {
                    bool success = this.prefsInstance.Save();

                    if (!success)
                    {
                        toolStripStatusLabel1.Text = "Preferences not Saved in " + PrefsModuleName;
                        return;
                    }

                    this._prefsData = this.prefsInstance.GetPrefs();

                    if (_prefsData != null && ApplyPrefsClick != null)
                        ApplyPrefsClick(this, new EventArgs<string, IPreferenceData>(PrefsModuleName, this._prefsData));

                    toolStripStatusLabel1.Text = "Preferences Saved in " + PrefsModuleName;
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }

        }

        private void btnRestoreDefault_Click(object sender, System.EventArgs e)
        {
            if (this.prefsInstance != null)
            {
                this.prefsInstance.RestoreDefault();
                toolStripStatusLabel1.Text = "Default Preferences Stored in " + PrefsModuleName;
            }
        }

        private void PreferencesMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //SK 20061101 There is no need to apply preferences while closing pref main window
            //if(ApplyPrefsClick != null)
            //    ApplyPrefsClick((string)this.ultraTreePrefs.ActiveNode.Tag, this._prefsData);

            if (this.PreferencesClosed != null)
                this.PreferencesClosed(sender, e);
        }

        private void btnApply_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (this.prefsInstance != null)
                {
                    IPreferenceData prefDataLocal = this.prefsInstance.GetPrefs();
                    if (prefDataLocal != null && ApplyPrefsClick != null)
                        ApplyPrefsClick(this, new EventArgs<string, IPreferenceData>(PrefsModuleName, prefDataLocal));
                    toolStripStatusLabel1.Text = "Preferences Applied in " + PrefsModuleName;
                }
                else
                {
                    toolStripStatusLabel1.Text = "No preferences to apply";
                }
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

        private void PreferencesMain_Load(object sender, EventArgs e)
        {
            try
            {
                ChangeIconAccordingToTheme();

                CustomThemeHelper.SetThemeProperties(sender as Form, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_PREFERENCES);
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
                    this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(88, 88, 90);
                    this.statusStrip1.ForeColor = System.Drawing.Color.WhiteSmoke;
                    this.toolStripStatusLabel1.BackColor = System.Drawing.Color.FromArgb(88, 88, 90);
                    this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.WhiteSmoke;
                    this.toolStripStatusLabel1.Font = new Font("Century Gothic", 9F);
                }
                if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
                }
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

        private void SetButtonsColor()
        {
            try
            {
                btnRestoreDefault.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnRestoreDefault.ForeColor = System.Drawing.Color.White;
                btnRestoreDefault.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnRestoreDefault.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnRestoreDefault.UseAppStyling = false;
                btnRestoreDefault.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnApply.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnApply.ForeColor = System.Drawing.Color.White;
                btnApply.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnApply.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnApply.UseAppStyling = false;
                btnApply.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnSave.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnSave.ForeColor = System.Drawing.Color.White;
                btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSave.UseAppStyling = false;
                btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnClose.BackColor = System.Drawing.Color.FromArgb(140, 5, 5);
                btnClose.ForeColor = System.Drawing.Color.White;
                btnClose.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnClose.UseAppStyling = false;
                btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        private void ChangeIconAccordingToTheme()
        {
            try
            {
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PreferencesMain));
                if (CustomThemeHelper.ApplyTheme)
                    this.Icon = ((System.Drawing.Icon)(resources.GetObject("PreferencesIconWithTheme")));
                else
                    this.Icon = ((System.Drawing.Icon)(resources.GetObject("PreferencesIconWithoutTheme")));
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
        /// Returns securityMasterObject
        /// </summary>
        public static ISecurityMasterServices GetSecurityMaster()
        {
            return securityMasterObject;
        }
    }

    //public delegate void SaveClickHandler(object sender, StringEventArgs e);
    public delegate void ApplyPreferenceHandler(object sender, EventArgs<string, IPreferenceData> e);

}
