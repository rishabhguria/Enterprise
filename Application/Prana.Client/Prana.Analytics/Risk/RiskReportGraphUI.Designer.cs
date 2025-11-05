namespace Prana.Analytics
{
    partial class RiskReportGraphUI
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
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement1 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.GradientEffect gradientEffect1 = new Infragistics.UltraChart.Resources.Appearance.GradientEffect();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RiskReportGraphUI));
            this.chartPortfolioGrouping = new Infragistics.Win.UltraWinChart.UltraChart();
            this.ultraToolbarsManager1 = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this._RiskReportGraphUI_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._RiskReportGraphUI_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._RiskReportGraphUI_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._RiskReportGraphUI_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            ((System.ComponentModel.ISupportInitialize)(this.chartPortfolioGrouping)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // chartPortfolioGrouping
            // 
            this.chartPortfolioGrouping.Axis.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(220)))));
            paintElement1.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            paintElement1.Fill = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(220)))));
            this.chartPortfolioGrouping.Axis.PE = paintElement1;
            this.chartPortfolioGrouping.Axis.X.Labels.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chartPortfolioGrouping.Axis.X.Labels.FontColor = System.Drawing.Color.DimGray;
            this.chartPortfolioGrouping.Axis.X.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chartPortfolioGrouping.Axis.X.Labels.ItemFormatString = "<ITEM_LABEL>";
            this.chartPortfolioGrouping.Axis.X.Labels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.chartPortfolioGrouping.Axis.X.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.chartPortfolioGrouping.Axis.X.Labels.SeriesLabels.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chartPortfolioGrouping.Axis.X.Labels.SeriesLabels.FontColor = System.Drawing.Color.DimGray;
            this.chartPortfolioGrouping.Axis.X.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Center;
            this.chartPortfolioGrouping.Axis.X.Labels.SeriesLabels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.chartPortfolioGrouping.Axis.X.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chartPortfolioGrouping.Axis.X.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartPortfolioGrouping.Axis.X.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartPortfolioGrouping.Axis.X.LineThickness = 1;
            this.chartPortfolioGrouping.Axis.X.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chartPortfolioGrouping.Axis.X.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chartPortfolioGrouping.Axis.X.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartPortfolioGrouping.Axis.X.MajorGridLines.Visible = true;
            this.chartPortfolioGrouping.Axis.X.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chartPortfolioGrouping.Axis.X.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chartPortfolioGrouping.Axis.X.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartPortfolioGrouping.Axis.X.MinorGridLines.Visible = false;
            this.chartPortfolioGrouping.Axis.X.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
            this.chartPortfolioGrouping.Axis.X.Visible = false;
            this.chartPortfolioGrouping.Axis.X2.Labels.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chartPortfolioGrouping.Axis.X2.Labels.FontColor = System.Drawing.Color.Gray;
            this.chartPortfolioGrouping.Axis.X2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Far;
            this.chartPortfolioGrouping.Axis.X2.Labels.ItemFormatString = "<ITEM_LABEL>";
            this.chartPortfolioGrouping.Axis.X2.Labels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.chartPortfolioGrouping.Axis.X2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.chartPortfolioGrouping.Axis.X2.Labels.SeriesLabels.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chartPortfolioGrouping.Axis.X2.Labels.SeriesLabels.FontColor = System.Drawing.Color.Gray;
            this.chartPortfolioGrouping.Axis.X2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Center;
            this.chartPortfolioGrouping.Axis.X2.Labels.SeriesLabels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.chartPortfolioGrouping.Axis.X2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chartPortfolioGrouping.Axis.X2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartPortfolioGrouping.Axis.X2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartPortfolioGrouping.Axis.X2.LineThickness = 1;
            this.chartPortfolioGrouping.Axis.X2.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chartPortfolioGrouping.Axis.X2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chartPortfolioGrouping.Axis.X2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartPortfolioGrouping.Axis.X2.MajorGridLines.Visible = true;
            this.chartPortfolioGrouping.Axis.X2.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chartPortfolioGrouping.Axis.X2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chartPortfolioGrouping.Axis.X2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartPortfolioGrouping.Axis.X2.MinorGridLines.Visible = false;
            this.chartPortfolioGrouping.Axis.X2.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
            this.chartPortfolioGrouping.Axis.X2.Visible = false;
            this.chartPortfolioGrouping.Axis.Y.Labels.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chartPortfolioGrouping.Axis.Y.Labels.FontColor = System.Drawing.Color.DimGray;
            this.chartPortfolioGrouping.Axis.Y.Labels.HorizontalAlign = System.Drawing.StringAlignment.Far;
            this.chartPortfolioGrouping.Axis.Y.Labels.ItemFormatString = "<DATA_VALUE:00.##>";
            this.chartPortfolioGrouping.Axis.Y.Labels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.chartPortfolioGrouping.Axis.Y.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chartPortfolioGrouping.Axis.Y.Labels.SeriesLabels.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chartPortfolioGrouping.Axis.Y.Labels.SeriesLabels.FontColor = System.Drawing.Color.DimGray;
            this.chartPortfolioGrouping.Axis.Y.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Center;
            this.chartPortfolioGrouping.Axis.Y.Labels.SeriesLabels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.chartPortfolioGrouping.Axis.Y.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.chartPortfolioGrouping.Axis.Y.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartPortfolioGrouping.Axis.Y.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartPortfolioGrouping.Axis.Y.LineThickness = 1;
            this.chartPortfolioGrouping.Axis.Y.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chartPortfolioGrouping.Axis.Y.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chartPortfolioGrouping.Axis.Y.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartPortfolioGrouping.Axis.Y.MajorGridLines.Visible = true;
            this.chartPortfolioGrouping.Axis.Y.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chartPortfolioGrouping.Axis.Y.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chartPortfolioGrouping.Axis.Y.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartPortfolioGrouping.Axis.Y.MinorGridLines.Visible = false;
            this.chartPortfolioGrouping.Axis.Y.TickmarkInterval = 20D;
            this.chartPortfolioGrouping.Axis.Y.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
            this.chartPortfolioGrouping.Axis.Y.Visible = true;
            this.chartPortfolioGrouping.Axis.Y2.Labels.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chartPortfolioGrouping.Axis.Y2.Labels.FontColor = System.Drawing.Color.Gray;
            this.chartPortfolioGrouping.Axis.Y2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chartPortfolioGrouping.Axis.Y2.Labels.ItemFormatString = "<DATA_VALUE:00.##>";
            this.chartPortfolioGrouping.Axis.Y2.Labels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.chartPortfolioGrouping.Axis.Y2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chartPortfolioGrouping.Axis.Y2.Labels.SeriesLabels.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chartPortfolioGrouping.Axis.Y2.Labels.SeriesLabels.FontColor = System.Drawing.Color.Gray;
            this.chartPortfolioGrouping.Axis.Y2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Center;
            this.chartPortfolioGrouping.Axis.Y2.Labels.SeriesLabels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.chartPortfolioGrouping.Axis.Y2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.chartPortfolioGrouping.Axis.Y2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartPortfolioGrouping.Axis.Y2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartPortfolioGrouping.Axis.Y2.LineThickness = 1;
            this.chartPortfolioGrouping.Axis.Y2.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chartPortfolioGrouping.Axis.Y2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chartPortfolioGrouping.Axis.Y2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartPortfolioGrouping.Axis.Y2.MajorGridLines.Visible = true;
            this.chartPortfolioGrouping.Axis.Y2.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chartPortfolioGrouping.Axis.Y2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chartPortfolioGrouping.Axis.Y2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartPortfolioGrouping.Axis.Y2.MinorGridLines.Visible = false;
            this.chartPortfolioGrouping.Axis.Y2.TickmarkInterval = 20D;
            this.chartPortfolioGrouping.Axis.Y2.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
            this.chartPortfolioGrouping.Axis.Y2.Visible = false;
            this.chartPortfolioGrouping.Axis.Z.Labels.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chartPortfolioGrouping.Axis.Z.Labels.FontColor = System.Drawing.Color.DimGray;
            this.chartPortfolioGrouping.Axis.Z.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chartPortfolioGrouping.Axis.Z.Labels.ItemFormatString = "";
            this.chartPortfolioGrouping.Axis.Z.Labels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.chartPortfolioGrouping.Axis.Z.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chartPortfolioGrouping.Axis.Z.Labels.SeriesLabels.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chartPortfolioGrouping.Axis.Z.Labels.SeriesLabels.FontColor = System.Drawing.Color.DimGray;
            this.chartPortfolioGrouping.Axis.Z.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Center;
            this.chartPortfolioGrouping.Axis.Z.Labels.SeriesLabels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.chartPortfolioGrouping.Axis.Z.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chartPortfolioGrouping.Axis.Z.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartPortfolioGrouping.Axis.Z.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartPortfolioGrouping.Axis.Z.LineThickness = 1;
            this.chartPortfolioGrouping.Axis.Z.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chartPortfolioGrouping.Axis.Z.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chartPortfolioGrouping.Axis.Z.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartPortfolioGrouping.Axis.Z.MajorGridLines.Visible = true;
            this.chartPortfolioGrouping.Axis.Z.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chartPortfolioGrouping.Axis.Z.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chartPortfolioGrouping.Axis.Z.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartPortfolioGrouping.Axis.Z.MinorGridLines.Visible = false;
            this.chartPortfolioGrouping.Axis.Z.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
            this.chartPortfolioGrouping.Axis.Z.Visible = false;
            this.chartPortfolioGrouping.Axis.Z2.Labels.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chartPortfolioGrouping.Axis.Z2.Labels.FontColor = System.Drawing.Color.Gray;
            this.chartPortfolioGrouping.Axis.Z2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.chartPortfolioGrouping.Axis.Z2.Labels.ItemFormatString = "";
            this.chartPortfolioGrouping.Axis.Z2.Labels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.chartPortfolioGrouping.Axis.Z2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chartPortfolioGrouping.Axis.Z2.Labels.SeriesLabels.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chartPortfolioGrouping.Axis.Z2.Labels.SeriesLabels.FontColor = System.Drawing.Color.Gray;
            this.chartPortfolioGrouping.Axis.Z2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Center;
            this.chartPortfolioGrouping.Axis.Z2.Labels.SeriesLabels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.chartPortfolioGrouping.Axis.Z2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.chartPortfolioGrouping.Axis.Z2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartPortfolioGrouping.Axis.Z2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.chartPortfolioGrouping.Axis.Z2.LineThickness = 1;
            this.chartPortfolioGrouping.Axis.Z2.MajorGridLines.AlphaLevel = ((byte)(255));
            this.chartPortfolioGrouping.Axis.Z2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.chartPortfolioGrouping.Axis.Z2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartPortfolioGrouping.Axis.Z2.MajorGridLines.Visible = true;
            this.chartPortfolioGrouping.Axis.Z2.MinorGridLines.AlphaLevel = ((byte)(255));
            this.chartPortfolioGrouping.Axis.Z2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.chartPortfolioGrouping.Axis.Z2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.chartPortfolioGrouping.Axis.Z2.MinorGridLines.Visible = false;
            this.chartPortfolioGrouping.Axis.Z2.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
            this.chartPortfolioGrouping.Axis.Z2.Visible = false;
            this.chartPortfolioGrouping.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.chartPortfolioGrouping.ColorModel.AlphaLevel = ((byte)(150));
            this.chartPortfolioGrouping.ColorModel.ColorBegin = System.Drawing.Color.Pink;
            this.chartPortfolioGrouping.ColorModel.ColorEnd = System.Drawing.Color.DarkRed;
            this.chartPortfolioGrouping.ColorModel.ModelStyle = Infragistics.UltraChart.Shared.Styles.ColorModels.CustomLinear;
            this.chartPortfolioGrouping.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartPortfolioGrouping.Effects.Effects.Add(gradientEffect1);
            this.chartPortfolioGrouping.Legend.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chartPortfolioGrouping.Location = new System.Drawing.Point(0, 0);
            this.chartPortfolioGrouping.Name = "chartPortfolioGrouping";
            this.chartPortfolioGrouping.Size = new System.Drawing.Size(611, 311);
            this.chartPortfolioGrouping.TabIndex = 0;
            this.chartPortfolioGrouping.Tooltips.HighlightFillColor = System.Drawing.Color.DimGray;
            this.chartPortfolioGrouping.Tooltips.HighlightOutlineColor = System.Drawing.Color.DarkGray;
            // 
            // ultraToolbarsManager1
            // 
            this.ultraToolbarsManager1.DesignerFlags = 1;
            this.ultraToolbarsManager1.DockWithinContainer = this;
            this.ultraToolbarsManager1.DockWithinContainerBaseType = typeof(System.Windows.Forms.Form);
            // 
            // _RiskReportGraphUI_Toolbars_Dock_Area_Left
            // 
            this._RiskReportGraphUI_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._RiskReportGraphUI_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
            this._RiskReportGraphUI_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._RiskReportGraphUI_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._RiskReportGraphUI_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 0);
            this._RiskReportGraphUI_Toolbars_Dock_Area_Left.Name = "_RiskReportGraphUI_Toolbars_Dock_Area_Left";
            this._RiskReportGraphUI_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 311);
            this._RiskReportGraphUI_Toolbars_Dock_Area_Left.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _RiskReportGraphUI_Toolbars_Dock_Area_Right
            // 
            this._RiskReportGraphUI_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._RiskReportGraphUI_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
            this._RiskReportGraphUI_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._RiskReportGraphUI_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._RiskReportGraphUI_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(611, 0);
            this._RiskReportGraphUI_Toolbars_Dock_Area_Right.Name = "_RiskReportGraphUI_Toolbars_Dock_Area_Right";
            this._RiskReportGraphUI_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 311);
            this._RiskReportGraphUI_Toolbars_Dock_Area_Right.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _RiskReportGraphUI_Toolbars_Dock_Area_Top
            // 
            this._RiskReportGraphUI_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._RiskReportGraphUI_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
            this._RiskReportGraphUI_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._RiskReportGraphUI_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._RiskReportGraphUI_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._RiskReportGraphUI_Toolbars_Dock_Area_Top.Name = "_RiskReportGraphUI_Toolbars_Dock_Area_Top";
            this._RiskReportGraphUI_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(611, 0);
            this._RiskReportGraphUI_Toolbars_Dock_Area_Top.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _RiskReportGraphUI_Toolbars_Dock_Area_Bottom
            // 
            this._RiskReportGraphUI_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._RiskReportGraphUI_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
            this._RiskReportGraphUI_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._RiskReportGraphUI_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._RiskReportGraphUI_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 311);
            this._RiskReportGraphUI_Toolbars_Dock_Area_Bottom.Name = "_RiskReportGraphUI_Toolbars_Dock_Area_Bottom";
            this._RiskReportGraphUI_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(611, 0);
            this._RiskReportGraphUI_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // RiskReportGraphUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(611, 311);
            this.Controls.Add(this.chartPortfolioGrouping);
            this.Controls.Add(this._RiskReportGraphUI_Toolbars_Dock_Area_Left);
            this.Controls.Add(this._RiskReportGraphUI_Toolbars_Dock_Area_Right);
            this.Controls.Add(this._RiskReportGraphUI_Toolbars_Dock_Area_Bottom);
            this.Controls.Add(this._RiskReportGraphUI_Toolbars_Dock_Area_Top);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RiskReportGraphUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Risk Graph";
            ((System.ComponentModel.ISupportInitialize)(this.chartPortfolioGrouping)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinChart.UltraChart chartPortfolioGrouping;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager ultraToolbarsManager1;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _RiskReportGraphUI_Toolbars_Dock_Area_Left;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _RiskReportGraphUI_Toolbars_Dock_Area_Right;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _RiskReportGraphUI_Toolbars_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _RiskReportGraphUI_Toolbars_Dock_Area_Top;
    }
}