using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Prana.Utilities.Win32Utilities
{
    /// <summary>
    /// 
    /// Summary description for WinUtilities.
    /// </summary>
    public class WinUtilities
    {

        // ----------------------------------------------------------------------------
        // ----------------------------------------------------------------------------
        // -- FUNCTION DEFINITIONS
        // ----------------------------------------------------------------------------
        // ----------------------------------------------------------------------------

        //These functions are part of the Windows API.

        #region Windows API Functions
        public static bool IsNetworkAlive()
        {
            int flags = 0;
            return SafeNativeMethods.IsNetworkAlive(out flags);
        }
        public static bool SetForegroundWindow(IntPtr hWnd)
        {
            return SafeNativeMethods.SetForegroundWindow(hWnd);
        }
        public static bool ShowWindowAsync(IntPtr hWnd, int nCmdShow)
        {
            return SafeNativeMethods.ShowWindowAsync(hWnd, nCmdShow);
        }
        public static bool LogonUser(string strUserName, string strDomainName, string strPwd, int logonType, int logonProvider, ref int token)
        {
            return SafeNativeMethods.LogonUser(strUserName, strDomainName, strPwd, logonType, logonProvider, ref token);
        }

        public static int GetPrivateProfileString(string lpApplicationName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName)
        {
            return SafeNativeMethods.GetPrivateProfileString(lpApplicationName, lpKeyName, lpDefault, lpReturnedString, nSize, lpFileName);
        }
        public static int GetWindowsDirectoryA(string lpBuffer, int nSize)
        {
            return SafeNativeMethods.GetWindowsDirectoryA(lpBuffer, nSize);
        }
        public static int GetDesktopWindow()
        {
            return SafeNativeMethods.GetDesktopWindow();
        }
        public static int GetWindow(int hWnd, int wFlag)
        {
            return SafeNativeMethods.GetWindow(hWnd, wFlag);
        }
        public static int GetWindowText(int hWnd, StringBuilder lpString, int cch)
        {
            return SafeNativeMethods.GetWindowText(hWnd, lpString, cch);
        }
        public static int GetWindowThreadProcessId(int hWnd, ref int lpdwProcessId)
        {
            return SafeNativeMethods.GetWindowThreadProcessId(hWnd, ref lpdwProcessId);
        }
        public static int GetParent(int hWnd)
        {
            return SafeNativeMethods.GetParent(hWnd);
        }
        public static int FindWindowA(string lpClassName, string lpWindowName)
        {
            return SafeNativeMethods.FindWindowA(lpClassName, lpWindowName);
        }
        public static int SendMessageA(int hWnd, int wMsg, int wParam, int lParam)
        {
            return SafeNativeMethods.SendMessageA(hWnd, wMsg, wParam, lParam);
        }
        public static int GetWindowLongA(int hWnd, int nIndex)
        {
            return SafeNativeMethods.GetWindowLongA(hWnd, nIndex);
        }
        //public const int SW_HIDE = 0; 
        public const int SW_SHOWNORMAL = 1;
        //public const int SW_SHOWMINIMIZED = 2; 
        //public const int SW_SHOWMAXIMIZED = 3; 
        //public const int SW_SHOWNOACTIVATE = 4; 
        //public const int SW_RESTORE = 9; 
        //public const int SW_SHOWDEFAULT = 10; 


        #endregion

        #region Functions based on Win32 functions


        /// <summary>
        /// Detects if the System is On Lan Currently or not
        /// </summary>
        /// <returns></returns>
        public static bool IsLanConnected()
        {
            try
            {
                //return false;
                int flags = 0;
                if (SafeNativeMethods.IsNetworkAlive(out flags))
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public static bool SigletonCheck()
        {
            //Only one instance of the process should run...
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            int nCurrentProcessID = current.Id;
            foreach (Process process in processes)
            {
                if (process.Id != nCurrentProcessID)
                {
                    try
                    {
                        if (process.MainModule.FileName.Equals(current.MainModule.FileName))
                        {
                            SafeNativeMethods.SetForegroundWindow(process.MainWindowHandle);
                            SafeNativeMethods.ShowWindowAsync(process.MainWindowHandle, WinUtilities.SW_SHOWNORMAL);
                            return false;
                        }
                    }
                    catch (Exception)
                    {
                        //Handle the execption
                    }
                }
            }

            return true;

        }

        #endregion

    }
    public static class SafeNativeMethods
    {
        #region Windows API Functions

        [DllImport("sensapi.dll")]
        internal static extern bool IsNetworkAlive(out int flags);

        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        //private static extern bool SetForegroundWindow(IntPtr hWnd);
        internal static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        //private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
        internal static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
        //private static extern bool LogonUser(string strUserName, string strDomainName, string strPwd, int logonType, int logonProvider ,ref  int token);
        //private static int LOGON32_LOGON_INTERACTIVE = 2;
        //private static int LOGON32_PROVIDER_DEFAULT = 0;
        internal static extern bool LogonUser(string strUserName, string strDomainName, string strPwd, int logonType, int logonProvider, ref int token);
        //internal static int LOGON32_LOGON_INTERACTIVE = 2;
        //internal static int LOGON32_PROVIDER_DEFAULT = 0;

        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode, EntryPoint = "GetPrivateProfileString")]
        //private static extern int GetPrivateProfileString(string lpApplicationName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);
        internal static extern int GetPrivateProfileString(string lpApplicationName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);

        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        //private static extern int GetWindowsDirectoryA(string lpBuffer, int nSize);
        internal static extern int GetWindowsDirectoryA(string lpBuffer, int nSize);

        // The following functions are used to launch and terminate the
        // eSignal Data Manager application.
        // These functions are not part of the eSignal Control or the standard
        // eSignal API.  These functions are part of the Windows API.

        [DllImport("User32.dll")]
        //private static extern int GetDesktopWindow();
        internal static extern int GetDesktopWindow();

        [DllImport("User32.dll", EntryPoint = "GetWindow")]
        //private static extern int GetWindow(int hWnd, int wFlag);
        internal static extern int GetWindow(int hWnd, int wFlag);

        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        //private static extern int GetWindowText(int hWnd, StringBuilder lpString, int cch);
        internal static extern int GetWindowText(int hWnd, StringBuilder lpString, int cch);

        [DllImport("User32.dll")]
        //private static extern int GetWindowThreadProcessId(int hWnd, ref int lpdwProcessId);
        internal static extern int GetWindowThreadProcessId(int hWnd, ref int lpdwProcessId);

        [DllImport("User32.dll")]
        //private static extern int GetParent(int hWnd);
        internal static extern int GetParent(int hWnd);

        [DllImport("User32.dll", CharSet = CharSet.Unicode, EntryPoint = "FindWindow")]
        //private static extern int FindWindowA(string lpClassName, string lpWindowName);
        internal static extern int FindWindowA(string lpClassName, string lpWindowName);

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        //private static extern int SendMessageA(int hWnd, int wMsg, int wParam, int lParam);
        internal static extern int SendMessageA(int hWnd, int wMsg, int wParam, int lParam);

        [DllImport("User32.dll", EntryPoint = "GetWindowLong")]
        //private static extern int GetWindowLongA(int hWnd, int nIndex);
        internal static extern int GetWindowLongA(int hWnd, int nIndex);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessageB(int hWnd, int wMsg, int wParam, int lParam);

        #endregion
    }
}
