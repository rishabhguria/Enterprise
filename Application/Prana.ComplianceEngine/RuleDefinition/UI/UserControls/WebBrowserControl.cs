using mshtml;
using Prana.BusinessObjects.Compliance.Definition;
using Prana.BusinessObjects.Compliance.Delegates;
using Prana.BusinessObjects.Compliance.EventArguments;
using Prana.ComplianceEngine.RuleDefinition.DAL;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Prana.ComplianceEngine.RuleDefinition.UI.UserControls
{
    public partial class WebBrowserControl : UserControl, IWebBrowserControl
    {
        public event RuleBrowerSaveCompleteHandle RuleBrowerSaveCompleteEvent;

        private bool _isPostbackInitiated = false;
        public Uri Url { get; set; }

        public WebBrowserControl()
        {
            try
            {
                InitializeComponent();
                if (!(DesignMode || CustomThemeHelper.IsDesignMode()))
                {
                    this.Dock = DockStyle.Fill;
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
        ///  Event raised when guvnor completes save rule.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>S
        void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                if (RuleBrowerSaveCompleteEvent != null)
                    RuleBrowerSaveCompleteEvent(this, new BrowserLoadCompletedEventArgs { URI = e.Url.ToString(), IsSuccess = true, IsPostBack = _isPostbackInitiated });
                HtmlElement headElement = webBrowser.Document.GetElementsByTagName("head")[0];
                HtmlElement scriptElement = webBrowser.Document.CreateElement("script");
                IHTMLScriptElement element = (IHTMLScriptElement)scriptElement.DomElement;
                element.text = @"function checkInScript() {var buttons = document.getElementsByTagName('button');
                                        for (var i=0; i<buttons.length; i++) {
                                        if (buttons[i].firstChild.nodeValue == 'Check in')
                                        {
                                        buttons[i].click();
										 break;
                                        }
                                        } }";
                headElement.AppendChild(scriptElement);
            }
            catch (Exception)
            {
                if (RuleBrowerSaveCompleteEvent != null)
                    RuleBrowerSaveCompleteEvent(this, new BrowserLoadCompletedEventArgs { URI = e.Url.ToString(), IsSuccess = false, IsPostBack = _isPostbackInitiated });
                throw;
            }
            finally
            {
                _isPostbackInitiated = false;
                try
                {
                    webBrowser.IsWebBrowserContextMenuEnabled = false;
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



        #region Inherited members

        /// <summary>
        /// Save Rule
        /// </summary>
        /// <param name="rule"></param>
        public void SaveRule(RuleBase rule)
        {

            // TODO: Split the method into 2 smaller ones
            if (rule.Category == BusinessObjects.Compliance.Enums.RuleCategory.UserDefined)
            {
                try
                {
                    _isPostbackInitiated = true;
                    webBrowser.Document.GetElementById("SaveRuleBtn").InvokeMember("click");
                    webBrowser.Document.InvokeScript("checkInScript");
                }
                catch (Exception)
                {
                    MessageBox.Show(this, "Please wait until rule open.", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
            else if (rule.Category == BusinessObjects.Compliance.Enums.RuleCategory.CustomRule)
            {
                try
                {
                    List<Object> constants = new List<Object>();
                    foreach (HtmlElement el in webBrowser.Document.GetElementsByTagName("input"))
                    {
                        String name = el.GetAttribute("name");
                        String displayName = el.GetAttribute("displayName");
                        String type = el.GetAttribute("valuetype");
                        var value = webBrowser.Document.GetElementById(name).GetAttribute("value");
                        String comboList = String.Empty;
                        if (type.Equals("Combo"))
                            comboList = CustomRuleHandler.GetComboListforCustomRule(name, value);

                        var constant = new { name, value, displayName, type, comboList };
                        constants.Add(constant);

                        // validate the values
                        switch (type.ToLower())
                        {
                            case "int":
                                Convert.ToInt32(value);
                                break;
                            case "double":
                                Convert.ToDouble(value);
                                break;
                        }
                    }

                    foreach (HtmlElement el in webBrowser.Document.GetElementsByTagName("select"))
                    {
                        String name = el.GetAttribute("name");
                        String displayName = el.GetAttribute("displayName");
                        String type = el.GetAttribute("valuetype");
                        var value = webBrowser.Document.GetElementById(name).GetAttribute("value");
                        String comboList = String.Empty;
                        if (type.Equals("Combo"))
                            comboList = CustomRuleHandler.GetComboListforCustomRule(name, value);
                        var constant = new { name, value, displayName, type, comboList };
                        constants.Add(constant);
                    }

                    if (RuleBrowerSaveCompleteEvent != null)
                        RuleBrowerSaveCompleteEvent(this, new BrowserLoadCompletedEventArgs { URI = rule.RuleURL, IsSuccess = true, IsPostBack = true, Tag = System.Text.Encoding.UTF8.GetString(Prana.AmqpAdapter.Json.JsonHelper.Serialize(new { constantValues = constants })) });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }




        /// <summary>
        /// Navigate
        /// </summary>
        public void Navigate()
        {
            try
            {
                if (!webBrowser.IsDisposed)
                    webBrowser.Navigate(this.Url.AbsoluteUri);
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
        /// Focus On Browser
        /// </summary>
        public void FocusOnBrowser()
        {
            try
            {
                this.webBrowser.Focus();
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
    }
}
