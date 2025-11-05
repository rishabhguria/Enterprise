using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Prana.ExposurePnlCache;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.BusinessObjects;
using Prana.Interfaces;
using Prana.PNL.UI.Preferences;
using Prana.PNL.UI.Controls;
using Infragistics.Win.UltraWinTabControl;
using Prana.BusinessObjects.AppConstants;
using Prana.Utilities.MiscUtilities;
namespace Prana.PNL.UI.Forms
{
    /// <summary>
    /// We are implementing IPositionManagement as PNL is now a subset of Position Management
    /// </summary>
    public partial class PNLMain : Form, IPositionManagement
    {
        #region Constructor
        public PNLMain()
        {
            InitializeComponent();
        } 
        #endregion

        #region Variable Declaration
        ExposurePnlCacheManager _exInstance = null;
        private bool _isFormClosed = false;
        private const string FORM_NAME = "P&L";

        ExPnlBindableView _exPnlBindableView = null;

        private const string DefaultView_AssetClass = "Asset Class";
        private const string DefaultView_Symbol = "Symbol";
        private const string DefaultView_TradingAccount = "Trading Account";


        private const string SUB_MODULE_NAME = "PNL";
        private static PNLPrefrenceManager _prefrenceManager = null;
        PNLPreferenceList _preferencesList = null;

        Dictionary<string, MainPnlGrid> _instanceLookup;

        #endregion

