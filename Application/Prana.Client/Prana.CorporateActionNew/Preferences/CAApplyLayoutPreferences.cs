using Prana.ClientCommon;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Prana.CorporateActionNew.Forms
{
    [XmlRoot("CAApplyLayoutPreferences")]
    [Serializable]
    public class CAApplyLayoutPreferences : IDisposable
    {
        [XmlElement("CAApplyValue", typeof(string))]
        public string CAApplyValue;

        [XmlArray("CAApplyColumns"), XmlArrayItem("ColumnData", typeof(ColumnData))]
        public List<ColumnData> CAApplyColumns = new List<ColumnData>();

        #region IDisposable Members
        /// <summary>
        /// Dispose() calls Dispose(true)
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposing Objects
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    CAApplyValue = null;
                    CAApplyColumns = null;
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
        #endregion
    }

    [XmlRoot("CARedoLayoutPreferences")]
    [Serializable]
    public class CARedoLayoutPreferences : IDisposable
    {
        [XmlElement("CARedoValue", typeof(string))]
        public string CARedoValue;

        [XmlArray("CARedoColumns"), XmlArrayItem("ColumnData", typeof(ColumnData))]
        public List<ColumnData> CARedoColumns = new List<ColumnData>();

        #region IDisposable Members
        /// <summary>
        /// Dispose() calls Dispose(true)
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposing Objects
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    CARedoValue = null;
                    CARedoColumns = null;
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
        #endregion
    }

    [XmlRoot("CAUndoLayoutPreferences")]
    [Serializable]
    public class CAUndoLayoutPreferences : IDisposable
    {
        [XmlElement("CAUndoValue", typeof(string))]
        public string CAUndoValue;

        [XmlArray("CAUndoColumns"), XmlArrayItem("ColumnData", typeof(ColumnData))]
        public List<ColumnData> CAUndoColumns = new List<ColumnData>();

        #region IDisposable Members
        /// <summary>
        /// Dispose() calls Dispose(true)
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposing Objects
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    CAUndoValue = null;
                    CAUndoColumns = null;
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
        #endregion
    }
}