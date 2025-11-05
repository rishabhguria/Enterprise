using System;
using Prana.ATDLLibrary.Resources;
using ThrowHelper = Prana.ATDLLibrary.Diagnostics.ThrowHelper;

namespace Prana.ATDLLibrary.Model.Types.Support
{
    /// <summary>
    /// Represents a FIX/FIXatdl MonthYear value.
    /// </summary>
    public struct MonthYear : IComparable
    {
        private const string ExceptionContext = "MonthYear";

        public MonthYear(ushort year, ushort month, ushort? day, ushort? week)
        {
            Year = year;
            Month = month;
            Day = day;
            Week = week;
        }

        private readonly ushort Year;
        private readonly ushort Month;
        private readonly ushort? Day;
        private readonly ushort? Week;

        /// <summary>
        /// Provides the string representation of this MonthYear instance.
        /// </summary>
        /// <returns>MonthYear as a string.</returns>
        public override string ToString()
        {
            string suffix = Week != null ? string.Format("w{0}", Week) : (Day != null ? string.Format("{0:00}", Day) : string.Empty);

            return string.Format("{0:0000}{1:00}{2}", Year, Month, suffix);
        }

        /// <summary>
        /// Compares two MonthYear values for equality.
        /// </summary>
        /// <param name="lhs">Left hand side value.</param>
        /// <param name="rhs">Right hand side value.</param>
        /// <returns>True if the day, month and year values of the two operands are the same; false otherwise.</returns>
        public static bool operator ==(MonthYear lhs, MonthYear rhs)
        {
            return lhs.Year == rhs.Year && lhs.Month == rhs.Month && lhs.Day == rhs.Day && lhs.Month == rhs.Month;
        }

        /// <summary>
        /// Compares two MonthYear values for inequality.
        /// </summary>
        /// <param name="lhs">Left hand side value.</param>
        /// <param name="rhs">Right hand side value.</param>
        /// <returns>True if any of the day, month and year values of the two operands are not the same; false otherwise.</returns>
        public static bool operator !=(MonthYear lhs, MonthYear rhs)
        {
            return !(lhs == rhs);
        }

        /// <summary>
        /// Compares the supplied object for equality with this MonthYear instance.
        /// </summary>
        /// <param name="obj">Object to compare this instance with.</param>
        /// <returns>True if the supplied object is a MonthYear, and the day, month and year values of the two are the same; false otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return (MonthYear)obj == this;
        }

        /// <summary>
        /// Serves as a hash function for this type.  Overridden because Equals(object) is overridden.
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        /// <remarks>The value 251 is used here because it is a prime number, helpful for generating unique hash values.</remarks>
        public override int GetHashCode()
        {
            unchecked // No issue with int overflow
            {
                int hashCode = ((Year * 251) + Month) * 251;

                hashCode = (Day != null) ? (hashCode + (ushort)Day) * 251 : hashCode * 251;
                hashCode = (Week != null) ? (hashCode + (ushort)Week) * 251 : hashCode * 251;

                return hashCode;
            }
        }

        /// <summary>
        /// Compares one MonthYear value to see whether it is less than or equal to a second MonthYear value.
        /// </summary>
        /// <param name="lhs">Left hand side value.</param>
        /// <param name="rhs">Right hand side value.</param>
        /// <returns>True if the left hand operand occurs before or at the same time as the right hand operand; false otherwise.</returns>
        public static bool operator <=(MonthYear lhs, MonthYear rhs)
        {
            if (lhs.Year > rhs.Year)
                return false;

            if (lhs.Year < rhs.Year)
                return true;

            // Years equal...
            if (lhs.Month > rhs.Month)
                return false;

            if (lhs.Month < rhs.Month)
                return true;

            // Years and months equal...
            if (lhs.Day == null && rhs.Day == null && lhs.Week == null && lhs.Week == null)
                return true;

            if (lhs.Day != null && rhs.Day != null)
                return lhs.Day <= rhs.Day;

            if (lhs.Week != null && lhs.Week != null)
                return lhs.Week <= rhs.Week;

            throw ThrowHelper.New<NotSupportedException>(ExceptionContext, ErrorMessages.UnsupportedComparisonOperation, lhs, rhs);
        }

        /// <summary>
        /// Compares one MonthYear value to see whether it is greater than or equal to a second MonthYear value.
        /// </summary>
        /// <param name="lhs">Left hand side value.</param>
        /// <param name="rhs">Right hand side value.</param>
        /// <returns>True if the left hand operand occurs at the same time or after the right hand operand; false otherwise.</returns>
        public static bool operator >=(MonthYear lhs, MonthYear rhs)
        {
            if (lhs.Year < rhs.Year)
                return false;

            if (lhs.Year > rhs.Year)
                return true;

            // Years equal...
            if (lhs.Month < rhs.Month)
                return false;

            if (lhs.Month > rhs.Month)
                return true;

            // Years and months equal...
            if (lhs.Day == null && rhs.Day == null && lhs.Week == null && lhs.Week == null)
                return true;

            if (lhs.Day != null && rhs.Day != null)
                return lhs.Day >= rhs.Day;

            if (lhs.Week != null && lhs.Week != null)
                return lhs.Week >= rhs.Week;

            throw ThrowHelper.New<NotSupportedException>(ExceptionContext, ErrorMessages.UnsupportedComparisonOperation, lhs, rhs);
        }

        #region IComparable Members

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates 
        /// whether the current instance precedes, follows, or occurs in the same position in the sort order as the 
        /// other object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings:
        /// <list type="bullet">
        /// <item><description>Less than zero - this instance precedes obj in the sort order.</description></item></list>
        /// <item><description></description>Zero - this instance occurs in the same position in the sort order as obj.</item></list>
        /// <item><description>Greater than zero - this instance follows obj in the sort order.</description></item></list>
        /// </returns>
        public int CompareTo(object obj)
        {
            // Null references are by definition less than the current instance.
            if (obj == null)
                return 1;

            if (!(obj is MonthYear))
                throw ThrowHelper.New<ArgumentException>(this, InternalErrors.UnexpectedArgumentType, obj.GetType().FullName, this.GetType().FullName);

            MonthYear rhs = (MonthYear)obj;

            if (rhs == this)
                return 0;

            return rhs <= this ? 1 : -1;
        }

        #endregion
    }
}