using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Prana.Global;
using Prana.Interfaces;
using Prana.IM.PranaIMService;
using App.DAC;

namespace Prana.IM
{
    public class PranaIM : IPranaIM
    {
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);
        [DllImport("user32.dll")]
        protected static extern bool SetWindowText(IntPtr hwnd, StringBuilder lpString);
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern int FindWindowA(string lpClassName, string lpWindowName);

        private const int SW_MINIMIZE = 6;
        private const int SW_RESTORE = 9;

        private string _userName = "";
        private IntPtr _pranaWindowHandle = (IntPtr)0;


        // This class Starts th Prana IM client
        public void StartPranaIM(int PranaCompanyUserID)
        {
            NIM chatClient = new NIM();

            DataSet user = chatClient.GetUser(PranaIMConfiguration.PranaIMUnitID, PranaCompanyUserID);

            if (user == null)
            {
                MessageBox.Show("User is not configured. Please call Prana Support.", "Prana Support", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                _userName = user.Tables[0].Rows[0]["UserName"].ToString();
                StartChatClient(_userName);
                //Thread.Sleep(3000); // SK 20071221 Wait for windowhandle to be created. This code could be dependent on machine performance.
                _pranaWindowHandle = (IntPtr)FindWindowA(Microsoft.VisualBasic.Constants.vbNullString, _userName + PranaIMConfiguration.SoapClientWindowTextFixedPart);
                //SetWindowText(_pranaWindowHandle, new StringBuilder(PranaIMConfiguration.PranaIMWindowTitle));
                ResizePranaIMWindow(FormWindowState.Normal);
            }
        }

        private void StartChatClient(string userName)
        {
            try
            {
                NimProxy.StopChatClient();

                NIM chatClient = new NIM();

                DataSet units = chatClient.GetUserUnits(userName);

                NimProxy.StartChatClient(userName, PranaIMConfiguration.PranaIMServerIPAddress, PranaIMConfiguration.PranaIMServerPort, units);

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void StopPranaIM()
        {
            NimProxy.StopChatClient();
        }

        public void ResizePranaIMWindow(FormWindowState fs)
        {
            uint winState = SW_MINIMIZE;
            if(fs != FormWindowState.Minimized)
            {
                winState = SW_RESTORE;
            }
            ShowWindow(_pranaWindowHandle, winState);
        }
    }
}
