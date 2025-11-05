using Prana.Admin.BLL;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.ThirdParty;
using Prana.BusinessObjects.Enums;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.ThirdPartyManager.DataAccess;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.Admin
{
    /// <summary>
    /// Summary description for ThirdParty.
    /// </summary>
    public class ThirdPartyForm : System.Windows.Forms.Form
    {
        #region Constant Definitions
        private const string FORM_NAME = "ThirdPartyForm: ";
        //Tab Constants
        const int C_TAB_THIRDPARTY = 0;
        const int C_TAB_COMMISSIONS = 1;

        //Third party type constants
        const int C_TYPE_PRIMEBROKERCLEARER = 1;
        const int C_TYPE_VENDOR = 2;
        //const int C_TYPE_CUSTODIAN = 3;
        //const int C_TYPE_ADMINISTRATOR = 4;
        const int C_TYPE_EXECUTINGBROKER = 3;
        const int C_TYPE_ALLDATAPARTIES = 4;

        const string C_COMBO_SELECT = "- Select -";
        #endregion

        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TreeView trvThirdParty;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Panel panel1;
        private Prana.Admin.Controls.ThirdPartyForm uctThirdPartyContactDetails;
        private Infragistics.Win.Misc.UltraPanel panelThirdParty;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager ultraToolbarsManager1;
        private Infragistics.Win.Misc.UltraPanel ThirdPartyForm_Fill_Panel;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _ThirdPartyForm_Toolbars_Dock_Area_Left;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _ThirdPartyForm_Toolbars_Dock_Area_Right;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _ThirdPartyForm_Toolbars_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _ThirdPartyForm_Toolbars_Dock_Area_Top;
        private IContainer components;
        //private Prana.Admin.Controls.ThirdPartyForm uctThirdPartyContactDetails;

        public ThirdPartyForm()
        {
            AuditManager.BLL.AuditHandler.GetInstance().SetUIonPermission(false, int.MinValue);

            InitializeComponent();
            //Binds the tree as soon as the class constructor is called.
            BindThirdPartyTree();
            SetUpMenuPermissions();
            ApplyTheme();
        }

        /// <summary>
        /// Applying Black Gray Theme
        /// </summary>
        private void ApplyTheme()
        {
            try
            {
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.trvThirdParty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                    this.trvThirdParty.ForeColor = System.Drawing.Color.White;
                    this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                    uctThirdPartyContactDetails.ApplyTheme();

                    this.MaximizeBox = true;
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

        private bool chkAddThirdParties = false;
        private bool chkDeleteThirdParties = false;
        private bool chkEditThirdParties = false;
        //This method fetches the user permissions from the database.
        private void SetUpMenuPermissions()
        {
            // To set permission for third party UI
            ModuleResources module = ModuleResources.Third_Party;
            AuthAction action = AuthAction.Write;
            var hasAccess = AuthorizationManager.GetInstance().CheckAccesibilityForMoldule(module, action);
            if (hasAccess)
            {
                chkAddThirdParties = true;
                chkDeleteThirdParties = true;
                chkEditThirdParties = true;
            }
            //Preferences preferences = Preferences.Instance;
            //chkAddThirdParties = preferences.Add_ThirdParties;
            //chkDeleteThirdParties = preferences.Delete_ThirdParties;
            //chkEditThirdParties = preferences.Edit_ThirdParties;
            //If the user doesnt have the permissions to add or delete ThirdParties then the respecive Add or Delete buttons are
            //disabled so that he/she can't add or delete the ThirdParties.
            if (chkAddThirdParties == false)
            {
                btnAdd.Enabled = false;
            }
            if (chkDeleteThirdParties == false)
            {
                btnDelete.Enabled = false;
            }
            if (chkEditThirdParties == false)
            {
                btnSave.Enabled = false;
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
                if (btnDelete != null)
                {
                    btnDelete.Dispose();
                }
                if (btnClose != null)
                {
                    btnClose.Dispose();
                }
                if (btnSave != null)
                {
                    btnSave.Dispose();
                }
                if (btnAdd != null)
                {
                    btnAdd.Dispose();
                }
                if (trvThirdParty != null)
                {
                    trvThirdParty.Dispose();
                }
                if (splitContainer1 != null)
                {
                    splitContainer1.Dispose();
                }
                if (splitContainer2 != null)
                {
                    splitContainer2.Dispose();
                }
                if (panel1 != null)
                {
                    panel1.Dispose();
                }
                if (uctThirdPartyContactDetails != null)
                {
                    uctThirdPartyContactDetails.Dispose();
                }
                if (panelThirdParty != null)
                {
                    panelThirdParty.Dispose();
                }
                if (ultraToolbarsManager1 != null)
                {
                    ultraToolbarsManager1.Dispose();
                }
                if (ThirdPartyForm_Fill_Panel != null)
                {
                    ThirdPartyForm_Fill_Panel.Dispose();
                }
                if (_ThirdPartyForm_Toolbars_Dock_Area_Bottom != null)
                {
                    _ThirdPartyForm_Toolbars_Dock_Area_Bottom.Dispose();
                }
                if (_ThirdPartyForm_Toolbars_Dock_Area_Left != null)
                {
                    _ThirdPartyForm_Toolbars_Dock_Area_Left.Dispose();
                }
                if (_ThirdPartyForm_Toolbars_Dock_Area_Right != null)
                {
                    _ThirdPartyForm_Toolbars_Dock_Area_Right.Dispose();
                }
                if (_ThirdPartyForm_Toolbars_Dock_Area_Top != null)
                {
                    _ThirdPartyForm_Toolbars_Dock_Area_Top.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ThirdPartyForm));
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.trvThirdParty = new System.Windows.Forms.TreeView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelThirdParty = new Infragistics.Win.Misc.UltraPanel();
            this.uctThirdPartyContactDetails = new Prana.Admin.Controls.ThirdPartyForm();
            this.ultraToolbarsManager1 = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this.ThirdPartyForm_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this._ThirdPartyForm_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._ThirdPartyForm_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._ThirdPartyForm_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._ThirdPartyForm_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.panel1.SuspendLayout();
            this.panelThirdParty.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.ThirdPartyForm_Fill_Panel.ClientArea.SuspendLayout();
            this.ThirdPartyForm_Fill_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(252)))), ((int)(((byte)(202)))));
            this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDelete.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnDelete.Location = new System.Drawing.Point(80, 5);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.btnAdd.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAdd.BackgroundImage")));
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAdd.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnAdd.Location = new System.Drawing.Point(0, 5);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnClose.Location = new System.Drawing.Point(580, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(204)))), ((int)(((byte)(102)))));
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSave.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnSave.Location = new System.Drawing.Point(499, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(1041, 492);
            this.splitContainer1.SplitterDistance = 450;
            this.splitContainer1.TabIndex = 7;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Vertical;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.trvThirdParty);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.panelThirdParty);
            this.splitContainer2.Size = new System.Drawing.Size(1041, 466);
            this.splitContainer2.SplitterDistance = 156;
            this.splitContainer2.TabIndex = 0;
            // 
            // trvThirdParty
            //            
            this.trvThirdParty.BackColor = System.Drawing.Color.White;
            this.trvThirdParty.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.trvThirdParty.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trvThirdParty.FullRowSelect = true;
            this.trvThirdParty.HideSelection = false;
            this.trvThirdParty.Location = new System.Drawing.Point(0, 0);
            this.trvThirdParty.Name = "trvThirdParty";
            this.trvThirdParty.Size = new System.Drawing.Size(156, 466);
            this.trvThirdParty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trvThirdParty.TabIndex = 7;
            this.trvThirdParty.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvThirdParty_AfterSelect);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(999, 34);
            this.panel1.TabIndex = 9;
            // 
            // panelThirdParty
            // 
            this.panelThirdParty.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelThirdParty.ClientArea.Controls.Add(this.uctThirdPartyContactDetails);
            this.panelThirdParty.Location = new System.Drawing.Point(0, 0);
            this.panelThirdParty.Name = "panelThirdParty";
            this.panelThirdParty.Size = new System.Drawing.Size(871, 439);
            this.panelThirdParty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelThirdParty.TabIndex = 10;
            // 
            // ultraToolbarsManager1
            // 
            this.ultraToolbarsManager1.DesignerFlags = 1;
            this.ultraToolbarsManager1.DockWithinContainer = this;
            this.ultraToolbarsManager1.DockWithinContainerBaseType = typeof(System.Windows.Forms.Form);
            this.ultraToolbarsManager1.FormDisplayStyle = Infragistics.Win.UltraWinToolbars.FormDisplayStyle.RoundedSizable;
            this.ultraToolbarsManager1.IsGlassSupported = false;
            // 
            // ThirdPartyForm_Fill_Panel
            // 
            // 
            // ThirdPartyForm_Fill_Panel.ClientArea
            // 
            this.ThirdPartyForm_Fill_Panel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ThirdPartyForm_Fill_Panel.ClientArea.Controls.Add(this.splitContainer1);
            this.ThirdPartyForm_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.ThirdPartyForm_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ThirdPartyForm_Fill_Panel.Location = new System.Drawing.Point(4, 27);
            this.ThirdPartyForm_Fill_Panel.Name = "ThirdPartyForm_Fill_Panel";
            this.ThirdPartyForm_Fill_Panel.Size = new System.Drawing.Size(999, 473);
            this.ThirdPartyForm_Fill_Panel.TabIndex = 0;
            // 
            // _ThirdPartyForm_Toolbars_Dock_Area_Left
            // 
            this._ThirdPartyForm_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ThirdPartyForm_Toolbars_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ThirdPartyForm_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._ThirdPartyForm_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ThirdPartyForm_Toolbars_Dock_Area_Left.InitialResizeAreaExtent = 4;
            this._ThirdPartyForm_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 27);
            this._ThirdPartyForm_Toolbars_Dock_Area_Left.Name = "_ThirdPartyForm_Toolbars_Dock_Area_Left";
            this._ThirdPartyForm_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(4, 473);
            this._ThirdPartyForm_Toolbars_Dock_Area_Left.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _ThirdPartyForm_Toolbars_Dock_Area_Right
            // 
            this._ThirdPartyForm_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ThirdPartyForm_Toolbars_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ThirdPartyForm_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._ThirdPartyForm_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ThirdPartyForm_Toolbars_Dock_Area_Right.InitialResizeAreaExtent = 4;
            this._ThirdPartyForm_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(1003, 27);
            this._ThirdPartyForm_Toolbars_Dock_Area_Right.Name = "_ThirdPartyForm_Toolbars_Dock_Area_Right";
            this._ThirdPartyForm_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(4, 473);
            this._ThirdPartyForm_Toolbars_Dock_Area_Right.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _ThirdPartyForm_Toolbars_Dock_Area_Top
            // 
            this._ThirdPartyForm_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ThirdPartyForm_Toolbars_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ThirdPartyForm_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._ThirdPartyForm_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ThirdPartyForm_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._ThirdPartyForm_Toolbars_Dock_Area_Top.Name = "_ThirdPartyForm_Toolbars_Dock_Area_Top";
            this._ThirdPartyForm_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(1007, 27);
            this._ThirdPartyForm_Toolbars_Dock_Area_Top.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _ThirdPartyForm_Toolbars_Dock_Area_Bottom
            // 
            this._ThirdPartyForm_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ThirdPartyForm_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ThirdPartyForm_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._ThirdPartyForm_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ThirdPartyForm_Toolbars_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
            this._ThirdPartyForm_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 500);
            this._ThirdPartyForm_Toolbars_Dock_Area_Bottom.Name = "_ThirdPartyForm_Toolbars_Dock_Area_Bottom";
            this._ThirdPartyForm_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(1007, 4);
            this._ThirdPartyForm_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // uctThirdPartyContactDetails
            // 
            this.uctThirdPartyContactDetails.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.uctThirdPartyContactDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uctThirdPartyContactDetails.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.uctThirdPartyContactDetails.Location = new System.Drawing.Point(0, 0);
            this.uctThirdPartyContactDetails.Name = "uctThirdPartyContactDetails";
            this.uctThirdPartyContactDetails.Size = new System.Drawing.Size(879, 470);
            this.uctThirdPartyContactDetails.TabIndex = 10;
            this.uctThirdPartyContactDetails.Load += new System.EventHandler(this.uctThirdPartyContactDetails_Load);
            // 
            // ThirdPartyForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(1007, 534);
            this.Controls.Add(this.ThirdPartyForm_Fill_Panel);
            this.Controls.Add(this._ThirdPartyForm_Toolbars_Dock_Area_Left);
            this.Controls.Add(this._ThirdPartyForm_Toolbars_Dock_Area_Right);
            this.Controls.Add(this._ThirdPartyForm_Toolbars_Dock_Area_Bottom);
            this.Controls.Add(this._ThirdPartyForm_Toolbars_Dock_Area_Top);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(870, 535);
            this.Name = "ThirdPartyForm";
            this.Text = "ThirdParty";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ThirdPartyForm_FormClosing);
            this.Load += new System.EventHandler(this.ThirdPartyForm_Load);
            this.panel1.ResumeLayout(false);
            this.panelThirdParty.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ThirdPartyForm_Fill_Panel.ClientArea.ResumeLayout(false);
            this.ThirdPartyForm_Fill_Panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// This method deletes the selected thirdparty or vendor.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, System.EventArgs e)
        {
            try
            {
                string result = string.Empty;
                if (trvThirdParty.SelectedNode.Parent != null)
                {
                    ApplicationConstants.ThirdPartyNodeType nodeType = (ApplicationConstants.ThirdPartyNodeType)Enum.Parse(typeof(ApplicationConstants.ThirdPartyNodeType), trvThirdParty.SelectedNode.Parent.Text);
                    if (trvThirdParty.SelectedNode != null)
                    {
                        NodeDetails prevNodeDetails = new NodeDetails();
                        if (trvThirdParty.SelectedNode.PrevNode != null)
                        {
                            prevNodeDetails = (NodeDetails)trvThirdParty.SelectedNode.PrevNode.Tag;
                        }
                        else
                        {
                            prevNodeDetails = (NodeDetails)trvThirdParty.SelectedNode.Parent.Tag;
                        }

                        NodeDetails nodeDetails = (NodeDetails)trvThirdParty.SelectedNode.Tag;

                        int thirdPartyID = nodeDetails.NodeID;
                        if (MessageBox.Show(this, "Do you want to delete selected Third Party?", "Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            bool isPermanentDeletion = CachedDataManager.GetInstance.IsPermanentDeletionEnabled();
                            result = ThirdPartyDataManager.DeleteThirdParty(thirdPartyID, isPermanentDeletion);
                            if (result != string.Empty)
                            {
                                MessageBox.Show(this, result, "Alert");
                            }
                            else
                            {
                                BindThirdPartyTree();
                                SelectTreeNode(nodeType, prevNodeDetails);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            finally
            {
                #region LogEntry
                Logger.LoggerWrite("button1_Click",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "button1_Click", null);

                #endregion
            }
        }

        /// <summary>
        /// Binds left tree with relevent data.
        /// </summary>
        private void BindThirdPartyTree()
        {
            try
            {
                bool gotFirstNode = false;
                Font font = new Font("Vedana", 8.25F, System.Drawing.FontStyle.Bold);

                //Creating PrimeBrokerClearer a root node.
                TreeNode treeNodePrimeBrokerClearerRoot = new TreeNode(ApplicationConstants.ThirdPartyNodeType.PrimeBrokerClearer.ToString());
                //Making the root node to bold by assigning it to the font object defined above. 
                treeNodePrimeBrokerClearerRoot.NodeFont = font;
                NodeDetails primeBrokerClearerNode = new NodeDetails(ApplicationConstants.ThirdPartyNodeType.PrimeBrokerClearer, int.MinValue);
                treeNodePrimeBrokerClearerRoot.Tag = primeBrokerClearerNode;

                ////Creating Vendor a root node.
                //TreeNode treeNodeVendorRoot = new TreeNode("Vendor");
                ////Making the root node to bold by assigning it to the font object defined above. 
                //treeNodeVendorRoot.NodeFont = font;
                //NodeDetails vendorNode = new NodeDetails(NodeType.Vendor, int.MinValue); 
                //treeNodeVendorRoot.Tag = vendorNode;

                //Creating Custodian a root node.
                TreeNode treeNodeCustodianRoot = new TreeNode(ApplicationConstants.ThirdPartyNodeType.ExecutingBroker.ToString());
                //Making the root node to bold by assigning it to the font object defined above. 
                treeNodeCustodianRoot.NodeFont = font;
                NodeDetails custodianNode = new NodeDetails(ApplicationConstants.ThirdPartyNodeType.ExecutingBroker, int.MinValue);
                treeNodeCustodianRoot.Tag = custodianNode;

                //Creating Administrator a root node.
                TreeNode treeNodeAdministratorRoot = new TreeNode(ApplicationConstants.ThirdPartyNodeType.AllDataParties.ToString());
                //Making the root node to bold by assigning it to the font object defined above. 
                treeNodeAdministratorRoot.NodeFont = font;
                NodeDetails administratorNode = new NodeDetails(ApplicationConstants.ThirdPartyNodeType.AllDataParties, int.MinValue);
                treeNodeAdministratorRoot.Tag = administratorNode;

                trvThirdParty.Nodes.Clear();

                ThirdParties thirdParties = ThirdPartyDataManager.GetAllThirdPartiesForTree();

                int entityType = int.MinValue;
                //Loop through all the ThirdParties and Vendors, assigning each node an id 
                //corresponding to its unique value in the database.
                foreach (ThirdParty thirdParty in thirdParties)
                {
                    if (gotFirstNode.Equals(false))
                    {
                        gotFirstNode = true;
                        entityType = int.Parse(thirdParty.ThirdPartyTypeID.ToString());
                    }
                    switch (thirdParty.ThirdPartyTypeID)
                    {
                        case (int)ApplicationConstants.ThirdPartyNodeType.PrimeBrokerClearer:
                            TreeNode treeNodePrimeBrokerClearer = new TreeNode(thirdParty.ShortName);
                            primeBrokerClearerNode = new NodeDetails(ApplicationConstants.ThirdPartyNodeType.PrimeBrokerClearer, thirdParty.ThirdPartyID);
                            treeNodePrimeBrokerClearer.Tag = primeBrokerClearerNode;
                            treeNodePrimeBrokerClearerRoot.Nodes.Add(treeNodePrimeBrokerClearer);
                            break;
                        //case C_TYPE_VENDOR:
                        //    TreeNode treeNodeVendor = new TreeNode(thirdParty.ShortName);
                        //    vendorNode = new NodeDetails(NodeType.Vendor, thirdParty.ThirdPartyID); 
                        //    treeNodeVendor.Tag = vendorNode;
                        //    treeNodeVendorRoot.Nodes.Add(treeNodeVendor);
                        //    break;
                        case (int)ApplicationConstants.ThirdPartyNodeType.ExecutingBroker:
                            TreeNode treeNodeCustodian = new TreeNode(thirdParty.ShortName);
                            custodianNode = new NodeDetails(ApplicationConstants.ThirdPartyNodeType.ExecutingBroker, thirdParty.ThirdPartyID);
                            treeNodeCustodian.Tag = custodianNode;
                            treeNodeCustodianRoot.Nodes.Add(treeNodeCustodian);
                            break;
                        case (int)ApplicationConstants.ThirdPartyNodeType.AllDataParties:
                            TreeNode treeNodeAdministrator = new TreeNode(thirdParty.ShortName);
                            administratorNode = new NodeDetails(ApplicationConstants.ThirdPartyNodeType.AllDataParties, thirdParty.ThirdPartyID);
                            treeNodeAdministrator.Tag = administratorNode;
                            treeNodeAdministratorRoot.Nodes.Add(treeNodeAdministrator);
                            break;
                    }
                }
                trvThirdParty.Nodes.Add(treeNodePrimeBrokerClearerRoot);
                //trvThirdParty.Nodes.Add(treeNodeVendorRoot);
                trvThirdParty.Nodes.Add(treeNodeCustodianRoot);
                trvThirdParty.Nodes.Add(treeNodeAdministratorRoot);

                trvThirdParty.ExpandAll();
                if (thirdParties.Count > 0 && gotFirstNode == true)
                {

                    NodeDetails selectNodeDetails = new NodeDetails(ApplicationConstants.ThirdPartyNodeType.PrimeBrokerClearer, 1);
                    if (trvThirdParty.Nodes[0].Nodes.Count > 0)
                    {
                        trvThirdParty.SelectedNode = trvThirdParty.Nodes[0].Nodes[0];
                    }
                    else if (trvThirdParty.Nodes[1].Nodes.Count > 0)
                    {
                        trvThirdParty.SelectedNode = trvThirdParty.Nodes[1].Nodes[0];
                    }
                    else
                    {
                        trvThirdParty.SelectedNode = trvThirdParty.Nodes[0];
                    }
                    //SelectTreeNode(selectNodeDetails);
                }
                else
                {
                    trvThirdParty.SelectedNode = trvThirdParty.Nodes[0];
                }
            }
            #region Catch
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            #endregion

            finally
            {
                #region LogEntry
                Logger.LoggerWrite("BindThirdPartyTree",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "BindThirdPartyTree", null);

                #endregion
            }
        }

        /// <summary>
        /// Validation to Save the records
        /// </summary>
        /// <returns></returns>
        private bool ValidateRule()
        {
            bool status = true;
            // check for thirdparty detail section
            ThirdParty thirdParty = new ThirdParty();
            if (!uctThirdPartyContactDetails.GetThirdPartyDetailsForSave(thirdParty))
            {
                status = false;
                return status;
            }

            //check for File Format Section
            ThirdPartyFileFormats thirdPartyFileFormats = new ThirdPartyFileFormats();
            thirdPartyFileFormats = uctThirdPartyContactDetails.thirdPartyFileFormatProperties;
            if (thirdPartyFileFormats == null)
            {
                status = false;
                return status;
            }
            return status;
        }

        /// <summary>
        /// This method saves the Thirdparty details or the vendor details as per the selected node in the tree.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, System.EventArgs e)
        {
            try
            {
                // validation check 
                bool validateRule = ValidateRule();
                if (!validateRule)
                {
                    return;
                }

                ThirdParty thirdParty = new ThirdParty();

                int result = int.MinValue;
                if (uctThirdPartyContactDetails.GetThirdPartyDetailsForSave(thirdParty) != false)
                {                  
                    thirdParty.ThirdPartyID = ((NodeDetails)trvThirdParty.SelectedNode.Tag).NodeID;
                    result = ThirdPartyDataManager.SaveThirdParty(thirdParty);

                    if (result == -1)
                    {
                        MessageBox.Show("ThirdParty with the same short name already exists.", "Prana Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    // Added by Ankit Gupta on 10 Oct, 2014.
                    // http://jira.nirvanasolutions.com:8080/browse/CHMW-1595
                    // If a third party is deleted, it should not be removed from Database, instead it should be marked as Inactive.
                    else if (result == -2)
                    {
                        MessageBox.Show("There is already a Third Party with the same name but in Inactive State", "Prana Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (result > 0)
                    {
                        ThirdPartyFileFormats thirdPartyFileFormats = new ThirdPartyFileFormats();
                        thirdPartyFileFormats = uctThirdPartyContactDetails.thirdPartyFileFormatProperties;
                        if (thirdPartyFileFormats != null)
                        {
                            int intFileFormatId = int.Parse(ThirdPartyDataManager.SaveThirdPartyFileFormat(thirdPartyFileFormats, result).ToString());
                            if (intFileFormatId < 0)
                                return;
                        }
                        else
                        {
                            return;
                        }

                        int i = uctThirdPartyContactDetails.SaveThirdPartyDetails();
                        // Modified by Ankit Gupta on 20 Nov, 2014.
                        // http://jira.nirvanasolutions.com:8080/browse/CHMW-1933
                        if (i > 0)
                        {
                            ApplicationConstants.ThirdPartyNodeType nodeType = (ApplicationConstants.ThirdPartyNodeType)thirdParty.ThirdPartyTypeID;
                            NodeDetails selectNodeDetails = new NodeDetails(nodeType, result);
                            BindThirdPartyTree();
                            SelectTreeNode(nodeType, selectNodeDetails);
                            uctThirdPartyContactDetails.SaveThirdPartyAuditDetails(thirdParty, result);
                            MessageBox.Show("Third Party Details Saved", "Success Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            finally
            {
                #region LogEntry
                Logger.LoggerWrite("btnSave_Click",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "btnSave_Click", null);

                #endregion
            }
        }

        /// <summary>
        /// This method closes the application on clicking of this button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// This method shows the details of the selected node on the click event of the tree. It fetches the
        /// details of the selected node from the database by sending the nodeID.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trvThirdParty_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            try
            {
                NodeDetails nodeDetails = (NodeDetails)trvThirdParty.SelectedNode.Tag;
                if (trvThirdParty.SelectedNode == null)
                {
                    //Prana.Admin.Utility.Common.ResetStatusPanel(stbThirdParty);
                    //Prana.Admin.Utility.Common.SetStatusPanel(stbThirdParty, "Please select ThirdParty to be shown with the details!");
                }
                else
                {
                    uctThirdPartyContactDetails.SetupControl();
                    //tbcThirdParty.SelectedTab = tbcThirdParty.Tabs[C_TAB_THIRDPARTY];
                    int thirdPartyID = nodeDetails.NodeID;
                    string thirdPartyName = trvThirdParty.SelectedNode.Text;

                    //Modified by Omshiv, get permission to set file settings for Batch
                    ModuleResources module = ModuleResources.ImportFileSetupTab;
                    AuthAction action = AuthAction.Write;
                    var hasAccess = AuthorizationManager.GetInstance().CheckAccesibilityForMoldule(module, action);
                    uctThirdPartyContactDetails.InitializeThirdPartyControls(thirdPartyID, thirdPartyName, hasAccess);

                    ThirdParty thirdParty = ThirdPartyDataManager.GetThirdParty(thirdPartyID);

                    uctThirdPartyContactDetails.SetThirdPartyDetails(thirdParty);

                    // Get all File Formats if there
                    ThirdPartyFileFormats thirdPartyFileFormats = ThirdPartyDataManager.GetThirdPartyFileFormats(thirdPartyID);
                    if (thirdPartyFileFormats.Count > 0)
                    {
                        // Retrieve third-party timed batches for the specified thirdPartyID
                        Dictionary<int, List<ThirdPartyTimeBatch>> thirdPartyTimedBatches = ThirdPartyDataManager.GetThirdPartyTimedBatches(thirdPartyID);

                        foreach (ThirdPartyFileFormat fileFormat in thirdPartyFileFormats)
                        {
                            if (thirdPartyTimedBatches.ContainsKey(fileFormat.FileFormatId))
                            {
                                fileFormat.TimeBatches = thirdPartyTimedBatches[fileFormat.FileFormatId];
                            }
                        }
                        uctThirdPartyContactDetails.thirdPartyFileFormatProperties = thirdPartyFileFormats;
                    }
                    else
                    {
                        uctThirdPartyContactDetails.BindFileFormatGrid(0);
                    }
                    //tabThirdParty.Show();
                    if (chkAddThirdParties == false && nodeDetails.NodeID == int.MinValue)
                    {
                        uctThirdPartyContactDetails.DisableThirdPartyControls();
                    }
                    else
                    {
                        uctThirdPartyContactDetails.EnableThirdPartyControls();
                    }
                }
            }
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            finally
            {
                #region LogEntry
                Logger.LoggerWrite("trvThirdParty_AfterSelect",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "trvThirdParty_AfterSelect", null);

                #endregion
            }
        }



        /// <summary>
        /// This method selects the node in the tree based on the parameter passed to it in nodedetails. 
        /// </summary>
        /// <param name="nodeDetails"></param>
        private void SelectTreeNode(ApplicationConstants.ThirdPartyNodeType thirdPartyType, NodeDetails nodeDetails)
        {
            int nodeIndex = 0;
            foreach (TreeNode node in trvThirdParty.Nodes)
            {
                if ((ApplicationConstants.ThirdPartyNodeType)Enum.Parse(typeof(ApplicationConstants.ThirdPartyNodeType), node.Text) == thirdPartyType)
                {
                    break;
                }
                nodeIndex++;
            }
            foreach (TreeNode node in trvThirdParty.Nodes[nodeIndex].Nodes)
            {
                if (((NodeDetails)node.Tag).NodeID == nodeDetails.NodeID)
                {
                    trvThirdParty.SelectedNode = node;
                    break;
                }
            }
        }

        private void btnAdd_Click(object sender, System.EventArgs e)
        {
            if (trvThirdParty.SelectedNode == null)
            {
            }
            else
            {
                NodeDetails nodeDetails = (NodeDetails)trvThirdParty.SelectedNode.Tag;
                uctThirdPartyContactDetails.RefreshThirdPartyDetails();

                //tbcThirdParty.SelectedTab = tbcThirdParty.Tabs[C_TAB_THIRDPARTY];
                //tbcThirdParty.Show();

                if (nodeDetails.NodeID != int.MinValue)
                {
                    trvThirdParty.SelectedNode = trvThirdParty.SelectedNode.Parent;
                }


            }
        }

        /// <summary>
        /// On Form Load, Hide the File Format Section tab, if the release is CHMiddleWare 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThirdPartyForm_Load(object sender, System.EventArgs e)
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_TRADING_TICKET);
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

        #region Highlight Selected Tab
        //To highlight and show the currently selected node in a different color.
        //private void tbcThirdParty_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
        //{
        //    Font f;
        //    Brush backBrush;
        //    Brush foreBrush;

        //    if(e.Index == this.tbcThirdParty.SelectedTab.Index)
        //    {
        //        f = new Font(e.Font, FontStyle.Regular);
        //        backBrush = new System.Drawing.SolidBrush(Color.Brown);
        //        backBrush = new System.Drawing.Drawing2D.LinearGradientBrush(e.Bounds, Color.Orange, Color.White, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
        //        foreBrush = Brushes.Black;
        //    }
        //    else
        //    {
        //        f = e.Font;
        //        backBrush = new SolidBrush(e.BackColor); 
        //        foreBrush = new SolidBrush(e.ForeColor);
        //    }

        //    string tabName = this.tbcThirdParty.Tabs[e.Index].Text;
        //    StringFormat sf = new StringFormat();
        //    sf.Alignment = StringAlignment.Center;
        //    e.Graphics.FillRectangle(backBrush, e.Bounds);
        //    Rectangle r = e.Bounds;
        //    r = new Rectangle(r.X, r.Y + 3, r.Width, r.Height - 3);
        //    e.Graphics.DrawString(tabName, f, foreBrush, r, sf);

        //    sf.Dispose();
        //    if(e.Index == this.tbcThirdParty.SelectedTab.Index)
        //    {
        //        f.Dispose();
        //        backBrush.Dispose();
        //    }
        //    else
        //    {
        //        backBrush.Dispose();
        //        foreBrush.Dispose();
        //    }
        //}	
        #endregion

        #region NodeDetails
        //Creating class NodeDetails to be used for the purpose of tree giving it some methods & properties.
        class NodeDetails
        {
            private ApplicationConstants.ThirdPartyNodeType _type = ApplicationConstants.ThirdPartyNodeType.PrimeBrokerClearer;
            private int _nodeID = int.MinValue;

            public NodeDetails()
            {
            }

            public NodeDetails(ApplicationConstants.ThirdPartyNodeType type, int nodeID)
            {
                _type = type;
                _nodeID = nodeID;
            }

            public ApplicationConstants.ThirdPartyNodeType Type
            {
                get { return _type; }
                set { _type = value; }
            }
            public int NodeID
            {
                get { return _nodeID; }
                set { _nodeID = value; }
            }
        }
        private void ThirdPartyForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                uctThirdPartyContactDetails.ultraTabControl1_SelectedTabChanged(null, null);
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
        private void uctThirdPartyContactDetails_Load(object sender, EventArgs e)
        {
            //Prana.PM.BLL.BatchSetupManager.InitializeData();
        }
        //Creating enumeration to be used to distinguish tree nodetype on the basis of ThirdParty/Vendor
        //enum NodeType
        //{
        //    PrimeBrokerClearer = 1,
        //    Vendor = 2,
        //    Custodian = 3,
        //    Administrator = 4
        //}

        //enum ThirdPartyNodeType
        //{
        //    PrimeBrokerClearer = 1,
        //    Vendor = 2,
        //    ExecutingBroker = 3,
        //    AllDataParties = 4
        //}

        #endregion

    }
}
