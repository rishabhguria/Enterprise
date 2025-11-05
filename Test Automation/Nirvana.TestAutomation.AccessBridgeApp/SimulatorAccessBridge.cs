using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.AccessBridgeApp;
using Nirvana.TestAutomation.AccessBridgeApp.Win32;
using System.ServiceModel;
using Nirvana.TestAutomation.Interfaces;

namespace Nirvana.TestAutomation.AccessBridgeApp
{
    public partial class SimulatorAccessBridge : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatorAccessBridge"/> class.
        /// </summary>
        public SimulatorAccessBridge()
        {
            try
            {
                instance = this;
                _accessBridge = new AccessBridge();
                _accessBridge.Initialize();
                InitializeComponent();
            }
            catch (Exception ex)
            {
                LogMessage(ex.ToString());
            }
        }

        /// <summary>
        /// The instance
        /// </summary>
        public static SimulatorAccessBridge instance = null;
        /// <summary>
        /// The initial refresh timer
        /// </summary>
        private System.Windows.Forms.Timer _initialRefreshTimer;
        /// <summary>
        /// The access bridge
        /// </summary>
        public static AccessBridge _accessBridge;
        /// <summary>
        /// The locker object
        /// </summary>
        private static object _lockerObject = new object();
        /// <summary>
        /// The vm identifier
        /// </summary>
        private int vmID = 0;
        /// <summary>
        /// The root pane
        /// </summary>
        private JavaObjectHandle rootPane = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buttonName">Name of the button.</param>
        private delegate void ClickButtonDelegate(string buttonName);

        /// <summary>
        /// Handles the Load event of the SimulatorHelper control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void SimulatorHelper_Load(object sender, System.EventArgs e)
        {
            try
            {

                var host = new ServiceHost(typeof(AccessBridgeService), new Uri("net.pipe://localhost"));
                host.AddServiceEndpoint(typeof(IAccessBridgeService), new NetNamedPipeBinding(), "AccessBridgeApp");
                host.Open();
            }
            catch (Exception ex)
            {
                LogMessage(ex.ToString());
            }

        }

        /// <summary>
        /// Handles the Tick event of the _initialRefreshTimer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void _initialRefreshTimer_Tick(object sender, System.EventArgs e)
        {
            try
            {

                _initialRefreshTimer.Stop();
                IntPtr ptr = NativeMethods.FindWindow("SunAwtFrame", "MS");
                if (ptr.Equals(0))
                    ptr = NativeMethods.FindWindow("SunAwtFrame", "Administrator: MS");
                JavaObjectHandle ac;
                bool y = _accessBridge.Functions.GetAccessibleContextFromHWND(ptr, out vmID, out ac);
                rootPane = _accessBridge.Functions.GetAccessibleChildFromContext(vmID, ac, 0);
            }
            catch (Exception ex)
            {
                LogMessage(ex.ToString());
            }
        }

        /// <summary>
        /// Clicks the button.
        /// </summary>
        /// <param name="buttonName">Name of the button.</param>
        public void ClickButton(string buttonName)
        {
            try
            {

                if (this.InvokeRequired)
                {
                    ClickButtonDelegate invokeDelegate = new ClickButtonDelegate(ClickButton);
                    Invoke(invokeDelegate, buttonName);
                }
                else
                {
                    JavaObjectHandle mainContainer = _accessBridge.Functions.GetAccessibleChildFromContext(vmID, rootPane, 1);
                    JavaObjectHandle nonMenuContainer = _accessBridge.Functions.GetAccessibleChildFromContext(vmID, mainContainer, 0);
                    JavaObjectHandle nonMenuContainerChild = _accessBridge.Functions.GetAccessibleChildFromContext(vmID, nonMenuContainer, 0);
                    JavaObjectHandle bottomPanel = _accessBridge.Functions.GetAccessibleChildFromContext(vmID, nonMenuContainerChild, 2);
                    JavaObjectHandle buttonPanel = _accessBridge.Functions.GetAccessibleChildFromContext(vmID, bottomPanel, 1);
                    int buttonIndex = 0;
                    switch (buttonName)
                    {
                        case CameronConstants.ackButton:
                            buttonIndex = 0;
                            break;
                        case CameronConstants.exeButton:
                            buttonIndex = 1;
                            break;
                        case CameronConstants.rejButton:
                            buttonIndex = 2;
                            break;
                        case CameronConstants.corButton:
                            buttonIndex = 3;
                            break;
                        case CameronConstants.canButton:
                            buttonIndex = 4;
                            break;
                    }
                    JavaObjectHandle button = _accessBridge.Functions.GetAccessibleChildFromContext(vmID, buttonPanel, buttonIndex);
                    AccessibleActionsToDo td = new AccessibleActionsToDo();
                    AccessibleActionInfo inf = new AccessibleActionInfo();
                    inf.name = "click";
                    td.actions = new AccessibleActionInfo[256];
                    td.actions[0] = inf;
                    td.actionsCount = 1;
                    int re = 0;
                    bool ids = _accessBridge.Functions.DoAccessibleActions(vmID, button, ref td, out re);
                }
            }
            catch (Exception ex)
            {
                LogMessage(ex.ToString());
            }
        }

