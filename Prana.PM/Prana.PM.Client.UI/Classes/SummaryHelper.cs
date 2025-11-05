using Infragistics.Win.UltraWinGrid;
using Prana.LogManager;
using System;

namespace Prana.PM.Client.UI
{
    class SummaryHelper
    {
        public static void AddColumnSummary(Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e, string ColumnName, string columnVattedFrom, SummaryType summaryType)
        {
            try
            {
                if (e.Layout.Bands.Count > 0)
                {
                    if (summaryType == SummaryType.Sum)
                    {
                        e.Layout.Bands[0].Summaries.Add(SummaryType.Sum, e.Layout.Bands[0].Columns[ColumnName]).DisplayFormat = "{0:#,#0.0000}";
                    }
                    else
                    {
                        string calculationColumn = ColumnName + "CalculationColumn";
                        string summaryColumn = ColumnName + "Summary";
                        UltraGridColumn column = e.Layout.Bands[0].Columns.Add(calculationColumn);
                        column.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                        column.Hidden = true;
                        column.DataType = typeof(double);
                        if (summaryType == SummaryType.Average)
                        {
                            column.Formula = "[" + ColumnName + "] * [" + columnVattedFrom + "]/sum([" + columnVattedFrom + "(*)])";
                        }
                        if (summaryType == SummaryType.Custom)
                        {
                            column.Formula = "[" + ColumnName + "] * [" + columnVattedFrom + "]";
                        }

                        column.Hidden = true;

                        e.Layout.Bands[0].Summaries.Add(summaryColumn, SummaryType.Sum, e.Layout.Bands[0].Columns[ColumnName]).DisplayFormat = "{0:#,#0.0000}";
                        e.Layout.Bands[0].Summaries[summaryColumn].Formula = "sum([" + calculationColumn + "])";
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
    }
}
