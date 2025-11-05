using Prana.LogManager;
using Prana.Utilities.Win32Utilities;
using System;
using System.Collections.Generic;

namespace Prana.OptionCalculator.Common
{
    /// <summary>
    /// Summary description for PublicConst.
    /// </summary>
    public class PublicConst
    {
        public PublicConst()
        {
        }
        // ----------------------------------------------------------------------------
        // -- ABOUT THIS MODULE
        // -- -----------------
        // --
        // -- This module contains global constants, public variables, and public
        // -- function used throughout this sample application.
        // --
        // ----------------------------------------------------------------------------

        // ----------------------------------------------------------------------------
        // -- DISCLAIMER AND DISTRIBUTION
        // -- ---------------------------
        // --
        // -- All code, programming styles, and architecture are provided for
        // -- demonstration purposes only, and may not be suitable for every project.
        // --
        // -- The code contained in this sample application may be used freely within
        // -- third-party applications if the following criteria are met:
        // --
        // -- 1. This sample code must be a part of a larger application.
        // -- 2. This sample code may not be packaged and distributed as a commercial
        // --    product.
        // ----------------------------------------------------------------------------

        /// <summary>
        /// Take the daylight saving time from the app.config file
        /// </summary>
        //public static int AddDayLightSavingHours
        //{
        //    get
        //    {
        //        //return Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["AddDayLightSavingHours"]);
        //        return Convert.ToInt32(ConfigurationHelper.Instance.GetAppSettingValueByKey("AddDayLightSavingHours"));
        //    }
        //}

        public const short ForReading = 1;
        public const short ForWriting = 2;
        public const short ForAppending = 3;

        public const short TristateUseDefault = -2;
        public const short TristateTrue = -1;
        public const short TristateFalse = 0;

        // Sets the number fo symbols to queue into the local eSignal Data Manager.
        // Used with the file method for sending symbols to the local eSignal Data
        // Manager from frmDm.
        public const short symbolsMax = 1000;

        // Sets the number of timers that will wait for non-updating symbols.
        // Used with the file method for sending symbols to the local eSignal Data
        // Manager from frmDm.
        public const short timersToWait = 100;

        // ----------------------------------------------------------------------------
        // -- Private Constants
        // ----------------------------------------------------------------------------

        // The following constants are used to find the eSignal Data Manager
        // application window, and to close the application.
        //
        //   NOTE:  This sample application only attempts to close the eSignal Data
        //          Manager when it has launched the eSignal Data Manager.  If the
        //          eSignal Data Manager was running prior to execution of this sample
        //          application, then the eSignal Data Manager is left running upon
        //          exit of this sample application.
        private const short GW_HWNDNEXT = 2;
        private const short GW_CHILD = 5;
        private const short GWL_HWNDPARENT = (-8);
        private const short WM_CLOSE = 0x10;
        private const string applicationName = "eSignal Data Manager";

        // ----------------------------------------------------------------------------
        // -- Public Variables
        // ----------------------------------------------------------------------------

        // Indicates if the eSignal Data Manager is running or not
        public static bool g_dmIsRunning = false;

        public static object g_fileinobject;
        public static object g_fileoutobject;
        public static object g_filein;
        public static object g_fileout;
        public static object g_streamin;
        public static object g_streamout;

        public static object g_fileticker;
        public static object g_filetickerobject;
        public static object g_streamticker;

        // Used for time & sales stream to file
        public static bool g_timesales;
        public static object g_tsFile;
        public static object g_tsObject;
        public static object g_tsStream;

        // Used for collecting all data to file
        public static bool g_saveall;
        public static object g_dataFile;
        public static object g_dataObject;
        public static object g_dataStream;

        public static bool g_fileprocess;
        public static bool g_filereading;
        public static bool g_nextsymbol;
        public static bool g_ticker;
        public static bool g_options;
        public static bool g_getroots;
        //UPGRADE_WARNING: Lower bound of array g_optionArray was changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1033"'
        public static string[] g_optionArray = new string[2001];
        public static short g_optionArrayCount;

        public static string g_symbol;
        public static short g_timercount;
        public static short g_symbolCount;
        public static string[] g_symbols = new string[symbolsMax + 1];

        // Used to ignore headline updates
        public static bool g_ignoreHeadlines;

        // The following public variables are used for the syncServer form
        public static bool g_syncTicker;
        public static bool g_syncDm;
        public static bool g_syncProcess;
        public static string g_syncCurrentSymbol;
        public static object g_syncFile;
        public static object g_syncFileObject;
        public static object g_syncStream;

        public static string g_username = string.Empty;
        public static string g_password = string.Empty;
        public static string g_internetAddress = string.Empty;

