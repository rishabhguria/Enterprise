using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Historical.Pricing.Library
{
    /// <summary>
    /// Symbol Map Dictionary
    /// </summary>
    /// <remarks></remarks>
    public class SymbolMapping : Dictionary<string, List<string>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        public SymbolMapping()
        {
            
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolMapping"/> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        /// <remarks></remarks>
        public SymbolMapping(int capacity)
            : base(capacity)
        {
            
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolMapping"/> class.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        /// <remarks></remarks>
        public SymbolMapping(IEqualityComparer<string> comparer)
            : base(comparer)
        {
            
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolMapping"/> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        /// <param name="comparer">The comparer.</param>
        /// <remarks></remarks>
        public SymbolMapping(int capacity, IEqualityComparer<string> comparer)
            : base(capacity, comparer)
        {
            
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolMapping"/> class.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <remarks></remarks>
        public SymbolMapping(IDictionary<string, List<string>> dictionary)
            : base(dictionary)
        {
            
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolMapping"/> class.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="comparer">The comparer.</param>
        /// <remarks></remarks>
        public SymbolMapping(IDictionary<string, List<string>> dictionary, IEqualityComparer<string> comparer)
            : base(dictionary, comparer)
        {
            
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolMapping"/> class.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        /// <remarks></remarks>
        protected SymbolMapping(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
            
        }
        /// <summary>
        /// Maps the specified provider.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="symbols">The symbols.</param>
        /// <remarks></remarks>
        public void Map(string provider, List<string> symbols)
        {
            if (this.ContainsKey(provider) == false)
                this.Add(provider, symbols);
            else
                this[provider] = symbols;
        }
        /// <summary>
        /// Gets the provider.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string GetProvider(string symbol)
        {
            List<string> providers = new List<string>();
            foreach (var kvp in this)
                if (kvp.Value.Contains(symbol))
                    providers.Add(kvp.Key);
            return string.Join(";", providers.ToArray());
        }
    }
}
