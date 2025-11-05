using Prana.ATDLLibrary.Model.Types.Support;

namespace Prana.ATDLLibrary.Model.Types
{
    /// <summary>
    /// Represents a char field containing one of two values: "Y" = True/Yes; "N" = False/No.'
    /// </summary>
    public class Boolean_t : AtdlValueType<bool>
    {
        /// <summary>Gets or sets the false wire value (for use with Boolean type parameters).<br/>
        /// <b>This attribute is targeted for deprecation.</b><br/>
        /// Defines the value with which to populate the FIX message when the boolean parameter is False. Overrides the 
        /// standard FIX boolean value of “N”. I.e. if this attribute is not provided then the order-sending application 
        /// must use “N”.<br/>
        /// If it is desired that the FIX message is not to be populated with this tag when the value of the parameter is 
        /// false, then falseWireValue should be defined as “{NULL}”.</summary>
        public string FalseWireValue { get; set; }

        /// <summary>
        /// Applicable only when xsi:type is Boolean_t.
        /// This attribute is targeted for deprecation.
        /// To achieve the same functionality, it is recommended that a Char_t or String_t type parameter be used instead 
        /// of a Boolean_t. The parameter should have two EnumPairs defined with one defining the false wire-value and the
        /// other defining the true wire-value. The parameter should be bound to a CheckBox control. The CheckBox control
        /// should define the parameters checkedEnumRef and uncheckedEnumRef to refer to the enumIDs of the parameter.
        /// See the section “A Sample FIXatdl Document” in this document for an example. (See the section “A Sample FIXatdl
        /// Document” in this document for an example. Examine the Parameter “AllowDarkPoolExec” and Control “DPOption” 
        /// for details.)
        /// The deprecated use is described as follows:
        /// Defines the value with which to populate the FIX message when the boolean parameter is True. Overrides the 
        /// standard FIX boolean value of “Y”. I.e. if this attribute is not provided then the order-sending application
        /// must use “Y”.
        /// If it is desired that the FIX message is not to be populated with this tag when the value of the parameter 
        /// is true, then trueWireValue should be defined as “{NULL}”.
        /// </summary>
        public string TrueWireValue { get; set; }
    }
}
