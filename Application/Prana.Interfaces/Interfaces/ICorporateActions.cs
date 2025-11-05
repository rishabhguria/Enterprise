using Prana.BusinessObjects;
using System;

namespace Prana.Interfaces
{
    public interface ICorporateActions
    {
        System.Windows.Forms.Form Reference();
        event EventHandler FormClosed;
        void InitControl();
        CompanyUser LoginUser
        {
            get;
            set;
        }

        event EventHandler LaunchSymbolLookup;
    }
}
