using System;
using System.Windows.Forms;
using Prana.PM.BLL;
//using Prana.PM.Common;
using Prana.PM.DAL;
using System.Text;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UIUtilities;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects.PositionManagement;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;


namespace Prana.PM.Admin.UI.Controls
{
    public partial class CtrlSelectColumns : UserControl
    {
        const int TRANSACTION_TYPE = 1;
        const int NET_POSITION_TYPE = 2;
        #region Grid Column Names

        const string COL_SourceColumnName = "SourceColumnName";
        const string COL_Description = "Description";
        const string COL_Type = "Type";
        const string COL_SampleValue = "SampleValue";
        const string COL_Notes = "Notes";
        const string COL_ColumnSequenceNo = "ColumnSequenceNo";
        const string COL_ID = "ID";
        const string COL_IsRequiredInUpload = "IsRequiredInUpload";

        #endregion Grid Column Names

        #region Combo Table Type Column Names

        const string COL_TableTypeName = "TableTypeName";
        const string COL_TableTypeID = "TableTypeID";

        #endregion Combo Table Type Column Names

        BindingSource _formBindingSource = new BindingSource();
        private SelectColumns _selectColumns = new SelectColumns();

        public CtrlSelectColumns()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            // Close the Container Form
            FindForm().Close();
        }

        #region Initialize the control
        private bool _isInitialized = false;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is initialized.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is initialized; otherwise, <c>false</c>.
        /// </value>
        public bool IsInitialized
        {
            get { return _isInitialized; }
            set { _isInitialized = value; }
        }


