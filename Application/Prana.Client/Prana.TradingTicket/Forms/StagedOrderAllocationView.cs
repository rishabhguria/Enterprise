using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.CommonDataCache;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.TradingTicket.Forms
{
    public partial class StagedOrderAllocationView : Form
    {
        /// <summary>
        /// The column account
        /// </summary>
        private const string COLUMN_ACCOUNT = "Account";

        /// <summary>
        /// The column allocated quantity
        /// </summary>
        private const string COLUMN_ALLOCATED_QUANTITY = "Allocated Quantity";

        /// <summary>
        /// The column allocation %
        /// </summary>
        private const string COLUMN_ALLOCATION_PERCENTAGE = "Allocation %";

        /// <summary>
        /// Represents a DataTable for storing grid data.
        /// </summary>
        private DataTable dt;
       
        /// <summary>
        /// Gets or sets the allocation operation preference.
        /// </summary>
        public Dictionary<int, AccountValue> AllocationDetails { get; set; }

        public bool IsComingFromBlotter { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="StagedOrderAllocationView"/> class.
        /// </summary>
        public StagedOrderAllocationView()
        {
            try
            {
                InitializeComponent();
                IntitializeGrid();
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
        /// Set the theme and appearance of form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StagedOrderAllocationView_Load(object sender, EventArgs e)
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_PRANA_TRADING_TICKET);
                this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, IsComingFromBlotter ? "Allocation Details" : "Custom Allocation", CustomThemeHelper.UsedFont);
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.BackColor = Color.FromArgb(90, 89, 90);
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
        /// Initializes the grdStagedOrder grid.
        /// </summary>
        private void IntitializeGrid()
        {
            try
            {
                dt = new DataTable();
                dt.Columns.Add(COLUMN_ACCOUNT);
                dt.Columns.Add(COLUMN_ALLOCATED_QUANTITY, typeof(double));
                dt.Columns.Add(COLUMN_ALLOCATION_PERCENTAGE, typeof(double));
                grdStagedOrder.DataSource = dt;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Loads data into the staged order grid.
        /// </summary>
        /// <param name="totalQty">The total quantity</param>
        public void LoadAllocationData(decimal totalQty)
        {
            try
            {
                dt.Rows.Clear();
                foreach (KeyValuePair<int, AccountValue> kvp in AllocationDetails)
                {
                    string accountName = CachedDataManager.GetInstance.GetAccount(kvp.Key);
                    decimal allocationPercent = kvp.Value.Value;
                    decimal allocatedPosition = (totalQty * allocationPercent) / 100;
                    dt.Rows.Add(accountName, allocatedPosition, allocationPercent);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Initilaize the grdStagedOrder with required properties.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdStagedOrder_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                foreach (UltraGridColumn column in grdStagedOrder.DisplayLayout.Bands[0].Columns)
                {
                    column.CellActivation = Activation.NoEdit;
                    column.Width = 50;                   
                }

                grdStagedOrder.DisplayLayout.Bands[0].Columns[COLUMN_ACCOUNT].CellAppearance.TextHAlign = HAlign.Left;
                grdStagedOrder.DisplayLayout.Bands[0].Columns[COLUMN_ALLOCATED_QUANTITY].CellAppearance.TextHAlign = HAlign.Right;
                grdStagedOrder.DisplayLayout.Bands[0].Columns[COLUMN_ALLOCATED_QUANTITY].Format = "N2";
                grdStagedOrder.DisplayLayout.Bands[0].Columns[COLUMN_ALLOCATION_PERCENTAGE].CellAppearance.TextHAlign = HAlign.Right;
                grdStagedOrder.DisplayLayout.Bands[0].Columns[COLUMN_ALLOCATION_PERCENTAGE].Format = "N2";

                grdStagedOrder.DisplayLayout.Bands[0].Summaries.Add(COLUMN_ALLOCATED_QUANTITY, SummaryType.Sum, grdStagedOrder.DisplayLayout.Bands[0].Columns[COLUMN_ALLOCATED_QUANTITY]);
                grdStagedOrder.DisplayLayout.Bands[0].Summaries[COLUMN_ALLOCATED_QUANTITY].DisplayFormat = "{0:#,0}";
                grdStagedOrder.DisplayLayout.Bands[0].Summaries[COLUMN_ALLOCATED_QUANTITY].Appearance.TextHAlign = HAlign.Right;
                grdStagedOrder.DisplayLayout.Bands[0].Summaries.Add(COLUMN_ALLOCATION_PERCENTAGE, SummaryType.Sum, grdStagedOrder.DisplayLayout.Bands[0].Columns[COLUMN_ALLOCATION_PERCENTAGE]);
                grdStagedOrder.DisplayLayout.Bands[0].Summaries[COLUMN_ALLOCATION_PERCENTAGE].DisplayFormat = "{0:N0}";
                grdStagedOrder.DisplayLayout.Bands[0].Summaries[COLUMN_ALLOCATION_PERCENTAGE].Appearance.TextHAlign = HAlign.Right;
                grdStagedOrder.DisplayLayout.Bands[0].Override.SummaryFooterCaptionVisible = Infragistics.Win.DefaultableBoolean.False;
                grdStagedOrder.DisplayLayout.Override.CellPadding = 2;
            }
            catch (Exception ex)
            {
                // Handle the exception
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
