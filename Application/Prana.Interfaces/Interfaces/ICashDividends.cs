using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.Interfaces
{
    public interface ICashDividends
    {
        System.Windows.Forms.Form Reference();
        event EventHandler FormClosedHandler;
        ISecurityMasterServices SecurityMaster
        {
            set;
        }
    }
}