        // Used for the Ticker object
        public static object g_startTime;
        public static object g_endTime;
        public static object g_startDate;
        public static object g_endDate;
        public static object g_openTime;
        public static object g_openDate;
        public static object g_interval;

        // ----------------------------------------------------------------------------
        // -- TYPE DEFINITIONS
        // ----------------------------------------------------------------------------
        public struct FILETIME
        {
            public int dwLowDateTime;
            public int dwHighDateTime;
        }

        public struct SYSTEMTIME
        {
            public short wYear;
            public short wMonth;
            public short wDayOfWeek;
            public short wDay;
            public short wHour;
            public short wMinute;
            public short wSecond;
            public short wMilliseconds;
        }

        public static bool ParseINI()
        {
            try
            {

                List<string> lstCrenditials = new List<string>();
                lstCrenditials = ESignalCredentialManager.ReadESignalFile();
                if (lstCrenditials.Count > 0)
                {
                    g_username = lstCrenditials[0];
                    g_password = lstCrenditials[1];
                    g_internetAddress = lstCrenditials[2];
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        //Provide string with spaces
        private static string Space(int no)
        {
            try
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i <= no; i++)
                    sb.Append(" ");
                return Convert.ToString(sb);
            }
            catch (Exception ex)
            {
                Logger.LoggerWrite("Could not Provide string with spaces " + "\nDescription " + ex.Message + " " + ex.Source);
                return string.Empty;
            }
        }

        // --------------------------------------------------------------------------
        // Function: Gets the window handle for the local eSignal Data Manager.
        //
        //    Usage: Used when the sample application attempts to shut down the
        //           local eSignal Data Manager.
        //
        //  Returns: The handle to the local eSignal Data Manager application
        //           window.
        // --------------------------------------------------------------------------

        private static int InstanceToWnd(int target_pid)
        {
            int tempInstanceToWnd = 0;
            int test_hwnd = 0;
            int test_pid = 0;
            int test_thread_id = 0;

            test_hwnd = WinUtilities.FindWindowA(Microsoft.VisualBasic.Constants.vbNullString, "eSignal Data Manager");

            while (test_hwnd != 0)
            {
                if (WinUtilities.GetParent(test_hwnd) == 0)
                {
                    test_thread_id = WinUtilities.GetWindowThreadProcessId(test_hwnd, ref test_pid);
                    if (test_pid == target_pid)
                    {
                        tempInstanceToWnd = test_hwnd;
                        break;
                    }
                }
                test_hwnd = WinUtilities.GetWindow(test_hwnd, GW_HWNDNEXT);
            }
            return tempInstanceToWnd;
        }

        // --------------------------------------------------------------------------
        // Function: Converts a date into a SYSTEMTIME data structure.
        //
        //    Usage: Used by the Ticker objects to display the time properly.
        //
        //  Returns: A SYSTEMTIME structure by reference.
        //
        //     Note: Code from http://www.vb-helper.com/howto_utc_to_local_time.html
        // --------------------------------------------------------------------------
        private static void DateToSystemTime(DateTime the_date, ref SYSTEMTIME system_time)
        {
            try
            {
                SYSTEMTIME with_1 = system_time;
                with_1.wYear = (short)the_date.Year;
                with_1.wMonth = (short)the_date.Month;
                with_1.wDay = (short)the_date.Day;
                with_1.wHour = (short)the_date.Hour;
                with_1.wMinute = (short)the_date.Minute;
                with_1.wSecond = (short)the_date.Second;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        // --------------------------------------------------------------------------
        // Function: Converts a SYSTEMTIME into a date.
        //
        //    Usage: Used by the Ticker objects to display the time properly.
        //
        //  Returns: A Date structure by reference.
        //
        //     Note: Code from http://www.vb-helper.com/howto_utc_to_local_time.html
        // --------------------------------------------------------------------------

        //2 digit formatting check
        private void SystemTimeToDate(ref SYSTEMTIME system_time, ref DateTime the_date)
        {
            try
            {
                string s = system_time.wMonth + "/" + system_time.wDay + "/" + system_time.wYear + " " + system_time.wHour + ":" + string.Format("{0:00}", system_time.wMinute) + ":" + string.Format("{0:00}", system_time.wSecond);
                the_date = Convert.ToDateTime(s);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        // --------------------------------------------------------------------------
        // Function: Converts a UTC time to local time.
        //
        //    Usage: Used by the Ticker objects to display the time properly.
        //
        //  Returns: The local time for a given UTC time.
        //
        //     Note: Code from http://www.vb-helper.com/howto_utc_to_local_time.html
        // --------------------------------------------------------------------------

        internal static DateTime UTCToLocalTime(DateTime the_date)
        {
            the_date = the_date.ToLocalTime();
            return the_date;
        }
    }
}
