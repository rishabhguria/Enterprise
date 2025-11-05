using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nirvana.TestAutomation.BussinessObjects
{
    /// <summary>
    /// Class Containing Properties Of File
    /// </summary>
    public class ConfigModificatorSettings
    {
        public string RootNode { get; set; }

        public string NodeForEdit { get; set; }

        public string ConfigPath { get; set; }

        public ConfigModificatorSettings(String Path)
        {
            NodeForEdit = "//add[@key='{0}']";
            RootNode = "//appSettings";
            ConfigPath = Path;
        }
    }
}
