using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Windows.Forms;

namespace Prana
{
    public partial class PMDashboard : Form
    {
        public System.Windows.Forms.Timer t1 = new System.Windows.Forms.Timer();
        private string _url;
        private string _divSectionName;

        public PMDashboard()
        {
            InitializeComponent();


        }


        /// <summary>
        /// Setup
        /// </summary>
        /// <param name="url"></param>
        /// <param name="divSectionName"></param>
        public void Setup(string url, string divSectionName)
        {
            try
            {
                string touchBaseUrl = ConfigurationHelper.Instance.GetAppSettingValueByKey("TouchBaseUrl");
                if (!string.IsNullOrEmpty(touchBaseUrl))
                {
                    t1.Stop();
                    this._url = touchBaseUrl + url;
                    this._divSectionName = divSectionName;

                    t1.Interval = 1000;
                    t1.Tick += t1_Tick;
                    t1.Start();

                    webBrowser1.ScriptErrorsSuppressed = true;
                    webBrowser1.DocumentCompleted += webBrowser1_DocumentCompleted;
                    webBrowser1.Url = new Uri(_url, UriKind.Absolute);
                }
                else
                {
                    MessageBox.Show(this, "Touch base URL is not set. Please set the Touch base URL in config.", "Warning", MessageBoxButtons.OK);
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
        /// webBrowser1 _DocumentCompleted event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

            try
            {


                if (e.Url.AbsolutePath != (sender as WebBrowser).Url.AbsolutePath)
                {
                    //  webBrowser1.Navigate(url);
                }

                if (e.Url.AbsolutePath.Contains("Login"))//(sender as WebBrowser).Url.AbsolutePath)
                {

                    Login();
                    // webBrowser1.Navigate(url);
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

        private void Login()
        {
            try
            {

                HtmlElement company = webBrowser1.Document.GetElementById("SiteContent_MainContent_Company");
                if (company != null)
                {
                    company.SetAttribute("Value", "weeden");
                }
                HtmlElement email = webBrowser1.Document.GetElementById("SiteContent_MainContent_Email");
                if (email != null)
                {
                    email.SetAttribute("Value", "admin\\nirvana");
                }
                HtmlElement password = webBrowser1.Document.GetElementById("SiteContent_MainContent_Password");
                if (password != null)
                {
                    password.SetAttribute("Value", "Support1%");
                }
                if (webBrowser1.Document.Forms.Count > 0)
                    webBrowser1.Document.Forms[0].InvokeMember("submit");


            }
            catch (Exception)
            {

                // throw;
            }
        }

        /// <summary>
        /// t1_Tick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void t1_Tick(object sender, EventArgs e)
        {
            try
            {

                HtmlElement menu = webBrowser1.Document.GetElementById("MenuStickyContainer");
                if (menu != null)
                {


                    RemoveMenuAndFooter(menu);
                    if (!string.IsNullOrWhiteSpace(_divSectionName))
                    {
                        #region commented
                        //HtmlElement htmlelement = webBrowser1.Document.GetElementById(divSectionName);
                        //if (htmlelement == null)
                        //{
                        //    return;
                        //}
                        //else
                        //{
                        //    Thread.Sleep(5000);
                        //    webBrowser1.Document.Body.InnerHtml = webBrowser1.Document.GetElementById(divSectionName).InnerHtml;
                        //    t1.Stop();
                        //} 
                        #endregion

                        switch (_divSectionName)
                        {
                            case "Dashboard":
                                SetUIForDashboardPage();
                                break;
                            case "Graphs":
                                SetUIForPortfolioSnapshotPage();
                                break;
                            case "PortfolioAnalytics":
                                SetUIForPortfolioAnalyticsPage();
                                break;
                            case "Reporting":
                                SetUIForReportingPage();
                                break;
                        }
                    }
                    //t1.Stop();
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
        /// SetUIForReportingPage
        /// </summary>
        private void SetUIForReportingPage()
        {
            try
            {
                this.Text = "Reports";
                this.Width = 1340;
                this.Height = 800;
                if (!webBrowser1.Url.AbsoluteUri.Contains("Report"))
                    webBrowser1.Navigate(_url);
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
        /// SetUIForPortfolioAnalyticsPage
        /// </summary>
        private void SetUIForPortfolioAnalyticsPage()
        {
            try
            {
                this.Text = "Portfolio Analytics";
                this.Width = 1315;
                this.Height = 800;

                if (webBrowser1.Url.AbsoluteUri != _url)
                    webBrowser1.Navigate(_url);

                SetAlternateRowsColor();
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
        /// SetUIForPortfolioSnapshotPage
        /// </summary>
        private void SetUIForPortfolioSnapshotPage()
        {
            try
            {
                this.Width = 1280;
                this.Height = 700;
                this.Text = "Graphs";

                if (webBrowser1.Url.AbsoluteUri != _url)
                    webBrowser1.Navigate(_url);

                HtmlElement UDAGridContainer = webBrowser1.Document.GetElementById("UDAGridContainer");
                if (UDAGridContainer != null)
                {
                    UDAGridContainer.Style = "display:none";
                }
                HtmlElement topDash_container = webBrowser1.Document.GetElementById("topDash_container");
                if (topDash_container != null)
                {
                    topDash_container.Style = "display:none";
                }

                HtmlElement dashGrids = webBrowser1.Document.GetElementById("dashGrids");
                if (dashGrids != null)
                {
                    dashGrids.Style = "display:none";
                    //var isFirstElement = true;
                    //var dashGridsElements = dashGrids.Children.GetEnumerator();
                    //while (dashGridsElements.MoveNext())
                    //{
                    //    if (isFirstElement)
                    //    {
                    //        ((HtmlElement)dashGridsElements.Current).Style = "display:none";
                    //        isFirstElement = false;
                    //        break;
                    //    }


                    //}
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
        /// SetUIForDashboardPage
        /// </summary>
        private void SetUIForDashboardPage()
        {
            try
            {
                if (webBrowser1.Url.AbsoluteUri != _url)
                    webBrowser1.Navigate(_url);
                this.Width = 970;
                this.Height = 800;
                this.Text = "Dashboard";

                HtmlElement UDAGridContainer = webBrowser1.Document.GetElementById("UDAGridContainer");
                if (UDAGridContainer != null)
                {
                    UDAGridContainer.Style = "display:none";
                }
                HtmlElement topDash_container = webBrowser1.Document.GetElementById("topDash_container");
                if (topDash_container != null)
                {
                    topDash_container.Style = "display:none";
                }



                HtmlElement dashGrids = webBrowser1.Document.GetElementById("dashGrids");
                if (dashGrids != null)
                {
                    var isFirstElement = true;
                    var dashGridsElements = dashGrids.Children.GetEnumerator();
                    while (dashGridsElements.MoveNext())
                    {
                        if (isFirstElement)
                        {
                            ((HtmlElement)dashGridsElements.Current).Style = "display:none";
                            isFirstElement = false;
                            break;
                        }


                    }
                }

                SetAlternateRowsColor();

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
        /// Set Alternate Rows Color 
        /// </summary>
        private void SetAlternateRowsColor()
        {
            try
            {
                HtmlElementCollection alternetRow = webBrowser1.Document.GetElementsByTagName("tr");
                if (alternetRow != null)
                {
                    var rows = alternetRow.GetEnumerator();
                    while (rows.MoveNext())
                    {
                        var div = ((HtmlElement)rows.Current).GetAttribute("className");
                        if (div.Contains("ui-ig-altrecord"))
                        {
                            ((HtmlElement)rows.Current).Style = "background-color: #cbd6e0 !important; border: 1px solid black !important";
                        }
                        else
                        {
                            ((HtmlElement)rows.Current).Style = "background-color: #e7e8e9 !important; border: 1px solid black !important";
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
        /// Remove Menu And Footer view
        /// </summary>
        /// <param name="menu"></param>
        private void RemoveMenuAndFooter(HtmlElement menu)
        {
            try
            {
                var elements = menu.Children.GetEnumerator();
                while (elements.MoveNext())
                {

                    var div = ((HtmlElement)elements.Current).GetAttribute("className");
                    if (div.Contains("level1"))
                    {
                        ((HtmlElement)elements.Current).Style = "display:none";
                    }
                    else if (div.Contains("level2"))
                    {
                        ((HtmlElement)elements.Current).Style = "display:none";
                    }
                }

                HtmlElementCollection footer = webBrowser1.Document.GetElementsByTagName("footer");
                if (footer != null)
                {
                    var footerElements = footer.GetEnumerator();
                    while (footerElements.MoveNext())
                    {
                        ((HtmlElement)footerElements.Current).Style = "display:none";
                    }
                }


                HtmlElementCollection columns = webBrowser1.Document.GetElementsByTagName("td");
                if (columns != null)
                {
                    var column = columns.GetEnumerator();
                    while (column.MoveNext())
                    {
                        //var div = ((HtmlElement)column.Current).GetAttribute("className");
                        //if (div.Contains("ui-ig-altrecord"))
                        //{
                        ((HtmlElement)column.Current).Style = " border-top: 1px solid black !important";
                        //}
                        //else
                        //{
                        //    ((HtmlElement)column.Current).Style = " border-top: 1px solid black !important";
                        //}

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
        /// PMDashboard_Load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PMDashboard_Load(object sender, EventArgs e)
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(this, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_CLOSING_WIZARD);
                // usrCtrlCounterPartyStatusDetails1.UseAppStyling = false;
                this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"Text-align:Left\">" + this.Text + "</p>";
                this.ultraFormManager1.DrawFilter = new FormTitleHelper(this.Text, CustomThemeHelper.PRODUCT_COMPANY_NAME, CustomThemeHelper.UsedFont);
                this.MaximizeBox = false;
                this.MinimizeBox = false;
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
