using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.Utilities;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using System.Data;
using System.Collections.Generic;
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.CorporateAction
{
    [UITestFixture]
    public partial class CorporateActionUIMap : UIMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CorporateActionUIMap"/> class.
        /// </summary>
        public CorporateActionUIMap()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CorporateActionUIMap"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public CorporateActionUIMap(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        /// <summary>
        /// Minimize Corporate Action
        /// </summary>
        public void MinimizeCorporateActionUI()
        {
            try
            {
                FrmCorporateActionNew_UltraFormManager_Dock_Area_Top.RightClick(540, 16);
                Keyboard.SendKeys("[DOWN][DOWN][DOWN][DOWN][ENTER]");
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;                
            }
           
        }
        /// <summary>
        /// Opens the corporate actions.
        /// </summary>
        public  void OpenCorporateActionsUI()
        {
            try
            {
                //PranaMain.WaitForVisible();
                if (!PranaMain.IsVisible)
                {
                    ExtentionMethods.WaitForVisible(ref PranaMain, 40);
                }
                //Shortcut to open closing module(CTRL + ALT + I)
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_CA"]);
                ExtentionMethods.WaitForVisible(ref FrmCorporateActionNew, 15);
 
                //Wait(7000);
                //PortfolioManagement.Click(MouseButtons.Left);
                //CorporateActions.Click(MouseButtons.Left);                
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }


        protected void ViewAllColumnsOnGrid(DataTable dTable)
        {
            try
            {
                List<string> columns = new List<string>();
                foreach (DataColumn item in dTable.Columns)
                {
                    columns.Add(item.ColumnName);
                }
                this.GrdPositions.InvokeMethod("AddColumnsToGrid", columns); 
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Opens the spin off from corporate action
        /// Not in use currently.
        /// </summary>
        public void OpenSpinOffUI()
        {
            try
            {
                PortfolioManagement.Click(MouseButtons.Left);
                CorporateActions.Click(MouseButtons.Left);
                CmbCATypeApply1.Click(MouseButtons.Left);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
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
