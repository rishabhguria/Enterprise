using Prana.Utilities.UI.UIUtilities;

namespace Prana.CorporateActionNew.Controls
{
    partial class ctrlCAEntry
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
                if (_caTable != null)
                {
                    _caTable.Dispose();
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            this.grdCorporateActionEntry = new PranaUltraGrid();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuPreviewUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPreviewRedo = new System.Windows.Forms.ToolStripMenuItem();
            this.ultraGridExcelExporter1 = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.grdCorporateActionEntry)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdCorporateActionEntry
            // 
            this.grdCorporateActionEntry.ContextMenuStrip = this.contextMenuStrip1;
            appearance1.BackColor = System.Drawing.Color.Black;
            this.grdCorporateActionEntry.DisplayLayout.Appearance = appearance1;
            ultraGridBand1.CardSettings.ShowCaption = false;
            ultraGridBand1.CardView = true;
            this.grdCorporateActionEntry.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdCorporateActionEntry.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.grdCorporateActionEntry.DisplayLayout.MaxColScrollRegions = 1;
            this.grdCorporateActionEntry.DisplayLayout.MaxRowScrollRegions = 1;
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(33)))));
            appearance2.ForeColor = System.Drawing.Color.White;
            this.grdCorporateActionEntry.DisplayLayout.Override.ActiveCardCaptionAppearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.LightSlateGray;
            appearance3.BackColor2 = System.Drawing.Color.DarkSlateGray;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            appearance3.BorderColor = System.Drawing.Color.DimGray;
            appearance3.ForeColor = System.Drawing.Color.White;
            this.grdCorporateActionEntry.DisplayLayout.Override.ActiveRowAppearance = appearance3;
            this.grdCorporateActionEntry.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.None;
            this.grdCorporateActionEntry.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.grdCorporateActionEntry.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.grdCorporateActionEntry.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            this.grdCorporateActionEntry.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(33)))));
            appearance4.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(33)))));
            appearance4.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance4.ForeColor = System.Drawing.Color.White;
            this.grdCorporateActionEntry.DisplayLayout.Override.CardAreaAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(33)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(33)))));
            appearance5.ForeColor = System.Drawing.Color.White;
            this.grdCorporateActionEntry.DisplayLayout.Override.CardCaptionAppearance = appearance5;
            appearance6.BorderColor = System.Drawing.Color.Transparent;
            this.grdCorporateActionEntry.DisplayLayout.Override.CellAppearance = appearance6;
            this.grdCorporateActionEntry.DisplayLayout.Override.ColumnSizingArea = Infragistics.Win.UltraWinGrid.ColumnSizingArea.EntireColumn;
            appearance7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(33)))));
            appearance7.ForeColor = System.Drawing.Color.White;
            this.grdCorporateActionEntry.DisplayLayout.Override.HeaderAppearance = appearance7;
            this.grdCorporateActionEntry.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed;
            appearance8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(33)))));
            appearance8.ForeColor = System.Drawing.Color.White;
            this.grdCorporateActionEntry.DisplayLayout.Override.SelectedCardCaptionAppearance = appearance8;
            this.grdCorporateActionEntry.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdCorporateActionEntry.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdCorporateActionEntry.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdCorporateActionEntry.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            this.grdCorporateActionEntry.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdCorporateActionEntry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdCorporateActionEntry.Location = new System.Drawing.Point(0, 0);
            this.grdCorporateActionEntry.Margin = new System.Windows.Forms.Padding(4);
            this.grdCorporateActionEntry.Name = "grdCorporateActionEntry";
            this.grdCorporateActionEntry.Size = new System.Drawing.Size(799, 171);
            this.grdCorporateActionEntry.TabIndex = 1;
            this.grdCorporateActionEntry.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            this.grdCorporateActionEntry.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnUpdate;
            this.grdCorporateActionEntry.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdCorporateActionEntry_AfterCellUpdate);
            this.grdCorporateActionEntry.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdCorporateActionEntry_CellChange);
            this.grdCorporateActionEntry.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grdCorporateActionEntry_MouseDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuPreviewUndo,
            this.mnuPreviewRedo});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(148, 48);
            // 
            // mnuPreviewUndo
            // 
            this.mnuPreviewUndo.Enabled = false;
            this.mnuPreviewUndo.Name = "mnuPreviewUndo";
            this.mnuPreviewUndo.Size = new System.Drawing.Size(147, 22);
            this.mnuPreviewUndo.Text = "Preview Undo";
            this.mnuPreviewUndo.Click += new System.EventHandler(this.mnuPreviewUndo_Click);
            // 
            // mnuPreviewRedo
            // 
            this.mnuPreviewRedo.Enabled = false;
            this.mnuPreviewRedo.Name = "mnuPreviewRedo";
            this.mnuPreviewRedo.Size = new System.Drawing.Size(147, 22);
            this.mnuPreviewRedo.Text = "Preview Redo";
            this.mnuPreviewRedo.Click += new System.EventHandler(this.mnuPreviewRedo_Click);
            // 
            // ctrlCAEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.Controls.Add(this.grdCorporateActionEntry);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ctrlCAEntry";
            this.Size = new System.Drawing.Size(799, 171);
            ((System.ComponentModel.ISupportInitialize)(this.grdCorporateActionEntry)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private PranaUltraGrid grdCorporateActionEntry;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuPreviewUndo;
        private System.Windows.Forms.ToolStripMenuItem mnuPreviewRedo;
        private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter ultraGridExcelExporter1;
    }
}
