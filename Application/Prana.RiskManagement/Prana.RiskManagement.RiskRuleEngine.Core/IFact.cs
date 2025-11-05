using System;
using System.Collections.Generic;
using System.Text;

namespace Nirvana.RuleEngine.Core
{
    public interface IFact
    {
        string Id
        {
            get;
            set;
        }

        string PropertyName
        {
            get;
            set;
        }
    }
}
