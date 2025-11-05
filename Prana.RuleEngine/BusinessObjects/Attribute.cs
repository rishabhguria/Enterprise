using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.RuleEngine.BusinessObjects
{
    public class Attribute
    {
        private String _attributeName;

        public String attributeName
        {
            get { return _attributeName; }
            set { _attributeName = value; }
        }
        private string _value;

        public string value
        {
            get { return _value; }
            set { _value = value; }
        }

    }
}
