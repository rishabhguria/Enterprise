using Prana.LogManager;
using System;
using System.Windows.Forms;

namespace Prana.Utilities.UI.UIUtilities
{
    public class FormMarshaller
    {
        // This is a reference to the application main form, used to get the UI thread
        // It is write only, and should be set in the onLoad event of the UI's main form
        private Form _form = null;
        public Form Form
        {
            get { return _form; }
            set { _form = value; }
        }

        // Behaves as per usual, but if no UI is registered, assumes ModelLayer is running
        // in a service or on a server without a UI
        public bool InvokeRequired
        {
            get
            {
                // No UI registered
                if (_form == null)
                    return false;

                // Check with the main form
                return _form.InvokeRequired;
            }
        }

        // Behaves as per usual, but if no UI is registed, assumes the user didn't check InvokeRequired first
        // and throws an exception.
        public void Invoke(Delegate delegateMethod, params object[] args)
        {
            //object returnObject = null;
            try
            {
                // No UI registered
                if (_form == null)
                {
                    Exception ex = new Exception("No UI registered with UIThreadMarshal. Invoke is meaningless.");
                    throw ex;
                }
                if (_form.Disposing)
                {
                    Exception ex = new Exception("No UI registered with UIThreadMarshal. Invoke is meaningless.");
                    throw ex;
                }

                _form.Invoke(delegateMethod, args);
            }
            catch (Exception ex)
            {
                bool result = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (result)
                    throw;
            }
        }

    }
}
