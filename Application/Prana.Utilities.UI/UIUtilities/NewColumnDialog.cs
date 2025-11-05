using Infragistics.Win;
using Infragistics.Win.UltraWinCalcManager;
using Infragistics.Win.UltraWinCalcManager.FormulaBuilder;
using Infragistics.Win.UltraWinGrid;
using Prana.Global;
using System;
using System.Windows.Forms;

namespace Prana.Utilities.UI.UIUtilities
{
    /// <summary>
    /// Summary description for NewColumnDialog.
    /// </summary>
    public class NewColumnDialog : System.Windows.Forms.Form
    {
        private Infragistics.Win.UltraWinEditors.UltraTextEditor ultraTextEditorName;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor ultraComboEditorType;
        private Infragistics.Win.Misc.UltraButton ultraButtonOk;
        private Infragistics.Win.Misc.UltraButton ultraButtonCancel;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor ultraTextEditorFormula;
        private Infragistics.Win.Misc.UltraLabel ultraLabelFormula;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkAddAll;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public NewColumnDialog()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            _addToAllGrids = true;
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

                if (ultraTextEditorName != null)
                {
                    ultraTextEditorName.Dispose();
                }

                if (ultraLabel1 != null)
                {
                    ultraLabel1.Dispose();
                }

                if (ultraLabel2 != null)
                {
                    ultraLabel2.Dispose();
                }

                if (ultraComboEditorType != null)
                {
                    ultraComboEditorType.Dispose();
                }

                if (ultraButtonOk != null)
                {
                    ultraButtonOk.Dispose();
                }

                if (ultraButtonCancel != null)
                {
                    ultraButtonCancel.Dispose();
                }

                if (ultraTextEditorFormula != null)
                {
                    ultraTextEditorFormula.Dispose();
                }

                if (ultraLabelFormula != null)
                {
                    ultraLabelFormula.Dispose();
                }

                if (chkAddAll != null)
                {
                    chkAddAll.Dispose();
                }

                if (band != null)
                {
                    band.Dispose();
                }

