using System;

namespace Prana.BusinessObjects
{
    [Serializable()]
    public class UnderLying
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Underlying"/> class.
        /// </summary>
        public UnderLying()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Underlying"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="name">The name.</param>
        public UnderLying(int id, string name)
        {
            _id = id;
            _name = name;
        }

        private int _id;

        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        /// <value>The ID.</value>
        public int UnderlyingID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;

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
