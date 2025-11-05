
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.Sql;

using System.Configuration;
using Prana.Global;
using Prana.Utilities.MiscUtilities;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Infragistics.Win.UltraWinGrid;
using Prana.CommonDataCache;
using Prana.Interfaces;
using Prana.WCFConnectionMgr;
using Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;

namespace Prana.UDATool
{
    public partial class UsrCtrlSymbolUDAData : UserControl
    {
        /// <summary>
        /// Event and delegate for sending request for UDA Data 
        /// </summary>
        /// <param name="udaDataReqObj"></param>
        public delegate void GetUDASymbolInfo(UDADataReqObj udaDataReqObj);
        public event GetUDASymbolInfo GetUDASymbolInfoEventHandler;

        /// <summary>
        /// Event and delegate for sending uddated Security data to save. 
        /// </summary>
        /// <param name="_SecMasterObjList"></param>
        public delegate void SaveUDASymbolInfo(UDASymbolDataCollection UDASymbolDataCol);
        public event SaveUDASymbolInfo SaveUDASymbolInfoEventHandler;

        UDASymbolDataCollection _collection = new UDASymbolDataCollection();
        UDASymbolDataCollection _changedCollection = new UDASymbolDataCollection();

        Dictionary<string, UDASymbolData> _changedDataCollection = new Dictionary<string, UDASymbolData>();
       

        public UsrCtrlSymbolUDAData()
        {
            InitializeComponent();
        }
        private bool _isChanged = false;
        public UDASymbolDataCollection Collection
        {
            get { return _collection; }
            set { _collection = value; }
        }
        ProxyBase<IPranaPositionServices> _pranaPositionServices = null;
        public ProxyBase<IPranaPositionServices> PranaPositionServices
        {
            set
            {
                _pranaPositionServices = value;
            }
        }


