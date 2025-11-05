using Prana.Global;
using Prana.Utilities.UI.UIUtilities;

namespace Prana.NirvanaQualityChecker
{
    partial class QualityCheck
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
            if (disposing && (components != null))
            {
                components.Dispose();
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
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QualityCheck));
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.moduleTree = new System.Windows.Forms.TreeView();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.datePickerEnabler = new System.Windows.Forms.CheckBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnDiagnose = new System.Windows.Forms.Button();
            this.fundChooser = new Prana.NirvanaQualityChecker.CheckedComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.datagridview_ScriptSelecter = new PranaUltraGrid();
            this.searchinggif = new System.Windows.Forms.Button();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._ErrorDetectorTool_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ErrorDetectorTool_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ErrorDetectorTool_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ErrorDetectorTool_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.datagridview_ScriptSelecter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Right;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.GreenYellow;
            this.label1.Location = new System.Drawing.Point(860, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(152, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Quality Checker";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Visible = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.panel1.Controls.Add(this.checkBox1);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(8, 31);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1012, 87);
            this.panel1.TabIndex = 0;
            this.panel1.Visible = false;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(612, 51);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(15, 14);
            this.checkBox1.TabIndex = 10;
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.Visible = false;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(519, 49);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(88, 16);
            this.label10.TabIndex = 9;
            this.label10.Text = "Save db pref:";
            this.label10.Visible = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.LightGray;
            this.panel2.Controls.Add(this.moduleTree);
            this.panel2.Controls.Add(this.dateTimePicker1);
            this.panel2.Controls.Add(this.datePickerEnabler);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(8, 118);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(245, 356);
            this.panel2.TabIndex = 1;
            // 
            // moduleTree
            // 
            this.moduleTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.moduleTree.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(88)))), ((int)(((byte)(90)))));
            this.moduleTree.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.moduleTree.CheckBoxes = true;
            this.moduleTree.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.moduleTree.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.moduleTree.Location = new System.Drawing.Point(0, 30);
            this.moduleTree.Name = "moduleTree";
            this.moduleTree.Size = new System.Drawing.Size(245, 326);
            this.moduleTree.TabIndex = 1;
            this.moduleTree.BeforeCheck += new System.Windows.Forms.TreeViewCancelEventHandler(this.scriptsList_BeforeCheck);
            this.moduleTree.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.scriptsList_AfterCheck);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Enabled = false;
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker1.Location = new System.Drawing.Point(115, 5);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(80, 22);
            this.dateTimePicker1.TabIndex = 1;
            // 
            // datePickerEnabler
            // 
            this.datePickerEnabler.AutoSize = true;
            this.datePickerEnabler.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datePickerEnabler.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.datePickerEnabler.Location = new System.Drawing.Point(15, 7);
            this.datePickerEnabler.Name = "datePickerEnabler";
            this.datePickerEnabler.Size = new System.Drawing.Size(93, 20);
            this.datePickerEnabler.TabIndex = 0;
            this.datePickerEnabler.Text = "From Date:";
            this.datePickerEnabler.UseVisualStyleBackColor = true;
            this.datePickerEnabler.CheckedChanged += new System.EventHandler(this.datePickerEnabler_CheckedChanged);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Controls.Add(this.datagridview_ScriptSelecter);
            this.panel3.Controls.Add(this.searchinggif);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(253, 118);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(767, 356);
            this.panel3.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.LightGray;
            this.panel4.Controls.Add(this.btnDiagnose);
            this.panel4.Controls.Add(this.fundChooser);
            this.panel4.Controls.Add(this.label7);
            this.panel4.Controls.Add(this.label9);
            this.panel4.Controls.Add(this.dateTimePicker2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(767, 30);
            this.panel4.TabIndex = 0;
            // 
            // btnDiagnose
            // 
            this.btnDiagnose.ForeColor = System.Drawing.Color.Black;
            this.btnDiagnose.Location = new System.Drawing.Point(686, 3);
            this.btnDiagnose.Name = "btnDiagnose";
            this.btnDiagnose.Size = new System.Drawing.Size(75, 23);
            this.btnDiagnose.TabIndex = 8;
            this.btnDiagnose.Text = "Diagnose";
            this.btnDiagnose.UseVisualStyleBackColor = true;
            this.btnDiagnose.Click += new System.EventHandler(this.diagnose_Click);
            // 
            // fundChooser
            // 
            this.fundChooser.CheckOnClick = true;
            this.fundChooser.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.fundChooser.DropDownHeight = 1;
            this.fundChooser.FormattingEnabled = true;
            this.fundChooser.IntegralHeight = false;
            this.fundChooser.Location = new System.Drawing.Point(501, 4);
            this.fundChooser.Name = "fundChooser";
            this.fundChooser.Size = new System.Drawing.Size(59, 23);
            this.fundChooser.TabIndex = 7;
            this.fundChooser.ValueSeparator = ", ";
            this.fundChooser.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.label7.Location = new System.Drawing.Point(6, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 16);
            this.label7.TabIndex = 2;
            this.label7.Text = "To Date:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.label9.Location = new System.Drawing.Point(447, 8);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(48, 16);
            this.label9.TabIndex = 6;
            this.label9.Text = "Funds:";
            this.label9.Visible = false;
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker2.Location = new System.Drawing.Point(72, 4);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(80, 22);
            this.dateTimePicker2.TabIndex = 3;
            // 
            // datagridview_ScriptSelecter
            // 
            this.datagridview_ScriptSelecter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.datagridview_ScriptSelecter.DisplayLayout.Appearance = appearance1;
            this.datagridview_ScriptSelecter.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.datagridview_ScriptSelecter.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.datagridview_ScriptSelecter.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.datagridview_ScriptSelecter.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.datagridview_ScriptSelecter.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.datagridview_ScriptSelecter.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.datagridview_ScriptSelecter.DisplayLayout.GroupByBox.Hidden = true;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.datagridview_ScriptSelecter.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.datagridview_ScriptSelecter.DisplayLayout.MaxColScrollRegions = 1;
            this.datagridview_ScriptSelecter.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.datagridview_ScriptSelecter.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.datagridview_ScriptSelecter.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.datagridview_ScriptSelecter.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.datagridview_ScriptSelecter.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.datagridview_ScriptSelecter.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.datagridview_ScriptSelecter.DisplayLayout.Override.CellAppearance = appearance8;
            this.datagridview_ScriptSelecter.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.datagridview_ScriptSelecter.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.datagridview_ScriptSelecter.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Center";
            appearance10.TextVAlignAsString = "Middle";
            this.datagridview_ScriptSelecter.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.datagridview_ScriptSelecter.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.datagridview_ScriptSelecter.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.datagridview_ScriptSelecter.DisplayLayout.Override.RowAppearance = appearance11;
            this.datagridview_ScriptSelecter.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.datagridview_ScriptSelecter.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.datagridview_ScriptSelecter.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.datagridview_ScriptSelecter.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.datagridview_ScriptSelecter.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.datagridview_ScriptSelecter.Location = new System.Drawing.Point(0, 33);
            this.datagridview_ScriptSelecter.Name = "datagridview_ScriptSelecter";
            this.datagridview_ScriptSelecter.Size = new System.Drawing.Size(767, 320);
            this.datagridview_ScriptSelecter.TabIndex = 4;
            this.datagridview_ScriptSelecter.Text = "ultraGrid1";
            this.datagridview_ScriptSelecter.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.datagridview_ScriptSelecter_InitializeRow);
            this.datagridview_ScriptSelecter.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.datagridview_ScriptSelecter_ClickCellButton);
            // 
            // searchinggif
            // 
            this.searchinggif.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchinggif.FlatAppearance.BorderSize = 0;
            this.searchinggif.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.searchinggif.ForeColor = System.Drawing.Color.Black;
            this.searchinggif.Image = ((System.Drawing.Image)(resources.GetObject("searchinggif.Image")));
            this.searchinggif.Location = new System.Drawing.Point(0, 0);
            this.searchinggif.Name = "searchinggif";
            this.searchinggif.Size = new System.Drawing.Size(767, 356);
            this.searchinggif.TabIndex = 3;
            this.searchinggif.UseVisualStyleBackColor = true;
            this.searchinggif.Visible = false;
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _ErrorDetectorTool_UltraFormManager_Dock_Area_Left
            // 
            this._ErrorDetectorTool_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ErrorDetectorTool_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ErrorDetectorTool_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._ErrorDetectorTool_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.Color.White;
            this._ErrorDetectorTool_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._ErrorDetectorTool_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._ErrorDetectorTool_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 31);
            this._ErrorDetectorTool_UltraFormManager_Dock_Area_Left.Name = "_ErrorDetectorTool_UltraFormManager_Dock_Area_Left";
            this._ErrorDetectorTool_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 443);
            // 
            // _ErrorDetectorTool_UltraFormManager_Dock_Area_Right
            // 
            this._ErrorDetectorTool_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ErrorDetectorTool_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ErrorDetectorTool_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ErrorDetectorTool_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.Color.White;
            this._ErrorDetectorTool_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._ErrorDetectorTool_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._ErrorDetectorTool_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(1020, 31);
            this._ErrorDetectorTool_UltraFormManager_Dock_Area_Right.Name = "_ErrorDetectorTool_UltraFormManager_Dock_Area_Right";
            this._ErrorDetectorTool_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 443);
            // 
            // _ErrorDetectorTool_UltraFormManager_Dock_Area_Top
            // 
            this._ErrorDetectorTool_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ErrorDetectorTool_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ErrorDetectorTool_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._ErrorDetectorTool_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.Color.White;
            this._ErrorDetectorTool_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._ErrorDetectorTool_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._ErrorDetectorTool_UltraFormManager_Dock_Area_Top.Name = "_ErrorDetectorTool_UltraFormManager_Dock_Area_Top";
            this._ErrorDetectorTool_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(1028, 31);
            // 
            // _ErrorDetectorTool_UltraFormManager_Dock_Area_Bottom
            // 
            this._ErrorDetectorTool_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ErrorDetectorTool_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._ErrorDetectorTool_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ErrorDetectorTool_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.Color.White;
            this._ErrorDetectorTool_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._ErrorDetectorTool_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._ErrorDetectorTool_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 474);
            this._ErrorDetectorTool_UltraFormManager_Dock_Area_Bottom.Name = "_ErrorDetectorTool_UltraFormManager_Dock_Area_Bottom";
            this._ErrorDetectorTool_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(1028, 8);
            // 
            // QualityCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(88)))), ((int)(((byte)(90)))));
            this.ClientSize = new System.Drawing.Size(1028, 482);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this._ErrorDetectorTool_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ErrorDetectorTool_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ErrorDetectorTool_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ErrorDetectorTool_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "QualityCheck";
            this.Text = "Quality Checker";
            this.Load += new System.EventHandler(this.ErrorDetectorTool_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.datagridview_ScriptSelecter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TreeView moduleTree;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.CheckBox datePickerEnabler;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnDiagnose;
        private CheckedComboBox fundChooser;
        private System.Windows.Forms.Button searchinggif;
        private PranaUltraGrid datagridview_ScriptSelecter;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label10;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ErrorDetectorTool_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ErrorDetectorTool_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ErrorDetectorTool_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ErrorDetectorTool_UltraFormManager_Dock_Area_Bottom;
    }
}

