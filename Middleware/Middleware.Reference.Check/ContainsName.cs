using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Middleware.Reference.Check
{
    class ContainsName : IEqualityComparer<QueryResults>
    {

        public bool Equals(QueryResults x, QueryResults y)
        {
            return x.Name.ToLower().Equals(y.Name.ToLower());
        }

        public int GetHashCode(QueryResults obj)
        {
            return obj.Name.ToLower().GetHashCode();
        }
    }
}
