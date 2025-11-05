using Prana.BusinessObjects.Compliance.Constants;
using Prana.BusinessObjects.Compliance.Definition;
using Prana.BusinessObjects.Compliance.Delegates;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.BusinessObjects.Compliance.EventArguments;
using Prana.CommonDataCache;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace Prana.ComplianceEngine.RuleDefinition.UI.UserControls
{
    public partial class RuleDefViewer : UserControl
    {
        /// <summary>
        /// Max size of browser cache.
        /// after that it removes the top element and assigns new url to its browser.
        /// </summary>
        private int _maxBrowserLimit = 5;

        /// <summary>
        /// max size of queue that is used to send navigate request.
        /// </summary>
        private int _maxNavigationQueueSize = 1;
        /// <summary>
        /// stores id of last navigated rule url
        /// if same as requested then increase timer.
        /// </summary>
        private string _lastNavigatedId = string.Empty;

        /// <summary>
        /// Queue which enqueue urls to be navigated
        /// on timer elapsed of 1 sec if queue max size is reached then dequeue urls and navigate browser to that page.
        /// </summary>
        private ConcurrentQueue<string> _urlNavigationQueue = new ConcurrentQueue<string>();

        private object _lockerObject = new object();
        /// <summary>
        /// browser dict for storing url as key and browser as value.
        /// </summary>
        private SortedDictionary<String, WebBrowserControl> _browserDict = new SortedDictionary<string, WebBrowserControl>();

        public event RuleBrowerSaveCompleteHandle RuleDefBrowserSaveCompleteEvent;

        System.Timers.Timer timer = new System.Timers.Timer(1000);

        /// <summary>
        /// 
        /// </summary>
        public RuleDefViewer()
        {
            try
            {
                InitializeComponent();
                if (!(DesignMode || CustomThemeHelper.IsDesignMode()))
                {
                    InitializeBrowser();
                    timer.Elapsed += timer_Elapsed;
                    timer.Start();
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
        /// timer elapsed of 1 sec
        /// if queue max size is reached then dequeue urls and navigate browser to that page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            try
            {
                if (this.InvokeRequired)
                {
                    MethodInvoker del = delegate { timer_Elapsed(sender, e); };
                    this.BeginInvoke(del);
                }
                else
                {
                    lock (_lockerObject)
                    {
                        timer.Enabled = false;



                        string result;
                        bool isSuccessFull = _urlNavigationQueue.TryDequeue(out result);

                        //Extra handling for reload(if _lastNavigatedId == requested url then return in 1 sec timer)
                        if (_lastNavigatedId == result)
                            Thread.Sleep(1500);
                        //if dequeue is successfull url is not null and browser dictionary contains url then navvigate to page
                        //else do nothing.
                        if (isSuccessFull && !string.IsNullOrEmpty(result) && _browserDict.ContainsKey(result))
                            _browserDict[result].Navigate();


                        //if queue count is ==0 then do not start timer to check elapsed time
                        //else start because as there are not url to navigate then what is the need to run timer.
                        if (_urlNavigationQueue.Count == 0)
                            timer.Enabled = false;
                        else
                            timer.Enabled = true;
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

        /// <summary>
        /// Add url to queue if not already in queue.
        /// </summary>
        /// <param name="url"></param>
        private void AddToQueue(string url)
        {
            try
            {

                if (!_urlNavigationQueue.Contains(url))
                {
                    //if queue size is greater than maz queue size then dequeue all urls. and then enqueue new url.
                    while (_urlNavigationQueue.Count >= _maxNavigationQueueSize)
                    {
                        string result;
                        _urlNavigationQueue.TryDequeue(out result);
                    }
                    _urlNavigationQueue.Enqueue(url);

                    //If timer is not enabled then enable timer for checking elapsed time.
                    if (!timer.Enabled)
                    {
                        timer.Enabled = true;
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


        /// <summary>
        /// Initialize browser with startup page.
        /// </summary>
        private void InitializeBrowser()
        {
            try
            {
                LoadDocument(Application.StartupPath + "\\HtmlFiles\\Startup.Htm");
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
        /// Loads rule browser to definition viewer control.
        /// and add browser to dictionary of rule browser, if not in dictionary
        /// else load browser for that rule from cache
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="useExisting"> true only in case of reload as new control is not needed</param>
        public void LoadDocument(string uri, bool useExisting = false)
        {
            try
            {
                lock (_lockerObject)
                {

                    bool controlAlreadyExist = _browserDict.ContainsKey(uri);

                    if (!controlAlreadyExist)
                    {
                        SetUpNewBrowserControl(uri);
                    }

                    if (!useExisting)
                    {
                        foreach (Control ctrl in ultraPnlMain.ClientArea.Controls)
                        {
                            if (ctrl is WebBrowserControl)
                            {
                                ultraPnlMain.ClientArea.Controls.Remove(ctrl);
                            }
                        }

                        ultraPnlMain.ClientArea.Controls.Add((UserControl)_browserDict[uri]);
                    }
                    if (controlAlreadyExist)
                    {
                        BrowserLoadCompletedEventArgs e = new BrowserLoadCompletedEventArgs { URI = uri, IsPostBack = false, IsSuccess = true };
                        if (RuleDefBrowserSaveCompleteEvent != null)
                            RuleDefBrowserSaveCompleteEvent(this, e);
                    }

                    // AddToQueue(uri);
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
        /// Sets browser at rule url and add it in browser cache.
        /// </summary>
        /// <param name="uri"></param>
        private void SetUpNewBrowserControl(String uri)
        {
            try
            {
                lock (_lockerObject)
                {
                    WebBrowserControl browser = null;
                    if (_browserDict.Count > _maxBrowserLimit - 1)
                    {
                        //If max limit is reached then gets browser and remove key from dictionary
                        //and changes browser uri to new uri and adds into cache with new key.
                        browser = _browserDict.ElementAt(0).Value;
                        _browserDict.Remove(browser.Url.OriginalString);
                    }
                    else
                    {
                        browser = BrowserFactoryHandler.GetBrowserControl();
                        browser.RuleBrowerSaveCompleteEvent += RuleDefViewer_RuleBrowerSaveCompleteEvent;
                    }
                    if (browser != null)
                    {
                        _browserDict.Add(uri, browser);
                        browser.Url = new Uri(uri);
                        AddToQueue(uri);
                    }

                    // browser.Navigate();
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
        /// Invoke save button of drools guvnor for saving rule.
        /// </summary>
        /// <param name="ruleList"></param>
        internal void SaveRule(List<RuleBase> ruleList)
        {
            try
            {
                foreach (RuleBase rule in ruleList)
                {

                    lock (_lockerObject)
                    {
                        //if (rule.Category.Equals(RuleCategory.UserDefined) && _browserDict.ContainsKey(rule.RuleURL))
                        if (_browserDict.ContainsKey(rule.RuleURL))
                        {
                            _browserDict[rule.RuleURL].SaveRule(rule);

                            //_isSave = true;
                        }
                        else
                        {
                            BrowserLoadCompletedEventArgs e = new BrowserLoadCompletedEventArgs { URI = rule.RuleURL, IsPostBack = true, IsSuccess = true };
                            if (RuleDefBrowserSaveCompleteEvent != null)
                                RuleDefBrowserSaveCompleteEvent(this, e);
                        }
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

        /// <summary>
        /// Updates browser dictionary after rule operation.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="oldRuleId"></param>
        internal void UpdateDictionary(List<RuleBase> list, string oldRuleId)
        {
            try
            {
                lock (_lockerObject)
                {
                    String oldRuleUrl = String.Empty;
                    if (!String.IsNullOrEmpty(oldRuleId))
                    {
                        oldRuleUrl = GetUrl(oldRuleId);
                    }
                    if (!String.IsNullOrEmpty(oldRuleUrl))
                    {
                        if (_browserDict.ContainsKey(oldRuleUrl))
                            _browserDict.Remove(oldRuleUrl);
                    }

                    foreach (RuleBase rule in list)
                    {
                        // Ideally there should not be any check on rule type
                        // TODO: Need to fix this
                        if (_browserDict.ContainsKey(rule.RuleURL) && rule.Category == RuleCategory.UserDefined)
                            UpdateBrowser(rule.RuleURL);
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

        /// <summary>
        /// Checks if url is valid or not if valid update dictionary
        /// else remove browser from dictionary.
        /// </summary>
        /// <param name="url"></param>
        private void UpdateBrowser(String url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Timeout = 3000;
                HttpWebResponse response;
                response = (HttpWebResponse)request.GetResponse();
                lock (_lockerObject)
                {
                    if ((int)response.StatusCode == 200)
                    {
                        // OpenWebkitWebControl browser = BrowserFactoryHandler.GetBrowserControl();
                        //browser.RuleBrowerSaveCompleteEvent += RuleDefViewer_RuleBrowerSaveCompleteEvent;
                        _browserDict[url].Url = new Uri(url);
                        //browser.Navigate();
                        //browserDict[url] = browser;

                    }
                    else
                    {
                        if (_browserDict.ContainsKey(url))
                            _browserDict.Remove(url);
                    }

                    AddToQueue(url);
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
        /// returns Url for ruleId renamed rule for old rule Id
        /// </summary>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        private string GetUrl(String ruleId)
        {
            try
            {
                bool _isPowerUser = ComplianceCacheManager.GetPowerUserCheck(CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                String param = "client=" + UserDefinedRuleConstants.CLIENT_NAME + "&assetsUUIDs=" + ruleId + "&isPowerUser=" + _isPowerUser;
                return UserDefinedRuleConstants.GUVNOR_STANDALONE_BASE_URL + param;
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
                return String.Empty;
            }
        }

        /// <summary>
        ///  Event raised when guvnor completes save rule.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void RuleDefViewer_RuleBrowerSaveCompleteEvent(object sender, BrowserLoadCompletedEventArgs e)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    MethodInvoker del = delegate { RuleDefViewer_RuleBrowerSaveCompleteEvent(sender, e); };
                    this.BeginInvoke(del);
                }
                else
                {
                    if (RuleDefBrowserSaveCompleteEvent != null)
                        RuleDefBrowserSaveCompleteEvent(this, e);
                    System.GC.Collect();
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

        private void ultraPnlMain_PaintClient(object sender, PaintEventArgs e)
        {
            if (!CustomThemeHelper.ApplyTheme)
            {
                SetAppearanceWithoutTheme();

            }
        }

        private void SetAppearanceWithoutTheme()
        {
            try
            {
                Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();

                appearance1.BackColor = System.Drawing.SystemColors.ActiveCaption;
                this.ultraPnlMain.Appearance = appearance1;
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
