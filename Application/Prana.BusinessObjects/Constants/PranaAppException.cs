using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Prana.BusinessObjects
{

    [DataContract]
   public class PranaAppException
   {
        private  string _exception = "Error in Server";

       private string _stackTrace = string.Empty;

       
       public string StackTrace
       {
           get { return _stackTrace; }
           set { _stackTrace = value; }
       }

       private string _message = string.Empty;

       public string Message
       {
           get { return _message; }
           set { _message = value; }
       }
	
       public string Exception
        {
            get { return _exception; }
        }

       public PranaAppException(Exception ex)
       {
           _stackTrace = ex.StackTrace;
           _message = ex.Message;
       }

    }

}

