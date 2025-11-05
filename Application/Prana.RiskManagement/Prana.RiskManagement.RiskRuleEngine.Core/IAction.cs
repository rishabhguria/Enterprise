using System;
using System.Collections.Generic;
using System.Text;

namespace Nirvana.RuleEngine.Core
{
    public interface IAction
    {
        string Name
        {
            get;
            set;
        }

        string Message
        {
            get;
            set;
        }
    }
}
