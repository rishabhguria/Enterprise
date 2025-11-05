//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Runtime.InteropServices;
//using UIAutomationClient;

//namespace WorkflowAutomation
//{
//    public class WorkflowAutomation_COMLibrary
//    {
//        public static void FetchElements()
//        {
//            try
//            {
//                // Initialize COM
//                CoInitializeEx(IntPtr.Zero, COINIT.COINIT_MULTITHREADED);

//                // Create the UI Automation object
//                IUIAutomation automation = new CUIAutomation8();

//                // Get the root element of the desktop
//                IUIAutomationElement rootElement = automation.GetRootElement();

//                // Find the first button with the name "MyButton"
//                string elementName = "Create Order";
//                IUIAutomationCondition condition = automation.CreatePropertyCondition(UIA_PropertyIds.UIA_ControlTypePropertyId, UIA_ControlTypeIds.UIA_ComboBoxControlTypeId);

//                IUIAutomationElementArray comboboxes = rootElement.FindAll(
//                    TreeScope.TreeScope_Descendants, condition);
//                if (comboboxes != null)
//                {
//                    for (int i = 0; i < comboboxes.Length; i++)
//                    {
//                        IUIAutomationElement combobox = comboboxes.GetElement(i);
//                        string comboboxText = combobox.GetCurrentPropertyValue(
//                            UIA_PropertyIds.UIA_NamePropertyId).ToString();
//                        string comboboxText1 = combobox.GetCurrentPropertyValue(
//                            UIA_PropertyIds.UIA_AutomationIdPropertyId).ToString();
//                        Console.WriteLine($"ComboBox {i + 1} text: {comboboxText}");

//                        Marshal.ReleaseComObject(combobox);
//                    }
//                    Console.WriteLine(comboboxes.Length);
//                }
//                // Do something with the buttonElement

//                // Release COM objects
//                if (comboboxes != null)
//                    Marshal.ReleaseComObject(comboboxes);
//                Marshal.ReleaseComObject(condition);
//                Marshal.ReleaseComObject(rootElement);
//                Marshal.ReleaseComObject(automation);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("An error occurred: " + ex.Message);
//            }
//            finally
//            {
//                // Uninitialize COM
//                CoUninitialize();
//            }
//        }

//        // COM Initialization flags
//        [DllImport("ole32.dll")]
//        private static extern int CoInitializeEx(IntPtr pvReserved, COINIT dwCoInit);

//        // COM Uninitialization
//        [DllImport("ole32.dll")]
//        private static extern void CoUninitialize();

//        // COM Initialization constants
//        [Flags]
//        private enum COINIT : uint
//        {
//            COINIT_APARTMENTTHREADED = 0x2,
//            COINIT_MULTITHREADED = 0x0,
//            COINIT_DISABLE_OLE1DDE = 0x4,
//            COINIT_SPEED_OVER_MEMORY = 0x8
//        }
//    }
//}
