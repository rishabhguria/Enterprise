using Infragistics.Win.UltraWinGrid;
using System;
using System.Windows.Forms;

namespace Prana.Utilities.UI.UIUtilities
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class CustomColumnChooserOld : System.Windows.Forms.Form
    {
        private Infragistics.Win.UltraWinGrid.UltraGridColumnChooser ultraGridColumnChooser1;
        private Infragistics.Win.Misc.UltraGridBagLayoutManager ultraGridBagLayoutManager1;
        private System.ComponentModel.IContainer components;

        public CustomColumnChooserOld()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            ultraGridColumnChooser1.Click += new EventHandler(ultraGridColumnChooser1_Click);
            ultraGridColumnChooser1.MouseClick += new MouseEventHandler(ultraGridColumnChooser1_MouseClick);
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        void ultraGridColumnChooser1_MouseClick(object sender, MouseEventArgs e)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        void ultraGridColumnChooser1_Click(object sender, EventArgs e)
        {
            throw new Exception("The method or operation is not implemented.");
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

                if (ultraGridColumnChooser1 != null)
                {
                    ultraGridColumnChooser1.Dispose();
                }

                if (ultraGridBagLayoutManager1 != null)
                {
                    ultraGridBagLayoutManager1.Dispose();
                }

                if (grid != null)
                {
                    grid.Dispose();
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
            Infragistics.Win.Layout.GridBagConstraint gridBagConstraint1 = new Infragistics.Win.Layout.GridBagConstraint();
            this.ultraGridColumnChooser1 = new Infragistics.Win.UltraWinGrid.UltraGridColumnChooser();
            this.ultraGridBagLayoutManager1 = new Infragistics.Win.Misc.UltraGridBagLayoutManager(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridColumnChooser1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridBagLayoutManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraGridColumnChooser1
            // 
            this.ultraGridColumnChooser1.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.ultraGridColumnChooser1.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGridColumnChooser1.DisplayLayout.MaxColScrollRegions = 1;
            this.ultraGridColumnChooser1.DisplayLayout.MaxRowScrollRegions = 1;
            this.ultraGridColumnChooser1.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed;
            this.ultraGridColumnChooser1.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.None;
            this.ultraGridColumnChooser1.DisplayLayout.Override.AllowRowLayoutCellSizing = Infragistics.Win.UltraWinGrid.RowLayoutSizing.None;
            this.ultraGridColumnChooser1.DisplayLayout.Override.AllowRowLayoutLabelSizing = Infragistics.Win.UltraWinGrid.RowLayoutSizing.None;
            this.ultraGridColumnChooser1.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.ultraGridColumnChooser1.DisplayLayout.Override.CellPadding = 2;
            this.ultraGridColumnChooser1.DisplayLayout.Override.ExpansionIndicator = Infragistics.Win.UltraWinGrid.ShowExpansionIndicator.Never;
            this.ultraGridColumnChooser1.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.Select;
            this.ultraGridColumnChooser1.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGridColumnChooser1.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.AutoFixed;
            this.ultraGridColumnChooser1.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.ultraGridColumnChooser1.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.ultraGridColumnChooser1.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.ultraGridColumnChooser1.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.None;
            this.ultraGridColumnChooser1.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ultraGridColumnChooser1.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            gridBagConstraint1.Fill = Infragistics.Win.Layout.FillType.Both;
            gridBagConstraint1.OriginX = 0;
            gridBagConstraint1.OriginY = 1;
            gridBagConstraint1.SpanX = 2;
            gridBagConstraint1.WeightY = 1F;
            this.ultraGridBagLayoutManager1.SetGridBagConstraint(this.ultraGridColumnChooser1, gridBagConstraint1);
            this.ultraGridColumnChooser1.Location = new System.Drawing.Point(0, 0);
            this.ultraGridColumnChooser1.MultipleBandSupport = Infragistics.Win.UltraWinGrid.MultipleBandSupport.SingleBandOnly;
            this.ultraGridColumnChooser1.Name = "ultraGridColumnChooser1";
            this.ultraGridBagLayoutManager1.SetPreferredSize(this.ultraGridColumnChooser1, new System.Drawing.Size(264, 240));
            this.ultraGridColumnChooser1.Size = new System.Drawing.Size(192, 294);
            this.ultraGridColumnChooser1.Style = Infragistics.Win.UltraWinGrid.ColumnChooserStyle.AllColumnsWithCheckBoxes;
            this.ultraGridColumnChooser1.StyleLibraryName = "";
            this.ultraGridColumnChooser1.StyleSetName = "";
            this.ultraGridColumnChooser1.TabIndex = 0;
            this.ultraGridColumnChooser1.Text = "ultraGridColumnChooser1";
            // 
            // ultraGridBagLayoutManager1
            // 
            this.ultraGridBagLayoutManager1.ContainerControl = this;
            this.ultraGridBagLayoutManager1.ExpandToFitHeight = true;
            this.ultraGridBagLayoutManager1.ExpandToFitWidth = true;
            // 
            // CustomColumnChooser
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(192, 294);
            this.Controls.Add(this.ultraGridColumnChooser1);
            this.Name = "CustomColumnChooser";
            this.Text = "Column Chooser";
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridColumnChooser1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridBagLayoutManager1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        #region Grid

        private UltraGridBase grid = null;

        /// <summary>
        /// Gets or sets the UltraGrid instances whose columns are displayed in the column chooser.
        /// </summary>
        public UltraGridBase Grid
        {
            get
            {
                return this.grid;
            }
            set
            {
                if (value != this.grid)
                {
                    this.grid = value;

                    this.ultraGridColumnChooser1.SourceGrid = this.grid;
                }
            }
        }

        #endregion // Grid

        #region ColumnChooserControl

        /// <summary>
        /// Returns the column chooser control.
        /// </summary>
        public UltraGridColumnChooser ColumnChooserControl
        {
            get
            {
                return this.ultraGridColumnChooser1;
            }
        }

        #endregion // ColumnChooserControl

    }
}
