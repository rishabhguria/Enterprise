using Infragistics.Win.UltraWinGrid;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.UI;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.Tools
{
    public partial class ZeroPositionAlertForm : Form, IPluggableTools
    {
        /// <summary>
        /// modified by: sachin mishra,02 Feb 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        public ZeroPositionAlertForm()
        {
            try
            {
                InitializeComponent();
                this.Location = new Point(0, 0);
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

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                RefreshZeroAlertData();
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
        private void grdZeroPosAlert_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                UltraGridBand band = grdZeroPosAlert.DisplayLayout.Bands[0];
                band.Columns["Symbol"].Header.VisiblePosition = 1;
                band.Columns["ZeroPositionFrequnecy"].Header.VisiblePosition = 2;
                band.Columns["ZeroPositionFrequnecy"].Header.Caption = "Zero Position Frequency";
                band.Columns["NetQuantity"].Header.VisiblePosition = 3;
                band.Columns["NetQuantity"].Header.Caption = "Net Quantity";
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

        Dictionary<string, ZeroPositionAlert> _dictZeroPosAlert = new Dictionary<string, ZeroPositionAlert>();
        /// <summary>
        ///modified by: sachin mishra 02 Feb 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        private void RefreshZeroAlertData()
        {
            try
            {
                DataSet ds = PranaDataManager.GetMinimalDataForGroup(DateTime.Today);
                DataTable dtLastDay = ds.Tables[0];
                DataTable dtDay = ds.Tables[1];
                SetStartingPositionForDay(dtLastDay);
                CalculateFrequencyOfZeroCrossing(dtDay);
                grdZeroPosAlert.DataSource = GetNonZeroPosAlertList();
                grdZeroPosAlert.DataBind();
                _dictZeroPosAlert.Clear();
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
        ///modified by: sachin mishra 02 Feb 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <param name="dtLastDay"></param>
        private void SetStartingPositionForDay(DataTable dtLastDay)
        {
            try
            {
                foreach (DataRow row in dtLastDay.Rows)
                {
                    string symbol = row["Symbol"].ToString();
                    double quantity = Double.Parse(row["Quantity"].ToString());

                    if (quantity != 0)
                    {
                        if (!_dictZeroPosAlert.ContainsKey(symbol))
                        {
                            ZeroPositionAlert zeroPosAlert = new ZeroPositionAlert();
                            zeroPosAlert.Symbol = symbol;
                            zeroPosAlert.NetQuantity = quantity;
                            _dictZeroPosAlert.Add(symbol, zeroPosAlert);
                        }
                        else
                        {
                            ZeroPositionAlert zeroPosAlert = _dictZeroPosAlert[symbol];
                            //double oldQty = zeroPosAlert.NetQuantity;
                            zeroPosAlert.NetQuantity += quantity;
                        }
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
        ///modified by: sachin mishra 02 Feb 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <param name="dtDay"></param>
        private void CalculateFrequencyOfZeroCrossing(DataTable dtDay)
        {
            try
            {
                foreach (DataRow row in dtDay.Rows)
                {
                    string symbol = row["Symbol"].ToString();
                    double quantity = Double.Parse(row["Quantity"].ToString());

                    if (quantity != 0)
                    {
                        if (!_dictZeroPosAlert.ContainsKey(symbol))
                        {
                            ZeroPositionAlert zeroPosAlert = new ZeroPositionAlert();
                            zeroPosAlert.Symbol = symbol;
                            zeroPosAlert.NetQuantity = quantity;
                            _dictZeroPosAlert.Add(symbol, zeroPosAlert);
                        }
                        else
                        {
                            ZeroPositionAlert zeroPosAlert = _dictZeroPosAlert[symbol];
                            double oldQty = zeroPosAlert.NetQuantity;
                            zeroPosAlert.NetQuantity += quantity;
                            if (zeroPosAlert.NetQuantity == 0 || oldQty * zeroPosAlert.NetQuantity < 0)
                            {
                                zeroPosAlert.ZeroPositionFrequnecy++;
                            }
                        }
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
        ///modified by: sachin mishra 02 Feb 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <returns></returns>
        private List<ZeroPositionAlert> GetNonZeroPosAlertList()
        {
            List<ZeroPositionAlert> zeroPosAlertList = new List<ZeroPositionAlert>();

            try
            {
                foreach (KeyValuePair<string, ZeroPositionAlert> zeroPos in _dictZeroPosAlert)
                {
                    if (zeroPos.Value.ZeroPositionFrequnecy > 0)
                    {
                        zeroPosAlertList.Add(zeroPos.Value);
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
            return zeroPosAlertList;
        }

        #region IPluggableTools Members
        /// <summary>
        /// modified by: sachin mishra,02 Feb 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        public void SetUP()
        {
            try
            {
                RefreshZeroAlertData();
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
        public Form Reference()
        {
            return this;
        }
        public event EventHandler PluggableToolsClosed;

        public ISecurityMasterServices SecurityMaster
        {
            set {; }
        }
        public IPostTradeServices PostTradeServices
        {
            set {; }
        }
        public IPricingAnalysis PricingAnalysis
        {
            set {; }
        }
        /// <summary>
        /// modified by: sachin mishra,02 Feb 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZeroPositionAlertForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (PluggableToolsClosed != null)
                {
                    PluggableToolsClosed(this, EventArgs.Empty);
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

        #endregion

        private void ZeroPositionAlertForm_Load(object sender, EventArgs e)
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(sender as Form, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_ZERO_POSITION_ALERT);
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
                }
                if (!CustomThemeHelper.IsDesignMode() && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
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

        private void SetButtonsColor()
        {
            try
            {
                btnRefresh.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnRefresh.ForeColor = System.Drawing.Color.White;
                btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnRefresh.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnRefresh.UseAppStyling = false;
                btnRefresh.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        private void grdZeroPosAlert_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {

                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdZeroPosAlert);
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

        private void grdZeroPosAlert_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }
    }
}