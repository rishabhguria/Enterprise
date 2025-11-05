using Prana.BusinessObjects;
using Prana.ThirdPartyReport;
namespace Prana.ThirdPartyUI
{
    partial class frmThirdPartyEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmThirdPartyEditor));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
            this.toleranceProfileAddNewItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnEmailSettings = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.btnFtpSettings = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnGnuPGSettings = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btnJobSettings = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.btnToleranceProfileSettings = new System.Windows.Forms.ToolStripButton();
            this.dataView = new System.Windows.Forms.DataGridView();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.thirdPartyGnuPGBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.thirdPartyFtpBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._frmThirdPartyEditor_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._frmThirdPartyEditor_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._frmThirdPartyEditor_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._frmThirdPartyEditor_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.thirdPartyBatchBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.thirdPartyGnuPGBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.thirdPartyFtpBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.thirdPartyBatchBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(4, 665);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(941, 22);
            this.inboxControlStyler1.SetStyleSettings(this.statusStrip1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // bindingNavigator1
            // 
            this.bindingNavigator1.AddNewItem = this.bindingNavigatorAddNewItem;
            this.bindingNavigator1.BindingSource = this.bindingSource1;
            this.bindingNavigator1.CountItem = this.bindingNavigatorCountItem;
            this.bindingNavigator1.DeleteItem = null;
            this.bindingNavigator1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2,
            this.bindingNavigatorAddNewItem,
            this.toleranceProfileAddNewItem,
            this.bindingNavigatorDeleteItem,
            this.toolStripSeparator2,
            this.toolStripButton1,
            this.toolStripSeparator1,
            this.btnEmailSettings,
            this.toolStripSeparator5,
            this.btnFtpSettings,
            this.toolStripSeparator3,
            this.btnGnuPGSettings,
            this.toolStripSeparator4,
            this.btnJobSettings,
            this.toolStripSeparator6,
            this.btnToleranceProfileSettings});
            this.bindingNavigator1.Location = new System.Drawing.Point(4, 27);
            this.bindingNavigator1.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.bindingNavigator1.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.bindingNavigator1.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.bindingNavigator1.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = this.bindingNavigatorPositionItem;
            this.bindingNavigator1.Size = new System.Drawing.Size(941, 25);
            this.inboxControlStyler1.SetStyleSettings(this.bindingNavigator1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.bindingNavigator1.TabIndex = 3;
            // 
            // bindingNavigatorAddNewItem
            // 
            this.bindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorAddNewItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorAddNewItem.Image")));
            this.bindingNavigatorAddNewItem.Name = "bindingNavigatorAddNewItem";
            this.bindingNavigatorAddNewItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorAddNewItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorAddNewItem.Text = "Add new";
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(35, 22);
            this.bindingNavigatorCountItem.Text = "of {0}";
            this.bindingNavigatorCountItem.ToolTipText = "Total number of items";
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveFirstItem.Text = "Move first";
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMovePreviousItem.Text = "Move previous";
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorPositionItem
            // 
            this.bindingNavigatorPositionItem.AccessibleName = "Position";
            this.bindingNavigatorPositionItem.AutoSize = false;
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(50, 23);
            this.bindingNavigatorPositionItem.Text = "0";
            this.bindingNavigatorPositionItem.ToolTipText = "Current position";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveNextItem.Text = "Move next";
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveLastItem.Text = "Move last";
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorDeleteItem
            // 
            this.bindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorDeleteItem.Image")));
            this.bindingNavigatorDeleteItem.Name = "bindingNavigatorDeleteItem";
            this.bindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorDeleteItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorDeleteItem.Text = "Delete";
            this.bindingNavigatorDeleteItem.Click += new System.EventHandler(this.bindingNavigatorDeleteItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(66, 22);
            this.toolStripButton1.Text = "Refresh";
            this.toolStripButton1.Click += new System.EventHandler(this.btnAutoCreate);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnEmailSettings
            // 
            this.btnEmailSettings.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnEmailSettings.Image = ((System.Drawing.Image)(resources.GetObject("btnEmailSettings.Image")));
            this.btnEmailSettings.Name = "btnEmailSettings";
            this.btnEmailSettings.Size = new System.Drawing.Size(101, 22);
            this.btnEmailSettings.Text = "Email Settings";
            this.btnEmailSettings.Click += new System.EventHandler(this.btnEmailSettings_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // btnFtpSettings
            // 
            this.btnFtpSettings.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnFtpSettings.Image = ((System.Drawing.Image)(resources.GetObject("btnFtpSettings.Image")));
            this.btnFtpSettings.Name = "btnFtpSettings";
            this.btnFtpSettings.Size = new System.Drawing.Size(89, 22);
            this.btnFtpSettings.Text = "Ftp Settings";
            this.btnFtpSettings.Click += new System.EventHandler(this.btnFtpSettings_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // btnGnuPGSettings
            // 
            this.btnGnuPGSettings.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnGnuPGSettings.Image = ((System.Drawing.Image)(resources.GetObject("btnGnuPGSettings.Image")));
            this.btnGnuPGSettings.Name = "btnGnuPGSettings";
            this.btnGnuPGSettings.Size = new System.Drawing.Size(129, 22);
            this.btnGnuPGSettings.Text = "Encryption Settings";
            this.btnGnuPGSettings.Click += new System.EventHandler(this.btnGnuPGSettings_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // btnJobSettings
            // 
            this.btnJobSettings.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnJobSettings.Image = ((System.Drawing.Image)(resources.GetObject("btnJobSettings.Image")));
            this.btnJobSettings.Name = "btnJobSettings";
            this.btnJobSettings.Size = new System.Drawing.Size(90, 22);
            this.btnJobSettings.Text = "Job Settings";
            this.btnJobSettings.Click += new System.EventHandler(this.btnJobSettings_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            //
            // ToleranceProfileAddNewItem
            //
            this.toleranceProfileAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toleranceProfileAddNewItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorAddNewItem.Image")));
            this.toleranceProfileAddNewItem.Name = "toleranceProfileAddNewItem";
            this.toleranceProfileAddNewItem.RightToLeftAutoMirrorImage = true;
            this.toleranceProfileAddNewItem.Size = new System.Drawing.Size(23, 22);
            this.toleranceProfileAddNewItem.Text = "Add new Tolerance Profile";
            this.toleranceProfileAddNewItem.Click += new System.EventHandler(this.toleranceProfileAddNewItem_Click);
            // 
            // btnToleranceProfileSettings
            // 
            this.btnToleranceProfileSettings.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnToleranceProfileSettings.Name = "btnToleranceProfileSettings";
            this.btnToleranceProfileSettings.Size = new System.Drawing.Size(90, 22);
            this.btnToleranceProfileSettings.Text = "Tolerance Profile";
            this.btnToleranceProfileSettings.Click += new System.EventHandler(this.btnToleranceProfileSettings_Click);
            // 
            // dataView
            // 
            this.dataView.AllowUserToDeleteRows = false;
            this.dataView.AllowUserToResizeRows = false;
            this.dataView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataView.Location = new System.Drawing.Point(4, 52);
            this.dataView.Name = "dataView";
            this.dataView.RowHeadersVisible = false;
            this.dataView.Size = new System.Drawing.Size(941, 613);
            this.inboxControlStyler1.SetStyleSettings(this.dataView, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.dataView.TabIndex = 4;
            this.dataView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataView_CellClick);
            this.dataView.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataView_CellEnter);
            this.dataView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataView_CellFormatting);
            this.dataView.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataView_CellPainting);
            this.dataView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataView_DataError);
            this.dataView.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataView_EditingControlShowing);
            this.dataView.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView1_RowValidating);
            this.dataView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataView_CellValueChanged);
            this.dataView.ColumnHeadersDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataView.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dataView.KeyDown += DataView_KeyDown;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(25, 42);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.inboxControlStyler1.SetStyleSettings(this.comboBox1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.comboBox1.TabIndex = 5;
            this.comboBox1.Visible = false;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            this.comboBox1.SelectionChangeCommitted += new System.EventHandler(this.comboBox1_SelectionChangeCommitted);
            // 
            // thirdPartyGnuPGBindingSource
            // 
            this.thirdPartyGnuPGBindingSource.DataSource = typeof(Prana.BusinessObjects.ThirdPartyGnuPG);
            // 
            // thirdPartyFtpBindingSource
            // 
            this.thirdPartyFtpBindingSource.DataSource = typeof(Prana.BusinessObjects.ThirdPartyFtp);
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _frmThirdPartyEditor_UltraFormManager_Dock_Area_Left
            // 
            this._frmThirdPartyEditor_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmThirdPartyEditor_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._frmThirdPartyEditor_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._frmThirdPartyEditor_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmThirdPartyEditor_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._frmThirdPartyEditor_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 4;
            this._frmThirdPartyEditor_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 27);
            this._frmThirdPartyEditor_UltraFormManager_Dock_Area_Left.Name = "_frmThirdPartyEditor_UltraFormManager_Dock_Area_Left";
            this._frmThirdPartyEditor_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(4, 660);
            // 
            // _frmThirdPartyEditor_UltraFormManager_Dock_Area_Right
            // 
            this._frmThirdPartyEditor_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmThirdPartyEditor_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._frmThirdPartyEditor_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._frmThirdPartyEditor_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmThirdPartyEditor_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._frmThirdPartyEditor_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 4;
            this._frmThirdPartyEditor_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(945, 27);
            this._frmThirdPartyEditor_UltraFormManager_Dock_Area_Right.Name = "_frmThirdPartyEditor_UltraFormManager_Dock_Area_Right";
            this._frmThirdPartyEditor_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(4, 660);
            // 
            // _frmThirdPartyEditor_UltraFormManager_Dock_Area_Top
            // 
            this._frmThirdPartyEditor_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmThirdPartyEditor_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._frmThirdPartyEditor_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._frmThirdPartyEditor_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmThirdPartyEditor_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._frmThirdPartyEditor_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._frmThirdPartyEditor_UltraFormManager_Dock_Area_Top.Name = "_frmThirdPartyEditor_UltraFormManager_Dock_Area_Top";
            this._frmThirdPartyEditor_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(949, 27);
            // 
            // _frmThirdPartyEditor_UltraFormManager_Dock_Area_Bottom
            // 
            this._frmThirdPartyEditor_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmThirdPartyEditor_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._frmThirdPartyEditor_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._frmThirdPartyEditor_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmThirdPartyEditor_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._frmThirdPartyEditor_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
            this._frmThirdPartyEditor_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 687);
            this._frmThirdPartyEditor_UltraFormManager_Dock_Area_Bottom.Name = "_frmThirdPartyEditor_UltraFormManager_Dock_Area_Bottom";
            this._frmThirdPartyEditor_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(949, 4);
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataSource = typeof(Prana.BusinessObjects.ThirdPartyBatchCommon);
            // 
            // thirdPartyBatchBindingSource
            // 
            this.thirdPartyBatchBindingSource.DataSource = typeof(Prana.BusinessObjects.ThirdPartyBatch);
            // 
            // frmThirdPartyEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(949, 691);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.dataView);
            this.Controls.Add(this.bindingNavigator1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this._frmThirdPartyEditor_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._frmThirdPartyEditor_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._frmThirdPartyEditor_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._frmThirdPartyEditor_UltraFormManager_Dock_Area_Bottom);
            this.Name = "frmThirdPartyEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Text = "Third Party Editor";
            this.Load += new System.EventHandler(this.frmThirdPartyEditor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.thirdPartyGnuPGBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.thirdPartyFtpBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.thirdPartyBatchBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorAddNewItem;
        private System.Windows.Forms.ToolStripButton toleranceProfileAddNewItem;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        private System.Windows.Forms.ToolStripButton bindingNavigatorDeleteItem;
        private System.Windows.Forms.ToolStripButton btnFtpSettings;
        private System.Windows.Forms.ToolStripButton btnGnuPGSettings;
        private System.Windows.Forms.ToolStripButton btnJobSettings;
        private System.Windows.Forms.ToolStripButton btnToleranceProfileSettings;
        private System.Windows.Forms.DataGridView dataView;
        private System.Windows.Forms.BindingSource thirdPartyFtpBindingSource;
        private System.Windows.Forms.BindingSource thirdPartyBatchBindingSource;
        private System.Windows.Forms.BindingSource thirdPartyGnuPGBindingSource;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton btnEmailSettings;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _frmThirdPartyEditor_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _frmThirdPartyEditor_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _frmThirdPartyEditor_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _frmThirdPartyEditor_UltraFormManager_Dock_Area_Bottom;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;

    }
}