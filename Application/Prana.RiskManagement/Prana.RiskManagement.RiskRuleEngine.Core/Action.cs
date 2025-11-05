using System;
using System.Collections.Generic;
using System.Text;

namespace Nirvana.RuleEngine.Core
{
    public class Action : IAction
    {
        #region IAction Members

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }


        private string _message;

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }


        #endregion
    }
}
