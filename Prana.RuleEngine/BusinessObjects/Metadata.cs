using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.RuleEngine.BussinessObjects
{
    public class Metadata
    {
        private string _format;

        public string format
        {
            get { return _format; }
            set { _format = value; }
        }

        private string _uuid;
        public string uuid
        {
            get { return _uuid; }
            set { _uuid = value; }
        }

        private string _created;
        public string created
        {
            get { return _created; }
            set { _created = value; }
        }

        private bool _disabled;
        public bool disabled
        {
            get { return _disabled; }
            set { _disabled = value; }
        }

        private int _versionNumber;
        public int versionNumber
        {
            get { return _versionNumber; }
            set { _versionNumber = value; }
        }



    }
}
