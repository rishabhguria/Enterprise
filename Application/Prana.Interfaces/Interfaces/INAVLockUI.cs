using System;

namespace Prana.Interfaces
{
    public interface INAVLockUI
    {
        System.Windows.Forms.Form Reference();
        event EventHandler FormClosedHandler;
        Prana.BusinessObjects.CompanyUser LoginUser
        {
            get;
            set;
        }
    }
}
