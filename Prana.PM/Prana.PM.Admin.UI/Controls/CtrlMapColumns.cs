using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;
//using Prana.PM.Common;
using Prana.PM.BLL;
using Prana.PM.DAL;
using System.Text;
using Prana.BusinessObjects.PositionManagement;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Utilities.UIUtilities;
namespace Prana.PM.Admin.UI.Controls
{
    public partial class CtrlMapColumns : UserControl
    {
        #region Grid Column Names

        const string COL_SourceItemID = "SourceItemID";
        const string COL_SourceItemName = "SourceItemName";
        const string COL_SourceItemFullName = "SourceItemFullName";
        const string COL_ApplicationItemID = "ApplicationItemID";
        const string COL_ApplicationItemName = "ApplicationItemName";
        const string COL_ApplicationItemFullName = "ApplicationItemFullName";
        const string COL_Lock = "Lock";

        #endregion Grid Column Names

        #region Combo Table Type Column Names

        const string COL_TableTypeName = "TableTypeName";
        const string COL_TableTypeID = "TableTypeID";

        #endregion Combo Table Type Column Names

        BindingSource _formBindingSource = new BindingSource();
        private MapColumns _mapColumns = new MapColumns();
        private DataSourceNameID _dataSourceNameID = new DataSourceNameID();
        private int _dataSourceID = 0;
        private int _tableTypeID = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="CtrlMapColumns"/> class.
        /// </summary>
        public CtrlMapColumns()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Handles the Click event of the btnClose control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            // Close the Container Form

