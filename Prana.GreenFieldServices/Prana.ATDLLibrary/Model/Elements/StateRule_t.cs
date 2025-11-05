using Prana.ATDLLibrary.Model.Elements.Support;

namespace Prana.ATDLLibrary.Model.Elements
{
    // TODO: Implement IDisposable
    public class StateRule_t : EditEvaluator<Control_t>
    {
        /// <summary>
        /// Enabled state for this state rule.
        /// </summary>
        public bool? Enabled { get; set; }

        /// <summary>
        /// Value attribute for this state rule.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Visible state for this state rule.
        /// </summary>
        public bool? Visible { get; set; }

        /// <summary>
        /// ReuiredGroupName state for this state rule
        /// </summary>
        public string ReuiredGroupName { get; set; }

        /// <summary>
        /// MinValue state for this state rule
        /// </summary>
        public string MinValue { get; set; }

        /// <summary>
        /// MaxValue state for this state rule
        /// </summary>
        public string MaxValue { get; set; }

        /// <summary>
        /// ValidateWithOrderProperty state for this state rule
        /// </summary>
        public string ValidateWithOrderProperty { get; set; }
        /// <summary>
        /// Required state for this state rule
        /// </summary>
        public bool? Required { get; set; }
    }
}
