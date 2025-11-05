using Prana.Global;

namespace Prana.BusinessObjects.Classes.PostTrade
{
    /// <summary>
    /// The StateObject class
    /// </summary>
    public class StateObject
    {
        /// <summary>
        /// The taxlot state
        /// </summary>
        private ApplicationConstants.TaxLotState state;

        /// <summary>
        /// Gets or sets the Taxlot state
        /// </summary>
        public ApplicationConstants.TaxLotState State
        {
            get { return state; }
            set { state = value; }
        }

        /// <summary>
        /// The Id of state object
        /// </summary>
        private string id;

        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// Bool issent
        /// </summary>
        private bool issent;

        /// <summary>
        /// Gets or sets IsSent
        /// </summary>
        public bool IsSent
        {
            get { return issent; }
            set { issent = value; }
        }

    }
}
