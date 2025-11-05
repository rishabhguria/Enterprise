using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects.PositionManagement;
using Prana.LogManager;
using Prana.PM.DAL;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Drawing;
using System.Windows.Forms;
namespace Prana.PortfolioReports.Controls
{
    public partial class CntrlRealizedPL : UserControl
    {
        public CntrlRealizedPL()
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
            {
                SetButtonsColor();
            }
        }

        private void SetButtonsColor()
        {
            try
            {
                btnExportToExcel.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnExportToExcel.ForeColor = System.Drawing.Color.White;
                btnExportToExcel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnExportToExcel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnExportToExcel.UseAppStyling = false;
                btnExportToExcel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnGetDetailedReport.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnGetDetailedReport.ForeColor = System.Drawing.Color.White;
                btnGetDetailedReport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnGetDetailedReport.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnGetDetailedReport.UseAppStyling = false;
                btnGetDetailedReport.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnGenerateReport.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnGenerateReport.ForeColor = System.Drawing.Color.White;
                btnGenerateReport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnGenerateReport.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnGenerateReport.UseAppStyling = false;
                btnGenerateReport.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        UltraGridBand _gridBandRealizedPL = null;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;


        const string C_COMBO_SELECT = "--Select--";
        #region Grid Columns

        const string CAPTION_ID = "ID";
        const string CAPTION_StartTaxLotID = "StartTaxLotID";
        const string CAPTION_Side = "Side";
        const string CAPTION_ClosingQuantity = "ClosedQty";
        const string CAPTION_OpenQuantity = "OpenQty";
        const string CAPTION_StartDate = "StartDate";
        const string CAPTION_LastActivityDate = "LastActivityDate"; // BB
        const string CAPTION_PositionType = "PositionType";
        const string CAPTION_Quantity = "Quantity";
        const string CAPTION_PNL = "PNLWhenTaxLotsPopulated";
        const string CAPTION_AccountValue = "AccountValue";
        const string CAPTION_Strategy = "Strategy";
        const string CAPTION_Symbol = "Symbol";
        const string CAPTION_AveragePrice = "AveragePrice";
        const string CAPTION_Description = "Description";
        const string CAPTION_Multiplier = "Multiplier";
        const string CAPTION_Commission = "Commission";
        const string CAPTION_Fees = "Fees";
        const string CAPTION_AccountID = "AccountID";
        const string CAPTION_StrategyID = "StrategyID";
        const string CAPTION_PositionStartQuantity = "PositionStartQuantity";
        const string CAPTION_PositionTaxLots = "PositionTaxLots";

        const string CAPTION_PositionState = "Status";

        const string CAPTION_RealizedPNL = "RealizedPNL";
        const string CAPTION_PositionEndDate = "EndDate";

        const string CAPTION_RecordType = "RecordType";
        const string CAPTION_AUECID = "AUECID";

        #endregion

        public void SetUpControl(/*int companyID, int userID*/)
        {
            //_companyID = companyID;
            //BindComboBoxes(companyID, userID);
            dtStartDate.Value = DateTime.Now;
            dtEndDate.Value = DateTime.Now;
            NetPositionList realizedPLPositionList = new NetPositionList();
            grdRealizedPL.DataSource = null;
            grdRealizedPL.DataSource = realizedPLPositionList;

            BindComboes();
        }

        private void grdRealizedPL_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            UltraGridLayout gridLayout = grdRealizedPL.DisplayLayout;

            // Set the appearance for template add-rows. 
            e.Layout.Override.TemplateAddRowAppearance.BackColor = Color.Black;
            e.Layout.Override.TemplateAddRowAppearance.ForeColor = SystemColors.GrayText;

            // Once  the user modifies the contents of a template add-row, it becomes
            // an add-row and the AddRowAppearance gets applied to such rows.            
            e.Layout.Override.AddRowAppearance.BackColor = Color.Black;
            e.Layout.Override.AddRowAppearance.ForeColor = Color.White;

            SetRealizedPLGridAppearanceAndLayout(ref gridLayout);
            e.Layout.Bands[1].Hidden = true;

            _gridBandRealizedPL = grdRealizedPL.DisplayLayout.Bands[0];

            UltraGridColumn colGUID = _gridBandRealizedPL.Columns[CAPTION_ID];
            colGUID.Hidden = true;

            UltraGridColumn colStartTaxLotID = _gridBandRealizedPL.Columns[CAPTION_StartTaxLotID];
            colStartTaxLotID.Hidden = true;


            UltraGridColumn colPositionEndDate = _gridBandRealizedPL.Columns[CAPTION_PositionEndDate];
            colPositionEndDate.Hidden = true;






            //UltraGridColumn colPositionType = _gridBandAddPositions.Columns[CAPTION_PositionType];
            //colPositionType.Hidden = true;


            //UltraGridColumn colInstrumentType = _gridBandAddPositions.Columns[CAPTION_InstrumentType];
            //colInstrumentType.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            //colInstrumentType.Width = 45;
            ////colInstrumentType.ValueList = cmbInstrumentType; BB
            //colInstrumentType.Header.Caption = "Instrument Type";
            //colInstrumentType.Header.VisiblePosition = 1;

            //UltraGridColumn colToBeIncluded = _gridBandRealizedPL.Columns[CAPTION_ToBeIncluded];
            //colToBeIncluded.Width = 45;
            //colToBeIncluded.Header.Caption = "Select";
            //colToBeIncluded.Header.VisiblePosition = 1;
            //colToBeIncluded.CellClickAction = CellClickAction.Edit;
            //colToBeIncluded.CellActivation = Activation.AllowEdit;
            //colToBeIncluded.AllowGroupBy = DefaultableBoolean.False;

            _gridBandRealizedPL.Columns["ToBeIncluded"].Width = 35;
            _gridBandRealizedPL.Columns["ToBeIncluded"].Header.Caption = "Select";
            _gridBandRealizedPL.Columns["ToBeIncluded"].Header.VisiblePosition = 1;
            _gridBandRealizedPL.Columns["ToBeIncluded"].CellClickAction = CellClickAction.Edit;
            _gridBandRealizedPL.Columns["ToBeIncluded"].CellActivation = Activation.AllowEdit;
            _gridBandRealizedPL.Columns["ToBeIncluded"].AllowGroupBy = DefaultableBoolean.False;

            UltraGridColumn colStartDate = _gridBandRealizedPL.Columns[CAPTION_StartDate];
            colStartDate.Width = 55;
            //colTradeDate.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Date;
            colStartDate.MaskInput = "mm/dd/yyyy";
            colStartDate.MaskDataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeLiterals;
            colStartDate.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeLiterals;
            colStartDate.Header.Caption = "Date";
            colStartDate.CellActivation = Activation.NoEdit;
            colStartDate.Header.VisiblePosition = 2;
            //colStartDate.Hidden = true;
            colStartDate.AllowGroupBy = DefaultableBoolean.False;

            _gridBandRealizedPL.Columns["ToBeIncluded"].Width = 35;

            //UltraGridColumn colModifiedAt = _gridBandRealizedPL.Columns[CAPTION_ModifiedAt];
            //colModifiedAt.Width = 55;
            //colModifiedAt.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Date;
            //colModifiedAt.MaskInput = "mm/dd/yyyy";
            //colModifiedAt.MaskDataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeLiterals;
            //colModifiedAt.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeLiterals;
            //colModifiedAt.Header.Caption = "Trade Date";
            //colModifiedAt.Header.VisiblePosition = 1;
            //colModifiedAt.Hidden = true;

            UltraGridColumn colSymbol = _gridBandRealizedPL.Columns[CAPTION_Symbol];
            colSymbol.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Edit;
            colSymbol.CharacterCasing = CharacterCasing.Upper;
            colSymbol.Width = 50;
            colSymbol.Header.Caption = "Symbol";
            colSymbol.Header.VisiblePosition = 3;

            UltraGridColumn colRealizedPNL = _gridBandRealizedPL.Columns[CAPTION_RealizedPNL];
            colRealizedPNL.Width = 45;
            colRealizedPNL.Header.Caption = "Realized P&L";
            colRealizedPNL.Header.VisiblePosition = 4;
            colRealizedPNL.AllowGroupBy = DefaultableBoolean.False;

            UltraGridColumn colPositionState = _gridBandRealizedPL.Columns[CAPTION_PositionState];
            colPositionState.Width = 45;
            colPositionState.Header.Caption = "Position State";
            colPositionState.Header.VisiblePosition = 5;
            colPositionState.AllowGroupBy = DefaultableBoolean.False;

            UltraGridColumn colOpenQuantity = _gridBandRealizedPL.Columns[CAPTION_OpenQuantity];
            colOpenQuantity.Width = 70;
            colOpenQuantity.Header.Caption = "Balance Open Quantity";
            colOpenQuantity.Header.VisiblePosition = 6;
            colOpenQuantity.AllowGroupBy = DefaultableBoolean.False;

            UltraGridColumn colAccount = _gridBandRealizedPL.Columns[CAPTION_AccountValue];
            colAccount.Width = 45;
            colAccount.Header.Caption = "Account";
            colAccount.Header.VisiblePosition = 7;
            colAccount.AllowGroupBy = DefaultableBoolean.True;

            UltraGridColumn colstrategy = _gridBandRealizedPL.Columns[CAPTION_Strategy];
            colstrategy.Width = 45;
            //colstrategy.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
            //colstrategy.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            //colstrategy.ValueList = cmbStrategy;
            colstrategy.Header.Caption = "Strategy";
            colstrategy.Header.VisiblePosition = 8;

            UltraGridColumn colPositionType = _gridBandRealizedPL.Columns[CAPTION_PositionType];
            colPositionType.Width = 45;
            //colPositionType.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            //colPositionType.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            //colPositionType.ValueList = cmbPositionType;
            //colPositionType.Header.Caption = "Side";
            //colPositionType.Header.VisiblePosition = 9;
            colPositionType.Hidden = true;





            UltraGridColumn colClosingQuantity = _gridBandRealizedPL.Columns[CAPTION_ClosingQuantity];
            colClosingQuantity.Width = 45;
            colClosingQuantity.Header.Caption = "Position State";
            //colClosingQuantity.Header.VisiblePosition = 4;
            colClosingQuantity.Hidden = true;

            UltraGridColumn colGeneratedPNL = _gridBandRealizedPL.Columns[CAPTION_PNL];
            //colGeneratedPNL.Width = 45;
            //colGeneratedPNL.Header.Caption = "Realized P&L";
            //colGeneratedPNL.Header.VisiblePosition = 3;
            colGeneratedPNL.Hidden = true;

            UltraGridColumn ColSide = _gridBandRealizedPL.Columns[CAPTION_Side];
            ColSide.Hidden = true;

            UltraGridColumn colPositionStartQuantity = _gridBandRealizedPL.Columns[CAPTION_PositionStartQuantity];
            //colPositionStartQuantity.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.IntegerPositiveWithSpin;
            //colPositionStartQuantity.Width = 60;
            //colPositionStartQuantity.Header.Caption = "Quantity";
            //colPositionStartQuantity.Header.VisiblePosition = 5;
            colPositionStartQuantity.Hidden = true;

            UltraGridColumn colAveragePrice = _gridBandRealizedPL.Columns[CAPTION_AveragePrice];
            colAveragePrice.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DoubleWithSpin;
            colAveragePrice.Width = 60;
            colAveragePrice.Header.Caption = "Average Price";
            //colAveragePrice.Header.VisiblePosition = 6;
            colAveragePrice.Hidden = true;



            UltraGridColumn colDescription = _gridBandRealizedPL.Columns[CAPTION_Description];
            colDescription.Width = 150;
            colDescription.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Edit;
            colDescription.Header.Caption = "Description";
            //colDescription.Header.VisiblePosition = 10;
            colDescription.Hidden = true;



            UltraGridColumn colAccountID = _gridBandRealizedPL.Columns[CAPTION_AccountID];
            colAccountID.Width = 70;
            colAccountID.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            colAccountID.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            //colAccountID.ValueList = _accountsList; BB
            //colAccount.DefaultCellValue = new Account();
            //colAccount.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
            colAccountID.Header.Caption = "Account";
            //colAccountID.Header.VisiblePosition = 6;
            colAccountID.Hidden = true;





            UltraGridColumn colstrategyID = _gridBandRealizedPL.Columns[CAPTION_StrategyID];
            colstrategyID.Width = 80;
            colstrategyID.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            //colstrategy.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            //colstrategyID.ValueList = cmbStrategy; BB
            colstrategyID.Header.Caption = "Strategy";
            //colstrategyID.Header.VisiblePosition = 7;
            colstrategyID.Hidden = true;

            //After taking the latest changes on 22nd May, 07
            UltraGridColumn colMultiplier = _gridBandRealizedPL.Columns[CAPTION_Multiplier];
            colMultiplier.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.IntegerPositiveWithSpin;
            colMultiplier.Width = 60;
            colMultiplier.Header.Caption = "Multiplier";
            colMultiplier.Header.VisiblePosition = 13;
            colMultiplier.Hidden = true;

            UltraGridColumn colCommission = _gridBandRealizedPL.Columns[CAPTION_Commission];
            colCommission.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CurrencyNonNegative;
            colCommission.Width = 75;
            colCommission.Header.Caption = "Commission";
            colCommission.Header.VisiblePosition = 14;
            colCommission.Hidden = true;

            UltraGridColumn colFees = _gridBandRealizedPL.Columns[CAPTION_Fees];
            colFees.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CurrencyNonNegative;
            colFees.Width = 45;
            colFees.Header.Caption = "Fees";
            colFees.Header.VisiblePosition = 15;
            colFees.Hidden = true;

            //UltraGridColumn colPositionStartQuantity = _gridBandAddPositions.Columns[CAPTION_PositionStartQuantity];
            //colPositionStartQuantity.Width = 45;
            //colPositionStartQuantity.Header.Caption = "PositionStartQuantity";
            //colPositionStartQuantity.Header.VisiblePosition = 20;
            //colPositionStartQuantity.Hidden = true;

            UltraGridColumn colPositionTaxLots = _gridBandRealizedPL.Columns[CAPTION_PositionTaxLots];
            colPositionTaxLots.Width = 45;
            colPositionTaxLots.Header.Caption = "PositionTaxLots";
            colPositionTaxLots.Header.VisiblePosition = 21;
            colPositionTaxLots.Hidden = true;

            UltraGridColumn colRecordType = _gridBandRealizedPL.Columns[CAPTION_RecordType];
            colRecordType.Hidden = true;


            UltraGridColumn colAUECID = _gridBandRealizedPL.Columns[CAPTION_AUECID];
            colAUECID.Hidden = true;
        }

        /// <summary>
        /// Sets the grid appearance and layout.
        /// </summary>
        /// <param name="gridLayout">The grid layout.</param>
        private void SetRealizedPLGridAppearanceAndLayout(ref UltraGridLayout gridLayout)
        {

            gridLayout.Appearance.BackColor = Color.Black; //BB
            gridLayout.Override.RowAppearance.BackColor = Color.Black; //BB
            //gridLayout.Override.RowAlternateAppearance.BackColor = Color.FromArgb(64, 64, 64);

            gridLayout.Override.SelectedRowAppearance.BackColor = Color.Black;
            gridLayout.Override.SelectedRowAppearance.BorderColor = Color.White;
            gridLayout.Override.SelectedRowAppearance.ForeColor = Color.White;
            gridLayout.Override.ActiveRowAppearance.ForeColor = Color.White;
            //gridLayout.Override.SelectedCellAppearance.BackColor = Color.Transparent;
            //gridLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.Default;
            //gridLayout.Override.ActiveRowAppearance.BackColor = Color.Transparent;
            ////gridLayout.Override.ActiveCellAppearance.BackColor = Color.Gold;
            ////gridLayout.Override.ActiveRowAppearance.ForeColor = Color.Black;
            //gridLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            //gridLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            gridLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            gridLayout.Override.CellAppearance.BorderColor = Color.Transparent; ;
            gridLayout.Override.RowAppearance.BorderColor = Color.Transparent; ;

            gridLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            gridLayout.ScrollBounds = ScrollBounds.ScrollToLastItem;

            //gridLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            //gridLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            gridLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            gridLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
            gridLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            gridLayout.Override.RowFilterMode = RowFilterMode.AllRowsInBand;

            gridLayout.Override.ColumnSizingArea = ColumnSizingArea.EntireColumn;
            gridLayout.Override.ColumnAutoSizeMode = ColumnAutoSizeMode.VisibleRows;

            //UltraGridLayout gridLayoutHeader = this.grdCreatePosition.DisplayLayout;
            UltraGridBand band = this.grdRealizedPL.DisplayLayout.Bands[0];
            gridLayout.Override.HeaderAppearance.TextVAlign = VAlign.Middle;

            //band.Override.HeaderStyle = HeaderStyle.WindowsXPCommand;
            band.Override.HeaderAppearance.BackColor = Color.WhiteSmoke;
            //band.Override.HeaderAppearance.BackColor2 = Color.DarkGray;
            //band.Override.HeaderAppearance.BackGradientStyle = GradientStyle.Vertical;
            band.Override.HeaderAppearance.ForeColor = Color.Black;
            //band.Override.HeaderAppearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;

            grdRealizedPL.DisplayLayout.Appearance.ForeColor = Color.White;
            this.grdRealizedPL.DisplayLayout.GroupByBox.Hidden = true; // .Bands[0]. GroupHeadersVisible = false;


        }

        private void BindComboes()
        {
            cmbReportType.DisplayMember = "DisplayText";
            cmbReportType.ValueMember = "Value";
            cmbReportType.DataSource = null;
            cmbReportType.DataSource = EnumHelper.ConvertEnumForBindingWithSelect(typeof(ReportType));
            Utils.UltraComboFilter(cmbReportType, "DisplayText");
            cmbReportType.Value = int.MinValue;
        }

        //bool _isGroupBy = false;
        private void cmbReportType_ValueChanged(object sender, EventArgs e)
        {
            if (cmbReportType.Value != null)
            {
                this.grdRealizedPL.DisplayLayout.Bands[0].SortedColumns.Clear();

                if (cmbReportType.Value.ToString() == "GroupBySymbol")
                {
                    //_isGroupBy = true;
                    this.grdRealizedPL.DisplayLayout.Bands[0].SortedColumns.Add(CAPTION_Symbol, false, true);
                }
                if (cmbReportType.Value.ToString() == "GroupByAccounts")
                {
                    //_isGroupBy = true;
                    this.grdRealizedPL.DisplayLayout.Bands[0].SortedColumns.Add(CAPTION_AccountValue, false, true);
                    //this.grdRealizedPL.DisplayLayout.Bands[0].SortedColumns.Add(CAPTION_PositionState, false, true);
                }
                if (cmbReportType.Value.ToString() == "GroupByStrategy")
                {
                    //_isGroupBy = true;
                    this.grdRealizedPL.DisplayLayout.Bands[0].SortedColumns.Add(CAPTION_Strategy, false, true);
                }
            }
        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            DateTime startDate = (DateTime)dtStartDate.Value;
            DateTime endDate = (DateTime)dtEndDate.Value;
            NetPositionList realizedPLPositionList = PortfolioReportManager.GetInstance().GetRealizedPLPositions(startDate, endDate, 1, 1);
            grdRealizedPL.DataSource = null;
            grdRealizedPL.DataSource = realizedPLPositionList;
        }

        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            if (ExportToExcel())
            {
                MessageBox.Show("Report Succesfully saved.", "Reports Save Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool ExportToExcel()
        {
            bool result = false;
            //try
            //{
            NetPositionList realizedPLPOsitions = (NetPositionList)grdRealizedPL.DataSource;
            if (realizedPLPOsitions.Count > 0)
            {
                Infragistics.Documents.Excel.Workbook workBook = new Infragistics.Documents.Excel.Workbook();
                string pathName = null;
                saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.InitialDirectory = Application.StartupPath;
                saveFileDialog1.Filter = "Excel WorkBook Files (*.xls)|*.xls|All Files (*.*)|*.*";
                saveFileDialog1.RestoreDirectory = true;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    pathName = saveFileDialog1.FileName;
                }
                else
                {
                    return result;

                }
                string workbookName = "Report" + DateTime.Now.Date.ToString("yyyyMMdd");
                workBook.Worksheets.Add(workbookName);

                workBook.WindowOptions.SelectedWorksheet = workBook.Worksheets[workbookName];

                workBook = this.ultraGridExcelExporter1.Export(this.grdRealizedPL, workBook.Worksheets[workbookName]);
                //Infragistics.Excel.BIFF8Writer.WriteWorkbookToFile(workBook, pathName);
                workBook.Save(pathName);
                result = true;
            }
            //}
            //catch (Exception ex)
            //{
            //    bool rethrow = Logger.HandleException(ex, Common.POLICY_LOGANDSHOW);
            //    if (rethrow)
            //    {
            //        throw;
            //    }

            //}
            //finally
            //{
            //    #region LogEntry

            //    LogEntry logEntry = new LogEntry("ExportToExcel",
            //        Common.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
            //        FORM_NAME + "ExportToExcel", null);
            //    Logger.LoggerWrite(logEntry);

            //    #endregion
            //}

            return result;
        }

        //private void btnClosingHistory_Click(object sender, EventArgs e)
        //{

        //}

        private NetPositionList _realizedPLPositions = new NetPositionList();
        public NetPositionList RealizedPLPositions
        {
            get
            {

                _realizedPLPositions = new NetPositionList();
                //NetPositionList realizedPLPositionsTemp = (NetPositionList)grdRealizedPL.DataSource;
                //foreach(AddPositionDerived addPositionDerived in realizedPLPositionsTemp)
                //{
                //    if(addPositionDerived.ToBeIncluded == true)
                //    {
                //        _realizedPLPositions.Add(addPositionDerived);
                //    }
                //}
                foreach (UltraGridRow row in grdRealizedPL.Rows)
                {
                    if (row.Cells != null)
                    {
                        if (bool.Parse(row.Cells["ToBeIncluded"].Value.ToString()) == true)
                        {
                            _realizedPLPositions.Add((Position)row.ListObject);
                        }
                    }
                    else
                    {

                        string cellValue = string.Empty;
                        foreach (UltraGridGroupByRow ultraGridGroupByRow in row.ParentCollection.All)
                        {
                            foreach (UltraGridRow gRow in ultraGridGroupByRow.Rows)
                            {
                                cellValue = gRow.Cells["ToBeIncluded"].Value.ToString();
                                if (cellValue.Equals("True"))
                                {
                                    if (!_realizedPLPositions.Contains((Position)gRow.ListObject))
                                    {
                                        _realizedPLPositions.Add((Position)gRow.ListObject);
                                    }
                                }
                            }
                        }
                        //foreach (UltraGridRow groupRow in ((UltraGridGroupByRow)row.ParentCollection.All[0]).Rows)
                        //{
                        //}
                        //UltraGridRow groupByRow = ((UltraGridRow)(((UltraGridGroupByRow)(row.ParentCollection.All[0])).Rows.All[0]));
                        //string cellValue = groupByRow.Cells["ToBeIncluded"].Value.ToString(); //((UltraGridRow)(((UltraGridGroupByRow)(row.ParentCollection.All[0])).Rows.All[0])).Cells["ToBeIncluded"].Value.ToString();
                        //if (cellValue.Equals("True"))
                        //{
                        //    _realizedPLPositions.Add((Position)groupByRow.ListObject);
                        //}
                    }
                }
                return _realizedPLPositions;
            }
        }
    }
}

