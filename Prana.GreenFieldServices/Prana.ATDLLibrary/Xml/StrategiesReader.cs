using Prana.ATDLLibrary.Diagnostics;
using Prana.ATDLLibrary.Diagnostics.Exceptions;
using Prana.ATDLLibrary.Model.Elements;
using Prana.ATDLLibrary.Resources;
using Prana.ATDLLibrary.Xml.Serialization;
using System.Xml.Linq;
using Prana.LogManager;
using System.Diagnostics;
using System;

namespace Prana.ATDLLibrary.Xml
{
    public class StrategiesReader
    {
        public Strategies_t Load(string path)
        {
            try
            {
                Logger.LoggerWrite(string.Format("Attempting to load strategies from file '{0}'.", path), LoggingConstants.ATDL_LOGGING, 1, 1, TraceEventType.Verbose);

                XDocument document = XDocument.Load(path, LoadOptions.SetLineInfo | LoadOptions.PreserveWhitespace);

                Strategies_t strategies = LoadStrategies(document);

                Logger.LoggerWrite(string.Format("{0} strategies loaded from file '{1}'.", strategies.Count, path), LoggingConstants.ATDL_LOGGING, 1, 1, TraceEventType.Verbose);

                return strategies;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return null;
        }

        private Strategies_t LoadStrategies(XDocument document)
        {
            try
            {
                XElement element = document.Element(AtdlNamespaces.core + "Strategies");

                if (element == null)
                    throw ThrowHelper.New<Atdl4netException>(this, ErrorMessages.StrategiesLoadFailure);

                ElementFactory factory = new ElementFactory(SchemaDefinitions.Strategies_t);

                Strategies_t strategies = (Strategies_t)factory.DeserializeElement(element);

                return strategies;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return null;
        }
    }
}
