using Infragistics.Win.UltraWinGrid;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using Prana.WashSale.Classes;
using Prana.WashSale.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.WashSale.Controls
{
    public partial class WashSaleTradesButtonUC : UserControl
    {
        /// <summary>
        /// Bind upload data to WashSaleDataToGrid
        /// </summary>
        public static event EventHandler<EventArgs<List<WashSaleTrades>>> BindUploadedDataToWashSaleGrid;
        /// <summary>
        /// ExportWashSaleGrid
        /// </summary>
        public event EventHandler<EventArgs> ExportWashSaleGrid;
        /// <summary>
        /// Event to Update the Binding list of the Wash Sale Grid
        /// </summary>
        public event EventHandler UpdateWashSaleGridData;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <summary>
        /// _washSaleDataCache
        /// </summary>
        public static List<WashSaleTrades> _washSaleDataUploadCache = new List<WashSaleTrades>();
        public static Action<object, ClickCellEventArgs> DisableGridData { get; internal set; }

        public WashSaleTradesButtonUC()
        {
            InitializeComponent();
        }

        static WashSaleUploadDesign form = null;
        /// <summary>
        /// Load the Upload Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UploadDataFromExcel(object sender, System.EventArgs e)
        {
            try
            {
                if (form == null)
                {
                    if (WashSaleTradesFiltersUC.IsDataLoadedOnGrid && WashSaleTradesGridUC.isSaveGridData)
                    {
                        CustomMessageBox customMessage = new CustomMessageBox(WashSaleConstants.CONST_WASHSALE_POPUP_MESSAGE_TITLE, WashSaleConstants.CONST_WASHSALE_POPUP_MESSAGE, true, CustomThemeHelper.PRODUCT_COMPANY_NAME, FormStartPosition.CenterScreen, MessageBoxButtons.YesNo);
                        DialogResult dialog = customMessage.ShowDialog();
                        if (dialog == DialogResult.Cancel)
                        {
                            WashSale.SetStatusBarText(WashSaleConstants.CONST_BLANK);
                            customMessage.Close();
                            return;
                        }
                        else if (dialog == DialogResult.Yes)
                        {
                            if (WashSaleTradesGridUC.IsGridContainsError())
                            {
                                WashSale.SetStatusBarText(WashSaleConstants.CONST_WASHSALE_GRID_ERROR_MESSAGE);
                                MessageBox.Show(WashSaleConstants.CONST_WASHSALE_GRID_ERROR_MESSAGEBOX, WashSaleConstants.CONST_WASHSALE_MESSAGEBOX_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                customMessage.Close();
                                return;
                            }
                            WashSale.SetStatusBarText(WashSaleConstants.CONST_WASHSALE_SAVING_DATA);
                            if (UpdateWashSaleGridData != null)
                                UpdateWashSaleGridData(e, null);
                            WashSaleDataManager.SaveWashSaleTaxlotData();
                            WashSale.SetStatusBarText(WashSaleConstants.CONST_WASHSALE_DATA_SAVED);
                            WashSaleTradesGridUC.isSaveGridData = false;
                            WashSaleTradesGridUC._gridHasError.Clear();
                            customMessage.Close();
                        }
                        else
                        {
                            WashSale.SetStatusBarText(WashSaleConstants.CONST_BLANK);
                            WashSaleTradesGridUC.DiscardChanges();
                            WashSaleTradesGridUC._gridHasError.Clear();
                            customMessage.Close();
                        }
                    }
                    Parent.Parent.Enabled = false;
                    WashSaleTradesFiltersUC.IsGetDataOrUploadOrSaveClick = true;
                    DisableGridData(null, null);
                    WashSaleTradesGridUC._washSaleDataCache.AllowEdit = false;
                    form = new WashSaleUploadDesign();
                    form.Show();
                    form.FormClosing += new System.Windows.Forms.FormClosingEventHandler(UploadFormClosing);
                }
                else
                {
                    form.BringToFront();
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
        /// Close the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UploadFormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Parent.Parent.Enabled = true;
                WashSaleTradesGridUC._washSaleDataCache.AllowEdit = true;
                form = null;
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
        /// Call method to save data in DB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SaveWashSaleData(object sender, EventArgs e)
        {
            try
            {

                if (WashSaleTradesGridUC.IsGridContainsError())
                {
                    WashSale.SetStatusBarText(WashSaleConstants.CONST_WASHSALE_GRID_ERROR_MESSAGE);
                    MessageBox.Show(WashSaleConstants.CONST_WASHSALE_GRID_ERROR_MESSAGEBOX, WashSaleConstants.CONST_WASHSALE_MESSAGEBOX_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ChangeStatusOfSave(true);
                    return;
                }
                if (WashSaleTradesFiltersUC.IsDataLoadedOnGrid)
                {
                    ChangeStatusOfSave(false);
                    WashSale.SetStatusBarText(WashSaleConstants.CONST_WASHSALE_SAVING_DATA);
                    if (UpdateWashSaleGridData != null)
                        UpdateWashSaleGridData(e, null);
                    WashSaleDataManager.SaveWashSaleTaxlotData();
                    WashSale.SetStatusBarText(WashSaleConstants.CONST_WASHSALE_DATA_SAVED);
                    ChangeStatusOfSave(true);
                    WashSaleTradesGridUC.isSaveGridData = false;
                    WashSaleTradesGridUC._gridHasError.Clear();
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
        /// Call method to Export data 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnExportToExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (ExportWashSaleGrid != null)
                    ExportWashSaleGrid(this, new EventArgs());
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
        /// Check if the form is open
        /// </summary>
        /// <returns></returns>
        public static bool CheckFormOpen()
        {
            try
            {
                if (form != null)
                {
                    form.BringToFront();
                    return true;
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
            return false;
        }
        /// <summary>
        /// Bind the uploaded data to the washsalegrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void OkButtonHandler(object sender, EventArgs e)
        {
            try
            {
                if (BindUploadedDataToWashSaleGrid != null)
                {
                    BindUploadedDataToWashSaleGrid(e, new EventArgs<List<WashSaleTrades>>(_washSaleDataUploadCache));
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
        /// Call method to change state of save icon
        /// </summary>
        private void ChangeStatusOfSave(bool isWorkComplated)
        {
            try
            {

                if (isWorkComplated)
                {
                    SaveImage.BackColor = Color.Transparent;
                    SaveImage.Enabled = true;
                }
                else
                {
                    SaveImage.BackColor = Color.DarkGray;
                    SaveImage.Enabled = false;
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
    }
}
