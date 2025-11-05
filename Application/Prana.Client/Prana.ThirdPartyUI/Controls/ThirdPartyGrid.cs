using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.ThirdParty;
using Prana.LogManager;
using Prana.ThirdPartyUI.Forms;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace Prana.ThirdPartyReport.Controls
{
    public partial class ThirdPartyGrid : UserControl, ISupportInitialize, IComparer
    {
        public DataSet _dsForXMLFile = null;
        public CheckBoxOnHeader_CreationFilter HeaderCheckBoxUnallocated = new CheckBoxOnHeader_CreationFilter();

        private const string HEADCOL_GROUPENDS = "GROUPENDS";
        private const string HEADCOL_PBUNIQUEID = "PBUniqueID";
        private const string HEADCOL_ROWHEADER = "RowHeader";
        private const string HEADCOL_GroupAllocationReq = "GroupAllocationReq";
        private const string HEADCOL_FILEHEADER = "FileHeader";
        private const string HEADCOL_FILEFOOTER = "FileFooter";

        private const string HEADCOL_TAXLOTSTATE = "TAXLOTSTATE";

        private const string HEADCOL_EntityID = "EntityID";

        private const string HEADCOL_CheckBox = "checkBox";
        private const string HEADCOL_ALLOCQTY = "ALLOCQTY";
        private const string HEADCOL_ISCAPCHANGEREQ = "IsCaptionChangeRequired";

        private string _groupByColumn = HEADCOL_TAXLOTSTATE;

        private const string HEADCOL_CLORDERID = "ClOrderID";
        private const string HEADCOL_ORDERID = "OrderID";
        private const string HEADCOL_ALLOCID = "AllocID";
        private const string CONST_VIEW_DETAILS = "View Details";

        public SerializableDictionary<string, List<ThirdPartyOrderDetail>> OrderDetails = new SerializableDictionary<string, List<ThirdPartyOrderDetail>>();
        public string GroupByColumn
        {
            set { _groupByColumn = value; }
        }

        private const int TYPE_VENDOR = 2;

        public object DataSource
        {
            get { return grdThirdParty.DataSource; }
            set
            {
                try
                {
                    grdThirdParty.DataSource = null;
                    grdThirdParty.DataSource = value;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }
        }

        public ThirdPartyGrid()
        {
            InitializeComponent();
            this.grdThirdParty.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdThirdParty.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdThirdParty.DisplayLayout.Override.HeaderPlacement = Infragistics.Win.UltraWinGrid.HeaderPlacement.OncePerGroupedRowIsland;
            this.grdThirdParty.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
        }

        //void grdThirdParty_BindingContextChanged(object sender, EventArgs e)
        //{
        //    SetColumnHidden();
        //    AddCheckBoxinGrid();
        //    //SetDefaultFilters();
        //}

        private void grdThirdParty_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                grdThirdParty.DisplayLayout.Override.GroupByRowInitialExpansionState = GroupByRowInitialExpansionState.Expanded;
                UltraGridBand band = this.grdThirdParty.DisplayLayout.Bands[0];
                if (band.Columns.Exists(_groupByColumn))
                {
                    band.SortedColumns.Add(_groupByColumn, false, true);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdThirdParty_AfterRowExpanded(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
        {
            if (e != null && e.Row != null && e.Row.Activated)
            {
                e.Row.ExpandAll();
            }
        }

        private void grdThirdParty_Error(object sender, Infragistics.Win.UltraWinGrid.ErrorEventArgs e)
        {
            //  e.Cancel = true;
        }

        public void AddCheckBoxinGrid()
        {
            try
            {
                grdThirdParty.CreationFilter = HeaderCheckBoxUnallocated;
                grdThirdParty.DisplayLayout.Bands[0].Columns.Add(HEADCOL_CheckBox, "");
                grdThirdParty.DisplayLayout.Bands[0].Columns[HEADCOL_CheckBox].DataType = typeof(bool);
                grdThirdParty.DisplayLayout.Bands[0].Columns[HEADCOL_CheckBox].CellClickAction = CellClickAction.EditAndSelectText;
                SetCheckBoxAtFirstPosition();
            }
            catch (Exception)
            {
                return;
            }
        }

        public void SetCheckBoxAtFirstPosition()
        {
            grdThirdParty.DisplayLayout.Bands[0].Columns[HEADCOL_CheckBox].Hidden = false;
            grdThirdParty.DisplayLayout.Bands[0].Columns[HEADCOL_CheckBox].Header.VisiblePosition = 0;
            grdThirdParty.DisplayLayout.Bands[0].Columns[HEADCOL_CheckBox].Width = 10;
        }

        public void SetHeaderChecked()
        {
            UltraGridRow[] rows = grdThirdParty.Rows.GetFilteredInNonGroupByRows();
            foreach (UltraGridRow dr in rows)
            {
                dr.Cells[HEADCOL_CheckBox].Value = true;
            }
        }

        public bool ColumnExists(string colName)
        {
            if (colName.Equals("CompanyID") || colName.Equals("ThirdPartyID") || colName.Equals("CompanyAccountID") ||
                colName.Equals("AssetID") || colName.Equals("UnderLyingID") || colName.Equals("CurrencyID") ||
                colName.Equals("ExchangeID") || colName.Equals("AUECID") || colName.Equals("CompanyAccountTypeID") ||
                colName.Equals("CommissionRateTypeID") || colName.Equals("ThirdPartyTypeID") || colName.Equals("CompanyCVID") ||
                colName.Equals("VenueID") || colName.Equals(HEADCOL_EntityID) || colName.Equals("CounterPartyID") ||
                colName.Equals("TradAccntID") || colName.Equals("GroupEnds") || colName.Equals(HEADCOL_GroupAllocationReq) ||
                colName.Equals(HEADCOL_FILEHEADER) || colName.Equals(HEADCOL_FILEFOOTER) || colName.Equals(HEADCOL_PBUNIQUEID) ||
                colName.Equals(HEADCOL_ROWHEADER) || colName.Equals("TaxLotStateID") || colName.ToUpper().Equals(HEADCOL_ALLOCQTY) ||
                colName.Equals("TaxLots_Id") || colName.Equals("Group_Id") || colName.Equals("TaxLots_ThirdPartyFlatFileDetail") ||
                colName.Equals("TaxLotState1") || colName.Equals("IsCaptionChangeRequired") || colName.Equals("FromDeleted") ||
                 colName.ToUpper().Equals("XMLMAINTAG") || colName.ToUpper().Equals("XMLCHILDTAG"))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public void SetColumnHidden()
        {
            int bandCount = grdThirdParty.DisplayLayout.Bands.Count;
            ColumnsCollection columns = grdThirdParty.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columns)
            {
                if (ColumnExists(column.Key))
                {
                    column.Hidden = true;
                }
                else
                {
                    if (column.Key.Equals(HEADCOL_CheckBox))
                        column.CellActivation = Activation.AllowEdit;
                    else
                        column.CellActivation = Activation.NoEdit;

                }
            }
            if (bandCount.Equals(2))
            {
                ColumnsCollection childColumns = grdThirdParty.DisplayLayout.Bands[1].Columns;
                foreach (UltraGridColumn column in childColumns)
                {
                    if (ColumnExists(column.Key))
                    {
                        column.Hidden = true;
                    }
                    else
                    {
                        column.CellActivation = Activation.NoEdit;
                    }
                }
            }

        }

        /* unused method
        public void SetColourToRow(UltraGridRow activeRow)
        {

            UltraGridBand band = this.grdThirdParty.DisplayLayout.Bands[0];
            if (activeRow.Cells.Exists(HEADCOL_TAXLOTSTATE))
            {
                band.SortedColumns.Add(HEADCOL_TAXLOTSTATE, false, true);
                band.Columns[HEADCOL_TAXLOTSTATE].GroupByComparer = new ThirdPartyGrid();
            }

            if (activeRow.Cells.Exists(HEADCOL_TAXLOTSTATE))
            {
                if (activeRow.Cells[HEADCOL_TAXLOTSTATE] != null)
                {
                    if (activeRow.Cells[HEADCOL_TAXLOTSTATE].Value.ToString().ToUpper().Equals(Prana.BusinessObjects.AppConstants.PranaTaxLotState.Allocated.ToString().ToUpper()))
                    {
                        activeRow.Appearance.ForeColor = Color.Orange;
                    }
                    else if (activeRow.Cells[HEADCOL_TAXLOTSTATE].Value.ToString().ToUpper().Equals(Prana.BusinessObjects.AppConstants.PranaTaxLotState.Sent.ToString().ToUpper()))
                    {
                        activeRow.Appearance.ForeColor = Color.LightGray;
                    }
                    else if (activeRow.Cells[HEADCOL_TAXLOTSTATE].Value.ToString().ToUpper().Equals(Prana.BusinessObjects.AppConstants.PranaTaxLotState.Amemded.ToString().ToUpper()))
                    {
                        activeRow.Appearance.ForeColor = Color.GreenYellow;
                    }
                    else if (activeRow.Cells[HEADCOL_TAXLOTSTATE].Value.ToString().ToUpper().Equals(Prana.BusinessObjects.AppConstants.PranaTaxLotState.Deleted.ToString().ToUpper()))
                    {
                        activeRow.Appearance.ForeColor = Color.Red;
                    }
                    else if (activeRow.Cells[HEADCOL_TAXLOTSTATE].Value.ToString().ToUpper().Equals(Prana.BusinessObjects.AppConstants.PranaTaxLotState.Ignore.ToString().ToUpper()))
                    {
                        activeRow.Appearance.ForeColor = Color.HotPink;
                    }
                }
            }
        }
        */
        public void SetIgnoreTaxLotFilters()
        {
            if (grdThirdParty.Rows.Count > 0)
            {
                UltraGridBand band = grdThirdParty.DisplayLayout.Bands[0];
                if (band.Columns.Exists(HEADCOL_TAXLOTSTATE))
                {
                    band.ColumnFilters.ClearAllFilters();
                    band.ColumnFilters[HEADCOL_TAXLOTSTATE].LogicalOperator = FilterLogicalOperator.Or;
                    band.ColumnFilters[HEADCOL_TAXLOTSTATE].FilterConditions.Add(FilterComparisionOperator.Equals, Prana.BusinessObjects.AppConstants.PranaTaxLotState.Ignore);

                    //TODO: Add OnSetIgnoreTaxLotFilters
                    //BackToolStripMenuItem.Visible = true;
                    //IgnoreToolStripMenuItem.Visible = false;
                }
            }
            else
            {
                //BackToolStripMenuItem.Visible = false;
                //IgnoreToolStripMenuItem.Visible = false;
            }
        }
        public void SetActiveTaxLotFilters()
        {

            if (grdThirdParty.Rows.Count > 0)
            {
                UltraGridBand band = grdThirdParty.DisplayLayout.Bands[0];
                if (band.Columns.Exists(HEADCOL_TAXLOTSTATE))
                {
                    band.ColumnFilters.ClearAllFilters();
                    band.ColumnFilters[HEADCOL_TAXLOTSTATE].LogicalOperator = FilterLogicalOperator.Or;
                    band.ColumnFilters[HEADCOL_TAXLOTSTATE].FilterConditions.Add(FilterComparisionOperator.Equals, Prana.BusinessObjects.AppConstants.PranaTaxLotState.Ignore);

                    //BackToolStripMenuItem.Visible = true;
                    //IgnoreToolStripMenuItem.Visible = false;
                }
            }
            else
            {
                //BackToolStripMenuItem.Visible = false;
                //IgnoreToolStripMenuItem.Visible = false;
            }
        }

        private void ThirdPartyGrid_Load(object sender, EventArgs e)
        {

        }

        public void BeginInit()
        {

        }

        public void EndInit()
        {
            // throw new NotImplementedException();
        }

        #region IComparer Members
        /// <summary>
        /// this method is implemeted from icomparer interface in order to custom sort for taxlots. 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(object x, object y)
        {
            UltraGridGroupByRow xObj = (UltraGridGroupByRow)x;
            UltraGridGroupByRow yObj = (UltraGridGroupByRow)y;
            try
            {
                //following xml contain name of sorted columns in sorted order
                string filePath = Application.StartupPath + @"\MappingFiles\ThirdPartyXML\ThirdPartyTaxlotStateSortPreference.xml";
                if (System.IO.File.Exists(filePath))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(filePath);
                    List<string> ThirdPartySortingOrder = new List<string>();
                    foreach (XmlNode xmlNode in xmlDoc.DocumentElement.ChildNodes)
                    {
                        ThirdPartySortingOrder.Add(xmlNode.InnerText);
                    }

                    if (xObj.Rows.Count > 0 && yObj.Rows.Count > 0)
                    {
                        if (xObj.Rows[0].Cells.Exists("TaxLotState") && yObj.Rows[0].Cells.Exists("TaxLotState"))
                        {
                            string state1 = xObj.Rows[0].Cells["TaxLotState"].Text.ToUpper();
                            string state2 = yObj.Rows[0].Cells["TaxLotState"].Text.ToUpper();

                            int i = ThirdPartySortingOrder.IndexOf(state1);
                            int j = ThirdPartySortingOrder.IndexOf(state2);
                            if (i >= 0 && j >= 0)
                            {
                                if (i > j)
                                    return 1;
                                else if (i < j)
                                    return -1;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            // Compare the group rows by the number of items they contain.
            return 0;
        }
        #endregion

        //private void grdThirdParty_FilterCellValueChanged(object sender, FilterCellValueChangedEventArgs e)
        //{          
        //}
        public void SaveState(string file)
        {

            System.IO.FileStream fs = null;

            try
            {
                // Open a new file to save the layout to.
                fs = new System.IO.FileStream(file, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None);
            }
            #region Catch
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
            #endregion


            // Save the layout to the file.
            this.grdThirdParty.DisplayLayout.Save(fs, PropertyCategories.All);

            // Close the file.
            fs.Close();

        }
        /// <summary>
        /// Load State
        /// </summary>
        /// <param name="file"></param>
        public void LoadState(string file)
        {
            // Following code loads the saved layout from a file.

            System.IO.FileStream fs = null;
            if (File.Exists(file) == false) return;
            try
            {
                // Open the file where the layout has been saved before.
                fs = new System.IO.FileStream(file, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
            }
            #region Catch
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
            #endregion


            // Load the layout from the input stream by calling Load method. It is very important that you reset the
            // Position of the file stream to where the layout data begins in case the file stream has
            // been read from previously.
            fs.Position = 0;
            this.grdThirdParty.DisplayLayout.Load(fs, PropertyCategories.All);

            // Close the file
            fs.Close();
        }

        /// <summary>
        /// Get Filtered Row DataSet
        /// </summary>
        /// <returns></returns>
        public DataSet GetFilteredRowDataSet(bool isViewGenerateSendAll = false)
        {

            if (grdThirdParty.DataSource == null) return null;

            DataSet dataSet = null;
            //ultraTabControl1.Tabs["activetaxlots"].Selected = true;
            grdThirdParty.UpdateData();
            UltraGridRow[] rows = grdThirdParty.Rows.GetFilteredInNonGroupByRows();
            DataSet ds = (DataSet)grdThirdParty.DataSource;

            if (ds.Tables == null || ds.Tables.Count == 0)
                return null;
            DataTable dsTable = ds.Tables[0];
            DataTable dt = new DataTable();
            dt.TableName = "ThirdPartyFlatFileDetail";
            int iColCountHeader = dsTable.Columns.Count;
            for (int i = 0; i < iColCountHeader; i++)
            {
                dt.Columns.Add(dsTable.Columns[i].ColumnName);
            }
            //DataRow dr;
            foreach (UltraGridRow row in rows)
            {
                bool isSelected = isViewGenerateSendAll ? true : (bool)row.Cells[HEADCOL_CheckBox].Value;
                if (isSelected /*&& !row.Cells["TaxLotState"].Value.ToString().Equals(Prana.BusinessObjects.AppConstants.PranaTaxLotState.Ignore.ToString())*/)
                {
                    //dr = dt.NewRow();
                    DataRowView dtRowview = (DataRowView)row.ListObject;
                    //dr.ItemArray = dtRowview.Row.ItemArray;
                    //dt.Rows.Add(dr);
                    dt.ImportRow(dtRowview.Row);
                    if (row.HasChild())
                    {
                        //DataRow drChild;
                        foreach (UltraGridRow childrow in row.ChildBands[0].Rows)
                        {
                            //drChild = dt.NewRow();
                            DataRowView dtRowviewChild = (DataRowView)childrow.ListObject;
                            //drChild.ItemArray = dtRowviewChild.Row.ItemArray;
                            //dt.Rows.Add(drChild);
                            dt.ImportRow(dtRowviewChild.Row);
                        }
                    }
                }
            }

            if (dt.Rows.Count > 0)
            {
                dataSet = new DataSet();
                dataSet.DataSetName = "ThirdPartyFlatFileDetailCollection";
                dataSet.Tables.Add(dt);
            }
            return dataSet;

        }

        /// <summary>
        /// This method is to get selected data in xml form
        /// </summary>
        /// <param name="dataSourceXml"></param>
        /// <returns></returns>
        public string GetSelectedDataXml(string dataSourceXml)
        {
            try
            {
                if(grdThirdParty.DataSource == null) return null;

                grdThirdParty.UpdateData();
                UltraGridRow[] rows = grdThirdParty.Rows.GetFilteredInNonGroupByRows();
                DataSet ds = (DataSet)grdThirdParty.DataSource;

                if (ds.Tables == null || ds.Tables.Count == 0)
                    return null;

                DataSet dataSet = new DataSet();
                dataSet.ReadXml(new StringReader(dataSourceXml));

                foreach(UltraGridRow row in rows)
                {
                    if(!(bool)row.Cells[HEADCOL_CheckBox].Value)
                    {
                        DataRowView dtRowview = (DataRowView)row.ListObject;
                        var allocId = dtRowview["AllocId"];

                        DataRow toDelete = null;
                        foreach(DataRow dataRow in dataSet.Tables[0].Rows)
                        {
                            if (dataRow["AllocId"].Equals(allocId))
                            {
                                toDelete = dataRow;
                                break;
                            }
                        }
                        if(toDelete != null)
                        {
                            dataSet.Tables[0].Rows.Remove(toDelete);
                        }
                    }
                }
                return dataSet.GetXml();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        /// <summary>
        /// Update Captions
        /// </summary>
        /// <param name="_dsXML"></param>
        public void UpdateCaptions(DataSet _dsXML)
        {
            try
            {
                if (_dsXML == null || _dsXML.Tables.Count == 0) return;
                _dsForXMLFile = _dsXML.Copy();
                string captionChangeRequiredValue = string.Empty;
                bool captionChangeRequiredExists = _dsXML.Tables[0].Columns.Contains(HEADCOL_ISCAPCHANGEREQ);
                if (captionChangeRequiredExists)
                {
                    // Handling based on Taxlot State of Column Captions, Check If IsCaptionChangeRequired tag is added in xslt but Column captions are missing in xslt.
                    //Jira: - http://jira.nirvanasolutions.com:8080/browse/PRANA-4620
                    //Taxlots state for Trade will be somthing value(Like allocated,sent etc) but for caption it will be taxlotState.
                    if (_dsXML.Tables[0].Rows[0][HEADCOL_TAXLOTSTATE].ToString().ToUpper() == HEADCOL_TAXLOTSTATE || _dsXML.Tables[0].Rows[0][HEADCOL_TAXLOTSTATE].ToString().ToUpper() == "TRUE")
                    {
                        if (_dsXML.Tables[0].Rows.Count > 1)
                        {
                            captionChangeRequiredValue = _dsXML.Tables[0].Rows[1][HEADCOL_ISCAPCHANGEREQ].ToString();
                            if (!String.IsNullOrEmpty(captionChangeRequiredValue) && captionChangeRequiredValue.Trim().ToUpper().Equals("TRUE"))
                            {
                                DataTable dtCaption = _dsXML.Tables[0].Copy();
                                foreach (DataColumn col in _dsXML.Tables[0].Columns)
                                {
                                    DataRow rowCaption = dtCaption.Rows[0];
                                    foreach (DataColumn captionCol in dtCaption.Columns)
                                    {
                                        if (captionCol.ColumnName.Equals(col.ColumnName) && !ColumnExists(col.ColumnName) && !col.ColumnName.Equals("TaxlotState"))
                                        {
                                            col.ColumnName = rowCaption[captionCol.ColumnName].ToString();
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        _dsXML.Tables[0].Rows[0].Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private bool HandlingForGTCReplacedCase(List<ThirdPartyOrderDetail> orderDetails)
        {
            bool isViewDetailsNeeded = true;
            try
            {
                HashSet<string> clOrderIds = new HashSet<string>();
                HashSet<string> orderIds = new HashSet<string>();
                foreach (var item in orderDetails)
                {
                    clOrderIds.Add(item.CLOrderID);
                    orderIds.Add(item.OrderID);
                }
                isViewDetailsNeeded = clOrderIds.Count > 1 || orderIds.Count > 1;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return isViewDetailsNeeded;
        }

        /// <summary>
        /// Handles the InitializeRow event of the grdThirdParty control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdThirdParty_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                if (e.Row.ParentRow == null && e.Row.Band.Columns.Exists(HEADCOL_ALLOCID))
                {
                    var rowAllocId = e.Row.Cells[HEADCOL_ALLOCID].Value?.ToString();
                    if (OrderDetails != null && !string.IsNullOrEmpty(rowAllocId) && OrderDetails.ContainsKey(rowAllocId) && OrderDetails[rowAllocId].Count > 1)
                    {
                        bool isViewDetailsNeeded = HandlingForGTCReplacedCase(OrderDetails[rowAllocId]);
                        if (e.Row.Band.Columns.Exists(HEADCOL_CLORDERID) && isViewDetailsNeeded)
                        {
                            e.Row.Cells[HEADCOL_CLORDERID].Value = CONST_VIEW_DETAILS;
                            e.Row.Cells[HEADCOL_CLORDERID].Appearance.ForeColor = Color.Blue;
                            e.Row.Cells[HEADCOL_CLORDERID].Appearance.FontData.Underline = Infragistics.Win.DefaultableBoolean.True;
                        }
                        else if (e.Row.Band.Columns.Exists(HEADCOL_CLORDERID))
                        {
                            e.Row.Cells[HEADCOL_CLORDERID].Value = OrderDetails[rowAllocId][0].CLOrderID;
                        }

                        if (e.Row.Band.Columns.Exists(HEADCOL_ORDERID) && isViewDetailsNeeded)
                        {
                            e.Row.Cells[HEADCOL_ORDERID].Value = CONST_VIEW_DETAILS;
                            e.Row.Cells[HEADCOL_ORDERID].Appearance.ForeColor = Color.Blue;
                            e.Row.Cells[HEADCOL_ORDERID].Appearance.FontData.Underline = Infragistics.Win.DefaultableBoolean.True;
                        }
                        else if (e.Row.Band.Columns.Exists(HEADCOL_ORDERID))
                        {
                            e.Row.Cells[HEADCOL_ORDERID].Value = OrderDetails[rowAllocId][0].OrderID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the click event for cells in the "ClOrderID" or "OrderID" columns of the grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdThirdParty_ClickCell(object sender, ClickCellEventArgs e)
        {
            try
            {
                if (e.Cell.Column.Key == HEADCOL_CLORDERID || e.Cell.Column.Key == HEADCOL_ORDERID)
                {
                    var rowAllocId = e.Cell.Row.Cells[HEADCOL_ALLOCID].Value?.ToString();
                    if (OrderDetails != null && !string.IsNullOrEmpty(rowAllocId) && OrderDetails.ContainsKey(rowAllocId) && e.Cell.Value.ToString().Equals(CONST_VIEW_DETAILS))
                    {
                        ViewGroupedDetails orderDetailForm = new ViewGroupedDetails(OrderDetails[rowAllocId]);
                        orderDetailForm.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}