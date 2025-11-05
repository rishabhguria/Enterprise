using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects.PositionManagement;
using Prana.LogManager;
using Prana.PM.DAL;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Prana.PortfolioReports
{
    public partial class ClosingHistory : Form
    {
        public ClosingHistory()
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

                btnClose.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnClose.ForeColor = System.Drawing.Color.White;
                btnClose.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnClose.UseAppStyling = false;
                btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

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

        //public ClosingHistory(int companyID, int userID)
        //{
        //    InitializeComponent();
        //    AddPositionDerivedList addRealizedPLList = new AddPositionDerivedList();
        //    grdHistoryPositions.DataSource = addRealizedPLList;

        //    BindComboes();    
        //}

        public void SetUpForm(NetPositionList positionList)
        {
            AllocatedTradesList positionTaxLotPositions = new AllocatedTradesList();
            if (positionList.Count > 0)
            {
                //AddPositionDerivedList addRealizedPLList = new AddPositionDerivedList();
                StringBuilder positionIDStringBuilder = new StringBuilder();
                //positionIDStringBuilder.Append("'");
                foreach (Position addPositionDerived in positionList)
                {
                    //positionIDStringBuilder.Append('
                    positionIDStringBuilder.Append(addPositionDerived.ID);
                    positionIDStringBuilder.Append(",");
                }
                int len = positionIDStringBuilder.Length;
                if (positionIDStringBuilder.Length > 0)
                {
                    positionIDStringBuilder.Remove((len - 1), 1);
                }
                positionTaxLotPositions = PortfolioReportManager.GetInstance().GetHistoryPositions(positionIDStringBuilder, 1, 1);
            }

            grdHistoryPositions.DisplayLayout.Bands[0].Summaries.Clear();
            grdHistoryPositions.DataSource = null;
            grdHistoryPositions.DataSource = positionTaxLotPositions;
            //grdHistoryPositions.DataSource = historyPositionList;

            BindComboes();
        }

        UltraGridBand _gridBandHistoryPositions = null;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;

        const string C_COMBO_SELECT = "--Select--";
        #region Grid Columns

        const string CAPTION_AlloctionID = "ID";
        //const string CAPTION_StartTaxLotID = "StartTaxLotID";
        const string CAPTION_PositionID = "PositionID";
        const string CAPTION_AccountValue = "AccountValue";
        const string CAPTION_StrategyValue = "StrategyValue";
        const string CAPTION_TradeStartDate = "TradeDate";
        const string CAPTION_PositionType = "Side";
        const string CAPTION_Symbol = "Symbol";
        const string CAPTION_AveragePrice = "AveragePrice";
        const string CAPTION_Quantity = "Quantity";
        const string CAPTION_SideID = "SideID";
        const string CAPTION_IsPosition = "IsPosition";
        const string CAPTION_Commission = "Commission";
        const string CAPTION_Fees = "Fees";
        const string CAPTION_NetNotionalValue = "NetNotionalValue";
        const string CAPTION_PNL = "PNL";
        const string CAPTION_ParentPositionBalanceQuantity = "ParentPositionBalanceQuantity";
        const string CAPTION_AUECID = "AUECID";
        const string CAPTION_PositionEndDate = "ParentPositionEndDate";
        const string CAPTION_PositionStartDate = "PositionStartDate";


        #endregion

        private void grdHistoryPositions_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            // Get Column Chooser for the grid
            e.Layout.Override.RowSelectors = DefaultableBoolean.True;
            e.Layout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            UltraGridLayout gridLayout = grdHistoryPositions.DisplayLayout;

            // Set the appearance for template add-rows. 
            e.Layout.Override.TemplateAddRowAppearance.BackColor = Color.Black;
            e.Layout.Override.TemplateAddRowAppearance.ForeColor = SystemColors.GrayText;

            // Once  the user modifies the contents of a template add-row, it becomes
            // an add-row and the AddRowAppearance gets applied to such rows.            
            e.Layout.Override.AddRowAppearance.BackColor = Color.Black;
            e.Layout.Override.AddRowAppearance.ForeColor = Color.White;

            SetHistoryPositionGridAppearanceAndLayout(ref gridLayout);
            if (e.Layout.Bands.Count > 1)
            {
                e.Layout.Bands[1].Hidden = true;
            }

            _gridBandHistoryPositions = grdHistoryPositions.DisplayLayout.Bands[0];


            UltraGridColumn colAlloctionID = _gridBandHistoryPositions.Columns[CAPTION_AlloctionID];
            colAlloctionID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colAlloctionID.Hidden = true;

            UltraGridColumn colPositionID = _gridBandHistoryPositions.Columns[CAPTION_PositionID];
            colPositionID.Hidden = true;





            //UltraGridColumn colPositionType = _gridBandAddPositions.Columns[CAPTION_PositionType];
            //colPositionType.Hidden = true;


            //UltraGridColumn colInstrumentType = _gridBandAddPositions.Columns[CAPTION_InstrumentType];
            //colInstrumentType.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            //colInstrumentType.Width = 45;
            ////colInstrumentType.ValueList = cmbInstrumentType; BB
            //colInstrumentType.Header.Caption = "Instrument Type";
            //colInstrumentType.Header.VisiblePosition = 1;

            UltraGridColumn colPositionStartDate = _gridBandHistoryPositions.Columns[CAPTION_PositionStartDate];
            colPositionStartDate.Width = 55;
            //colPositionStartDate.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Date;
            colPositionStartDate.MaskInput = "mm/dd/yyyy";
            //colPositionStartDate.MaskDataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeLiterals;
            //colPositionStartDate.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeLiterals;
            colPositionStartDate.Header.Caption = "Open Date";
            colPositionStartDate.CellActivation = Activation.NoEdit;
            colPositionStartDate.Header.VisiblePosition = 1;
            //colPositionStartDate.Hidden = true;

            UltraGridColumn colPositionEndDate = _gridBandHistoryPositions.Columns[CAPTION_PositionEndDate];
            colPositionEndDate.Width = 55;
            colPositionEndDate.MaskInput = "mm/dd/yyyy";
            //colPositionEndDate.MaskDataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeLiterals;
            //colPositionEndDate.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeLiterals;
            colPositionEndDate.Header.Caption = "Closing Date";
            colPositionEndDate.CellActivation = Activation.NoEdit;
            colPositionEndDate.Header.VisiblePosition = 2;

            //UltraGridColumn colModifiedAt = _gridBandHistoryPositions.Columns[CAPTION_ModifiedAt];
            //colModifiedAt.Width = 55;
            //colModifiedAt.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Date;
            //colModifiedAt.MaskInput = "mm/dd/yyyy";
            //colModifiedAt.MaskDataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeLiterals;
            //colModifiedAt.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeLiterals;
            //colModifiedAt.Header.Caption = "Trade Date";
            //colModifiedAt.Header.VisiblePosition = 1;
            //colModifiedAt.Hidden = true;

            UltraGridColumn colSymbol = _gridBandHistoryPositions.Columns[CAPTION_Symbol];
            colSymbol.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Edit;
            colSymbol.CharacterCasing = CharacterCasing.Upper;
            colSymbol.Width = 45;
            colSymbol.Header.Caption = "Symbol";
            colSymbol.Header.VisiblePosition = 3;



            UltraGridColumn colPositionType = _gridBandHistoryPositions.Columns[CAPTION_PositionType];
            colPositionType.Width = 45;
            colPositionType.Header.Caption = "Position Type";
            colPositionType.Header.VisiblePosition = 4;

            UltraGridColumn colRealizedPNL = _gridBandHistoryPositions.Columns[CAPTION_PNL];
            colRealizedPNL.Width = 65;
            colRealizedPNL.Header.Caption = "Realized P&L";
            colRealizedPNL.Header.VisiblePosition = 5;


            UltraGridColumn colPositionStartQuantity = _gridBandHistoryPositions.Columns[CAPTION_Quantity];
            colPositionStartQuantity.Width = 45;
            colPositionStartQuantity.Header.Caption = "Quantity";
            colPositionStartQuantity.Header.VisiblePosition = 6;

            UltraGridColumn colOpenQuantity = _gridBandHistoryPositions.Columns[CAPTION_ParentPositionBalanceQuantity];
            colOpenQuantity.Width = 45;
            colOpenQuantity.Header.Caption = "Balance Quantity";
            colOpenQuantity.Header.VisiblePosition = 7;

            UltraGridColumn colAccount = _gridBandHistoryPositions.Columns[CAPTION_AccountValue];
            //colAccount.Width = 45;
            //colAccount.Header.Caption = "Account";
            //colOpenQuantity.Header.VisiblePosition = 6;
            //colAccount.AllowGroupBy = DefaultableBoolean.True;
            colAccount.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colAccount.Hidden = true;

            UltraGridColumn colstrategy = _gridBandHistoryPositions.Columns[CAPTION_StrategyValue];
            //colstrategy.Width = 45;
            //colstrategy.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
            //colstrategy.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
            //colstrategy.ValueList = cmbStrategy;
            //colstrategy.Header.Caption = "Strategy";
            colstrategy.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colstrategy.Hidden = true;

            UltraGridColumn colTradeStartDate = _gridBandHistoryPositions.Columns[CAPTION_TradeStartDate];
            colTradeStartDate.Width = 45;
            colTradeStartDate.Header.Caption = "Trade Start Date";
            colTradeStartDate.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colTradeStartDate.Hidden = true;



            UltraGridColumn colAveragePrice = _gridBandHistoryPositions.Columns[CAPTION_AveragePrice];
            colAveragePrice.Width = 45;
            colAveragePrice.Header.Caption = "Position State";
            //colAveragePrice.Header.VisiblePosition = 4;
            colAveragePrice.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colAveragePrice.Hidden = true;

            UltraGridColumn colSideID = _gridBandHistoryPositions.Columns[CAPTION_SideID];
            //colSideID.Width = 45;
            //colSideID.Header.Caption = "Realized P&L";
            //colSideID.Header.VisiblePosition = 3;
            colSideID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colSideID.Hidden = true;

            UltraGridColumn colIsPosition = _gridBandHistoryPositions.Columns[CAPTION_IsPosition];
            //colIsPosition.Width = 45;
            //colIsPosition.Header.Caption = "Position State";
            //colIsPosition.Header.VisiblePosition = 4;
            colIsPosition.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colIsPosition.Hidden = true;



            //UltraGridColumn colSymbolConvention = _gridBandAddPositions.Columns[CAPTION_SymbolConvention];
            ////colSymbolConvention.Width = 60;
            ////colSymbolConvention.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            ////colSymbolConvention.ValueList = cmbSymbolConvention;
            ////colSymbolConvention.Header.Caption = "Symbol Convention";
            ////colSymbolConvention.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
            //colSymbolConvention.Hidden = true;

            UltraGridColumn colNetNotionalValue = _gridBandHistoryPositions.Columns[CAPTION_NetNotionalValue];
            colNetNotionalValue.Width = 110;
            colNetNotionalValue.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            //colNetNotionalValue.ValueList = cmbSymbolConvention; BB
            colNetNotionalValue.Header.Caption = "Symbol Convention";
            //colNetNotionalValue.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
            //colNetNotionalValue.Header.VisiblePosition = 3;
            colNetNotionalValue.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colNetNotionalValue.Hidden = true;


            UltraGridColumn colCommission = _gridBandHistoryPositions.Columns[CAPTION_Commission];
            colCommission.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CurrencyNonNegative;
            colCommission.Width = 75;
            colCommission.Header.Caption = "Commission";
            //colCommission.Header.VisiblePosition = 14;
            colCommission.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colCommission.Hidden = true;

            UltraGridColumn colFees = _gridBandHistoryPositions.Columns[CAPTION_Fees];
            colFees.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CurrencyNonNegative;
            colFees.Width = 45;
            colFees.Header.Caption = "Fees";
            //colFees.Header.VisiblePosition = 15;
            colFees.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colFees.Hidden = true;

            UltraGridColumn colAUECID = _gridBandHistoryPositions.Columns[CAPTION_AUECID];
            //colAUECID.Width = 45;
            //colAUECID.Header.Caption = "Position State";
            //colAUECID.Header.VisiblePosition = 4;
            colAUECID.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            colAUECID.Hidden = true;

            //_gridBandHistoryPositions.Summaries.Add(SummaryType.Sum, _gridBandHistoryPositions.Columns[CAPTION_Quantity], SummaryPosition.UseSummaryPositionColumn);
            _gridBandHistoryPositions.Summaries.Add(SummaryType.Sum, _gridBandHistoryPositions.Columns[CAPTION_PNL], SummaryPosition.UseSummaryPositionColumn);
            //_gridBandHistoryPositions.Summaries.Add(SummaryType.Sum, _gridBandHistoryPositions.Columns[CAPTION_ParentPositionBalanceQuantity], SummaryPosition.UseSummaryPositionColumn);
            //_gridBandHistoryPositions.                

            //_gridBandHistoryPositions.Summaries.Band.Columns[CAPTION_Quantity].CellAppearance = Infragistics.Win.UltraWinGrid.app

        }

        /// <summary>
        /// Sets the grid appearance and layout.
        /// </summary>
        /// <param name="gridLayout">The grid layout.</param>
        private void SetHistoryPositionGridAppearanceAndLayout(ref UltraGridLayout gridLayout)
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
            gridLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            gridLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
            gridLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            gridLayout.Override.RowFilterMode = RowFilterMode.AllRowsInBand;

            gridLayout.Override.ColumnSizingArea = ColumnSizingArea.EntireColumn;
            gridLayout.Override.ColumnAutoSizeMode = ColumnAutoSizeMode.VisibleRows;

            //UltraGridLayout gridLayoutHeader = this.grdCreatePosition.DisplayLayout;
            UltraGridBand band = this.grdHistoryPositions.DisplayLayout.Bands[0];
            gridLayout.Override.HeaderAppearance.TextVAlign = VAlign.Middle;

            //band.Override.HeaderStyle = HeaderStyle.WindowsXPCommand;
            band.Override.HeaderAppearance.BackColor = Color.WhiteSmoke;
            //band.Override.HeaderAppearance.BackColor2 = Color.DarkGray;
            //band.Override.HeaderAppearance.BackGradientStyle = GradientStyle.Vertical;
            band.Override.HeaderAppearance.ForeColor = Color.Black;
            //band.Override.HeaderAppearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;

            grdHistoryPositions.DisplayLayout.Appearance.ForeColor = Color.White;


        }

        private void cmbReportType_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            //AddPositionDerivedList historyPositionList = PortfolioReportManager.GetHistoryPositions(startDate, endDate, 1, 1);
            //grdHistoryPositions.DataSource = historyPositionList;
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

            workBook = this.ultraGridExcelExporter1.Export(this.grdHistoryPositions, workBook.Worksheets[workbookName]);
            //Infragistics.Excel.BIFF8Writer.WriteWorkbookToFile(workBook, pathName);
            workBook.Save(pathName);
            result = true;
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

        private void BindComboes()
        {
            cmbReportType.DisplayMember = "DisplayText";
            cmbReportType.ValueMember = "Value";
            cmbReportType.DataSource = null;
            cmbReportType.DataSource = EnumHelper.ConvertEnumForBindingWithSelect(typeof(ReportType));
            Utils.UltraComboFilter(cmbReportType, "DisplayText");
            cmbReportType.Value = int.MinValue;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void grdHistoryPositions_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            string positionType = e.Row.Cells[CAPTION_PositionType].Text;

            if (!positionType.Equals(string.Empty))
            {
                if (positionType.Equals("Closing"))
                {
                    e.Row.Appearance.ForeColor = Color.FromArgb(255, 91, 71);
                }
                else
                {
                    //e.Row.Appearance.ForeColor = Color.FromArgb(25, 91, 255);
                    e.Row.Appearance.ForeColor = Color.FromArgb(25, 191, 255);
                }
            }
        }
    }
}