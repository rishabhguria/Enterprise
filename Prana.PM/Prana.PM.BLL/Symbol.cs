namespace Prana.PM.BLL
{
    /// <summary>
    /// Summary description for Symbol.
    /// </summary>
    public class Symbol
    {
        #region Private members

        private int _id = int.MinValue;
        private string _name = string.Empty;
        private string _company = string.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Symbol"/> class.
        /// </summary>
        public Symbol()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Symbol"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="name">The name.</param>
		public Symbol(int id, string name)
        {
            _id = id;
            _name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Symbol"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="name">The name.</param>
        /// <param name="company">The company.</param>
        public Symbol(int id, string name, string company)
        {
            _id = id;
            _name = name;
            _company = company;
        }

        #endregion

        #region Properties

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Company
        {
            get { return _company; }
            set { _company = value; }
        }

        public override string ToString()
        {
            return Name;
        }
        #endregion
    }
}
