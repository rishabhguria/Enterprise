using Prana.ATDLLibrary.Model.Collections;
using Prana.ATDLLibrary.Model.Enumerations;

namespace Prana.ATDLLibrary.Model.Elements.Support
{
    /// <summary>
    /// The type parameter specifies whether the Edit relates to a StateRule or to a StrategyEdit.
    /// </summary>
    /// <typeparam name="T">One of <see cref="Control_t"/> or <see cref="Parameter_t"/>.</typeparam>
    public interface IEdit<T>
    {
        /// <summary>
        /// Gets/sets the name of field to be used as left hand side of the evaluation.
        /// </summary>
        string Field { get; set; }

        /// <summary>
        /// Gets/sets the name of second (optional) field, to be used as the right hand side of the evaluation.
        /// </summary>
        string Field2 { get; set; }

        /// <summary>
        /// Gets/sets the optional ID for this Edit.
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// Gets/sets the optional operator - used when comparing two values.
        /// </summary>
        Operator_t? Operator { get; set; }

        /// <summary>
        /// Gets/sets the optional logical operator - used when combining two or more Edits.
        /// </summary>
        LogicOperator_t? LogicOperator { get; set; }

        /// <summary>
        /// Gets/sets the optional fixed value to be used as the right hand side of the evaluation.
        /// </summary>
        string Value { get; set; }

        /// <summary>
        /// Gets the collection of child Edits.  May be empty, unless LogicOperator is non-null.
        /// </summary>
        EditEvaluatingCollection<T> Edits { get; }
    }
}
