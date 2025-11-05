using System;
using System.Runtime.InteropServices;

namespace Prana.Utilities
{
    public class NativeMethods
    {
        [DllImport("wtsapi32.dll")]
        internal static extern int WTSEnumerateSessions(
            IntPtr pServer,
            [MarshalAs(UnmanagedType.U4)] int iReserved,
            [MarshalAs(UnmanagedType.U4)] int iVersion,
            ref IntPtr pSessionInfo,
            [MarshalAs(UnmanagedType.U4)] ref int iCount);

        [DllImport("Wtsapi32.dll")]
        internal static extern bool WTSQuerySessionInformation(
            System.IntPtr pServer,
            int iSessionID,
            WTSUtility.WTS_INFO_CLASS oInfoClass,
            out System.IntPtr pBuffer,
            out uint iBytesReturned);

        [DllImport("wtsapi32.dll")]
        internal static extern void WTSFreeMemory(
            IntPtr pMemory);
    }
}
