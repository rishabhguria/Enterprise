using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.RuleEngine.BusinessObjects
{
    public class Attributes
    {
        private List<Prana.RuleEngine.BusinessObjects.Attribute> _attribute = new List<Prana.RuleEngine.BusinessObjects.Attribute>();
        public List<Attribute> attribute
        {
            get { return _attribute; }
            set { _attribute = value; }
        }

    }
}