            this.FindForm().Close();
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
        /// <param name="dataSourceNameID">The data source name ID.</param>
        public void InitControl(DataSourceNameID dataSourceNameID)
        {
            this._dataSourceNameID = dataSourceNameID;
            try
            {
                if (!_isInitialized)
                {
                    TableTypeList tableTypeList = DataSourceManager.GetTableTypes(true);
                    this.bindingSourceTableType.DataSource = tableTypeList;

                    // pass -1 for the table type at the time of initialisation.
                    SetupBinding(_dataSourceNameID, -1);
                    _isInitialized = true;
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion

        /// <summary>
        /// Setups the binding.
        /// TableTypeId, added so that we pass the table type for which we set up the data.
        /// </summary>
        /// <param name="dataSourceID">The data source ID.</param>
        /// <param name="tableTypeID">The table type ID.</param>
        private void SetupBinding(DataSourceNameID dataSourceID, int tableTypeID)
        {
            try
            {
                errorProvider1.DataSource = _mapColumns;
                this._dataSourceID = dataSourceID.ID;
                
                _mapColumns.DataSourceNameID = dataSourceID;
                _mapColumns.MappingItems = DataSourceManager.GetMapColumns(_dataSourceID, tableTypeID);
                _formBindingSource.DataSource = _mapColumns;

                BindGridComboBoxes();
                cmbTableType.Value = tableTypeID;
                //lblDataSourceName.DataBindings.Add(_formBindingSource,

                //create a binding object
                Binding sourceNameBinding = new System.Windows.Forms.Binding("Text", _formBindingSource, "DataSourceNameID");
                //add new binding
                lblDataSourceName.DataBindings.Clear();
                lblDataSourceName.DataBindings.Add(sourceNameBinding);

                grdMapColumns.DataMember = "MappingItems";
                grdMapColumns.DataSource = _formBindingSource;
                

                //Defining Combo headers
                UltraGridBand cmbTTBand = cmbTableType.DisplayLayout.Bands[0];
                UltraGridColumn colTableTypeName = cmbTTBand.Columns[COL_TableTypeName];
                colTableTypeName.Header.VisiblePosition = 1;
                colTableTypeName.Header.Caption = "Table Type Name";
                cmbTTBand.Columns[COL_TableTypeName].Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;

                UltraGridColumn colTableTypeID = cmbTTBand.Columns[COL_TableTypeID];
                colTableTypeName.Header.VisiblePosition = 2;
                colTableTypeID.Hidden = true;


            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

        }

        
        //private void GetAllApplicationItemsList()
        //{
        //    //List<EnumerationValue> applicationColumnsList = DataSourceManager.GetAllApplicationColumns();
        //    //_applicationColumnsList = (ValueList)applicationColumnsList;
        //    //applicationColumnsList.

        //    //foreach (List<EnumerationValue> mappingItem in applicationColumnsList)
        //    //{
        //    //    ;
        //    //}
        //    //grdMapColumns.ActiveRow.Cells["ApplicationItemNAme"].ValueList = (ValueList)applicationColumnsList;
        //    List<EnumerationValue> applicationColumnsList = DataSourceManager.GetAllApplicationColumns();
        //    //foreach(EnumerationValue enumerationValue in applicationColumnsList)
        //    //{
        //    //    ;
        //    //}
        //}

        int _idForFundName = 0;
        int _idForSymbol = 0;
        int _idForCostBasis = 0;
        int _idForQuantity = 0;

        ValueList _unlockedApplicationColumnList = new ValueList();
        ValueList _lockedApplicationColumnList = new ValueList();

        /// <summary>
        /// Binds the grid combo boxes.
        /// </summary>
        private void BindGridComboBoxes()
        {
            try
            {
            //    List<EnumerationValue> dataSourceColumnsList = DataSourceManager.GetAllDataSourceColumnsByID(_mapColumns.DataSourceNameID.ID, _tableTypeID);
            //    dataSourceColumnsList.Add(new EnumerationValue(Constants.C_COMBO_SELECT, -1));
            //    cmbSourceColumn.DisplayMember = "DisplayText";
            //    cmbSourceColumn.ValueMember = "Value";
            //    cmbSourceColumn.DataSource = dataSourceColumnsList;
            //    Utils.UltraDropDownFilter(cmbSourceColumn, "DisplayText");

                   
                _unlockedApplicationColumnList = DataSourceManager.GetAllApplicationColumns();
                cmbApplicationColumn.Width = 134;

                cmbApplicationColumn.DisplayMember = "DisplayText";
                cmbApplicationColumn.ValueMember = "DataValue";
                cmbApplicationColumn.DataSource = null;
                cmbApplicationColumn.DataSource = _unlockedApplicationColumnList.ValueListItems;
                Utils.UltraDropDownFilter(cmbApplicationColumn, "DisplayText");

                // Do not go again for the for loop if this value is already set earlier. 
                if (int.Equals(this._idForFundName, 0))
                {
                    for (int counter = 0; counter < _unlockedApplicationColumnList.ValueListItems.Count; counter++)
                    {
                        if (string.Equals(_unlockedApplicationColumnList.ValueListItems[counter].DisplayText, "FundName"))
                        {
                            _idForFundName = Convert.ToInt16(_unlockedApplicationColumnList.ValueListItems[counter].DataValue);
                            continue;
                        }
                        if (string.Equals(_unlockedApplicationColumnList.ValueListItems[counter].DisplayText, "Symbol"))
                        {
                            _idForSymbol = Convert.ToInt16(_unlockedApplicationColumnList.ValueListItems[counter].DataValue);
                            continue;
                        }
                        if (string.Equals(_unlockedApplicationColumnList.ValueListItems[counter].DisplayText, "CostBasis"))
                        {
                            _idForCostBasis = Convert.ToInt16(_unlockedApplicationColumnList.ValueListItems[counter].DataValue);
                            continue;
                        }
                        if (string.Equals(_unlockedApplicationColumnList.ValueListItems[counter].DisplayText, "NetPosition"))
                        {
                            _idForQuantity = Convert.ToInt16(_unlockedApplicationColumnList.ValueListItems[counter].DataValue);
                            continue;
                        }



                    }
                }

                //if (int.Equals(this._idForSymbol, 0))
                //{
                //    for (int counter = 0; counter < _unlockedApplicationColumnList.ValueListItems.Count; counter++)
                //    {
                        
                //    }
                //}

                //if (int.Equals(this._idForCostBasis, 0))
                //{
                //    for (int counter = 0; counter < _unlockedApplicationColumnList.ValueListItems.Count; counter++)
                //    {
                       
                //    }
                //}
                
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        UltraGridColumn colLock;
        /// <summary>
        /// Handles the InitializeLayout event of the grdMapColumns control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs"/> instance containing the event data.</param>
        private void grdMapColumns_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                UltraGridBand band = e.Layout.Bands[0];
                e.Layout.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;

                grdMapColumns.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
                grdMapColumns.DisplayLayout.ScrollBounds = ScrollBounds.ScrollToLastItem;
                grdMapColumns.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
                grdMapColumns.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
                grdMapColumns.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
                grdMapColumns.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
                grdMapColumns.DisplayLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
                grdMapColumns.DisplayLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
                grdMapColumns.DisplayLayout.Bands[0].Override.AllowColSwapping = AllowColSwapping.NotAllowed;

                ///TODO : Remove this hardcoding of column names, We might have some attribute to be put on the name 
                ///of the property.

                UltraGridColumn colSourceItemID = band.Columns[COL_SourceItemID];
                colSourceItemID.Hidden = true;

                UltraGridColumn colSourceItemFullName = band.Columns[COL_SourceItemFullName];
                colSourceItemFullName.Hidden = true;

                UltraGridColumn colApplicationItemFullName = band.Columns[COL_ApplicationItemFullName];
                colApplicationItemFullName.Hidden = true;

                UltraGridColumn colApplicationItemName = band.Columns[COL_ApplicationItemName];
                colApplicationItemName.Hidden = true;

                UltraGridColumn colSourceItemName = band.Columns[COL_SourceItemName];
                colSourceItemName.Header.Caption = "Source Columns";
                colSourceItemName.CellActivation = Activation.NoEdit;
                //colSourceItemName.Style =  Infragistics.Win.UltraWinGrid.ColumnStyle
                //colSourceItemName.ButtonDisplayStyle = ButtonDisplayStyle.Always;
                //colSourceItemName.ValueList = cmbSourceColumn;
                colSourceItemName.Header.VisiblePosition = 1;
                colSourceItemName.SortIndicator = SortIndicator.Disabled;
                                

                UltraGridColumn colApplicationItemID = band.Columns[COL_ApplicationItemID];
                colApplicationItemID.Width = 70;
                colApplicationItemID.Header.Caption = "Application Columns";
                colApplicationItemID.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colApplicationItemID.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                colApplicationItemID.ValueList = cmbApplicationColumn;
                colApplicationItemID.Header.VisiblePosition = 2;
                colApplicationItemID.SortIndicator = SortIndicator.Disabled;

                colLock = band.Columns[COL_Lock];
                colLock.Header.Caption = "Locked";
                colLock.Header.VisiblePosition = 3;
                colLock.SortIndicator = SortIndicator.Disabled;
                if (isLockingEnabled.Checked)
                {
                    colLock.Hidden = false;
                }
                else
                {
                    colLock.Hidden = true;
                }                
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

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
                string fundColumnName = string.Empty;
                string symbolName = string.Empty;
                string costBasisValue = string.Empty;
                string quantity = string.Empty;
                if (_mapColumns.MappingItems.Count > 0)
                {
                    foreach (MappingItem mappingItem in _mapColumns.MappingItems)
                    {
                        if (!mappingItem.IsValid)
                        {
                            isCollectionValid = false;
                            break;
                        }
                    }

                    if (isCollectionValid)
                    {
                        bool isRepeated = false;
                        
                        foreach (MappingItem mappingItem in _mapColumns.MappingItems)
                        {

                            if (int.Equals(mappingItem.ApplicationItemId, _idForFundName))
                            {
                                fundColumnName = mappingItem.SourceItemName;                                
                            }
                            else if (int.Equals(mappingItem.ApplicationItemId, _idForSymbol))
                            {
                                symbolName = mappingItem.SourceItemName;
                            }
                            else if (int.Equals(mappingItem.ApplicationItemId, _idForCostBasis))
                            {
                                costBasisValue = mappingItem.SourceItemName;
                            }
                            else if (int.Equals(mappingItem.ApplicationItemId, _idForQuantity))
                            {
                                quantity = mappingItem.SourceItemName;
                            }

                            // we will check one application column to be mapped with single datasource column, only
                            // if the locking is enabled.
                            if (isLockingEnabled.Checked)
                            {
                                foreach (UltraGridRow row in grdMapColumns.Rows)
                                {

                                    if (mappingItem.ApplicationItemId == int.Parse(row.Cells["ApplicationItemID"].Value.ToString()) && mappingItem.SourceItemID != int.Parse(row.Cells["SourceItemID"].Value.ToString()) && mappingItem.ApplicationItemId != -1)
                                    {

                                        MessageBox.Show(this, "Application column is repeated i.e. assigned more than one time to source column. Please remove the repeatition!", "Position Management", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        isRepeated = true;
                                        return;
                                    }
                                }    
                            }                            
                            //if (isRepeated == true)
                            //    break;
                        }
                        if(string.Equals(fundColumnName, string.Empty))
                        {
                            MessageBox.Show(this, "DataSource Column list must have a column for FundName and it should be mapped to the application fund column.", "Position Management", MessageBoxButtons.OK, MessageBoxIcon.Error);                                    
                            return;
                        }
                        if (string.Equals(symbolName, string.Empty))
                        {
                            MessageBox.Show(this, "DataSource Column list must have a column for Symbol and it should be mapped to the application symbol column.", "Position Management", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        if (string.Equals(costBasisValue, string.Empty))
                        {
                            MessageBox.Show(this, "DataSource Column list must have a column for AveragePrice and it should be mapped to the application CostBasis column.", "Position Management", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        if (string.Equals(quantity, string.Empty))
                        {
                            MessageBox.Show(this, "DataSource Column list must have a column for Quantity and it should be mapped to the application NetPosition column.", "Position Management", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                            
                        }
                        if (isRepeated == false)
                        {
                            if (_tableTypeID >= 0)
                            {
                                int isTriggerCreatedSuccessfully = 0;
                                if (int.Equals(_tableTypeID, 2))
                                {

                                    StringBuilder createTriggerCommand = new StringBuilder(2048);
                                    string tableName = "PM_" + this._dataSourceNameID.ShortName + "NetPosition";
                                    string triggerName = "TRG_" + tableName;
                                    bool exists = DataSourceManager.CheckTriggerExistsInDataBase(triggerName);
                                    if (!exists)
                                    {
                                        string triggerCommand  = GetTriggerScript(fundColumnName, createTriggerCommand, tableName, triggerName);
                                        isTriggerCreatedSuccessfully = DataSourceManager.CreateTriggerForTable(triggerCommand);
                                        if (int.Equals(isTriggerCreatedSuccessfully, 0))
                                        {
                                            _mapColumns.ApplyEdit();
                                            DataSourceManager.SaveMapColumns(_mapColumns, _dataSourceID, _tableTypeID);
                                            _mapColumns.BeginEdit();

                                            //fetch newly saved data from db
                                            // _mapColumns.MappingItems = DataSourceManager.GetMapColumns(_dataSourceID, _tableTypeID);
                                            //reset bindings
                                            _formBindingSource.ResetBindings(false);
                                            MessageBox.Show(this, "Mapping of columns is saved !", "Position Management", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                        else
                                        {
                                            MessageBox.Show(this, "The Trigger Structure to store the Data could not be made.Please contact Prana Administrator", "Position Management", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                    else
                                    {
                                        _mapColumns.ApplyEdit();
                                        DataSourceManager.SaveMapColumns(_mapColumns, _dataSourceID, _tableTypeID);
                                        _mapColumns.BeginEdit();


                                        //fetch newly saved data from db
                                        // _mapColumns.MappingItems = DataSourceManager.GetMapColumns(_dataSourceID, _tableTypeID);

                                        //reset bindings
                                        _formBindingSource.ResetBindings(false);
                                        MessageBox.Show(this, "Mapping of columns is saved !", "Position Management", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }                                    
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Unable to save");
                    }
                }
                else
                {
                    MessageBox.Show(this, "Please map some columns before saving !", "Position Management", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        /// <summary>
        /// Gets the trigger script.
        /// </summary>
        /// <param name="fundColumnName">Name of the fund column.</param>
        /// <param name="createTriggerCommand">The create trigger command.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="triggerName">Name of the trigger.</param>
        /// <returns></returns>
        private string GetTriggerScript(string fundColumnName, StringBuilder createTriggerCommand, string tableName, string triggerName)
        {
            if (isLockingEnabled.Checked)
            {
                createTriggerCommand.Append("CREATE TRIGGER " + triggerName + " ON " + tableName + " AFTER INSERT ");
                createTriggerCommand.Append("AS ");
                createTriggerCommand.Append("DECLARE @rc AS INT;");
                createTriggerCommand.Append("SET @rc = @@rowcount; ");

                createTriggerCommand.Append("--Return if no row is inserted " + Environment.NewLine);
                createTriggerCommand.Append("IF @rc = 0 RETURN;");
                createTriggerCommand.Append("declare @fundid int; " + Environment.NewLine);
                createTriggerCommand.Append("DECLARE IsNullCursor CURSOR FAST_FORWARD FOR SELECT  " + Environment.NewLine);
                createTriggerCommand.Append("distinct (PDSCF.CompanyFundID) " + Environment.NewLine);
			    createTriggerCommand.Append("FROM  " + Environment.NewLine);
				createTriggerCommand.Append("inserted I " + Environment.NewLine);
				createTriggerCommand.Append("LEFT OUTER JOIN dbo.PM_DataSourceCompanyFund AS PDSCF ON I." + fundColumnName+ "= PDSCF.DataSourceFundName;  " + Environment.NewLine);

                createTriggerCommand.Append("OPEN IsNullCursor ;  " + Environment.NewLine);
                createTriggerCommand.Append("FETCH NEXT FROM IsNullCursor INTO @fundid;  " + Environment.NewLine);
                createTriggerCommand.Append("WHILE @@fetch_status = 0  " + Environment.NewLine);
                createTriggerCommand.Append("BEGIN  " + Environment.NewLine);

                createTriggerCommand.Append("	IF @FundID is null  " + Environment.NewLine);
		        createTriggerCommand.Append("RAISERROR('File contains unmapped fund name',16, 1) " + Environment.NewLine);

                createTriggerCommand.Append("FETCH NEXT FROM IsNullCursor INTO @fundid;  " + Environment.NewLine);
                createTriggerCommand.Append("END CLOSE IsNullCursor;  " + Environment.NewLine);
                createTriggerCommand.Append("DEALLOCATE IsNullCursor; 	 " + Environment.NewLine);


                createTriggerCommand.Append("DECLARE @DataSourceID int;");
                createTriggerCommand.Append("SET @DataSourceID = (SELECT dataSourceID from PM_DatasourceTables where TableName = '" + tableName + "') ; ");

                createTriggerCommand.Append("CREATE TABLE #TempColumnInfo ( ApplicationColumnName varchar(100),DataSourceColumnName varchar(100)) ");
                createTriggerCommand.Append("INSERT INTO #TempColumnInfo ( ApplicationColumnName,DataSourceColumnName ) ");
                createTriggerCommand.Append("SELECT 	PMA.Name,PMD.ColumnName FROM dbo.PM_ApplicationColumns PMA ");
                createTriggerCommand.Append("INNER JOIN dbo.PM_DataSourceColumns AS PMD ON PMA.ApplicationColumnID = PMD.ApplicationColumnID ");
                createTriggerCommand.Append("WHERE PMD.DataSourceID = @DataSourceID ");
                createTriggerCommand.Append("SELECT * INTO #TempInserted FROM inserted; ");
                createTriggerCommand.Append("DECLARE @cmd AS NVARCHAR(4000), @ApplicationColumn AS Varchar(100), @DataSourceColumn AS Varchar(100); ");
                createTriggerCommand.Append("SET @cmd = N'INSERT INTO dbo.PM_NetPositions ( PositionID, ' ");
                createTriggerCommand.Append("DECLARE ColumnsMapped CURSOR FAST_FORWARD FOR ");
                createTriggerCommand.Append("SELECT  ApplicationColumnName, DataSourceColumnName FROM #TempColumnInfo; ");
                createTriggerCommand.Append("OPEN ColumnsMapped ; ");
                createTriggerCommand.Append("FETCH NEXT FROM ColumnsMapped INTO @ApplicationColumn, @DataSourceColumn; ");
                createTriggerCommand.Append("WHILE @@fetch_status = 0 ");
                createTriggerCommand.Append("BEGIN ");
                createTriggerCommand.Append("SET @cmd = @cmd + @ApplicationColumn + ','  ");
                createTriggerCommand.Append("FETCH NEXT FROM ColumnsMapped INTO @ApplicationColumn, @DataSourceColumn; ");
                createTriggerCommand.Append("END ");
                createTriggerCommand.Append("CLOSE ColumnsMapped; ");
                createTriggerCommand.Append("SET @cmd = SubSTRING(@cmd, 0, LEN(@cmd)) + ') SELECT newid(), ' ");
                createTriggerCommand.Append("OPEN ColumnsMapped; ");
                createTriggerCommand.Append("FETCH NEXT FROM ColumnsMapped INTO @ApplicationColumn, @DataSourceColumn; ");
                createTriggerCommand.Append("WHILE @@fetch_status = 0 ");
                createTriggerCommand.Append("BEGIN ");
                createTriggerCommand.Append("SET @cmd = @cmd + @DataSourceColumn + ','    ");
                createTriggerCommand.Append("FETCH NEXT FROM ColumnsMapped INTO @ApplicationColumn, @DataSourceColumn; ");
                createTriggerCommand.Append("END ");
                createTriggerCommand.Append("CLOSE ColumnsMapped; ");
                createTriggerCommand.Append("DEALLOCATE ColumnsMapped; ");
                createTriggerCommand.Append("SET @cmd = SUBSTRING(@cmd, 0, LEN(@cmd)) + ' FROM #TempInserted '; ");
                createTriggerCommand.Append("exec(@cmd) ");
                createTriggerCommand.Append("drop table #TempColumnInfo ");
                createTriggerCommand.Append("drop table #TempInserted ");
            }
            else
            {

            }
            
            return createTriggerCommand.ToString();
        }

        /// <summary>
        /// Handles the Click event of the btnClear control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnClear_Click(object sender, EventArgs e)
        {

            try
            {
                this._mapColumns.CancelEdit();
                //this._formBindingSource.RaiseListChangedEvents = true;
                this._formBindingSource.ResetBindings(false);

            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }


        /// <summary>
        /// Handles the ValueChanged event of the cmbTableType control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void cmbTableType_ValueChanged(object sender, EventArgs e)
        {
            this._tableTypeID = int.Parse(cmbTableType.Value.ToString());
            //this._tableTypeID = tableTypeValue;
            //if (tableTypeValue == TRANSACTION_TYPE)
            //{
            BindGridComboBoxes();
            _mapColumns.MappingItems = DataSourceManager.GetMapColumns(_dataSourceID, _tableTypeID);
            //if (!int.Equals(grdMapColumns.Rows.Count, 0))
            //{
            //    SetGridCellsData();
            //}

            RemoveUploadIDItemFromMappingItems();

            _formBindingSource.DataSource = _mapColumns;    

            
        }

        /// <summary>
        /// Removes the upload ID item from mapping items.
        /// </summary>
        private void RemoveUploadIDItemFromMappingItems()
        {
            int index = 0;
            bool foundUploadID = false;
            foreach (MappingItem mappingItem in _mapColumns.MappingItems)
            {
                if (mappingItem.SourceItemName == "UploadID")
                {
                    foundUploadID = true;
                    break;
                }
                index++;
            }
            if (foundUploadID == true)
            {
                _mapColumns.MappingItems.RemoveAt(index);
            }
        }

        private ValueList GetApplicationItemsList(int abc)
        {
            ValueList valueList = new ValueList();
            
            return valueList;
        }


       

        /// <summary>
        /// Handles the AfterCellUpdate event of the grdMapColumns control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.CellEventArgs"/> instance containing the event data.</param>
        private void grdMapColumns_AfterCellUpdate(object sender, CellEventArgs e)
        {
            if (isLockingEnabled.Checked)
            {
                Infragistics.Win.UltraWinGrid.UltraGridCell applicationColumnCell = e.Cell.Row.Cells[COL_ApplicationItemID];
                MappingItem item = e.Cell.Row.ListObject as MappingItem;
                int index = _mapColumns.MappingItems.IndexOf(item); // item].ApplicationColumnTypeID = Convert.ToInt32(e.Cell.Tag.ToString());
                if (e.Cell.Row.Cells[COL_ApplicationItemID].Activated == true)
                {
                    if (((Infragistics.Shared.SubObjectBase)(((Infragistics.Win.UltraWinGrid.UltraGridBase)(e.Cell.ValueListResolved)).ActiveRow.ListObject)).Tag.ToString() != null)
                    {
                        //_mapColumns.MappingItems[index].ApplicationColumnTypeID = Convert.ToInt32(((ValueListItem)((ValueList)e.Cell.ValueListResolved.SelectedItemIndex).SelectedItem).Tag.ToString());
                        _mapColumns.MappingItems[index].ApplicationColumnTypeID = Convert.ToInt32(((Infragistics.Shared.SubObjectBase)(((Infragistics.Win.UltraWinGrid.UltraGridBase)(e.Cell.ValueListResolved)).ActiveRow.ListObject)).Tag.ToString());
                    }
                }
                if (string.Equals(e.Cell.Column.Key, COL_Lock, StringComparison.Ordinal))
                {

                    string applicationColumnName = applicationColumnCell.Text;
                    int applicationColumnID = (int)item.ApplicationItemId;

                    if (item != null && !int.Equals(item.ApplicationItemId, -1))
                    {
                        ValueListItem presentInLockedItems = _lockedApplicationColumnList.FindByDataValue((object)applicationColumnID);
                        ValueListItem presentInUnlockedItems = _unlockedApplicationColumnList.FindByDataValue((object)applicationColumnID);
                        if (item.Lock)
                        {

                            if (presentInLockedItems == null)
                                _lockedApplicationColumnList.ValueListItems.Add(new ValueListItem( presentInUnlockedItems.DataValue, presentInUnlockedItems.DisplayText));

                            if (presentInUnlockedItems != null)
                                _unlockedApplicationColumnList.ValueListItems.Remove(presentInUnlockedItems);

                            applicationColumnCell.ValueList = (ValueList)_lockedApplicationColumnList;

                            applicationColumnCell.Activation = Activation.NoEdit;

                            // Check If some other cell which is unlocked contains the same value in application column as selected.
                            // In that case the value will become numeric as the corresponding item is removed from the unlocked list
                            // and we set its value to -1, that will make --Select-- in that cell.
                            foreach (UltraGridRow row in grdMapColumns.Rows)
                            {
                                int value;
                                bool isNumericText = int.TryParse(row.Cells[COL_ApplicationItemID].Text, out value);
                                if (isNumericText)
                                {
                                    row.Cells[COL_ApplicationItemID].Value = -1;
                                }
                            }

                        }
                        else
                        {
                            if (presentInLockedItems != null)
                                _lockedApplicationColumnList.ValueListItems.Remove(presentInLockedItems);

                            if (presentInUnlockedItems == null)
                                _unlockedApplicationColumnList.ValueListItems.Add(new ValueListItem( presentInLockedItems.DataValue,presentInLockedItems.DisplayText));

                            applicationColumnCell.ValueList = (ValueList)_unlockedApplicationColumnList;
                            applicationColumnCell.Activation = Activation.AllowEdit;


                        }
                        applicationColumnCell.Value = applicationColumnID;
                    }
                    else
                    {
                        MessageBox.Show(this, "Please Select Application Column Before locking.", "Position Management", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        item.Lock = false;
                        applicationColumnCell.Activate();
                    }
                }
            }
        }


        /// <summary>
        /// Sets the grid cells data.
        /// </summary>
        //private void SetGridCellsData()
        //{
        //    foreach (UltraGridRow row in grdMapColumns.Rows)
        //    {
        //        MappingItem item = row.ListObject as MappingItem;
        //        Infragistics.Win.UltraWinGrid.UltraGridCell applicationColumnCell = row.Cells[COL_ApplicationItemID];
        //        string applicationColumnName = applicationColumnCell.Text;
        //        int applicationColumnID = (int)applicationColumnCell.Value;

        //        if (item != null && !int.Equals(item.ApplicationItemId, -1))
        //        {
        //            ValueListItem presentInLockedItems = _lockedApplicationColumnList.FindByDataValue((object)applicationColumnID);
        //            ValueListItem presentInUnlockedItems = _unlockedApplicationColumnList.FindByDataValue((object)applicationColumnID);
        //            if (item.Lock)
        //            {

        //                if (presentInLockedItems == null)
        //                    _lockedApplicationColumnList.ValueListItems.Add(presentInUnlockedItems);

        //                if(presentInUnlockedItems != null)
        //                    _unlockedApplicationColumnList.ValueListItems.Remove(presentInUnlockedItems);

        //                applicationColumnCell.ValueList = (ValueList)_lockedApplicationColumnList;
        //                applicationColumnCell.Value = applicationColumnID;
        //                applicationColumnCell.Activation = Activation.NoEdit;            
        //            }                                    
        //        }        		 
        //    }            
        //}


        /// <summary>
        /// Handles the InitializeRow event of the grdMapColumns control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.InitializeRowEventArgs"/> instance containing the event data.</param>
        private void grdMapColumns_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (isLockingEnabled.Checked)
            {
                if (!e.ReInitialize)
                {

                    MappingItem item = e.Row.ListObject as MappingItem;
                    Infragistics.Win.UltraWinGrid.UltraGridCell applicationColumnCell = e.Row.Cells[COL_ApplicationItemID];
                    string applicationColumnName = applicationColumnCell.Text;
                    int applicationColumnID = (int)applicationColumnCell.Value;

                    if (item != null && !int.Equals(item.ApplicationItemId, -1))
                    {
                        ValueListItem presentInLockedItems = _lockedApplicationColumnList.FindByDataValue((object)applicationColumnID);
                        ValueListItem presentInUnlockedItems = _unlockedApplicationColumnList.FindByDataValue((object)applicationColumnID);
                        if (item.Lock)
                        {

                            if (presentInLockedItems == null)
                                _lockedApplicationColumnList.ValueListItems.Add(presentInUnlockedItems);

                            if (presentInUnlockedItems != null)
                                _unlockedApplicationColumnList.ValueListItems.Remove(presentInUnlockedItems);

                            applicationColumnCell.ValueList = (ValueList)_lockedApplicationColumnList;
                            applicationColumnCell.Value = applicationColumnID;
                            applicationColumnCell.Activation = Activation.NoEdit;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the isLockingEnabled control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void isLockingEnabled_CheckedChanged(object sender, EventArgs e)
        {

            SetupBinding(_dataSourceNameID, (int)cmbTableType.Value);            
            if (isLockingEnabled.Checked)
            {
                colLock.Hidden = false;
            }
            else
            {
                colLock.Hidden = true;
            }
            RemoveUploadIDItemFromMappingItems();
        }        
    }
}
