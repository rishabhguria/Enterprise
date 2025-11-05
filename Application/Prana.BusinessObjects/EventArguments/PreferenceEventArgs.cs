using Prana.BusinessObjects;
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
    public class PreferenceEventArgs : EventArgs
    {
        public string preferencesModuleName { get; set; }
        public IPreferenceData IPrefData { get; set; }
    }
}
