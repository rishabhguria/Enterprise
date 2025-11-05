using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.CashManagement.Classes;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.UI;
using Prana.Utilities.UI.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Windows.Forms;

namespace Prana.CashManagement.Controls
{
    public partial class ctrlDataGrid : UserControl
    {

        public event EventHandler<EventArgs<CashManagementLayout>> CashLayout;

        public ctrlDataGrid()
        {
            InitializeComponent();
        }

        private string _keyForXML = null;
        public string KeyForXML
        {
            get { return _keyForXML; }
            set { _keyForXML = value; }
        }

        private void ctrlDataGrid_Load(object sender, EventArgs e)
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(this, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_CASH_MANAGEMENT);
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

        private void ultraGrid_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
            try
            {
                HelperClass.RowColorSettings(e);
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

        private void ultraGrid_BeforeColumnChooserDisplayed(object sender, Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                if (ultraGrid.DataSource != null)
                {
                    (this.FindForm()).AddCustomColumnChooser(this.ultraGrid);
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

        void ultraGrid_BeforeRowFilterDropDown(object sender, Infragistics.Win.UltraWinGrid.BeforeRowFilterDropDownEventArgs e)
        {
            try
            {
                if (e.Column.Key.Equals(CashManagementConstants.COLUMN_DATE))
                {
                    e.ValueList.ValueListItems.Insert(4, "(Today)", "(Today)");
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

        void ultraGrid_AfterRowFilterChanged(object sender, Infragistics.Win.UltraWinGrid.AfterRowFilterChangedEventArgs e)
        {
            try
            {
                if ((e.Column.Key.Equals(CashManagementConstants.COLUMN_DATE)) && e.NewColumnFilter.FilterConditions != null && e.NewColumnFilter.FilterConditions.Count == 1 && e.NewColumnFilter.FilterConditions[0].CompareValue.Equals("(Today)"))
                {
                    ultraGrid.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Clear();
                    ultraGrid.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Add(FilterComparisionOperator.StartsWith, DateTime.Now.Date.ToString(DateTimeConstants.DateformatForClosing));
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

        private void saveLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveGridLayout saveGridLayout = new SaveGridLayout();
                CashManagementLayout cashManagementLayout = saveGridLayout.GetLayout(ultraGrid, KeyForXML.Split('_')[0]);
                CashPreferenceManager.GetInstance().SetCashGridLayout(cashManagementLayout, KeyForXML);
                if (CashLayout != null)
                {
                    CashLayout(this, new EventArgs<CashManagementLayout>(cashManagementLayout));
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

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ExportToExcelHelper.ExportToExcel(ultraGrid);
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

        public void HideContextMenu()
        {
            try
            {
                contextMenuStrip1.Items[0].Visible = false;
                contextMenuStrip1.Items[1].Visible = false;
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

        private void ultraGrid_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }

    }
}
