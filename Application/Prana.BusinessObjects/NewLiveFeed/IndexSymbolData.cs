using System;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Class IndexSymbolData
    /// </summary>
    [Serializable]
    public class IndexSymbolData : SymbolData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IndexSymbolData"/> class.
        /// </summary>
        public IndexSymbolData()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolData"></see> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="i">The i.</param>
        public IndexSymbolData(string[] data, ref int i)
            : base(data, ref i)
        {

        }
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return base.ToString();
        }
    }


}
