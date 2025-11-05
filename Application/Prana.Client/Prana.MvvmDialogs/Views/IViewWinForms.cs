using System;
using System.Windows.Forms;

namespace Prana.MvvmDialogs.Views
{
    public interface IViewWinForms
    {

        #region IsChildOfWinformControl
        Boolean IsChildOfWinformControl { get; set; }
        #endregion

        #region ParentWinformControl
        Form ParentWinformControl { get; set; }
        #endregion

    }
}
