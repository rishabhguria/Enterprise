using Infragistics.Win;
using Infragistics.Win.UltraWinCalcManager;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using Prana.Utilities.UI;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace Prana.Rebalancer.PercentTradingTool.Preferences
{
    public partial class MasterFundAccountGridControl : UserControl
    {
        public MasterFundAccountGridControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Bind Grid
        /// </summary>
        /// <param name="mfAccountPref"></param>
        public void BindGrid(BindingList<PTTMFAccountPref> mfAccountPref)
        {
            try
            {
                masterfundAccountGrid.DataSource = mfAccountPref;
                SetGridLayout();
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
        /// Save the Preference
        /// </summary>
        /// <param name="preferenceType"></param>
        /// <returns></returns>
        public BindingList<PTTMFAccountPref> Save(PTTPreferenceType preferenceType)
        {
            try
            {
                masterfundAccountGrid.UpdateData();
                BindingList<PTTMFAccountPref> mfAccPrefBindingList = (BindingList<PTTMFAccountPref>)masterfundAccountGrid.DataSource;
                mfAccPrefBindingList.Cast<PTTMFAccountPref>().ToList().ForEach(x => x.PreferenceType = preferenceType);
                bool exists = mfAccPrefBindingList.Cast<PTTMFAccountPref>().ToList().Exists(x => x.TotalPercentage != 100 && x.TotalPercentage > 0);
                if (exists)
                {
                    MessageBox.Show(PTTConstants.MSG_INVALID_TOTALPERCENTAGE, PTTConstants.CAP_PTTMODULE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return null;
                }
                else
                {
                    return mfAccPrefBindingList;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                return null;
            }
        }

        /// <summary>
        /// Set Grid Layout
        /// </summary>
        private void SetGridLayout()
        {
            try
            {
                masterfundAccountGrid.DisplayLayout.Bands[0].Columns[PTTConstants.COL_MASTERFUNDNAME].CellActivation = Activation.NoEdit;
                masterfundAccountGrid.DisplayLayout.Bands[0].Columns[PTTConstants.COL_MASTERFUNDNAME].Header.Caption = PTTConstants.CAP_MASTERFUND;
                masterfundAccountGrid.DisplayLayout.Bands[0].Columns[PTTConstants.COL_TOTALPERCENTAGE].Header.Caption = PTTConstants.CAP_TOTALPERCENTAGE;
                masterfundAccountGrid.DisplayLayout.Bands[0].Columns[PTTConstants.COL_TOTALPERCENTAGE].CellActivation = Activation.NoEdit;
                masterfundAccountGrid.DisplayLayout.Bands[0].Columns[PTTConstants.COL_IS_PRORATA_PERCENTAGE].Header.Caption = PTTConstants.CAP_USE_PRORATA_PREF;
                masterfundAccountGrid.DisplayLayout.Bands[0].Columns[PTTConstants.COL_PREFERENCETYPE].Hidden = true;
                masterfundAccountGrid.DisplayLayout.Bands[1].Columns[PTTConstants.COL_ACCOUNTNAME].Header.Caption = PTTConstants.CAP_ACCOUNT;
                masterfundAccountGrid.DisplayLayout.Bands[1].Columns[PTTConstants.COL_ACCOUNTNAME].CellActivation = Activation.NoEdit;
                masterfundAccountGrid.DisplayLayout.Bands[1].Columns[PTTConstants.COL_ACCOUNTFACTOR].Hidden = true;
                masterfundAccountGrid.DisplayLayout.Bands[1].Columns[PTTConstants.COL_PERCENTAGE].Format = "#,##,###0.000000";
                masterfundAccountGrid.DisplayLayout.Bands[1].Columns[PTTConstants.COL_PERCENTAGE].MaxValue = 100;
                masterfundAccountGrid.DisplayLayout.Bands[1].Columns[PTTConstants.COL_PERCENTAGE].MinValue = 0;

                masterfundAccountGrid.DisplayLayout.Bands[1].Override.AllowUpdate = DefaultableBoolean.True;
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
        /// InitializeLayout of the masterfundAccount Grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterfundAccountGrid_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                UltraCalcManager calcManager;
                calcManager = new UltraCalcManager(this.Container);
                e.Layout.Grid.CalcManager = calcManager;
                e.Layout.Bands[0].Columns[PTTConstants.COL_TOTALPERCENTAGE].Formula = "Sum([AccountWisePercentage/Percentage])";
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
        /// masterfundAccountGrid BeforeCustomRowFilterDialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterfundAccountGrid_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            try
            {
                (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
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
        /// masterfundAccountGrid BeforeExitEditMode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterfundAccountGrid_BeforeExitEditMode(object sender, Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventArgs e)
        {
            try
            {
                UltraGrid grid = (UltraGrid)sender;
                UltraGridCell activeCell = grid.ActiveCell;
                float cellText = 0;
                if (!grid.ActiveCell.Column.Key.Equals(PTTConstants.COL_IS_PRORATA_PERCENTAGE))
                {
                    if (float.TryParse(Convert.ToString(activeCell.Text), out cellText))
                    {
                        if (cellText > 100)
                        {
                            activeCell.Value = 100;
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        activeCell.Value = 0;
                        e.Cancel = true;
                    }
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


    }
}
