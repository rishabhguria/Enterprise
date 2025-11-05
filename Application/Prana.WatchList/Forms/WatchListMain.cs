using ExportGridsData;
using Infragistics.Win.UltraWinDock;
using Infragistics.Win.UltraWinToolbars;
using Prana.BusinessObjects;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.UI.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using Prana.WatchList.Classes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.WatchList.Forms
{
    public partial class WatchListMain : Form, IExportGridData
    {
        //This object is use to declare total no of rows any grid contain.

        /// <summary>
        /// The adding symbol value
        /// </summary>
        private string _addingSymbolValue = string.Empty;

        //This object is responsible for all most all operation
        WatchListManager _symbolLiveFeedManager = null;

        //This event handler is responsible for open tt for the particular symbol
        public event EventHandler<EventArgs<string, string>> SendSymbolToTT;

        public event EventHandler<EventArgs<string>> SendSymbolToPTT;

        public event EventHandler<EventArgs<string>> HighlightSymbolFromWatchlist;

        /// <summary>
        /// Occurs when [send symbol to MTT].
        /// </summary>
        public event EventHandler<EventArgs<OrderBindingList>> SendSymbolToMTT;

        /// <summary>
        /// Occures when Optio Chain Window is opened
        /// </summary>
        public event EventHandler<EventArgs<int, string>> OptionChainModuleOpened;

        /// <summary>
        /// Gets or sets the login user.
        /// </summary>
        /// <value>
        /// The login user.
        /// </value>
        private Prana.BusinessObjects.CompanyUser LoginUser
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WatchListMain"/> class.
        /// </summary>
        /// <param name="LoginUser">The login user.</param>
        public WatchListMain(Prana.BusinessObjects.CompanyUser LoginUser, ISecurityMasterServices secMasterServices)
        {
            try
            {
                InitializeComponent();
                this.LoginUser = LoginUser;
                _symbolLiveFeedManager = new WatchListManager(ultraDockManager1, secMasterServices, LoginUser);
                _symbolLiveFeedManager.LinkedSymbolSelected += SendLinkedSymbolToMain;
                _symbolLiveFeedManager.SendSymbolForTTToMain += SendTradeToTT;
                _symbolLiveFeedManager.SendSymbolForPTTToMain += SendSymbolForPTTToMain;
                _symbolLiveFeedManager.SendSymbolForMTTToMain += SendTradeToMTT;
                _symbolLiveFeedManager.OptionChainModuleOpened += _symbolLiveFeedManager_OptionChainModuleOpened;
                ultraDockManager1.PaneActivate += ultraDockManager1_PaneActivate;
                InstanceManager.RegisterInstance(this);
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

        void SendSymbolForPTTToMain(object sender, EventArgs<string> e)
        {
            try
            {
                if (SendSymbolToPTT != null)
                    SendSymbolToPTT(null, e);
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
        /// Sends the trade to tt.HighlightSymbolFromWatchlist
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs{System.String, System.String}"/> instance containing the event data.</param>
        internal void SendTradeToTT(object sender, EventArgs<string, string> e)
        {
            try
            {
                if (SendSymbolToTT != null)
                    SendSymbolToTT(null, e);
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

        internal void SendTradeToMTT(object sender, EventArgs<OrderBindingList> e)
        {
            try
            {
                if (SendSymbolToMTT != null)
                    SendSymbolToMTT(null, e);
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

        void _symbolLiveFeedManager_OptionChainModuleOpened(object sender, EventArgs<int, string> e)
        {
            if (OptionChainModuleOpened != null)
                OptionChainModuleOpened(this, e);
        }

        /// <summary>
        /// Sends the linked symbol to main.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs{System.String}"/> instance containing the event data.</param>
        internal void SendLinkedSymbolToMain(object sender, EventArgs<string> e)
        {
            try
            {
                if (HighlightSymbolFromWatchlist != null)
                    HighlightSymbolFromWatchlist(sender, e);
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
        /// This method set the theam on the watch List UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WatchListMain_Load(object sender, EventArgs e)
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_WATCHLIST);
                ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, Text, CustomThemeHelper.UsedFont);
                this.Load -= WatchListMain_Load;
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
        /// This method is use to updates some value in manager class.
        /// </summary>
        private void ultraDockManager1_PaneActivate(object sender, ControlPaneEventArgs e)
        {
            try
            {
                if (ultraDockManager1.ActivePane != null)
                {
                    string activeTabName = ultraDockManager1.ActivePane.Key;
                    _symbolLiveFeedManager.TabChangeProcess(activeTabName);
                    if (_symbolLiveFeedManager.IsTabPermanent(activeTabName))
                        ultraToolbarsManager1.Tools[1].SharedProps.Enabled = false;
                    else
                        ultraToolbarsManager1.Tools[1].SharedProps.Enabled = true;
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

        void ultraDockManager1_AfterToggleDockState(object sender, PaneEventArgs e)
        {
            if (e.Pane.DockedState == DockedState.Floating)
            {
                e.Pane.Settings.AllowMaximize = Infragistics.Win.DefaultableBoolean.True;
                e.Pane.Settings.ShowCaption = Infragistics.Win.DefaultableBoolean.True;
                e.Pane.Settings.AllowDragging = Infragistics.Win.DefaultableBoolean.True;
                e.Pane.Settings.CaptionAppearance.ForeColor = Color.White;
                e.Pane.Settings.CaptionAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
                e.Pane.Settings.CaptionAppearance.BackColor = Color.FromArgb(88, 88, 90);
                e.Pane.Settings.CaptionAppearance.BackGradientStyle = Infragistics.Win.GradientStyle.None;
                e.Pane.Settings.ActiveCaptionAppearance.ForeColor = Color.White;
                e.Pane.Settings.ActiveCaptionAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
                e.Pane.Settings.ActiveCaptionAppearance.BackColor = Color.FromArgb(88, 88, 90);
                e.Pane.Settings.ActiveCaptionAppearance.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            }
            else
            {
                e.Pane.Settings.AllowMaximize = Infragistics.Win.DefaultableBoolean.False;
                e.Pane.Settings.ShowCaption = Infragistics.Win.DefaultableBoolean.False;
                e.Pane.Settings.AllowDragging = Infragistics.Win.DefaultableBoolean.False;
            }
        }



        /// <summary>
        /// this method is use to show a small pop up from taking an stirng and return it.
        /// </summary>
        /// <returns></returns>
        private string GetTabNameFromInputBox()
        {
            DialogResult result = DialogResult.None;
            string tabName = string.Empty;
            try
            {
                tabName = Prana.Utilities.UI.UIUtilities.InputBox.ShowInputBox("Custom View", tabName, out result).Trim();

                // Don't do anything if user has hit cancel or closed the dialog from left top "X"
                if (result == DialogResult.Cancel)
                    return null;

                if (!GeneralUtilities.CheckNameValidation(tabName))
                {
                    MessageBox.Show(this, "Invalid view name.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tabName = string.Empty;
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
            return tabName;
        }

        /// <summary>
        /// This method is use to add new tab into tabcontrol
        /// </summary>
        private void addTabBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string tabName = GetTabNameFromInputBox();
                if (!string.IsNullOrWhiteSpace(tabName))
                {
                    _symbolLiveFeedManager.AddNewTab(tabName);
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
        /// Set Label Message Delegate
        /// </summary>
        /// <param name="errMessage">The error message.</param>
        public delegate void SetLabelMessageDelegate(string errMessage);

        /// <summary>
        /// This method is responsible for delete the selected tab with its all symbol
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void deleteTabBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (ultraDockManager1.ControlPanes == null || ultraDockManager1.ControlPanes.Count <= 0)
                {
                    return;
                }
                if (_symbolLiveFeedManager.GetTabSymbolsCount() > 0)
                {
                    DialogResult result = MessageBox.Show("Do you want to delete all the symbols also?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result != DialogResult.Yes)
                        return;
                }
                _symbolLiveFeedManager.DeleteTab();
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
        /// This method is responsible for run specified functionality which we click in the right-click menu.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">ToolClickEventArgs</param>
        private void ultraToolbarsManager1_ToolClick(object sender, ToolClickEventArgs e)
        {
            try
            {
                switch (e.Tool.Key)
                {
                    case "AddTab":    // ButtonTool
                        addTabBtn_Click(sender, e);
                        break;

                    case "DelTab":    // ButtonTool
                        deleteTabBtn_Click(sender, e);
                        break;

                    case "RenameTab":
                        renameTabBtn_Click(sender, e);
                        break;

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
        /// Handles the Click event of the renameTabBtn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ToolClickEventArgs"/> instance containing the event data.</param>
        private void renameTabBtn_Click(object sender, ToolClickEventArgs e)
        {
            try
            {
                if (ultraDockManager1.ControlPanes == null || ultraDockManager1.ControlPanes.Count <= 0)
                {
                    return;
                }
                string newTabName = GetTabNameFromInputBox();
                if (!string.IsNullOrWhiteSpace(newTabName))
                    _symbolLiveFeedManager.RenameTab(newTabName);
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
        /// Add symbols/options from outside to WatchList
        /// </summary>
        /// <param name="tabNumber">Tab number where options will be added</param>
        /// <param name="symbols">List of symbols to add</param>
        public bool AddSymbolsToWatchList(int tabNumber, List<string> symbols)
        {
            try
            {
                // Adding options to WatchList
                if (tabNumber != int.MinValue && _symbolLiveFeedManager.IsTabPresent(tabNumber) && symbols != null && symbols.Count > 0)
                {
                    foreach (string option in symbols)
                        if (!_symbolLiveFeedManager.AddSymbolToTab(option, tabNumber))
                            return false;

                    return true;
                }
                else
                    new CustomMessageBox("Watchlist Tab Deleted!", "Watchlist tab not found so unable to add option(s).", false, string.Empty, FormStartPosition.CenterParent).ShowDialog();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        /// <summary>
        /// used to Export Data for automation
        /// </summary>
        /// <param name="gridName"></param>
        /// <param name="WindowName"></param>
        /// <param name="tabName"></param>
        /// <param name="filePath"></param>
        public void ExportData(string gridName, string WindowName, string tabName, string filePath)
        {
            if (_symbolLiveFeedManager != null)
                _symbolLiveFeedManager.ExportGridData(filePath, tabName);
        }
    }
}
