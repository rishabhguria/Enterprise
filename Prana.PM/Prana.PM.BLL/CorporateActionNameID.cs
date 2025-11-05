namespace Prana.PM.BLL
{
    public class CorporateActionNameID
    {
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
        /// Gets or sets the name of Corporate Action.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
