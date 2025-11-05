#pragma warning disable 1591

using System;
using System.Runtime.InteropServices;

namespace Prana.ESignalAdapter
{
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct cidFieldData
    {
        /// double
        internal double m_value;

        /// double
        internal double m_size;

        /// TIME_T->int
        internal int m_time;

        /// DWORD32->unsigned int
        internal uint m_sequence;

        /// WORD->unsigned short
        internal ushort m_lineid;

        /// BYTE->unsigned char
        internal byte m_series;

        /// BYTE->unsigned char
        internal byte m_flags;

        /// unsigned char[4]
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 4)]
        internal string m_qual;

        /// unsigned char[4]
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 4)]
        internal string m_exg;
    }

    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct cidPriceFieldData
    {

        /// DWORD32->unsigned int
        internal uint m_sequence;

        /// double
        internal double m_value;

        /// TIME_T->int
        internal int m_time;

        /// WORD->unsigned short
        internal ushort m_lineid;

        /// BYTE->unsigned char
        internal byte m_series;

        /// BYTE->unsigned char
        internal byte m_flags;

        /// unsigned char[4]
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 4)]
        internal string m_qual;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    internal struct cidData
    {

        /// DWORD32->unsigned int
        internal uint m_sequence;

        /// WORD->unsigned short
        internal ushort m_lineid;

        /// BYTE->unsigned char
        internal byte m_series;

        /// BYTE->unsigned char
        internal byte m_flags;

        /// TIME_T->int
        internal int m_beacon;

        /// TIME_T->int
        internal int m_exgtime;

        /// BYTE[4]
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
        internal byte[] m_secqual;
    }

    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct dbc_api_version
    {

        /// char[128]
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 128)]
        internal string szDescription;

        /// WORD->unsigned short
        internal ushort wBuildNumber;

        /// char
        internal byte cBuildType;

        /// WORD->unsigned short
        internal ushort wApiSupport;

        /// char[255]
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 255)]
        internal string reserved;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    internal struct dbc_api_version2
    {
        /// char[32]
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 32)]
        internal string szProductVersion;

        /// char[32]
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 32)]
        internal string szBuildDate;

        /// WORD->unsigned short
        internal ushort wMajorBuildNumber;

        /// WORD->unsigned short
        internal ushort wMinorBuildNumber;

        /// char[128]
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 128)]
        internal string szDescription;

        /// char
        internal byte cBuildType;

        /// WORD->unsigned short
        internal ushort wApiSupport;

        /// char[255]
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 1)]
        internal string cPadding;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    internal struct Anonymous_8c79199d_c013_4ed8_8e6f_bd4329c85492
    {

        /// BYTE->unsigned char
        internal byte nMonth;

        /// BYTE->unsigned char
        internal byte nDay;

        /// WORD->unsigned short
        internal ushort nYear;
    }

    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct dbc_application_version
    {

        /// char[13]
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 13)]
        internal string szProgramName;

        /// WORD->unsigned short
        internal ushort wBuildNumber;

        /// Anonymous_8c79199d_c013_4ed8_8e6f_bd4329c85492
        internal Anonymous_8c79199d_c013_4ed8_8e6f_bd4329c85492 sDate;

        /// char[5]
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 5)]
        internal string szMisc;
    }

    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct DM_MESSAGE_EX
    {
        /// char[56]
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 56)]
        internal string szText1;

        /// char[256]
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 256)]
        internal string szText2;

        /// int
        internal int iStatusType;

        // DWORD32->unsigned int
        internal uint dwBitmap;

        /// DWORD32[4]
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U4)]
        internal uint[] dwArgs;
    }

    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct DM_MESSAGE
    {
        /// char[56]
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 56)]
        internal string szText1;

        /// char[256]
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 256)]
        internal string szText2;

        /// int
        internal int iStatusType;

    }

    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct LAST_RECORD_TYPE
    {
        /// BYTE*
        //[MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)]
        internal IntPtr pBuffer;

        /// BYTE*
        //[MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)]
        internal IntPtr pData;

        /// UINT->unsigned int
        internal uint nDataLength;

        /// UINT->unsigned int
        internal uint nAllocLength;

        /// UINT->unsigned int
        internal uint nDataRead;
    }

    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct connection_status
    {

        /// char[64]
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 64)]
        internal string szServerVersion;

        /// TIME_T->int
        internal int tConnect;

        /// ULONG->unsigned int
        internal uint lBytesReceived;

        /// ULONG->unsigned int
        internal uint lSequenceErrors;

        /// ULONG->unsigned int
        internal uint lOutOfSyncs;

        /// ULONG->unsigned int
        internal uint lRecords;

        /// ULONG->unsigned int
        internal uint lGoodResponse;

        /// ULONG->unsigned int
        internal uint lBadResponse;

        /// ULONG->unsigned int
        internal uint lHeadlineResponse;

        /// int
        internal int iReceptionState;

        /// int
        internal int iPasswordState;

        /// int
        internal int iConnectionState;
    }

    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct DMSymbolKey
    {

        /// char[80]
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 80)]
        internal string szKey;

        /// int
        internal int iKeyType;
    }

    internal partial class NativeMethods
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetEvent(IntPtr hEvent);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr CreateEvent(IntPtr lpEventAttributes, bool bManualReset, bool bInitialState, string lpName);

        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr OpenEvent(UInt32 dwDesiredAccess,
                bool bInheritHandle, String lpName);

        [DllImport("kernel32", SetLastError = true, ExactSpelling = true)]
        internal static extern Int32 WaitForSingleObject(IntPtr handle, Int32 milliseconds);

        /// Return Type: int
        ///wClientBuild: WORD->unsigned short
        ///pOrigName: char*
        ///pVersion: DbcApiVersion*
        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true, EntryPoint = "DbcInitialize")]
        internal static extern int DbcInitialize(ushort wClientBuild, [InAttribute()][MarshalAsAttribute(UnmanagedType.LPStr)] string pOrigName, ref dbc_api_version pVersion);


        /// Return Type: int
        ///pHostName: char*
        ///wPort: WORD->unsigned short
        ///iVersion: int
        ///pUsername: char*
        ///pPassword: char*
        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "DbcInitializeProxySettings")]
        internal static extern int DbcInitializeProxySettings(IntPtr pHostName, ushort wPort, int iVersion, IntPtr pUsername, IntPtr pPassword);


        /// Return Type: int
        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "DbcCleanup")]
        internal static extern int DbcCleanup();


        /// Return Type: int
        ///pHostName: char*
        ///pUserName: char*
        ///pPassword: char*
        ///iConnectionType: int
        ///pConnection: DBCCONNECTION*
        ///hNotificationEvent: HANDLE->void*
        ///hNotificationWindow: HWND->HWND__*
        ///dwNotificationMessage: DWORD32->unsigned int
        ///dwFlags: DWORD32->unsigned int
        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true, EntryPoint = "DbcOpenConnection")]
        internal static extern int DbcOpenConnection([InAttribute()][MarshalAsAttribute(UnmanagedType.LPStr)] string pHostName, [InAttribute()][MarshalAsAttribute(UnmanagedType.LPStr)] string pUserName, [InAttribute()][MarshalAsAttribute(UnmanagedType.LPStr)] string pPassword, int iConnectionType, ref IntPtr pConnection, IntPtr hNotificationEvent, IntPtr hNotificationWindow, uint dwNotificationMessage, uint dwFlags);


        /// Return Type: int
        ///Connection: DBCCONNECTION->void*
        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "DbcCloseConnection")]
        internal static extern int DbcCloseConnection(IntPtr Connection);


        /// Return Type: int
        ///Connection: DBCCONNECTION->void*
        ///pType: int*
        ///pStatus: int*
        ///pBuffer: char*
        ///pLength: UINT*
        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "DbcGetMessage")]
        internal static extern int DbcGetMessage(IntPtr Connection, ref int pType, ref int pStatus, IntPtr pBuffer, ref uint pLength);


        /// Return Type: int
        ///Connection: DBCCONNECTION->void*
        ///pType: int*
        ///pStatus: int*
        ///pKey: char*
        ///pKeyLength: UINT*
        ///pBuffer: char*
        ///pBufferLength: UINT*
        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "DbcGetMessageEx")]
        internal static extern int DbcGetMessageEx(IntPtr Connection, ref int pType, ref int pStatus, IntPtr pKey, ref uint pKeyLength, IntPtr pBuffer, ref uint pBufferLength);


        /// Return Type: int
        ///bEnableLog: BOOL->int
        ///pLogFileName: char*
        ///bEnableSyslog: BOOL->int
        ///dwLogMask: DWORD32->unsigned int
        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "DbcEnableDebug")]
        internal static extern int DbcEnableDebug([MarshalAsAttribute(UnmanagedType.Bool)] bool bEnableLog, IntPtr pLogFileName, [MarshalAsAttribute(UnmanagedType.Bool)] bool bEnableSyslog, uint dwLogMask);


        /// Return Type: int
        ///Connection: DBCCONNECTION->void*
        ///pStatus: connection_status*
        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "DbcGetStatus")]
        internal static extern int DbcGetStatus(IntPtr Connection, ref connection_status pStatus);


        /// Return Type: int
        ///Connection: DBCCONNECTION->void*
        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "DbcResetStatus")]
        internal static extern int DbcResetStatus(IntPtr Connection);

        /// Return Type: int
        ///wClientBuild: WORD->unsigned short
        ///pAppVer: DbcApplicationVersion*
        ///pVersion: DbcApiVersion*
        [System.Runtime.InteropServices.DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "DbcInitializeEx")]
        public static extern int DbcInitializeEx(ushort wClientBuild, ref dbc_application_version pAppVer, ref dbc_api_version pVersion);

        /// Return Type: int
        ///pAppVer: DbcApplicationVersion*
        ///pVersion: DbcApiVersion*
        [System.Runtime.InteropServices.DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, EntryPoint = "DbcInitializeEx2")]
        public static extern int DbcInitializeEx2(ref dbc_application_version pAppVer, ref dbc_api_version2 pVersion);

        /// Return Type: intDbcGetMessageMx
        ///pszSymbol: char*
        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true, EntryPoint = "DbcIsInternationalSymbol")]
        internal static extern int DbcIsInternationalSymbol([InAttribute()][MarshalAsAttribute(UnmanagedType.LPStr)] string pszSymbol);


        /// Return Type: int
        ///Connection: DBCCONNECTION->void*
        ///pType: int*
        ///pStatus: int*
        ///pKey: char*
        ///pKeyLength: UINT*
        ///pBuffer: char*
        ///pBufferLength: UINT*
        ///uncompressBuffer: boolean
        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "DbcGetMessageMx")]
        internal static extern int DbcGetMessageMx(IntPtr Connection, ref int pType, ref int pStatus, IntPtr pKey, ref uint pKeyLength, IntPtr pBuffer, ref uint pBufferLength, [MarshalAsAttribute(UnmanagedType.I1)] bool uncompressBuffer);

        /// Return Type: int
        ///Connection: DBCCONNECTION->void*
        ///pSymbol: char*
        ///bRequest: int
        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true, EntryPoint = "DbcAddDMSymbol")]
        internal static extern int DbcAddDMSymbol(IntPtr Connection, [InAttribute()][MarshalAsAttribute(UnmanagedType.LPStr)] string pSymbol, int bRequest);


        /// Return Type: int
        ///Connection: DBCCONNECTION->void*
        ///pSymbol: char*
        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true, EntryPoint = "DbcDeleteDMSymbol")]
        internal static extern int DbcDeleteDMSymbol(IntPtr Connection, [InAttribute()][MarshalAsAttribute(UnmanagedType.LPStr)] string pSymbol);


        /// Return Type: int
        ///Connection: DBCCONNECTION->void*
        ///pSymbol: char*
        ///iType: int
        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true, EntryPoint = "DbcDeleteDMSymbolEx")]
        internal static extern int DbcDeleteDMSymbolEx(IntPtr Connection, [InAttribute()][MarshalAsAttribute(UnmanagedType.LPStr)] string pSymbol, int iType);


        /// Return Type: int
        ///Connection: DBCCONNECTION->void*
        ///pszIdentifier: char*
        ///iConversionType: int
        ///pszLRTKey: char*
        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true, EntryPoint = "DbcRequestKey")]
        internal static extern int DbcRequestKey(IntPtr Connection, [InAttribute()][MarshalAsAttribute(UnmanagedType.LPStr)] string pszIdentifier, int iConversionType, IntPtr pszLRTKey);


        /// Return Type: int
        ///Connection: DBCCONNECTION->void*
        ///pszIdentifier: char*
        ///iConversionType: int
        ///pszKey: char*
        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true, EntryPoint = "DbcGetKey")]
        internal static extern int DbcGetKey(IntPtr Connection, [InAttribute()][MarshalAsAttribute(UnmanagedType.LPStr)] string pszIdentifier, int iConversionType, IntPtr pszKey);


        /// Return Type: int
        ///Connection: DBCCONNECTION->void*
        ///pszIdentifier: char*
        ///iConversionType: int
        ///pKeyArray: DMSymbolKey*
        ///pKeyCount: int*
        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true, EntryPoint = "DbcGetKeyEx")]
        internal static extern int DbcGetKeyEx(IntPtr Connection, [InAttribute()][MarshalAsAttribute(UnmanagedType.LPStr)] string pszIdentifier, int iConversionType, ref DMSymbolKey pKeyArray, ref int pKeyCount);


        /// Return Type: int
        ///Connection: DBCCONNECTION->void*
        ///pszIdentifier: char*
        ///pKeyArray: DMSymbolKey*
        ///pKeyCount: int*
        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true, EntryPoint = "DbcGetKeys")]
        internal static extern int DbcGetKeys(IntPtr Connection, [InAttribute()][MarshalAsAttribute(UnmanagedType.LPStr)] string pszIdentifier, ref DMSymbolKey pKeyArray, ref int pKeyCount);


        /// Return Type: int
        ///Connection: DBCCONNECTION->void*
        ///pszIdentifier: char*
        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true, EntryPoint = "DbcIsValidSymbol")]
        internal static extern int DbcIsValidSymbol(IntPtr Connection, [InAttribute()][MarshalAsAttribute(UnmanagedType.LPStr)] string pszIdentifier);


        /// Return Type: int
        ///Connection: DBCCONNECTION->void*
        ///pszIdentifier: char*
        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true, EntryPoint = "DbcIsValidCusip")]
        internal static extern int DbcIsValidCusip(IntPtr Connection, [InAttribute()][MarshalAsAttribute(UnmanagedType.LPStr)] string pszIdentifier);


        /// Return Type: int
        ///Connection: DBCCONNECTION->void*
        ///pszIdentifier: char*
        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true, EntryPoint = "DbcIsValidSedol")]
        internal static extern int DbcIsValidSedol(IntPtr Connection, [InAttribute()][MarshalAsAttribute(UnmanagedType.LPStr)] string pszIdentifier);


        /// Return Type: int
        ///Connection: DBCCONNECTION->void*
        ///pszIdentifier: char*
        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true, EntryPoint = "DbcIsValidFtex")]
        internal static extern int DbcIsValidFtex(IntPtr Connection, [InAttribute()][MarshalAsAttribute(UnmanagedType.LPStr)] string pszIdentifier);


        /// Return Type: int
        ///Connection: DBCCONNECTION->void*
        ///pszIdentifier: char*
        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true, EntryPoint = "DbcIsValidIsin")]
        internal static extern int DbcIsValidIsin(IntPtr Connection, [InAttribute()][MarshalAsAttribute(UnmanagedType.LPStr)] string pszIdentifier);


        /// Return Type: int
        ///Connection: DBCCONNECTION->void*
        ///pszIdentifier: char*
        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true, EntryPoint = "DbcIsValidValoren")]
        internal static extern int DbcIsValidValoren(IntPtr Connection, [InAttribute()][MarshalAsAttribute(UnmanagedType.LPStr)] string pszIdentifier);

        /// Return Type: int
        ///Connection: DBCCONNECTION->void*
        ///pszIdentifier: char*
        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true, EntryPoint = "DbcIsValidWKN")]
        internal static extern int DbcIsValidWKN(IntPtr Connection, [InAttribute()][MarshalAsAttribute(UnmanagedType.LPStr)] string pszIdentifier);

        /// Return Type: void
        ///iErrorCode: int
        ///pString: char*
        ///nSize: int
        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "DbcGetErrorMessage")]
        internal static extern void DbcGetErrorMessage(int iErrorCode, IntPtr pString, int nSize);

        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LrtMoveNext")]
        internal static extern int LrtMoveNext(IntPtr pLrt);

        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LrtMoveFirst")]
        internal static extern int LrtMoveFirst(IntPtr pLrt);

        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LrtFind")]
        internal static extern int LrtFind(IntPtr pLrt, uint wFindType, uint wFindFmt);

        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LrtCreate")]
        internal static extern int LrtCreate(uint wFindType, IntPtr pLrt);

        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LrtRelease")]
        internal static extern void LrtRelease(IntPtr pLrt);

        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LrtLoadData")]
        internal static extern int LrtLoadData(IntPtr pLrt, IntPtr pData, uint wFindType);

        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LrtGetByte")]
        internal static extern byte LrtGetByte(IntPtr pLrt);

        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LrtGetDouble")]
        internal static extern double LrtGetDouble(IntPtr pLrt);

        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LrtGetString")]
        internal static extern IntPtr LrtGetString(IntPtr pLrt, IntPtr pStringBuffer, ref int pdwLength);

        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LrtGetLong")]
        internal static extern int LrtGetLong(IntPtr pLrt);

        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LrtGetBLLong")]
        internal static extern int LrtGetBLLong(IntPtr pLrt);

        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LrtGetBLBase")]
        internal static extern byte LrtGetBLBase(IntPtr pLrt);

        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "DbcLongBaseToDouble")]
        internal static extern double DbcLongBaseToDouble(int lPrice, int iBase);

        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LrtGetType")]
        internal static extern ushort LrtGetType(IntPtr pLrt);

        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LrtGetFieldCid")]
        internal static extern int LrtGetFieldCid(IntPtr pLrt, IntPtr ps);

        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LrtGetFormat")]
        internal static extern ushort LrtGetFormat(IntPtr pLrt);

        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LrtGetStringArray")]
        internal static extern IntPtr LrtGetStringArray(IntPtr pLrt, IntPtr pBuffer, ref uint pdwLength);

        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LrtGetCid")]
        internal static extern int LrtGetCid(IntPtr pLrt, IntPtr ps);

        [DllImportAttribute("Esignal Dlls\\dbcapi.dll", CallingConvention = CallingConvention.Cdecl, BestFitMapping = false, ThrowOnUnmappableChar = true, EntryPoint = "DbcGetKeys")]
        internal static extern int DbcGetKeys(IntPtr connection, string symbol, IntPtr ps, ref int count);
    }
}