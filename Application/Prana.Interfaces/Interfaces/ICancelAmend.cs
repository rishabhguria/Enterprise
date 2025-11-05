using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.Interfaces
{
    public interface ICancelAmend
    {
        System.Windows.Forms.Form Reference();
        event EventHandler FormClosed;

        Prana.BusinessObjects.CompanyUser User
        {
            get;
            set;
        }

    }
}
