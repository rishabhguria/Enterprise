using Prana.BusinessObjects;
using Prana.Global;
using Prana.Interfaces;
using System.Collections.Generic;

namespace Prana.OptionServer
{
    internal class ThreadInfo
    {
        private List<string> _listOfSymbols = null;
        public List<string> ListOfSymbol
        {
            get { return _listOfSymbols; }
            set { _listOfSymbols = value; }
        }
        ApplicationConstants.SymbologyCodes _symbologyCode;
        public ApplicationConstants.SymbologyCodes SymbologyCode
        {
            get { return _symbologyCode; }
            set { _symbologyCode = value; }
        }

        bool _isGreekRequired;
        public bool IsGreekRequired
        {
            get { return _isGreekRequired; }
            set { _isGreekRequired = value; }
        }

        ILiveFeedCallback _subscriber;
        public ILiveFeedCallback Subscriber
        {
            get { return _subscriber; }
            set { _subscriber = value; }
        }

        bool _completeInfo;
        public bool CompleteInfo
        {
            get { return _completeInfo; }
            set { _completeInfo = value; }
        }

        private List<fxInfo> _listOfFxSymbols = null;
        public List<fxInfo> ListOfFxSymbols
        {
            get { return _listOfFxSymbols; }
            set { _listOfFxSymbols = value; }
        }
    }
}