        /// <summary>
        /// Initialize the control.
        /// </summary>
        public void InitControl(Prana.BusinessObjects.PositionManagement.DataSourceNameID dataSourceNameID)
        {
            try
            {
                if (!_isInitialized)
                {
                    //_formBindingSource.DataMemberChanged += new EventHandler(_formBindingSource_DataMemberChanged);
                    //_formBindingSource.CurrentChanged += new EventHandler(_formBindingSource_CurrentChanged);
                    //_formBindingSource.DataError += new BindingManagerDataErrorEventHandler(_formBindingSource_DataError);
                    SetupBinding(dataSourceNameID);
                    _isInitialized = true;
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        //void _formBindingSource_DataError(object sender, BindingManagerDataErrorEventArgs e)
        //{
        //    btnSave.Enabled = _selectColumns.IsValid;
        //    //throw new Exception("The method or operation is not implemented.");
        //}

        //void _formBindingSource_CurrentChanged(object sender, EventArgs e)
        //{
        //    btnSave.Enabled = _selectColumns.IsValid;
        //    //throw new Exception("The method or operation is not implemented.");
        //}

        //void _formBindingSource_DataMemberChanged(object sender, EventArgs e)
        //{
        //    btnSave.Enabled = _selectColumns.IsValid;
        //    //throw new Exception("The method or operation is not implemented.");
        //}



        #endregion

        Prana.BusinessObjects.PositionManagement.DataSourceNameID _dataSourceNameID = new DataSourceNameID();
        private void SetupBinding(Prana.BusinessObjects.PositionManagement.DataSourceNameID dataSourceNameID)
        {

            try
            {
                
                errDataSourceColumns.DataSource = _formBindingSource;
                _dataSourceNameID = dataSourceNameID;

                //AddDataBinding();
                TableTypeList tableTypeList = DataSourceManager.GetTableTypes(true);
                this.bindingSourceTableType.DataSource = tableTypeList;
                
                _selectColumns.DataSourceNameIDValue = dataSourceNameID;
                _selectColumns.SelectColumnItems = DataSourceManager.GetSelectColumns(dataSourceNameID); //SelectColumnsItemList.Retrieve(dataSourceNameID);

                _formBindingSource.DataSource = _selectColumns;
                SelectColumnsErrProvider.DataSource = _formBindingSource;
                BindGridComboBoxes();

                //create a binding object
                Binding sourceNameBinding = new Binding("Text", _formBindingSource, "DataSourceNameIDValue");
                //add new binding
                lblDataSourceName.DataBindings.Add(sourceNameBinding);

                //cmbSourceColumnType.DataBindings.Clear();
                //cmbSourceColumnType.DataBindings.Add(new System.Windows.Forms.Binding("Value", this._formBindingSource, "SelectCoumnsItems.TableTypeID", true));

                //_selectColumns.BeginEdit();
                grdColumns.DataMember = "SelectColumnItems";
                grdColumns.DataSource = _formBindingSource;

                //grdColumns.DataMember = "SelectColumnItems";
                //dataGridView1.DataSource = _formBindingSource;
                //_selectColumns.BeginEdit();

                //_formBindingSource.te
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void AddDataBinding()
        {
            //cmbSourceColumnType.DataBindings.Clear();
            //cmbSourceColumnType.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.bindingSourceTableType, "TableTypeID", true));
        }

        private void BindGridComboBoxes()
        {
            try
            {
                cmbSourceColumnType.DisplayMember = "DisplayText";
                cmbSourceColumnType.ValueMember = "Value";
                cmbSourceColumnType.DataSource = null;
                cmbSourceColumnType.DataSource = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(SelectColumnsType));
                Utils.UltraDropDownFilter(cmbSourceColumnType, "DisplayText");
                
                cmbTableType.Value = -1;
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdColumns_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                UltraGridBand band = e.Layout.Bands[0];
                e.Layout.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;

                grdColumns.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
                grdColumns.DisplayLayout.ScrollBounds = ScrollBounds.ScrollToLastItem;
                grdColumns.DisplayLayout.Override.AllowAddNew = AllowAddNew.Yes;
                grdColumns.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
                grdColumns.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
                grdColumns.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
                grdColumns.DisplayLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
                grdColumns.DisplayLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;

                ///TODO : Remove this hardcoding of column names, We might have some attribute to be put on the name 
                ///of the property.

                UltraGridColumn colID = band.Columns[COL_ID];
                colID.Hidden = true;


                UltraGridColumn colSourceColumnName = band.Columns[COL_SourceColumnName];
                colSourceColumnName.Header.Caption = "Source Column Name";
                colSourceColumnName.Header.VisiblePosition = 1;
                colSourceColumnName.SortIndicator = SortIndicator.Disabled;
                grdColumns.DisplayLayout.Bands[0].Columns[COL_SourceColumnName].Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;

                UltraGridColumn colDescription = band.Columns[COL_Description];
                colDescription.Header.Caption = "Description";
                colDescription.Header.VisiblePosition = 2;
                colDescription.SortIndicator = SortIndicator.Disabled;
                grdColumns.DisplayLayout.Bands[0].Columns[COL_Description].Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;

                UltraGridColumn colType = band.Columns[COL_Type];
                colType.Header.Caption = "Type";
                colType.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colType.ButtonDisplayStyle = ButtonDisplayStyle.Always;
                colType.ValueList = cmbSourceColumnType;
                colType.SortIndicator = SortIndicator.Disabled;
                colType.Header.VisiblePosition = 3;

                UltraGridColumn colSampleValue = band.Columns[COL_SampleValue];
                colSampleValue.Header.Caption = "Sample Value";
                colSampleValue.Header.VisiblePosition = 4;
                colSampleValue.SortIndicator = SortIndicator.Disabled;
                grdColumns.DisplayLayout.Bands[0].Columns[COL_SampleValue].Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;

                UltraGridColumn colNotes = band.Columns[COL_Notes];
                colNotes.Header.Caption = "Notes";
                colNotes.Header.VisiblePosition = 5;
                colNotes.SortIndicator = SortIndicator.Disabled;
                grdColumns.DisplayLayout.Bands[0].Columns[COL_Notes].Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                colNotes.Hidden = true;

                UltraGridColumn colColumnSequenceNo = band.Columns[COL_ColumnSequenceNo];
                colColumnSequenceNo.Header.VisiblePosition = 6;
                colColumnSequenceNo.Header.Caption = "Column Sequence No";
                colColumnSequenceNo.SortIndicator = SortIndicator.Disabled;
                colColumnSequenceNo.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                colColumnSequenceNo.Hidden = true;
                //colColumnSequenceNo.

                //grdColumns.DisplayLayout.Bands[0].Columns[COL_ColumnSequenceNo].Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                //grdColumns.DisplayLayout.Bands[0].Columns[COL_ColumnSequenceNo].NullText = "0";
                //colColumnSequenceNo.InvalidValueBehavior = InvalidValueBehavior.RevertValueAndRetainFocus;
                //colColumnSequenceNo.MaskInput = Mas
                //colColumnSequenceNo.RegexPattern = UltraGridColumn.
                
                UltraGridColumn colIsRequiredInUpload = band.Columns[COL_IsRequiredInUpload];
                colIsRequiredInUpload.Header.VisiblePosition = 7;
                colIsRequiredInUpload.SortIndicator = SortIndicator.Disabled;
                colIsRequiredInUpload.Header.Caption = "Required in Upload";
                
                UltraGridColumn colGrdTableTypeID = band.Columns[COL_TableTypeID];
                colGrdTableTypeID.Header.VisiblePosition = 8;
                colGrdTableTypeID.Hidden = true;

                //Defining Combo headers
                UltraGridBand cmbTTBand = cmbTableType.DisplayLayout.Bands[0];
                UltraGridColumn colTableTypeName = cmbTTBand.Columns[COL_TableTypeName];
                colTableTypeName.Header.VisiblePosition = 1;
                colTableTypeName.Header.Caption = "Table Type Name";
                colTableTypeName.SortIndicator = SortIndicator.Disabled;
                cmbTTBand.Columns[COL_TableTypeName].Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;

                UltraGridColumn colTableTypeID = cmbTTBand.Columns[COL_TableTypeID];
                colTableTypeName.Header.VisiblePosition = 2;
                colTableTypeID.Hidden = true;

                
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            bool isCollectionValid = true;

            try
            {
                int tableTypeID = int.Parse(cmbTableType.Value.ToString());
                if (tableTypeID >= 0)
                {
                    if (_selectColumns.SelectColumnItems.Count > 0)
                    {
                        bool containsUploadID = false;
                        foreach (SelectColumnsItem selectColumnsItem in _selectColumns.SelectColumnItems)
                        {
                            if (!selectColumnsItem.IsValid)
                            {
                                isCollectionValid = false;
                                break;
                            }

                            if (selectColumnsItem.SourceColumnName == "UploadID")
                            {
                                containsUploadID = true;
                            }
                            selectColumnsItem.TableTypeID = tableTypeID;
                        }
                        //if (containsUploadID == false)
                        //{
                        //    _selectColumns.SelectColumnItems.Add(new SelectColumnsItem("UploadID", 0, SelectColumnsType.Integer, true, tableTypeID));
                        //}

                        if (isCollectionValid)
                        {
                            if (containsUploadID == false)
                            {
                                _selectColumns.SelectColumnItems.Add(new SelectColumnsItem("UploadID", 0, SelectColumnsType.Integer, true, tableTypeID));
                            }

                            //TODO: Create script from the columns and make a table out of that.
                            StringBuilder command = new StringBuilder(8192);
                            string tableName = "PM_" + lblDataSourceName.Text + cmbTableType.Text.Replace(" ", "");
                            string isNullable = string.Empty;
                            string columnType = string.Empty;
                            string columnName = string.Empty;
                            int originalColumnCount;
                            bool exists = DataSourceManager.CheckTableExistsInDataBase(tableName, out originalColumnCount);
                            if (!exists)
                            {
                                DialogResult result = new DialogResult();
                                result = MessageBox.Show(this,"Please verify all the set up details are correct before saving. These will not be changed once done. Do you want to continue?",
                                    "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                                if (result == DialogResult.Yes)
                                {
                                    command.Append("CREATE TABLE " + tableName + "(");



                                    //command = createtableString  + "(";

                                    //sb.Append(command);
                                    foreach (SelectColumnsItem selectColumnsItem in _selectColumns.SelectColumnItems)
                                    {
                                        columnName = selectColumnsItem.SourceColumnName.Replace(" ", "").Replace("/", "_");
                                        if (selectColumnsItem.IsRequiredInUpload)
                                        {
                                            isNullable = "NOT NULL";
                                        }
                                        else
                                        {
                                            isNullable = "NULL";
                                        }
                                        switch (selectColumnsItem.Type)
                                        {
                                            case SelectColumnsType.Integer:
                                                columnType = "int";
                                                break;
                                            case SelectColumnsType.Decimal:
                                                columnType = "float";
                                                break;
                                            case SelectColumnsType.Text:
                                                columnType = "varchar(500)";
                                                break;
                                            case SelectColumnsType.Boolean:
                                                columnType = "bit";
                                                break;
                                            case SelectColumnsType.DateTime:
                                                columnType = "DateTime";
                                                break;
                                            default:
                                                break;
                                        }
                                        command.Append(" " + columnName + " " + columnType + " " + isNullable + ",");
                                    }
                                    int len = int.Parse(command.Length.ToString());
                                    command.Remove(command.ToString().LastIndexOf(","), 1);
                                    //sb.Remove(len - 2, 1);
                                    command.Append(")");

                                    _selectColumns.ApplyEdit();
                                    int isTableCreatedSuccessfully = DataSourceManager.CreateOrUpdateDataSourceTable(tableName, command.ToString(), _selectColumns.DataSourceNameIDValue.ID, tableTypeID);
                                    if (int.Equals(isTableCreatedSuccessfully, 0))
                                    {
                                        DataSourceManager.SaveSelectColumns(_selectColumns.SelectColumnItems,
                                                                            _selectColumns.DataSourceNameIDValue.ID);
                                        //_selectColumns.SelectColumnItems =
                                        //    DataSourceManager.GetSelectColumns(_selectColumns.DataSourceNameIDValue);
                                        //_formBindingSource.ResetBindings(false);
                                        _selectColumns.BeginEdit();
                                        MessageBox.Show(this, "Columns Succesfully Set for DataSource", "Position Management", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    else
                                    {
                                        MessageBox.Show(this, "The Table structure to store the Data could not be made.Please contact Prana Administrator", "Position Management", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                            }
                            else if (exists && !int.Equals(originalColumnCount, _selectColumns.SelectColumnItems.Count))
                            {

                                MessageBox.Show(this, "No changes can be made to the Set up once created.", "Position Management", MessageBoxButtons.OK, MessageBoxIcon.Information);


                                ////set start looping counter starting position of the new columns
                                //int counter = originalColumnCount;
                                //command.Append("ALTER TABLE " + tableName + " ADD ");
                                //for(;counter < _selectColumns.SelectColumnItems.Count;counter++)
                                //{
                                //    SelectColumnsItem selectColumnsItem = _selectColumns.SelectColumnItems[counter];
                                //    columnName = selectColumnsItem.SourceColumnName.Replace(" ", "").Replace("/", "_");
                                //    isNullable = "NULL";                                
                                //    switch (selectColumnsItem.Type)
                                //    {
                                //        case SelectColumnsType.Integer:
                                //            columnType = "int";
                                //            break;
                                //        case SelectColumnsType.Decimal:
                                //            columnType = "float";
                                //            break;
                                //        case SelectColumnsType.Text:
                                //            columnType = "varchar(500)";
                                //            break;
                                //        case SelectColumnsType.Boolean:
                                //            columnType = "bit";
                                //            break;
                                //        case SelectColumnsType.DateTime:
                                //            columnType = "DateTime";
                                //            break;
                                //        default:
                                //            break;
                                //    }
                                //    command.Append(" " + columnName + " " + columnType + " " + isNullable + ",");
                                //}
                                ////int len = int.Parse(command.Length.ToString());
                                //command.Remove(command.ToString().LastIndexOf(","), 1);
                                ////sb.Remove(len - 2, 1);
                                ////command.Append(")");

                                //_selectColumns.ApplyEdit();
                                //DataSourceManager.CreateOrUpdateDataSourceTable(tableName, command.ToString(), _selectColumns.DataSourceNameIDValue.ID, tableTypeID);

                                //DataSourceManager.SaveSelectColumns(_selectColumns.SelectColumnItems,
                                //                                    _selectColumns.DataSourceNameIDValue.ID);

                                ////DataSourceTable dataSourceTableDetails = new DataSourceTable();
                                ////dataSourceTableDetails.DataSourceID = _selectColumns.DataSourceNameIDValue.ID;
                                ////dataSourceTableDetails.TableTypeID = int.Parse(cmbTableType.Value.ToString());
                                ////dataSourceTableDetails.TableName = tableName;
                                ////DataSourceManager.SaveDataSourceTableData(dataSourceTableDetails);

                                //_selectColumns.SelectColumnItems =
                                //    DataSourceManager.GetSelectColumns(_selectColumns.DataSourceNameIDValue);
                                //_formBindingSource.ResetBindings(false);

                                //_selectColumns.BeginEdit();

                                ////here the update query on the table will be built
                            }
                            else
                            {
                                MessageBox.Show(this, "No changes to save.", "Position Management", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                _selectColumns.BeginEdit();
                            }
                        }
                        //else
                        //{
                        //    //MessageBox.Show(this, "No changes to save.", "Prana Alert", MessageBoxButtons.OK);
                        //}
                    }
                    else
                    {
                        MessageBox.Show(this, "Please enter some columns before saving !", "Position Management", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show(this, "Please select Table Type !", "Position Management", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }



        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (_selectColumns == null)
                {
                    MessageBox.Show("Error in creating new column", "Column creation error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                SelectColumnsItem item = new SelectColumnsItem();
                item.SourceColumnName = "";
                _selectColumns.SelectColumnItems.Add(item);
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                // stop the flow of events
                //this._formBindingSource.RaiseListChangedEvents = false;
                //this._formBindingSource.RaiseListChangedEvents = false;
                //_selectColumns

                //_selectColumns.SelectColumnItems.Clear();
                _selectColumns.CancelEdit();
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                _formBindingSource.ResetBindings(false);

                this._formBindingSource.RaiseListChangedEvents = true;
            }
        }

        int tableTypeValue = int.MinValue;
        private void cmbTableType_ValueChanged(object sender, EventArgs e)
        {
            tableTypeValue = int.Parse(cmbTableType.Value.ToString());
            if (tableTypeValue == TRANSACTION_TYPE || tableTypeValue == NET_POSITION_TYPE)
            {
                _selectColumns.SelectColumnItems = DataSourceManager.GetSelectColumns(_dataSourceNameID, tableTypeValue);

                if (_selectColumns.SelectColumnItems.Count > 0)
                {
                    _selectColumns.BeginEdit();
                }
                _formBindingSource.DataSource = _selectColumns;
                SelectColumnsErrProvider.DataSource = _formBindingSource;
            }            
            else
            {
                _selectColumns.SelectColumnItems = new SelectColumnsItemList();
                _formBindingSource.DataSource = _selectColumns;
                SelectColumnsErrProvider.DataSource = _formBindingSource;
            }
            //_selectColumns.BeginEdit();
        }

        private void grdColumns_Click(object sender, EventArgs e)
        {
            //SelectColumnsItem selectColumnsItem = new SelectColumnsItem();
            //selectColumnsItem = e.
            //_selectColumns.CancelEdit();
        }

        private void grdColumns_Enter(object sender, EventArgs e)
        {

        }

        private void grdColumns_Error(object sender, ErrorEventArgs e)
        {
            //MessageBox.Show("Error !!!");
            e.Cancel = true;
        }
    }
}
