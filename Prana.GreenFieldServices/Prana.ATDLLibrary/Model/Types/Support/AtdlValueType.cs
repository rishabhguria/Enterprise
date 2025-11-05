using System;

namespace Prana.ATDLLibrary.Model.Types.Support
{
    /// <summary>
    /// Base class for all value type parameters (Int_t, Float_t, etc.).
    /// </summary>
    /// <remarks>Parameter types must be one of <see cref="AtdlValueType{T}"/> or <see cref="AtdlReferenceType{T}"/>.
    /// The reason for the differentiation is that most FIXatdl types that use value types for the underlying storage
    /// (Int_t, Float_t, UTCTimestamp_t, etc.) actually use <see cref="Nullable{T}"/> so that they can also contain
    /// null, meaning don't include this value in the FIX output.  However, Nullable&lt;T&gt; is a value type, not
    /// a reference type, and so a different base type is required to support underlying reference type usage, such
    /// as in String_t.  (This is the same reason that it isn't possible to factor out apparently duplicated code
    /// across AtdlValueType&lt;T&gt; and AtdlReferenceType&lt;T&gt;, because one uses T? internally and the
    /// other uses T.)</remarks>
    public abstract class AtdlValueType<T> where T : struct
    {
        /// <summary>
        /// Storage for the value of this parameter, as type T?.
        /// </summary>
        protected T? _value;

        /// <summary>
        /// Gets/sets an optional constant value for this parameter.
        /// </summary>
        /// <value>The const value.</value>
        public T? ConstValue { get; set; }
    }
}
