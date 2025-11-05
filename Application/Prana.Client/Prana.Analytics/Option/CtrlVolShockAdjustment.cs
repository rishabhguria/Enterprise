using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Data;
using System.Windows.Forms;

namespace Prana.Analytics
{
    public partial class CtrlVolShockAdjustment : UserControl
    {
        //string _viewID = string.Empty;
        string _viewName = string.Empty;
        bool _isAlreadyStarted = false;

        public CtrlVolShockAdjustment()
        {
            try
            {
                InitializeComponent();
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

        public void SetUp(string stepAnalViewName)
        {
            try
            {
                if (!_isAlreadyStarted)
                {
                    // _viewID = stepAnalViewID;
                    _viewName = stepAnalViewName;
                    SetPreferences();
                    BindGrid();
                    _isAlreadyStarted = true;
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

        private void SetPreferences()
        {
            try
            {
                StepAnalysisPref preferences = RiskPreferenceManager.RiskPrefernece.GetStepAnalViewPreferences(_viewName);
                if (preferences != null)
                {
                    chkBoxUseAdjustment.Checked = preferences.UseVolShockAdjustment;
                    chkBoxUseAdjustment_CheckedChanged(null, null);
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

        private void BindGrid()
        {
            try
            {
                DataTable dtVolShockfactor = null;
                StepAnalysisPref preferences = RiskPreferenceManager.RiskPrefernece.GetStepAnalViewPreferences(_viewName);
                if (preferences != null)
                {
                    dtVolShockfactor = preferences.DtVolShockFactor;

                    if (dtVolShockfactor != null)
                    {
                        grdVolShockfactor.DataSource = dtVolShockfactor;
                        grdVolShockfactor.DataBind();
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

        private void grdVolShockfactor_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                UltraGridLayout gridLayout = grdVolShockfactor.DisplayLayout;

                UltraGridColumn columnFromDaysToExp = gridLayout.Bands[0].Columns["FromDaysToExp"];
                columnFromDaysToExp.Header.Caption = "From DaysToExp";
                columnFromDaysToExp.CellClickAction = CellClickAction.RowSelect;

                UltraGridColumn columnToDaysToExp = gridLayout.Bands[0].Columns["ToDaysToExp"];
                columnToDaysToExp.Header.Caption = "To DaysToExp";
                columnToDaysToExp.NullText = "inf";
                columnToDaysToExp.CellClickAction = CellClickAction.RowSelect;

                UltraGridColumn columnVolShockFactor = gridLayout.Bands[0].Columns["VolShockAdjFactor"];
                columnVolShockFactor.Header.Caption = "Applied Factor";

                // DataTable dt = grdVolShockfactor.DataSource as DataTable;
                gridLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.False;
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

        public void SavePreferences()
        {
            try
            {
                RiskPrefernece riskpreference = RiskPreferenceManager.RiskPrefernece;
                StepAnalysisPref StepAnalPreferences = riskpreference.GetStepAnalViewPreferences(_viewName);
                DataTable dtVolShockFactor = grdVolShockfactor.DataSource as DataTable;
                if (dtVolShockFactor != null)
                {
                    // int viewID = int.Parse(_viewID);
                    StepAnalPreferences.DtVolShockFactor = dtVolShockFactor;
                    riskpreference.UpdateStepAnalPrefDict(_viewName, StepAnalPreferences);

                }
                StepAnalPreferences.UseVolShockAdjustment = chkBoxUseAdjustment.Checked;
                RiskPreferenceManager.SavePreferences(riskpreference);
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

        private void chkBoxUseAdjustment_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkBoxUseAdjustment.Checked)
                {
                    grdVolShockfactor.Enabled = true;
                }
                else
                {
                    grdVolShockfactor.Enabled = false;
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
    }
}
