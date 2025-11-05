using System;
using System.ComponentModel;

using TestAutomationFX.Core;
using TestAutomationFX.UI;
using System.Configuration;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nirvana.TestAutomation.Steps;
using System.Threading.Tasks;
using System.Data;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.BussinessObjects;
using System.Windows.Forms;

using System.IO;
using Nirvana.TestAutomation.Utilities;

using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.Factory;
using CommandType = Nirvana.TestAutomation.Interfaces.Enums.CommandType;
using System.Runtime.InteropServices;
using System.Threading;

namespace Nirvana.TestAutomation.Steps.Compliance
{
    [UITestFixture]
    public partial class PopUpUIMap : UIMap
    {
        public PopUpUIMap()
        {
            InitializeComponent();
        }

        public PopUpUIMap(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        // Activate an application window.
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        /// <summary>
        /// Function used to dump any file base on name given as string.
        /// </summary>
        /// <param name="windowName">Name of file that is going to dump.</param>
        public static void DumpWindow(string windowName)
        {
            try
            {
                IntPtr CmdHandle = FindWindow(null, "'Esper Calculation Engine'");
                if (CmdHandle == IntPtr.Zero)
                {
                    Console.WriteLine("Esper is not running.");
                }
                SetForegroundWindow(CmdHandle);
                Keyboard.SendKeys("dump -n " + windowName);
                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }
    }
}
