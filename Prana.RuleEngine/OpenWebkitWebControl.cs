using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Prana.RuleEngine.BusinessLogic;
using WebKit;
using WebKit.Interop;

namespace Prana.RuleEngine
{
    public partial class OpenWebkitWebControl : System.Windows.Forms.UserControl, IWebBrowserControl
    {
        public OpenWebkitWebControl()
        {
            InitializeComponent();
            InitializeJSEvents();
            webBrowser.UseJavaScript = true;
            
        }

        private void InitializeJSEvents()
        {
            try
            {
                webBrowser.ShowJavaScriptAlertPanel += new ShowJavaScriptAlertPanelEventHandler(webKitBrowser1_ShowJavaScriptAlertPanel);
                webBrowser.ShowJavaScriptConfirmPanel += new ShowJavaScriptConfirmPanelEventHandler(webKitBrowser1_ShowJavaScriptConfirmPanel);
                //webKitBrowser1.ShowJavaScriptPromptBeforeUnload += new ShowJavaScriptPromptBeforeUnloadEventHandler(webKitBrowser1_ShowJavaScriptPromptBeforeUnload);
                webBrowser.ShowJavaScriptPromptPanel += new ShowJavaScriptPromptPanelEventHandler(webKitBrowser1_ShowJavaScriptPromptPanel);
           
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        #region IWebBrowserControl Members

        public void GetWebBrowserControl()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Navigate(String Url)
        {
            webBrowser.Navigate(Url);
        }

        public void FocusOnBrowser()
        {
            this.webBrowser.Focus();
        }

        #endregion


        private void webKitBrowser1_ShowJavaScriptPromptPanel(object sender, WebKit.ShowJavaScriptPromptPanelEventArgs e)
        {
            MessageBox.Show("Not Impplemented, Please contact admin");
            //  e.ReturnValue = Microsof.Interaction.InputBox(e.Message, "", e.DefaultValue);
        }
        private void webKitBrowser1_ShowJavaScriptAlertPanel(object sender, WebKit.ShowJavaScriptAlertPanelEventArgs e)
        {
            MessageBox.Show(e.Message);
        }
        private void webKitBrowser1_ShowJavaScriptConfirmPanel(object sender, WebKit.ShowJavaScriptConfirmPanelEventArgs e)
        {
            DialogResult dr = MessageBox.Show(e.Message, "Nirvana Compliance", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

            if (dr == DialogResult.OK)
            {
                e.ReturnValue = true;
            }
            else
            {
                e.ReturnValue = false;
            }
          //  bool val = (MessageBox.Show(e.Message, "Nirvana Compliance", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK);
           // e.ReturnValue = dr;
        }

        #region IWebBrowserControl Members


        public void InvokeSaveBtnOfGuvnor()
        {
            try
            {
                webBrowser.StringByEvaluatingJavaScriptFromString("document.getElementById('SaveRuleBtn').click()");
              
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Please wait until rule open.", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Asterisk); 
                //throw;
            }
        }

        #endregion
    }
}
