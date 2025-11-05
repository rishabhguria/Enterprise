using Prana.BusinessObjects;
using Prana.Global;
using System;

namespace Prana.Interfaces
{
    public interface IClosingUI
    {
        System.Windows.Forms.Form Reference();
        event EventHandler FormClosed;

        CompanyUser User
        {
            set;
        }
        ISecurityMasterServices SecurityMaster { set; }
        void SetUp(string ParentFormText, AllocationGroup group);
        //IClosingServices ClosingServices
        //{
        //    set;
        //}
        void ApplyPreferences(object sender, EventArgs<string, IPreferenceData> e);

        void ToggleUIElementsWithMessage(bool flag);

        void SetParentFormAndCreateProxies(bool SetParentForm);
    }
}