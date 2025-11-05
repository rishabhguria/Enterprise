using Prana.Global;
namespace Prana.Tools
{
    public partial class DataCompareForm
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
                if (components != null)
                {
                    components.Dispose();
                }
                if (_bgReconcile != null)
                {
                    _bgReconcile.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataCompareForm));
            this.btnCompare = new Infragistics.Win.Misc.UltraButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.subMenuExcelExport = new System.Windows.Forms.ToolStripMenuItem();
            this.applicationMatchedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.applicationDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.primeBrokerDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.applicationUnmatchedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pranaDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.primeBrokerDataToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.xmlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuXmlAppUnMatched = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuXmlBrokerUnMatched = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuXmlAppMatched = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuXmlBrokerMatched = new System.Windows.Forms.ToolStripMenuItem();
            this.mappingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyUnMappedDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeDuplicateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPreferences = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.drpButtonExport = new System.Windows.Forms.ToolStripDropDownButton();
            this.matchedDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pranaDataToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.primeBrokerDataToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.unMathcedDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pranaDataToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.primeBrokerDataToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.btnDataMapping = new System.Windows.Forms.ToolStripButton();
            this.btnUnmappedSymbols = new System.Windows.Forms.ToolStripButton();
            this.btnPreferences = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.xMLBrokerMatchedmnuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xMLAppMatchedmnuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xMLBrokerUnmatchedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xMLAppUnMatchedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chkBoxClrExpCache = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanel2 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._DataCompareForm_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._DataCompareForm_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._DataCompareForm_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._DataCompareForm_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            //this.ctrlImageListButtons1 = new Prana.Utilities.UI.UIUtilities.CtrlImageListButtons(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkBoxClrExpCache)).BeginInit();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            this.ultraPanel2.ClientArea.SuspendLayout();
            this.ultraPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCompare
            // 
            this.btnCompare.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCompare.Location = new System.Drawing.Point(455, 10);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(132, 23);
            this.btnCompare.TabIndex = 4;
            this.btnCompare.Text = "Run Recon";
            this.btnCompare.Click += new System.EventHandler(this.btnCompare_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 49);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer1.Size = new System.Drawing.Size(1297, 542);
            this.splitContainer1.SplitterDistance = 690;
            this.inboxControlStyler1.SetStyleSettings(this.splitContainer1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.splitContainer1.TabIndex = 20;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.splitContainer2.Size = new System.Drawing.Size(690, 542);
            this.splitContainer2.SplitterDistance = 253;
            this.inboxControlStyler1.SetStyleSettings(this.splitContainer2, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.splitContainer2.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.splitContainer3.Size = new System.Drawing.Size(603, 542);
            this.splitContainer3.SplitterDistance = 253;
            this.inboxControlStyler1.SetStyleSettings(this.splitContainer3, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.splitContainer3.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1297, 24);
            this.inboxControlStyler1.SetStyleSettings(this.menuStrip1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.menuStrip1.TabIndex = 21;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.subMenuExcelExport,
            this.xmlToolStripMenuItem,
            this.mappingToolStripMenuItem,
            this.copyUnMappedDataToolStripMenuItem,
            this.removeDuplicateToolStripMenuItem,
            this.mnuPreferences});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(42, 20);
            this.mnuFile.Text = "File";
            // 
            // subMenuExcelExport
            // 
            this.subMenuExcelExport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.applicationMatchedToolStripMenuItem,
            this.applicationUnmatchedToolStripMenuItem});
            this.subMenuExcelExport.Name = "subMenuExcelExport";
            this.subMenuExcelExport.Size = new System.Drawing.Size(257, 22);
            this.subMenuExcelExport.Text = "Export to Excel";
            // 
            // applicationMatchedToolStripMenuItem
            // 
            this.applicationMatchedToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.applicationDataToolStripMenuItem,
            this.primeBrokerDataToolStripMenuItem});
            this.applicationMatchedToolStripMenuItem.Name = "applicationMatchedToolStripMenuItem";
            this.applicationMatchedToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.applicationMatchedToolStripMenuItem.Text = "Matched Data";
            // 
            // applicationDataToolStripMenuItem
            // 
            this.applicationDataToolStripMenuItem.Name = "applicationDataToolStripMenuItem";
            this.applicationDataToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.applicationDataToolStripMenuItem.Text = "Application Data";
            this.applicationDataToolStripMenuItem.Click += new System.EventHandler(this.applicationMatchedToolStripMenuItem_Click);
            // 
            // primeBrokerDataToolStripMenuItem
            // 
            this.primeBrokerDataToolStripMenuItem.Name = "primeBrokerDataToolStripMenuItem";
            this.primeBrokerDataToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.primeBrokerDataToolStripMenuItem.Text = "Prime Broker Data";
            this.primeBrokerDataToolStripMenuItem.Click += new System.EventHandler(this.brokerMatchedToolStripMenuItem_Click);
            // 
            // applicationUnmatchedToolStripMenuItem
            // 
            this.applicationUnmatchedToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pranaDataToolStripMenuItem,
            this.primeBrokerDataToolStripMenuItem1});
            this.applicationUnmatchedToolStripMenuItem.Name = "applicationUnmatchedToolStripMenuItem";
            this.applicationUnmatchedToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.applicationUnmatchedToolStripMenuItem.Text = "Unmatched Data";
            // 
            // pranaDataToolStripMenuItem
            // 
            this.pranaDataToolStripMenuItem.Name = "pranaDataToolStripMenuItem";
            this.pranaDataToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.pranaDataToolStripMenuItem.Text = "Application Data";
            this.pranaDataToolStripMenuItem.Click += new System.EventHandler(this.applicationUnmatchedToolStripMenuItem_Click);
            // 
            // primeBrokerDataToolStripMenuItem1
            // 
            this.primeBrokerDataToolStripMenuItem1.Name = "primeBrokerDataToolStripMenuItem1";
            this.primeBrokerDataToolStripMenuItem1.Size = new System.Drawing.Size(193, 22);
            this.primeBrokerDataToolStripMenuItem1.Text = "Prime Broker Data";
            this.primeBrokerDataToolStripMenuItem1.Click += new System.EventHandler(this.brokerUnMatchedToolStripMenuItem_Click);
            // 
            // xmlToolStripMenuItem
            // 
            this.xmlToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuXmlAppUnMatched,
            this.mnuXmlBrokerUnMatched,
            this.mnuXmlAppMatched,
            this.mnuXmlBrokerMatched});
            this.xmlToolStripMenuItem.Name = "xmlToolStripMenuItem";
            this.xmlToolStripMenuItem.Size = new System.Drawing.Size(257, 22);
            this.xmlToolStripMenuItem.Text = "Xml";
            this.xmlToolStripMenuItem.Visible = false;
            // 
            // mnuXmlAppUnMatched
            // 
            this.mnuXmlAppUnMatched.Name = "mnuXmlAppUnMatched";
            this.mnuXmlAppUnMatched.Size = new System.Drawing.Size(227, 22);
            this.mnuXmlAppUnMatched.Text = "Application Unmatched";
            this.mnuXmlAppUnMatched.Click += new System.EventHandler(this.mnuXmlAppUnMatched_Click);
            // 
            // mnuXmlBrokerUnMatched
            // 
            this.mnuXmlBrokerUnMatched.Name = "mnuXmlBrokerUnMatched";
            this.mnuXmlBrokerUnMatched.Size = new System.Drawing.Size(227, 22);
            this.mnuXmlBrokerUnMatched.Text = "Broker Unmatched";
            this.mnuXmlBrokerUnMatched.Click += new System.EventHandler(this.mnuXmlBrokerUnMatched_Click);
            // 
            // mnuXmlAppMatched
            // 
            this.mnuXmlAppMatched.Name = "mnuXmlAppMatched";
            this.mnuXmlAppMatched.Size = new System.Drawing.Size(227, 22);
            this.mnuXmlAppMatched.Text = "Application Matched";
            this.mnuXmlAppMatched.Click += new System.EventHandler(this.mnuXmlAppMatched_Click);
            // 
            // mnuXmlBrokerMatched
            // 
            this.mnuXmlBrokerMatched.Name = "mnuXmlBrokerMatched";
            this.mnuXmlBrokerMatched.Size = new System.Drawing.Size(227, 22);
            this.mnuXmlBrokerMatched.Text = "Broker Matched";
            this.mnuXmlBrokerMatched.Click += new System.EventHandler(this.mnuXmlBrokerMatched_Click);
            // 
            // mappingToolStripMenuItem
            // 
            this.mappingToolStripMenuItem.Name = "mappingToolStripMenuItem";
            this.mappingToolStripMenuItem.Size = new System.Drawing.Size(257, 22);
            this.mappingToolStripMenuItem.Text = "Data Mapping";
            this.mappingToolStripMenuItem.Click += new System.EventHandler(this.mappingToolStripMenuItem_Click);
            // 
            // copyUnMappedDataToolStripMenuItem
            // 
            this.copyUnMappedDataToolStripMenuItem.Name = "copyUnMappedDataToolStripMenuItem";
            this.copyUnMappedDataToolStripMenuItem.Size = new System.Drawing.Size(257, 22);
            this.copyUnMappedDataToolStripMenuItem.Text = "Export Un-mapped Symbols";
            this.copyUnMappedDataToolStripMenuItem.ToolTipText = "Export the un-mapped prime broker symbols to the symbol mapping tool";
            this.copyUnMappedDataToolStripMenuItem.Click += new System.EventHandler(this.copyUnMappedDataToolStripMenuItem_Click);
            // 
            // removeDuplicateToolStripMenuItem
            // 
            this.removeDuplicateToolStripMenuItem.Name = "removeDuplicateToolStripMenuItem";
            this.removeDuplicateToolStripMenuItem.Size = new System.Drawing.Size(257, 22);
            this.removeDuplicateToolStripMenuItem.Text = "Remove Duplicate";
            // 
            // mnuPreferences
            // 
            this.mnuPreferences.Name = "mnuPreferences";
            this.mnuPreferences.Size = new System.Drawing.Size(257, 22);
            this.mnuPreferences.Text = "Preferences";
            this.mnuPreferences.Click += new System.EventHandler(this.mnuPreferences_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.drpButtonExport,
            this.btnDataMapping,
            this.btnUnmappedSymbols,
            this.btnPreferences,
            this.toolStripButton1,
            this.toolStripDropDownButton1});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1297, 25);
            this.inboxControlStyler1.SetStyleSettings(this.toolStrip1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // drpButtonExport
            // 
            this.drpButtonExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.drpButtonExport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.matchedDataToolStripMenuItem,
            this.unMathcedDataToolStripMenuItem});
            this.drpButtonExport.Image = ((System.Drawing.Image)(resources.GetObject("drpButtonExport.Image")));
            this.drpButtonExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.drpButtonExport.Name = "drpButtonExport";
            this.drpButtonExport.Size = new System.Drawing.Size(124, 22);
            this.drpButtonExport.Text = "Export To Excel";
            // 
            // matchedDataToolStripMenuItem
            // 
            this.matchedDataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pranaDataToolStripMenuItem1,
            this.primeBrokerDataToolStripMenuItem2});
            this.matchedDataToolStripMenuItem.Name = "matchedDataToolStripMenuItem";
            this.matchedDataToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.matchedDataToolStripMenuItem.Text = "Matched Data";
            // 
            // pranaDataToolStripMenuItem1
            // 
            this.pranaDataToolStripMenuItem1.Name = "pranaDataToolStripMenuItem1";
            this.pranaDataToolStripMenuItem1.Size = new System.Drawing.Size(193, 22);
            this.pranaDataToolStripMenuItem1.Text = "Application Data";
            this.pranaDataToolStripMenuItem1.Click += new System.EventHandler(this.applicationMatchedToolStripMenuItem_Click);
            // 
            // primeBrokerDataToolStripMenuItem2
            // 
            this.primeBrokerDataToolStripMenuItem2.Name = "primeBrokerDataToolStripMenuItem2";
            this.primeBrokerDataToolStripMenuItem2.Size = new System.Drawing.Size(193, 22);
            this.primeBrokerDataToolStripMenuItem2.Text = "Prime Broker Data";
            this.primeBrokerDataToolStripMenuItem2.Click += new System.EventHandler(this.brokerMatchedToolStripMenuItem_Click);
            // 
            // unMathcedDataToolStripMenuItem
            // 
            this.unMathcedDataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pranaDataToolStripMenuItem2,
            this.primeBrokerDataToolStripMenuItem3});
            this.unMathcedDataToolStripMenuItem.Name = "unMathcedDataToolStripMenuItem";
            this.unMathcedDataToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.unMathcedDataToolStripMenuItem.Text = "Unmatched Data";
            // 
            // pranaDataToolStripMenuItem2
            // 
            this.pranaDataToolStripMenuItem2.Name = "pranaDataToolStripMenuItem2";
            this.pranaDataToolStripMenuItem2.Size = new System.Drawing.Size(193, 22);
            this.pranaDataToolStripMenuItem2.Text = "Application Data";
            this.pranaDataToolStripMenuItem2.Click += new System.EventHandler(this.applicationUnmatchedToolStripMenuItem_Click);
            // 
            // primeBrokerDataToolStripMenuItem3
            // 
            this.primeBrokerDataToolStripMenuItem3.Name = "primeBrokerDataToolStripMenuItem3";
            this.primeBrokerDataToolStripMenuItem3.Size = new System.Drawing.Size(193, 22);
            this.primeBrokerDataToolStripMenuItem3.Text = "Prime Broker Data";
            this.primeBrokerDataToolStripMenuItem3.Click += new System.EventHandler(this.brokerUnMatchedToolStripMenuItem_Click);
            // 
            // btnDataMapping
            // 
            this.btnDataMapping.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnDataMapping.Image = ((System.Drawing.Image)(resources.GetObject("btnDataMapping.Image")));
            this.btnDataMapping.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDataMapping.Name = "btnDataMapping";
            this.btnDataMapping.Size = new System.Drawing.Size(102, 22);
            this.btnDataMapping.Text = "Data Mapping";
            this.btnDataMapping.Click += new System.EventHandler(this.mappingToolStripMenuItem_Click);
            // 
            // btnUnmappedSymbols
            // 
            this.btnUnmappedSymbols.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnUnmappedSymbols.Image = ((System.Drawing.Image)(resources.GetObject("btnUnmappedSymbols.Image")));
            this.btnUnmappedSymbols.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUnmappedSymbols.Name = "btnUnmappedSymbols";
            this.btnUnmappedSymbols.Size = new System.Drawing.Size(193, 22);
            this.btnUnmappedSymbols.Text = "Export Un-Mapped Symbols";
            this.btnUnmappedSymbols.Click += new System.EventHandler(this.copyUnMappedDataToolStripMenuItem_Click);
            // 
            // btnPreferences
            // 
            this.btnPreferences.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnPreferences.Image = ((System.Drawing.Image)(resources.GetObject("btnPreferences.Image")));
            this.btnPreferences.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPreferences.Name = "btnPreferences";
            this.btnPreferences.Size = new System.Drawing.Size(90, 22);
            this.btnPreferences.Text = "Preferences";
            this.btnPreferences.Click += new System.EventHandler(this.mnuPreferences_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(184, 22);
            this.toolStripButton1.Text = "Generate ExceptionReport";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.xMLBrokerMatchedmnuItem,
            this.xMLAppMatchedmnuItem,
            this.xMLBrokerUnmatchedToolStripMenuItem,
            this.xMLAppUnMatchedToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(60, 22);
            this.toolStripDropDownButton1.Text = "Xml";
            this.toolStripDropDownButton1.Visible = false;
            // 
            // xMLBrokerMatchedmnuItem
            // 
            this.xMLBrokerMatchedmnuItem.Name = "xMLBrokerMatchedmnuItem";
            this.xMLBrokerMatchedmnuItem.Size = new System.Drawing.Size(228, 22);
            this.xMLBrokerMatchedmnuItem.Text = "XML Broker Matched";
            this.xMLBrokerMatchedmnuItem.Click += new System.EventHandler(this.xMLBrokerMatched_Click);
            // 
            // xMLAppMatchedmnuItem
            // 
            this.xMLAppMatchedmnuItem.Name = "xMLAppMatchedmnuItem";
            this.xMLAppMatchedmnuItem.Size = new System.Drawing.Size(228, 22);
            this.xMLAppMatchedmnuItem.Text = "XML App Matched";
            this.xMLAppMatchedmnuItem.Click += new System.EventHandler(this.xMLAppMatchedToolStripMenuItem_Click);
            // 
            // xMLBrokerUnmatchedToolStripMenuItem
            // 
            this.xMLBrokerUnmatchedToolStripMenuItem.Name = "xMLBrokerUnmatchedToolStripMenuItem";
            this.xMLBrokerUnmatchedToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.xMLBrokerUnmatchedToolStripMenuItem.Text = "XML Broker Unmatched";
            this.xMLBrokerUnmatchedToolStripMenuItem.Click += new System.EventHandler(this.xMLBrokerUnmatchedToolStripMenuItem_Click);
            // 
            // xMLAppUnMatchedToolStripMenuItem
            // 
            this.xMLAppUnMatchedToolStripMenuItem.Name = "xMLAppUnMatchedToolStripMenuItem";
            this.xMLAppUnMatchedToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.xMLAppUnMatchedToolStripMenuItem.Text = "XML App UnMatched";
            this.xMLAppUnMatchedToolStripMenuItem.Click += new System.EventHandler(this.xMLAppUnMatchedToolStripMenuItem_Click);
            // 
            // chkBoxClrExpCache
            // 
            this.chkBoxClrExpCache.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.chkBoxClrExpCache.AutoSize = true;
            this.chkBoxClrExpCache.Checked = true;
            this.chkBoxClrExpCache.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBoxClrExpCache.Location = new System.Drawing.Point(618, 11);
            this.chkBoxClrExpCache.Name = "chkBoxClrExpCache";
            this.chkBoxClrExpCache.Size = new System.Drawing.Size(280, 21);
            this.chkBoxClrExpCache.TabIndex = 22;
            this.chkBoxClrExpCache.Text = "Clear Exception list before each Recon";
            this.chkBoxClrExpCache.CheckedChanged += new System.EventHandler(this.chkBoxClrExpCache_CheckedChanged);
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.splitContainer1);
            this.ultraPanel1.ClientArea.Controls.Add(this.toolStrip1);
            this.ultraPanel1.ClientArea.Controls.Add(this.menuStrip1);
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraPanel2);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(4, 27);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(1297, 635);
            this.ultraPanel1.TabIndex = 0;
            // 
            // ultraPanel2
            // 
            // 
            // ultraPanel2.ClientArea
            // 
            this.ultraPanel2.ClientArea.Controls.Add(this.btnCompare);
            this.ultraPanel2.ClientArea.Controls.Add(this.chkBoxClrExpCache);
            this.ultraPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraPanel2.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraPanel2.Location = new System.Drawing.Point(0, 591);
            this.ultraPanel2.Name = "ultraPanel2";
            this.ultraPanel2.Size = new System.Drawing.Size(1297, 44);
            this.ultraPanel2.TabIndex = 23;
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _DataCompareForm_UltraFormManager_Dock_Area_Left
            // 
            this._DataCompareForm_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._DataCompareForm_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._DataCompareForm_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._DataCompareForm_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._DataCompareForm_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._DataCompareForm_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 4;
            this._DataCompareForm_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 27);
            this._DataCompareForm_UltraFormManager_Dock_Area_Left.Name = "_DataCompareForm_UltraFormManager_Dock_Area_Left";
            this._DataCompareForm_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(4, 635);
            // 
            // _DataCompareForm_UltraFormManager_Dock_Area_Right
            // 
            this._DataCompareForm_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._DataCompareForm_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._DataCompareForm_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._DataCompareForm_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._DataCompareForm_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._DataCompareForm_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 4;
            this._DataCompareForm_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(1301, 27);
            this._DataCompareForm_UltraFormManager_Dock_Area_Right.Name = "_DataCompareForm_UltraFormManager_Dock_Area_Right";
            this._DataCompareForm_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(4, 635);
            // 
            // _DataCompareForm_UltraFormManager_Dock_Area_Top
            // 
            this._DataCompareForm_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._DataCompareForm_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._DataCompareForm_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._DataCompareForm_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._DataCompareForm_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._DataCompareForm_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._DataCompareForm_UltraFormManager_Dock_Area_Top.Name = "_DataCompareForm_UltraFormManager_Dock_Area_Top";
            this._DataCompareForm_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(1305, 27);
            // 
            // _DataCompareForm_UltraFormManager_Dock_Area_Bottom
            // 
            this._DataCompareForm_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._DataCompareForm_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._DataCompareForm_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._DataCompareForm_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._DataCompareForm_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._DataCompareForm_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
            this._DataCompareForm_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 662);
            this._DataCompareForm_UltraFormManager_Dock_Area_Bottom.Name = "_DataCompareForm_UltraFormManager_Dock_Area_Bottom";
            this._DataCompareForm_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(1305, 4);
            // 
            // DataCompareForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1305, 666);
            this.Controls.Add(this.ultraPanel1);
            this.Controls.Add(this._DataCompareForm_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._DataCompareForm_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._DataCompareForm_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._DataCompareForm_UltraFormManager_Dock_Area_Bottom);
            this.MinimumSize = new System.Drawing.Size(1297, 655);
            this.Name = "DataCompareForm";
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Text = "Reconciliation";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DataCompareForm_FormClosed);
            this.Load += new System.EventHandler(this.DataCompareForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkBoxClrExpCache)).EndInit();
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ClientArea.PerformLayout();
            this.ultraPanel1.ResumeLayout(false);
            this.ultraPanel2.ClientArea.ResumeLayout(false);
            this.ultraPanel2.ClientArea.PerformLayout();
            this.ultraPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion




        private Infragistics.Win.Misc.UltraButton btnCompare;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem subMenuExcelExport;
        private System.Windows.Forms.ToolStripMenuItem applicationMatchedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem applicationUnmatchedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xmlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuXmlAppUnMatched;
        private System.Windows.Forms.ToolStripMenuItem mnuXmlBrokerUnMatched;
        private System.Windows.Forms.ToolStripMenuItem mnuXmlAppMatched;
        private System.Windows.Forms.ToolStripMenuItem mnuXmlBrokerMatched;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ToolStripMenuItem mappingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyUnMappedDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeDuplicateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem applicationDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem primeBrokerDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pranaDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem primeBrokerDataToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mnuPreferences;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton drpButtonExport;
        private System.Windows.Forms.ToolStripMenuItem matchedDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pranaDataToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem primeBrokerDataToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem unMathcedDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pranaDataToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem primeBrokerDataToolStripMenuItem3;
        private System.Windows.Forms.ToolStripButton btnDataMapping;
        private System.Windows.Forms.ToolStripButton btnUnmappedSymbols;
        private System.Windows.Forms.ToolStripButton btnPreferences;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem xMLBrokerMatchedmnuItem;
        private System.Windows.Forms.ToolStripMenuItem xMLAppMatchedmnuItem;
        private System.Windows.Forms.ToolStripMenuItem xMLBrokerUnmatchedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xMLAppUnMatchedToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripScreenshot;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkBoxClrExpCache;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.Misc.UltraPanel ultraPanel2;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _DataCompareForm_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _DataCompareForm_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _DataCompareForm_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _DataCompareForm_UltraFormManager_Dock_Area_Bottom;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        //private Utilities.UIUtilities.CtrlImageListButtons ctrlImageListButtons1;
       
       

    }
}
