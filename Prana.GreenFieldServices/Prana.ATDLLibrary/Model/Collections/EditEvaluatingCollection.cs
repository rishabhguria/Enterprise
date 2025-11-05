using System.Collections.ObjectModel;
using System.Diagnostics;
using Prana.ATDLLibrary.Model.Elements.Support;
using Prana.ATDLLibrary.Model.Enumerations;
using Prana.LogManager;

namespace Prana.ATDLLibrary.Model.Collections
{
    /// <summary>
    /// Collection used to store typed instances of Edit_t, either for validating parameters via StrategyEdit, or 
    /// for implementing StateRules using control values.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EditEvaluatingCollection<T> : Collection<IEdit<T>>
    {
        /// <summary>
        /// Logic operator for this collection of Edits.
        /// </summary>
        public LogicOperator_t? LogicOperator { get; set; }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public new void Add(IEdit<T> item)
        {
            base.Add(item);         

            Logger.LoggerWrite(string.Format("Edit_t {0} added to EditEvaluatingCollection", item.ToString()), LoggingConstants.ATDL_LOGGING, 1, 1, TraceEventType.Verbose);
        }
    }
}
