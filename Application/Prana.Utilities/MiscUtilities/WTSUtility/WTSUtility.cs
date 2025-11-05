using Prana.LogManager;
using System;
using System.Runtime.InteropServices;

namespace Prana.Utilities
{
    public class WTSUtility
    {
        #region Constants
        private const int WTS_CURRENT_SESSION = -1;
        #endregion

        #region Structures
        //Structure for Terminal Service Client IP Address
        [StructLayout(LayoutKind.Sequential)]
        private struct WTS_CLIENT_ADDRESS
        {
            public int iAddressFamily;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public byte[] bAddress;
        }

        //Structure for Terminal Service Session Info
        [StructLayout(LayoutKind.Sequential)]
        private struct WTS_SESSION_INFO
        {
            public int iSessionID;
            [MarshalAs(UnmanagedType.LPStr)]
            public string sWinsWorkstationName;
            public WTS_CONNECTSTATE_CLASS oState;
        }

        //Structure for Terminal Service Session Client Display
        [StructLayout(LayoutKind.Sequential)]
        private struct WTS_CLIENT_DISPLAY
        {
            public int iHorizontalResolution;
            public int iVerticalResolution;
            //1 = The display uses 4 bits per pixel for a maximum of 16 colors.
            //2 = The display uses 8 bits per pixel for a maximum of 256 colors.
            //4 = The display uses 16 bits per pixel for a maximum of 2^16 colors.
            //8 = The display uses 3-byte RGB values for a maximum of 2^24 colors.
            //16 = The display uses 15 bits per pixel for a maximum of 2^15 colors.
            public int iColorDepth;
        }
        #endregion

        #region Enumurations
        private enum WTS_CONNECTSTATE_CLASS
        {
            WTSActive,
            WTSConnected,
            WTSConnectQuery,
            WTSShadow,
            WTSDisconnected,
            WTSIdle,
            WTSListen,
            WTSReset,
            WTSDown,
            WTSInit
        }

        internal enum WTS_INFO_CLASS
        {
            WTSInitialProgram,
            WTSApplicationName,
            WTSWorkingDirectory,
            WTSOEMId,
            WTSSessionId,
            WTSUserName,
            WTSWinStationName,
            WTSDomainName,
            WTSConnectState,
            WTSClientBuildNumber,
            WTSClientName,
            WTSClientDirectory,
            WTSClientProductId,
            WTSClientHardwareId,
            WTSClientAddress,
            WTSClientDisplay,
            WTSClientProtocolType,
            WTSIdleTime,
            WTSLogonTime,
            WTSIncomingBytes,
            WTSOutgoingBytes,
            WTSIncomingFrames,
            WTSOutgoingFrames,
            WTSClientInfo,
            WTSSessionInfo,
            WTSConfigInfo,
            WTSValidationInfo,
            WTSSessionAddressV4,
            WTSIsRemoteSession
        }
        #endregion

        public static string GetLoggedInUserIPAddress(int sessionId)
        {
            IntPtr pSessionInfo = IntPtr.Zero;
            string sIPAddress = string.Empty;

            try
            {
                IntPtr pServer = IntPtr.Zero;

                int iCount = 0;
                int iReturnValue = NativeMethods.WTSEnumerateSessions(pServer, 0, 1, ref pSessionInfo, ref iCount);

                if (iReturnValue != 0)
                {

                    WTS_CLIENT_ADDRESS oClientAddres = new WTS_CLIENT_ADDRESS();

                    int iDataSize = Marshal.SizeOf(typeof(WTS_SESSION_INFO));
                    long iCurrent = (long)pSessionInfo;

                    //Go to all sessions
                    for (int i = 0; i < iCount; i++)
                    {
                        WTS_SESSION_INFO oSessionInfo = (WTS_SESSION_INFO)Marshal.PtrToStructure((System.IntPtr)iCurrent, typeof(WTS_SESSION_INFO));
                        iCurrent += iDataSize;

                        if (sessionId != oSessionInfo.iSessionID)
                        {
                            continue;
                        }

                        uint iReturned = 0;

                        IntPtr pAddress = IntPtr.Zero;
                        if (NativeMethods.WTSQuerySessionInformation(pServer, oSessionInfo.iSessionID, WTS_INFO_CLASS.WTSClientAddress, out pAddress, out iReturned) == true)
                        {
                            oClientAddres = (WTS_CLIENT_ADDRESS)Marshal.PtrToStructure(pAddress, oClientAddres.GetType());
                            sIPAddress = oClientAddres.bAddress[2] + "." + oClientAddres.bAddress[3] + "." + oClientAddres.bAddress[4] + "." + oClientAddres.bAddress[5];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                NativeMethods.WTSFreeMemory(pSessionInfo);
            }
            return sIPAddress;
        }
    }
}