        /// <summary>
        /// Clicks the menu button.
        /// </summary>
        /// <param name="buttonName">Name of the button.</param>
        public void ClickMenuButton(string buttonName)
        {
            try
            {

                if (this.InvokeRequired)
                {
                    ClickButtonDelegate invokeDelegate = new ClickButtonDelegate(ClickMenuButton);
                    Invoke(invokeDelegate, buttonName);
                }
                else
                {
                    JavaObjectHandle mainContainer = _accessBridge.Functions.GetAccessibleChildFromContext(vmID, rootPane, 1);
                    JavaObjectHandle MenuContainer = _accessBridge.Functions.GetAccessibleChildFromContext(vmID, mainContainer, 1);
                    int menuIndex = 0;
                    int buttonIndex = 0;
                    switch (buttonName)
                    {
                        case CameronConstants.openLogButton:
                            buttonIndex = 0;
                            break;
                        case CameronConstants.clearButton:
                            buttonIndex = 1;
                            break;
                        case CameronConstants.scrollButton:
                            buttonIndex = 2;
                            break;
                        case CameronConstants.responseButton:
                            menuIndex = 1;
                            buttonIndex = 0;
                            break;
                        case CameronConstants.doneDayButton:
                            menuIndex = 1;
                            buttonIndex = 1;
                            break;
                    }
                    JavaObjectHandle menubutton = _accessBridge.Functions.GetAccessibleChildFromContext(vmID, MenuContainer, menuIndex);
                    AccessibleActionsToDo td = new AccessibleActionsToDo();
                    AccessibleActionInfo inf = new AccessibleActionInfo();
                    inf.name = "click";
                    td.actions = new AccessibleActionInfo[256];
                    td.actions[0] = inf;
                    td.actionsCount = 1;
                    int re = 0;
                    bool ids = _accessBridge.Functions.DoAccessibleActions(vmID, menubutton, ref td, out re);
                    UIMap.Wait(500);
                    JavaObjectHandle button = _accessBridge.Functions.GetAccessibleChildFromContext(vmID, menubutton, buttonIndex);
                    AccessibleActionsToDo td1 = new AccessibleActionsToDo();
                    AccessibleActionInfo inf1 = new AccessibleActionInfo();
                    inf1.name = "click";
                    td1.actions = new AccessibleActionInfo[256];
                    td1.actions[0] = inf1;
                    td1.actionsCount = 1;
                    re = 0;
                    ids = _accessBridge.Functions.DoAccessibleActions(vmID, button, ref td1, out re);
                    UIMap.Wait(500);
                    JavaObjectHandle nonMenuContainer = _accessBridge.Functions.GetAccessibleChildFromContext(vmID, mainContainer, 0);
                    JavaObjectHandle nonMenuContainerChild = _accessBridge.Functions.GetAccessibleChildFromContext(vmID, nonMenuContainer, 0);
                    AccessibleContextInfo ci1;
                    _accessBridge.Functions.GetAccessibleContextInfo(vmID, nonMenuContainerChild, out ci1);
                    MouseController.MoveTo(ci1.x + (ci1.width / 2), ci1.y + 10);
                    MouseController.Click();
                }
            }
            catch (Exception ex)
            {
                LogMessage(ex.ToString());
            }
        }

