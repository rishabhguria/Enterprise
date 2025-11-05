using Infragistics.Win.UltraWinGrid;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using Prana.WashSale.Classes;
using Prana.WashSale.Controls;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Prana.WashSale
{
    public partial class WashSale : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public WashSale()
        {
            try
            {
                InitializeComponent();
                washSaleTradesFiltersUC.BindDataToWashSaleGrid += BindDataToWashSaleGrid;
                washSaleTradesFiltersUC.DisableGridData += DisableGridData;
                WashSaleTradesButtonUC.DisableGridData += DisableGridData;
                washSaleTradesFiltersUC.UpdateWashSaleGridData += UpdateWashSaleTradeGridData;
                WashSaleTradesButtonUC.BindUploadedDataToWashSaleGrid += BindDataToWashSaleGrid;
                washSaleTradesButtonUC.ExportWashSaleGrid += ExportWashSaleGridData;
                washSaleTradesButtonUC.UpdateWashSaleGridData += UpdateWashSaleTradeGridData;
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
        /// DisableGridData
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisableGridData(object sender, ClickCellEventArgs e)
        {
            try
            {
                washSaleTradesGridUC.DisableGrid(null, null);
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
        /// Loading Wash sale module
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WashSale_Load(object sender, EventArgs e)
        {
            try
            {
                if (CustomThemeHelper.ApplyTheme)
                {
                    SetStatusBarText(WashSaleConstants.CONST_BLANK);
                    CustomThemeHelper.SetThemeProperties(this.washSaleUltraPanel_Top, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_WATCHLIST);
                    washSaleTradesGridUC.SetThemeForUserControl();
                    this.washSaleUltraFormManager.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.washSaleUltraFormManager.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
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
        // <summary>
        /// BindWashSaleDataToGrid
        /// </summary>
        private void BindDataToWashSaleGrid(object sender, EventArgs<List<WashSaleTrades>> e)
        {
            try
            {
                washSaleTradesGridUC.BindDataToWashSaleGrid(e.Value);
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
        // <summary>
        /// ExportWashSaleGridData
        /// </summary>
        private void ExportWashSaleGridData(object sender, EventArgs e)
        {
            try
            {
                washSaleTradesGridUC.ExportToExcel();
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
        /// CLosing wash sale form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WashSale_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (WashSaleTradesGridUC.isSaveGridData)
                {
                    if (FormClosed != null)
                    {
                        CustomMessageBox customMessage = new CustomMessageBox(WashSaleConstants.CONST_WASHSALE_POPUP_MESSAGE_TITLE, WashSaleConstants.CONST_WASHSALE_POPUP_MESSAGE, true, CustomThemeHelper.PRODUCT_COMPANY_NAME, FormStartPosition.CenterScreen, MessageBoxButtons.YesNo);
                        DialogResult dialog = customMessage.ShowDialog();
                        if (dialog == DialogResult.Cancel)
                        {
                            WashSale.SetStatusBarText(WashSaleConstants.CONST_BLANK);
                            customMessage.Close();
                            e.Cancel = true;
                            return;

                        }
                        else if (dialog == DialogResult.Yes)
                        {
                            if (WashSaleTradesGridUC.IsGridContainsError())
                            {
                                WashSale.SetStatusBarText(WashSaleConstants.CONST_WASHSALE_GRID_ERROR_MESSAGE);
                                MessageBox.Show(WashSaleConstants.CONST_WASHSALE_GRID_ERROR_MESSAGEBOX, WashSaleConstants.CONST_WASHSALE_MESSAGEBOX_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                e.Cancel = true;
                                return;
                            }
                            WashSale.SetStatusBarText(WashSaleConstants.CONST_WASHSALE_SAVING_DATA);
                            UpdateWashSaleTradeGridData(e, null);
                            WashSaleDataManager.SaveWashSaleTaxlotData();
                            WashSale.SetStatusBarText(WashSaleConstants.CONST_WASHSALE_DATA_SAVED);
                            WashSaleTradesGridUC.isSaveGridData = false;
                            WashSaleTradesGridUC._gridHasError.Clear();
                            customMessage.Close();
                        }
                        else
                        {
                            WashSale.SetStatusBarText(WashSaleConstants.CONST_BLANK);
                            WashSaleTradesGridUC._gridHasError.Clear();
                            customMessage.Close();
                        }
                        WashSaleTradesGridUC.isSaveGridData = false;
                        WashSaleTradesFiltersUC.IsDataLoadedOnGrid = false;
                    }
                }
                if (WashSaleTradesButtonUC.CheckFormOpen())
                {
                    e.Cancel = true;
                    return;
                }
                WashSaleTradesFiltersUC.IsDataLoadedOnGrid = false;
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
        /// Watch sale form disposed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void WashSale_Disposed(object sender, System.EventArgs e)
        {
            try
            {
                if (FormClosed != null)
                {
                    FormClosed(this, EventArgs.Empty);
                }
                if (washSaleTradesFiltersUC != null)
                {
                    washSaleTradesFiltersUC.BindDataToWashSaleGrid -= BindDataToWashSaleGrid;
                    washSaleTradesFiltersUC.DisableGridData -= DisableGridData;
                }
                if (washSaleTradesFiltersUC != null)
                {
                    washSaleTradesFiltersUC = null;
                }
                if (washSaleTradesGridUC != null)
                {
                    washSaleTradesGridUC.ClearGridData();
                    washSaleTradesGridUC = null;
                }
                if (washSaleTradesButtonUC != null)
                {
                    WashSaleTradesButtonUC.BindUploadedDataToWashSaleGrid -= BindDataToWashSaleGrid;
                    WashSaleTradesButtonUC.DisableGridData -= DisableGridData;
                    washSaleTradesButtonUC = null;
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
        /// Update the Status bar of Washsale
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void WashSale_UpdateStatusBar(object sender, EventArgs<string> e)
        {
            try
            {
                SetStatusBarText(e.Value);
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
        /// Set the status bar of WashSale
        /// </summary>
        /// <param name="message"></param>
        public static void SetStatusBarText(string message)
        {
            try
            {
                toolStripStatusLabel.Text = "[" + DateTime.Now + "]" + message;
                statusStrip.Update();
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Update the Binding list of the Wash Sale Grid
        /// </summary>
        public void UpdateWashSaleTradeGridData(object sender, EventArgs e)
        {
            try
            {
                washSaleTradesGridUC.UpdateWashSaleGridData();
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

        public new event EventHandler FormClosed;
    }
}
