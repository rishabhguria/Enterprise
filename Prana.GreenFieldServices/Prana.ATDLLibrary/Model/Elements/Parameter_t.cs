using System.Diagnostics;
using Prana.ATDLLibrary.Fix;
using Prana.ATDLLibrary.Model.Collections;
using Prana.ATDLLibrary.Model.Elements.Support;
using Prana.ATDLLibrary.Model.Enumerations;
using Prana.LogManager;

namespace Prana.ATDLLibrary.Model.Elements
{
    /// <summary>
    /// Represents a Parameter_t type.
    /// </summary>
    /// <typeparam name="T">Valid FIXatdl type <see cref="Prana.ATDLLibrary.Model.Types"/></typeparam>
    /// <example>To create a parameter with underlying type Amt_t, use <c>new Parameter_t&lt;Amt_t&gt;</c>.</example>
    public class Parameter_t<T> : IParameter where T : new()
    {

        private readonly EnumPairCollection _enumPairs = new EnumPairCollection();

        /// <summary>
        /// The underlying value of this parameter.
        /// </summary>
        protected T _value;

        /// <summary>
        /// Creates a new instance of <see cref="Parameter_t{T}"/>.
        /// </summary>
        /// <param name="name">Name of this parameter.  See <see cref="Parameter{T}.Name"/> for constraints on parameter names.</param>
        public Parameter_t(string name)
        {
            Logger.LoggerWrite(string.Format("New Parameter_t<{0}> created, Name='{1}'.", typeof(T).Name, name), LoggingConstants.ATDL_LOGGING, 1, 1, TraceEventType.Verbose);

            Name = name;
            Type = typeof(T).Name;

            // Set FIXatdl defaults
            Use = Use_t.Optional;
            MutableOnCxlRpl = true;

            _value = new T();
        }

        /// <summary>
        /// Value accessor.  <b>This property is intended to be used for deserialization purposes only.</b>
        /// </summary>
        public T Value { get { return _value; } }

        #region IParameter Members


        /// <summary>Gets/sets the DefinedByFIX property, which indicates whether the parameter is a redefinition of a 
        /// standard FIX tag. The default value is false.</summary>
        public bool? DefinedByFix { get; set; }

        /// <summary>
        /// Gets/sets the enum pairs for this parameter.  Although it doesn't necessarily make sense in all cases, all
        /// parameter types within FIXatdl may contain an EnumPairs element, so we must support it at the base level.
        /// </summary>
        /// <value>The enum pairs.  Will be an empty collection if no enum pairs were present in the parameter definition.</value>
        public EnumPairCollection EnumPairs { get { return _enumPairs; } }

        /// <summary>
        /// Indicates whether the parameter has an EnumPairs element with at least one sub-element.
        /// </summary>
        public bool HasEnumPairs { get { return _enumPairs.Count != 0; } }

        /// <summary>Gets or sets the FIX tag for this parameter, i.e., the tag that will hold the value of the 
        /// parameter. Required when parameter value is intended to be transported over the wire.  If fixTag is not 
        /// provided then the Strategies-level attribute, tag957Support, must be set to true, indicating that the 
        /// order recipient expects to receive algo parameters in the StrategyParameterGrp repeating group beginning 
        /// at tag 957.  <b>NB Atdl4net does not currently support usage of the StrategyParameterGrp element.</b></summary>
        /// <value>The FIX tag to use.</value>
        public FixTag? FixTag { get; set; }

        /// <summary>Indicates whether this parameter’s value can be modified by an Order Cancel/Replace Request message.
        /// The default value for this field is true.
        /// </summary>
        public bool? MutableOnCxlRpl { get; set; }

        /// <summary>The name of this parameter.</summary>
        /// <remarks>No two parameters of any strategy may have the same name. The name may be used as a unique key when referenced 
        /// from the other sub-schemas. Names must begin with an alpha character followed only by alpha-numeric characters 
        /// and must not contain whitespace characters.</remarks>
        public string Name { get; set; }

        /// <summary>Indicates how to interpret those tags that were populated in an original order but are not populated in
        /// a subsequent cancel/replace of the order message. If this value is true then revert to the value of the original 
        /// order, otherwise a null value or the parameter’s default value (Control/@initValue) is to be used or if none is
        /// specified, the parameter is to be omitted.  The default value for this field is false.<br/>
        /// </summary>
        /// <remarks>Although revertOnCxlRpl and mutableOnCxlRpl might appear to be mutually exclusive, this is not strictly
        /// the case, and as the default value for mutableOnCxlRpl is 'true', it is recommended practice to explicitly include
        /// mutableOnCxlRpl="false" if the option revertOnCxlRpl="true" is set for a given parameter (assuming of course this 
        /// is the intended behaviour).</remarks>
        public bool? RevertOnCxlRpl { get; set; }

        /// <summary>
        /// Gets or sets the type name of this parameter.
        /// </summary>
        /// <value>The type name (one of Amt_t, Boolean_t, Char_t, etc.).</value>
        public string Type { get; set; }

        /// <summary>Indicates whether a parameter is optional or required. Valid values are "optional" and "required".
        /// The default value for this field is "optional".
        /// </summary>
        public Use_t Use { get; set; }
        public string DefaultRegionKey { get; set; }
        public string DefaultRegionValue { get; set; }
        public string RequiredInRegions { get; set; }
        public string EnableInRegions { get; set; }
        public string AvailableInRegions { get; set; }
        public string ReplaceVisible { get; set; }
        //Added nullable SendOnReplace
        public bool? SendOnReplace { get; set; }
        public string IsReuiredGroupName { get; set; }
        public string ValidationWith { get; set; }
        public string ValidateWithOrderProperty { get; set; }
        public string Precision { get; set; }

        #endregion
    }
}
