using Infragistics.Win.UltraWinGrid;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Prana.Import.Controls
{
    public partial class ctrlSymbolMismatch : UserControl
    {
        #region variables
        bool _isAllColumnsLoaded = false;
        string _mismatchType = string.Empty;
        #endregion

        public ctrlSymbolMismatch()
        {
            InitializeComponent();
        }
        /// <summary>
        /// determine the column visibility of grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdMismatch_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                if (_isAllColumnsLoaded)
                {
                    SetupGridHiddenColumns(e.Layout.Bands[0]);
                    grdMismatch.DisplayLayout.Rows[0].Activation = Activation.NoEdit;
                }
                else
                {
                    SetupGridColumns(e.Layout.Bands[0]);
                    grdMismatch.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
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
        /// <summary>
        /// Set the columns for Uploaded symbol as hidden if they are null or empty
        /// </summary>
        /// <param name="ultraGridBand"></param>
        private void SetupGridHiddenColumns(UltraGridBand ultraGridBand)
        {
            foreach (UltraGridColumn col in ultraGridBand.Columns)
            {
                if (col.Key.Contains("Uploaded"))
                {
                    if (string.IsNullOrEmpty(grdMismatch.Rows[0].Cells[col.Key].Value.ToString()))
                    {
                        col.Hidden = true;
                    }
                    else
                    {
                        col.Hidden = false;
                    }
                }
            }
        }

        /// <summary>
        /// Setup grid display columns
        /// </summary>
        /// <param name="ultraGridBand"></param>
        private void SetupGridColumns(UltraGridBand ultraGridBand)
        {
            try
            {
                int visiblePosition = 0;
                string[] arrColumns = new string[] { ApplicationConstants.SymbologyCodes.TickerSymbol.ToString(), ApplicationConstants.SymbologyCodes.BloombergSymbol.ToString(), ApplicationConstants.SymbologyCodes.ISINSymbol.ToString(), ApplicationConstants.SymbologyCodes.SEDOLSymbol.ToString(), ApplicationConstants.SymbologyCodes.CUSIPSymbol.ToString(), ApplicationConstants.SymbologyCodes.ReutersSymbol.ToString(), OrderFields.PROPERTY_LONGNAME, "Multiplier", OrderFields.PROPERTY_CURRENCYID };
                List<string> lstColumnsToDisplay = arrColumns.ToList();
                switch (_mismatchType)
                {
                    case "DuplicateSymbol":
                        lstColumnsToDisplay.Add("Symbol(UploadedSymbol)");
                        lstColumnsToDisplay.Add("Bloomberg(UploadedSymbol)");
                        lstColumnsToDisplay.Add("SEDOL(UploadedSymbol)");
                        lstColumnsToDisplay.Add("ISIN(UploadedSymbol)");
                        lstColumnsToDisplay.Add("RIC(UploadedSymbol)");
                        lstColumnsToDisplay.Add("CUSIP(UploadedSymbol)");
                        break;
                    case "Currency Mismatch":
                        lstColumnsToDisplay.Add("CurrencyID(UploadedSymbol)");
                        break;
                    case "Multiplier Mismatch":
                        lstColumnsToDisplay.Add("Multiplier(UploadedSymbol)");
                        break;
                }

                if (ultraGridBand.Columns.Exists(ApplicationConstants.SymbologyCodes.TickerSymbol.ToString()))
                {
                    UltraGridColumn ColTickerSymbol = ultraGridBand.Columns[ApplicationConstants.SymbologyCodes.TickerSymbol.ToString()];
                    ColTickerSymbol.Header.VisiblePosition = visiblePosition++;
                    ColTickerSymbol.Header.Column.Width = 100;
                    ColTickerSymbol.Header.Caption = OrderFields.CAPTION_TICKERSYMBOL;
                    ColTickerSymbol.CharacterCasing = CharacterCasing.Upper;
                    ColTickerSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                    ColTickerSymbol.NullText = String.Empty;
                    ColTickerSymbol.SortIndicator = SortIndicator.Ascending;
                }

                if (ultraGridBand.Columns.Exists(OrderFields.PROPERTY_LONGNAME))
                {
                    UltraGridColumn ColLongName = ultraGridBand.Columns[OrderFields.PROPERTY_LONGNAME];
                    ColLongName.Width = 100;
                    ColLongName.Header.VisiblePosition = visiblePosition++;
                    ColLongName.Header.Column.Width = 150;
                    ColLongName.Header.Caption = "Description";
                    ColLongName.CharacterCasing = CharacterCasing.Upper;
                    ColLongName.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                    ColLongName.NullText = String.Empty;
                }

                if (ultraGridBand.Columns.Exists(ApplicationConstants.SymbologyCodes.ReutersSymbol.ToString()))
                {
                    UltraGridColumn ColReutresSymbol = ultraGridBand.Columns[ApplicationConstants.SymbologyCodes.ReutersSymbol.ToString()];
                    ColReutresSymbol.Header.VisiblePosition = visiblePosition++;
                    ColReutresSymbol.Header.Column.Width = 70;
                    ColReutresSymbol.CharacterCasing = CharacterCasing.Upper;
                    ColReutresSymbol.Header.Caption = OrderFields.CAPTION_RICSYMBOL;
                    ColReutresSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                    ColReutresSymbol.NullText = String.Empty;
                }

                if (ultraGridBand.Columns.Exists(ApplicationConstants.SymbologyCodes.BloombergSymbol.ToString()))
                {
                    UltraGridColumn ColBloombergSymbol = ultraGridBand.Columns[ApplicationConstants.SymbologyCodes.BloombergSymbol.ToString()];
                    ColBloombergSymbol.Width = 70;
                    ColBloombergSymbol.Header.VisiblePosition = visiblePosition++;
                    ColBloombergSymbol.Header.Column.Width = 70;
                    ColBloombergSymbol.CharacterCasing = CharacterCasing.Upper;
                    ColBloombergSymbol.Header.Caption = OrderFields.CAPTION_BLOOMBERGSYMBOL;
                    ColBloombergSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                    ColBloombergSymbol.NullText = String.Empty;
                }

                if (ultraGridBand.Columns.Exists(ApplicationConstants.SymbologyCodes.CUSIPSymbol.ToString()))
                {
                    UltraGridColumn ColCusipSymbol = ultraGridBand.Columns[ApplicationConstants.SymbologyCodes.CUSIPSymbol.ToString()];
                    ColCusipSymbol.Width = 70;
                    ColCusipSymbol.Header.VisiblePosition = visiblePosition++;
                    ColCusipSymbol.Header.Column.Width = 70;
                    ColCusipSymbol.CharacterCasing = CharacterCasing.Upper;
                    ColCusipSymbol.Header.Caption = OrderFields.CAPTION_CUSIPSYMBOL;
                    ColCusipSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                    ColCusipSymbol.NullText = String.Empty;
                }

                if (ultraGridBand.Columns.Exists(ApplicationConstants.SymbologyCodes.ISINSymbol.ToString()))
                {
                    UltraGridColumn ColISINSymbol = ultraGridBand.Columns[ApplicationConstants.SymbologyCodes.ISINSymbol.ToString()];
                    ColISINSymbol.Header.VisiblePosition = visiblePosition++;
                    ColISINSymbol.Header.Column.Width = 70;
                    ColISINSymbol.CharacterCasing = CharacterCasing.Upper;
                    ColISINSymbol.Header.Caption = OrderFields.CAPTION_ISINSYMBOL;
                    ColISINSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                    ColISINSymbol.NullText = String.Empty;
                }

                if (ultraGridBand.Columns.Exists(ApplicationConstants.SymbologyCodes.SEDOLSymbol.ToString()))
                {
                    UltraGridColumn ColSEDOLSymbol = ultraGridBand.Columns[ApplicationConstants.SymbologyCodes.SEDOLSymbol.ToString()];
                    ColSEDOLSymbol.Header.VisiblePosition = visiblePosition++;
                    ColSEDOLSymbol.Header.Column.Width = 70;
                    ColSEDOLSymbol.CharacterCasing = CharacterCasing.Upper;
                    ColSEDOLSymbol.Header.Caption = OrderFields.CAPTION_SEDOLSYMBOL;
                    ColSEDOLSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                    ColSEDOLSymbol.NullText = String.Empty;
                }

                if (ultraGridBand.Columns.Exists(OrderFields.PROPERTY_CURRENCYID))
                {
                    UltraGridColumn ColCurrency = ultraGridBand.Columns[OrderFields.PROPERTY_CURRENCYID];
                    ColCurrency.Header.VisiblePosition = visiblePosition++;
                    ColCurrency.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    ColCurrency.Header.Caption = "Currency";
                    //we are making clone because there was error we cannot add same valuelist again
                    ColCurrency.ValueList = SecMasterHelper.getInstance().Currencies.Clone();
                    ColCurrency.Header.Column.Width = 70;
                }

                if (ultraGridBand.Columns.Exists("CurrencyID(UploadedSymbol)"))
                {
                    UltraGridColumn ColCurrencyUploaded = ultraGridBand.Columns["CurrencyID(UploadedSymbol)"];
                    ColCurrencyUploaded.Header.VisiblePosition = visiblePosition++;
                    ColCurrencyUploaded.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    ColCurrencyUploaded.Header.Caption = "Currency (Uploaded Symbol)";
                    //we are making clone because there was error we cannot add same valuelist again
                    ColCurrencyUploaded.ValueList = SecMasterHelper.getInstance().Currencies.Clone();
                    ColCurrencyUploaded.Header.Column.Width = 70;
                }

                foreach (UltraGridColumn column in ultraGridBand.Columns)
                {
                    if (!lstColumnsToDisplay.Contains(column.Key))
                    {
                        column.Hidden = true;
                    }
                    column.CellActivation = Activation.ActivateOnly;
                    column.PerformAutoResize(PerformAutoSizeType.AllRowsInBand, AutoResizeColumnWidthOptions.All);
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
        /// Initialze layout
        /// </summary>
        internal void InitializeData(string mismatchType, DataTable dt)
        {
            try
            {
                _mismatchType = mismatchType;
                grdMismatch.DataSource = dt;
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
        /// intialize the data on the grid
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="isallColumnsToBeLoaded"></param>
        internal void InitializeDataOnGrid(DataTable dt, bool isallColumnsToBeLoaded)
        {
            try
            {
                //to set the columns that are to be visible in intializeLayout
                _isAllColumnsLoaded = isallColumnsToBeLoaded;
                grdMismatch.DataSource = dt;
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
    }
}