        #region Form Load
        private void frmPNLMain_Load(object sender, EventArgs e)
        {
            try
            {
                _prefrenceManager = PNLPrefrenceManager.GetInstance(SUB_MODULE_NAME);

                _instanceLookup = new Dictionary<string, MainPnlGrid>();

                _exInstance = ExposurePnlCacheManager.GetInstance();
                _exInstance.Initialise(_loginUser, _exPnlCommMgrInstance);
                ///Need to check the data for fund and strategy.
                _exInstance.Subscribe(ExPNLSubscriptionType.TradesOnly,ExPNLData.Fund);

                //_exInstance.ExposurePnlCacheItemListChanged += new ExposurePnlCacheManager.MethodHandler(_exposurePnlCacheManagerInstance_ExposurePnlCacheItemListChanged);


                _exPnlBindableView = new ExPnlBindableView();
                _exPnlBindableView.GridData = _exInstance.PNLView;

                LoadPnLViews(_loginUser.FirstName + _loginUser.LastName);
                _isFormClosed = false;
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

        }

        
        #endregion

        #region Load Custom Views

        private void AddDefaultViews(ref PNLPreferenceList prefList, string viewName)
        {
            if (!prefList.ContainsKey(viewName))
            {
                PNLPreference pref = new PNLPreference();
                pref.TabName = viewName;

                switch (viewName)
                {
                    case DefaultView_AssetClass:
                        //Add Grouping
                        pref.GroupByColumnsCollection.Add("Asset");
                        pref.GroupByColumnsCollection.Add("Exchange");

                        ///Remove Columns
                        
                        pref.DeselectedColumnsCollection.Add("Symbol");

                        pref.DeselectedColumnsCollection.Add("Exchange");

                        pref.DeselectedColumnsCollection.Add("AskPrice");
                        pref.DeselectedColumnsCollection.Add("BidPrice");
                        pref.DeselectedColumnsCollection.Add("LastPrice");
                        pref.DeselectedColumnsCollection.Add("ClosingPrice");
                        pref.DeselectedColumnsCollection.Add("PercentageChange");

                        pref.DeselectedColumnsCollection.Add("AvgPrice");
                        pref.DeselectedColumnsCollection.Add("ExecutedQty");
                        pref.DeselectedColumnsCollection.Add("SideName");

                        pref.DeselectedColumnsCollection.Add("TradingAccount");

                        break;
                    case DefaultView_Symbol:
                        //Add Grouping
                        pref.GroupByColumnsCollection.Add("Symbol");
                        pref.GroupByColumnsCollection.Add("SideName");

                        ///Remove Columns
                        pref.DeselectedColumnsCollection.Add("Asset");
                        pref.DeselectedColumnsCollection.Add("Underlying");
                        pref.DeselectedColumnsCollection.Add("Exchange");
                        pref.DeselectedColumnsCollection.Add("Currency");

                        pref.DeselectedColumnsCollection.Add("TradingAccount");

                        break;

                    case DefaultView_TradingAccount:
                        //Add Grouping
                        pref.GroupByColumnsCollection.Add("TradingAccount");
                        pref.GroupByColumnsCollection.Add("Symbol");

                        ///Remove Columns
                        
                        pref.DeselectedColumnsCollection.Add("Asset");
                        pref.DeselectedColumnsCollection.Add("Underlying");
                        pref.DeselectedColumnsCollection.Add("Exchange");
                        pref.DeselectedColumnsCollection.Add("Currency");

                        pref.DeselectedColumnsCollection.Add("PNLLong");
                        pref.DeselectedColumnsCollection.Add("PNLShort");

                        pref.DeselectedColumnsCollection.Add("AvgPrice");
                        pref.DeselectedColumnsCollection.Add("SideName");
                        break;
                   
                }

                prefList.Add(viewName, pref);
                _prefrenceManager.SetCustomViewPreference(pref);
            }
        }

        private void CreateDefaultViews(ref PNLPreferenceList prefList)
        {
            AddDefaultViews(ref _preferencesList, DefaultView_AssetClass);            
            AddDefaultViews(ref _preferencesList, DefaultView_Symbol);
            AddDefaultViews(ref _preferencesList, DefaultView_TradingAccount);
        }

        private void LoadPnLViews(string userName)
        {
            try
            {
                _preferencesList = _prefrenceManager.GetCustomViewPreferences(userName);

                ///Create default views
                CreateDefaultViews(ref _preferencesList);

                foreach (PNLPreference preference in _preferencesList.Values)
                {
                    string tabName = preference.TabName;
                    string tabKey = tabName;
                    CreateNewPNLViewTab(tabName, preference);
                }


            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }


        #endregion

        #region Add New PNL Views

        void AddNewConsolidationView(object sender, EventArgs e)
        {
            try
            {
                AddNewPNLView();
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        } 

        /// <summary>
        /// Adds the new PNL view.
        /// </summary>
        private void AddNewPNLView()
        {
            try
            {
                 string tabName = string.Empty;

                DialogResult result = GetTabNameFromInputBox(ref tabName);

                if (tabName != string.Empty && result == DialogResult.OK)
                {
                    PNLPreference pref = new PNLPreference();
                    pref.TabName = tabName;

                    CreateNewPNLViewTab(tabName, pref);

                    _preferencesList.Add(tabName, pref);
                }

            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void RenameCustomView(string tabname, PNLPreference preferences)
        {
            try
            {
                string newTabName = tabname.Trim();
                DialogResult result = GetTabNameFromInputBox(ref newTabName);

                if (newTabName != string.Empty && result == DialogResult.OK && !tabname.Trim().Equals(newTabName.Trim()))
                {
                    preferences.TabName = newTabName;

                    PNLPreferenceList savedList = _prefrenceManager.GetCustomViewPreferences(_loginUser.FirstName + _loginUser.LastName);

                    DeletePNLViewTab(tabname);

                    CreateNewPNLViewTab(newTabName, preferences);


                    if (savedList.ContainsKey(tabname))
                    {
                        ///remove old tab name and add new one
                        savedList.Remove(tabname);

                        savedList.Add(newTabName, preferences);

                        ///save back
                        _prefrenceManager.SetPreferences<PNLPreferenceList>(savedList);                    

                    }

                    _preferencesList.Add(newTabName, preferences);
                }

            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private DialogResult GetTabNameFromInputBox(ref string tabName)
        {
            DialogResult result = DialogResult.None;
            string OldTabName = tabName;
            tabName = Prana.PNL.UI.Controls.InputBox.ShowInputBox("Custom View", tabName, out result).Trim();

            // Don't do anything if user has hit cancel or closed the dialog from left top "X"
            if (result == DialogResult.Cancel) return result;

            if (!GeneralUtilities.CheckNameValidation(tabName))
            {
                MessageBox.Show(this, "Invalid view name.", "Prana Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tabName = string.Empty;
            }
            if (!OldTabName.Equals(tabName))
            {
                if (_preferencesList.ContainsKey(tabName))
                {
                    MessageBox.Show(this, tabName + " already exists! \n Please enter different view name.", "Prana Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tabName = string.Empty;
                }
            }

            return result;
        }

        #endregion

        #region Create Custom View Tab
        private void CreateNewPNLViewTab(string tabName, PNLPreference preference)
        {
            try
            {
                
                UltraTab tab = tbcPNL.Tabs.Add(tabName);
              
                tab.Text = tabName;

                MainPnlGrid newPnlGridControl = new MainPnlGrid();
                newPnlGridControl.Dock = DockStyle.Fill;

                newPnlGridControl.InitControl(true, _loginUser, preference, _exPnlBindableView);

                newPnlGridControl.TradeClick += new EventHandler(OnTradeClick);
                newPnlGridControl.DepthClick += new EventHandler(OnDepthClick);
                newPnlGridControl.OptionChainClick += new EventHandler(OnOptionChainClick);
                newPnlGridControl.ChartsClick += new EventHandler(OnChartsClick);
                newPnlGridControl.AddNewPNLView += new EventHandler(AddNewConsolidationView);

                if (tabName != DefaultView_AssetClass &&
                        tabName != DefaultView_Symbol &&
                        tabName != DefaultView_TradingAccount)
                {
                    newPnlGridControl.DeleteViewClick += new EventHandler(OnDeleteViewClick);
                    newPnlGridControl.RenameViewClick += new EventHandler(OnRenameViewClick);
                }
                else
                {
                    newPnlGridControl.DeleteViewEnabled = false;
                    newPnlGridControl.RenameViewEnabled = false;
                }
                

                tab.TabPage.Controls.Add(newPnlGridControl);

                //Add custom view instance to lookup
                _instanceLookup.Add(tabName, newPnlGridControl);

                tbcPNL.Tabs[tabName].Text = tabName;

                tbcPNL.Tabs[tabName].Selected = true;
                //}
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
           
        }

        void OnRenameViewClick(object sender, EventArgs e)
        {
            try
            {
                RenameCustomView(sender.ToString(), _preferencesList[sender.ToString()]);
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }  
        }

        void OnDeleteViewClick(object sender, EventArgs e)
        {
            try
            {
                string tabName = sender.ToString();

                DialogResult result = MessageBox.Show(this, "Delete " + tabName + "! \n Are you sure?", "P&L Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                if (result == DialogResult.Yes)
                {

                    DeletePNLViewTab(tabName);

                }
                
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }  
        }

        private void DeletePNLViewTab(string tabName)
        {
            try
            {
                ///Get the instance for lookup from sender(sender contains tabname)           
                MainPnlGrid control = _instanceLookup[tabName];
                control.TradeClick -= new EventHandler(OnTradeClick);
                control.OptionChainClick -= new EventHandler(OnOptionChainClick);
                control.DepthClick -= new EventHandler(OnDepthClick);
                control.ChartsClick -= new EventHandler(OnChartsClick);
                control.AddNewPNLView -= new EventHandler(AddNewConsolidationView);
                if (tabName != DefaultView_AssetClass &&
                    tabName != DefaultView_Symbol &&
                    tabName != DefaultView_TradingAccount)
                {
                    control.DeleteViewClick -= new EventHandler(OnDeleteViewClick);
                    control.RenameViewClick -= new EventHandler(OnRenameViewClick);
                }
                UltraTab tab = tbcPNL.Tabs[tabName];
                tab.TabPage.Controls.Remove(control);

                tbcPNL.Tabs.Remove(tab);

                _preferencesList.Remove(tabName);

                _instanceLookup.Remove(tabName);


                PNLPreferenceList savedList = _prefrenceManager.GetCustomViewPreferences(_loginUser.FirstName + _loginUser.LastName);

                PNLPreferenceList currentList = _preferencesList;

                PNLPreferenceList newList = new PNLPreferenceList();

                foreach (PNLPreference oldPref in savedList.Values)
                {
                    if (currentList.ContainsKey(oldPref.TabName))
                    {
                        newList.Add(oldPref.TabName, oldPref);
                    }
                }

                _prefrenceManager.SetPreferences<PNLPreferenceList>(newList);


                tab = null;
                control = null;
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }


        #endregion


        #region Contexet Menu Events here.....
        void OnChartsClick(object sender, EventArgs e)
        {
            try
            {
                if (ChartsClick != null)
                {
                    ChartsClick(sender, e);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        void OnOptionChainClick(object sender, EventArgs e)
        {
            try
            {
                if (OptionChainClick != null)
                {
                    OptionChainClick(sender, e);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        void OnDepthClick(object sender, EventArgs e)
        {
            try
            {
                if (DepthClick != null)
                {
                    DepthClick(sender, e);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        void OnTradeClick(object sender, EventArgs e)
        {
            try
            {
                if (TradeClick != null)
                {
                    TradeClick(sender, e);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        void _exposurePnlCacheManagerInstance_ExposurePnlCacheItemListChanged()
        {
            try
            {
                if (!_isFormClosed && _exPnlBindableView != null)
                {
                    _exPnlBindableView.GridData = _exInstance.PNLView;

                    //lock (_instanceLookup)
                    //{
                    //    foreach (MainPnlGrid childControl in _instanceLookup.Values)
                    //    {
                    //        childControl.RefreshGrid();
                    //    }
                    //}
                    
                }

            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void frmPNLMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                _exInstance.Unsubscribe(ExPNLSubscriptionType.TradesOnly,ExPNLData.Fund);
                //_exInstance.ExposurePnlCacheItemListChanged -= new ExposurePnlCacheManager.MethodHandler(_exposurePnlCacheManagerInstance_ExposurePnlCacheItemListChanged);
               
                _isFormClosed = true;

                if (FormClosedHandler != null)
                {
                    FormClosedHandler(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        } 
        #endregion



        #region IPositionManagement Members

        public event EventHandler FormClosedHandler;
        public event EventHandler TradeClick = null;
        public event EventHandler ChartsClick = null;
        public event EventHandler DepthClick = null;
        public event EventHandler OptionChainClick = null;

        Form IPositionManagement.Reference()
        {
            return this;
        }

        CompanyUser _loginUser = null;
        public CompanyUser LoginUser
        {
            get { return _loginUser; }
            set { _loginUser = value; }
        }

        ICommunicationManager _communicationManager = null;
        public ICommunicationManager CommunicationManagerInstance
        {
            get
            {
                return _communicationManager;
            }
            set
            {
                _communicationManager = value;
            }
        }

        ICommunicationManager _exPnlCommMgrInstance = null;
        public ICommunicationManager ExPNLCommMgrInstance
        {
            get
            {
                return _exPnlCommMgrInstance;
            }
            set
            {
                _exPnlCommMgrInstance = value;
            }
        }

        public System.Windows.Forms.Form Reference()
        {
            return this;
        }

        public void OnSubscriptionDataChange(bool reconnect)
        {
            
        }
        public event EventHandler ConnectServer;
        #endregion

        public event EventHandler PNLClosing;
        public event EventHandler ExceptionReportClick = null;
        public event EventHandler CreateTransactionClick = null;
        public event EventHandler ClosePositionClick = null;
        public event EventHandler MarkPriceClick = null;
        public event EventHandler CancelAmendClick = null;

        private void PNLMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {

                PNLPreferenceList savedList = _prefrenceManager.GetCustomViewPreferences(_loginUser.FirstName + _loginUser.LastName);

                PNLPreferenceList currentList = _preferencesList;

                List<string> unsavedTabs = new List<string>();

                foreach (string key in currentList.Keys)
                {
                    if (!savedList.ContainsKey(key)
                         && !key.Equals(DefaultView_AssetClass)
                        && !key.Equals(DefaultView_Symbol)
                        && !key.Equals(DefaultView_TradingAccount))
                    {

                        unsavedTabs.Add(key);

                    }

                }

                if (unsavedTabs.Count > 0)
                {
                    StringBuilder alterMessage = new StringBuilder("P&L Views : ");

                    foreach (string tabName in unsavedTabs)
                    {
                        alterMessage.Append("\n" + tabName);
                    }

                    alterMessage.Append("\n are not saved! \n Do you want to save new view(s) before closing?");

                    DialogResult result = MessageBox.Show(this, alterMessage.ToString(), "P&L", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                    if (result == DialogResult.OK)
                    {
                        foreach (MainPnlGrid view in _instanceLookup.Values)
                        {
                            view.SaveLayout();
                        }

                    }
                }


                if (PNLClosing != null)
                {
                    PNLClosing(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        
    }
}