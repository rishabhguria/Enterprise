using Csla;
using System;

namespace Prana.PM.BLL
{
    [Serializable()]
    public class Underlying : BusinessBase<Underlying>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Underlying"/> class.
        /// </summary>
        public Underlying()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Underlying"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="name">The name.</param>
        public Underlying(int id, string name)
        {
            this.ID = id;
            this.Name = name;
        }

        private int _id;

        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        /// <value>The ID.</value>
        public int ID
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

        protected override object GetIdValue()
        {
            return _id;
        }
    }
}
