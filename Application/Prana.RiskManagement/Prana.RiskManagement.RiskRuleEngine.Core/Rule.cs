using System;
using System.Collections.Generic;
using System.Text;

namespace Nirvana.RuleEngine.Core
{
    public class Rule : Nirvana.RuleEngine.Core.IRule
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }


        private string _description;

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }


        private string _condition;

        public string Condition
        {
            get { return _condition; }
            set { _condition = value; }
        }

        private Actions _action;

        public Actions Actions
        {
            get { return _action; }
            set { _action = value; }
        }
	
	
    }
}
