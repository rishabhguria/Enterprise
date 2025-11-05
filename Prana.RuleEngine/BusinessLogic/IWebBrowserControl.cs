using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.RuleEngine.BusinessLogic
{
    interface IWebBrowserControl
    {
      void GetWebBrowserControl();
      void Navigate(String Url);

        void FocusOnBrowser();

        void InvokeSaveBtnOfGuvnor();
  }
}
