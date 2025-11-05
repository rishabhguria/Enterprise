using System;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for NewsEventArg.
    /// </summary>
    public class LaunchFormEventArgs : EventArgs
    {
        public LaunchFormEventArgs(object param)
        {
            _params = param;
        }

        private object _params;
        public object Params
        {
            get { return _params; }
            set { _params = value; }
        }
    }
}