                if (createdColumn != null)
                {
                    createdColumn.Dispose();
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
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this.ultraTextEditorName = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraComboEditorType = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultraButtonOk = new Infragistics.Win.Misc.UltraButton();
            this.ultraButtonCancel = new Infragistics.Win.Misc.UltraButton();
            this.ultraTextEditorFormula = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabelFormula = new Infragistics.Win.Misc.UltraLabel();
            this.chkAddAll = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTextEditorName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraComboEditorType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTextEditorFormula)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraTextEditorName
            // 
            this.ultraTextEditorName.Location = new System.Drawing.Point(112, 16);
            this.ultraTextEditorName.Name = "ultraTextEditorName";
            this.ultraTextEditorName.Size = new System.Drawing.Size(144, 21);
            this.ultraTextEditorName.TabIndex = 0;
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.Location = new System.Drawing.Point(8, 16);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(100, 23);
            this.ultraLabel1.TabIndex = 1;
            this.ultraLabel1.Text = "Name";
            // 
            // ultraLabel2
            // 
            this.ultraLabel2.Location = new System.Drawing.Point(8, 40);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(100, 23);
            this.ultraLabel2.TabIndex = 2;
            this.ultraLabel2.Text = "Type";
            // 
            // ultraComboEditorType
            // 
            this.ultraComboEditorType.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.ultraComboEditorType.Location = new System.Drawing.Point(112, 40);
            this.ultraComboEditorType.Name = "ultraComboEditorType";
            this.ultraComboEditorType.Size = new System.Drawing.Size(144, 21);
            this.ultraComboEditorType.TabIndex = 3;
            // 
            // ultraButtonOk
            // 
            this.ultraButtonOk.Location = new System.Drawing.Point(48, 119);
            this.ultraButtonOk.Name = "ultraButtonOk";
            this.ultraButtonOk.Size = new System.Drawing.Size(75, 23);
            this.ultraButtonOk.TabIndex = 4;
            this.ultraButtonOk.Text = "&Ok";
            this.ultraButtonOk.Click += new System.EventHandler(this.ultraButtonOk_Click);
            // 
            // ultraButtonCancel
            // 
            this.ultraButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ultraButtonCancel.Location = new System.Drawing.Point(136, 119);
            this.ultraButtonCancel.Name = "ultraButtonCancel";
            this.ultraButtonCancel.Size = new System.Drawing.Size(75, 23);
            this.ultraButtonCancel.TabIndex = 5;
            this.ultraButtonCancel.Text = "&Cancel";
            this.ultraButtonCancel.Click += new System.EventHandler(this.ultraButtonCancel_Click);
            // 
            // ultraTextEditorFormula
            // 
            appearance1.FontData.BoldAsString = "True";
            appearance1.TextHAlignAsString = "Center";
            appearance1.TextVAlignAsString = "Middle";
            editorButton1.Appearance = appearance1;
            editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button;
            editorButton1.Text = "...";
            this.ultraTextEditorFormula.ButtonsRight.Add(editorButton1);
            this.ultraTextEditorFormula.Location = new System.Drawing.Point(112, 64);
            this.ultraTextEditorFormula.Name = "ultraTextEditorFormula";
            this.ultraTextEditorFormula.Size = new System.Drawing.Size(144, 21);
            this.ultraTextEditorFormula.TabIndex = 6;
            this.ultraTextEditorFormula.EditorButtonClick += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(this.ultraTextEditorFormula_EditorButtonClick);
            // 
            // ultraLabelFormula
            // 
            this.ultraLabelFormula.Location = new System.Drawing.Point(8, 64);
            this.ultraLabelFormula.Name = "ultraLabelFormula";
            this.ultraLabelFormula.Size = new System.Drawing.Size(100, 23);
            this.ultraLabelFormula.TabIndex = 7;
            this.ultraLabelFormula.Text = "Formula (optional)";
            // 
            // chkAddAll
            // 
            this.chkAddAll.Checked = true;
            this.chkAddAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAddAll.Location = new System.Drawing.Point(112, 91);
            this.chkAddAll.Name = "chkAddAll";
            this.chkAddAll.Size = new System.Drawing.Size(142, 20);
            this.chkAddAll.TabIndex = 8;
            this.chkAddAll.Text = "Add To All Grids";
            this.chkAddAll.Visible = false;
            this.chkAddAll.CheckedChanged += new System.EventHandler(this.chkAddAll_CheckedChanged);
            // 
            // NewColumnDialog
            // 
            this.AcceptButton = this.ultraButtonOk;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.ultraButtonCancel;
            this.ClientSize = new System.Drawing.Size(266, 154);
            this.Controls.Add(this.chkAddAll);
            this.Controls.Add(this.ultraLabelFormula);
            this.Controls.Add(this.ultraTextEditorFormula);
            this.Controls.Add(this.ultraButtonCancel);
            this.Controls.Add(this.ultraButtonOk);
            this.Controls.Add(this.ultraComboEditorType);
            this.Controls.Add(this.ultraLabel2);
            this.Controls.Add(this.ultraLabel1);
            this.Controls.Add(this.ultraTextEditorName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "NewColumnDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Column";
            this.Load += new System.EventHandler(this.NewColumnDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ultraTextEditorName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraComboEditorType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTextEditorFormula)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        #region NewColumnDialog_Load

        private void NewColumnDialog_Load(object sender, System.EventArgs e)
        {
            this.LoadTypesCombo();
        }

        #endregion // NewColumnDialog_Load

        #region LoadTypesCombo

        private void LoadTypesCombo()
        {
            object[] arr = new object[]
                {
                    typeof( string ), "Text",
                    typeof( bool ), "Checkbox",
                    //typeof( decimal ), "Currency",
					//typeof( int ), "Whole Number",
					typeof( double ), "Decimal",
                    typeof( DateTime ), "Date",
                };

            this.ultraComboEditorType.Items.Clear();

            for (int i = 0; i < arr.Length; i += 2)
            {
                Type type = (Type)arr[i];
                string description = arr[i + 1].ToString();
                this.ultraComboEditorType.Items.Add(type, description);
            }

            ValueList vl = this.ultraComboEditorType.Items[0].ValueList;
            vl.DisplayStyle = ValueListDisplayStyle.DisplayText;
            vl.SortStyle = ValueListSortStyle.Ascending;

            // Select a default type.
            if (this.ultraComboEditorType.Items.Count > 0)
                this.ultraComboEditorType.SelectedIndex = 0;
        }

        #endregion // LoadTypesCombo

        #region Band

        private UltraGridBand band = null;

        /// <summary>
        /// Gets or sets the band object to which the new column will be added.
        /// </summary>
        public UltraGridBand Band
        {
            get
            {
                return this.band;
            }
            set
            {
                this.band = value;
            }
        }

        #endregion // Band

        #region CreatedColumn

        private UltraGridColumn createdColumn = null;

        /// <summary>
        /// Returns the new column that was created. It will return null if the user cancels the process.
        /// </summary>
        public UltraGridColumn CreatedColumn
        {
            get
            {
                return this.createdColumn;
            }
        }

        #endregion // CreatedColumn

        #region ultraButtonOk_Click

        private void ultraButtonOk_Click(object sender, System.EventArgs e)
        {
            bool success = this.CreateColumnHelper();

            if (success)
                this.DialogResult = DialogResult.OK;
        }

        #endregion // ultraButtonOk_Click

        #region CreateColumnHelper

        /// <summary>
        /// Creates the column based on currently input data. If a column was previously created
        /// then reuses that.
        /// </summary>
        private bool CreateColumnHelper()
        {
            string columnName = this.ultraTextEditorName.Text;
            if (null != columnName)
                columnName = columnName.Trim();

            // Don't allow empty string as the column name.
            if (null == columnName || columnName.Length <= 0)
            {
                MessageBox.Show(this, "Please enter a column name.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            // Make sure that a column by the same name doesn't exist already.
            if (this.band.Columns.Exists(columnName)
                && (null == this.createdColumn || this.createdColumn.Key != columnName))
            {
                MessageBox.Show(this, "A column by this name already exists. Please enter a different name.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            ValueListItem item = this.ultraComboEditorType.SelectedItem;
            if (null == item)
            {
                MessageBox.Show(this, "Please select the type of column you want to add.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            // If we haven't already created the column then create one.
            if (null == this.createdColumn)
                this.createdColumn = this.band.Columns.Add(columnName);
            else
                // If we had already created the column then make sure that 
                // the key is the same.
                this.createdColumn.Key = columnName;

            // Assing the newly selected data tyle and formula if any.
            this.createdColumn.DataType = (Type)item.DataValue;
            if (this.createdColumn.DataType == typeof(double))
            {
                this.createdColumn.Format = ApplicationConstants.FORMAT_COSTBASIS;
            }
            this.createdColumn.Formula = this.ultraTextEditorFormula.Text;

            return true;
        }

        #endregion // CreateColumnHelper

        #region ultraTextEditorFormula_EditorButtonClick

        private void ultraTextEditorFormula_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            this.CreateColumnHelper();

            if (null == this.createdColumn)
                return;

            // In order to design a formula, CalcManager property of the grid must
            // be assigned an instance of a calc manager. If none is assigned then
            // create a new one and assign it.
            if (null == this.createdColumn.Layout.Grid.CalcManager)
                this.createdColumn.Layout.Grid.CalcManager = new UltraCalcManager();

            FormulaBuilderDialog dlg = new FormulaBuilderDialog(this.createdColumn, true);

            dlg.OperandInitializing += new OperandInitializingEventHandler(dlg_OperandInitializing);
            DialogResult result = dlg.ShowDialog(this);

            if (DialogResult.OK == result)
                this.ultraTextEditorFormula.Text = dlg.Formula;

            if (this.ultraTextEditorFormula.Text != string.Empty)
                this.createdColumn.Formula = this.ultraTextEditorFormula.Text;
            dlg.OperandInitializing -= new OperandInitializingEventHandler(dlg_OperandInitializing);
        }

        void dlg_OperandInitializing(object sender, OperandInitializingEventArgs e)
        {
            // See if the FormulaProvider is a SummarySettings
            if (e.OperandName.Equals(this.ParentGridName, StringComparison.OrdinalIgnoreCase))
            {

            }
            else
            {



                switch (e.OperandName)
                {
                    case "Controls":
                        break;

                    case "ExposurePnlCacheItemList":
                        break;

                    default:
                        if (e.OperandContext is SummarySettings)
                        {
                            e.Cancel = true;
                        }
                        if (e.OperandContext is Infragistics.Win.UltraWinGrid.UltraGrid)
                        {
                            e.Cancel = true;
                        }
                        //else
                        //{
                        //    bool d = true;
                        //}
                        break;
                }
            }


            //if (e.OperandName != this.ParentGridName && e.OperandName != s && e.OperandName != "Controls")
            //{
            //    if (e.OperandContext is SummarySettings)
            //    {
            //        e.Cancel = true;
            //    }
            //    //e.Cancel = true;
            //}

            //Infragistics.Win.CalcEngine.IFormulaProvider summarySettings = e.OperandContext as Infragistics.Win.CalcEngine.IFormulaProvider;





            //if (summarySettings is SummarySettings )
            //{
            //    e.Cancel = true;
            //}
            //else if (e.OperandContext.ToString() =="Named References")
            //{
            //    e.Cancel = true;
            //}

            //if (!(e.OperandContext.ToString().Equals("ExposurePnlCacheItemList")))
            //{
            //    if (!(e.OperandContext.ToString().Equals("Controls")))
            //    {
            //        e.Cancel = true;

            //    }
            //}
        }

        #endregion // ultraTextEditorFormula_EditorButtonClick

        #region ultraButtonCancel_Click

        private void ultraButtonCancel_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

            // If the dialog gets canceled then remove the column if we added one.
            // 
            if (null != this.createdColumn)
                this.band.Columns.Remove(this.createdColumn);

            this.createdColumn = null;
        }

        #endregion // ultraButtonCancel_Click


        private bool _addToAllGrids = false;

        public bool AddToAllGrids
        {
            get { return _addToAllGrids; }
        }

        private void chkAddAll_CheckedChanged(object sender, EventArgs e)
        {
            _addToAllGrids = chkAddAll.Checked;
        }

        private string _gridName;

        public string ParentGridName
        {
            get { return _gridName; }
            set { _gridName = value; }
        }

    }
}
