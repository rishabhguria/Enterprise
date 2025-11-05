using Prana.ATDLLibrary.Model.Collections;
using Prana.ATDLLibrary.Model.Elements.Support;
using Prana.ATDLLibrary.Model.Enumerations;

namespace Prana.ATDLLibrary.Model.Elements
{
    /// <summary>
    /// Represents the FIXatdl type Edit_t when it occurs outside of a StateRule_t or a StrategyEdit_t element.
    /// </summary>
    public class Edit_t
    {
        /// <summary>
        /// Gets/sets the first field name for comparison. When the edit is used within a StateRule, this field 
        /// must refer to the ID of a Control. When the edit is used within a StrategyEdit, this field must refer 
        /// to either the name of a parameter or a standard FIX field name. When referring to a standard FIX tag
        /// then the name must be pre-pended with the string "FIX_", e.g. "FIX_OrderQty". Required the Operator is 
        /// not null.
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// Gets/sets the optional second field name for comparison. When the edit is used within a StateRule, this field 
        /// must refer to the ID of a Control. When the edit is used within a StrategyEdit, this field must refer 
        /// to either the name of a parameter or a standard FIX field name. When referring to a standard FIX tag
        /// then the name must be pre-pended with the string "FIX_", e.g. "FIX_OrderQty".
        /// </summary>
        public string Field2 { get; set; }

        public string Id { get; set; }

        public Operator_t? Operator { get; set; }

        public LogicOperator_t? LogicOperator { get; set; }

        public string Value { get; set; }

        public EditCollection Edits { get; private set; }

        public Edit_t()
        {
            Edits = new EditCollection();
        }
    }

    /// <summary>
    /// Represents a FIXatdl Edit_t when implemented within a StateRule_t or StrategyEdit_t element.
    /// </summary>
    public class Edit_t<T> : IEdit<T>
    {
        private readonly EditEvaluatingCollection<T> _edits;
        private readonly EditRefCollection<T> _editRefs;

        /// <summary>
        /// Initializes a new <see cref="Edit{T}"/> instance.
        /// </summary>
        public Edit_t()
        {
            _edits = new EditEvaluatingCollection<T>();
            _editRefs = new EditRefCollection<T>();
        }  

        /// <summary>
        /// Gets the collection of EditRefs for this Edit.
        /// </summary>
        public EditRefCollection<T> EditRefs { get { return _editRefs; } }

        #region IEdit_t Members

        /// <summary>
        /// Gets/sets the name of field to be used as left hand side of the evaluation.
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// Gets/sets the name of second (optional) field, to be used as the right hand side of the evaluation.
        /// </summary>
        public string Field2 { get; set; }

        /// <summary>
        /// Gets/sets the optional ID for this Edit.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets/sets the optional operator - used when comparing two values.
        /// </summary>
        public Operator_t? Operator { get; set; }

        /// <summary>
        /// Gets/sets the optional fixed value to be used as the right hand side of the evaluation.
        /// </summary>
        /// <remarks>From the spec:<br/><br/>"When Edit is a descendant of a StateRule element, Value refers to the 
        /// value of the control referred by Field. If the control referred by Field has enumerated values then Value 
        /// refers to the enumID of one of the control's ListItem elements.<br/>
        /// When Edit is a descendant of a StrategyEdit element, Value refers to the wireValue of the parameter 
        /// referred by Field."</remarks>
        public string Value { get; set; }

        /// <summary>
        /// Gets the collection of child Edits.  May be empty, unless LogicOperator is non-null.
        /// </summary>
        public EditEvaluatingCollection<T> Edits { get { return _edits; } }

        /// <summary>
        /// Gets/sets the optional logical operator - used when combining two or more Edits.
        /// </summary>
        public LogicOperator_t? LogicOperator
        {
            get { return Edits.LogicOperator; }
            set { Edits.LogicOperator = value; }
        }
    
        #endregion IEdit_t Members       
    }
}