        /// <summary>
        /// Get UDA symbol Data for selected symbol View.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetData_Click(object sender, EventArgs e)
        {

            try
            {
                btnSave.Enabled = false;
                btnSave.Refresh();
                statusStrip1.ForeColor = System.Drawing.Color.Black;
                btnGetData.Text = "Loading Data";
                btnGetData.Refresh();
                toolStripStatusLabel1.Text = "Loading Data...";
                statusStrip1.Refresh();

                UDADataReqObj udaDataReqObj = new UDADataReqObj();
                Prana.BusinessObjects.SecMasterConstants.UDASymbolsViewType viewSymbol = Prana.BusinessObjects.SecMasterConstants.UDASymbolsViewType.Current; ;
                if (rdobtnCurrent.Checked)
                {
                    udaDataReqObj.ViewSymbol = Prana.BusinessObjects.SecMasterConstants.UDASymbolsViewType.Current;
                }

                if (rdobtnHistorical.Checked)
                {
                    udaDataReqObj.ViewSymbol = Prana.BusinessObjects.SecMasterConstants.UDASymbolsViewType.Historical;
                }

                if (GetUDASymbolInfoEventHandler != null)
                {
                    GetUDASymbolInfoEventHandler(udaDataReqObj);

                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Set revieved Symbols UDA data on UI.
        /// </summary>
        /// <param name="SecMasterObjList"></param>
        internal void HandelUDASymbolDataResponse(UDASymbolDataCollection UDASymbolDataCol)
        {

            try
            {
                btnGetData.Text = "Get Data";
                if (UDASymbolDataCol.Count > 0)
                {
                BindGridData();
                    grdSymbolData.DataSource = UDASymbolDataCol;

                    toolStripStatusLabel1.Text = "Data loaded!";
                btnSave.Enabled = true;
                btnSave.Refresh();
            }
                else {

                    toolStripStatusLabel1.Text = "No data found!";
                }
            }

            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }

        }

        private void BindUDAData()
        {
            UDACollection assetCollection = CentralDataManager.GetCollection(UDATypes.AssetClass.ToString());
            if (assetCollection != null)
            {
                grdSymbolData.DisplayLayout.Bands[0].Columns["AssetID"].ValueList = CentralDataManager.GetValueList(assetCollection);
                grdSymbolData.DisplayLayout.Bands[0].Columns["AssetID"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            }

            UDACollection securityTypeCollection = CentralDataManager.GetCollection(UDATypes.SecurityType.ToString());
            if (securityTypeCollection != null)
            {

                grdSymbolData.DisplayLayout.Bands[0].Columns["SecurityTypeID"].ValueList = CentralDataManager.GetValueList(securityTypeCollection);
                grdSymbolData.DisplayLayout.Bands[0].Columns["SecurityTypeID"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            }

            UDACollection sectorCollecion = CentralDataManager.GetCollection(UDATypes.Sector.ToString());
            if (sectorCollecion != null)
            {

                grdSymbolData.DisplayLayout.Bands[0].Columns["SectorID"].ValueList = CentralDataManager.GetValueList(sectorCollecion);
                grdSymbolData.DisplayLayout.Bands[0].Columns["SectorID"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            }

            UDACollection subSectorCollecion = CentralDataManager.GetCollection(UDATypes.SubSector.ToString());
            if (subSectorCollecion != null)
            {

                grdSymbolData.DisplayLayout.Bands[0].Columns["SubSectorID"].ValueList = CentralDataManager.GetValueList(subSectorCollecion);
                grdSymbolData.DisplayLayout.Bands[0].Columns["SubSectorID"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            }

            UDACollection countryCollecion = CentralDataManager.GetCollection(UDATypes.Country.ToString());
            if (countryCollecion != null)
            {

                grdSymbolData.DisplayLayout.Bands[0].Columns["CountryID"].ValueList = CentralDataManager.GetValueList(countryCollecion);
                grdSymbolData.DisplayLayout.Bands[0].Columns["CountryID"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            }
        }
        /// <summary>
        /// save changed symbols UDA sata
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                if (_changedCollection.Count > 0)
                {
                    btnGetData.Enabled = false;
                    btnGetData.Refresh();
                    btnSave.Text = "Saving Data...";
                    btnSave.Refresh();
                    toolStripStatusLabel1.Text = "Saving UDA Data...";
                    statusStrip1.Refresh();

                    if (SaveUDASymbolInfoEventHandler != null)
                        {
                        SaveUDASymbolInfoEventHandler(_changedCollection);
                    }

                    //TODO ask for still refresh data or not.
                    //Refresh UDA data on Client cache
                    //CachedDataManager.GetInstance.RefershUDAData();

                    //Refresh UDA data on Position services
                   // _pranaPositionServices.InnerChannel.RefershUDADataCache();

                _changedCollection.Clear();
               

                    btnSave.Text = "Save";
                    btnSave.Refresh();
                    toolStripStatusLabel1.Text = "";
                    statusStrip1.Refresh();
                    btnGetData.Enabled = true;
                    btnGetData.Refresh();

                    _isChanged = false;
                }
                else
                {
                    toolStripStatusLabel1.Text = "Nothing to Save";
                    statusStrip1.Refresh();
                }

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Get Screen shot of current view as image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnScreenShot_Click(object sender, EventArgs e)
        {
            SnapShotManager.GetInstance().TakeSnapshot(this.ParentForm);
        }


        // Nothing is happening on this fuction we can remove this - om 
        private void UsrCtrlSymbolUDAData_Load(object sender, EventArgs e)
        {

        }

        private void grdSymbolData_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                grdSymbolData.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
                Infragistics.Win.UltraWinGrid.UltraGridBand band = grdSymbolData.DisplayLayout.Bands[0];

                band.Columns["AssetID"].Header.Caption = "Asset Category";
                band.Columns["SecurityTypeID"].Header.Caption = "Security Type";
                band.Columns["SectorID"].Header.Caption = "Sector";
                band.Columns["SubSectorID"].Header.Caption = "Sub-Sector";
                band.Columns["CountryID"].Header.Caption = "Country";
                band.Columns["CompanyName"].Header.Caption = "Description";
                band.Columns["CompanyName"].CellActivation = Activation.NoEdit;
                band.Columns["Symbol"].CellActivation = Activation.NoEdit;

                band.Columns["Symbol"].Header.VisiblePosition = 1;
                band.Columns["CompanyName"].Header.VisiblePosition = 2;
                band.Columns["CountryID"].Header.VisiblePosition = 3;
                band.Columns["AssetID"].Header.VisiblePosition = 4;
                band.Columns["SectorID"].Header.VisiblePosition = 5;
                band.Columns["SubSectorID"].Header.VisiblePosition = 6;
                band.Columns["SecurityTypeID"].Header.VisiblePosition = 7;
                band.Columns["UnderlyingSymbol"].Hidden = true;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }

        }
        /// <summary>
        /// Export Symbols UDA Data in MS Excel file formate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExcelExport_Click(object sender, EventArgs e)
        {
            try
            {
                ExcelAndPrintUtilities excelAndPrintUtilities = new ExcelAndPrintUtilities();
                excelAndPrintUtilities.ExportToExcel(grdSymbolData);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdSymbolData_CellListSelect(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            _isChanged = true;
            grdSymbolData.UpdateData();
        }

        public bool IsChanged
        {
            get { return _isChanged; }
            set { _isChanged = value; }
        }

        public void SaveUDASymbolData()
        {
            if (_isChanged)
            {
                btnSave_Click(null, null);
            }
        }

        /// <summary>
        /// Handle Radio click btns - prompt user to save changes and get data according to selected view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdobtnAll_Click(object sender, EventArgs e)
        {
            try
            {
                //Prompt to user for save if any changes in UDA symbol data
                if (_isChanged)
                {
                    DialogResult result = MessageBox.Show("Would you like to save UDA changes?", "Warning!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        btnSave_Click(null, null);
                    }
                    if (result == DialogResult.No)
                    {
                        _isChanged = false;
                    }
                    if (result == DialogResult.Cancel)
                    {
                        RadioButton rb = (RadioButton)sender;
                        if (rb.Text == "Historical")
                        {
                            rdobtnHistorical.Checked = false;
                            rdobtnCurrent.Checked = true;
                            return;
                        }
                        else
                        {
                            rdobtnHistorical.Checked = true;
                            rdobtnCurrent.Checked = false;
                            return;
                        }
                    }
                }
                //get UDA Data 
                btnGetData_Click(null, null);

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }


        }


        private void BindGridData()
        {
            try
            {
                _collection.Clear();
                _changedCollection.Clear();
                grdSymbolData.DataSource = _collection;
                BindUDAData();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        // Commmented this function becuase no refrence has been found... OM 
        //private void SymbolDataCheck(ViewSymbol viewSymbol)
        //{
        //try
        //{
        //UDASymbolDataCollection SymbolDataCollection = DataManager.GetSymbolData(viewSymbol);
        ////foreach (SymbolData data in SymbolDataCollection)
        ////{
        ////    _collection.Add(data);
        ////}
        //foreach (UDASymbolData data in SymbolDataCollection)
        //{
        //    //if (!data.Symbol.Equals(data.UnderlyingSymbol) && !data.UnderlyingSymbol.Equals(string.Empty))
        //    //{
        //    //    foreach (SymbolData data1 in _collection)
        //    //    {
        //    //        if (data1.Symbol.Equals(data.UnderlyingSymbol))
        //    //        {
        //    //            count++;
        //    //        }
        //    //    }
        //    //    if (count == 0)
        //    //    {
        //    //        string companyName = DataManager.GetSymbolUdaData(data.UnderlyingSymbol);

        //    //        symbolData = new SymbolData(data.UnderlyingSymbol, companyName, 0, data.SecurityTypeID, data.SectorID, data.SubSectorID, data.CountryID, data.UnderlyingSymbol);

        //    //        _collection.Add(symbolData);
        //    //    }
        //    //}
        //    if (data.CountryID.Equals(int.MinValue) || data.SectorID.Equals(int.MinValue) || data.SubSectorID.Equals(int.MinValue))
        //{
        //        foreach (UDASymbolData sd in _collection)
        //    {
        //            if (data.UnderlyingSymbol.Equals(sd.Symbol) && !data.UnderlyingSymbol.Equals(string.Empty))
        //        {
        //                data.CountryID = sd.CountryID;
        //                data.SectorID = sd.SectorID;
        //                data.SubSectorID = sd.SubSectorID;
        //            }
        //        }
        //    }
        //    //count = 0;
        //    _collection.Add(data);

        //    //_collection = SymbolDataCollection;
        //    //grdSymbolData.Refresh();

        //        _collection.Add(symbolData);
        //    }
        //}
        //          if (data.CountryID.Equals(int.MinValue) || data.SectorID.Equals(int.MinValue) || data.SubSectorID.Equals(int.MinValue))
        //         {
        //             foreach (SymbolData sd in _collection)
        //            {
        //                 if (data.UnderlyingSymbol.Equals(sd.Symbol) && !data.UnderlyingSymbol.Equals(string.Empty))
        //                {
        //                 data.CountryID = sd.CountryID;
        //                  data.SectorID = sd.SectorID;
        //                  data.SubSectorID = sd.SubSectorID;
        //             }
        //          }
        //    }
        //            count = false;
        //              _collection.Add(data);
        //             }
        //       }
        //          catch (Exception ex)
        //          {
        //            bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
        //            if (rethrow)
        //             {
        //                throw;
        //          }
        //       }
        // }




        /// <summary>
        /// Handle on UDA data changed on grid. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdSymbolData_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            try
            {
                grdSymbolData.UpdateData();
                UDASymbolData symbolData = (UDASymbolData)e.Cell.Row.ListObject;
                symbolData.Symbol = symbolData.Symbol.Trim(); // removed unwanted space from symbols. http://jira.nirvanasolutions.com:8080/browse/IGUANA-30
                _changedDataCollection.Clear();
                if (!_changedCollection.Contains(symbolData))
                {
                    _changedCollection.Add(symbolData);
                }
                if (!_changedDataCollection.ContainsKey(symbolData.Symbol))
                {
                    _changedDataCollection.Add(symbolData.Symbol, symbolData);
                }
                string col = e.Cell.Column.Key;

                SetSameUDAForAllDerivedSymbols(e.Cell);

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

        }

        /// <summary>
        /// Set same UDA value for derived symbols
        /// </summary>
        /// <param name="cell"></param>
        private void SetSameUDAForAllDerivedSymbols(UltraGridCell cell)
        {
            //TODO check its working or not and change value on derivatives if not set.
            try
            {
                foreach (UltraGridRow row in grdSymbolData.Rows)
                {
                    //Modified By : Ankit Gupta
                    // On May 07, 2013
                    // This code has been modified for the special handling for future and future options and it won't affect other asset classes.

                    string UnderlyingSymbol = row.Cells["UnderlyingSymbol"].Value.ToString();
                    int index = UnderlyingSymbol.IndexOf(" ");
                    string UnderlyingRootSymbol = string.Empty;
                    if (index > 0)
                    {
                        UnderlyingRootSymbol = UnderlyingSymbol.Substring(0, index);
                    }
                    else
                    {
                        UnderlyingRootSymbol = UnderlyingSymbol;
                    }

                    if ((_changedDataCollection.ContainsKey(UnderlyingSymbol) || _changedDataCollection.ContainsKey(UnderlyingRootSymbol)) && row.ListObject != cell.Row.ListObject)
                    {
                        row.Cells[cell.Column.Key].Value = cell.Value;
                        UDASymbolData currentSymbolData = (UDASymbolData)row.ListObject;
                        if (!_changedCollection.Contains(currentSymbolData))
                        {
                            _changedCollection.Add(currentSymbolData);
                        }
                        if (!_changedDataCollection.ContainsKey(currentSymbolData.Symbol))
                        {
                            _changedDataCollection.Add(currentSymbolData.Symbol, currentSymbolData);
                        }

                        //if (row.Cells["UnderlyingSymbol"].Value.ToString() == symbolData.Symbol && row.ListObject != cell.Row.ListObject)
                        //{
                        //    row.Cells[cell.Column.Key].Value = cell.Value;
                        //    SymbolData currentSymbolData = (SymbolData)row.ListObject;
                        //    if (!_changedCollection.Contains(currentSymbolData))
                        //    {
                        //        _changedCollection.Add(currentSymbolData);

                        //    }
                        //}
                    }
                }

            }

            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Set Text on status bar 
        /// </summary>
        /// <param name="Msg"></param>
        internal void SetStatusBarText(String Msg)
        {
            toolStripStatusLabel1.Text = string.Empty;
            toolStripStatusLabel1.Text = Msg.ToString();
            
        }
    }

        
    }

 
