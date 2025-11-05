using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Installer.Library;

namespace Middleware.Installer
{
    class OrderByDependency : IComparer<ScriptItem>
    {
        public bool Equals(ScriptItem x, ScriptItem y)
        {
            return x.Order > y.Order;
        }

        public int GetHashCode(ScriptItem obj)
        {
           return obj.Order.GetHashCode();
        }

        public int Compare(ScriptItem x, ScriptItem y)
        {
            if (x.Order == y.Order)
                return 0;
            else if (x.Order > y.Order)
                return 1;
            else
                return -1;
            
            
        }
    }
}