        /// <summary>
        /// Clicks the bottom grid field.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        public void ClickBottomGridField(string fieldName)
        {
            try
            {

                if (this.InvokeRequired)
                {
                    ClickButtonDelegate invokeDelegate = new ClickButtonDelegate(ClickBottomGridField);
                    Invoke(invokeDelegate, fieldName);
                }
                else
                {
                    JavaObjectHandle mainContainer = _accessBridge.Functions.GetAccessibleChildFromContext(vmID, rootPane, 1);
                    JavaObjectHandle nonMenuContainer = _accessBridge.Functions.GetAccessibleChildFromContext(vmID, mainContainer, 0);
                    JavaObjectHandle nonMenuContainerChild = _accessBridge.Functions.GetAccessibleChildFromContext(vmID, nonMenuContainer, 0);
                    JavaObjectHandle bottomPanel = _accessBridge.Functions.GetAccessibleChildFromContext(vmID, nonMenuContainerChild, 2);
                    JavaObjectHandle fieldTablePanel = _accessBridge.Functions.GetAccessibleChildFromContext(vmID, bottomPanel, 0);
                    JavaObjectHandle fieldTextBoxesPanel = _accessBridge.Functions.GetAccessibleChildFromContext(vmID, fieldTablePanel, 0);
                    JavaObjectHandle fieldTextBoxesTable = _accessBridge.Functions.GetAccessibleChildFromContext(vmID, fieldTextBoxesPanel, 0);
                    AccessibleContextInfo aci;
                    _accessBridge.Functions.GetAccessibleContextInfo(vmID, fieldTextBoxesTable, out aci);
                    MouseController.MoveTo(aci.x + 10, aci.y + 10);
                    MouseController.Click();
                    UIMap.Wait(1000);
                    int fieldIndex = 0;
                    switch (fieldName)
                    {
                        case CameronConstants.orderIDField:
                            fieldIndex = 0;
                            break;
                        case CameronConstants.sharesField:
                            fieldIndex = 1;
                            break;
                        case CameronConstants.priceField:
                            fieldIndex = 2;
                            break;
                    }
                    JavaObjectHandle cell = _accessBridge.Functions.GetAccessibleSelectionFromContext(vmID, fieldTextBoxesTable, fieldIndex);
                    _accessBridge.Functions.GetAccessibleContextInfo(vmID, cell, out aci);
                    MouseController.MoveTo(aci.x + (aci.width / 2), aci.y + (aci.height / 2));
                    MouseController.DoubleClick();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Clicks the grid row.
        /// </summary>
        /// <param name="orderID">The order identifier.</param>
        public void ClickGridRow(string rowIndex)
        {
            try
            {


                if (this.InvokeRequired)
                {
                    ClickButtonDelegate invokeDelegate = new ClickButtonDelegate(ClickGridRow);
                    Invoke(invokeDelegate, rowIndex);
                }
                else
                {
                    JavaObjectHandle mainContainer = _accessBridge.Functions.GetAccessibleChildFromContext(vmID, rootPane, 1);
                    JavaObjectHandle nonMenuContainer = _accessBridge.Functions.GetAccessibleChildFromContext(vmID, mainContainer, 0);
                    JavaObjectHandle nonMenuContainerChild = _accessBridge.Functions.GetAccessibleChildFromContext(vmID, nonMenuContainer, 0);
                    JavaObjectHandle middlePanel = _accessBridge.Functions.GetAccessibleChildFromContext(vmID, nonMenuContainerChild, 1);
                    JavaObjectHandle gridPanel = _accessBridge.Functions.GetAccessibleChildFromContext(vmID, middlePanel, 0);
                    JavaObjectHandle grid = _accessBridge.Functions.GetAccessibleChildFromContext(vmID, gridPanel, 0);
                    AccessibleContextInfo ci1;
                    _accessBridge.Functions.GetAccessibleContextInfo(vmID, grid, out ci1);
                    MouseController.MoveTo(ci1.x + 150, ci1.y + (16*Convert.ToInt32(rowIndex)) + 5);
                    MouseController.Click();
                    //KeyboardUtilities.PressKey(1, KeyboardConstants.CTRLA);
                    //UIMap.Wait(1000);
                    //int count = _accessBridge.Functions.GetAccessibleSelectionCountFromContext(vmID, grid);
                    //for (int i = 0; i < count; i++)
                    //{

                    //    JavaObjectHandle cell = _accessBridge.Functions.GetAccessibleSelectionFromContext(vmID, grid, i);
                    //    StringBuilder sb1 = new StringBuilder(50);
                    //    _accessBridge.Functions.GetVirtualAccessibleName(vmID, cell, sb1, sb1.Capacity);
                    //    if (sb1.ToString().Equals(orderID))
                    //    {
                    //        AccessibleContextInfo ci;
                    //        _accessBridge.Functions.GetAccessibleContextInfo(vmID, cell, out ci);
                    //        MouseController.MoveTo(ci.x + (ci.width / 2), ci.y + (ci.height / 2));
                    //        MouseController.Click();
                    //        break;
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                LogMessage(ex.ToString());
            }
        }


        /// <summary>
        /// Logs the message.
        /// </summary>
        /// <param name="message">The message.</param>
        private void LogMessage(string message)
        {
            lock (_lockerObject)
            {
                using (StreamWriter w = File.AppendText(Application.StartupPath + @"\LogFiles\SimulatorBridgeLog.txt"))
                {
                    w.Write(Environment.NewLine + "Log Entry : " + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + "  :");
                    w.WriteLine(message);
                    w.WriteLine("--------------------------------------------------");
                }
            }
        }
    }
}
