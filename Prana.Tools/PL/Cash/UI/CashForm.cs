using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Prana.Interfaces;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Infragistics.Win;
using Prana.CommonDataCache;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using System.Data.SqlClient;


namespace Prana.Tools
{
    public partial class CashForm : UserControl, Prana.Interfaces.ICashManagement
    {
        //Contains The Traded Data 
        List<CashData> DataList;        
        
        //Contains the privious Day DayEnd Data
        List<CompanyFundCashCurrencyValue> YesterdayDataList;

        Dictionary<string, GenericBindingList<CompanyFundCashCurrencyValue>> _DateWiseDayEndDataDictionary;
        public CashForm()
        {
            InitializeComponent();
            Disposed += new EventHandler(CashForm_Disposed);
        }

        #region ICashManagement Members

        void CashForm_Disposed(object sender, EventArgs e)
        {
            if (FormClosedHandler != null)
            {
                FormClosedHandler(this, e);
            }
        }
        public Form Reference()
        {
            return null;
        }

        ICashManagementService _CashManagementServices = null;
        public ICashManagementService CashManagementServices
        {
            set
            {
                _CashManagementServices = value;
                CashDataManager.GetInstance().CashManagementServices = _CashManagementServices;

            }

        }

        #endregion

        
        
        private void btnRunBatch_Click(object sender, EventArgs e)
        {
            try
            {
                //Assigning Date Wise Day End Data in Global Variable _DateWiseDayEndDataDictionary
                CalculateDateWiseDayEndData();

                //Using Global Variable _DateWiseDayEndDataDictionary To Create Date Wise Day End Tabs
                CreateTabsAccordingToDate();

                #region Not In Use Code Section
                //TodayDayEndDataList.Clear();
                //_CashDataList = new GenericBindingList<CashData>();                
                                
                //foreach (CashData subAccountCashValue in DataList)
                //{
                //    if (!_CashDataList.Contains(subAccountCashValue))
                //        _CashDataList.Add(subAccountCashValue.Clone());
                //    else
                //        _CashDataList.GetItem(subAccountCashValue.GetKey()).Amount =_CashDataList.GetItem(subAccountCashValue.GetKey()).Amount+ subAccountCashValue.Amount;
                //}
                

              

                //CompanyFundCashCurrencyValue TodayDayEndData;
                //foreach (CashData _cashdata in _CashDataList)
                //{
                //    TodayDayEndData = new CompanyFundCashCurrencyValue();
                //    TodayDayEndData.Date = _cashdata.PayOutDate;
                //    TodayDayEndData.CashValueLocal = _cashdata.Amount;
                //    TodayDayEndData.FundID = _cashdata.FundID;
                //    TodayDayEndData.LocalCurrencyID = _cashdata.CurrencyID;
                //    if (_YesterdayDataList.ContainsKey(_cashdata.GetKey()))
                //        TodayDayEndData.CashValueLocal += _YesterdayDataList.GetItem(_cashdata.GetKey()).CashValueLocal;
                //    TodayDayEndDataList.Add(TodayDayEndData);

                //}

                //BindGridData<CompanyFundCashCurrencyValue>(grdTodayDayEnd, TodayDayEndDataList);
                #endregion
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



        private Dictionary<string, GenericBindingList<CashData>> getDateWiseDataForCashData()
        {
            Dictionary<string, GenericBindingList<CashData>> _DateWiseData = new Dictionary<string, GenericBindingList<CashData>>();
            GenericBindingList<CashData> _CashDataList;
            string DateKey;
            foreach (CashData subAccountCashValue in DataList)
            {
                //Cutting The Time part of the date time
                DateKey = subAccountCashValue.PayOutDate.ToShortDateString();
                if (!_DateWiseData.ContainsKey(DateKey))
                {
                    _CashDataList = new GenericBindingList<CashData>();
                    _CashDataList.Add(subAccountCashValue.Clone());
                    _DateWiseData.Add(DateKey, _CashDataList);
                }
                else
                {
                    
                    if (!_DateWiseData[DateKey].Contains(subAccountCashValue))
                        _DateWiseData[DateKey].Add(subAccountCashValue.Clone());
                    else
                        _DateWiseData[DateKey].GetItem(subAccountCashValue.GetKey()).Amount += subAccountCashValue.Amount;

                   

                }
            }
            return _DateWiseData;
        }

        //Assigning Date Wise Day End Data in Global Variable _DateWiseDayEndDataDictionary
        private void CalculateDateWiseDayEndData()
        {

            //Getting DateWiseCashDataDictionary
            Dictionary<string, GenericBindingList<CashData>> _DateWiseCashDataDictionary = getDateWiseDataForCashData();

            //Getting DateWiseYesterdayDataDictionary
            GenericBindingList<CompanyFundCashCurrencyValue> _YesterdayDataList = new GenericBindingList<CompanyFundCashCurrencyValue>();
            foreach (CompanyFundCashCurrencyValue _YesterdayData in YesterdayDataList)
            {
                if (!_YesterdayDataList.Contains(_YesterdayData))
                    _YesterdayDataList.Add(_YesterdayData);
            } 

            _DateWiseDayEndDataDictionary = new Dictionary<string, GenericBindingList<CompanyFundCashCurrencyValue>>();
            CompanyFundCashCurrencyValue TodayDayEndData;
            GenericBindingList<CompanyFundCashCurrencyValue> TodayDayEndDataList;
            string keyOfTodayData;
            foreach (GenericBindingList<CashData> _CashDataListOfPerticularDate in _DateWiseCashDataDictionary.Values)
            {
                keyOfTodayData = _CashDataListOfPerticularDate[0].PayOutDate.ToShortDateString();
                TodayDayEndDataList = new GenericBindingList<CompanyFundCashCurrencyValue>();
                foreach (CashData _cashdata in _CashDataListOfPerticularDate)
                {

                    TodayDayEndData = new CompanyFundCashCurrencyValue();
                    TodayDayEndData.Date = _cashdata.PayOutDate;
                    TodayDayEndData.CashValueLocal = _cashdata.Amount;
                    TodayDayEndData.FundID = _cashdata.FundID;
                    TodayDayEndData.LocalCurrencyID = _cashdata.CurrencyID;

                    if (_YesterdayDataList.ContainsKey(_cashdata.GetKey()))
                         TodayDayEndData.CashValueLocal += _YesterdayDataList.GetItem(_cashdata.GetKey()).CashValueLocal;

                    TodayDayEndDataList.Add(TodayDayEndData);
                }
                _DateWiseDayEndDataDictionary.Add(keyOfTodayData, TodayDayEndDataList);


                //For Next Iteration Today Data DayEnd Will Be Privious DayEnd Data

                _YesterdayDataList = TodayDayEndDataList;

            }           
        }


        //Using Global Variable _DateWiseDayEndDataDictionary To Create Date Wise Day End Tabs
        private void CreateTabsAccordingToDate()
        {
            tabCntlDayEndData.TabPages.Clear();
            UltraGrid grdTodayDayEnd;
            if (_DateWiseDayEndDataDictionary != null)
            {
                foreach (string Date in _DateWiseDayEndDataDictionary.Keys)
                {
                    grdTodayDayEnd = new UltraGrid();
                    CustomizeUltraGrid(grdTodayDayEnd);
                    BindGridTodayDayEndData(grdTodayDayEnd, _DateWiseDayEndDataDictionary[Date]);
                    tabCntlDayEndData.TabPages.Add(Date, Date);
                    tabCntlDayEndData.TabPages[Date].Controls.Add(grdTodayDayEnd);
                    grdTodayDayEnd.Dock = DockStyle.Fill;

                }
            }
            else
            {
                grdTodayDayEnd = new UltraGrid();
                CustomizeUltraGrid(grdTodayDayEnd);
                //BindGridTodayDayEndData(grdTodayDayEnd, null);
                tabCntlDayEndData.TabPages.Add("Day End Data");
                
                tabCntlDayEndData.TabPages[0].Controls.Add(grdTodayDayEnd);
                tabCntlDayEndData.TabPages[0].BackColor = Color.Black;
                grdTodayDayEnd.Dock = DockStyle.Fill;
            }

        }

        private void CustomizeUltraGrid(UltraGrid grd2Customize)
        {
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            grd2Customize.DisplayLayout.Appearance = appearance17;
            grd2Customize.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            grd2Customize.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            grd2Customize.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;
            grd2Customize.DisplayLayout.EmptyRowSettings.ShowEmptyRows = true;
            grd2Customize.DisplayLayout.EmptyRowSettings.Style = Infragistics.Win.UltraWinGrid.EmptyRowStyle.HideRowSelector;
            //grd2Customize.DisplayLayout.MaxColScrollRegions = 1;
            //grd2Customize.DisplayLayout.MaxRowScrollRegions = 1;
            appearance18.BackColor = System.Drawing.Color.LightSlateGray;
            appearance18.BackColor2 = System.Drawing.Color.DarkSlateGray;
            appearance18.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            appearance18.BorderColor = System.Drawing.Color.DimGray;
            appearance18.FontData.BoldAsString = "True";
            appearance18.ForeColor = System.Drawing.Color.White;
            grd2Customize.DisplayLayout.Override.ActiveRowAppearance = appearance18;
            grd2Customize.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.Yes;
            grd2Customize.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            grd2Customize.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Free;
            grd2Customize.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            grd2Customize.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.True;
            grd2Customize.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            grd2Customize.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            //grd2Customize.DisplayLayout.Override.CellPadding = 0;
            //grd2Customize.DisplayLayout.Override.CellSpacing = 0;
            grd2Customize.DisplayLayout.Override.ColumnSizingArea = Infragistics.Win.UltraWinGrid.ColumnSizingArea.EntireColumn;
            appearance19.BorderColor = System.Drawing.Color.Transparent;
            appearance19.ForeColor = System.Drawing.Color.White;
            grd2Customize.DisplayLayout.Override.GroupByRowAppearance = appearance19;
            appearance20.FontData.Name = "Tahoma";
            appearance20.FontData.SizeInPoints = 8F;
            appearance20.TextHAlignAsString = "Center";
            grd2Customize.DisplayLayout.Override.HeaderAppearance = appearance20;
            grd2Customize.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            grd2Customize.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.Standard;
            appearance21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance21.ForeColor = System.Drawing.Color.White;
            appearance21.TextHAlignAsString = "Right";
            appearance21.TextVAlignAsString = "Middle";
            grd2Customize.DisplayLayout.Override.RowAlternateAppearance = appearance21;
            appearance22.BackColor = System.Drawing.Color.Black;
            appearance22.ForeColor = System.Drawing.Color.White;
            appearance22.TextHAlignAsString = "Right";
            appearance22.TextVAlignAsString = "Middle";
            grd2Customize.DisplayLayout.Override.RowAppearance = appearance22;
            grd2Customize.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            grd2Customize.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            grd2Customize.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance23.BackColor = System.Drawing.Color.Transparent;
            appearance23.BorderColor = System.Drawing.Color.Transparent;
            appearance23.FontData.BoldAsString = "True";
            grd2Customize.DisplayLayout.Override.SelectedRowAppearance = appearance23;
            grd2Customize.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            grd2Customize.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            grd2Customize.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            grd2Customize.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            grd2Customize.DisplayLayout.Override.SpecialRowSeparator = Infragistics.Win.UltraWinGrid.SpecialRowSeparator.None;
            grd2Customize.DisplayLayout.Override.SpecialRowSeparatorHeight = 0;
            grd2Customize.DisplayLayout.Override.SummaryDisplayArea = Infragistics.Win.UltraWinGrid.SummaryDisplayAreas.InGroupByRows;
            grd2Customize.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            appearance24.BackColor = System.Drawing.SystemColors.ControlLight;
            grd2Customize.DisplayLayout.Override.TemplateAddRowAppearance = appearance24;
            grd2Customize.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            grd2Customize.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            grd2Customize.DisplayLayout.UseFixedHeaders = true;
            grd2Customize.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            grd2Customize.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            grd2Customize.Font = new System.Drawing.Font("Tahoma", 8.25F);
            grd2Customize.Location = new System.Drawing.Point(0, 22);
            grd2Customize.Name = "grd2Customize";
            //grd2Customize.Size = new System.Drawing.Size(369, 190);
            grd2Customize.TabIndex = 102;
            grd2Customize.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;

        }


        public static bool CheckForHoliday(DateTime DateToCheck)
        {
            bool isHoliday=false;
            if (DateToCheck.DayOfWeek == DayOfWeek.Saturday || DateToCheck.DayOfWeek == DayOfWeek.Sunday)
                isHoliday = true;

            //This Code Is not working will check it laterON
            //else
            //{
            //    //DAL CODE
            //    SqlCommand cmd = new SqlCommand("select count(*) from T_AUECHolidays where datediff(dd,HolidayDate,'" + DateToCheck + "')=0");
            //    Database db = DatabaseFactory.CreateDatabase();
            //    int NoOfRows = db.ExecuteNonQuery(cmd);
            //    if (NoOfRows > 0)
            //        isHoliday = true;
            //    else
            //        isHoliday = false;

            //}
            return isHoliday;
        }
       
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (_DateWiseDayEndDataDictionary != null && _DateWiseDayEndDataDictionary.Count > 0)
                {
                    //foreach (GenericBindingList<CompanyFundCashCurrencyValue> DayEndDataList in _DateWiseDayEndDataDictionary.Values)
                    _CashManagementServices.CalculateTodayDayEndData(_DateWiseDayEndDataDictionary);
                    MessageBox.Show("Day End Data Saved", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No Day End Data Available To Be Saved !", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }            
        }          

        private void addRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (grdGetLots.DataSource == null)
            {
                btnGetFx_Click(null, null);

            }
            DataList = ((List<CashData>)grdGetLots.DataSource);
            DataList.Add(new CashData());
            grdGetLots.DataBind();
        }

        public event EventHandler FormClosedHandler;

        private void CashForm_Load(object sender, EventArgs e)
        {
            btnGetFx_Click(null, null);
            
        }

        private void lblDate_Click(object sender, EventArgs e)
        {

        }

        private void grdGetLots_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {

        }

        private void BindGridData<T>(UltraGrid grd,List<T> DataSource)
        {
            try
            {
                grd.DataSource = DataSource;
                grd.DataBind();
                if (grd.Name == "grdGetLots")
                {
                    UltraGridBand band = grdGetLots.DisplayLayout.Bands[0];
                    band.Columns["CashID"].Hidden = true;
                    band.Columns["TaxLotState"].Hidden = true;
                    //band.Columns["TaxLot"].Hidden = false;
                    //band.Columns["FundID"].Hidden = true;

                    //band.Columns["FundName"].Header.Caption = "Fund";
                    band.Columns["FundName"].Hidden = true;

                    //band.Columns["AcName"].Header.Caption = "Account Name";
                    band.Columns["AcName"].Hidden = true;
                    band.Columns["SubAcID"].Hidden = true;
                    //band.Columns["SubAcName"].Header.Caption = "Sub Account Name";
                    band.Columns["SubAcName"].Hidden = true;
                    band.Columns["Amount"].Header.Caption = "Notional Amount";
                    band.Columns["TradedDate"].Header.Caption = "TradedDate";
                    band.Columns["FXRate"].Header.Caption = "FXRate";
                    band.Columns["CurrencyID"].Header.Caption = "CurrencyID";
                }
                else if (grd.Name == "grdYesterdayEnd")
                {
                    UltraGridBand band = grdYesterdayEnd.DisplayLayout.Bands[0];
                    band.Columns["CashCurrencyID"].Hidden = true;
                    band.Columns["FundName"].Hidden = true;
                    band.Columns["BaseCurrencyID"].Hidden = true;
                   // band.Columns["LocalCurrencyID"].Hidden = true;
                    band.Columns["CashValueBase"].Hidden = true;
                   
                }
                //else if (grd.Name == "grdTodayDayEnd")
                //{
                //    UltraGridBand band = grdTodayDayEnd.DisplayLayout.Bands[0];
                //    band.Columns["CashCurrencyID"].Hidden = true;
                //    band.Columns["FundName"].Hidden = true;
                //    band.Columns["BaseCurrencyID"].Hidden = true;
                //    band.Columns["LocalCurrencyID"].Hidden = true;
                //    band.Columns["CashValueBase"].Hidden = true;

                //}
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        private void BindGridTodayDayEndData(UltraGrid grd, GenericBindingList<CompanyFundCashCurrencyValue> DataSource)
        {
            try
            {
                grd.DataSource = DataSource;
                grd.DataBind();
                //UltraGridBand band = grd.DisplayLayout.Bands[0];
                //band.Columns["CashCurrencyID"].Hidden = true;
                //band.Columns["FundName"].Hidden = true;
                //band.Columns["BaseCurrencyID"].Hidden = true;
                //band.Columns["LocalCurrencyID"].Hidden = true;
                //band.Columns["CashValueBase"].Hidden = true;


            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdGetLots_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (!e.ReInitialize)
            {
                

            }
        }

        private void btnGetFx_Click(object sender, EventArgs e)
        {
            try
            {
                #region grdGetLots Binding Section
                if (!CheckForHoliday(dtDate.DateTime))
                {
                    //Retriving All The Data where Date>=dtDate.DateTime
                    DataList = _CashManagementServices.GetCashImpact(dtDate.DateTime);
                    BindGridData<CashData>(grdGetLots, DataList);
                }
                else
                {
                    MessageBox.Show("No Data available because it's Holiday !", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }

                #endregion

                #region grdYesterdayEnd Binding Section
                DateTime _priviousDayDate = dtDate.DateTime.AddDays(-1);
                while (CheckForHoliday(_priviousDayDate))
                {
                    _priviousDayDate = _priviousDayDate.AddDays(-1);
                }
                YesterdayDataList = _CashManagementServices.GetCashImpactYesterDay(dtDate.DateTime.AddDays(-1));
                BindGridData<CompanyFundCashCurrencyValue>(grdYesterdayEnd,YesterdayDataList);
                lblYesterdayDate.Text =_priviousDayDate.ToShortDateString();

                #endregion

                #region grdTodayDayEnd Binding Section
                //_DateWiseDayEndDataDictionary = null;
                CreateTabsAccordingToDate();

                //TodayDayEndDataList.Clear();
                //BindGridData<CompanyFundCashCurrencyValue>(grdTodayDayEnd, TodayDayEndDataList);

                #endregion
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




        #region Unused Code
        //private Dictionary<string, GenericBindingList<CompanyFundCashCurrencyValue>> getDateWiseDataForCompanyFundCashCurrencyValue()
        //{
        //    Dictionary<string, GenericBindingList<CompanyFundCashCurrencyValue>> _DateWiseData = new Dictionary<string, GenericBindingList<CompanyFundCashCurrencyValue>>();
        //    GenericBindingList<CompanyFundCashCurrencyValue> _YesterdayDataList;
        //    string DateKey;
        //    foreach (CompanyFundCashCurrencyValue _YesterdayData in YesterdayDataList)
        //    {
        //        DateKey = _YesterdayData.Date.ToShortDateString();
        //        if (!_DateWiseData.ContainsKey(DateKey))
        //        {
        //            _YesterdayDataList = new GenericBindingList<CompanyFundCashCurrencyValue>();
        //            _YesterdayDataList.Add(_YesterdayData);
        //            _DateWiseData.Add(DateKey, _YesterdayDataList);
        //        }
        //        else
        //            _DateWiseData[DateKey].Add(_YesterdayData);
        //    }


        //    return _DateWiseData;
        //}
        //private Dictionary<string, List<CompanyFundCashCurrencyValue>> CalculateDateWiseDayEndData_old()
        //{

        //    Dictionary<string, GenericBindingList<CashData>> _DateWiseCashDataDictionary = getDateWiseDataForCashData();
        //    Dictionary<string, GenericBindingList<CompanyFundCashCurrencyValue>> _DateWisePriviousDayEndDataDictionary = getDateWiseDataForCompanyFundCashCurrencyValue();
        //     _DateWiseDayEndDataDictionary = new Dictionary<string, GenericBindingList<CompanyFundCashCurrencyValue>>();

        //    //Cheking If PriviousDayEndData Exist otherwise Message No Privious Data Availabe
        //    //if (_DateWisePriviousDayEndDataDictionary.Count > 0)
        //    //{                
        //    CompanyFundCashCurrencyValue TodayDayEndData;
        //    List<CompanyFundCashCurrencyValue> TodayDayEndDataList;
        //    string keyOfYesterdayData; DateTime keyOfTodayData; DateTime _priviousDayDate;


        //    foreach (GenericBindingList<CashData> _CashDataListOfPerticularDate in _DateWiseCashDataDictionary.Values)
        //    {
        //        //GenericBindingList<CashData> _CashDataListOfPerticularDate = _DateWiseCashDataDictionary[i];
        //        TodayDayEndDataList = new List<CompanyFundCashCurrencyValue>();

        //        //Getting Date Of Current list that is the key of dictionaries
        //        keyOfTodayData = _CashDataListOfPerticularDate[0].PayOutDate;
        //        _priviousDayDate = keyOfTodayData.AddDays(-1);

        //        //Cheking For Holidays
        //        while (CheckForHoliday(_priviousDayDate))
        //        {
        //            _priviousDayDate = _priviousDayDate.AddDays(-1);
        //        }

        //        keyOfYesterdayData = _priviousDayDate.ToShortDateString();
        //        //Cheking If PriviousDayEndData Exist otherwise Message No Privious 'Date' Data Availabe
        //        if (_DateWisePriviousDayEndDataDictionary.ContainsKey(keyOfYesterdayData))
        //        {
        //            foreach (CashData _cashdata in _CashDataListOfPerticularDate)
        //            {
        //                TodayDayEndData = new CompanyFundCashCurrencyValue();
        //                TodayDayEndData.Date = _cashdata.PayOutDate;
        //                TodayDayEndData.CashValueLocal = _cashdata.Amount;
        //                TodayDayEndData.FundID = _cashdata.FundID;
        //                TodayDayEndData.LocalCurrencyID = _cashdata.CurrencyID;

        //                if (_DateWisePriviousDayEndDataDictionary[keyOfYesterdayData].ContainsKey(_cashdata.GetKey()))
        //                    TodayDayEndData.CashValueLocal += _DateWisePriviousDayEndDataDictionary[keyOfYesterdayData].GetItem(_cashdata.GetKey()).CashValueLocal;

        //                TodayDayEndDataList.Add(TodayDayEndData);
        //            }
        //            _DateWiseDayEndDataDictionary.Add(keyOfTodayData.ToShortDateString(), TodayDayEndDataList);
        //        }
        //    }
        //    //}

        //    //BindGridData<CompanyFundCashCurrencyValue>(grdTodayDayEnd, TodayDayEndDataList);

        //    return _DateWiseDayEndDataDictionary;
        //}
        #endregion
    }
}