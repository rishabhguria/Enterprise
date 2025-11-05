using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nirvana.TestAutomation.UIAutomation
{
    public class CustomizedDataKeeperClasses
    {
           

        public class WinAppAutomationMapper
        {
            public string SelectorType;

            public string SelectorValue;
        }


        public class ModuleStepWiseGridStorrer
        {
            public string ModuleName;
            public string StepName;
            public string gridElementName;
            public string gridType;
            public string duplicateValue;
            public List<string> duplicateValueReplacer;


        }
    }
}
