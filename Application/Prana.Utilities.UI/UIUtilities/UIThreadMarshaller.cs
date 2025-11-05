using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Prana.Utilities.UI.UIUtilities
{
    // Singleton class used to marshall events onto the UI thread inside the Exposure pnl cache.
    //By using this class we won't pass the ui objects into the model/cache layer
    public static class UIThreadMarshaller
    {

        /// <summary>
        /// TODO : Nicer to use the same module name as used in layout.
        /// </summary>
        public const string PM_FORM = "PM_FORM";

        static Dictionary<string, FormMarshaller> _formMarshallerDict = new Dictionary<string, FormMarshaller>();

        public static void AddFormForMarshalling(string key, Form formInstance)
        {
            try
            {
                if (!_formMarshallerDict.ContainsKey(key))
                {
                    FormMarshaller formMarshaller = new FormMarshaller();
                    formMarshaller.Form = formInstance;
                    _formMarshallerDict.Add(key, formMarshaller);
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }


        public static void RemoveFormForMarshalling(string key)
        {
            try
            {
                if (_formMarshallerDict.ContainsKey(key))
                {
                    _formMarshallerDict.Remove(key);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public static FormMarshaller GetFormMarshallerByKey(string key)
        {
            try
            {
                if (_formMarshallerDict.ContainsKey(key))
                {
                    return _formMarshallerDict[key];
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }
    }

}
