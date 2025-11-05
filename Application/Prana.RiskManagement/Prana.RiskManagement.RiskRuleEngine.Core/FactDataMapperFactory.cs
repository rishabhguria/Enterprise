using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Nirvana.RuleEngine.Core
{
    public class FactDataMapperFactory
    {
        public static IFactDataCollection CreateInstance(string className)
        {
            
            return (IFactDataCollection) Activator.CreateInstance(Type.GetType(className));
                      
        }

    }
}
