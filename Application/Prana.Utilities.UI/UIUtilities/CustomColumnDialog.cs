using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.Global;
using System;
using System.Windows.Forms;

namespace Prana.Utilities.UI.UIUtilities
{
    public partial class CustomColumnDialog : Form
    {
        public CustomColumnDialog()
        {
            InitializeComponent();
        }


        #region NewColumnDialog_Load

        private void CustomColumnDialog_Load(object sender, System.EventArgs e)
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
    }
}