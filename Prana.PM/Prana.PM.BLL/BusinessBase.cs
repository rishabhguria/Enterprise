///Applying the CSLA framework, hence commented custom created BusinessLogic

//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.ComponentModel;

//namespace Prana.PM.BLL
//{
//    public class BusinessBase : INotifyPropertyChanged , IDataErrorInfo
//    {
//        #region Commented Code
//        //private EntityStateEnum _EntityState;
//        //public EntityStateEnum EntityState
//        //{

//        //    get { return _EntityState; }
//        //    private set { _EntityState = value; }
//        //}

//        //private void DataStateChanged(EntityStateEnum dataState, string propertyName)
//        //{
//        //    // Raise the event
//        //    if (PropertyChanged != null && propertyName != null)
//        //    {
//        //        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
//        //    }

//        //    // If the state is deleted, mark it as deleted
//        //    if (dataState == EntityStateEnum.Deleted)
//        //    {
//        //        this.EntityState = dataState;
//        //    }

//        //    if (this.EntityState == EntityStateEnum.Unchanged)
//        //    {
//        //        this.EntityState = dataState;
//        //    }
//        //}
//        #endregion Commented Code

//        #region INotifyPropertyChanged Members

//        public event PropertyChangedEventHandler PropertyChanged;

//        protected void FirePropertyChanged(string propertyName)
//        {
//            PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);

//            if (PropertyChanged != null)
//            {
//                PropertyChanged(this, e);
//            }
//        }

//        #endregion

//        #region IDataErrorInfo Members

//        protected string _error = string.Empty;
//        protected Dictionary<string, string> _errDict = new Dictionary<string, string>();

//        public string Error
//        {
//            get { return _error; }
//        }

//        public string this[string columnName]
//        {
//            get
//            {
//                if (_errDict.ContainsKey(columnName))
//                {
//                    return _errDict[columnName];
//                }
//                return null;
//            }
//        }

//        #endregion

//    }
//}
