using System;
using System.Collections.Generic;
using System.Text;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    /// <summary>
    /// Summary description for Fund.
    /// </summary>
    [Serializable]
    public class Fund
    {
        int _id = int.MinValue;
        string _name = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="Fund"/> class.
        /// </summary>
        public Fund()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Fund"/> class.
        /// </summary>
        /// <param name="fundID">The fund ID.</param>
        /// <param name="name">The name.</param>
        public Fund(int fundID, string name)
        {
            _id = fundID;
            _name = name;
        }

        /// <summary>
        /// Gets or sets the fund ID.
        /// </summary>
        /// <value>The fund ID.</value>
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
