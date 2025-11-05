using System;

namespace Prana.NirvanaQualityChecker
{
    class Detail
    {
        private static String _moduleName;
        public String Module
        {
            get { return _moduleName; }
            set { _moduleName = value; }
        }
        private static String _scriptName;
        public String Script
        {
            get { return _scriptName; }
            set { _scriptName = value; }
        }
        private static String _errormsg;
        public String Error
        {
            get { return _errormsg; }
            set { _errormsg = value; }
        }
    }
}
