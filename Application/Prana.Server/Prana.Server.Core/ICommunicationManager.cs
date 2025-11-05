using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.Server
{
    public interface ICommunicationManager
    {


    }
    public delegate void MessageReceivedDelegate(object sender,string message);
    public delegate void ExceptionDelegate(object sender, Exception ex);
}
