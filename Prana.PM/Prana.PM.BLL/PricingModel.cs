namespace Prana.PM.BLL
{
    /// <summary>
    /// Represents Pricing Models for e.g., Numerix, Windale, etc.
    /// </summary>
    public class PricingModel
    {
        private int _id;
        private string _name;

        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        /// <value>The ID.</value>
        public int ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
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
                return _name;
            }
            set
            {
                _name = value;
            }
        }
    }
}
