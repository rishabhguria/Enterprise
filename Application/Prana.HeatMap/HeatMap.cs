using Infragistics.Win.UltraWinToolbars;
using Prana.Global;
using Prana.HeatMap.BLL;
using Prana.HeatMap.ChildWindows;
using Prana.HeatMap.Enums;
using Prana.HeatMap.EventArguments;
using Prana.HeatMapControls.EventArguments;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Prana.HeatMap
{
    public partial class HeatMap : Form, IPluggableTools
    {
        public HeatMap()
        {
            try
            {
                InitializeComponent();
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
            ultraPanel2.ClientArea.VerticalScroll.Enabled = true;
        }

        /// <summary>
        /// initialise the manager and wire the events
        /// </summary>
        public void SetUP()
        {
            try
            {
                HeatMapManager.GetInstance().Initialise();
                HeatMapManager.GetInstance().updateData += HeatMap_updateData;
                HeatMapManager.GetInstance().UpdateConnectionStatus += HeatMap_UpdateConnectionStatus;

                HeatMapManager.GetInstance().updateStatusBarHandler += HeatMap_updateStatusBarHandler;
                heatMapControl1.DrillDown += heatMapControl1_drillDown;
                heatMapControl1.DrillUp += heatMapControl1_drillUp;

                //cmbbxHeatSelector.DataSource = EnumHelper.ConvertEnumForBindingWithCaption(typeof(Heats));
                //cmbbxHeatSelector.DataBind();
                //cmbbxHeatSelector.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
                //cmbbxHeatSelector.Value = Heats.PnL.ToString();
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
        /// Update Status Bar Handler
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">StatusBar EventArgs</param>
        void HeatMap_updateStatusBarHandler(object sender, BusinessObjects.Compliance.EventArguments.StatusBarEventArgs e)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    MethodInvoker del = delegate { HeatMap_updateStatusBarHandler(sender, e); };
                    this.BeginInvoke(del);
                }
                else
                {
                    UpdataStausBar(e.Status);
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
        /// Update Status Bar Text on UI
        /// </summary>
        /// <param name="status"></param>
        private void UpdataStausBar(string status)
        {
            try
            {
                ultraStatusBar1.Text = status;
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
        /// Update the connection status on UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void HeatMap_UpdateConnectionStatus(object sender, EventArgs<Boolean> e)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    MethodInvoker del = delegate { HeatMap_UpdateConnectionStatus(sender, e); };
                    this.BeginInvoke(del);
                }
                else
                {
                    if (e.Value)
                    {
                        esperCon.Image = HeatMapResources.green;
                        //this.ultraStatusBar1.SetStatusBarText(this.esperCon, "Calculation Engine Connected");
                        //this.ultraStatusBar1.SetStatusBarText(this.ultraLabelEsperStatus, "Calculation Engine Connected");

                        //Updating status bar
                        UpdataStausBar("Calculation Engine Connected");
                    }
                    else
                    {
                        esperCon.Image = HeatMapResources.red;
                        //this.ultraStatusBar1.SetStatusBarText(this.esperCon, "Calculation Engine Disconnected");
                        //this.ultraStatusBar1.SetStatusBarText(this.ultraLabelEsperStatus, "Calculation Engine Disconnected");

                        //Updating status bar
                        UpdataStausBar("Calculation Engine Disconnected");
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

        void heatMapControl1_drillUp(object sender, EventArgs e)
        {
            try
            {
                HeatMapManager.GetInstance().DrillUp();
                ultraLabelGrouping.Text = HeatMapManager.GetInstance().GetGroupingText();
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

        void heatMapControl1_drillDown(object sender, EventArgs<String> e)
        {
            try
            {
                HeatMapManager.GetInstance().DrillDown(e.Value);
                ultraLabelGrouping.Text = HeatMapManager.GetInstance().GetGroupingText();
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
            try
            {
                return this;
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
                return null;
            }
        }

        public event EventHandler PluggableToolsClosed;

        public ISecurityMasterServices SecurityMaster
        {
            set { }
        }

        public IPostTradeServices PostTradeServices
        {
            set { }
        }

        public IPricingAnalysis PricingAnalysis
        {
            set { }
        }

        /// <summary>
        /// Heat Map Form Closed
        /// </summary>
        /// <param name="sender">send</param>
        /// <param name="e">Form Closed EventArgs</param>
        private void HeatMap_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                HeatMapManager.GetInstance().updateData += HeatMap_updateData;

                if (PluggableToolsClosed != null)
                    PluggableToolsClosed(this, null);
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
        /// Heat Map Form Load
        /// </summary>
        /// <param name="sender">send</param>
        /// <param name="e">EventArgs</param>
        private void HeatMap_Load(object sender, EventArgs e)
        {
            try
            {
                foreach (Heats heat in Enum.GetValues(typeof(Heats)))
                {
                    ButtonTool menuItem1 = new ButtonTool(heat.ToString());
                    ultraToolbarsManager1.Tools.Add(menuItem1);
                    ((PopupMenuTool)this.ultraToolbarsManager1.Tools["Heats"]).Tools.AddTool(heat.ToString());
                    ((PopupMenuTool)this.ultraToolbarsManager1.Tools["Heats"]).Tools[heat.ToString()].SharedProps.Caption = heat.ToString();
                    if (HeatMapManager.GetInstance().GetCurrentHeat() == heat)
                        ((PopupMenuTool)this.ultraToolbarsManager1.Tools["Heats"]).Tools[heat.ToString()].CustomizedImage = HeatMapResources.check;
                }
                HandleTheme();
                ultraLabelGrouping.Text = HeatMapManager.GetInstance().GetGroupingText();
                esperCon.Image = HeatMapManager.GetInstance().IsEsperConnected() ? HeatMapResources.green : HeatMapResources.red;
                DataTable dt = HeatMapManager.GetInstance().GetLatestData();

                if (dt != null)
                {
                    heatMapControl1.SetData(dt);
                    //Updating status bar
                    UpdataStausBar("Data Loaded");
                }

                // UpdataStausBar("Ready");

                groupingTool1.SetAttributes(new List<String>(Enum.GetNames(typeof(GroupingAttributes))));
                if (HeatMapManager.GetInstance().GetGrouping() != null)
                {
                    groupingTool1.ApplyGroup(HeatMapManager.GetInstance().GetGrouping());
                }
                else
                    //Updating status bar
                    UpdataStausBar("Please select grouping");
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
        /// Handels the Prana theme
        /// </summary>
        private void HandleTheme()
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_HEAT_MAP);
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
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

        #region RamdomDataGeneratorToBeDeleted
        Random rand = new Random((int)DateTime.Now.Ticks);
        private DataTable GetDemoData(int numberOfValues)
        {
            try
            {
                if (numberOfValues <= 0)
                {
                    throw new ArgumentException("numberOfValues needs to be greater than 0.", "numberofValues");
                }

                DataTable dt = new DataTable();
                dt.Columns.Add("Symbol", typeof(string));
                for (int i = 0; i < numberOfValues; i++)
                {
                    dt.Columns.Add("Value" + (i + 1), typeof(double));
                }

                DataRow dr;
                for (int i = 0; i < 100; i++)
                {
                    dr = dt.NewRow();
                    dr[0] = GenerateSymbol();
                    for (int j = numberOfValues; j > 0; j--)
                    {
                        dr[j] = rand.NextDouble() * (i % 2 == 0 ? 1 : -1);
                    }

                    dt.Rows.Add(dr);
                }
                return dt;
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
                return null;
            }
        }
        private string GenerateSymbol()
        {
            try
            {
                return string.Empty + (Char)rand.Next(65, 91) + (Char)rand.Next(65, 91) + (Char)rand.Next(65, 91);
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
                return null;
            }
        }
        #endregion

        private void HeatMap_Resize(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Filter Data
        /// </summary>
        private void FilterData()
        {
            try
            {
                HeatMapFilter filterWindow = new HeatMapFilter();
                filterWindow.ShowDialog();
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
        /// Update the data on the control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void HeatMap_updateData(object sender, UpdateDataEventArgs e)
        {
            try
            {
                heatMapControl1.SetData(e.Data);
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
        /// Oen the Change Grouping UI
        /// </summary>
        //private void ChangeGrouping()
        //{
        //    try
        //    {
        //        HeatMapGrouping groupingWindow = new HeatMapGrouping();
        //        groupingWindow.SetAttributes(new List<String>(Enum.GetNames(typeof(GroupingAttributes))));
        //        if (HeatMapManager.GetInstance().GetGrouping() != null)
        //            groupingWindow.ApplyGroup(HeatMapManager.GetInstance().GetGrouping());

        //        groupingWindow.ShowDialog();
        //        List<String> grouping = groupingWindow.GetSortSettings();

        //        HeatMapManager.GetInstance().SetGrouping(grouping);
        //        ultraLabelGrouping.Text = HeatMapManager.GetInstance().GetGroupingText();
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        /// <summary>
        /// Ultra Tool Bar Menu Items Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">ToolClickEventArgs</param>
        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            try
            {
                //TODO:add switch case instead of if else
                if (e.Tool.Key == "filterData")
                {
                    FilterData();
                }
                //else if (e.Tool.Key == "changeGrouping")
                //{
                //    ChangeGrouping();
                //}
                else if (e.Tool.Key == Heats.Exposure.ToString())
                {
                    ((PopupMenuTool)this.ultraToolbarsManager1.Tools["Heats"]).Tools[Heats.PnL.ToString()].CustomizedImage = null;
                    e.Tool.CustomizedImage = HeatMapResources.check;
                    HeatMapManager.GetInstance().SetCurrentHeat((Heats)Enum.Parse(typeof(Heats), Heats.Exposure.ToString()));
                }
                else if (e.Tool.Key == Heats.PnL.ToString())
                {
                    ((PopupMenuTool)this.ultraToolbarsManager1.Tools["Heats"]).Tools[Heats.Exposure.ToString()].CustomizedImage = null;
                    e.Tool.CustomizedImage = HeatMapResources.check;
                    HeatMapManager.GetInstance().SetCurrentHeat((Heats)Enum.Parse(typeof(Heats), Heats.PnL.ToString()));
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
        /// groupingTool1_groupingChanged event for grouping change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void groupingTool1_groupingChanged(object sender, GroupingChangedEventArgs e)
        {
            try
            {
                HeatMapManager.GetInstance().SetGrouping(e.Grouping);
                ultraLabelGrouping.Text = HeatMapManager.GetInstance().GetGroupingText();
                //Updating status bar
                if (e.Grouping.Count == 0)
                    UpdataStausBar("Please select grouping");
                else
                    UpdataStausBar("");
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
