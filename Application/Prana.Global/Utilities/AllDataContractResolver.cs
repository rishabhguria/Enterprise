using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

namespace Prana.Global.Utilities
{
    class AllDataContractResolver : DefaultContractResolver
    {
        static readonly Dictionary<Type, IList<JsonProperty>> dic = new Dictionary<Type, IList<JsonProperty>>();
        /// <summary>
        /// The base class properties are ignored by default . We are changing this behavior by creating a custom contract resolver.
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="memberSerialization">MemberSerialization</param>
        /// <returns>IList of tyoe JsonProperty</returns>
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            if (dic.ContainsKey(type))
                return dic[type];
            else
            {
                lock (dic)
                {
                    if (dic.ContainsKey(type))
                        return dic[type];
                    else
                    {
                        var list = base.CreateProperties(type, memberSerialization);
                        foreach (var prop in list)
                        {
                            prop.Ignored = false; // Don't ignore any property
                        }

                        dic.Add(type, list);
                        return list;
                    }
                }

            }

        }
    }
}
