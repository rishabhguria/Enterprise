using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;

namespace Nirvana.TestAutomation.Utilities
{
    public static class KeyboardUtilities
    {
        /// <summary>
        /// Presses the key.
        /// </summary>
        /// <param name="noOfTimes">The no of times.</param>
        /// <param name="key">The key.</param>
        public static void PressKey(int noOfTimes, string key)
        {
            try
            {
                for (int pressCounter = 0; pressCounter < noOfTimes; pressCounter++)
                {
                    UIMap.Wait(1000);
                    Keyboard.SendKeys(key);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Closes the UI.
        /// </summary>
        public static void CloseUI()
        {
            try
            {
                Keyboard.SendKeys(KeyboardConstants.ALTF4);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Minimizes the UI
        /// </summary>
        public static void MinimizeWPFWindow()
        {
            try
            {
                Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_N);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Minimizes the UI
        /// </summary>
        public static void MinimizeWindow(ref UIMsaa moduleWindow)
        {
            try
            {
                moduleWindow.Click(MouseButtons.Right);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Minimizes the UI
        /// </summary>
        public static void MinimizeWindow(ref UIWindow moduleWindow)
        {
            try
            {
                moduleWindow.Click(MouseButtons.Right);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Close the UI using right click option on UI Window
        /// </summary>
        public static void CloseWindow(ref UIWindow moduleWindow)
        {
            try
            {
                moduleWindow.BringToFront();
                moduleWindow.Click(MouseButtons.Right);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Minimizes the window.
        /// </summary>
        /// <param name="moduleWindow">The module window.</param>
        public static void MinimizeWindow(ref UIAutomationElement moduleWindow)
        {
            try
            {
                moduleWindow.Click(MouseButtons.Right);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Close the window using right click option on UI Automation Element.
        /// </summary>
        /// <param name="moduleWindow">The module window.</param>
        public static void CloseWindow(ref UIAutomationElement moduleWindow)
        {
            try
            {
                moduleWindow.Click(MouseButtons.Right);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Presses down key with wait.
        /// </summary>
        /// <param name="index">The index.</param>
        public static void PressDownKeyWithWait(int index)
        {
            try
            {
                for (int i = 0; i < index; i++)
                {
                    UIMap.Wait(500);
                    Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Press up key with wait
        /// </summary>
        /// <param name="index">number of times</param>
        public static void PressUpKeyWithWait(int index)
        {
            try
            {
                for (int i = 0; i < index; i++)
                {
                    UIMap.Wait(500);
                    Keyboard.SendKeys(KeyboardConstants.UP_ARROWKEY);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void MouseScrollDown(int index)
        {
            try
            {
                for (int i = 0; i < index; i++)
                {
                    UIMap.Wait(500);
                    MouseController.ScrollWheelDown();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Maximizes the UI
        /// </summary>
        public static void MaximizeWindow(ref UIWindow moduleWindow)
        {
            try
            {
                moduleWindow.Click(MouseButtons.Right);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
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

        // 
        public static void MaximizeWindow(ref UIAutomationElement moduleWindow)
        {
            try
            {
                moduleWindow.Click(MouseButtons.Right);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
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


		public static void OffCapsLock()
        {
			try{
				int KEYEVENTF_EXTENDEDKEY = 0x1;
				int KEYEVENTF_KEYUP = 0x2;
				int CAPSLOCK = 0x14;
				if (Console.CapsLock)
				{
					NativeMethods.keybd_event((byte)CAPSLOCK, (byte)0x45, (byte)KEYEVENTF_EXTENDEDKEY, (UIntPtr)0);
                    NativeMethods.keybd_event((byte)CAPSLOCK, (byte)0x45, (byte)(KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP), (UIntPtr)0);
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
        /// Close the window using right click option on UI MSAA .
        /// </summary>
        /// <param name="moduleWindow">The module window.</param>
        public static void CloseWindow(ref UIMsaa moduleWindow)
        {
            try
            {
                moduleWindow.Click(MouseButtons.Right);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
