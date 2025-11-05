using System;

namespace Prana.NirvanaQualityChecker
{
    public class XMLData
    {
        private string SMDBName;
        private Boolean Checkbox;

        public Boolean _Checkbox
        {
            get { return Checkbox; }
            set { Checkbox = value; }
        }

        public string _SMDBName
        {
            get { return SMDBName; }
            set { SMDBName = value; }
        }
        private string ClinetDBName;

        public string _ClinetDBName
        {
            get { return ClinetDBName; }
            set { ClinetDBName = value; }
        }
        private string IPAddress;

        public string _IPAddress
        {
            get { return IPAddress; }
            set { IPAddress = value; }
        }

    }
}
