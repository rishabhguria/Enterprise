namespace Prana.HeatMap
{
    partial class HeatMap
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HeatMap));
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("HeatMap ToolBar");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("filterData");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool7 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Heats");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("filterData");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("changeGrouping");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool8 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Heats");
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.heatMapControl1 = new Prana.HeatMapControlsWPF.HeatMapControl();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanel3 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraLabelGrouping = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabelEsperStatus = new Infragistics.Win.Misc.UltraLabel();
            this.esperCon = new Infragistics.Win.UltraWinEditors.UltraPictureBox();
            this._ClientArea_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.ultraToolbarsManager1 = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this._ClientArea_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._ClientArea_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._ClientArea_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.ultraPanel2 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraStatusBar1 = new Infragistics.Win.UltraWinStatusBar.UltraStatusBar();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._HeatMap_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._HeatMap_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._HeatMap_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._HeatMap_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.groupingTool1 = new Prana.HeatMapControls.GroupingTool();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            this.ultraPanel3.ClientArea.SuspendLayout();
            this.ultraPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).BeginInit();
            this.ultraPanel2.ClientArea.SuspendLayout();
            this.ultraPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraStatusBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // elementHost1
            // 
            this.elementHost1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHost1.Location = new System.Drawing.Point(0, 0);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(970, 405);
            this.inboxControlStyler1.SetStyleSettings(this.elementHost1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.elementHost1.TabIndex = 2;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = this.heatMapControl1;
            // 
            // ultraPanel1
            // 
            this.ultraPanel1.AutoScroll = true;
            this.ultraPanel1.AutoSize = true;
            this.ultraPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.groupingTool1);
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraPanel3);
            this.ultraPanel1.ClientArea.Controls.Add(this._ClientArea_Toolbars_Dock_Area_Left);
            this.ultraPanel1.ClientArea.Controls.Add(this._ClientArea_Toolbars_Dock_Area_Right);
            this.ultraPanel1.ClientArea.Controls.Add(this._ClientArea_Toolbars_Dock_Area_Bottom);
            this.ultraPanel1.ClientArea.Controls.Add(this._ClientArea_Toolbars_Dock_Area_Top);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraPanel1.Location = new System.Drawing.Point(4, 27);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(970, 140);
            this.ultraPanel1.TabIndex = 5;
            // 
            // ultraPanel3
            // 
            // 
            // ultraPanel3.ClientArea
            // 
            this.ultraPanel3.ClientArea.Controls.Add(this.ultraLabelGrouping);
            this.ultraPanel3.ClientArea.Controls.Add(this.ultraLabelEsperStatus);
            this.ultraPanel3.ClientArea.Controls.Add(this.esperCon);
            this.ultraPanel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraPanel3.Location = new System.Drawing.Point(0, 25);
            this.ultraPanel3.Name = "ultraPanel3";
            this.ultraPanel3.Size = new System.Drawing.Size(970, 22);
            this.ultraPanel3.TabIndex = 23;
            // 
            // ultraLabelGrouping
            // 
            appearance1.FontData.BoldAsString = "False";
            appearance1.ForeColor = System.Drawing.Color.Black;
            appearance1.TextVAlignAsString = "Middle";
            this.ultraLabelGrouping.Appearance = appearance1;
            this.ultraLabelGrouping.Dock = System.Windows.Forms.DockStyle.Left;
            this.ultraLabelGrouping.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabelGrouping.Location = new System.Drawing.Point(0, 0);
            this.ultraLabelGrouping.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.ultraLabelGrouping.Name = "ultraLabelGrouping";
            this.ultraLabelGrouping.Size = new System.Drawing.Size(604, 22);
            this.ultraLabelGrouping.TabIndex = 2;
            // 
            // ultraLabelEsperStatus
            // 
            this.ultraLabelEsperStatus.Anchor = System.Windows.Forms.AnchorStyles.Right;
            appearance2.BackColor = System.Drawing.Color.Transparent;
            appearance2.ForeColor = System.Drawing.Color.Black;
            appearance2.TextHAlignAsString = "Left";
            appearance2.TextVAlignAsString = "Middle";
            this.ultraLabelEsperStatus.Appearance = appearance2;
            this.ultraLabelEsperStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabelEsperStatus.Location = new System.Drawing.Point(836, 1);
            this.ultraLabelEsperStatus.Name = "ultraLabelEsperStatus";
            this.ultraLabelEsperStatus.Size = new System.Drawing.Size(126, 23);
            this.ultraLabelEsperStatus.TabIndex = 8;
            this.ultraLabelEsperStatus.Text = "Calculation Engine";
            // 
            // esperCon
            // 
            this.esperCon.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.esperCon.BorderShadowColor = System.Drawing.Color.Empty;
            this.esperCon.Image = ((object)(resources.GetObject("esperCon.Image")));
            this.esperCon.Location = new System.Drawing.Point(809, 2);
            this.esperCon.Name = "esperCon";
            this.esperCon.Size = new System.Drawing.Size(21, 20);
            this.esperCon.TabIndex = 13;
            // 
            // _ClientArea_Toolbars_Dock_Area_Left
            // 
            this._ClientArea_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ClientArea_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
            this._ClientArea_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._ClientArea_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ClientArea_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 25);
            this._ClientArea_Toolbars_Dock_Area_Left.Name = "_ClientArea_Toolbars_Dock_Area_Left";
            this._ClientArea_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 115);
            this._ClientArea_Toolbars_Dock_Area_Left.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // ultraToolbarsManager1
            // 
            this.ultraToolbarsManager1.AlwaysShowMenusExpanded = Infragistics.Win.DefaultableBoolean.False;
            this.ultraToolbarsManager1.DesignerFlags = 1;
            this.ultraToolbarsManager1.DockWithinContainer = this.ultraPanel1.ClientArea;
            this.ultraToolbarsManager1.FormDisplayStyle = Infragistics.Win.UltraWinToolbars.FormDisplayStyle.RoundedFixed;
            this.ultraToolbarsManager1.LockToolbars = true;
            this.ultraToolbarsManager1.ShowFullMenusDelay = 500;
            ultraToolbar1.DockedColumn = 0;
            ultraToolbar1.DockedRow = 0;
            ultraToolbar1.IsMainMenuBar = true;
            ultraToolbar1.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool1,
            popupMenuTool7});
            ultraToolbar1.Text = "HeatMap ToolBar";
            this.ultraToolbarsManager1.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar1});
            buttonTool2.SharedPropsInternal.Caption = "Filter Data";
            buttonTool2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool4.SharedPropsInternal.Caption = "Change Grouping";
            buttonTool4.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            popupMenuTool8.SharedPropsInternal.Caption = "Heat";
            popupMenuTool8.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            this.ultraToolbarsManager1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool2,
            buttonTool4,
            popupMenuTool8});
            this.ultraToolbarsManager1.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.ultraToolbarsManager1_ToolClick);
            // 
            // _ClientArea_Toolbars_Dock_Area_Right
            // 
            this._ClientArea_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ClientArea_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
            this._ClientArea_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._ClientArea_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ClientArea_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(970, 25);
            this._ClientArea_Toolbars_Dock_Area_Right.Name = "_ClientArea_Toolbars_Dock_Area_Right";
            this._ClientArea_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 115);
            this._ClientArea_Toolbars_Dock_Area_Right.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _ClientArea_Toolbars_Dock_Area_Bottom
            // 
            this._ClientArea_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ClientArea_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
            this._ClientArea_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._ClientArea_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ClientArea_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 140);
            this._ClientArea_Toolbars_Dock_Area_Bottom.Name = "_ClientArea_Toolbars_Dock_Area_Bottom";
            this._ClientArea_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(970, 0);
            this._ClientArea_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _ClientArea_Toolbars_Dock_Area_Top
            // 
            this._ClientArea_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ClientArea_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
            this._ClientArea_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._ClientArea_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ClientArea_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._ClientArea_Toolbars_Dock_Area_Top.Name = "_ClientArea_Toolbars_Dock_Area_Top";
            this._ClientArea_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(970, 25);
            this._ClientArea_Toolbars_Dock_Area_Top.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // ultraPanel2
            // 
            appearance3.BorderColor = System.Drawing.Color.Transparent;
            this.ultraPanel2.Appearance = appearance3;
            this.ultraPanel2.AutoScroll = true;
            // 
            // ultraPanel2.ClientArea
            // 
            this.ultraPanel2.ClientArea.Controls.Add(this.elementHost1);
            this.ultraPanel2.ClientArea.Controls.Add(this.ultraStatusBar1);
            this.ultraPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel2.Location = new System.Drawing.Point(4, 167);
            this.ultraPanel2.Name = "ultraPanel2";
            this.ultraPanel2.Size = new System.Drawing.Size(970, 429);
            this.ultraPanel2.TabIndex = 6;
            // 
            // ultraStatusBar1
            // 
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            appearance4.FontData.BoldAsString = "False";
            appearance4.FontData.SizeInPoints = 10F;
            appearance4.ForeColor = System.Drawing.Color.White;
            this.ultraStatusBar1.Appearance = appearance4;
            this.ultraStatusBar1.Location = new System.Drawing.Point(0, 405);
            this.ultraStatusBar1.Name = "ultraStatusBar1";
            this.ultraStatusBar1.Padding = new Infragistics.Win.UltraWinStatusBar.UIElementMargins(15, 0, 0, 0);
            this.ultraStatusBar1.Size = new System.Drawing.Size(970, 24);
            this.ultraStatusBar1.TabIndex = 1;
            this.ultraStatusBar1.Text = "Loading...";
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _HeatMap_UltraFormManager_Dock_Area_Left
            // 
            this._HeatMap_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._HeatMap_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._HeatMap_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._HeatMap_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._HeatMap_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._HeatMap_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 4;
            this._HeatMap_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 27);
            this._HeatMap_UltraFormManager_Dock_Area_Left.Name = "_HeatMap_UltraFormManager_Dock_Area_Left";
            this._HeatMap_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(4, 569);
            // 
            // _HeatMap_UltraFormManager_Dock_Area_Right
            // 
            this._HeatMap_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._HeatMap_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._HeatMap_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._HeatMap_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._HeatMap_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._HeatMap_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 4;
            this._HeatMap_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(974, 27);
            this._HeatMap_UltraFormManager_Dock_Area_Right.Name = "_HeatMap_UltraFormManager_Dock_Area_Right";
            this._HeatMap_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(4, 569);
            // 
            // _HeatMap_UltraFormManager_Dock_Area_Top
            // 
            this._HeatMap_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._HeatMap_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._HeatMap_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._HeatMap_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._HeatMap_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._HeatMap_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._HeatMap_UltraFormManager_Dock_Area_Top.Name = "_HeatMap_UltraFormManager_Dock_Area_Top";
            this._HeatMap_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(978, 27);
            // 
            // _HeatMap_UltraFormManager_Dock_Area_Bottom
            // 
            this._HeatMap_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._HeatMap_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._HeatMap_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._HeatMap_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._HeatMap_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._HeatMap_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
            this._HeatMap_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 596);
            this._HeatMap_UltraFormManager_Dock_Area_Bottom.Name = "_HeatMap_UltraFormManager_Dock_Area_Bottom";
            this._HeatMap_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(978, 4);
            // 
            // groupingTool1
            // 
            this.groupingTool1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupingTool1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(44)))), ((int)(((byte)(57)))));
            this.groupingTool1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupingTool1.Location = new System.Drawing.Point(0, 47);
            this.groupingTool1.Name = "groupingTool1";
            this.groupingTool1.Size = new System.Drawing.Size(970, 93);
            this.groupingTool1.TabIndex = 24;
            this.groupingTool1.groupingChanged += new Prana.HeatMapControls.Delegates.GroupingChanged(this.groupingTool1_groupingChanged);
            // 
            // HeatMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(978, 600);
            this.Controls.Add(this.ultraPanel2);
            this.Controls.Add(this.ultraPanel1);
            this.Controls.Add(this._HeatMap_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._HeatMap_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._HeatMap_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._HeatMap_UltraFormManager_Dock_Area_Bottom);
            this.MinimumSize = new System.Drawing.Size(600, 600);
            this.Name = "HeatMap";
            this.ShowIcon = false;
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Text = "Heat Map";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.HeatMap_FormClosed);
            this.Load += new System.EventHandler(this.HeatMap_Load);
            this.ResizeEnd += new System.EventHandler(this.HeatMap_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            this.ultraPanel1.PerformLayout();
            this.ultraPanel3.ClientArea.ResumeLayout(false);
            this.ultraPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).EndInit();
            this.ultraPanel2.ClientArea.ResumeLayout(false);
            this.ultraPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraStatusBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        private Infragistics.Win.Misc.UltraPanel ultraPanel2;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _HeatMap_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _HeatMap_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _HeatMap_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _HeatMap_UltraFormManager_Dock_Area_Bottom;
        private Infragistics.Win.Misc.UltraLabel ultraLabelGrouping;
        private Infragistics.Win.Misc.UltraLabel ultraLabelEsperStatus;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _ClientArea_Toolbars_Dock_Area_Left;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager ultraToolbarsManager1;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _ClientArea_Toolbars_Dock_Area_Right;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _ClientArea_Toolbars_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _ClientArea_Toolbars_Dock_Area_Top;
        private Infragistics.Win.UltraWinStatusBar.UltraStatusBar ultraStatusBar1;
        private Infragistics.Win.UltraWinEditors.UltraPictureBox esperCon;
        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private HeatMapControlsWPF.HeatMapControl heatMapControl1;
        private HeatMapControls.GroupingTool groupingTool1;
        private Infragistics.Win.Misc.UltraPanel ultraPanel3;
    }
}

