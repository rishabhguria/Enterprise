using System;

namespace Prana.BusinessObjects
{
    public class ControlState : EventArgs
    {
        bool _optionalFieldsVisible = false;
        bool _algoDisplayed = false;

        public bool AlgoDisplayed
        {
            get { return _algoDisplayed; }
            set { _algoDisplayed = value; }
        }
        public bool IsExpanded
        {
            get { return _optionalFieldsVisible; }
            set { _optionalFieldsVisible = value; }

        }
    }
}
