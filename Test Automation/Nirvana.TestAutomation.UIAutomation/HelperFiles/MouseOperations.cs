using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using TestAutomationFX.UI;
using System.Runtime.InteropServices;
using UIAutomationClient;
using WindowsInput;
using System.Diagnostics; 

namespace Nirvana.TestAutomation.UIAutomation
{
    public class MouseOperations
    {
        public static void ClickElement(IUIAutomationElement element, string clicktype = "Left", bool usecursorclick = false)
        {
            try
            {
                if (element == null)
                {
                    return;
                }
                if (!usecursorclick)
                {
                    try
                    {
                        object invokePatternObj;
                        if (element.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) != null && !usecursorclick)
                        {
                            invokePatternObj = element.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);
                            IUIAutomationInvokePattern invokePattern = invokePatternObj as IUIAutomationInvokePattern;

                            if (invokePattern != null)
                            {
                                invokePattern.Invoke();
                                Console.WriteLine("Button clicked.");
                            }
                            else
                            {
                                Console.WriteLine("Button does not support InvokePattern.");
                            }
                            return;
                        }


                        object scrollItemPatternObj;
                        if (element.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId) != null && !usecursorclick)
                        {
                            scrollItemPatternObj = element.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId);
                            IUIAutomationScrollItemPattern scrollItemPattern = scrollItemPatternObj as IUIAutomationScrollItemPattern;
                            //  scrollItemPattern?.ScrollIntoView();

                            if (scrollItemPattern != null)
                            {
                                scrollItemPattern.ScrollIntoView();
                                Console.WriteLine("Button clicked.");
                            }
                            else
                            {
                                Console.WriteLine("Button does not support InvokePattern.");
                            }
                        }
                    }
                    catch { }
                }
                    var boundingRect = element.CurrentBoundingRectangle;
                    int left = boundingRect.left;
                    int top = boundingRect.top;
                    int right = boundingRect.right;
                    int bottom = boundingRect.bottom;

                    if (left != int.MaxValue && top != int.MaxValue && right != int.MaxValue && bottom != int.MaxValue)
                    {
                        int x = left + (right - left) / 2;
                        int y = top + (bottom - top) / 2;

                        System.Windows.Forms.Cursor.Position = new System.Drawing.Point(x, y);

                        // Simulate the mouse click using InputSimulator
                        InputSimulator simulator = new InputSimulator();
                        if (clicktype == "Left")
                        {
                            simulator.Mouse.LeftButtonClick();
                        }
                        else if (clicktype == "Right")
                        {
                            simulator.Mouse.RightButtonClick();
                        }
                    }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }
        public static void ClickElementLeftPartial(IUIAutomationElement element, string clicktype = "Left", bool usecursorclick = false)
        {
            try
            {
                if (element == null)
                {
                    return;
                }

                object invokePatternObj;
                if (element.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) != null && !usecursorclick)
                {
                    invokePatternObj = element.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);
                    IUIAutomationInvokePattern invokePattern = invokePatternObj as IUIAutomationInvokePattern;

                    if (invokePattern != null)
                    {
                        invokePattern.Invoke();
                        Console.WriteLine("Button clicked.");
                    }
                    else
                    {
                        Console.WriteLine("Button does not support InvokePattern.");
                    }
                    return;
                }

                object scrollItemPatternObj;
                if (element.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId) != null && !usecursorclick)
                {
                    scrollItemPatternObj = element.GetCurrentPattern(UIA_PatternIds.UIA_ScrollItemPatternId);
                    IUIAutomationScrollItemPattern scrollItemPattern = scrollItemPatternObj as IUIAutomationScrollItemPattern;

                    if (scrollItemPattern != null)
                    {
                        scrollItemPattern.ScrollIntoView();
                        Console.WriteLine("Button clicked.");
                    }
                    else
                    {
                        Console.WriteLine("Button does not support InvokePattern.");
                    }
                }

                var boundingRect = element.CurrentBoundingRectangle;
                int left = boundingRect.left;
                int top = boundingRect.top;
                int right = boundingRect.right;
                int bottom = boundingRect.bottom;

                if (left != int.MaxValue && top != int.MaxValue && right != int.MaxValue && bottom != int.MaxValue)
                {
                    int x = left + (right - left) / 4;  // Click 1/4 of the width from the left
                    int y = top + (bottom - top) / 2;   // Click in the vertical middle

                    System.Windows.Forms.Cursor.Position = new System.Drawing.Point(x, y);

                    InputSimulator simulator = new InputSimulator();
                    if (clicktype == "Left")
                    {
                        simulator.Mouse.LeftButtonClick();
                    }
                    else if (clicktype == "Right")
                    {
                        simulator.Mouse.RightButtonClick();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

    }

}
