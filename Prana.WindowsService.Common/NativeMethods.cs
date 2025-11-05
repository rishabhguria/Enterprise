using System;
using System.Runtime.InteropServices;

namespace Prana.WindowsService.Common
{
    internal class NativeMethods
    {
        internal delegate bool PranaServiceConsoleCloseHandler(CtrlType sig);

        [System.Runtime.InteropServices.DllImport("Kernel32")]
        internal static extern bool SetConsoleCtrlHandler(PranaServiceConsoleCloseHandler handler, bool add);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GetStdHandle(int hConsoleHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint mode);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint mode);

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