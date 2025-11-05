using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// for passing arguments 
    /// </summary>
    public class ExceptionEventArgs : EventArgs
    {
        public Exception value { get; set; }
    }
}
