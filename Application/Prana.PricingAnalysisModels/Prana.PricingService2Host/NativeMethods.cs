using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.PricingConsoleHost
{
    internal class NativeMethods
    {
        //internal delegate void HandlerRoutine();

        //[System.Runtime.InteropServices.DllImport("Kernel32")]
        //internal static extern bool SetConsoleCtrlHandler(HandlerRoutine Handler);

        internal delegate bool PricingService2ConsoleEventHandler(CtrlType sig);

        [System.Runtime.InteropServices.DllImport("Kernel32")]
        internal static extern bool SetConsoleCtrlHandler(PricingService2ConsoleEventHandler handler, bool add);

        internal enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

    }
}