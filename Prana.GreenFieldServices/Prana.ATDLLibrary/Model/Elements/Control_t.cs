using Prana.ATDLLibrary.Model.Collections;
using Prana.ATDLLibrary.Model.Controls.Support;
using Prana.ATDLLibrary.Model.Enumerations;

namespace Prana.ATDLLibrary.Model.Elements
{
    /// <summary>
    /// Base class for all concrete <see cref="Control_t"/> types.
    /// </summary>
    public abstract class Control_t
    {
        private readonly StateRuleCollection _stateRules;

        /// <summary>
        /// Initializes a new <see cref="Control_t"/> instance with the specified identifier as id.
        /// </summary>
        /// <param name="id">Id of this control.</param>
        protected Control_t(string id)
        {
            Id = id;
            _stateRules = new StateRuleCollection(this);
        }

        #region Control_t Attributes

        // NB InitValue is not defined at this level because its data type depends on the type of control.

        /// <summary>For implementing systems that support saving order templates or pre-populated orders for basket trading/list
        ///  trading this attribute specifies that the control should be disabled when the order screen is going to be saved as a
        ///  template and not actually used to place an order.</summary>
        public bool? DisableForTemplate { get; set; }

        /// <summary>Unique identifier of this control. No two controls of the same strategy can have the same ID.</summary>
        public string Id { get; set; }

        /// <summary>Indicates the initialization value is to be taken from this standard FIX field. Format: "FIX_" + FIXFieldName. 
        /// E.g. "FIX_OrderQty".  Required when initPolicy=”UseFixField”.</summary>
        public string InitFixField { get; set; }

        /// <summary>Describes how to initialize the control.  If the value of this attribute is undefined or equal to "UseValue" and
        ///  initValue is defined then initialize with initValue.  If the value is equal to "UseFixField" then attempt to initialize 
        /// with the value of the tag specified in initFixField. If the value is equal to "UseFixField" and it is not possible to 
        /// access the value of the specified fix tag then revert to using initValue. If the value is equal to "UseFixField", the 
        /// field is not accessible, and initValue is not defined, then do not initialize.</summary>
        public InitPolicy_t? InitPolicy { get; set; }

        /// <summary>A title for this control which may be displayed.</summary>
        public string Label { get; set; }
        public string RegionWiseValueAvailable { get; set; }
        public string InnerControlNames { get; set; }
        public string InnerControlValues { get; set; }
        public bool? IsBlankAllowed { get; set; }

        /// <summary>The name of the parameter for which this control gives the visual representation. A parameter with this name 
        /// must be defined within the same strategy as this control.</summary>
        /// <remarks>The <see cref="ReferencedParameter"/> property provides access to the parameter instance itself, whilst
        /// this property provides access to the name of the parameter.</remarks>
        public string ParameterRef { get; set; }

        /// <summary>Tool tip text for rendered GUI objects rendered for the parameter.</summary>
        public string ToolTip { get; set; }

        #endregion

        /// <summary>
        /// Indicates whether this control can be toggled (i.e., is a checkbox or radiobutton).
        /// </summary>
        public bool IsToggleable { get { return this is BinaryControlBase; } }

        /// <summary>
        /// Gets the collection of <see cref="StateRule_t"/>s for this control.
        /// </summary>
        public StateRuleCollection StateRules { get { return _stateRules; } }
    }
}
