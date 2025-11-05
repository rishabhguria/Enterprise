using System;

namespace Prana.Interfaces
{
    public interface IExceptionReport
    {
        System.Windows.Forms.Form Reference();
        event EventHandler FormClosed;
        void InitControl();
    }
}
