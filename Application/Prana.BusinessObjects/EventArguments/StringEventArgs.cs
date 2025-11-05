using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// for passing arguments 
    /// </summary>
    public class StringEventArgs : EventArgs
    {
        public string value { get; set; }
    }
}
