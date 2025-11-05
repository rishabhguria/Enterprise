using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.RuleEngine.BusinessObjects
{
   public class Rule
    {
        private string _name;

        public string name
        {
            get { return _name; }
            set { _name = value; }
        }
        private string _modelVersion;

        public string modelVersion
        {
            get { return _modelVersion; }
            set { _modelVersion = value; }
        }

        private Attributes _attributes = new Attributes();
        public Attributes attributes
        {
            get { return _attributes; }
            set { _attributes = value; }
        }

       
	
    }
}
