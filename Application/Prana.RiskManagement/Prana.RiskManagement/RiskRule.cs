using System;
using System.Collections.Generic;
using System.Text;

namespace Nirvana.RiskManagement
{
    public class RiskRule : Nirvana.RuleEngine.Core.Rule
    {
        private string _subject;

        public string Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }
	
    }
}
