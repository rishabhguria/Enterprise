using System;

namespace Prana.AdminTools
{
    public interface IPluggableTools
    {
        void SetUP();
        System.Windows.Forms.Form Reference();
        event EventHandler PluggableToolsClosed;
    }
}
