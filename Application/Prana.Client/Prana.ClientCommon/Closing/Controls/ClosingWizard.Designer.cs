namespace Prana.ClientCommon
{
    partial class ClosingWizard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_proxy != null)
                    _proxy.Dispose();
                _previewTemplate.Dispose();
                if (components != null)
                {
                    components.Dispose();
                }
                if (_controlActivityIndicator != null)
                {
                    _controlActivityIndicator.Dispose();
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTree.UltraTreeColumnSet ultraTreeColumnSet1 = new Infragistics.Win.UltraWinTree.UltraTreeColumnSet();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTree.Override _override1 = new Infragistics.Win.UltraWinTree.Override();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            //Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab5 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab4 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            this.tabMain = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.tabStandardFilters = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.pnlWizardBtns = new Infragistics.Win.Misc.UltraPanel();
            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
            this.btnNext = new Infragistics.Win.Misc.UltraButton();
            this.btnBack = new Infragistics.Win.Misc.UltraButton();
            this.btnFinish = new Infragistics.Win.Misc.UltraButton();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.treeViewClosing = new Infragistics.Win.UltraWinTree.UltraTree();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.tabCustomFilters = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.tabClosingAlgo = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.mnuRootTemplate = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addTemplateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTemplate = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteTemplateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.tabClosingWizard = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._ClosingWizard_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ClosingWizard_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ClosingWizard_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ClosingWizard_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.optionMain = new Prana.ClientCommon.Options();
            this.ctrlStandardFilters = new Prana.ClientCommon.ctrlStandardFilters();
            this.ctrlClosingAlgo1 = new Prana.ClientCommon.ctrlClosingAlgo();
            this.ctrlCustomFilters = new Prana.ClientCommon.ctrlCustomFilters();
            this.tabMain.SuspendLayout();
            this.tabStandardFilters.SuspendLayout();
            this.pnlWizardBtns.ClientArea.SuspendLayout();
            this.pnlWizardBtns.SuspendLayout();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeViewClosing)).BeginInit();
            this.tabCustomFilters.SuspendLayout();
            this.tabClosingAlgo.SuspendLayout();
            this.mnuRootTemplate.SuspendLayout();
            this.mnuTemplate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabClosingWizard)).BeginInit();
            this.tabClosingWizard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.optionMain);
            this.tabMain.Controls.Add(this.pnlWizardBtns);
            this.tabMain.Controls.Add(this.ultraPanel1);
            this.tabMain.Location = new System.Drawing.Point(0, 0);
            this.tabMain.Name = "tabMain";
            this.tabMain.Size = new System.Drawing.Size(894, 540);
            // 
            // tabStandardFilters
            // 
            this.tabStandardFilters.Controls.Add(this.ctrlStandardFilters);
            this.tabStandardFilters.Location = new System.Drawing.Point(-10000, -10000);
            this.tabStandardFilters.Name = "tabStandardFilters";
            this.tabStandardFilters.Size = new System.Drawing.Size(894, 540);
            // 
            // pnlWizardBtns
            // 
            this.pnlWizardBtns.BackColorInternal = System.Drawing.Color.White;
            // 
            // pnlWizardBtns.ClientArea
            // 
            this.pnlWizardBtns.ClientArea.Controls.Add(this.btnCancel);
            this.pnlWizardBtns.ClientArea.Controls.Add(this.btnNext);
            this.pnlWizardBtns.ClientArea.Controls.Add(this.btnBack);
            this.pnlWizardBtns.ClientArea.Controls.Add(this.btnFinish);
            this.pnlWizardBtns.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlWizardBtns.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlWizardBtns.Location = new System.Drawing.Point(163, 504);
            this.pnlWizardBtns.Name = "pnlWizardBtns";
            this.pnlWizardBtns.Size = new System.Drawing.Size(731, 36);
            this.pnlWizardBtns.TabIndex = 12;
            // 
            // btnCancel
            // 
            this.btnCancel.AcceptsFocus = false;
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.ForeColor = System.Drawing.Color.Black;
            this.btnCancel.Appearance = appearance1;
            this.btnCancel.Location = new System.Drawing.Point(415, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(69, 24);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance2.ForeColor = System.Drawing.Color.Black;
            this.btnNext.Appearance = appearance2;
            this.btnNext.Enabled = false;
            this.btnNext.Location = new System.Drawing.Point(565, 8);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(69, 24);
            this.btnNext.TabIndex = 10;
            this.btnNext.Text = "Next >";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnBack
            // 
            this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance3.ForeColor = System.Drawing.Color.Black;
            this.btnBack.Appearance = appearance3;
            this.btnBack.Location = new System.Drawing.Point(490, 8);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(69, 24);
            this.btnBack.TabIndex = 9;
            this.btnBack.Text = "< Back";
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnFinish
            // 
            this.btnFinish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            appearance4.ForeColor = System.Drawing.Color.Black;
            this.btnFinish.Appearance = appearance4;
            this.btnFinish.Location = new System.Drawing.Point(640, 8);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(69, 24);
            this.btnFinish.TabIndex = 11;
            this.btnFinish.Text = "Finish";
            this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.treeViewClosing);
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraLabel1);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(163, 540);
            this.ultraPanel1.TabIndex = 13;
            // 
            // treeViewClosing
            // 
            appearance5.BackColor = System.Drawing.Color.White;
            appearance5.BackColor2 = System.Drawing.Color.SteelBlue;
            appearance5.BackGradientAlignment = Infragistics.Win.GradientAlignment.Form;
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance5.FontData.BoldAsString = "False";
            appearance5.ForeColor = System.Drawing.Color.Black;
            this.treeViewClosing.Appearance = appearance5;
            this.treeViewClosing.BorderStyle = Infragistics.Win.UIElementBorderStyle.RaisedSoft;
            appearance6.ForeColor = System.Drawing.Color.White;
            ultraTreeColumnSet1.ActiveCellAppearance = appearance6;
            this.treeViewClosing.ColumnSettings.RootColumnSet = ultraTreeColumnSet1;
            this.treeViewClosing.DisplayStyle = Infragistics.Win.UltraWinTree.UltraTreeDisplayStyle.WindowsVista;
            this.treeViewClosing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewClosing.ExpansionIndicatorColor = System.Drawing.Color.SteelBlue;
            this.treeViewClosing.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeViewClosing.HideSelection = false;
            this.treeViewClosing.Location = new System.Drawing.Point(0, 20);
            this.treeViewClosing.Name = "treeViewClosing";
            _override1.LabelEdit = Infragistics.Win.DefaultableBoolean.True;
            _override1.NodeStyle = Infragistics.Win.UltraWinTree.NodeStyle.CheckBoxTriState;
            appearance7.BackColor = System.Drawing.Color.LightSteelBlue;
            _override1.SelectedNodeAppearance = appearance7;
            _override1.UseEditor = Infragistics.Win.DefaultableBoolean.True;
            this.treeViewClosing.Override = _override1;
            this.treeViewClosing.Size = new System.Drawing.Size(163, 520);
            this.treeViewClosing.TabIndex = 4;
            this.treeViewClosing.AfterCheck += new Infragistics.Win.UltraWinTree.AfterNodeChangedEventHandler(this.treeViewClosing_AfterCheck);
            this.treeViewClosing.AfterLabelEdit += new Infragistics.Win.UltraWinTree.AfterNodeChangedEventHandler(this.treeViewClosing_AfterLabelEdit);
            this.treeViewClosing.AfterSelect += new Infragistics.Win.UltraWinTree.AfterNodeSelectEventHandler(this.treeViewClosing_AfterSelect);
            this.treeViewClosing.BeforeCheck += new Infragistics.Win.UltraWinTree.BeforeCheckEventHandler(this.treeViewClosing_BeforeCheck);
            this.treeViewClosing.BeforeLabelEdit += new Infragistics.Win.UltraWinTree.BeforeNodeChangedEventHandler(this.treeViewClosing_BeforeLabelEdit);
            this.treeViewClosing.BeforeSelect += new Infragistics.Win.UltraWinTree.BeforeNodeSelectEventHandler(this.treeViewClosing_BeforeSelect);
            this.treeViewClosing.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeViewClosing_MouseDown);
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraLabel1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel1.Location = new System.Drawing.Point(0, 0);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(163, 20);
            this.ultraLabel1.TabIndex = 17;
            this.ultraLabel1.Text = "Templates";
            this.ultraLabel1.UseAppStyling = false;
            // 
            // tabCustomFilters
            // 
            this.tabCustomFilters.Controls.Add(this.ctrlCustomFilters);
            this.tabCustomFilters.Location = new System.Drawing.Point(-10000, -10000);
            this.tabCustomFilters.Name = "tabCustomFilters";
            this.tabCustomFilters.Size = new System.Drawing.Size(894, 540);
            // 
            // tabClosingAlgo
            // 
            this.tabClosingAlgo.Controls.Add(this.ctrlClosingAlgo1);
            this.tabClosingAlgo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabClosingAlgo.Location = new System.Drawing.Point(-10000, -10000);
            this.tabClosingAlgo.Name = "tabClosingAlgo";
            this.tabClosingAlgo.Size = new System.Drawing.Size(894, 540);
            // 
            // mnuRootTemplate
            // 
            this.mnuRootTemplate.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addTemplateMenuItem,
            this.runToolStripMenuItem});
            this.mnuRootTemplate.Name = "mnuRootTemplate";
            this.mnuRootTemplate.Size = new System.Drawing.Size(151, 48);
            this.inboxControlStyler1.SetStyleSettings(this.mnuRootTemplate, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            // 
            // addTemplateMenuItem
            // 
            this.addTemplateMenuItem.Name = "addTemplateMenuItem";
            this.addTemplateMenuItem.Size = new System.Drawing.Size(150, 22);
            this.addTemplateMenuItem.Text = "Add Template";
            this.addTemplateMenuItem.Click += new System.EventHandler(this.addTemplateMenuItem_Click);
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            this.runToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.runToolStripMenuItem.Text = "Run (Selected)";
            this.runToolStripMenuItem.Click += new System.EventHandler(this.runToolStripMenuItem_Click);
            // 
            // mnuTemplate
            // 
            this.mnuTemplate.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteTemplateMenuItem});
            this.mnuTemplate.Name = "mnuTemplate";
            this.mnuTemplate.Size = new System.Drawing.Size(108, 26);
            this.inboxControlStyler1.SetStyleSettings(this.mnuTemplate, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            // 
            // deleteTemplateMenuItem
            // 
            this.deleteTemplateMenuItem.Name = "deleteTemplateMenuItem";
            this.deleteTemplateMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteTemplateMenuItem.Text = "Delete";
            this.deleteTemplateMenuItem.Click += new System.EventHandler(this.deleteTemplateMenuItem_Click);
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(894, 540);
            // 
            // tabClosingWizard
            // 
            appearance9.BackColor = System.Drawing.Color.White;
            this.tabClosingWizard.Appearance = appearance9;
            this.tabClosingWizard.Controls.Add(this.ultraTabSharedControlsPage1);
            this.tabClosingWizard.Controls.Add(this.tabMain);
            this.tabClosingWizard.Controls.Add(this.tabStandardFilters);
            this.tabClosingWizard.Controls.Add(this.tabClosingAlgo);
            this.tabClosingWizard.Controls.Add(this.tabCustomFilters);
            this.tabClosingWizard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabClosingWizard.Location = new System.Drawing.Point(4, 26);
            this.tabClosingWizard.Name = "tabClosingWizard";
            this.tabClosingWizard.SharedControls.AddRange(new System.Windows.Forms.Control[] {
            this.pnlWizardBtns,
            this.ultraPanel1});
            this.tabClosingWizard.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.tabClosingWizard.Size = new System.Drawing.Size(894, 540);
            this.tabClosingWizard.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Wizard;
            this.tabClosingWizard.TabIndex = 0;
            ultraTab1.Key = "TabMain";
            ultraTab1.TabPage = this.tabMain;
            ultraTab1.Text = "tab1";
            ultraTab2.AllowClosing = Infragistics.Win.DefaultableBoolean.True;
            ultraTab2.Key = "TabStandardFilter";
            ultraTab2.TabPage = this.tabStandardFilters;
            ultraTab2.Text = "tab2";
            ultraTab5.Key = "TabCustomFilter";
            ultraTab5.TabPage = this.tabCustomFilters;
            ultraTab5.Text = "tab3";
            ultraTab4.Key = "TabClosingAlgo";
            ultraTab4.TabPage = this.tabClosingAlgo;
            ultraTab4.Text = "tab4";
            this.tabClosingWizard.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2,
            ultraTab5,
            ultraTab4});
            this.tabClosingWizard.ActiveTabChanged += new Infragistics.Win.UltraWinTabControl.ActiveTabChangedEventHandler(this.tabClosingWizard_ActiveTabChanged);
            this.tabClosingWizard.SelectedTabChanging += new Infragistics.Win.UltraWinTabControl.SelectedTabChangingEventHandler(this.tabClosingWizard_SelectedTabChanging);
            this.tabClosingWizard.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.tabClosingWizard_SelectedTabChanged);
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _ClosingWizard_UltraFormManager_Dock_Area_Left
            // 
            this._ClosingWizard_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ClosingWizard_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ClosingWizard_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._ClosingWizard_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ClosingWizard_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._ClosingWizard_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 4;
            this._ClosingWizard_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 26);
            this._ClosingWizard_UltraFormManager_Dock_Area_Left.Name = "_ClosingWizard_UltraFormManager_Dock_Area_Left";
            this._ClosingWizard_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(4, 540);
            // 
            // _ClosingWizard_UltraFormManager_Dock_Area_Right
            // 
            this._ClosingWizard_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ClosingWizard_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ClosingWizard_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ClosingWizard_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ClosingWizard_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._ClosingWizard_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 4;
            this._ClosingWizard_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(898, 26);
            this._ClosingWizard_UltraFormManager_Dock_Area_Right.Name = "_ClosingWizard_UltraFormManager_Dock_Area_Right";
            this._ClosingWizard_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(4, 540);
            // 
            // _ClosingWizard_UltraFormManager_Dock_Area_Top
            // 
            this._ClosingWizard_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ClosingWizard_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ClosingWizard_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._ClosingWizard_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ClosingWizard_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._ClosingWizard_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._ClosingWizard_UltraFormManager_Dock_Area_Top.Name = "_ClosingWizard_UltraFormManager_Dock_Area_Top";
            this._ClosingWizard_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(902, 26);
            // 
            // _ClosingWizard_UltraFormManager_Dock_Area_Bottom
            // 
            this._ClosingWizard_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ClosingWizard_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ClosingWizard_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ClosingWizard_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ClosingWizard_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._ClosingWizard_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
            this._ClosingWizard_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 566);
            this._ClosingWizard_UltraFormManager_Dock_Area_Bottom.Name = "_ClosingWizard_UltraFormManager_Dock_Area_Bottom";
            this._ClosingWizard_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(902, 4);
            // 
            // optionMain
            // 
            this.optionMain.BackColor = System.Drawing.Color.White;
            this.optionMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.optionMain.ForeColor = System.Drawing.Color.White;
            this.optionMain.ImeMode = System.Windows.Forms.ImeMode.On;
            this.optionMain.Location = new System.Drawing.Point(163, 0);
            this.optionMain.Name = "optionMain";
            this.optionMain.Size = new System.Drawing.Size(731, 504);
            this.optionMain.TabIndex = 0;
            this.optionMain.Load += new System.EventHandler(this.optionMain_Load);
            // 
            // ctrlStandardFilters
            // 
            this.ctrlStandardFilters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ctrlStandardFilters.BackColor = System.Drawing.Color.White;
            this.ctrlStandardFilters.Location = new System.Drawing.Point(164, 0);
            this.ctrlStandardFilters.Name = "ctrlStandardFilters";
            this.ctrlStandardFilters.Size = new System.Drawing.Size(719, 499);
            this.ctrlStandardFilters.TabIndex = 0;
            this.ctrlStandardFilters.Load += new System.EventHandler(this.ctrlStandardFilters_Load);
            // 
            // ctrlClosingAlgo1
            // 
            this.ctrlClosingAlgo1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ctrlClosingAlgo1.BackColor = System.Drawing.Color.White;
            this.ctrlClosingAlgo1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ctrlClosingAlgo1.Location = new System.Drawing.Point(164, -1);
            this.ctrlClosingAlgo1.Name = "ctrlClosingAlgo1";
            this.ctrlClosingAlgo1.Size = new System.Drawing.Size(719, 494);
            this.ctrlClosingAlgo1.TabIndex = 12;
            // 
            // ctrlCustomFilters
            // 
            this.ctrlCustomFilters.BackColor = System.Drawing.Color.White;
            this.ctrlCustomFilters.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ctrlCustomFilters.Location = new System.Drawing.Point(164, 0);
            this.ctrlCustomFilters.Name = "ctrlCustomFilters";
            this.ctrlCustomFilters.Size = new System.Drawing.Size(719, 499);
            this.ctrlCustomFilters.TabIndex = 12;
            // 
            // ClosingWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(902, 570);
            this.ControlBox = false;
            this.Controls.Add(this.tabClosingWizard);
            this.Controls.Add(this._ClosingWizard_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ClosingWizard_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ClosingWizard_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ClosingWizard_UltraFormManager_Dock_Area_Bottom);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(902, 570);
            this.MinimumSize = new System.Drawing.Size(902, 570);
            this.Name = "ClosingWizard";
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Text = "Closing Wizard";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ClosingWizardUI_FormClosed);
            this.Load += new System.EventHandler(this.ClosingWizard_Load);
            this.tabMain.ResumeLayout(false);
            this.tabStandardFilters.ResumeLayout(false);
            this.pnlWizardBtns.ClientArea.ResumeLayout(false);
            this.pnlWizardBtns.ResumeLayout(false);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeViewClosing)).EndInit();
            this.tabCustomFilters.ResumeLayout(false);
            this.tabClosingAlgo.ResumeLayout(false);
            this.mnuRootTemplate.ResumeLayout(false);
            this.mnuTemplate.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabClosingWizard)).EndInit();
            this.tabClosingWizard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.ResumeLayout(false);

        }

       
        

        

        #endregion

        private System.Windows.Forms.ContextMenuStrip mnuRootTemplate;
        private System.Windows.Forms.ContextMenuStrip mnuTemplate;
        private System.Windows.Forms.ToolStripMenuItem addTemplateMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteTemplateMenuItem;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tabCustomFilters;
        private ctrlCustomFilters ctrlCustomFilters;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tabClosingAlgo;
        private ctrlClosingAlgo ctrlClosingAlgo1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tabStandardFilters;
        private ctrlStandardFilters ctrlStandardFilters;
        private Infragistics.Win.Misc.UltraPanel pnlWizardBtns;
        private Infragistics.Win.Misc.UltraButton btnCancel;
        private Infragistics.Win.Misc.UltraButton btnNext;
        private Infragistics.Win.Misc.UltraButton btnBack;
        private Infragistics.Win.Misc.UltraButton btnFinish;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.UltraWinTree.UltraTree treeViewClosing;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tabMain;
        private Options optionMain;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl tabClosingWizard;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ClosingWizard_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ClosingWizard_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ClosingWizard_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ClosingWizard_UltraFormManager_Dock_Area_Bottom;


 


       
    }
}