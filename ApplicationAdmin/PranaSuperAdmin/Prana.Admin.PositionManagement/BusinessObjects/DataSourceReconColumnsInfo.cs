using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    class DataSourceReconColumnsInfo : INotifyPropertyChanged
    {

        private DataSourceNameID _datasourceNameId;

        public DataSourceNameID DataSourceNameIDValue
        {
            get { return _datasourceNameId; }
            set
            {
                _datasourceNameId = value;
                OnPropertyChanged("DataSourceNameIDValue");
            }
        }

        private BindingList<AppReconciliedColumn> _appReconciliedColumnList;

        public BindingList<AppReconciliedColumn> AppReconciliedColumnList
        {
            get { return _appReconciliedColumnList; }
            set 
            {
                _appReconciliedColumnList = value;
                OnPropertyChanged("AppReconciliedColumnList");
            }
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
