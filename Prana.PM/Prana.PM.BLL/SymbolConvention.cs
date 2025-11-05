using Csla;
using System;

namespace Prana.PM.BLL
{
    [Serializable]
    public class SymbolConvention : BusinessBase<SymbolConvention>
    {
        public SymbolConvention()
        {
        }
        public SymbolConvention(int id, string name)
        {
            _ID = id;
            _name = name;
        }
        private int _ID;

        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        /// <value>The ID.</value>
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
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

        private string _description;

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }




        /// <summary>
        /// Gets the id value.
        /// </summary>
        /// <returns></returns>
        protected override object GetIdValue()
        {
            return ID;
        }
    }
}
