using System;
using System.ComponentModel;

namespace Prana.BusinessObjects
{
    public class WPFErrorObject : BindableBase, IDataErrorInfo
    {
        [NonSerialized]
        private readonly DataErrorInfoSupport dataErrorInfoSupport;

        public WPFErrorObject()
        {
            dataErrorInfoSupport = new DataErrorInfoSupport(this);
        }

        private int _errorCount = 0;
        public int ErrorCount
        {
            get { return _errorCount; }
            set { SetProperty(ref _errorCount, value); }
        }

        private object _errorDescription;
        public object ErrorDescription
        {
            get { return _errorDescription; }
            set { SetProperty(ref _errorDescription, value); }
        }

        private static string _errorMsg;
        [ValidateErrorMessage("ErrorDescription")]
        public string ErrorMsg
        {
            get { return _errorMsg; }
            set
            {
                _errorMsg = value;
                RaisePropertyChangedEvent("ErrorMsg");
            }
        }

        #region IDataErrorInfo
        public string Error
        {
            get { return dataErrorInfoSupport.Error; }
        }

        public string this[string columnName]
        {
            get { return dataErrorInfoSupport[columnName]; }
        }
        #endregion
    }
}
