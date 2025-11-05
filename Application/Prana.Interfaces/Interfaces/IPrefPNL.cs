using System;

namespace Prana.Interfaces
{
    /// <summary>
    /// Summary description for IPrefPNL.
    /// </summary>
    public interface IPrefPNL
    {
        System.Windows.Forms.UserControl Reference();

        event EventHandler SaveClick;

        event EventHandler ApplyPreferences;
    }
}
