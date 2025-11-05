//using System;
//using System.Runtime.InteropServices;
//using IESignal;
//using Prana.Interfaces;

//namespace Prana.LiveFeed.UI
//{

//    class eSignalDesktopAPI : IeSignalDesktopAPI
//    {
//        [DllImport("user32.dll")] 
//        static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);

//        [DllImport("kernel32.dll")]
//        static extern long GetLastError();

//        private const int SW_MINIMIZE = 6;
//        private const int SW_HIDE = 0;

//        private HooksClass eSignal =   new HooksClass();
//        private IntPtr eSignalHWND = (IntPtr)0;

//        public HooksClass eSignalApp
//        {
//            get
//            {
//                return eSignal;
//            }
//        }

//        public IntPtr eSignalAppHWND
//        {
//            get
//            {
//                return eSignalHWND;
//            }
//        }

//        public eSignalDesktopAPI()
//        {
//            InitializeeSignal();
//        }

//        public void ShowCharts(string Symbol)
//        {
//            eSignal.CreateNewWindow(windowTypeFlags.wtfADVANCEDCHART, 100, 100, 400, 400, 1, Symbol);
//            MinimizeeSignal();
//        }

//        public void MinimizeeSignal()
//        {
//            ShowWindow(eSignalHWND, SW_MINIMIZE);
//        }

//        private void InitializeeSignal()
//        {
//            eSignal.SetApplication("Prana1");
//            eSignalHWND = (IntPtr)eSignal.GetAppHWND;
//            MinimizeeSignal();
//        }
//    }
//}
