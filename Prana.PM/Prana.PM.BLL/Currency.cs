using Csla;
using System;

namespace Prana.PM.BLL
{
    /// <summary>
    /// Summary description for Currency.
    /// </summary>
    [Serializable()]
    public class Currency : BusinessBase<Currency>
    {

        #region private members

        private int _id = Int32.MinValue;
        private string _name = string.Empty;

        #endregion

        public Currency()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public Currency(int id, string name, string symbol)
        {
            _id = id;
            _name = name;
            _symbol = symbol;
        }
        #region

        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        /// <value>The ID.</value>
        public int ID
        {
            get
            {
                return this._id;
            }

            set
            {
                this._id = value;
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
		public string Name
        {
            get
            {
                return this._name;
            }

            set
            {
                this._name = value;
            }
        }

        private string _symbol;

        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        public override string ToString()
        {
            return Symbol;
        }

        #endregion


        public override bool IsValid
        {
            get { return base.IsValid; }
        }

        public override bool IsDirty
        {
            get { return base.IsDirty; }
        }

        /// <summary>
        /// TODO : Can't afford to have this ID property in multiuer environment
        /// </summary>
        /// <returns></returns>
        protected override object GetIdValue()
        {
            return _id;
        }

    }
}
