using System;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for Strategy.
    /// </summary>
    [Serializable]
    public class Strategy
    {
        int _strategyID = int.MinValue;
        string _name = string.Empty;

        public Strategy()
        {
        }

        public Strategy(int strategyID, string name)
        {
            _strategyID = strategyID;
            _name = name;
        }

        /// <summary>
        /// constructor overloading
        /// </summary>
        /// <param name="fullName"></param>
        public Strategy(int strategyID, string name, string fullName) : this(strategyID, name)
        {
            _fullName = fullName;
        }

        public int StrategyID
        {
            get { return _strategyID; }
            set { _strategyID = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        string _fullName = string.Empty;
        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        /// <value>The full name.</value>
        public string FullName
        {
            get { return _fullName; }
            set { _fullName = value; }
        }

        public override string ToString()
        {
            return FullName;
        }
    }
}
