using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using System;
using System.ComponentModel;
namespace Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects
{
    [Serializable, System.Runtime.InteropServices.ComVisible(false)]
    public class UDASymbolDataCollection : BindingList<UDAData>
    {
        public UDASymbolDataCollection()
        {

        }
        private string _requestID;

        public string RequestID
        {
            get { return _requestID; }
            set { _requestID = value; }
        }
        private string _userID;

        public string UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }
    }
}
