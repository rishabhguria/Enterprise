using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Data.Linq;

namespace Historical.Pricing.Library
{
    /// <summary>
    /// Symbol Request
    /// </summary>
    /// <remarks></remarks>
    public class SymbolRequest
    {
      
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        /// <remarks></remarks>
        public string MarkSymbol { get; set; }
        /// <summary>
        /// Gets or sets the asset.
        /// </summary>
        /// <value>The asset.</value>
        /// <remarks></remarks>
        public string Asset { get; set; }
        /// <summary>
        /// Gets or sets the symbol.
        /// </summary>
        /// <value>The symbol.</value>
        /// <remarks></remarks>
        public string PriceSymbol { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [use divisor].
        /// </summary>
        /// <value><c>true</c> if [use divisor]; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        public bool UseDivisor { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        public SymbolRequest()
        {
            PriceSymbol = string.Empty;
            MarkSymbol = string.Empty;
            Asset = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolRequest"/> class.
        /// </summary>
        /// <param name="markSymbol">The mark symbol.</param>
        /// <param name="priceSymbol">The price symbol.</param>
        /// <param name="asset">The asset.</param>
        /// <remarks></remarks>
        public SymbolRequest(string markSymbol, string priceSymbol, string asset)
        {
            // TODO: Complete member initialization
            MarkSymbol = markSymbol;
            PriceSymbol = priceSymbol;
            Asset = asset;
        }
        /// <summary>
        /// Gets the bloomberg key.
        /// </summary>
        /// <value>The bloomberg key.</value>
        public string BloombergKey
        {
            get { return String.Format("{0} {1}", PriceSymbol, Asset); }
        }
    }
}
