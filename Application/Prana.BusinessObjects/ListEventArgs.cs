using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// for passing arguments 
    /// </summary>
    public class ListEventAargs : EventArgs
    {
        /// <summary>
        /// Pass generic data object.
        /// </summary>
        public object argsObject = new object();

        /// <summary>
        /// pass argument as List of string values 
        /// </summary>

        public List<string> listOfValues = new List<string>();

        public object argsObject2 = new object();
    }
}
