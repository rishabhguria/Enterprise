using Prana.LogManager;
using System;
using System.Data;
using System.IO;

namespace Act40OrderGeneratorTool.Cache
{
    class ReplacementMatrix : IDisposable
    {
        #region SingiltonInstance

        /// <summary>
        /// Locker object
        /// </summary>
        private static Object _lock = new Object();

        /// <summary>
        /// The singilton instance
        /// </summary>
        private static ReplacementMatrix _singiltonObject = null;

        /// <summary>
        /// private cunstructor
        /// </summary>
        private ReplacementMatrix()
        {
            // Build the cache
            _cache = new DataTable("Replacement Matrix");
            if (File.Exists(REPLACEMENT_MATRIX_FILE))
            {
                _cache.ReadXml(REPLACEMENT_MATRIX_FILE);
            }
            else
            {
                _cache.Columns.Add("Group");
                _cache.Columns.Add("Side");
                _cache.Columns.Add("Target");
                _cache.Columns.Add("Replacement Symbol");
                _cache.Columns.Add("Price", typeof(Double));
            }
        }

        /// <summary>
        /// Singilton instance
        /// </summary>
        /// <returns></returns>
        internal static ReplacementMatrix GetInstance()
        {
            lock (_lock)
            {
                if (_singiltonObject == null)
                    _singiltonObject = new ReplacementMatrix();
                return _singiltonObject;
            }
        }
        #endregion

        private DataTable _cache;
        private const String REPLACEMENT_MATRIX_FILE = "ReplacementMatrix.xml";

        internal DataTable Get()
        {
            return _cache.Copy();
        }

        /// <summary>
        /// Save to cache and file
        /// </summary>
        /// <param name="table"></param>
        internal void Set(DataTable table)
        {
            try
            {
                _cache = table.Copy();
                _cache.WriteXml(REPLACEMENT_MATRIX_FILE, XmlWriteMode.WriteSchema);
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (_cache != null)
                        _cache.Dispose();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
