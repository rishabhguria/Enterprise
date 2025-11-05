using System;
using System.ComponentModel;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.ThirdParty
{
    [UITestFixture]
    public partial class ThirdPartyUIMap : UIMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThirdPartyUIMap"/> class.
        /// </summary>
        public ThirdPartyUIMap()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ThirdPartyUIMap"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public ThirdPartyUIMap(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        /// <summary>
        /// Deselect preselected column header of checkbox
        /// </summary>
        internal void DeselectGridCheckbox()
        {
            try
            {
                ColumnHeader.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Open Third Party Manager
        /// </summary>
        internal void OpenThirdPartyManager()
        {
            try
            {
                if (!PranaMain.IsVisible)
                {
                    ExtentionMethods.WaitForVisible(ref PranaMain, 40);
                }
                //Shortcut to open Third Party Manager (CTRL + ALT + T)
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_THIRD_PARTY"]);
                //Wait(5000);
                ExtentionMethods.WaitForVisible(ref FrmThirdParty, 15);
                //Tools.Click(MouseButtons.Left);
                //ThirdPartyManager.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Minimization Third Party Manager.
        /// </summary>
        internal void MinimizeThirdPartyManager()
        {
            try
            {
                KeyboardUtilities.MinimizeWindow(ref FrmThirdParty_UltraFormManager_Dock_Area_Top);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
        }


        /// <summary>
        /// Get last modified filename from Archives folder
        /// </summary>
        /// <returns></returns>
        internal string GetLastModifiedFileFromArchives()
        {
            string lastModifiedFile = "";
            try
            {
                string pattern = "*.txt";
                var dirInfo = new DirectoryInfo(ApplicationArguments.ClientReleasePath + "\\EOD\\ISOX\\Archives");
                var myfile = (from f in dirInfo.GetFiles(pattern) orderby f.LastWriteTime descending select f).First();

                lastModifiedFile=myfile.ToString();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return lastModifiedFile;
        }


        /// <summary>
        /// Get last modified filename from EOD folder
        /// </summary>
        /// <returns></returns>
        internal string GetLastModifiedFileFromEOD()
        {
            string lastModifiedFile = "";
            try
            {
                string pattern = "*.txt";
                var dirInfo = new DirectoryInfo(ApplicationArguments.ClientReleasePath + "\\EOD\\ISOX");
                var myfile = (from f in dirInfo.GetFiles(pattern) orderby f.LastWriteTime descending select f).First();
                lastModifiedFile = myfile.ToString();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return lastModifiedFile;
        }
    }
}
