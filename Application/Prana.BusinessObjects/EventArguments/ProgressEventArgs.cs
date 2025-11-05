using Prana.BusinessObjects.AppConstants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// for passing arguments 
    /// </summary>
    public class ProgressEventArgs : EventArgs
    {
        public string message { get; set; }

        public bool isAddMessage { get; set; }

        public Progress progress { get; set; }

    }
}
