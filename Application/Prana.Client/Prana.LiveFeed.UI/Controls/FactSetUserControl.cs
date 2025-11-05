using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using PerCederberg.Grammatica.Runtime.RE;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;

namespace Prana.LiveFeed.UI.Controls
{
    public partial class FactSetUserControl : UserControl
    {
        private const string CONST_WebView_DocumentElement = "document.documentElement.outerHTML";
        private const string CONST_FactSetAuthenticationURL = "FactSetAuthenticationURL";
        private const string CONST_FactSetRedirectPageURL = "FactSetRedirectPageURL";
        private const string CONST_FactSetRegexPatternForTokenGeneration = "FactSetRegexPatternForTokenGeneration";
        private const string CONST_FactSetResellerUserProfiles = "FactSetResellerUserProfiles";
        public FactSetUserControl()
        {
            InitializeComponent();
            InitBrowser();
        }

        /// <summary>
        /// Event to sets token for the factset user
        /// </summary>
        public event EventHandler<EventArgs<string, string>> SetTokenFactsetUser;
        /// <summary>
        /// Event to close the Factset Authentication form.
        /// </summary>
        public event EventHandler CloseTheFactSetForm;
        private async Task initialized()
        {
            try
            {
                string userProfileDirectoryPath = Path.Combine(Application.StartupPath, CONST_FactSetResellerUserProfiles, CachedDataManager.GetInstance.LoggedInUser.CompanyUserID.ToString());
                if (!Directory.Exists(userProfileDirectoryPath))
                    Directory.CreateDirectory(userProfileDirectoryPath);
                CoreWebView2Environment webViewEnvironment = await CoreWebView2Environment.CreateAsync(null, userProfileDirectoryPath);
                await factSetWebsiteView.EnsureCoreWebView2Async(webViewEnvironment);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        private async void InitBrowser()
        {
            try
            {
                await initialized();
                string factsetURL = ConfigurationManager.AppSettings[CONST_FactSetAuthenticationURL];
                if (!string.IsNullOrWhiteSpace(factsetURL))
                    factSetWebsiteView.CoreWebView2.Navigate(factsetURL);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        private void RedirectToHomePage(string url)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(url))
                    factSetWebsiteView.CoreWebView2.Navigate(url);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        private async void FactSetWebsiteView_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            try
            {
                string regexPatternforToken = ConfigurationManager.AppSettings[CONST_FactSetRegexPatternForTokenGeneration];
                Regex regexPattern = new Regex(@regexPatternforToken);
                string htmlPage = await factSetWebsiteView.ExecuteScriptAsync(CONST_WebView_DocumentElement);
                Match match = regexPattern.Match(htmlPage);
                if (match != null && match.Success && match.Groups != null)
                {
                    if (match.Groups.Count > 3)
                    {
                        string token = match.Groups[1].Value;
                        string username = match.Groups[2].Value;
                        string serial = match.Groups[3].Value;
                        string usernameAndSerialNumber = username + '-' + serial;
                        if (!string.IsNullOrEmpty(token))
                        {
                            if (SetTokenFactsetUser != null)
                                SetTokenFactsetUser(this, new EventArgs<string, string>(token, usernameAndSerialNumber));

                            string factsetRedirectPageUrl = ConfigurationManager.AppSettings[CONST_FactSetRedirectPageURL];
                            if (!string.IsNullOrEmpty(factsetRedirectPageUrl))
                            {
                                RedirectToHomePage(factsetRedirectPageUrl);
                            }
                            else if (CloseTheFactSetForm != null)
                            {
                                CloseTheFactSetForm(this, e);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
    }
}
