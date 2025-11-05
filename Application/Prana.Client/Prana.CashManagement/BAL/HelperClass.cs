using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Prana.CashManagement
{
    class CreateFilter : IUIElementCreationFilter
    {
        #region Methods

        /// <summary>
        /// Called after an element's ChildElements have been
        /// created. The child element's can be repositioned here
        /// and/or new element's can be added.
        /// </summary>
        /// <param name="parent">The <see cref="T:Infragistics.Win.UIElement" /> whose child elements have been created/positioned.</param>
        public void AfterCreateChildElements(UIElement parent)
        {
            if (parent is GroupByRowUIElement)
            {
                // remove the group-by row description
                UIElement child = parent.GetDescendant(typeof(GroupByRowDescriptionUIElement));
                if (child != null)
                {
                    parent.ChildElements.Remove(child);
                }
                // resize the summary so it takes the full height
                child = parent.GetDescendant(typeof(SummaryFooterUIElement));
                if (child != null)
                {
                    Rectangle rect = child.Rect;
                    rect.Y = parent.Rect.Top;
                    rect.Height = parent.Rect.Height;
                    child.Rect = rect;
                }
                parent.DirtyChildElements();
            }
        }

        /// <summary>
        /// Called before child elements are to be created/positioned.
        /// This is called during a draw operation for an element
        /// whose ChildElementsDirty is set to true. Returning true from
        /// this method indicates that the default creation logic
        /// should be bypassed.
        /// </summary>
        /// <param name="parent">The <see cref="T:Infragistics.Win.UIElement" /> whose child elements are going to be created/positioned.</param>
        /// <returns>
        /// True if the default creation logic should be bypassed.
        /// </returns>
        public bool BeforeCreateChildElements(UIElement parent)
        {
            return false;
        }

        #endregion Methods
    }

    class HelperClass
    {
        #region Members

        /// <summary>
        /// The first level back color
        /// </summary>
        static Color _firstLevelBackColor;

        /// <summary>
        /// The second level back color
        /// </summary>
        static Color _secondLevelBackColor;

        /// <summary>
        /// The third level back color
        /// </summary>
        static Color _thirdLevelBackColor;

        /// <summary>
        /// The first level fore color
        /// </summary>
        static Color _firstLevelForeColor;

        /// <summary>
        /// The second level fore color
        /// </summary>
        static Color _secondLevelForeColor;

        /// <summary>
        /// The third level fore color
        /// </summary>
        static Color _thirdLevelForeColor;

        /// <summary>
        /// The is group by row color geted
        /// </summary>
        static bool _isGroupByRowColorGeted = false;

        #endregion Members

        #region Methods

        /// <summary>
        /// Accountses the summary settings.
        /// </summary>
        /// <param name="e">The <see cref="InitializeLayoutEventArgs"/> instance containing the event data.</param>
        public static void AccountsSummarySettings(InitializeLayoutEventArgs e)
        {
            try
            {

                UltraGridBand band = null;

                band = e.Layout.Bands[0];
                if (band.Summaries.Count == 0)
                {
                    e.Layout.Override.RowSelectors = DefaultableBoolean.True;
                    e.Layout.Override.RowSelectorHeaderStyle = RowSelectorHeaderStyle.ColumnChooserButton;
                    e.Layout.ColumnChooserEnabled = DefaultableBoolean.True;


                    SummarySettingsCollection accountSummaries = band.Summaries;
                    ColumnsCollection currentCollumnCollection = band.Columns;
                    CashSummaryFactory summFactory = new CashSummaryFactory();

                    //while Grouping grouped column must be shown in grid too.
                    //e.Layout.Override.GroupByColumnsHidden = DefaultableBoolean.True;
                    e.Layout.Override.GroupBySummaryDisplayStyle = GroupBySummaryDisplayStyle.SummaryCells;
                    e.Layout.Override.SummaryDisplayArea = SummaryDisplayAreas.Bottom;

                    //Added by: Bharat Raturi
                    //Change the summary row appearance
                    //http://jira.nirvanasolutions.com:8080/browse/PRANA-5423
                    if (!CustomThemeHelper.ApplyTheme)
                    {
                        Infragistics.Win.Appearance appSummaryRow = new Infragistics.Win.Appearance();
                        appSummaryRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(76)))), ((int)(((byte)(122)))));
                        appSummaryRow.ForeColor = System.Drawing.Color.White;
                        appSummaryRow.TextHAlignAsString = "Right";
                        appSummaryRow.TextVAlignAsString = "Middle";
                        e.Layout.Override.SummaryValueAppearance = appSummaryRow;

                        Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
                        appearance5.TextVAlign = VAlign.Middle;
                        appearance5.ForeColor = Color.White;
                        appearance5.BorderColor = System.Drawing.Color.Transparent;
                        e.Layout.Override.GroupBySummaryValueAppearance = appearance5;
                        e.Layout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
                    }

                    e.Layout.Override.GroupByRowExpansionStyle = GroupByRowExpansionStyle.ExpansionIndicatorAndDoubleClick;

                    e.Layout.UseFixedHeaders = true;

                    band.Override.HeaderPlacement = HeaderPlacement.FixedOnTop;
                    e.Layout.Override.SummaryFooterCaptionVisible = Infragistics.Win.DefaultableBoolean.False;

                    #region code from Consolidation View

                    e.Layout.Override.GroupByRowDescriptionMask = " ";
                    e.Layout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
                    e.Layout.GroupByBox.ButtonConnectorColor = System.Drawing.Color.Black;
                    e.Layout.GroupByBox.ButtonConnectorStyle = Infragistics.Win.UIElementBorderStyle.Solid;
                    e.Layout.GroupByBox.ShowBandLabels = ShowBandLabels.None;

                    Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
                    appearance4.BackColor = System.Drawing.Color.Black;
                    appearance4.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
                    appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
                    appearance4.BorderColor = System.Drawing.Color.Black;
                    //appearance4.FontData.BoldAsString = "True";
                    appearance4.ForeColor = System.Drawing.Color.White;
                    e.Layout.Override.GroupByRowAppearance = appearance4;

                    e.Layout.Override.GroupByRowPadding = 0;
                    e.Layout.Override.GroupByRowSpacingAfter = 0;
                    e.Layout.Override.GroupByRowSpacingBefore = 0;

                    e.Layout.RowConnectorColor = System.Drawing.Color.Transparent;

                    #endregion



                    SummarySettings transactionDatesummary = accountSummaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcDate"), currentCollumnCollection["TransactionDate"], SummaryPosition.UseSummaryPositionColumn, null);
                    transactionDatesummary.DisplayFormat = "{0:MM/dd/yyyy}";

                    SummarySettings OpenBalDatesummary = accountSummaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcDate"), currentCollumnCollection["OpenBalDate"], SummaryPosition.UseSummaryPositionColumn, null);
                    OpenBalDatesummary.DisplayFormat = "{0:MM/dd/yyyy}";

                    SummarySettings accountName = accountSummaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), currentCollumnCollection["FundName"], SummaryPosition.UseSummaryPositionColumn, null);
                    accountName.DisplayFormat = "{0}";

                    //SummarySettings symbol = summaries.Add(SummaryType.Custom, new customSymbolSummary(), currentCollumnCollection[CashManagementConstants.COLUMN_SYMBOL], SummaryPosition.UseSummaryPositionColumn, null);
                    //symbol.DisplayFormat = "{0}";

                    SummarySettings currencynamesummary = accountSummaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), currentCollumnCollection["CurrencySymbol"], SummaryPosition.UseSummaryPositionColumn, null);
                    currencynamesummary.DisplayFormat = "{0}";

                    SummarySettings basecurrencynamesummary = accountSummaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), currentCollumnCollection["BaseCurrency"], SummaryPosition.UseSummaryPositionColumn, null);
                    basecurrencynamesummary.DisplayFormat = "{0}";

                    SummarySettings subAcName = accountSummaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), currentCollumnCollection["SubAccName"], SummaryPosition.UseSummaryPositionColumn, null);
                    subAcName.DisplayFormat = "{0}";

                    SummarySettings masterCategory = accountSummaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), currentCollumnCollection["TransactionType"], SummaryPosition.UseSummaryPositionColumn, null);
                    masterCategory.DisplayFormat = "{0}";

                    SummarySettings acType = accountSummaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), currentCollumnCollection["MasterCategoryName"], SummaryPosition.UseSummaryPositionColumn, null);
                    acType.DisplayFormat = "{0}";


                    SummarySettings OpenDRBalSumSummary = accountSummaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), currentCollumnCollection["OpenDrBal"], SummaryPosition.UseSummaryPositionColumn, null);//, CurrentCollumnCollection[CashManagementConstants.COLUMN_CURRENCYNAME]);
                    OpenDRBalSumSummary.DisplayFormat = "{0}";
                    //DRSumSummary.DisplayFormat = "{0:#,###0}";

                    SummarySettings OpenCRBalSumSummary = accountSummaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), currentCollumnCollection["OpenCrBal"], SummaryPosition.UseSummaryPositionColumn, null);//, CurrentCollumnCollection[CashManagementConstants.COLUMN_CURRENCYNAME]);
                    OpenCRBalSumSummary.DisplayFormat = "{0}";
                    //DRSumSummary.DisplayFormat = "{0:#,###0}";

                    SummarySettings DayDrSumSummary = accountSummaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), currentCollumnCollection["DayDr"], SummaryPosition.UseSummaryPositionColumn, null);//, CurrentCollumnCollection[CashManagementConstants.COLUMN_CURRENCYNAME]);
                    DayDrSumSummary.DisplayFormat = "{0}";
                    //DRSumSummary.DisplayFormat = "{0:#,###0}";

                    SummarySettings DayCrSumSummary = accountSummaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), currentCollumnCollection["DayCr"], SummaryPosition.UseSummaryPositionColumn, null);//, CurrentCollumnCollection[CashManagementConstants.COLUMN_CURRENCYNAME]);
                    DayCrSumSummary.DisplayFormat = "{0}";
                    //DRSumSummary.DisplayFormat = "{0:#,###0}";

                    SummarySettings CloseDRBalSumSummary = accountSummaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), currentCollumnCollection["CloseDrBal"], SummaryPosition.UseSummaryPositionColumn, null);//, CurrentCollumnCollection[CashManagementConstants.COLUMN_CURRENCYNAME]);
                    CloseDRBalSumSummary.DisplayFormat = "{0}";
                    //DRSumSummary.DisplayFormat = "{0:#,###0}";

                    SummarySettings CloseCRBalSumSummary = accountSummaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), currentCollumnCollection["CloseCrBal"], SummaryPosition.UseSummaryPositionColumn, null);//, CurrentCollumnCollection[CashManagementConstants.COLUMN_CURRENCYNAME]);
                    CloseCRBalSumSummary.DisplayFormat = "{0}";
                    //DRSumSummary.DisplayFormat = "{0:#,###0}";

                    SummarySettings OpenDRBalBaseSumSummary = accountSummaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), currentCollumnCollection["OpenDrBalBase"], SummaryPosition.UseSummaryPositionColumn, null);//, CurrentCollumnCollection[CashManagementConstants.COLUMN_CURRENCYNAME]);
                    OpenDRBalBaseSumSummary.DisplayFormat = "{0}";
                    //DRSumSummary.DisplayFormat = "{0:#,###0}";

                    SummarySettings OpenCRBalBaseSumSummary = accountSummaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), currentCollumnCollection["OpenCrBalBase"], SummaryPosition.UseSummaryPositionColumn, null);//, CurrentCollumnCollection[CashManagementConstants.COLUMN_CURRENCYNAME]);
                    OpenCRBalBaseSumSummary.DisplayFormat = "{0}";
                    //DRSumSummary.DisplayFormat = "{0:#,###0}";

                    SummarySettings DayDrBaseSumSummary = accountSummaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), currentCollumnCollection["DayDrBase"], SummaryPosition.UseSummaryPositionColumn, null);//, CurrentCollumnCollection[CashManagementConstants.COLUMN_CURRENCYNAME]);
                    DayDrBaseSumSummary.DisplayFormat = "{0}";
                    //DRSumSummary.DisplayFormat = "{0:#,###0}";

                    SummarySettings DayCrBaseSumSummary = accountSummaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), currentCollumnCollection["DayCrBase"], SummaryPosition.UseSummaryPositionColumn, null);//, CurrentCollumnCollection[CashManagementConstants.COLUMN_CURRENCYNAME]);
                    DayCrBaseSumSummary.DisplayFormat = "{0}";
                    //DRSumSummary.DisplayFormat = "{0:#,###0}";

                    SummarySettings CloseDRBalBaseSumSummary = accountSummaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), currentCollumnCollection["CloseDrBalBase"], SummaryPosition.UseSummaryPositionColumn, null);//, CurrentCollumnCollection[CashManagementConstants.COLUMN_CURRENCYNAME]);
                    CloseDRBalBaseSumSummary.DisplayFormat = "{0}";
                    //DRSumSummary.DisplayFormat = "{0:#,###0}";

                    SummarySettings CloseCRBalBaseSumSummary = accountSummaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), currentCollumnCollection["CloseCrBalBase"], SummaryPosition.UseSummaryPositionColumn, null);//, CurrentCollumnCollection[CashManagementConstants.COLUMN_CURRENCYNAME]);
                    CloseCRBalBaseSumSummary.DisplayFormat = "{0}";
                    //DRSumSummary.DisplayFormat = "{0:#,###0}";
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
        /// Activities the summary settings.
        /// </summary>
        /// <param name="e">The <see cref="InitializeLayoutEventArgs"/> instance containing the event data.</param>
        public static void ActivitySummarySettings(InitializeLayoutEventArgs e)
        {
            try
            {

                UltraGridBand band = e.Layout.Bands[0];

                e.Layout.Override.RowSelectors = DefaultableBoolean.True;
                e.Layout.Override.RowSelectorHeaderStyle = RowSelectorHeaderStyle.ColumnChooserButton;
                e.Layout.ColumnChooserEnabled = DefaultableBoolean.True;


                SummarySettingsCollection summaries = band.Summaries;
                ColumnsCollection currentCollumnCollection = band.Columns;
                CashSummaryFactory summFactory = new CashSummaryFactory();

                //  e.Layout.Override.GroupByColumnsHidden = DefaultableBoolean.True;
                e.Layout.Override.GroupBySummaryDisplayStyle = GroupBySummaryDisplayStyle.SummaryCells;
                e.Layout.Override.SummaryDisplayArea = SummaryDisplayAreas.InGroupByRows;
                e.Layout.Override.GroupByRowExpansionStyle = GroupByRowExpansionStyle.ExpansionIndicatorAndDoubleClick;

                //appearance31.BorderColor = System.Drawing.Color.Transparent;
                //appearance31.ForeColor = System.Drawing.Color.White;

                //e.Layout.Override.GroupBySummaryValueAppearance.BackColor = Color.LightGray;
                e.Layout.UseFixedHeaders = true;
                //e.Layout.Override.GroupBySummaryValueAppearance.ForeColor = Color.White;


                band.Override.HeaderPlacement = HeaderPlacement.FixedOnTop;

                e.Layout.Override.SummaryFooterCaptionVisible = Infragistics.Win.DefaultableBoolean.False;



                #region code from Consolidation View

                e.Layout.Override.GroupByRowDescriptionMask = " ";
                e.Layout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
                e.Layout.GroupByBox.ButtonConnectorColor = System.Drawing.Color.Black;
                e.Layout.GroupByBox.ButtonConnectorStyle = Infragistics.Win.UIElementBorderStyle.Solid;
                e.Layout.GroupByBox.ShowBandLabels = ShowBandLabels.None;

                Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
                appearance4.BackColor = System.Drawing.Color.Black;
                appearance4.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
                appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
                appearance4.BorderColor = System.Drawing.Color.Black;
                //appearance4.FontData.BoldAsString = "True";
                appearance4.ForeColor = System.Drawing.Color.White;
                e.Layout.Override.GroupByRowAppearance = appearance4;

                e.Layout.Override.GroupByRowPadding = 0;
                e.Layout.Override.GroupByRowSpacingAfter = 0;
                e.Layout.Override.GroupByRowSpacingBefore = 0;

                if (!CustomThemeHelper.ApplyTheme)
                {
                    Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
                    appearance5.TextVAlign = VAlign.Middle;
                    appearance5.ForeColor = Color.White;
                    appearance5.BorderColor = System.Drawing.Color.Transparent;
                    e.Layout.Override.GroupBySummaryValueAppearance = appearance5;
                    e.Layout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
                }

                e.Layout.RowConnectorColor = System.Drawing.Color.Transparent;

                #endregion


                //changed
                SummarySettings tradeDatesummary = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcDate"), currentCollumnCollection[CashManagementConstants.COLUMN_TRADEDATE], SummaryPosition.UseSummaryPositionColumn, null);
                tradeDatesummary.DisplayFormat = "{0:MM/dd/yyyy}";

                //mew added
                SummarySettings settlementDatesummary = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcDate"), currentCollumnCollection[CashManagementConstants.COLUMN_SETTLEMENTDATE], SummaryPosition.UseSummaryPositionColumn, null);
                settlementDatesummary.DisplayFormat = "{0:MM/dd/yyyy}";


                SummarySettings accountName = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), currentCollumnCollection[CashManagementConstants.COLUMN_ACCOUNT], SummaryPosition.UseSummaryPositionColumn, null);
                accountName.DisplayFormat = "{0}";

                SummarySettings symbol = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), currentCollumnCollection[CashManagementConstants.COLUMN_SYMBOL], SummaryPosition.UseSummaryPositionColumn, null);
                symbol.DisplayFormat = "{0}";

                SummarySettings currencynameSummary = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), currentCollumnCollection[CashManagementConstants.COLUMN_CURRENCYNAME], SummaryPosition.UseSummaryPositionColumn, null);
                currencynameSummary.DisplayFormat = "{0}";

                SummarySettings leadCurrencySummary = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), currentCollumnCollection[CashManagementConstants.COLUMN_LEADCURRENCY], SummaryPosition.UseSummaryPositionColumn, null);
                leadCurrencySummary.DisplayFormat = "{0}";

                SummarySettings vsCurrencySummary = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), currentCollumnCollection[CashManagementConstants.COLUMN_VSCURRENCY], SummaryPosition.UseSummaryPositionColumn, null);
                vsCurrencySummary.DisplayFormat = "{0}";


                SummarySettings closedQtySummary = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), currentCollumnCollection[CashManagementConstants.COLUMN_CLOSEDQTY], SummaryPosition.UseSummaryPositionColumn, null);
                closedQtySummary.DisplayFormat = "{0:#,###0.00}";

                SummarySettings totalCommissionSummary = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), currentCollumnCollection[CashManagementConstants.COLUMN_TOTALCOMMISSION], SummaryPosition.UseSummaryPositionColumn, null);
                totalCommissionSummary.DisplayFormat = "{0:#,###0.00}";

                SummarySettings amountSummary = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), currentCollumnCollection[CashManagementConstants.COLUMN_AMOUNT], SummaryPosition.UseSummaryPositionColumn, null);
                amountSummary.DisplayFormat = "{0:#,###0.00}";

                SummarySettings FxRate = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcNumText"), currentCollumnCollection[CashManagementConstants.COLUMN_FXRATE], SummaryPosition.UseSummaryPositionColumn, null);
                FxRate.DisplayFormat = "{0}";

                SummarySettings FxConversionMethodOperator = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), currentCollumnCollection[CashManagementConstants.COLUMN_FXCONVERSIONMETHODOPERATOR], SummaryPosition.UseSummaryPositionColumn, null);
                FxConversionMethodOperator.DisplayFormat = "{0}";

                SummarySettings balanceType = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), currentCollumnCollection[CashManagementConstants.COLUMN_BALANCETYPE], SummaryPosition.UseSummaryPositionColumn, null);
                balanceType.DisplayFormat = "{0}";

                SummarySettings activityType = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), currentCollumnCollection[CashManagementConstants.COLUMN_ACTIVITYTYPE], SummaryPosition.UseSummaryPositionColumn, null);
                activityType.DisplayFormat = "{0}";



                SummarySettings description = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), currentCollumnCollection[CashManagementConstants.COLUMN_DESCRIPTION], SummaryPosition.UseSummaryPositionColumn, null);
                description.DisplayFormat = "{0}";

                //SummarySettings transactionSourceSummary = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcDisplayText"), currentCollumnCollection[CashManagementConstants.COLUMN_TRANSACTIONSOURCE], SummaryPosition.UseSummaryPositionColumn, null);
                //transactionSourceSummary.DisplayFormat = "{0}";

                SummarySettings transactionSourceSummary = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcDisplayText"), currentCollumnCollection["TransactionSource"], SummaryPosition.UseSummaryPositionColumn, null);
                transactionSourceSummary.DisplayFormat = "{0}";

                SummarySettings activitySourceSummary = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcDisplayText"), currentCollumnCollection[CashManagementConstants.COLUMN_ACTIVITYSOURCE], SummaryPosition.UseSummaryPositionColumn, null);
                activitySourceSummary.DisplayFormat = "{0}";

                //Added by sachin mishra purpose:PRANA-7896
                SummarySettings SettlementCurrency = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcDisplayText"), currentCollumnCollection[OrderFields.PROPERTY_SETTLEMENTCURRENCY], SummaryPosition.UseSummaryPositionColumn, null);
                SettlementCurrency.DisplayFormat = "{0}";

                //PRANA-9777
                SummarySettings entryDate = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcDate"), currentCollumnCollection[CashManagementConstants.COLUMN_ENTRYDATE], SummaryPosition.UseSummaryPositionColumn, null);
                entryDate.DisplayFormat = "{0:MM/dd/yyyy}";

                //PRANA-9777
                SummarySettings modifyDate = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcDate"), currentCollumnCollection[CashManagementConstants.COLUMN_MODIFYDATE], SummaryPosition.UseSummaryPositionColumn, null);
                modifyDate.DisplayFormat = "{0:MM/dd/yyyy}";

                //PRANA-9776
                SummarySettings userSummary = summaries.Add(currentCollumnCollection[CashManagementConstants.COLUMN_USERNAME].Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcDisplayText"), currentCollumnCollection[CashManagementConstants.COLUMN_USERNAME], SummaryPosition.UseSummaryPositionColumn, null);
                userSummary.DisplayFormat = "{0}";

                SummarySettings feesSummary;
                feesSummary = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), currentCollumnCollection[OrderFields.PROPERTY_ORFFEE], SummaryPosition.UseSummaryPositionColumn, null);
                feesSummary.DisplayFormat = "{0:#,###0.00}";

                feesSummary = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), currentCollumnCollection[OrderFields.PROPERTY_OCCFEE], SummaryPosition.UseSummaryPositionColumn, null);
                feesSummary.DisplayFormat = "{0:#,###0.00}";

                feesSummary = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), currentCollumnCollection[OrderFields.PROPERTY_SECFEE], SummaryPosition.UseSummaryPositionColumn, null);
                feesSummary.DisplayFormat = "{0:#,###0.00}";

                feesSummary = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), currentCollumnCollection[OrderFields.PROPERTY_MISCFEES], SummaryPosition.UseSummaryPositionColumn, null);
                feesSummary.DisplayFormat = "{0:#,###0.00}";

                feesSummary = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), currentCollumnCollection[OrderFields.PROPERTY_CLEARINGBROKERFEE], SummaryPosition.UseSummaryPositionColumn, null);
                feesSummary.DisplayFormat = "{0:#,###0.00}";

                feesSummary = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), currentCollumnCollection[OrderFields.PROPERTY_CLEARINGFEE], SummaryPosition.UseSummaryPositionColumn, null);
                feesSummary.DisplayFormat = "{0:#,###0.00}";

                feesSummary = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), currentCollumnCollection[OrderFields.PROPERTY_OTHERBROKERFEES], SummaryPosition.UseSummaryPositionColumn, null);
                feesSummary.DisplayFormat = "{0:#,###0.00}";

                feesSummary = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), currentCollumnCollection[OrderFields.PROPERTY_SOFTCOMMISSION], SummaryPosition.UseSummaryPositionColumn, null);
                feesSummary.DisplayFormat = "{0:#,###0.00}";

                feesSummary = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), currentCollumnCollection[OrderFields.PROPERTY_STAMPDUTY], SummaryPosition.UseSummaryPositionColumn, null);
                feesSummary.DisplayFormat = "{0:#,###0.00}";

                feesSummary = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), currentCollumnCollection[OrderFields.PROPERTY_TRANSACTIONLEVY], SummaryPosition.UseSummaryPositionColumn, null);
                feesSummary.DisplayFormat = "{0:#,###0.00}";

                feesSummary = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), currentCollumnCollection[OrderFields.PROPERTY_TAXONCOMMISSIONS], SummaryPosition.UseSummaryPositionColumn, null);
                feesSummary.DisplayFormat = "{0:#,###0.00}";

                feesSummary = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), currentCollumnCollection[OrderFields.PROPERTY_COMMISSION], SummaryPosition.UseSummaryPositionColumn, null);
                feesSummary.DisplayFormat = "{0:#,###0.00}";

                //Added by surendra Bisht
                if (!CustomThemeHelper.ApplyTheme)
                {
                    Infragistics.Win.Appearance appSummaryRow = new Infragistics.Win.Appearance();
                    appSummaryRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(76)))), ((int)(((byte)(122)))));
                    appSummaryRow.ForeColor = System.Drawing.Color.White;
                    appSummaryRow.TextHAlignAsString = "Right";
                    appSummaryRow.TextVAlignAsString = "Middle";
                    e.Layout.Override.SummaryValueAppearance = appSummaryRow;
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
        /// Binds the grid today day end data.
        /// </summary>
        /// <param name="grd">The GRD.</param>
        /// <param name="dataSource">The data source.</param>
        /// <param name="lsColumnsToDisplay">The ls columns to display.</param>
        public static void BindGridTodayDayEndData(UltraGrid grd, GenericBindingList<CompanyAccountCashCurrencyValue> dataSource, List<string> lsColumnsToDisplay)
        {
            try
            {
                grd.DataSource = dataSource;
                SetDayEndCashDisplayNames(grd, lsColumnsToDisplay);
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
        /// Gets the group by row color from configuration.
        /// </summary>
        private static void GetGroupByRowColorFromConfig()
        {
            try
            {
                _firstLevelForeColor = Color.FromName(ConfigurationHelper.Instance.GetAppSettingValueByKey("CustomViewFirstLevelForeColor"));
                _firstLevelBackColor = Color.FromArgb(Convert.ToInt32("0x78" + ConfigurationHelper.Instance.GetAppSettingValueByKey("CustomViewFirstLevelBackColor"), 16));

                _secondLevelForeColor = Color.FromName(ConfigurationHelper.Instance.GetAppSettingValueByKey("CustomViewSecondLevelForeColor"));
                _secondLevelBackColor = Color.FromArgb(Convert.ToInt32("0x78" + ConfigurationHelper.Instance.GetAppSettingValueByKey("CustomViewSecondLevelBackColor"), 16));

                _thirdLevelForeColor = Color.FromName(ConfigurationHelper.Instance.GetAppSettingValueByKey("CustomViewThirdLevelForeColor"));
                _thirdLevelBackColor = Color.FromArgb(Convert.ToInt32("0x78" + ConfigurationHelper.Instance.GetAppSettingValueByKey("CustomViewThirdLevelBackColor"), 16));
                _isGroupByRowColorGeted = true;
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
        /// Globals the grid setting.
        /// </summary>
        /// <param name="band">The band.</param>
        public static void GlobalGridSetting(UltraGridBand band)
        {
            try
            {
                foreach (ColumnFilter filter in band.ColumnFilters)
                {
                    if (filter.Column.Hidden)
                    {
                        filter.ClearFilterConditions();
                    }
                }

                #region header setting

                FontData fontData = band.Override.HeaderAppearance.FontData;
                fontData.Bold = DefaultableBoolean.False;
                fontData.SizeInPoints = 8;
                fontData.Name = "Tahoma";
                band.Override.HeaderAppearance.TextHAlign = HAlign.Center;
                band.Override.HeaderAppearance.TextVAlign = VAlign.Middle;
                band.Override.HeaderStyle = HeaderStyle.WindowsXPCommand;
                band.Override.HeaderClickAction = HeaderClickAction.SortSingle;

                Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
                appearance2.BackColor = System.Drawing.Color.Silver;
                appearance2.BackColor2 = System.Drawing.Color.DimGray;
                appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
                band.Layout.GroupByBox.Appearance = appearance2;
                #endregion

                #region Row Setting

                Infragistics.Win.Appearance appearance = new Infragistics.Win.Appearance();
                if (!CustomThemeHelper.ApplyTheme)
                {
                    appearance.BackColor = System.Drawing.Color.Black;
                    appearance.ForeColor = System.Drawing.Color.White;
                }
                else
                {
                    appearance.BackColor = System.Drawing.Color.FromArgb(231, 232, 233);
                }
                appearance.BorderColor2 = System.Drawing.Color.DimGray;
                appearance.TextHAlignAsString = "Middle";
                appearance.TextVAlignAsString = "Middle";
                band.Override.RowAppearance = appearance;

                band.Override.RowSelectorHeaderStyle = RowSelectorHeaderStyle.ColumnChooserButton;
                #endregion

                UpdateColumnTextAlignment(band.Columns);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Grids the setting for journal look.
        /// </summary>
        /// <param name="grid">The grid.</param>
        public static void GridSettingForJournalLook(UltraGrid grid)
        {
            try
            {
                // grid.CreationFilter = _CreationFilterObj;
                UltraGridBand transactionEntryBand;
                if (grid.DisplayLayout.Bands.Exists("TransactionEntries"))
                    transactionEntryBand = grid.DisplayLayout.Bands["TransactionEntries"];
                else
                    transactionEntryBand = grid.DisplayLayout.Bands[0];


                #region Merge Cell

                if (transactionEntryBand.Columns.Count > 0)
                {
                    transactionEntryBand.Columns[CashManagementConstants.COLUMN_SYMBOL].MergedCellStyle = MergedCellStyle.Always;
                    transactionEntryBand.Columns[CashManagementConstants.COLUMN_ACCOUNT].MergedCellStyle = MergedCellStyle.Always;
                    transactionEntryBand.Columns[CashManagementConstants.COLUMN_CURRENCYNAME].MergedCellStyle = MergedCellStyle.Always;
                    transactionEntryBand.Columns["TransactionDate"].MergedCellStyle = MergedCellStyle.Always;
                    transactionEntryBand.Columns["TransactionID"].MergedCellStyle = MergedCellStyle.Always;
                    transactionEntryBand.Columns[CashManagementConstants.COLUMN_DESCRIPTION].MergedCellStyle = MergedCellStyle.Always;

                    //PRANA-9777
                    transactionEntryBand.Columns[CashManagementConstants.COLUMN_MODIFYDATE].MergedCellStyle = MergedCellStyle.Always;
                    transactionEntryBand.Columns[CashManagementConstants.COLUMN_ENTRYDATE].MergedCellStyle = MergedCellStyle.Always;

                    //PRANA-9776
                    transactionEntryBand.Columns[CashManagementConstants.COLUMN_USERNAME].MergedCellStyle = MergedCellStyle.Always;

                }

                #endregion

                #region Row Setting
                if (!CustomThemeHelper.ApplyTheme)
                {

                    Infragistics.Win.Appearance appearance = new Infragistics.Win.Appearance();
                    appearance.BackColor = System.Drawing.Color.Black;
                    appearance.ForeColor = System.Drawing.Color.White;
                    appearance.BorderColor = System.Drawing.Color.DimGray;
                    appearance.BorderColor2 = System.Drawing.Color.DimGray;

                    //appearance.TextHAlignAsString = "Right";
                    appearance.TextVAlignAsString = "Middle";
                    grid.DisplayLayout.Bands[0].Override.RowAppearance = appearance;
                }
                else
                {
                    Infragistics.Win.Appearance appearance = new Infragistics.Win.Appearance();
                    appearance.BackColor = Color.FromArgb(231, 232, 233);
                    appearance.ForeColor = System.Drawing.Color.White;
                    appearance.BorderColor = Color.FromArgb(33, 44, 57);
                    appearance.BorderColor2 = Color.FromArgb(33, 44, 57);

                    //appearance.TextHAlignAsString = "Right";
                    appearance.TextVAlignAsString = "Middle";
                    grid.DisplayLayout.Bands[0].Override.RowAppearance = appearance;
                }
                #endregion
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Groups the by row setting.
        /// </summary>
        /// <param name="e">The <see cref="InitializeGroupByRowEventArgs"/> instance containing the event data.</param>
        internal static void GroupByRowSetting(InitializeGroupByRowEventArgs e)
        {
            try
            {
                if (!CustomThemeHelper.ApplyTheme)
                {
                    if (e.Row is UltraGridGroupByRow)
                    {
                        if (!_isGroupByRowColorGeted)
                            GetGroupByRowColorFromConfig();

                        if (!e.Row.HasParent())
                        {
                            //let it to be the default black color
                            e.Row.Appearance.BackColor = _firstLevelBackColor;
                            e.Row.Appearance.ForeColor = _firstLevelForeColor;
                            //e.Row..Layout.Override.GroupByRowDescriptionMask = " ";
                        }
                        else
                        {
                            ///this is the intermediate node
                            if (!e.Row.ParentRow.HasParent())
                            {
                                e.Row.Appearance.BackColor = _secondLevelBackColor;
                                e.Row.Appearance.ForeColor = _secondLevelForeColor;
                                //  e.Layout.Override.GroupByRowDescriptionMask = "[value]";
                            }
                            else
                            {// this is the child node
                                e.Row.Appearance.BackColor = _thirdLevelBackColor;
                                e.Row.Appearance.ForeColor = _thirdLevelForeColor;
                                //e.Layout.Override.GroupByRowDescriptionMask = "[value]";
                            }
                        }

                        e.Row.Appearance.BackGradientStyle = GradientStyle.None;
                    }
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
        /// Rows the color settings.
        /// </summary>
        /// <param name="e">The <see cref="InitializeRowEventArgs"/> instance containing the event data.</param>
        public static void RowColorSettings(InitializeRowEventArgs e)
        {
            try
            {
                if (e.Row.Cells.Exists("cashValuelocal"))
                {
                    double cashValuelocal = Convert.ToDouble(e.Row.Cells["cashValuelocal"].Value);

                    if (cashValuelocal >= 0)
                    {
                        e.Row.Appearance.ForeColor = Color.FromArgb(39, 174, 96);
                    }
                    else
                    {

                        e.Row.Appearance.ForeColor = Color.FromArgb(192, 57, 43);
                    }
                    e.Row.Cells["cashValuelocal"].SetValue(cashValuelocal.ToString("#0.00"), true);
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
        /// Sets the column display names.
        /// </summary>
        /// <param name="grd">The GRD.</param>
        /// <param name="lsColumnsToDisplay">The ls columns to display.</param>
        public static void SetColumnDisplayNames(UltraGrid grd, List<string> lsColumnsToDisplay)
        {
            try
            {
                if (grd.DataSource as GenericBindingList<TransactionEntry> != null)
                {
                    if (!grd.IsUpdating)
                    {

                        UltraGridBand band = null;
                        if (grd.DisplayLayout.Bands.Exists("TransactionEntries"))
                        {
                            band = grd.DisplayLayout.Bands["TransactionEntries"];
                        }
                        else
                        {
                            band = grd.DisplayLayout.Bands[0];
                        }

                        if (band.Columns.Count > 0)
                        {
                            band.Columns["TaxLotId"].CellActivation = Activation.NoEdit;
                            band.Columns["TransactionID"].CellActivation = Activation.NoEdit;
                            band.Columns["TransactionSource"].CellActivation = Activation.NoEdit;
                            //band.Columns[CashManagementConstants.COLUMN_SYMBOL].MergedCellStyle = MergedCellStyle.Always;
                            //band.Columns[CashManagementConstants.COLUMN_ACCOUNT].MergedCellStyle = MergedCellStyle.Always;
                            //band.Columns[CashManagementConstants.COLUMN_CURRENCYNAME].MergedCellStyle = MergedCellStyle.Always;
                            //band.Columns["TransactionDate"].MergedCellStyle = MergedCellStyle.Always;
                            //band.Columns["TransactionID"].MergedCellStyle = MergedCellStyle.Always;

                            if (lsColumnsToDisplay == null)
                                lsColumnsToDisplay = new List<string>(new string[] { "TransactionDate", CashManagementConstants.COLUMN_ACCOUNT, CashManagementConstants.COLUMN_SYMBOL, CashManagementConstants.COLUMN_CURRENCYNAME, "SubAcName", "DR", "CR" });
                            UltraWinGridUtils.HideColumns(band);
                            UltraWinGridUtils.SetBand(lsColumnsToDisplay, band);
                            band.Columns[CashManagementConstants.COLUMN_SYMBOL].Width = 100;
                            band.Columns[CashManagementConstants.COLUMN_SYMBOL].Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                            band.Columns[CashManagementConstants.COLUMN_SYMBOL].CharacterCasing = CharacterCasing.Upper;
                            band.Columns[CashManagementConstants.COLUMN_ACCOUNT].Header.Caption = "Account";
                            band.Columns[CashManagementConstants.COLUMN_ACCOUNT].Width = 75;
                            //band.Columns[CashManagementConstants.COLUMN_ACCOUNT].CellActivation = Activation.NoEdit;
                            band.Columns["DR"].Header.Caption = "DR";
                            band.Columns["DR"].Width = 90;
                            band.Columns["DR"].Format = "#,###0.00";
                            band.Columns["CR"].Header.Caption = "CR";
                            band.Columns["CR"].Width = 90;
                            band.Columns["CR"].Format = "#,###0.00";

                            band.Columns["DrBase"].Header.Caption = "DR (Base)";
                            band.Columns["DrBase"].Width = 90;
                            band.Columns["DrBase"].Format = "#,###0.00";
                            band.Columns["DrBase"].CellActivation = Activation.ActivateOnly;
                            band.Columns["CrBase"].Header.Caption = "CR (Base)";
                            band.Columns["CrBase"].Width = 90;
                            band.Columns["CrBase"].Format = "#,###0.00";
                            band.Columns["CrBase"].CellActivation = Activation.ActivateOnly;

                            band.Columns[CashManagementConstants.COLUMN_FXRATE].Width = 90;
                            //band.Columns[CashManagementConstants.COLUMN_FXRATE].Format = "#,###0.00";
                            band.Columns[CashManagementConstants.COLUMN_FXRATE].Header.Caption = "Fx Rate";

                            band.Columns[CashManagementConstants.COLUMN_FXCONVERSIONMETHODOPERATOR].Width = 90;
                            band.Columns[CashManagementConstants.COLUMN_FXCONVERSIONMETHODOPERATOR].Header.Caption = "FX Conversion Method Operator";


                            band.Columns[CashManagementConstants.COLUMN_CURRENCYNAME].Header.Caption = "Currency";
                            band.Columns[CashManagementConstants.COLUMN_CURRENCYNAME].Width = 75;

                            band.Columns["BaseCurrencyName"].Header.Caption = "Base Currency";
                            band.Columns["BaseCurrencyName"].Width = 100;
                            band.Columns["BaseCurrencyName"].CellActivation = Activation.NoEdit;
                            //band.Columns[CashManagementConstants.COLUMN_CURRENCYNAME].CellActivation = Activation.NoEdit;

                            band.Columns["SubAcName"].Header.Caption = "Cash Sub-Account";
                            band.Columns["SubAcName"].Width = 160;
                            //band.Columns["SubAcName"].CellActivation = Activation.NoEdit;

                            band.Columns["TransactionDate"].Header.Caption = CashManagementConstants.COLUMN_DATE;
                            band.Columns["TransactionDate"].Width = 120;

                            //band.Columns["TransactionID"].Header.Caption = "Transaction ID";
                            //band.Columns["TransactionID"].Width = 0;
                            //band.Columns["TransactionID"].Header.VisiblePosition = 0;

                            band.Columns["EntryAccountSide"].Header.Caption = "Account Side";
                            band.Columns["EntryAccountSide"].Width = 120;
                            band.Columns["EntryAccountSide"].Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;


                            band.Columns[CashManagementConstants.COLUMN_DESCRIPTION].Width = 120;
                            band.Columns[CashManagementConstants.COLUMN_DESCRIPTION].Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;

                            band.Columns["CurrencyID"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                            band.Columns["BaseCurrencyID"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                            band.Columns["AccountID"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                            band.Columns["SubAcID"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                            //PRANA-9777
                            band.Columns[CashManagementConstants.COLUMN_ENTRYDATE].Header.Caption = CashManagementConstants.CAPTION_ENTRYDATE;
                            band.Columns[CashManagementConstants.COLUMN_ENTRYDATE].CellActivation = Activation.NoEdit;
                            band.Columns[CashManagementConstants.COLUMN_MODIFYDATE].Header.Caption = CashManagementConstants.CAPTION_MODIFYDATE;
                            band.Columns[CashManagementConstants.COLUMN_MODIFYDATE].CellActivation = Activation.NoEdit;
                            //PRANA-9776
                            band.Columns[CashManagementConstants.COLUMN_USERNAME].Header.Caption = CashManagementConstants.CAPTION_USERNAME;
                            band.Columns[CashManagementConstants.COLUMN_USERNAME].CellActivation = Activation.NoEdit;
                            band.Columns[CashManagementConstants.COLUMN_USERID].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                            if (grd.Name.Equals("grdTradingTransactions") || grd.Name.Equals("grdDividend"))
                                band.Columns[CashManagementConstants.COLUMN_USERNAME].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                            grd.DisplayLayout.ColumnChooserEnabled = DefaultableBoolean.True;
                            GlobalGridSetting(band);

                        }
                    }
                }

                else if (grd.DataSource as GenericBindingList<CashActivity> != null)
                {
                    UltraGridBand band = grd.DisplayLayout.Bands[0];
                    if (band.Columns.Count > 0)
                    {
                        if (lsColumnsToDisplay == null)
                            lsColumnsToDisplay = new List<string>(new string[] { CashManagementConstants.COLUMN_TRADEDATE, CashManagementConstants.COLUMN_ACCOUNT, CashManagementConstants.COLUMN_SYMBOL, CashManagementConstants.COLUMN_CURRENCYNAME, CashManagementConstants.COLUMN_AMOUNT, CashManagementConstants.COLUMN_ACTIVITYTYPE, CashManagementConstants.COLUMN_TRANSACTIONSOURCE, CashManagementConstants.COLUMN_TOTALCOMMISSION });
                        UltraWinGridUtils.HideColumns(band);
                        UltraWinGridUtils.SetBand(lsColumnsToDisplay, band);
                        band.Columns[CashManagementConstants.COLUMN_SYMBOL].Width = 100;
                        band.Columns[CashManagementConstants.COLUMN_SYMBOL].Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                        band.Columns[CashManagementConstants.COLUMN_SYMBOL].CharacterCasing = CharacterCasing.Upper;
                        band.Columns[CashManagementConstants.COLUMN_SYMBOL].CellActivation = Activation.NoEdit;

                        band.Columns[CashManagementConstants.COLUMN_ACCOUNT].Header.Caption = CashManagementConstants.CAPTION_FUND;
                        band.Columns[CashManagementConstants.COLUMN_ACCOUNT].Width = 75;
                        band.Columns[CashManagementConstants.COLUMN_ACCOUNT].CellActivation = Activation.NoEdit;
                        band.Columns[CashManagementConstants.COLUMN_ACCOUNT].NullText = String.Empty;

                        band.Columns[CashManagementConstants.COLUMN_AMOUNT].Width = 75;
                        band.Columns[CashManagementConstants.COLUMN_AMOUNT].CellActivation = Activation.NoEdit;
                        band.Columns[CashManagementConstants.COLUMN_AMOUNT].Format = "#,###0.00";
                        band.Columns[CashManagementConstants.COLUMN_AMOUNT].Header.Caption = "Net Amount(Local)";

                        band.Columns[CashManagementConstants.COLUMN_FXRATE].Width = 50;
                        band.Columns[CashManagementConstants.COLUMN_FXRATE].Format = "#,#####0.00";
                        band.Columns[CashManagementConstants.COLUMN_FXRATE].Header.Caption = "Fx Rate";
                        band.Columns[CashManagementConstants.COLUMN_FXRATE].CellActivation = Activation.NoEdit;

                        band.Columns[CashManagementConstants.COLUMN_FXCONVERSIONMETHODOPERATOR].Width = 90;
                        band.Columns[CashManagementConstants.COLUMN_FXCONVERSIONMETHODOPERATOR].Header.Caption = "FX Conversion Method Operator";
                        band.Columns[CashManagementConstants.COLUMN_FXCONVERSIONMETHODOPERATOR].CellActivation = Activation.NoEdit;

                        band.Columns[OrderFields.PROPERTY_SETTLEMENTCURRENCY].Width = 90;
                        band.Columns[OrderFields.PROPERTY_SETTLEMENTCURRENCY].Header.Caption = OrderFields.CAPTION_SETTLEMENT_CURRENCY;
                        band.Columns[OrderFields.PROPERTY_SETTLEMENTCURRENCY].CellActivation = Activation.NoEdit;
                        Dictionary<int, string> dictCurrencies = CachedDataManager.GetInstance.GetAllCurrencies();
                        ValueList currencies = new ValueList();
                        foreach (KeyValuePair<int, string> item in dictCurrencies)
                        {
                            currencies.ValueListItems.Add(item.Value, item.Value);
                        }
                        currencies.ValueListItems.Add(0, ApplicationConstants.C_COMBO_NONE);
                        band.Columns[OrderFields.PROPERTY_SETTLEMENTCURRENCY].ValueList = currencies;

                        ValueList SettFXConversionMethodOperatorList = new ValueList();
                        List<EnumerationValue> fxConversionMethodOperator = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(Prana.BusinessObjects.AppConstants.Operator));
                        foreach (EnumerationValue var in fxConversionMethodOperator)
                        {
                            if (!var.Value.Equals((int)Prana.BusinessObjects.AppConstants.Operator.Multiple))
                            {
                                SettFXConversionMethodOperatorList.ValueListItems.Add(var.Value, var.DisplayText);
                            }
                        }

                        band.Columns[CashManagementConstants.COLUMN_TRADEDATE].Width = 120;
                        band.Columns[CashManagementConstants.COLUMN_TRADEDATE].CellActivation = Activation.NoEdit;
                        band.Columns[CashManagementConstants.COLUMN_TRADEDATE].Hidden = false;
                        band.Columns[CashManagementConstants.COLUMN_TRADEDATE].Header.Caption = CashManagementConstants.CAPTION_TRADEDATE;

                        band.Columns[CashManagementConstants.COLUMN_SETTLEMENTDATE].Width = 150;
                        band.Columns[CashManagementConstants.COLUMN_SETTLEMENTDATE].CellActivation = Activation.NoEdit;
                        band.Columns[CashManagementConstants.COLUMN_SETTLEMENTDATE].Header.Caption = CashManagementConstants.CAPTION_SETTLEMENTDATE;

                        band.Columns[CashManagementConstants.COLUMN_BALANCETYPE].Header.Caption = CashManagementConstants.CAPTION_BALANCETYPE;
                        band.Columns[CashManagementConstants.COLUMN_BALANCETYPE].CellActivation = Activation.NoEdit;
                        band.Columns[CashManagementConstants.COLUMN_BALANCETYPE].Width = 100;
                        band.Columns[CashManagementConstants.COLUMN_BALANCETYPE].CellActivation = Activation.NoEdit;


                        band.Columns[CashManagementConstants.COLUMN_ACTIVITYTYPE].Width = 100;
                        band.Columns[CashManagementConstants.COLUMN_ACTIVITYTYPE].CellActivation = Activation.NoEdit;

                        band.Columns[CashManagementConstants.COLUMN_CURRENCYNAME].Header.Caption = CashManagementConstants.CAPTION_CURRENCY;
                        band.Columns[CashManagementConstants.COLUMN_CURRENCYNAME].Width = 75;
                        band.Columns[CashManagementConstants.COLUMN_CURRENCYNAME].CellActivation = Activation.NoEdit;

                        band.Columns[CashManagementConstants.COLUMN_TRANSACTIONSOURCE].Header.Caption = CashManagementConstants.CAPTION_TRANSACTIONSOURCE;
                        band.Columns[CashManagementConstants.COLUMN_TRANSACTIONSOURCE].Width = 100;
                        band.Columns[CashManagementConstants.COLUMN_TRANSACTIONSOURCE].CellActivation = Activation.NoEdit;
                        band.Columns[CashManagementConstants.COLUMN_TRANSACTIONSOURCE].CellActivation = Activation.NoEdit;

                        band.Columns[CashManagementConstants.COLUMN_ACTIVITYSOURCE].Header.Caption = CashManagementConstants.CAPTION_ACTIVITYSOURCE;
                        band.Columns[CashManagementConstants.COLUMN_ACTIVITYSOURCE].Width = 100;
                        band.Columns[CashManagementConstants.COLUMN_ACTIVITYSOURCE].CellActivation = Activation.NoEdit;
                        band.Columns[CashManagementConstants.COLUMN_ACTIVITYSOURCE].CellActivation = Activation.NoEdit;

                        band.Columns[CashManagementConstants.COLUMN_LEADCURRENCY].Header.Caption = CashManagementConstants.CAPTION_LEADCURRENCY;
                        band.Columns[CashManagementConstants.COLUMN_LEADCURRENCY].Width = 75;
                        band.Columns[CashManagementConstants.COLUMN_LEADCURRENCY].CellActivation = Activation.NoEdit;

                        band.Columns[CashManagementConstants.COLUMN_VSCURRENCY].Header.Caption = CashManagementConstants.CAPTION_VSCURRENCY;
                        band.Columns[CashManagementConstants.COLUMN_VSCURRENCY].Width = 75;
                        band.Columns[CashManagementConstants.COLUMN_VSCURRENCY].CellActivation = Activation.NoEdit;
                        //band.Columns[CashManagementConstants.COLUMN_CURRENCYNAME].CellActivation = Activation.NoEdit;

                        band.Columns[CashManagementConstants.COLUMN_CLOSEDQTY].Header.Caption = CashManagementConstants.CAPTION_CLOSEDQTY;
                        band.Columns[CashManagementConstants.COLUMN_CLOSEDQTY].Width = 160;
                        band.Columns[CashManagementConstants.COLUMN_CLOSEDQTY].CellActivation = Activation.NoEdit;

                        band.Columns[CashManagementConstants.COLUMN_DESCRIPTION].Width = 120;
                        band.Columns[CashManagementConstants.COLUMN_DESCRIPTION].Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                        band.Columns[CashManagementConstants.COLUMN_DESCRIPTION].CellActivation = Activation.NoEdit;

                        band.Columns[CashManagementConstants.COLUMN_TOTALCOMMISSION].Header.Caption = CashManagementConstants.CAPTION_TOTALCOMMISSION;
                        band.Columns[CashManagementConstants.COLUMN_TOTALCOMMISSION].Width = 120;
                        band.Columns[CashManagementConstants.COLUMN_TOTALCOMMISSION].Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                        band.Columns[CashManagementConstants.COLUMN_TOTALCOMMISSION].CellActivation = Activation.NoEdit;

                        band.Columns[OrderFields.PROPERTY_ORFFEE].Header.Caption = OrderFields.CAPTION_ORFFEE;
                        band.Columns[OrderFields.PROPERTY_OCCFEE].Header.Caption = OrderFields.CAPTION_OCCFEE;
                        band.Columns[OrderFields.PROPERTY_SECFEE].Header.Caption = OrderFields.CAPTION_SECFEE;
                        band.Columns[OrderFields.PROPERTY_MISCFEES].Header.Caption = OrderFields.CAPTION_MISCFEES;
                        band.Columns[OrderFields.PROPERTY_CLEARINGBROKERFEE].Header.Caption = OrderFields.CAPTION_CLEARINGBROKERFEE;
                        band.Columns[OrderFields.PROPERTY_CLEARINGFEE].Header.Caption = OrderFields.CAPTION_CLEARINGFEE;
                        band.Columns[OrderFields.PROPERTY_OTHERBROKERFEES].Header.Caption = OrderFields.CAPTION_OTHERBROKERFEES;
                        band.Columns[OrderFields.PROPERTY_SOFTCOMMISSION].Header.Caption = OrderFields.CAPTION_SOFTCOMMISSION;
                        band.Columns[OrderFields.PROPERTY_STAMPDUTY].Header.Caption = OrderFields.CAPTION_STAMPDUTY;
                        band.Columns[OrderFields.PROPERTY_TRANSACTIONLEVY].Header.Caption = OrderFields.CAPTION_TRANSACTIONLEVY;
                        band.Columns[OrderFields.PROPERTY_TAXONCOMMISSIONS].Header.Caption = OrderFields.CAPTION_TAXONCOMMISSIONS;

                        //PRANA-9777
                        band.Columns[CashManagementConstants.COLUMN_ENTRYDATE].Header.Caption = CashManagementConstants.CAPTION_ENTRYDATE;
                        band.Columns[CashManagementConstants.COLUMN_ENTRYDATE].CellActivation = Activation.NoEdit;
                        band.Columns[CashManagementConstants.COLUMN_MODIFYDATE].Header.Caption = CashManagementConstants.CAPTION_MODIFYDATE;
                        band.Columns[CashManagementConstants.COLUMN_MODIFYDATE].CellActivation = Activation.NoEdit;

                        //PRANA-9776
                        band.Columns[CashManagementConstants.COLUMN_USERNAME].Header.Caption = CashManagementConstants.CAPTION_USERNAME;
                        band.Columns[CashManagementConstants.COLUMN_USERNAME].CellActivation = Activation.NoEdit;
                        band.Columns[CashManagementConstants.COLUMN_USERID].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                        grd.DisplayLayout.ColumnChooserEnabled = DefaultableBoolean.True;

                        GlobalGridSetting(band);

                    }
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
        /// Sets the day end cash display names.
        /// </summary>
        /// <param name="grd">The GRD.</param>
        /// <param name="lsColumnsToDisplay">The ls columns to display.</param>
        internal static void SetDayEndCashDisplayNames(UltraGrid grd, List<string> lsColumnsToDisplay)
        {
            try
            {
                UltraGridBand band = grd.DisplayLayout.Bands[0];
                if (band.Columns.Count > 0 && !grd.IsUpdating)
                {
                    //grd.DisplayLayout.ColumnChooserEnabled = DefaultableBoolean.False;
                    if (lsColumnsToDisplay == null || lsColumnsToDisplay.Count == 0)
                        lsColumnsToDisplay = new List<string>(new string[] { CashManagementConstants.COLUMN_ACCOUNT, CashManagementConstants.COLUMN_DATE, "LocalCurrencyName", "CashValueLocal", "CashValueBaseWithTransactionFX", "BaseCurrencyName" });

                    UltraWinGridUtils.SetColumns(lsColumnsToDisplay, grd);
                    band.Columns[CashManagementConstants.COLUMN_ACCOUNT].Header.Caption = "Account";
                    band.Columns["LocalCurrencyName"].Header.Caption = "Currency (Local)";
                    band.Columns["CashValueBase"].Header.Caption = "Cash (Base)";
                    band.Columns["CashValueBase"].Format = "#,###0.00";
                    band.Columns["CashValueLocal"].Header.Caption = "Cash (Local)";
                    band.Columns["CashValueLocal"].Width = 100;
                    band.Columns["CashValueLocal"].Format = "#,###0.00";
                    band.Columns["BaseCurrencyName"].Header.Caption = "Currency (Base)";
                    band.Columns["CashCurrencyID"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    GlobalGridSetting(band);

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
        /// Sets the taxlot display names.
        /// </summary>
        /// <param name="grd">The GRD.</param>
        internal static void SetTaxlotDisplayNames(UltraGrid grd)
        {
            try
            {
                UltraGridBand band = grd.DisplayLayout.Bands[0];
                if (band.Columns.Count > 0 && !grd.IsUpdating)
                {
                    //grd.DisplayLayout.ColumnChooserEnabled = DefaultableBoolean.False;
                    List<string> lsColumnsToDisplay = new List<string>(new string[] { CashManagementConstants.COLUMN_SYMBOL, "TaxLotQty", "AvgPrice", "AUECLocalDate", "OrderSide" });

                    UltraWinGridUtils.SetColumns(lsColumnsToDisplay, grd);

                    #region Columns Excluded From Column Choser

                    //modified by: Bharat raturi, 15 jul 2014
                    //purpose: Show the account name column and change the caption of the header
                    //band.Columns["Level1Name"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["Level1Name"].Header.Caption = "Account";
                    band.Columns["Level2Name"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["Percentage"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["Quantity"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["CumQty"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["ExecutedQty"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["Commission"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["TaxLotClosingId"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["GroupID"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["IsPreAllocated"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["ClosingMode"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["TimeOfSaveUTC"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["PositionTag"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["AUECModifiedDate"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["OpenTotalCommissionandFees"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["ClosedTotalCommissionandFees"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["UnitCost"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["NetNotionalValue"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["SettledQty"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["IsExerciseAtZero"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["AssetCategoryValue"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["CashSettledPrice"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["IsPosition"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["PositionType"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["SecurityTypeName"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["SectorName"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["SubSectorName"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["OrderSideTagValue"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["OrderType"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["UnderlyingName"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["ExchangeName"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["CurrencyID"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["TradingAccountName"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["CompanyUserName"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["CounterPartyName"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["Venue"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["FXConversionMethodOperator"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["CompanyName"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["ClosingDate"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["Delta"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["VsCurrencyID"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["LeadCurrencyID"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["StrikePrice"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["PutOrCall"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["PutOrCall"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["FXRate"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                    #endregion

                    #region Columns Renamed

                    band.Columns["TaxLotID"].Header.Caption = "TaxlotId";
                    band.Columns[OrderFields.PROPERTY_OTHERBROKERFEES].Header.Caption = OrderFields.CAPTION_OTHERBROKERFEES;
                    band.Columns[OrderFields.PROPERTY_CLEARINGBROKERFEE].Header.Caption = OrderFields.CAPTION_CLEARINGBROKERFEE;
                    band.Columns["TotalCommissionandFees"].Header.Caption = "Total Commissionand Fees";
                    band.Columns[OrderFields.PROPERTY_STAMPDUTY].Header.Caption = OrderFields.CAPTION_STAMPDUTY;
                    band.Columns[OrderFields.PROPERTY_TRANSACTIONLEVY].Header.Caption = OrderFields.CAPTION_TRANSACTIONLEVY;
                    band.Columns[OrderFields.PROPERTY_CLEARINGFEE].Header.Caption = OrderFields.CAPTION_CLEARINGFEE;
                    band.Columns[OrderFields.PROPERTY_TAXONCOMMISSIONS].Header.Caption = OrderFields.CAPTION_TAXONCOMMISSIONS;
                    band.Columns[OrderFields.PROPERTY_MISCFEES].Header.Caption = OrderFields.CAPTION_MISCFEES;
                    band.Columns[OrderFields.PROPERTY_SECFEE].Header.Caption = OrderFields.CAPTION_SECFEE;
                    band.Columns[OrderFields.PROPERTY_OCCFEE].Header.Caption = OrderFields.CAPTION_OCCFEE;
                    band.Columns[OrderFields.PROPERTY_ORFFEE].Header.Caption = OrderFields.CAPTION_ORFFEE;
                    band.Columns["ISSwap"].Header.Caption = "IS Swap";
                    band.Columns["AUECModifiedDate"].Header.Caption = "AUEC Modified Date";
                    band.Columns["SwapParameters"].Header.Caption = "Swap Parameters";
                    band.Columns["CountryName"].Header.Caption = "Country";
                    band.Columns["OrderSide"].Header.Caption = "Order Side";
                    band.Columns["AvgPrice"].Header.Caption = "Avg. Price";
                    band.Columns["AssetName"].Header.Caption = "Asset";
                    band.Columns[CashManagementConstants.COLUMN_CURRENCYNAME].Header.Caption = "Currency";
                    band.Columns["CumQty"].Header.Caption = "Cum. Qty";
                    band.Columns["ContractMultiplier"].Header.Caption = "Contract Multiplier";
                    band.Columns["ProcessDate"].Header.Caption = "Process Date";
                    band.Columns["OriginalPurchaseDate"].Header.Caption = "Original Purchase Date";
                    band.Columns["SettlementDate"].Header.Caption = "Settlement Date";
                    band.Columns["ExpirationDate"].Header.Caption = "Expiration Date";
                    band.Columns["UnderlyingSymbol"].Header.Caption = "Underlying Symbol";
                    band.Columns["AccruedInterest"].Header.Caption = "Accrued Interest";
                    band.Columns["AUECLocalDate"].Header.Caption = "AUEC Local Date";
                    band.Columns["TaxLotQty"].Header.Caption = "Tax Lot Qty";

                    #endregion

                    GlobalGridSetting(band);
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
        /// Summaries the settings.
        /// </summary>
        /// <param name="e">The <see cref="InitializeLayoutEventArgs"/> instance containing the event data.</param>
        public static void SummarySettings(InitializeLayoutEventArgs e)
        {
            try
            {

                UltraGridBand band = null;
                if (e.Layout.Bands.Exists("TransactionEntries"))
                {
                    band = e.Layout.Bands["TransactionEntries"];
                }
                else
                {
                    band = e.Layout.Bands[0];
                }
                e.Layout.Override.RowSelectors = DefaultableBoolean.True;
                e.Layout.Override.RowSelectorHeaderStyle = RowSelectorHeaderStyle.ColumnChooserButton;
                e.Layout.ColumnChooserEnabled = DefaultableBoolean.True;


                SummarySettingsCollection summaries = band.Summaries;
                ColumnsCollection currentCollumnCollection = band.Columns;
                CashSummaryFactory summFactory = new CashSummaryFactory();

                //commented as it shouldnt be hide 
                // e.Layout.Override.GroupByColumnsHidden = DefaultableBoolean.True;
                e.Layout.Override.GroupBySummaryDisplayStyle = GroupBySummaryDisplayStyle.SummaryCells;
                e.Layout.Override.SummaryDisplayArea = SummaryDisplayAreas.InGroupByRows;
                e.Layout.Override.GroupByRowExpansionStyle = GroupByRowExpansionStyle.ExpansionIndicatorAndDoubleClick;

                //appearance31.BorderColor = System.Drawing.Color.Transparent;
                //appearance31.ForeColor = System.Drawing.Color.White;



                //e.Layout.Override.GroupBySummaryValueAppearance.BackColor = Color.LightGray;
                e.Layout.UseFixedHeaders = true;
                //e.Layout.Override.GroupBySummaryValueAppearance.ForeColor = Color.White;


                band.Override.HeaderPlacement = HeaderPlacement.FixedOnTop;

                //band.SortedColumns.Add("DR", false, false);
                //Modified By: Ishan Gandhi(06/10/2014)
                //Commented the following statement to remove default sorting by account side
                //JIRA Link: Dr. Account Side should be first by default(http://jira.nirvanasolutions.com:8080/browse/PRANA-5039)
                //band.SortedColumns.Add("EntryAccountSide", false, false);
                band.SortedColumns.Add("TransactionID", true, true);


                //transactionEntryBand.Override.RowSelectorHeaderStyle = RowSelectorHeaderStyle.None;               
                e.Layout.Override.SummaryFooterCaptionVisible = Infragistics.Win.DefaultableBoolean.False;

                //ultraGrid.Rows.ExpandAll(true);
                // ultraGrid.CreationFilter = new CreatingTextUIElement();


                #region code from Consolidation View

                e.Layout.Override.GroupByRowDescriptionMask = " ";
                e.Layout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
                e.Layout.GroupByBox.ButtonConnectorColor = System.Drawing.Color.Black;
                e.Layout.GroupByBox.ButtonConnectorStyle = Infragistics.Win.UIElementBorderStyle.Solid;
                e.Layout.GroupByBox.ShowBandLabels = ShowBandLabels.None;


                e.Layout.Override.GroupByRowPadding = 0;
                e.Layout.Override.GroupByRowSpacingAfter = 0;
                e.Layout.Override.GroupByRowSpacingBefore = 0;


                e.Layout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;

                e.Layout.RowConnectorColor = System.Drawing.Color.Transparent;

                #endregion



                SummarySettings transactionDatesummary = summaries.Add(currentCollumnCollection["TransactionDate"].Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcDate"), currentCollumnCollection["TransactionDate"], SummaryPosition.UseSummaryPositionColumn, null);
                transactionDatesummary.DisplayFormat = "{0:MM/dd/yyyy}";

                SummarySettings accountName = summaries.Add(currentCollumnCollection[CashManagementConstants.COLUMN_ACCOUNT].Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), currentCollumnCollection[CashManagementConstants.COLUMN_ACCOUNT], SummaryPosition.UseSummaryPositionColumn, null);
                accountName.DisplayFormat = "{0}";

                SummarySettings symbol = summaries.Add(currentCollumnCollection[CashManagementConstants.COLUMN_SYMBOL].Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), currentCollumnCollection[CashManagementConstants.COLUMN_SYMBOL], SummaryPosition.UseSummaryPositionColumn, null);
                symbol.DisplayFormat = "{0}";

                SummarySettings currencynamesummary = summaries.Add(currentCollumnCollection[CashManagementConstants.COLUMN_CURRENCYNAME].Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), currentCollumnCollection[CashManagementConstants.COLUMN_CURRENCYNAME], SummaryPosition.UseSummaryPositionColumn, null);
                currencynamesummary.DisplayFormat = "{0}";

                SummarySettings basecurrencynamesummary = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), currentCollumnCollection["BaseCurrencyName"], SummaryPosition.UseSummaryPositionColumn, null);
                basecurrencynamesummary.DisplayFormat = "{0}";

                SummarySettings subAcName = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), currentCollumnCollection["SubAcName"], SummaryPosition.UseSummaryPositionColumn, null);
                subAcName.DisplayFormat = "{0}";

                SummarySettings DRSumSummary = summaries.Add(currentCollumnCollection["DR"].Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), currentCollumnCollection["DR"], SummaryPosition.UseSummaryPositionColumn, null);//, CurrentCollumnCollection[CashManagementConstants.COLUMN_CURRENCYNAME]);
                //DRSumSummary.DisplayFormat = "{0}";
                DRSumSummary.DisplayFormat = "{0:#,###0.00}";

                SummarySettings CRSumSummary = summaries.Add(currentCollumnCollection["CR"].Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), currentCollumnCollection["CR"], SummaryPosition.UseSummaryPositionColumn, null);//, CurrentCollumnCollection[CashManagementConstants.COLUMN_CURRENCYNAME]);
                //CRSumSummary.DisplayFormat = "{0}";
                CRSumSummary.DisplayFormat = "{0:#,###0.00}";

                SummarySettings DRBaseSumSummary = summaries.Add(currentCollumnCollection["DRBase"].Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), currentCollumnCollection["DRBase"], SummaryPosition.UseSummaryPositionColumn, null);//, CurrentCollumnCollection["currencyname"]);
                //DRBaseSumSummary.DisplayFormat = "{0}";
                DRBaseSumSummary.DisplayFormat = "{0:#,###0.00}";

                SummarySettings CRBaseSumSummary = summaries.Add(currentCollumnCollection["CRBase"].Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), currentCollumnCollection["CRBase"], SummaryPosition.UseSummaryPositionColumn, null);//, CurrentCollumnCollection["currencyname"]);
                CRBaseSumSummary.DisplayFormat = "{0}";
                CRBaseSumSummary.DisplayFormat = "{0:#,###0.00}";

                SummarySettings FxRate = summaries.Add(currentCollumnCollection[CashManagementConstants.COLUMN_FXRATE].Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcNumText"), currentCollumnCollection[CashManagementConstants.COLUMN_FXRATE], SummaryPosition.UseSummaryPositionColumn, null);
                FxRate.DisplayFormat = "{0}";

                SummarySettings FxConversionMethodOperator = summaries.Add(currentCollumnCollection[CashManagementConstants.COLUMN_FXCONVERSIONMETHODOPERATOR].Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), currentCollumnCollection[CashManagementConstants.COLUMN_FXCONVERSIONMETHODOPERATOR], SummaryPosition.UseSummaryPositionColumn, null);
                FxConversionMethodOperator.DisplayFormat = "{0}";

                //SummarySettings TransactionIDsummary = Summaries.Add(SummaryType.Custom, new customTransactionIDSummary(), CurrentCollumnCollection["TransactionID"], SummaryPosition.UseSummaryPositionColumn, null);
                //TransactionIDsummary.DisplayFormat = "{0}";

                SummarySettings taxLotsummary = summaries.Add(currentCollumnCollection["TaxLotid"].Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), currentCollumnCollection["TaxLotid"], SummaryPosition.UseSummaryPositionColumn, null);
                taxLotsummary.DisplayFormat = "{0}";
                //UltraWinGridUtils.EnableFixedFilterRow(e);

                SummarySettings description = summaries.Add(currentCollumnCollection[CashManagementConstants.COLUMN_DESCRIPTION].Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), currentCollumnCollection[CashManagementConstants.COLUMN_DESCRIPTION], SummaryPosition.UseSummaryPositionColumn, null);
                description.DisplayFormat = "{0}";

                SummarySettings transactionSourceSummary = summaries.Add(currentCollumnCollection["TransactionSource"].Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcDisplayText"), currentCollumnCollection["TransactionSource"], SummaryPosition.UseSummaryPositionColumn, null);
                transactionSourceSummary.DisplayFormat = "{0}";

                //PRANA-9777
                SummarySettings entryDate = summaries.Add(currentCollumnCollection[CashManagementConstants.COLUMN_ENTRYDATE].Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcDate"), currentCollumnCollection[CashManagementConstants.COLUMN_ENTRYDATE], SummaryPosition.UseSummaryPositionColumn, null);
                entryDate.DisplayFormat = "{0:MM/dd/yyyy}";

                //PRANA-9777
                SummarySettings modifyDate = summaries.Add(currentCollumnCollection[CashManagementConstants.COLUMN_MODIFYDATE].Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcDate"), currentCollumnCollection[CashManagementConstants.COLUMN_MODIFYDATE], SummaryPosition.UseSummaryPositionColumn, null);
                modifyDate.DisplayFormat = "{0:MM/dd/yyyy}";

                //PRANA-9776
                SummarySettings userSummary = summaries.Add(currentCollumnCollection[CashManagementConstants.COLUMN_USERNAME].Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcDisplayText"), currentCollumnCollection[CashManagementConstants.COLUMN_USERNAME], SummaryPosition.UseSummaryPositionColumn, null);
                userSummary.DisplayFormat = "{0}";

                //Added by surendra Bisht
                if (!CustomThemeHelper.ApplyTheme)
                {

                    Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
                    appearance4.BackColor = System.Drawing.Color.Black;
                    appearance4.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
                    appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
                    appearance4.BorderColor = System.Drawing.Color.Black;
                    //appearance4.FontData.BoldAsString = "True";
                    appearance4.ForeColor = System.Drawing.Color.White;
                    e.Layout.Override.GroupByRowAppearance = appearance4;

                    Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
                    appearance5.TextVAlign = VAlign.Middle;
                    appearance5.ForeColor = Color.White;
                    appearance5.BorderColor = System.Drawing.Color.Transparent;
                    e.Layout.Override.GroupBySummaryValueAppearance = appearance5;

                    Infragistics.Win.Appearance appSummaryRow = new Infragistics.Win.Appearance();
                    appSummaryRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(76)))), ((int)(((byte)(122)))));
                    appSummaryRow.ForeColor = System.Drawing.Color.White;
                    appSummaryRow.TextHAlignAsString = "Right";
                    appSummaryRow.TextVAlignAsString = "Middle";
                    e.Layout.Override.SummaryValueAppearance = appSummaryRow;
                }
                else
                {

                    Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
                    appearance4.BackColor = System.Drawing.Color.Black;
                    appearance4.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
                    appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
                    appearance4.BorderColor = System.Drawing.Color.Black;
                    //appearance4.FontData.BoldAsString = "True";
                    appearance4.ForeColor = System.Drawing.Color.White;
                    e.Layout.Override.GroupByRowAppearance = appearance4;

                    Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
                    appearance5.TextVAlign = VAlign.Middle;
                    appearance5.ForeColor = Color.Black;
                    appearance5.BorderColor = System.Drawing.Color.Transparent;
                    e.Layout.Override.GroupBySummaryValueAppearance = appearance5;

                    Infragistics.Win.Appearance appSummaryRow = new Infragistics.Win.Appearance();
                    appSummaryRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(76)))), ((int)(((byte)(122)))));
                    appSummaryRow.ForeColor = System.Drawing.Color.White;
                    appSummaryRow.TextHAlignAsString = "Right";
                    appSummaryRow.TextVAlignAsString = "Middle";
                    e.Layout.Override.SummaryValueAppearance = appSummaryRow;

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
        /// Updates the column text alignment.
        /// </summary>
        /// <param name="columnsCollection">The columns collection.</param>
        internal static void UpdateColumnTextAlignment(ColumnsCollection columnsCollection)
        {
            try
            {
                columnsCollection.Cast<UltraGridColumn>().ToList().ForEach(col =>
                        {
                            switch (col.DataType.Name)
                            {
                                case "String":
                                case "DateTime":
                                case "Boolean":
                                    col.CellAppearance.TextHAlign = HAlign.Right;
                                    break;

                                case "Int32":
                                case "Int64":
                                case "Double":
                                case "Single":
                                case "Decimal":
                                    col.CellAppearance.TextHAlign = HAlign.Right;
                                    if (string.IsNullOrWhiteSpace(col.Format))
                                        col.Format = "#,###,###,###,##0.##############";
                                    break;

                                default:
                                    col.CellAppearance.TextHAlign = HAlign.Right;
                                    break;
                            }
                        });
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Updates the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="L"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="lsObj">The ls object.</param>
        internal static void UpdateList<T, L>(T obj, L lsObj)
        {
            try
            {
                CashActivity cashActivity = obj as CashActivity;
                GenericBindingList<CashActivity> lsToUpdate = lsObj as GenericBindingList<CashActivity>;
                if (cashActivity != null && lsToUpdate != null)
                {
                    if (cashActivity.ActivityState == ApplicationConstants.TaxLotState.Deleted && lsToUpdate.Contains(cashActivity))
                    {
                        lsToUpdate.Remove(cashActivity);
                        // Trading Entries Deletion for Publishing on CM
                        var itemToRemove = lsToUpdate.Where(x => (x.Symbol == cashActivity.Symbol) && (x.AccountID == cashActivity.AccountID) && (DateTime.Compare(x.Date.Date, cashActivity.Date.Date) >= 0) && (x.TransactionSource == CashTransactionType.Revaluation)).ToList();
                        for (int i = itemToRemove.Count - 1; i >= 0; i--)
                        {
                            lsToUpdate.Remove(itemToRemove[i]);
                        }
                    }
                    else if (cashActivity.ActivityState == ApplicationConstants.TaxLotState.Updated)
                    {
                        if (lsToUpdate.Contains(cashActivity))
                        {
                            lsToUpdate.UpdateItem(cashActivity);
                        }
                        else
                        {
                            lsToUpdate.Add(cashActivity);
                        }
                        //Revaluation Entries Deletion for Publishing on CM
                        var itemToRemove = lsToUpdate.Where(x => (x.Symbol == cashActivity.Symbol) && (x.AccountID == cashActivity.AccountID) && (DateTime.Compare(x.Date.Date, cashActivity.Date.Date) >= 0) && (x.TransactionSource == CashTransactionType.Revaluation)).ToList();
                        for (int i = itemToRemove.Count - 1; i >= 0; i--)
                        {
                            lsToUpdate.Remove(itemToRemove[i]);
                        }
                    }
                    else if (cashActivity.ActivityState == ApplicationConstants.TaxLotState.New && !lsToUpdate.Contains(cashActivity))
                        lsToUpdate.Add(cashActivity);
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

        #endregion Methods
    }
}
