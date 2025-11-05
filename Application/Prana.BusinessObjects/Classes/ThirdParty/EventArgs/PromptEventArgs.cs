using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class PromptEventArgs : EventArgs
    {
        private ThirdPartyUserDefinedFormat formatter;

        public ThirdPartyUserDefinedFormat Formatter
        {
            get { return formatter; }
            set { formatter = value; }
        }
    }
}
