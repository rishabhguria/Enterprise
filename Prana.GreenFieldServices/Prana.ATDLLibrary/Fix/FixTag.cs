using Prana.ATDLLibrary.Diagnostics;
using Prana.ATDLLibrary.Resources;
using System;

namespace Prana.ATDLLibrary.Fix
{
    /// <summary>
    /// Represents a FIX tag, a non-zero positive integer.
    /// </summary>
    [Serializable]
    public struct FixTag
    {
        public readonly int _value;

        /// <summary>
        /// Initializes a new instance of FixTag.
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if a value less than or equal to zero is supplied.</exception>
        public FixTag(int value)
        {
            if (value <= 0)
                throw ThrowHelper.New<ArgumentOutOfRangeException>(typeof(FixTag).FullName, ErrorMessages.NonZeroPositiveIntRequired, value);

            _value = value;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return _value.ToString();
        }
    }
}
