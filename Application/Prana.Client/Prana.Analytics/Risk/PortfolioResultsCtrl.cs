using Infragistics.Win.UltraWinGrid;
using Prana.LogManager;
using Prana.Utilities.UI;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.Analytics
{
    public partial class PortfolioResultsCtrl : UserControl
    {
        DataTable dt = null;
        public PortfolioResultsCtrl()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void SetData(DataTable dtPortfolio)
        {
            try
            {
                dt = dtPortfolio;
                Refresh();
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public new void Refresh()
        {
            try
            {
                // grdData.DataSource = null;
                grdData.DataSource = dt;
                grdData.DataBind();
                SetColumnFormatting(grdData.DisplayLayout.Bands[0].Columns);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_SHOWONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetColumnFormatting(ColumnsCollection columns)
        {
            try
            {
                if (columns.Exists("Beta"))
                {
                    columns["Beta"].Format = "#,#.0000";
                }
                if (columns.Exists("Correlation"))
                {
                    columns["Correlation"].Format = "#,0.0000";
                }
                if (columns.Exists("Risk"))
                {
                    columns["Risk"].Format = "#,#";
                }
                if (columns.Exists("Percentage Change in Portfolio Value"))
                {
                    columns["Percentage Change in Portfolio Value"].Format = "#,0.00";
                }
                if (columns.Exists("Old Portfolio Value"))
                {
                    columns["Old Portfolio Value"].Format = "#,#.00";
                }
                if (columns.Exists("New Portfolio Value"))
                {
                    columns["New Portfolio Value"].Format = "#,#.00";
                }
                if (columns.Exists("Old BenchMark Value"))
                {
                    columns["Old BenchMark Value"].Format = "#,#.00";
                }
                if (columns.Exists("New BenchMark Value"))
                {
                    columns["New BenchMark Value"].Format = "#,#.00";
                }
                if (columns.Exists("StandardDeviation"))
                {
                    columns["StandardDeviation"].Header.Caption = "Standard Deviation";
                    columns["StandardDeviation"].Format = "#,#";
                }
                if (columns.Exists("ComponentRisk"))
                {
                    columns["ComponentRisk"].Hidden = true;
                    columns["ComponentRisk"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                }
                if (columns.Exists("P&L Impact"))
                {
                    columns["P&L Impact"].Format = "#,#.00";
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdData_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdData);
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void SetGridFonts(Font newFont)
        {
            try
            {
                grdData.Font = newFont;
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdData_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (dt != null)
                {
                    dt.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }
}
