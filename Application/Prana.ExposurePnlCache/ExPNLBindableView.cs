using Prana.LogManager;
using System;

namespace Prana.ExposurePnlCache
{
    [Serializable()]
    public class ExPnlBindableView : IDisposable //: BusinessBase<ExPnlBindableView>
    {
        private string _rowColorBasis = "0";  //OrderSide
        public string RowColorBasis
        {
            get
            {
                return _rowColorBasis;
            }
            set
            {
                _rowColorBasis = value;
                if (_gridData != null)
                {
                    _gridData.RowColorBasis = _rowColorBasis;
                }
            }
        }

        private ExposurePnlCacheBindableDictionary _gridData;
        public ExposurePnlCacheBindableDictionary GridData
        {
            get
            {
                if (_gridData == null)
                {
                    _gridData = new ExposurePnlCacheBindableDictionary();
                }
                return _gridData;
            }
            set
            {
                try
                {
                    _gridData = value;
                    if (_gridData != null)
                        _gridData.RowColorBasis = RowColorBasis;
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

        /// <summary>
        /// TODO : Can't afford to have this ID property in multiuer environment
        /// Need to have some id which uniquely identify this.
        /// </summary>
        /// <returns></returns>
        protected object GetIdValue()
        {
            return Guid.NewGuid();
        }

        #region IDisposable Members
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            try
            {
                if (isDisposing)
                {
                    if (_gridData != null)
                    {
                        _gridData.Dispose();
                        _gridData = null;
                    }
                    RowColorBasis = null;
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
