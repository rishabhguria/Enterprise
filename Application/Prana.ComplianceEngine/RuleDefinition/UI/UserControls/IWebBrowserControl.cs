using Prana.BusinessObjects.Compliance.Definition;
using Prana.BusinessObjects.Compliance.Delegates;
using System;

namespace Prana.ComplianceEngine.RuleDefinition.UI.UserControls
{
    public interface IWebBrowserControl : IDisposable
    {

        //Uri Url { get; set; }

        //public IWebBrowserControl()
        //{
        // this.Dock = DockStyle.Fill;
        //}

        void Navigate();

        void FocusOnBrowser();

        void SaveRule(RuleBase rule);
        event RuleBrowerSaveCompleteHandle RuleBrowerSaveCompleteEvent;
    }
}
