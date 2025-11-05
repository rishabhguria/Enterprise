using Prana.Global;
namespace Prana.Utilities.UI.UIUtilities
{
    partial class CustomizableCardGrid
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
                if (_dtSummPrint != null)
                {
                    _dtSummPrint.Dispose();
                }
                if (_customColumnChooserDialog != null)
                {
                    _customColumnChooserDialog.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomizableCardGrid));
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            this.panel1 = new Infragistics.Win.Misc.UltraPanel();
            this.btnColumnChooser = new System.Windows.Forms.Button();
            this.ultraGrid1 = new PranaUltraGrid();
            this.grdPrint = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.panel1.ClientArea.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGrid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdPrint)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColorInternal = System.Drawing.Color.Black;
            // 
            // panel1.ClientArea
            // 
            this.panel1.ClientArea.Controls.Add(this.btnColumnChooser);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.ForeColor = System.Drawing.SystemColors.Control;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(22, 268);
            this.panel1.TabIndex = 5;
            // 
            // btnColumnChooser
            // 
            this.btnColumnChooser.Image = ((System.Drawing.Image)(resources.GetObject("btnColumnChooser.Image")));
            this.btnColumnChooser.Location = new System.Drawing.Point(0, 0);
            this.btnColumnChooser.Name = "btnColumnChooser";
            this.btnColumnChooser.Size = new System.Drawing.Size(22, 20);
            this.btnColumnChooser.TabIndex = 2;
            this.btnColumnChooser.Text = "Column Chooser";
            this.btnColumnChooser.UseVisualStyleBackColor = true;
            this.btnColumnChooser.Click += new System.EventHandler(this.btnColumnChooser_Click);
            // 
            // ultraGrid1
            // 
            appearance1.BackColor = System.Drawing.Color.Black;
            this.ultraGrid1.DisplayLayout.Appearance = appearance1;
            appearance2.BackColor = System.Drawing.Color.Black;
            this.ultraGrid1.DisplayLayout.Override.CardAreaAppearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.Black;
            appearance3.ForeColor = System.Drawing.Color.White;
            this.ultraGrid1.DisplayLayout.Override.CellAppearance = appearance3;
            appearance4.BackColor = System.Drawing.Color.Black;
            appearance4.ForeColor = System.Drawing.Color.White;
            this.ultraGrid1.DisplayLayout.Override.HeaderAppearance = appearance4;
            this.ultraGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGrid1.Location = new System.Drawing.Point(22, 0);
            this.ultraGrid1.Name = "ultraGrid1";
            this.ultraGrid1.Size = new System.Drawing.Size(566, 268);
            this.ultraGrid1.TabIndex = 6;
            this.ultraGrid1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ultraGrid1.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ultraGrid1_InitializeLayout);
            this.ultraGrid1.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.ultraGrid1_InitializeRow);
            this.ultraGrid1.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.ultraGrid1_BeforeCustomRowFilterDialog);
            this.ultraGrid1.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.ultraGrid1_BeforeColumnChooserDisplayed);
            this.ultraGrid1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ultraGrid1_MouseClick);
            // 
            // grdPrint
            // 
            appearance5.BackColor = System.Drawing.Color.White;
            appearance5.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            appearance5.ForeColor = System.Drawing.Color.Black;
            this.grdPrint.DisplayLayout.Appearance = appearance5;
            this.grdPrint.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridBand1.ColHeadersVisible = false;
            this.grdPrint.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdPrint.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.grdPrint.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance6.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance6.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance6.BorderColor = System.Drawing.SystemColors.Window;
            this.grdPrint.DisplayLayout.GroupByBox.Appearance = appearance6;
            appearance7.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdPrint.DisplayLayout.GroupByBox.BandLabelAppearance = appearance7;
            this.grdPrint.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance8.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance8.BackColor2 = System.Drawing.SystemColors.Control;
            appearance8.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance8.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdPrint.DisplayLayout.GroupByBox.PromptAppearance = appearance8;
            this.grdPrint.DisplayLayout.MaxColScrollRegions = 1;
            this.grdPrint.DisplayLayout.MaxRowScrollRegions = 1;
            appearance9.BackColor = System.Drawing.SystemColors.Window;
            appearance9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grdPrint.DisplayLayout.Override.ActiveCellAppearance = appearance9;
            appearance10.BackColor = System.Drawing.SystemColors.Highlight;
            appearance10.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grdPrint.DisplayLayout.Override.ActiveRowAppearance = appearance10;
            this.grdPrint.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.grdPrint.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            this.grdPrint.DisplayLayout.Override.CardAreaAppearance = appearance11;
            appearance12.BorderColor = System.Drawing.Color.Silver;
            appearance12.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grdPrint.DisplayLayout.Override.CellAppearance = appearance12;
            this.grdPrint.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdPrint.DisplayLayout.Override.CellPadding = 0;
            this.grdPrint.DisplayLayout.Override.ColumnSizingArea = Infragistics.Win.UltraWinGrid.ColumnSizingArea.EntireColumn;
            appearance13.BackColor = System.Drawing.SystemColors.Control;
            appearance13.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance13.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance13.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance13.BorderColor = System.Drawing.SystemColors.Window;
            this.grdPrint.DisplayLayout.Override.GroupByRowAppearance = appearance13;
            appearance14.TextHAlignAsString = "Left";
            this.grdPrint.DisplayLayout.Override.HeaderAppearance = appearance14;
            this.grdPrint.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdPrint.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance15.BackColor = System.Drawing.Color.White;
            appearance15.BorderColor = System.Drawing.Color.Silver;
            appearance15.FontData.BoldAsString = "True";
            appearance15.ForeColor = System.Drawing.Color.Black;
            this.grdPrint.DisplayLayout.Override.RowAppearance = appearance15;
            this.grdPrint.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance16.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdPrint.DisplayLayout.Override.TemplateAddRowAppearance = appearance16;
            this.grdPrint.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdPrint.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdPrint.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdPrint.Location = new System.Drawing.Point(28, 115);
            this.grdPrint.Name = "grdPrint";
            this.grdPrint.Size = new System.Drawing.Size(560, 153);
            this.grdPrint.TabIndex = 8;
            this.grdPrint.Text = "ultraGrid2";
            this.grdPrint.Visible = false;
            this.grdPrint.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdPrint_InitializeLayout);
            // 
            // CustomizableCardGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.ultraGrid1);
            this.Controls.Add(this.grdPrint);
            this.Controls.Add(this.panel1);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "CustomizableCardGrid";
            this.Size = new System.Drawing.Size(487, 126);
            this.panel1.ClientArea.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGrid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdPrint)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel panel1;
        private System.Windows.Forms.Button btnColumnChooser;
        private PranaUltraGrid ultraGrid1;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdPrint;
    }
}
