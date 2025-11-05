using System.Collections.ObjectModel;
using System.Diagnostics;
using Prana.ATDLLibrary.Model.Elements;
using Prana.LogManager;

namespace Prana.ATDLLibrary.Model.Collections
{
    public class StateRuleCollection : Collection<StateRule_t>
    {

        private readonly string _owner;

        public StateRuleCollection(Control_t owner)
        {
            _owner = owner.Id;
        }

        public new void Add(StateRule_t item)
        {
            base.Add(item);

            Logger.LoggerWrite(string.Format("StateRule_t {0} added to StateRules for control Id {1}", item.ToString(), _owner), LoggingConstants.ATDL_LOGGING, 1, 1, TraceEventType.Verbose);
        }


    }
}
