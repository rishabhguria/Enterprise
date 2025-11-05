using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Prana.LogManager
{
    public class LogAndDisplayOnInformationReporter
    {
        /// <summary>
        /// The log and display on information reporter
        /// </summary>
        private static LogAndDisplayOnInformationReporter _logAndDisplayOnInformationReporter = null;
        /// <summary>
        /// The locker
        /// </summary>
        private static readonly object _locker = new object();

        /// <summary>
        /// Gets the get instance.
        /// </summary>
        /// <value>
        /// The get instance.
        /// </value>
        public static LogAndDisplayOnInformationReporter GetInstance
        {
            get
            {
                lock (_locker)
                {
                    if (_logAndDisplayOnInformationReporter == null)
                    {
                        _logAndDisplayOnInformationReporter = new LogAndDisplayOnInformationReporter();
                    }
                }
                return _logAndDisplayOnInformationReporter;
            }
        }

        /// <summary>
        /// Writes the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="category">The category.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="severity">The severity.</param>
        public void Write(string message, string category, int priority, int eventId, TraceEventType severity, bool isOverrideLoggingPrefs = false)
        {
            try
            {
                Logger.LoggerWrite(message, category, priority, eventId, severity, isOverrideLoggingPrefs);
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

        /// <summary>
        /// Writes the and display on information reporter.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="category">The category.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="severity">The severity.</param>
        public void WriteAndDisplayOnInformationReporter(object message, bool isOverrideLoggingPrefs = false)
        {
            try
            {
                if (message.GetType() == typeof(string))
                    InformationReporter.GetInstance.Write(message.ToString());
                else if (message.GetType() == typeof(Exception))
                    InformationReporter.GetInstance.Write(((Exception)message).Message);

                Logger.LoggerWrite(message, isOverrideLoggingPrefs);
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

        /// <summary>
        /// Writes the and display on information reporter.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="category">The category.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="severity">The severity.</param>
        public void WriteAndDisplayOnInformationReporter(object message, string category, int priority, int eventId, TraceEventType severity, bool isOverrideLoggingPrefs = false)
        {
            try
            {
                if (message.GetType() == typeof(string))
                    InformationReporter.GetInstance.Write(message.ToString());
                else if (message.GetType() == typeof(Exception))
                    InformationReporter.GetInstance.Write(((Exception)message).Message);

                Logger.LoggerWrite(message, category, priority, eventId, severity, isOverrideLoggingPrefs);
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

        /// <summary>
        /// Writes the and display on information reporter.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="category">The category.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="severity">The severity.</param>
        public void WriteAndDisplayOnInformationReporter(object message, string category, int priority, int eventId, TraceEventType severity, string title, bool isOverrideLoggingPrefs = false)
        {
            try
            {
                if (message.GetType() == typeof(string))
                    InformationReporter.GetInstance.Write(message.ToString());
                else if (message.GetType() == typeof(Exception))
                    InformationReporter.GetInstance.Write(((Exception)message).Message);

                Logger.LoggerWrite(message, category, priority, eventId, severity, title, isOverrideLoggingPrefs);
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

        /// <summary>
        /// Writes the and display on information reporter.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="category">The category.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="severity">The severity.</param>
        public void WriteAndDisplayOnInformationReporter(object message, string category, int priority, int eventId, TraceEventType severity, string title, IDictionary<string, object> properties, bool isOverrideLoggingPrefs = false)
        {
            try
            {
                if (message.GetType() == typeof(string))
                    InformationReporter.GetInstance.Write(message.ToString());
                else if (message.GetType() == typeof(Exception))
                    InformationReporter.GetInstance.Write(((Exception)message).Message);

                Logger.LoggerWrite(message, category, priority, eventId, severity, title, properties, isOverrideLoggingPrefs);
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
    }
}
