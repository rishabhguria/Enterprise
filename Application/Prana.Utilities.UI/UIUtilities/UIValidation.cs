using Infragistics.Win.Misc;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinTabControl;
using Prana.LogManager;
using System;
using System.Windows.Forms;

namespace Prana.Utilities.UI.UIUtilities
{
    public class UIValidation
    {
        private static UIValidation _singleton = null;
        private static object _locker = new object();
        public bool validate(Form form)
        {
            lock (_locker)
            {
                if (form != null && form.IsHandleCreated && !form.IsDisposed && !form.Disposing)
                {
                    return true;
                }
                else
                {
                    string message = string.Empty;
                    if (form == null)
                        message = "UIValidation - Form object is NULL";
                    else if (!form.IsHandleCreated)
                        message = "UIValidation - Form handle not created";
                    else if (!form.IsDisposed)
                        message = "UIValidation - Form in disposed mode";
                    else if (!form.Disposing)
                        message = "UIValidation - Form in disposing mode";

                    Logger.LoggerWrite(message + Environment.StackTrace, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, "UIValidation");
                    return false;
                }
            }
        }
        public bool validate(Control control)
        {
            lock (_locker)
            {
                if (control != null && control.IsHandleCreated && !control.IsDisposed && !control.Disposing)
                {
                    return true;
                }
                else
                {
                    if (control == null)
                        Logger.LoggerWrite("UIValidation - Control object is NULL");
                    else if (!control.IsHandleCreated)
                        Logger.LoggerWrite("UIValidation - Control handle not created");
                    else if (!control.IsDisposed)
                        Logger.LoggerWrite("UIValidation - Control in disposed mode");
                    else if (!control.Disposing)
                        Logger.LoggerWrite("UIValidation - Control in disposing mode");

                    Logger.LoggerWrite(Environment.StackTrace, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, "UIValidation");
                    return false;
                }
            }
        }
        public bool validate(UltraLabel ultraLabel)
        {
            lock (_locker)
            {
                if (ultraLabel != null && ultraLabel.IsHandleCreated && !ultraLabel.IsDisposed && !ultraLabel.Disposing)
                {
                    return true;
                }
                else
                {
                    if (ultraLabel == null)
                        Logger.LoggerWrite("UIValidation - UltraLabel object is NULL");
                    else if (!ultraLabel.IsHandleCreated)
                        Logger.LoggerWrite("UIValidation - UltraLabel handle not created");
                    else if (!ultraLabel.IsDisposed)
                        Logger.LoggerWrite("UIValidation - UltraLabel in disposed mode");
                    else if (!ultraLabel.Disposing)
                        Logger.LoggerWrite("UIValidation - UltraLabel in disposing mode");

                    Logger.LoggerWrite(Environment.StackTrace, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, "UIValidation");
                    return false;
                }
            }
        }
        public bool validate(UltraGrid ultraGrid)
        {
            lock (_locker)
            {
                if (ultraGrid != null && ultraGrid.IsHandleCreated && !ultraGrid.IsDisposed && !ultraGrid.Disposing)
                {
                    return true;
                }
                else
                {
                    if (ultraGrid == null)
                        Logger.LoggerWrite("UIValidation - UltraGrid object is NULL");
                    else if (!ultraGrid.IsHandleCreated)
                        Logger.LoggerWrite("UIValidation - UltraGrid handle not created");
                    else if (!ultraGrid.IsDisposed)
                        Logger.LoggerWrite("UIValidation - UltraGrid in disposed mode");
                    else if (!ultraGrid.Disposing)
                        Logger.LoggerWrite("UIValidation - UltraGrid in disposing mode");

                    Logger.LoggerWrite(Environment.StackTrace, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, "UIValidation");
                    return false;
                }
            }
        }
        public bool validate(Label label)
        {
            lock (_locker)
            {
                if (label != null && label.IsHandleCreated && !label.IsDisposed && !label.Disposing)
                {
                    return true;
                }
                else
                {
                    if (label == null)
                        Logger.LoggerWrite("UIValidation - Label object is NULL", "");
                    else if (!label.IsHandleCreated)
                        Logger.LoggerWrite("UIValidation - Label handle not created");
                    else if (!label.IsDisposed)
                        Logger.LoggerWrite("UIValidation - Label in disposed mode");
                    else if (!label.Disposing)
                        Logger.LoggerWrite("UIValidation - Label in disposing mode");

                    Logger.LoggerWrite(Environment.StackTrace, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, "UIValidation");
                    return false;
                }
            }
        }
        public bool validate(UltraTabControl ultraTabControl)
        {
            lock (_locker)
            {
                if (ultraTabControl != null && ultraTabControl.IsHandleCreated && !ultraTabControl.IsDisposed && !ultraTabControl.Disposing)
                {
                    return true;
                }
                else
                {
                    if (ultraTabControl == null)
                        Logger.LoggerWrite("UIValidation - UltraTabControl object is NULL");
                    else if (!ultraTabControl.IsHandleCreated)
                        Logger.LoggerWrite("UIValidation - UltraTabControl handle not created");
                    else if (!ultraTabControl.IsDisposed)
                        Logger.LoggerWrite("UIValidation - UltraTabControl in disposed mode");
                    else if (!ultraTabControl.Disposing)
                        Logger.LoggerWrite("UIValidation - UltraTabControl in disposing mode");

                    Logger.LoggerWrite(Environment.StackTrace, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, "UIValidation");
                    return false;
                }
            }
        }
        public bool validate(ListBox listBox)
        {
            lock (_locker)
            {
                if (listBox != null && listBox.IsHandleCreated && !listBox.IsDisposed && !listBox.Disposing)
                {
                    return true;
                }
                else
                {
                    if (listBox == null)
                        Logger.LoggerWrite("UIValidation - ListBox object is NULL");
                    else if (!listBox.IsHandleCreated)
                        Logger.LoggerWrite("UIValidation - ListBox handle not created");
                    else if (!listBox.IsDisposed)
                        Logger.LoggerWrite("UIValidation - ListBox in disposed mode");
                    else if (!listBox.Disposing)
                        Logger.LoggerWrite("UIValidation - ListBox in disposing mode");

                    Logger.LoggerWrite(Environment.StackTrace, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, "UIValidation");
                    return false;
                }
            }
        }
        public bool validate(Button button)
        {
            lock (_locker)
            {
                if (button != null && button.IsHandleCreated && !button.IsDisposed && !button.Disposing)
                {
                    return true;
                }
                else
                {
                    if (button == null)
                        Logger.LoggerWrite("UIValidation - Button object is NULL");
                    else if (!button.IsHandleCreated)
                        Logger.LoggerWrite("UIValidation - Button handle not created");
                    else if (!button.IsDisposed)
                        Logger.LoggerWrite("UIValidation - Button in disposed mode");
                    else if (!button.Disposing)
                        Logger.LoggerWrite("UIValidation - Button in disposing mode");

                    Logger.LoggerWrite(Environment.StackTrace, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, "UIValidation");
                    return false;
                }
            }
        }
        public static UIValidation GetInstance()
        {
            if (_singleton == null)
            {
                lock (_locker)
                {
                    if (_singleton == null)
                    {
                        _singleton = new UIValidation();
                    }
                }
            }
            return _singleton;
        }
    }
}
