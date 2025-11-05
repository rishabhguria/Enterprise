using Csla;
using System;

namespace Prana.BusinessObjects.PositionManagement
{
    /// <summary>
    /// Summary description for Strategy.
    //TODO : We need to use the same object of Prana.BusinessObject, hence need to merge...
    /// </summary>
    [Serializable, System.Runtime.InteropServices.ComVisible(false)]
    public class Strategy : BusinessBase<Strategy>
    {
        int _strategyID = int.MinValue;
        string _name = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="Strategy"/> class.
        /// </summary>
		public Strategy()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Strategy"/> class.
        /// </summary>
        /// <param name="strategyID">The strategy ID.</param>
        /// <param name="name">The name.</param>
		public Strategy(int strategyID, string name)
        {
            _strategyID = strategyID;
            _name = name;
        }

        /// <summary>
        /// Gets or sets the strategy ID.
        /// </summary>
        /// <value>The strategy ID.</value>
		public int StrategyID
        {
            get { return _strategyID; }
            set { _strategyID = value; }
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

        private string _shortName;

        public string ShortName
        {
            get { return _shortName; }
            set { _shortName = value; }
        }

        protected override object GetIdValue()
        {
            if (_name == null)
                _name = string.Empty;

            return _name;
        }
    }
}
