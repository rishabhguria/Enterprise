using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.CashManagement;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Drawing;

namespace Prana.ShortLocate.Classes
{
    class Summary
    {
        /// Activities the summary settings.
        /// </summary>
        /// <param name="e">The <see cref="InitializeLayoutEventArgs"/> instance containing the event data.</param>
        public static void ShortLocateSummarySetting(InitializeLayoutEventArgs e)
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

                // e.Layout.Override.GroupBySummaryValueAppearance.BackColor = Color.LightGray;
                // e.Layout.UseFixedHeaders = true;
                // e.Layout.Override.GroupBySummaryValueAppearance.ForeColor = Color.White;


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

                AddColumnIntoSummary(summaries, currentCollumnCollection, summFactory);

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

        private static void AddColumnIntoSummary(SummarySettingsCollection summaries, ColumnsCollection currentCollumnCollection, CashSummaryFactory summFactory)
        {
            try
            {
                SummarySettings Ticker = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), currentCollumnCollection[ShortLocateConstants.COL_Ticker], SummaryPosition.UseSummaryPositionColumn, null);
                Ticker.DisplayFormat = "{0}";

                SummarySettings Broker = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), currentCollumnCollection[ShortLocateConstants.COL_Broker], SummaryPosition.UseSummaryPositionColumn, null);
                Broker.DisplayFormat = "{0}";

                SummarySettings LastPx = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), currentCollumnCollection[ShortLocateConstants.COL_LastPx], SummaryPosition.UseSummaryPositionColumn, null);
                LastPx.DisplayFormat = "{0:#,###0.00}";

                SummarySettings TradeQty = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), currentCollumnCollection[ShortLocateConstants.COL_TradeQuantity], SummaryPosition.UseSummaryPositionColumn, null);
                TradeQty.DisplayFormat = "{0:#,###0.00}";

                //SummarySettings ClientMasterfund = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), currentCollumnCollection[ShortLocateConstants.COL_ClientMasterfund], SummaryPosition.UseSummaryPositionColumn, null);
                //ClientMasterfund.DisplayFormat = "{0}";

                SummarySettings BorrowSharesAvailable = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), currentCollumnCollection[ShortLocateConstants.COL_BorrowSharesAvailable], SummaryPosition.UseSummaryPositionColumn, null);
                BorrowSharesAvailable.DisplayFormat = "{0:#,###0.00}";


                SummarySettings BorrowRate = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), currentCollumnCollection[ShortLocateConstants.COL_BorrowRate], SummaryPosition.UseSummaryPositionColumn, null);
                BorrowRate.DisplayFormat = "{0:#,###0.00}";

                SummarySettings TotalBorrowAmount = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), currentCollumnCollection[ShortLocateConstants.COL_TotalBorrowAmount], SummaryPosition.UseSummaryPositionColumn, null);
                TotalBorrowAmount.DisplayFormat = "{0:#,###0.00}";


                SummarySettings BorrowerId = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcDisplayText"), currentCollumnCollection[ShortLocateConstants.COL_BorrowerId], SummaryPosition.UseSummaryPositionColumn, null);
                BorrowerId.DisplayFormat = "{0}";

                SummarySettings BorrowedShare = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), currentCollumnCollection[ShortLocateConstants.COL_BorrowedShare], SummaryPosition.UseSummaryPositionColumn, null);
                BorrowedShare.DisplayFormat = "{0:#,###0.00}";

                SummarySettings BorrowedRate = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), currentCollumnCollection[ShortLocateConstants.COL_BorrowedRate], SummaryPosition.UseSummaryPositionColumn, null);
                BorrowedRate.DisplayFormat = "{0:#,###0.00}";

                SummarySettings TotalBorrowedAmount = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), currentCollumnCollection[ShortLocateConstants.COL_TotalBorrowedAmount], SummaryPosition.UseSummaryPositionColumn, null);
                TotalBorrowedAmount.DisplayFormat = "{0:#,###0.00}";


                SummarySettings SODBorrowshareAvailable = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), currentCollumnCollection[ShortLocateConstants.COL_SODBorrowshareAvailable], SummaryPosition.UseSummaryPositionColumn, null);
                SODBorrowshareAvailable.DisplayFormat = "{0:#,###0.00}";

                SummarySettings SODBorrowRate = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), currentCollumnCollection[ShortLocateConstants.COL_SODBorrowRate], SummaryPosition.UseSummaryPositionColumn, null);
                SODBorrowRate.DisplayFormat = "{0:#,###0.00}";


                SummarySettings StatusSource = summaries.Add(SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), currentCollumnCollection[ShortLocateConstants.COL_StatusSource], SummaryPosition.UseSummaryPositionColumn, null);
                StatusSource.DisplayFormat = "{0}";
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
