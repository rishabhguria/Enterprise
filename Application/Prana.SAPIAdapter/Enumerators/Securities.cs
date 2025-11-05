using Bloomberglp.Blpapi;
using System.Collections.Generic;
namespace Bloomberg.Library
{
    public class Securities : List<SecurityInfo>
    {
        readonly Element securities = null;

        public void Add(params string[] items)
        {
            foreach (string item in items)
            {
                SecurityInfo security = new SecurityInfo(securities, item);
                Add(security);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Securities"></see> class.
        /// </summary>
        /// <param name="securities">The securities.</param>
        public Securities(Element securities)
        {

            this.securities = securities;
        }
    }
}