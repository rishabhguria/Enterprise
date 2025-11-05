using Infragistics.Win;
using Infragistics.Win.UltraWinDataSource;
using Infragistics.Win.UltraWinGrid;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Prana.Utilities.UI.UIUtilities
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class CustomColumnChooser : System.Windows.Forms.Form
    {
        private Infragistics.Win.UltraWinGrid.UltraGridColumnChooser ultraGridColumnChooser1;
        private Infragistics.Win.Misc.UltraGridBagLayoutManager ultraGridBagLayoutManager1;
        private Infragistics.Win.UltraWinGrid.UltraCombo ultraComboBandSelector;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtFilterColumn;
        private Infragistics.Win.Misc.UltraLabel lblMsg;
        private Infragistics.Win.Misc.UltraButton ultraButtonDeleteColumn;
        private Infragistics.Win.Misc.UltraButton ultraButtonNewColumn;

        private System.ComponentModel.IContainer components;

        public CustomColumnChooser()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            ultraComboBandSelector.Visible = false;
            //ultraButtonNewColumn.Visible = false;
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
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

                if (ultraComboBandSelector != null)
                {
                    ultraComboBandSelector.Dispose();
                }

                if (txtFilterColumn != null)
                {
                    txtFilterColumn.Dispose();
                }

                if (lblMsg != null)
                {
                    lblMsg.Dispose();
                }

                if (ultraButtonDeleteColumn != null)
                {
                    ultraButtonDeleteColumn.Dispose();
                }

                if (ultraButtonNewColumn != null)
                {
                    ultraButtonNewColumn.Dispose();
                }

                if (_createdColumn != null)
                {
                    _createdColumn.Dispose();
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
            Infragistics.Win.Layout.GridBagConstraint gridBagConstraint6 = new Infragistics.Win.Layout.GridBagConstraint();
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
            Infragistics.Win.Layout.GridBagConstraint gridBagConstraint1 = new Infragistics.Win.Layout.GridBagConstraint();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Layout.GridBagConstraint gridBagConstraint4 = new Infragistics.Win.Layout.GridBagConstraint();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Layout.GridBagConstraint gridBagConstraint5 = new Infragistics.Win.Layout.GridBagConstraint();
            Infragistics.Win.Layout.GridBagConstraint gridBagConstraint3 = new Infragistics.Win.Layout.GridBagConstraint();
            Infragistics.Win.Layout.GridBagConstraint gridBagConstraint2 = new Infragistics.Win.Layout.GridBagConstraint();
            this.ultraGridColumnChooser1 = new Infragistics.Win.UltraWinGrid.UltraGridColumnChooser();
            this.ultraComboBandSelector = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.txtFilterColumn = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblMsg = new Infragistics.Win.Misc.UltraLabel();
            this.ultraGridBagLayoutManager1 = new Infragistics.Win.Misc.UltraGridBagLayoutManager(this.components);
            this.ultraButtonNewColumn = new Infragistics.Win.Misc.UltraButton();
            this.ultraButtonDeleteColumn = new Infragistics.Win.Misc.UltraButton();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridColumnChooser1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraComboBandSelector)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFilterColumn)).BeginInit();
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
            gridBagConstraint6.Fill = Infragistics.Win.Layout.FillType.Both;
            gridBagConstraint6.OriginX = 0;
            gridBagConstraint6.OriginY = 3;
            gridBagConstraint6.SpanX = 2;
            gridBagConstraint6.WeightY = 1F;
            this.ultraGridBagLayoutManager1.SetGridBagConstraint(this.ultraGridColumnChooser1, gridBagConstraint6);
            this.ultraGridColumnChooser1.Location = new System.Drawing.Point(0, 71);
            this.ultraGridColumnChooser1.MultipleBandSupport = Infragistics.Win.UltraWinGrid.MultipleBandSupport.SingleBandOnly;
            this.ultraGridColumnChooser1.Name = "ultraGridColumnChooser1";
            this.ultraGridBagLayoutManager1.SetPreferredSize(this.ultraGridColumnChooser1, new System.Drawing.Size(264, 240));
            this.ultraGridColumnChooser1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ultraGridColumnChooser1.Size = new System.Drawing.Size(274, 199);
            this.ultraGridColumnChooser1.Style = Infragistics.Win.UltraWinGrid.ColumnChooserStyle.AllColumnsWithCheckBoxes;
            this.ultraGridColumnChooser1.StyleLibraryName = "";
            this.ultraGridColumnChooser1.StyleSetName = "";
            this.ultraGridColumnChooser1.TabIndex = 0;
            this.ultraGridColumnChooser1.Text = "ultraGridColumnChooser1";
            // 
            // ultraComboBandSelector
            // 
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.ultraComboBandSelector.DisplayLayout.Appearance = appearance1;
            this.ultraComboBandSelector.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraComboBandSelector.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.ultraComboBandSelector.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ultraComboBandSelector.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.ultraComboBandSelector.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ultraComboBandSelector.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.ultraComboBandSelector.DisplayLayout.MaxColScrollRegions = 1;
            this.ultraComboBandSelector.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ultraComboBandSelector.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.ultraComboBandSelector.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.ultraComboBandSelector.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.ultraComboBandSelector.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.ultraComboBandSelector.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.ultraComboBandSelector.DisplayLayout.Override.CellAppearance = appearance8;
            this.ultraComboBandSelector.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.ultraComboBandSelector.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Client;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.ultraComboBandSelector.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.ultraComboBandSelector.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.ultraComboBandSelector.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.ultraComboBandSelector.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.ultraComboBandSelector.DisplayLayout.Override.RowAppearance = appearance11;
            this.ultraComboBandSelector.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ultraComboBandSelector.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.ultraComboBandSelector.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ultraComboBandSelector.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ultraComboBandSelector.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.ultraComboBandSelector.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            gridBagConstraint1.Fill = Infragistics.Win.Layout.FillType.Horizontal;
            gridBagConstraint1.Insets.Bottom = 1;
            gridBagConstraint1.Insets.Left = 1;
            gridBagConstraint1.Insets.Right = 1;
            gridBagConstraint1.Insets.Top = 1;
            gridBagConstraint1.OriginX = 0;
            gridBagConstraint1.OriginY = 0;
            gridBagConstraint1.SpanX = 2;
            this.ultraGridBagLayoutManager1.SetGridBagConstraint(this.ultraComboBandSelector, gridBagConstraint1);
            this.ultraComboBandSelector.Location = new System.Drawing.Point(1, 1);
            this.ultraComboBandSelector.Name = "ultraComboBandSelector";
            this.ultraGridBagLayoutManager1.SetPreferredSize(this.ultraComboBandSelector, new System.Drawing.Size(100, 21));
            this.ultraComboBandSelector.Size = new System.Drawing.Size(272, 23);
            this.ultraComboBandSelector.TabIndex = 3;
            this.ultraComboBandSelector.RowSelected += new Infragistics.Win.UltraWinGrid.RowSelectedEventHandler(this.ultraComboBandSelector_RowSelected);
            // 
            // txtFilterColumn
            // 
            appearance13.FontData.BoldAsString = "False";
            appearance13.ForeColor = System.Drawing.Color.DimGray;
            this.txtFilterColumn.Appearance = appearance13;
            gridBagConstraint4.Fill = Infragistics.Win.Layout.FillType.Horizontal;
            gridBagConstraint4.Insets.Bottom = 1;
            gridBagConstraint4.Insets.Left = 1;
            gridBagConstraint4.Insets.Right = 1;
            gridBagConstraint4.Insets.Top = 1;
            gridBagConstraint4.OriginX = 0;
            gridBagConstraint4.OriginY = 1;
            gridBagConstraint4.SpanX = 2;
            this.ultraGridBagLayoutManager1.SetGridBagConstraint(this.txtFilterColumn, gridBagConstraint4);
            this.txtFilterColumn.Location = new System.Drawing.Point(1, 24);
            this.txtFilterColumn.Name = "txtFilterColumn";
            this.ultraGridBagLayoutManager1.SetPreferredSize(this.txtFilterColumn, new System.Drawing.Size(100, 23));
            this.txtFilterColumn.Size = new System.Drawing.Size(272, 22);
            this.txtFilterColumn.TabIndex = 1;
            this.txtFilterColumn.Text = "Filter Column";
            this.txtFilterColumn.TextChanged += new System.EventHandler(this.txtFilterColumn_TextChanged);
            this.txtFilterColumn.Enter += new System.EventHandler(this.txtFilterColumn_Enter);
            this.txtFilterColumn.Leave += new System.EventHandler(this.txtFilterColumn_Leave);
            // 
            // lblMsg
            // 
            appearance14.ForeColor = System.Drawing.Color.Red;
            this.lblMsg.Appearance = appearance14;
            this.lblMsg.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            gridBagConstraint5.Fill = Infragistics.Win.Layout.FillType.Horizontal;
            gridBagConstraint5.Insets.Bottom = 1;
            gridBagConstraint5.Insets.Left = 1;
            gridBagConstraint5.Insets.Right = 1;
            gridBagConstraint5.Insets.Top = 1;
            gridBagConstraint5.OriginX = 0;
            gridBagConstraint5.OriginY = 2;
            gridBagConstraint5.SpanX = 2;
            gridBagConstraint5.WeightX = 1F;
            this.ultraGridBagLayoutManager1.SetGridBagConstraint(this.lblMsg, gridBagConstraint5);
            this.lblMsg.Location = new System.Drawing.Point(1, 49);
            this.lblMsg.Name = "lblMsg";
            this.ultraGridBagLayoutManager1.SetPreferredSize(this.lblMsg, new System.Drawing.Size(95, 21));
            this.lblMsg.Size = new System.Drawing.Size(272, 21);
            this.lblMsg.TabIndex = 2;
            this.lblMsg.Visible = false;
            // 
            // ultraGridBagLayoutManager1
            // 
            this.ultraGridBagLayoutManager1.ContainerControl = this;
            this.ultraGridBagLayoutManager1.ExpandToFitHeight = true;
            this.ultraGridBagLayoutManager1.ExpandToFitWidth = true;
            // 
            // ultraButtonNewColumn
            // 
            gridBagConstraint3.Fill = Infragistics.Win.Layout.FillType.Horizontal;
            gridBagConstraint3.Insets.Bottom = 2;
            gridBagConstraint3.Insets.Left = 2;
            gridBagConstraint3.Insets.Right = 2;
            gridBagConstraint3.Insets.Top = 2;
            gridBagConstraint3.OriginX = 0;
            gridBagConstraint3.OriginY = 4;
            gridBagConstraint3.WeightX = 1F;
            this.ultraGridBagLayoutManager1.SetGridBagConstraint(this.ultraButtonNewColumn, gridBagConstraint3);
            this.ultraButtonNewColumn.Location = new System.Drawing.Point(2, 272);
            this.ultraButtonNewColumn.Name = "ultraButtonNewColumn";
            this.ultraGridBagLayoutManager1.SetPreferredSize(this.ultraButtonNewColumn, new System.Drawing.Size(100, 23));
            this.ultraButtonNewColumn.Size = new System.Drawing.Size(133, 23);
            this.ultraButtonNewColumn.TabIndex = 1;
            this.ultraButtonNewColumn.Text = "New Column";
            this.ultraButtonNewColumn.Visible = false;
            this.ultraButtonNewColumn.Click += new System.EventHandler(this.ultraButtonNewColumn_Click);
            // 
            // ultraButtonDeleteColumn
            // 
            gridBagConstraint2.Fill = Infragistics.Win.Layout.FillType.Horizontal;
            gridBagConstraint2.Insets.Bottom = 2;
            gridBagConstraint2.Insets.Left = 2;
            gridBagConstraint2.Insets.Right = 2;
            gridBagConstraint2.Insets.Top = 2;
            gridBagConstraint2.OriginX = 1;
            gridBagConstraint2.OriginY = 4;
            gridBagConstraint2.WeightX = 1F;
            this.ultraGridBagLayoutManager1.SetGridBagConstraint(this.ultraButtonDeleteColumn, gridBagConstraint2);
            this.ultraButtonDeleteColumn.Location = new System.Drawing.Point(139, 272);
            this.ultraButtonDeleteColumn.Name = "ultraButtonDeleteColumn";
            this.ultraGridBagLayoutManager1.SetPreferredSize(this.ultraButtonDeleteColumn, new System.Drawing.Size(100, 23));
            this.ultraButtonDeleteColumn.Size = new System.Drawing.Size(133, 23);
            this.ultraButtonDeleteColumn.TabIndex = 2;
            this.ultraButtonDeleteColumn.Text = "Delete Column";
            this.ultraButtonDeleteColumn.Visible = false;
            this.ultraButtonDeleteColumn.Click += new System.EventHandler(this.ultraButtonDeleteColumn_Click);
            // 
            // CustomColumnChooser
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(274, 297);
            this.Controls.Add(this.ultraComboBandSelector);
            this.Controls.Add(this.ultraButtonDeleteColumn);
            this.Controls.Add(this.ultraButtonNewColumn);
            this.Controls.Add(this.ultraGridColumnChooser1);
            this.Controls.Add(this.txtFilterColumn);
            this.Controls.Add(this.lblMsg);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Name = "CustomColumnChooser";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Column Chooser";
            this.Load += new System.EventHandler(this.CustomColumnChooser_Load);
            this.FormClosed += CustomColumnChooser_FormClosed;
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridColumnChooser1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraComboBandSelector)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFilterColumn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridBagLayoutManager1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public void CustomColumnChooser_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (ultraGridColumnChooser1 != null && ultraGridColumnChooser1.CurrentBand != null)
                {
                    foreach (UltraGridColumn column in ultraGridColumnChooser1.CurrentBand.Columns)
                    {
                        //column.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                        if (column != null && column.ExcludeFromColumnChooser == ExcludeFromColumnChooser.True && lstColumns != null && lstColumns.Contains(column))
                        {
                            column.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                        }
                    }
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
        #endregion

        // public delegate void AddNewColumn(bool newColumnToBeAdded);
        //public event AddNewColumn AddNewColumnInProgress = null;
        public event EventHandler<EventArgs<bool>> AddNewColumnInProgress;

        #region ultraButtonNewColumn_Click
        private bool _addToOtherGrids;

        public bool AddToOtherGrids
        {
            get { return _addToOtherGrids; }
            set { _addToOtherGrids = value; }
        }

        private UltraGridColumn _createdColumn = null;

        public UltraGridColumn NewCreatedColumn
        {
            get { return _createdColumn; }
            set { _createdColumn = value; }
        }

        private string _deletedColumnKey = string.Empty;

        public string DeletedColumnKey
        {
            get { return _deletedColumnKey; }
            set { _deletedColumnKey = value; }
        }


        private void ultraButtonNewColumn_Click(object sender, System.EventArgs e)
        {
            if (AddNewColumnInProgress != null)
            {
                AddNewColumnInProgress(this, new EventArgs<bool>(true));
            }

            UltraGridBand selectedBand = this.ultraGridColumnChooser1.CurrentBand;

            if (null == selectedBand)
                return;

            int calculatedColumnsInBand = 0;
            foreach (UltraGridColumn col in selectedBand.Columns)
            {
                if (col.Formula != null && col.Formula != string.Empty)
                {
                    calculatedColumnsInBand = calculatedColumnsInBand + 1;
                }
            }

            if (calculatedColumnsInBand >= _numberOfCustomColumnsAllowed)
            {
                MessageBox.Show("Maximum number of calculated columns reached");
                return;
            }
            NewColumnDialog dlg = new NewColumnDialog();
            dlg.Band = selectedBand;
            dlg.ParentGridName = this.grid.Name;


            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _createdColumn = dlg.CreatedColumn;
                _addToOtherGrids = dlg.AddToAllGrids;
            }
            else
            {
                _createdColumn = null;
                _addToOtherGrids = false;
            }

            if (AddNewColumnInProgress != null)
            {
                AddNewColumnInProgress(this, new EventArgs<bool>(false));
            }
        }

        #endregion // ultraButtonNewColumn_Click

        #region ultraButtonDeleteColumn_Click

        private void ultraButtonDeleteColumn_Click(object sender, System.EventArgs e)
        {
            // Delete the column that's currently selected in the column chooser control.
            // 

            UltraGridColumn column = this.ultraGridColumnChooser1.CurrentSelectedItem as UltraGridColumn;
            if (null == column)
            {
                MessageBox.Show(this, "Please select a column to delete.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (column.Formula == null)
            {
                MessageBox.Show(this, "Only custom columns can be deleted.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (column.IsBound)
            {
                MessageBox.Show(this, "Only custom columns can be deleted.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult dlgResult = MessageBox.Show(this,
                string.Format("Deleting {0} column. Continue?", column.Header.Caption),
                this.Text,
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question);

            if (DialogResult.OK == dlgResult)
            {
                _deletedColumnKey = column.Key;
                if (customColumnRemoved != null)
                {
                    customColumnRemoved(this, new EventArgs<string>(column.Key));

                }
                //column.Band.Columns.Remove(column);
            }
        }

        //public delegate void ColumnRemoved(string columnKey);
        //public event ColumnRemoved customColumnRemoved;
        public event EventHandler<EventArgs<string>> customColumnRemoved;
        #endregion // ultraButtonDeleteColumn_Click

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
                    this.InitializeBandsCombo(this.grid);

                    // Select the first band in the band selector.
                    if (this.ultraComboBandSelector.Rows.Count > 0)
                        this.ultraComboBandSelector.SelectedRow = this.ultraComboBandSelector.Rows[0];
                }
            }
        }

        #endregion // Grid

        #region CurrentBand

        /// <summary>
        /// Gets or sets the band whose columns are being displayed.
        /// </summary>
        public UltraGridBand CurrentBand
        {
            get
            {
                return this.ColumnChooserControl.CurrentBand;
            }
            set
            {
                if (null != value && (null == this.Grid || this.Grid != value.Layout.Grid))
                    throw new ArgumentException();

                this.ultraComboBandSelector.Value = value;
            }
        }

        #endregion // CurrentBand

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

        #region InitializeBandsCombo

        private void InitializeBandsCombo(UltraGridBase grid)
        {
            this.ultraComboBandSelector.SetDataBinding(null, null);
            if (null == grid)
                return;

            // Create the data source that we can bind to UltraCombo for displaying 
            // list of bands. The datasource will have two columns. One that contains
            // the instances of UltraGridBand and the other that contains the text
            // representation of the bands.
            UltraDataSource bandsUDS = new UltraDataSource();
            bandsUDS.Band.Columns.Add("Band", typeof(UltraGridBand));
            bandsUDS.Band.Columns.Add("DisplayText", typeof(string));

            foreach (UltraGridBand band in grid.DisplayLayout.Bands)
            {
                if (!this.IsBandExcluded(band))
                {
                    bandsUDS.Rows.Add(new object[] { band, band.Header.Caption });
                }
            }

            this.ultraComboBandSelector.DisplayMember = "DisplayText";
            this.ultraComboBandSelector.ValueMember = "Band";
            this.ultraComboBandSelector.SetDataBinding(bandsUDS, null);

            // Hide the Band column.
            this.ultraComboBandSelector.DisplayLayout.Bands[0].Columns["Band"].Hidden = true;

            // Hide the column headers.
            this.ultraComboBandSelector.DisplayLayout.Bands[0].ColHeadersVisible = false;

            // Set some properties to improve the look & feel of the ultra combo.
            this.ultraComboBandSelector.DropDownWidth = 0;
            this.ultraComboBandSelector.DisplayLayout.Override.HotTrackRowAppearance.BackColor = Color.LightYellow;
            this.ultraComboBandSelector.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            this.ultraComboBandSelector.DisplayLayout.BorderStyle = UIElementBorderStyle.Solid;
            this.ultraComboBandSelector.DisplayLayout.Appearance.BorderColor = SystemColors.Highlight;
        }

        #endregion // InitializeBandsCombo

        #region IsBandExcluded

        /// <summary>
        /// Checks to see if the ExcludeFromColumnChooser property of the band or
        /// any of its ancestor bands is set to True.
        /// </summary>
        /// <param name="band"></param>
        /// <returns></returns>
        private bool IsBandExcluded(UltraGridBand band)
        {
            while (null != band)
            {
                if (ExcludeFromColumnChooser.True == band.ExcludeFromColumnChooser)
                    return true;

                band = band.ParentBand;
            }

            return false;
        }

        #endregion // IsBandExcluded

        #region ultraComboBandSelector_RowSelected

        private void ultraComboBandSelector_RowSelected(object sender, Infragistics.Win.UltraWinGrid.RowSelectedEventArgs e)
        {
            if (null == this.Grid || this.Grid.IsDisposed)
                return;

            UltraGridBand selectedBand = this.ultraComboBandSelector.Value as UltraGridBand;
            if (null == selectedBand)
            {
                System.Diagnostics.Debug.Assert(false);
                selectedBand = this.Grid.DisplayLayout.Bands[0];
                this.ultraComboBandSelector.Value = selectedBand;
            }

            this.ultraGridColumnChooser1.CurrentBand = selectedBand;
        }

        #endregion // ultraComboBandSelector_RowSelected

        private int _numberOfCustomColumnsAllowed = 100;

        public int NumberOfCustomColumnsAllowed
        {
            get { return _numberOfCustomColumnsAllowed; }
            set { _numberOfCustomColumnsAllowed = value; }
        }
        private bool _showCustomButtons = true;

        public bool ShowCustomButtons
        {
            get { return _showCustomButtons; }
            set
            {
                _showCustomButtons = value;
                ultraButtonNewColumn.Visible = value;
                ultraButtonDeleteColumn.Visible = value;
            }
        }

        #region Added by: Bharat Raturi, 5 aug 2014, Code for filtering the columns in the column chooser 

        //private void txtFilterColumn_GotFocus(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (string.IsNullOrWhiteSpace(txtFilterColumn.Text.Trim()) || txtFilterColumn.Text.Equals("Filter Column"))
        //        {
        //            txtFilterColumn.Text = string.Empty;
        //            txtFilterColumn.Appearance.ForeColor = Color.Black;
        //            //txtFilterColumn.Appearance.FontData.Bold = DefaultableBoolean.True;
        //        }

        //        if (lstColumns.Count == 0)
        //        {
        //            foreach (UltraGridColumn col in ultraGridColumnChooser1.CurrentBand.Columns)
        //            {
        //                lstColumns.Add(col);
        //            }
        //            //this.txtFilterColumn.GotFocus -= new EventHandler(this.txtFilterColumn_GotFocus);
        //        }
        //        //if (!string.IsNullOrWhiteSpace(txtFilterColumn.Text.Trim()) && txtFilterColumn.Text.Trim().Equals("Filter Column"))
        //        //{
        //        //    txtFilterColumn.Text = string.Empty;
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = EnterpriseLibraryManager.HandleException(ex, EnterpriseLibraryConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        List<UltraGridColumn> lstColumns;

        /// <summary>
        /// added by: Bharat Raturi, 5 Aug 2014
        /// Filter the columns according to the text of combo box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFilterColumn_TextChanged(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Text = string.Empty;
                lblMsg.Visible = false;
                if (!string.IsNullOrWhiteSpace(txtFilterColumn.Text.Trim()))
                {
                    List<UltraGridColumn> lstcols = new List<UltraGridColumn>();
                    lstcols = (from lst in lstColumns
                               where lst.Header.Caption.ToLower().Contains(txtFilterColumn.Text.Trim().ToLower())
                               select lst).ToList();
                    if (ultraGridColumnChooser1.CurrentBand != null && lstcols.Count > 0)
                    {
                        // Modify by Sumeet Kumar 18/2/2015 (Commented because the following was not working properly)
                        //foreach (UltraGridColumn column in ultraGridColumnChooser1.CurrentBand.Columns)
                        foreach (UltraGridColumn column in lstColumns)
                        {
                            if (ultraGridColumnChooser1 != null && ultraGridColumnChooser1.CurrentBand != null && ultraGridColumnChooser1.CurrentBand.Columns.Count > 0 && ultraGridColumnChooser1.CurrentBand.Columns.Exists(column.Key))
                            {
                                column.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                            }
                        }
                        foreach (UltraGridColumn column in lstcols)
                        {
                            if (ultraGridColumnChooser1 != null && ultraGridColumnChooser1.CurrentBand != null && ultraGridColumnChooser1.CurrentBand.Columns.Count > 0 && ultraGridColumnChooser1.CurrentBand.Columns.Exists(column.Key))
                            {
                                column.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                            }
                        }
                    }
                    else if (!txtFilterColumn.Text.Equals("Filter Column"))
                    {
                        lblMsg.Text = "No matches found.";
                        lblMsg.Visible = true;
                        // Modify by Sumeet Kumar 18/2/2015 (Commented because the following was not working properly)
                        //foreach (UltraGridColumn column in ultraGridColumnChooser1.CurrentBand.Columns)
                        foreach (UltraGridColumn column in lstColumns)
                        {
                            if (ultraGridColumnChooser1 != null && ultraGridColumnChooser1.CurrentBand != null && ultraGridColumnChooser1.CurrentBand.Columns.Count > 0 && ultraGridColumnChooser1.CurrentBand.Columns.Exists(column.Key))
                            {
                                column.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                            }
                        }
                    }
                }
                else
                {
                    if (ultraGridColumnChooser1.CurrentBand != null)
                    {
                        // Modify by Sumeet Kumar 18/2/2015 (Commented because the following was not working properly)
                        //foreach (UltraGridColumn column in ultraGridColumnChooser1.CurrentBand.Columns)
                        foreach (UltraGridColumn column in lstColumns)
                        {
                            if (ultraGridColumnChooser1 != null && ultraGridColumnChooser1.CurrentBand != null && ultraGridColumnChooser1.CurrentBand.Columns.Count > 0 && ultraGridColumnChooser1.CurrentBand.Columns.Exists(column.Key))
                            {
                                column.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Load the default text and change the text color if the textbox is empty
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFilterColumn_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFilterColumn.Text.Trim()))
            {
                txtFilterColumn.Text = "Filter Column";
                txtFilterColumn.Appearance.ForeColor = Color.DimGray;
                //txtFilterColumn.Appearance.FontData.Bold = DefaultableBoolean.True; 
            }
        }

        /// <summary>
        /// erase the default text and change the color of the text if default text is there 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFilterColumn_Enter(object sender, EventArgs e)
        {
            try
            {
                if (txtFilterColumn != null && !txtFilterColumn.IsDisposed && !txtFilterColumn.Disposing)
                {
                    if (string.IsNullOrWhiteSpace(txtFilterColumn.Text.Trim()) || txtFilterColumn.Text.Equals("Filter Column"))
                    {
                        txtFilterColumn.Text = string.Empty;
                        txtFilterColumn.Appearance.ForeColor = Color.Black;
                        //txtFilterColumn.Appearance.FontData.Bold = DefaultableBoolean.False;
                    }
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

            //if (lstColumns.Count == 0)
            //{
            //    foreach (UltraGridColumn col in ultraGridColumnChooser1.CurrentBand.Columns)
            //    {
            //        lstColumns.Add(col.Key.ToString());
            //    }
            //    //this.txtFilterColumn.GotFocus -= new EventHandler(this.txtFilterColumn_GotFocus);
            //}
        }

        /// <summary>
        /// Show all the columns in the column chooser when the column chooser loads 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomColumnChooser_Load(object sender, EventArgs e)
        {
            try
            {
                LoadColumnList();
                //SetSelectCheckState();
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

        /// <summary>
        ///Added by: Bharat raturi, 19 aug 2014
        /// Populate the List with the Columns of the grid
        /// </summary>
        private void LoadColumnList()
        {
            try
            {
                lstColumns = new List<UltraGridColumn>();
                foreach (UltraGridColumn column in ultraGridColumnChooser1.CurrentBand.Columns)
                {
                    //column.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    if (column.ExcludeFromColumnChooser != ExcludeFromColumnChooser.True && !lstColumns.Contains(column))
                    {
                        lstColumns.Add(column);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        #endregion
    }
}
