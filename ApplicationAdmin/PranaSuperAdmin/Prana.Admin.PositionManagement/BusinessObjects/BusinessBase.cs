using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;


namespace Nirvana.Admin.PositionManagement.BusinessObjects
{


    public class BusinessBase<T> : INotifyPropertyChanged, IDataErrorInfo
    {



        private EntityStateEnum _EntityState;

        public EntityStateEnum EntityState
        {

            get { return _EntityState; }

            private set { _EntityState = value; }

        }



        private void DataStateChanged(EntityStateEnum dataState, string propertyName)
        {

            // Raise the event

            if (PropertyChanged != null && propertyName != null)
            {

                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

            }



            // If the state is deleted, mark it as deleted

            if (dataState == EntityStateEnum.Deleted)
            {

                this.EntityState = dataState;

            }



            if (this.EntityState == EntityStateEnum.Unchanged)
            {

                this.EntityState = dataState;

            }

        }







        #region INotifyPropertyChanged Members



        public event PropertyChangedEventHandler PropertyChanged;



        protected void FirePropertyChanged(string propertyName)
        {

            PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);

            if (PropertyChanged != null )
            {

                PropertyChanged(this, e);

            }

        }



        #endregion


        #region IDataErrorInfo Members

        // Variables for IDataErrorInfo
        public string errorMessage = string.Empty;
        protected Dictionary<string, string> propErrors = new Dictionary<string, string>();

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        /// <value></value>
        /// <returns>An error message indicating what is wrong with this object. The default is an empty string ("").</returns>
        public string Error
        {
            get
            {
                return errorMessage;
            }
        }

        /// <summary>
        /// Gets the error message <see cref="System.String"/> with the specified column name.
        /// </summary>
        /// <value></value>
        public string this[string columnName]
        {
            get
            {
                return (string)propErrors[columnName];
            }
        }

        #endregion

    }
}
