using Prana.LogManager;
using Prana.Utilities.ImportExportUtilities;
using Prana.Utilities.UI;
using Prana.Utilities.UI.UIUtilities;
using Prana.WashSale.Classes;
using Prana.WashSale.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

namespace Prana.WashSale.Forms
{
    public partial class WashSaleUploadDesign : Form
    {
        public WashSaleUploadDesign()
        {
            InitializeComponent();
            CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_AUDIT_TRAIL);
            this.ultraFormUploadManager.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + "Upload File" + "</p>";
            this.ultraFormUploadManager.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
        }

        public static Dictionary<int, bool> _gridColumnnError = new Dictionary<int, bool>();
        /// <summary>
        /// List to store the unique Taxlot id's
        /// </summary>
        Dictionary<string, bool> _taxlotId = new Dictionary<string, bool>();
        /// <summary>
        /// Open File Dialog Box to Select the excel File for washsale grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BrowseButtonClick(object sender, EventArgs e)
        {
            try
            {
                string fileNameTobeImported = string.Empty;
                browsePath.Text = WashSaleConstants.CONST_BLANK;
                Boolean CheckAccessPermission = true;
                OpenFileDialogHelper.isComingFromWashSale = true;
                fileNameTobeImported = OpenFileDialogHelper.GetFileNameUsingOpenFileDialog(CheckAccessPermission);
                OpenFileDialogHelper.isComingFromWashSale = false;
                if (!string.IsNullOrWhiteSpace(fileNameTobeImported))
                {
                    browsePath.Text = fileNameTobeImported;
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
        /// Check for Validation of the uploaded File
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OkButtonClick(object sender, EventArgs e)
        {
            try
            {

                if (browsePath.Text != string.Empty)
                {
                    string filePath = Path.GetFileName(browsePath.Text);
                    string checkOnString = filePath.Split('.')[1];

                    if (checkOnString.Equals("csv") || checkOnString.Equals("xls") || checkOnString.Equals("xlsx"))
                    {
                        WashSale.SetStatusBarText(WashSaleConstants.CONST_DATA_UPLOADING);
                        bool temp = false;
                        bool isValid = true;
                        DataTable data = new DataTable();
                        data = FileReaderFactory.GetDataTableFromDifferentFileFormatsNew(browsePath.Text);
                        if (data == null)
                        {
                            MessageBox.Show(WashSaleConstants.CONST_FILE_USED_BY_ANOTHER_PROECESS, WashSaleConstants.CONST_WASHSALE_UPLOAD_POPUPMESSAGE_TITLE, MessageBoxButton.OK, MessageBoxImage.Warning);
                            WashSale.SetStatusBarText(WashSaleConstants.CONST_BLANK);
                            return;
                        }
                        else
                        {
                            WashSaleTradesGridUC._gridHasError.Clear();
                            int rowCount = data.Rows.Count;
                            if (rowCount > 1)
                            {
                                if (data.Rows[rowCount - 2][0].Equals(WashSaleConstants.CONST_GRAND_SUMMARY))
                                    rowCount -= 2;
                            }
                            for (int i = 0; i < rowCount; i++)
                            {
                                if (temp)
                                {
                                    DateTime purchaseDate, startDate, tradeDate;
                                    double quantity, unitCostLocal, totalCostLocal, totalCost;
                                    double holdingPeriod;
                                    decimal costBasis, realizedLoss;
                                    WashSaleTrades washSaleObject = new WashSaleTrades();
                                    if (_taxlotId.ContainsKey(data.Rows[i][0].ToString()) || data.Rows[i][0].ToString().Equals(WashSaleConstants.CONST_BLANK) || data.Rows[i][0].ToString().Any(c => char.IsLetter(c)) ||
                                        data.Rows[i][1].ToString() == null || data.Rows[i][1].ToString().Equals(WashSaleConstants.CONST_BLANK) || data.Rows[i][1].ToString().Any(c => char.IsNumber(c)) ||
                                        data.Rows[i][2].ToString() == null || !DateTime.TryParse(data.Rows[i][2].ToString(), out tradeDate) || data.Rows[i][2].ToString().Equals(WashSaleConstants.CONST_BLANK) ||
                                        data.Rows[i][3].ToString() == null || !DateTime.TryParse(data.Rows[i][3].ToString(), out purchaseDate) || data.Rows[i][3].ToString().Equals(WashSaleConstants.CONST_BLANK) ||
                                        data.Rows[i][4].ToString() == null || data.Rows[i][4].ToString().Equals(WashSaleConstants.CONST_BLANK) ||
                                        data.Rows[i][5].ToString() == null || data.Rows[i][5].ToString().Equals(WashSaleConstants.CONST_BLANK) || data.Rows[i][5].ToString().Any(c => char.IsNumber(c)) ||
                                        data.Rows[i][6].ToString() == null || data.Rows[i][6].ToString().Equals(WashSaleConstants.CONST_BLANK) || data.Rows[i][6].ToString().Any(c => char.IsNumber(c)) ||
                                        data.Rows[i][7].ToString() == null || data.Rows[i][7].ToString().Equals(WashSaleConstants.CONST_BLANK) || data.Rows[i][7].ToString().Any(c => char.IsNumber(c)) ||
                                        data.Rows[i][8].ToString() == null || data.Rows[i][8].ToString().Equals(WashSaleConstants.CONST_BLANK) || data.Rows[i][8].ToString().Any(c => char.IsNumber(c)) ||
                                        data.Rows[i][9].ToString() == null || data.Rows[i][9].ToString().Equals(WashSaleConstants.CONST_BLANK) ||
                                        data.Rows[i][10].ToString() == null ||
                                        data.Rows[i][11].ToString() == null ||
                                        data.Rows[i][12].ToString() == null ||
                                        data.Rows[i][13].ToString() == null ||
                                        data.Rows[i][14].ToString() == null || data.Rows[i][14].ToString().Equals(WashSaleConstants.CONST_BLANK) || !double.TryParse(data.Rows[i][14].ToString(), out quantity) ||
                                        data.Rows[i][15].ToString() == null || data.Rows[i][15].ToString().Equals(WashSaleConstants.CONST_BLANK) || !double.TryParse(data.Rows[i][15].ToString(), out unitCostLocal) ||
                                        data.Rows[i][16].ToString() == null || data.Rows[i][16].ToString().Equals(WashSaleConstants.CONST_BLANK) || !double.TryParse(data.Rows[i][16].ToString(), out totalCostLocal) ||
                                        data.Rows[i][17].ToString() == null || data.Rows[i][17].ToString().Equals(WashSaleConstants.CONST_BLANK) || !double.TryParse(data.Rows[i][17].ToString(), out totalCost)
                                        )
                                    {
                                        isValid = false;
                                        break;
                                    }
                                    _taxlotId[data.Rows[i][0].ToString()] = true;
                                    washSaleObject.TaxlotID = data.Rows[i][0].ToString();
                                    washSaleObject.TypeOfTransaction = data.Rows[i][1].ToString();
                                    washSaleObject.TradeDate = tradeDate;
                                    washSaleObject.OriginalPurchaseDate = purchaseDate;
                                    washSaleObject.Account = data.Rows[i][4].ToString();
                                    washSaleObject.Side = data.Rows[i][5].ToString();
                                    washSaleObject.Asset = data.Rows[i][6].ToString();
                                    washSaleObject.Currency = data.Rows[i][7].ToString();
                                    washSaleObject.Broker = data.Rows[i][8].ToString();
                                    washSaleObject.Symbol = data.Rows[i][9].ToString();
                                    washSaleObject.BloombergSymbol = data.Rows[i][10].ToString();
                                    washSaleObject.CUSIP = data.Rows[i][11].ToString();
                                    washSaleObject.Issuer = data.Rows[i][12].ToString();
                                    washSaleObject.UnderlyingSymbol = data.Rows[i][13].ToString();
                                    washSaleObject.Quantity = quantity;
                                    washSaleObject.UnitCostLocal = unitCostLocal;
                                    washSaleObject.TotalCostLocal = totalCostLocal;
                                    washSaleObject.TotalCost = totalCost;

                                    string realizedLossStr = string.Empty;
                                    decimal realizedLossDec = decimal.MinValue;
                                    decimal realizedLossDecOut = decimal.MinValue;
                                    if (data.Rows[i][18] != DBNull.Value)
                                    {
                                        realizedLossStr = data.Rows[i][18].ToString();
                                        if (!realizedLossStr.Equals(WashSaleConstants.CONST_BLANK))
                                        {
                                            if (decimal.TryParse(realizedLossStr, NumberStyles.Any, CultureInfo.InvariantCulture, out realizedLossDecOut))
                                                realizedLossDec = realizedLossDecOut;
                                        }
                                    }

                                    if (decimal.TryParse(realizedLossStr, out realizedLoss))
                                    {
                                        washSaleObject.WashSaleAdjustedRealizedLoss = realizedLoss;
                                    }
                                    else if (realizedLossDec != decimal.MinValue)
                                    {
                                        realizedLoss = realizedLossDec;
                                        washSaleObject.WashSaleAdjustedRealizedLoss = realizedLoss;
                                    }
                                    else if (realizedLossStr.Equals(WashSaleConstants.CONST_BLANK))
                                        washSaleObject.WashSaleAdjustedRealizedLoss = null;
                                    else { isValid = false; break; }

                                    string holdingPeriodStr = string.Empty;
                                    if (data.Rows[i][19] != DBNull.Value)
                                        holdingPeriodStr = data.Rows[i][19].ToString();

                                    if (double.TryParse(holdingPeriodStr.ToString(), out holdingPeriod))
                                    {
                                        int testHoldingPeriod;
                                        if (int.TryParse(holdingPeriodStr.ToString(), out testHoldingPeriod))
                                        {
                                            if (testHoldingPeriod > 10000)
                                            {
                                                _gridColumnnError[i] = true;
                                                WashSaleTradesGridUC._gridHasError[WashSaleConstants.CONST_WASH_SALE_ADJUSTED_HOLDINGS_PERIOD + (i - 1).ToString()] = true;
                                            }
                                            washSaleObject.WashSaleAdjustedHoldingsPeriod = Convert.ToInt32(testHoldingPeriod);
                                        }
                                    }
                                    else if (holdingPeriodStr.Equals(WashSaleConstants.CONST_BLANK))
                                        washSaleObject.WashSaleAdjustedHoldingsPeriod = null;
                                    else { isValid = false; break; }

                                    string costBasisStr = string.Empty;
                                    decimal costBasicDec = decimal.MinValue;
                                    decimal costBasicDecOut = decimal.MinValue;
                                    if (data.Rows[i][20] != DBNull.Value)
                                    {
                                        costBasisStr = data.Rows[i][20].ToString();
                                        if (!costBasisStr.Equals(WashSaleConstants.CONST_BLANK))
                                        {
                                            if (decimal.TryParse(costBasisStr, NumberStyles.Any, CultureInfo.InvariantCulture, out costBasicDecOut))
                                                costBasicDec = costBasicDecOut;
                                        }
                                    }
                                    if (decimal.TryParse(costBasisStr, out costBasis))
                                        washSaleObject.WashSaleAdjustedCostBasis = costBasis;
                                    else if (costBasicDec != decimal.MinValue)
                                        costBasis = costBasicDec;
                                    else if ((realizedLossStr.Equals(WashSaleConstants.CONST_BLANK) && costBasisStr.Equals(WashSaleConstants.CONST_BLANK)))
                                        washSaleObject.WashSaleAdjustedCostBasis = null;
                                    else if (washSaleObject.WashSaleAdjustedRealizedLoss != null && costBasisStr.Equals(WashSaleConstants.CONST_BLANK))
                                    {
                                        washSaleObject.WashSaleAdjustedCostBasis = realizedLoss + (decimal)totalCost;
                                    }
                                    else { isValid = false; break; }
                                    if (DateTime.TryParse(data.Rows[i][21].ToString(), out startDate))
                                        washSaleObject.WashSaleAdjustedHoldingsStartDate = DateTime.Parse(data.Rows[i][21].ToString());
                                    else if (data.Rows[i][19].ToString().Equals(WashSaleConstants.CONST_BLANK) && data.Rows[i][21].ToString().Equals(WashSaleConstants.CONST_BLANK))
                                        washSaleObject.WashSaleAdjustedHoldingsStartDate = null;
                                    else if (washSaleObject.WashSaleAdjustedHoldingsPeriod != null && (data.Rows[i][21].ToString().Equals(WashSaleConstants.CONST_BLANK) || data.Rows[i][21].ToString().Equals(DateTime.MinValue)))
                                    {
                                        if (holdingPeriod > 10000)
                                            washSaleObject.WashSaleAdjustedHoldingsStartDate = null;
                                        else washSaleObject.WashSaleAdjustedHoldingsStartDate = Convert.ToDateTime(purchaseDate.AddDays(-holdingPeriod));
                                    }
                                    else { isValid = false; break; }
                                    WashSaleTradesButtonUC._washSaleDataUploadCache.Add(washSaleObject);
                                }
                                else
                                {
                                    if (!data.Rows[i][0].ToString().Equals(WashSaleConstants.CAPS_ID) ||
                                        !data.Rows[i][1].ToString().Equals(WashSaleConstants.CAPS_TYPE_OF_TRANSACTION)
                                       ||
                                        !data.Rows[i][2].ToString().Equals(WashSaleConstants.CAPS_TRADE_DATE)
                                        ||
                                        !data.Rows[i][3].ToString().Equals(WashSaleConstants.CAPS_ORIGINAL_PURCHASE_DATE)
                                        ||
                                        !data.Rows[i][4].ToString().Equals(WashSaleConstants.CONST_ACCOUNT)
                                        ||
                                        !data.Rows[i][5].ToString().Equals(WashSaleConstants.CONST_SIDE)
                                        ||
                                        !data.Rows[i][6].ToString().Equals(WashSaleConstants.CONST_ASSET)
                                        ||
                                        !data.Rows[i][7].ToString().Equals(WashSaleConstants.CONST_CURRENCY)
                                        ||
                                        !data.Rows[i][8].ToString().Equals(WashSaleConstants.CONST_BROKER)
                                        ||
                                        !data.Rows[i][9].ToString().Equals(WashSaleConstants.CONST_SYMBOL)
                                        ||
                                        !data.Rows[i][10].ToString().Equals(WashSaleConstants.CAPS_BLOOMBERG_SYMBOL)
                                        ||
                                        !data.Rows[i][11].ToString().Equals(WashSaleConstants.CONST_CUSIP)
                                        ||
                                        !data.Rows[i][12].ToString().Equals(WashSaleConstants.CONST_ISSUER)
                                        ||
                                        !data.Rows[i][13].ToString().Equals(WashSaleConstants.CAPS_UNDERLYING_SYMBOL)
                                        ||
                                        !data.Rows[i][14].ToString().Equals(WashSaleConstants.CONST_QUANTITY)
                                        ||
                                        !data.Rows[i][15].ToString().Equals(WashSaleConstants.CAPS_UNIT_COST_LOCAL)
                                        ||
                                        !data.Rows[i][16].ToString().Equals(WashSaleConstants.CAPS_TOTAL_COST_LOCAL)
                                        ||
                                        !data.Rows[i][17].ToString().Equals(WashSaleConstants.CAPS_TOTAL_COST)
                                        ||
                                        !data.Rows[i][18].ToString().Equals(WashSaleConstants.CAPS_WASH_SALE_ADJUSTED_REALIZED_LOSS)
                                        ||
                                        !data.Rows[i][19].ToString().Equals(WashSaleConstants.CAPS_WASH_SALE_ADJUSTED_HOLDINGS_PERIOD)
                                        ||
                                        !data.Rows[i][20].ToString().Equals(WashSaleConstants.CAPS_WASH_SALE_ADJUSTED_COST_BASIS)
                                        ||
                                        !data.Rows[i][21].ToString().Equals(WashSaleConstants.CAPS_WASH_SALE_ADJUSTED_HOLDINGS_START_DATE)
                                        )
                                    {
                                        isValid = false;
                                        break;
                                    }
                                    temp = true;
                                }
                            }
                        }
                        if (isValid)
                        {
                            WashSaleTradesButtonUC.OkButtonHandler(sender, e);
                            _gridColumnnError.Clear();
                            WashSaleTradesFiltersUC.IsDataLoadedOnGrid = true;
                            WashSaleTradesButtonUC._washSaleDataUploadCache.Clear();
                            WashSale.SetStatusBarText(WashSaleConstants.CONST_DATA_UPLOADED);
                            WashSaleTradesGridUC._washSaleDataCache.AllowEdit = true;
                            isValid = true;
                            this.Close();
                            _taxlotId.Clear();
                        }
                        else
                        {
                            MessageBox.Show(WashSaleConstants.CONST_WASHSALE_UPLOAD_POPUPMESSAGE, WashSaleConstants.CONST_WASHSALE_UPLOAD_POPUPMESSAGE_TITLE, MessageBoxButton.OK, MessageBoxImage.Warning);
                            isValid = true;
                            _gridColumnnError.Clear();
                            WashSale.SetStatusBarText(WashSaleConstants.CONST_BLANK);
                            WashSaleTradesButtonUC._washSaleDataUploadCache.Clear();
                            _taxlotId.Clear();
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show(WashSaleConstants.CONST_WASHSALE_UPLOAD_POPUPMESSAGE, WashSaleConstants.CONST_WASHSALE_UPLOAD_POPUPMESSAGE_TITLE, MessageBoxButton.OK, MessageBoxImage.Warning);
                        WashSale.SetStatusBarText(WashSaleConstants.CONST_BLANK);
                        _gridColumnnError.Clear();
                    }
                }
                else
                {
                    MessageBox.Show(WashSaleConstants.CONST_WASHSALE_UPLOAD_POPUPMESSAGE, WashSaleConstants.CONST_WASHSALE_UPLOAD_POPUPMESSAGE_TITLE, MessageBoxButton.OK, MessageBoxImage.Warning);
                    WashSale.SetStatusBarText(WashSaleConstants.CONST_BLANK);
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show(WashSaleConstants.CONST_WASHSALE_UPLOAD_POPUPMESSAGE, WashSaleConstants.CONST_WASHSALE_UPLOAD_POPUPMESSAGE_TITLE, MessageBoxButton.OK, MessageBoxImage.Warning);
                WashSale.SetStatusBarText(WashSaleConstants.CONST_BLANK);
                return;
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
        /// Close the Pop-up Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButtonClick(object sender, EventArgs e)
        {
            try
            {
                WashSaleTradesFiltersUC.IsGetDataOrUploadOrSaveClick = false;
                WashSaleTradesButtonUC.DisableGridData(null, null);
                WashSaleTradesGridUC._washSaleDataCache.AllowEdit = true;
                this.FindForm().Close();
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

